using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using GameService.Data;
using GameService.Library;
using GameService.Library.Utils;

namespace GameService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class GameService : IGameService
    {
        private const int PlayerNameMinLength = 5;
        private const int PlayerPasswordMinLength = 5;

        // holds players available for game
        private static readonly PlayerStore PlayersOnline = new PlayerStore();    
        private static readonly SynchronizedCollection<GameState> Games = new SynchronizedCollection<GameState>();

        private IGameClient Client => OperationContext.Current.GetCallbackChannel<IGameClient>();

        private GameState GetGame(Player player)
        {
            try
            {
                return Games.Single(game => game.FirstPlayer.Equals(player) || game.SecondPlayer.Equals(player));
            }
            catch
            {
                return null;
            }
        }

        public Player RegisterPlayer(string name, string password)
        {
            // check if name and password are valid
            if (name.Length < PlayerNameMinLength || password.Length < PlayerPasswordMinLength)
            {
                return null;
            }

            using (var ctx = new GameContext())
            {
                // check if player's name is unique
                var count = ctx.Players.Count(p => p.Name.Equals(name));
                if (count != 0)
                {
                    return null;
                }

                var newPlayer = ctx.Players.Add(new Player(name, password));
                ctx.SaveChanges();

                return newPlayer;
            }
        }

        public Player ConnectPlayer(string name, string password)
        {
            var ctx = new GameContext();

            try
            {
                var player = ctx.Players.Single(p => p.Name.Equals(name) && p.Password.Equals(password));
                PlayersOnline.AddPlayer(Client, player);

                // notify other players
                PlayersOnline.GetActivePlayers().ForEach(p =>
                {
                    if (p.Id != player.Id)
                    {
                        PlayersOnline.GetGameClient(p)?.NotifyPlayerConnected(player);
                    }
                });

                return player;
            }
            catch
            {
                return null;
            }
            finally
            {
                ctx.Dispose();
            }      
        }

        public List<Player> GetAvailablePlayers()
        {
            return PlayersOnline.GetActivePlayers().Where(p => p.InGame == false).ToList();
        }

        public void InvitePlayer(Player player, GameParams.GameSizes gameSize)
        {
            var invitingPlayer = PlayersOnline.GetGamePlayer(Client);
            var client = PlayersOnline.GetGameClient(player);
            client?.InvitedBy(invitingPlayer, gameSize);
        }

        public void AcceptInvitation(Player invitingPlayer, GameParams.GameSizes gameSize)
        {
            var invitingClient = PlayersOnline.GetGameClient(invitingPlayer);
            var acceptingPlayer = PlayersOnline.GetGamePlayer(Client);

            // update players state - remove these in from list in PlayersView
            invitingPlayer = PlayersOnline.GetGamePlayer(invitingClient);
            invitingPlayer.InGame = true;
            acceptingPlayer.InGame = true;

            // notify clients
            NotifyPlayerUpdate(invitingPlayer);
            NotifyPlayerUpdate(acceptingPlayer);

            var gameParams = new GameParams(invitingPlayer, acceptingPlayer, gameSize);
            var newGame = new GameState(gameParams.FirstPlayer, gameParams.SecondPlayer, gameSize);

            Games.Add(newGame);

            // callback
            invitingClient.StartGame(gameParams);
            Client.StartGame(gameParams);
        }

        public void RefuseInvitation(Player player)
        {
            var refusingPlayer = PlayersOnline.GetGamePlayer(Client);
            var client = PlayersOnline.GetGameClient(player);
            client?.InvitationRefused(refusingPlayer);
        }

        public void DisconnectPlayer(Player player)
        {
            try
            {
                // notify players
                PlayersOnline.GetActivePlayers().ForEach(p =>
                {
                    if (p.Id != player.Id)
                    {
                        PlayersOnline.GetGameClient(p).NotifyPlayerDisconnected(player);
                    }
                });

                var game = GetGame(player);
                if (game != null)
                {
                    var otherPlayer = game.FirstPlayer.Equals(player) ? game.SecondPlayer : game.FirstPlayer;
                    var otherClient = PlayersOnline.GetGameClient(otherPlayer);
                    otherClient?.OpponentLeft();
                    Games.Remove(game);
                }

                PlayersOnline.RemovePlayer(player);
            }
            catch
            {
                // ignored
            }
        }

        public void RevealCardRequest(Player player, int cardIndex)
        {
            var game = GetGame(player);

            if (game == null) return;

            if (game.RevealedCardIndex != null)
            {
                var previouslyRevealed = game.RevealedCardIndex.Value;
                game.RevealedCardIndex = null;

                if (game.Map[cardIndex] == game.Map[previouslyRevealed] && cardIndex != previouslyRevealed)
                {               
                    game.IncrementScore(player);
                    var firstClient = PlayersOnline.GetGameClient(game.FirstPlayer);
                    var secondClient = PlayersOnline.GetGameClient(game.SecondPlayer);
                    firstClient.CardPairFound(cardIndex, previouslyRevealed, game.Map[previouslyRevealed]);
                    secondClient.CardPairFound(cardIndex, previouslyRevealed, game.Map[previouslyRevealed]);

                    if (!game.IsGameEnd()) return;
                    GameFinished(game, firstClient, secondClient);
                }
                else
                {
                    PlayersOnline.GetGameClient(game.FirstPlayer).SwapPlayerTurn(cardIndex, previouslyRevealed, game.Map[cardIndex], game.Map[previouslyRevealed]);
                    PlayersOnline.GetGameClient(game.SecondPlayer).SwapPlayerTurn(cardIndex, previouslyRevealed, game.Map[cardIndex], game.Map[previouslyRevealed]);
                }
            }
            else
            {
                game.RevealedCardIndex = cardIndex;
                PlayersOnline.GetGameClient(game.FirstPlayer).RevealCard(cardIndex, game.Map[cardIndex]);
                PlayersOnline.GetGameClient(game.SecondPlayer).RevealCard(cardIndex, game.Map[cardIndex]);
            }
        }

        public void SendMessage(string message)
        {
            var sendingPlayer = PlayersOnline.GetGamePlayer(Client);
            var game = GetGame(sendingPlayer);
            var receivingPlayer = game.FirstPlayer.Equals(sendingPlayer) ? game.SecondPlayer : game.FirstPlayer;
            PlayersOnline.GetGameClient(receivingPlayer).IncomingMessage(sendingPlayer, message);
            PlayersOnline.GetGameClient(sendingPlayer).IncomingMessage(sendingPlayer, message);
        }

        public void GameTimeout()
        {
            var player = PlayersOnline.GetGamePlayer(Client);

            if (player == null) return;
            var game = GetGame(player);

            if (game == null) return;
            var client1 = PlayersOnline.GetGameClient(game.FirstPlayer);
            var client2 = PlayersOnline.GetGameClient(game.SecondPlayer);
            Games.Remove(game);                
            client1.GameTimeout();
            client2.GameTimeout();
        }

        private void GameFinished(GameState game, IGameClient firstClient, IGameClient secondClient)
        {
            if (game.FirstPlayerScore == game.SecondPlayerScore)
            {
                firstClient.GameFinished(GameResult.FiftyFifty);
                secondClient.GameFinished(GameResult.FiftyFifty);
            }
            else if (game.FirstPlayerScore > game.SecondPlayerScore)
            {
                firstClient.GameFinished(GameResult.Win);
                secondClient.GameFinished(GameResult.Lose);
            }
            else
            {
                firstClient.GameFinished(GameResult.Lose);
                secondClient.GameFinished(GameResult.Win);
            }

            game.GameEnd = DateTime.Now;

            using (var ctx = new GameContext())
            {
                ctx.Games.Add(new GameStatistic
                {
                    FirstPlayer = game.FirstPlayer.Name,
                    SecondPlayer = game.SecondPlayer.Name,
                    FirstPlayerScore = game.FirstPlayerScore,
                    SecondPlayerScore = game.SecondPlayerScore,
                    GameDuration = (int) (game.GameEnd - game.GameBegin).TotalSeconds,
                    GameSize = Utils.GetGameTypeName(game.GameSize)
                });
                ctx.SaveChanges();
            }

            Games.Remove(game);
        }

        private void NotifyPlayerUpdate(Player updatedPlayer)
        {
            PlayersOnline.GetActivePlayers().ForEach(player =>
            {
                PlayersOnline.GetGameClient(player).NotifyPlayerUpdate(updatedPlayer);
            });
        }

        public List<GameStatistic> GetGamesStatistics()
        {
            using (var ctx = new GameContext())
            {
                return ctx.Games.ToList();
            }
        }
    }
}

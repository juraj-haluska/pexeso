using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using GameService.Data;
using GameService.Library;

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

        private GameState GetGameHistory(Player player)
        {
            return Games.Single(game => game.FirstPlayer.Equals(player) || game.SecondPlayer.Equals(player));
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
                        PlayersOnline.GetGameClient(p).NotifyPlayerConnected(player);
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

            // update players state - hide these in UI
            PlayersOnline.GetGamePlayer(invitingClient).InGame = true;
            acceptingPlayer.InGame = true;

            NotifyPlayerUpdate(invitingPlayer);
            NotifyPlayerUpdate(acceptingPlayer);

            var gameParams = new GameParams(invitingPlayer, acceptingPlayer, gameSize);
            Utils.GetGameSize(gameSize, out var rows, out var cols);

            // generate random map
            var rand = new Random();
            var elements = Enumerable.Range(1, rows * cols / 2).ToList();
            var map = elements.Concat(elements).OrderBy(x => rand.Next()).ToArray();

            var gameHistory = new GameState
            {
                FirstPlayer = gameParams.FirstPlayer,
                SecondPlayer = gameParams.SecondPlayer,
                Map = map
            };

            Games.Add(gameHistory);

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
            // try is here because we don't know in which store the player is
            // player doesn't have to be in either
            // TODO: do it the better way
            try
            {
                // notify other players
                PlayersOnline.GetActivePlayers().ForEach(p =>
                {
                    if (p.Id != player.Id)
                    {
                        PlayersOnline.GetGameClient(p).NotifyPlayerDisconnected(player);
                    }
                });

                PlayersOnline.RemovePlayer(player);
            }
            catch
            {
                // ignored
            }
        }

        private void NotifyPlayerUpdate(Player updatedPlayer)
        {
            PlayersOnline.GetActivePlayers().ForEach(player =>
            {
                PlayersOnline.GetGameClient(player).NotifyPlayerUpdate(updatedPlayer);
            });
        }

        public void RevealCardRequest(Player player, int cardIndex)
        {
            var game = GetGameHistory(player);

            if (game.RevealedCardIndex != null)
            {
                var previouslyRevealed = game.RevealedCardIndex.Value;
                game.RevealedCardIndex = null;

                if (game.Map[cardIndex] == game.Map[previouslyRevealed] && cardIndex != previouslyRevealed)
                {               
                    game.IncrementScore(player);
                    PlayersOnline.GetGameClient(game.FirstPlayer).CardPairFound(cardIndex, previouslyRevealed, game.Map[previouslyRevealed]);
                    PlayersOnline.GetGameClient(game.SecondPlayer).CardPairFound(cardIndex, previouslyRevealed, game.Map[previouslyRevealed]);

                    if (!game.IsGameEnd()) return;
                    Console.WriteLine("game end");
                    Games.Remove(game);
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
    }
}

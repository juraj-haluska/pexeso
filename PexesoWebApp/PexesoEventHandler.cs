using System;
using GameService.Library;
using GameService.Library.Models;

namespace PexesoWebApp
{
    public class PexesoEventHandler : IGameClient
    {
        public void InvitedBy(Player player, GameParams.GameSizes gameSize)
        {
            throw new NotImplementedException();
        }

        public void StartGame(GameParams gameParams)
        {
            throw new NotImplementedException();
        }

        public void InvitationRefused(Player player)
        {
            throw new NotImplementedException();
        }

        public void NotifyPlayerConnected(Player player)
        {
            throw new NotImplementedException();
        }

        public void NotifyPlayerDisconnected(Player player)
        {
            throw new NotImplementedException();
        }

        public void NotifyPlayerUpdate(Player player)
        {
            throw new NotImplementedException();
        }

        public void RevealCard(int cardIndex, int cardValue)
        {
            throw new NotImplementedException();
        }

        public void SwapPlayerTurn(int card1Index, int card2Index, int card1Value, int card2Value)
        {
            throw new NotImplementedException();
        }

        public void CardPairFound(int card1Index, int card2Index, int card2Value)
        {
            throw new NotImplementedException();
        }

        public void IncomingMessage(Player @from, string message)
        {
            throw new NotImplementedException();
        }

        public void GameTimeout()
        {
            throw new NotImplementedException();
        }

        public void GameFinished(GameResult result)
        {
            throw new NotImplementedException();
        }

        public void OpponentLeft()
        {
            throw new NotImplementedException();
        }
    }
}
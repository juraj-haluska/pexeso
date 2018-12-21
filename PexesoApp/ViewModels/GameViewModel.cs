using System;
using Caliburn.Micro;
using GameService.Library;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PexesoApp.Properties;
using PexesoApp.Views;

namespace PexesoApp.ViewModels
{
    public class GameViewModel : Screen
    {
        private readonly GameParams _gameParams;
        private readonly Player _me;

        public bool StartingPlayer { get; set; }

        public GameViewModel(GameParams gameParams, Player me)
        {
            _gameParams = gameParams;
            _me = me;

            StartingPlayer = gameParams.FirstPlayer.Equals(me);
        }

        protected override void OnViewReady(object view)
        {
            Utils.Utils.GetGameSize(_gameParams.GameSize, out var rows, out var cols);

            var grid = (Grid) ((GameView) GetView()).Content;

            // generate columns
            Enumerable.Range(0, cols).ToList().ForEach(i =>
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            });

            // generate rows
            Enumerable.Range(0, rows).ToList().ForEach(i =>
            {
                grid.RowDefinitions.Add(new RowDefinition());
            });

            // populate grid
            foreach (var i in Enumerable.Range(0, rows * cols))
            {
                var button = new Button {Width = 50, Height = 50};
                var row = i / cols;
                var col = i % cols;
                button.Margin = new Thickness(2.5);
                button.Content = "?";
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                grid.Children.Add(button);    
            }
        }
    }
}

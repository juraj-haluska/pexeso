using System;
using System.Collections.Generic;
using Caliburn.Micro;
using GameService.Library;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PexesoApp.Views;
using System.Timers;
using System.Windows.Threading;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class GameViewModel : Screen
    {
        private readonly GameParams _gameParams;
        private readonly Player _me;
        private readonly IGameService _gameService;
        private readonly GameEventHandler _eventHandler;
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<RoutedEventHandler> _handlers = new List<RoutedEventHandler>();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private string _message;

        public Action Exit { get; set; }

        public Action GameResult { get; set; }

        public bool MyTurn { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public BindableCollection<string> Messages { get; set; } = new BindableCollection<string>();

        public GameViewModel(GameParams gameParams, Player me, IGameService gameService, GameEventHandler eventHandler)
        {
            _gameParams = gameParams;
            _me = me;
            _gameService = gameService;
            _eventHandler = eventHandler;

            ResetTimer();

            MyTurn = gameParams.FirstPlayer.Equals(me);
            RegisterEventHandlers();
        }

        public void SendMessage()
        {
            if (!string.IsNullOrEmpty(Message))
            _gameService.SendMessage(Message);
            Message = "";
        }

        protected override void OnViewReady(object view)
        {
            Utils.Utils.GetGameSize(_gameParams.GameSize, out var rows, out var cols);

            var stackPanel = (StackPanel)((GameView)GetView()).Content;
            var grid = (Grid) stackPanel.Children[0];

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
                button.Content = new Label {FontSize = 20, Content = "?"};
                _handlers.Add((o, e) => _gameService.RevealCardRequest(_me, i));               
                _buttons.Add(button);

                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);

                grid.Children.Add(button);    
            }

            if (MyTurn)
            {
                EnableButtons();
            }
        }

        private void RegisterEventHandlers()
        {
            if (_eventHandler == null) return;

            _eventHandler.RevealCardEvent += (index, value) =>
            {
                ((Label)_buttons[index].Content).Content = value.ToString();
                ResetTimer();
            };
            _eventHandler.CardPairFoundEvent += (card1Index, card2Index, card2Value) =>
            {
                ((Label)_buttons[card1Index].Content).Content = card2Value.ToString();
                _buttons[card1Index].IsEnabled = false;
                _buttons[card2Index].IsEnabled = false;
            };
            _eventHandler.SwapPlayerTurnEvent += async (card1Index, card2Index, value, card2Value) =>
            {
                ((Label)_buttons[card1Index].Content).Content = value.ToString();
                ((Label)_buttons[card2Index].Content).Content = card2Value.ToString();
                DisableButtons();

                await Task.Delay(1000);

                ((Label) _buttons[card1Index].Content).Content = "?";
                ((Label)_buttons[card2Index].Content).Content = "?";

                if (MyTurn)
                {
                    MyTurn = false;
                }
                else
                {
                    MyTurn = true;
                    EnableButtons();
                }
            };
            _eventHandler.IncomingMessageEvent += (player, message) => Messages.Add($"{player.Name}: {message}");
            _eventHandler.GameTimeoutEvent += () =>
            {
                _gameService.DisconnectPlayer(_me);
                MessageBox.Show("Game has ended (timeout).", "Game ended", MessageBoxButton.OK, MessageBoxImage.Warning);
                Exit();
            };

            _timer.Tick += (sender, args) =>
            {
                _gameService.GameTimeout();
            };

            _timer.Start();
        }

        private void DisableButtons()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Click -= _handlers[i];
            }
        }

        private void EnableButtons()
        {
            for (var i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Click += _handlers[i];
            }
        }

        private void ResetTimer()
        {
            _timer.Interval = TimeSpan.FromSeconds(10);
        }
    }
}

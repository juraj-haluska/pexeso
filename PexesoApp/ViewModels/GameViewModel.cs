using System;
using System.Collections.Generic;
using Caliburn.Micro;
using GameService.Library;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PexesoApp.Views;
using System.Windows.Threading;
using GameService.Library.Models;
using GameService.Library.Utils;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class GameViewModel : Screen
    {
        private const int Timeout = 60;
        private const int CardSize = 50;

        private readonly GameParams _gameParams;
        private readonly Player _me;
        private readonly IGameService _gameService;
        private readonly GameEventHandler _eventHandler;
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<RoutedEventHandler> _handlers = new List<RoutedEventHandler>();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private string _message;
        private bool _myTurn;

        public Action Exit { get; set; }

        public Action GameFinished { get; set; }

        public bool MyTurn
        {
            get => _myTurn;
            set
            {
                _myTurn = value;
                TurnText = value ? "Your turn" : "Wait for opponent";
                NotifyOfPropertyChange(() => TurnText);
            }
        }

        public string TurnText { get; set; }

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

            MyTurn = gameParams.FirstPlayer.Equals(me);
            RegisterEventHandlers();

            ResetTimer();
            _timer.Start();
        }

        public void SendMessage()
        {
            if (!string.IsNullOrEmpty(Message))
            _gameService.SendMessage(Message);
            Message = "";
        }

        protected override void OnViewReady(object view)
        {
            Utils.GetGameSize(_gameParams.GameSize, out var rows, out var cols);

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
                var button = new Button {Width = CardSize, Height = CardSize };

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
                MessageBox.Show("Game has ended (timeout).", "Game ended", MessageBoxButton.OK, MessageBoxImage.Warning);
                Exit();
                _gameService.DisconnectPlayer(_me);
            };

            _eventHandler.GameFinishedEvent += result =>
            {               
                var message = Utils.GetGameResult(result);
                MessageBox.Show($"The game has ended. You are {message}.", "Game ended", MessageBoxButton.OK, MessageBoxImage.Information);
                GameFinished();
                _gameService.DisconnectPlayer(_me);
            };

            _eventHandler.OpponentLeftEvent += () =>
            {
                _gameService.DisconnectPlayer(_me);
                MessageBox.Show("Your opponent has left the game.", "Game ended", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Exit();
            };

            _timer.Tick += (sender, args) =>
            {
                _gameService.GameTimeout();
            };
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
            _timer.Interval = TimeSpan.FromSeconds(Timeout);
        }

        protected override void OnViewAttached(object view, object context)
        {
            var window = Window.GetWindow((DependencyObject) GetView());
            if (window != null)
            {
                window.Closed += (sender, args) => _gameService.DisconnectPlayer(_me);
            }
        }
    }
}

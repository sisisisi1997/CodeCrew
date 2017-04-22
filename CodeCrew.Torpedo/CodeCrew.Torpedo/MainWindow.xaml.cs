using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CodeCrew.Torpedo.GameLogic;

namespace CodeCrew.Torpedo
{
	public partial class MainWindow : Window
	{
		private GameLogic.GameState _currentGame;
		private int _remainingShips = 0;
		private GameLogic.Direction _shipDirection = GameLogic.Direction.Right;
		private bool started = false;

		public MainWindow()
		{
			InitializeComponent();
			this.MouseWheel += (sender, e) =>
			{
				_shipDirection = _shipDirection == GameLogic.Direction.Right ? GameLogic.Direction.Down : GameLogic.Direction.Right;
			};
		}

		private void ClearBoard()
		{
			this.GameTable.ColumnDefinitions.Clear();
			this.GameTable.RowDefinitions.Clear();
			this.GameTable.Children.Clear(); // GC
			this.GameTable.Background = Brushes.Transparent;

			this.EnemyTable.ColumnDefinitions.Clear();
			this.EnemyTable.RowDefinitions.Clear();
			this.EnemyTable.Children.Clear();
			this.EnemyTable.Background = Brushes.Transparent;
		}
		
		private void StartGame(int tableSize)
		{
			_currentGame = new GameLogic.GameState(tableSize);
			
			ClearBoard();
			
			for(int i = 0; i < tableSize; ++ i)
			{
				this.GameTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
				this.GameTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

				this.EnemyTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
				this.EnemyTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			for(int col = 0; col < tableSize; ++ col)
			{
				for(int row = 0; row < tableSize; ++ row)
				{
					Border b = new Border() { BorderThickness = new Thickness(1), Child = new Rectangle { Fill = Brushes.Aqua } };
					b.MouseEnter += (sender, e) =>
						{
							if(!_currentGame.IsFilled(Grid.GetColumn(b), Grid.GetRow(b)))
								(b.Child as Rectangle).Fill = Brushes.Orange;
						};
					b.MouseLeave += (sender, e) =>
						{
							if(!_currentGame.IsFilled(Grid.GetColumn(b), Grid.GetRow(b)))
								(b.Child as Rectangle).Fill = Brushes.Aqua;
						};
					b.MouseDown += (sender, e) =>
					{
						if(started)
						{
							
						}
						else
						{
							int shipSize = 0;
							switch(_remainingShips)
							{
								case 5:
									shipSize = 4;
									break;
								case 4: goto case 3;
								case 3:
									shipSize = 3;
									break;
								case 2:
									shipSize = 2;
									break;
								case 1:
									shipSize = 1;
									started = true;
									break;
							}

							if(_currentGame.IsShipValid(new GameLogic.Ship(Grid.GetColumn(b), Grid.GetRow(b), shipSize, _shipDirection)))
							{
								_currentGame.AddShip(new GameLogic.Ship(Grid.GetColumn(b), Grid.GetRow(b), shipSize, _shipDirection));
								_remainingShips --;
								
								if(_shipDirection == Direction.Down)
								{
									for(int i = Grid.GetRow(b); i < Grid.GetRow(b) + shipSize; ++ i)
									{
										
									}
								}
								else
								{
									for(int i = Grid.GetColumn(b); i < Grid.GetColumn(b) + shipSize; ++ i)
									{
										
									}
								}

							}
						}
					};
					Grid.SetColumn(b, col);
					Grid.SetRow(b, row);
					this.GameTable.Children.Add(b);

					Border b2 = new Border() { BorderThickness = new Thickness(1), Child = new Rectangle { Fill = Brushes.DarkBlue } };
					b2.MouseEnter += (sender, e) =>
						{
							if(!_currentGame.IsFilled(Grid.GetColumn(b2), Grid.GetRow(b2)))
								(b2.Child as Rectangle).Fill = Brushes.Orange;
						};
					b2.MouseLeave += (sender, e) =>
						{
							if(!_currentGame.IsFilled(Grid.GetColumn(b2), Grid.GetRow(b2)))
								(b2.Child as Rectangle).Fill = Brushes.DarkBlue;
						};
					Grid.SetColumn(b2, col);
					Grid.SetRow(b2, row);
					this.EnemyTable.Children.Add(b2);
				}

				_remainingShips = 5;
			}

			this.SetupDataInput.Visibility = Visibility.Collapsed;
			this.GameTable.Visibility = Visibility.Visible;
			this.EnemyTable.Visibility = Visibility.Visible;
		}

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			this.StartGame(8);
		}

		private void ProceedButton_Click(object sender, RoutedEventArgs e)
		{
			StartGame(int.Parse(this.TableSizeBox.Text));
		}
	}
}
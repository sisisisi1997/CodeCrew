using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace CodeCrew.Torpedo.GameLogic
{
	class GameState
	{
		private PointState[,] _table;
		private int _tableSize;

		public int TableSize { get { return _tableSize; } }
		
		public GameState(int size)
		{
			_table = new PointState[size, size];
			for(int i = 0; i < size; ++ i)
			{
				for(int j = 0; j < size; ++ j)
				{
					_table[i, j] = PointState.Blank;
				}
			}
			_tableSize = size;
		}

		public bool IsFilled(int x, int y)
			=> _table[x, y] == PointState.Ship || _table[x, y] == PointState.ShipHit;
		
		public bool BeenHit(int x, int y)
			=> _table[x, y] == PointState.BlankHit || _table[x, y] == PointState.ShipHit;

		public bool IsShipValid(Ship ship)
		{
			if(ship.Direction == Direction.Down)
			{
				if(ship.StartY + ship.Length >= _tableSize)
					return false;
				
				for(int i = ship.StartY; i < ship.StartY + ship.Length; ++ i)
				{
					if(IsFilled(ship.StartX, i))
						return false;
				}
			}
			else
			{
				if(ship.StartX + ship.Length >= _tableSize)
					return false;
				
				for(int i = ship.StartX; i < ship.StartX + ship.Length; ++ i)
				{
					if(IsFilled(i, ship.StartY))
						return false;
				}
			}

			return true;
		}

		public void AddShip(Ship ship)
		{
			if(!IsShipValid(ship))
				throw new ArgumentException("Érvénytelen elhelyezés!", nameof(ship));
			
			if(ship.Direction == Direction.Down)
			{
				for(int i = ship.StartY; i < ship.StartY + ship.Length; ++ i)
				{
					_table[ship.StartX, i] = PointState.Ship;
				}
			}
			else
			{
				for(int i = ship.StartX; i < ship.StartX + ship.Length; ++ i)
				{
					_table[i, ship.StartY] = PointState.Ship;
				}
			}
		}	

		public bool Hit(int x, int y)
		{
			if(BeenHit(x, y))
				throw new ArgumentException();
			bool tmp = IsFilled(x, y);
			_table[x, y] = _table[x, y] == PointState.Blank ? PointState.BlankHit : PointState.ShipHit;
			return tmp;
		}
	}

	struct Ship
	{
		public int			StartX		{ get; }
		public int			StartY		{ get; }
		public int			Length		{ get; }
		public Direction	Direction	{ get; }

		public Ship(int x, int y, int length, Direction direction)
		{
			this.StartX		= x;
			this.StartY		= y;
			this.Length		= length;
			this.Direction	= direction;
		}
	}

	enum Direction
	{
		Right,
		Down
	}

	enum PointState
	{
		Blank,
		Ship,
		BlankHit,
		ShipHit
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class Game
{
	[SerializeField]
	private GameBoard _gameBoard;

	public Game(IGameParameters parameters)
	{
		this._gameBoard = new GameBoard(parameters);
	}

	public Game(Game game)
	{
		this._gameBoard = new GameBoard(game._gameBoard);
	}

	public void Fill(CellStates cell)
	{
		for (int i = 0; i < _gameBoard.Count; i++)
		{
			_gameBoard[i] = cell;
		}
	}
	public void FillRandom()
	{
		for (int i = 0; i < _gameBoard.Count; i++)
		{
			_gameBoard[i] = Helpers.GetRandomEnum<CellStates>();
		}
	}

	public GameBoard GetGameBoard()
	{
		return _gameBoard;
	}
	public GameBoard GetCopyGameBoard()
	{
		return new GameBoard(_gameBoard);
	}
	
	public int Width => _gameBoard.Width;
	public int Height => _gameBoard.Height;
	public int Count => _gameBoard.Count;

	public void Update(float dt)
	{
		_gameBoard.Update(dt);
	}

	public bool PositionHasExploded(float positionX, float positionZ)
	{
		int x = Round(positionX);
		int z = Round(positionZ);
		return PositionHasExploded(x, z);
	}

	public bool PositionHasExploded(Vector2 position)
	{
		return PositionHasExploded(Round(position));
	}

	public bool PositionHasExploded(Vector2Int position)
	{
		return _gameBoard.PositionHasExploded(position.x, position.y);
	}

	public bool PositionHasExploded(int positionX, int positionZ)
	{
		return _gameBoard.PositionHasExploded(positionX, positionZ);
	}

	public Vector2Int[] GetPossiblePositions(Vector2 position)
	{
		List<Vector2Int> possibleMove = new List<Vector2Int>(5);
		int x = Round(position.x);
		int y = Round(position.y);
		Vector2Int Center = new Vector2Int(x, y);
		Vector2Int Up = Center + Vector2Int.up;
		Vector2Int Right = Center + Vector2Int.right;
		Vector2Int Down = Center + Vector2Int.down;
		Vector2Int Left = Center + Vector2Int.left;

		if(_gameBoard.GetCell(Center) == CellStates.None) possibleMove.Add(Center);

		if(Up.x >= 0 && Up.x < _gameBoard.Width && Up.y >= 0 && Up.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Up) != CellStates.Wall) possibleMove.Add(Up);
		}
		if(Right.x >= 0 && Right.x < _gameBoard.Width && Right.y >= 0 && Right.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Right) != CellStates.Wall) possibleMove.Add(Right);
		}
		if(Down.x >= 0 && Down.x < _gameBoard.Width && Down.y >= 0 && Down.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Down) != CellStates.Wall) possibleMove.Add(Down);
		}
		if(Left.x >= 0 && Left.x < _gameBoard.Width && Left.y >= 0 && Left.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Left) != CellStates.Wall) possibleMove.Add(Left);
		}

		Assert.IsTrue(possibleMove.Count > 0);

		return possibleMove.ToArray();
	}

	// 0 None, 1 up, 2 right, 3 down, 4 left, 5 bomb
	public bool[] GetPossibleActions(Vector2 position)
	{
		bool[] possibleMove = new bool[6];
		int x = Round(position.x);
		int y = Round(position.y);
		Vector2Int center = new Vector2Int(x, y);

		possibleMove[0] = true;
		if (_gameBoard.GetCell(center) == CellStates.None) possibleMove[5] = true;
		
		const float e = .1f;
		bool centeredOnX = (Mathf.Abs(position.x - Round(position.x)) <= e);
		bool centeredOnY = (Mathf.Abs(position.y - Round(position.y)) <= e);

		if (centeredOnX && centeredOnY) // On 1 square, check everything
		{
			Vector2Int up = new Vector2Int(x,y+1); //.5f is player capsule radius
			Vector2Int right = new Vector2Int(x+1,y);
			Vector2Int down = new Vector2Int(x,y-1);
			Vector2Int left = new Vector2Int(x-1,y);
			
			if(PosIsInBoard(up))
			{
				if(_gameBoard.GetCell(up) != CellStates.Wall) possibleMove[1] = true;
			}
			if(PosIsInBoard(right))
			{
				if(_gameBoard.GetCell(right) != CellStates.Wall) possibleMove[2] = true;
			}
			if(PosIsInBoard(down))
			{
				if (_gameBoard.GetCell(down) != CellStates.Wall) possibleMove[3] = true;
			}
			if(PosIsInBoard(left))
			{
				if(_gameBoard.GetCell(left) != CellStates.Wall) possibleMove[4] = true;
			}
		}
		else if (centeredOnX) // On 2 squares (vertical) => Check 4 horizontal cells (both left & right)
		{
			possibleMove[1] = true;
			possibleMove[3] = true;

			int yFloor = Mathf.FloorToInt(position.y);
			
			Vector2Int topLeft = new Vector2Int(x-1,yFloor + 1);
			Vector2Int topRight = new Vector2Int(x+1,yFloor + 1);
			Vector2Int bottomLeft = new Vector2Int(x-1,yFloor);
			Vector2Int bottomRight = new Vector2Int(x+1,yFloor);

			if(PosIsInBoard(topRight) && PosIsInBoard(bottomRight))
			{
				if (_gameBoard.GetCell(topRight) != CellStates.Wall && _gameBoard.GetCell(bottomRight) != CellStates.Wall) possibleMove[2] = true;
			}
			if(PosIsInBoard(topLeft) && PosIsInBoard(bottomLeft))
			{
				if(_gameBoard.GetCell(topLeft) != CellStates.Wall && _gameBoard.GetCell(bottomLeft) != CellStates.Wall) possibleMove[4] = true;
			}
		}
		else if (centeredOnY) // On 2 squares (horizontal) => Check 4 vertical cells (both up & down)
		{
			possibleMove[2] = true;
			possibleMove[4] = true;
			
			int xFloor = Mathf.FloorToInt(position.x);
			
			Vector2Int topLeft = new Vector2Int(xFloor,y+1);
			Vector2Int bottomLeft = new Vector2Int(xFloor,y-1);
			Vector2Int topRight = new Vector2Int(xFloor+1,y+1);
			Vector2Int bottomRight = new Vector2Int(xFloor+1,y-1);

			if(PosIsInBoard(topLeft) && PosIsInBoard(topRight))
			{
				if(_gameBoard.GetCell(topLeft) != CellStates.Wall && _gameBoard.GetCell(topRight) != CellStates.Wall) possibleMove[1] = true;
			}
			if(PosIsInBoard(bottomLeft) && PosIsInBoard(bottomRight))
			{
				if (_gameBoard.GetCell(bottomLeft) != CellStates.Wall && _gameBoard.GetCell(bottomRight) != CellStates.Wall) possibleMove[3] = true;
			}
		}
		else // On 4 squares => Free movement
		{
			possibleMove[1] = true;
			possibleMove[2] = true;
			possibleMove[3] = true;
			possibleMove[4] = true;
		}

		Assert.IsTrue(possibleMove.Length > 0);

		return possibleMove;

		bool PosIsInBoard(Vector2Int pos)
		{
			return pos.x >= 0 && pos.x < _gameBoard.Width && pos.y >= 0 && pos.y < _gameBoard.Height;
		}
	}

	public bool IsPositionValidToMove(int x, int y)
	{
		if(x >= 0 && x < _gameBoard.Width && y >= 0 && y < _gameBoard.Height)
		{
			return (_gameBoard.GetCell(x, y) == CellStates.None);
		}

		return false;
	}

	public static int Round(float input)
	{
		return Mathf.RoundToInt(input);
	}

	public static Vector2Int Round(Vector2 input)
	{
		return new Vector2Int(Round(input.x), Round(input.y));
	}

	public CellStates GetCell(Vector2 position)
	{
		return _gameBoard.GetCell(Round(position));
	}

	public void UpdatePlayer(PlayerUpdateResult? result)
	{
		if (result == null) return;	
		if (result.Value.HasDropBomb)
		{
			var pos = result.Value.Position;
			Vector2Int position = Game.Round(pos);
			var cell = GetGameBoard().GetCell(position.x, position.y);
			if (cell == CellStates.None)
			{
				GetGameBoard().SetCell(position, CellStates.Bomb);
			}
		}
	}
	public void UpdatePlayers(PlayerUpdateResult?[] results)
	{
		for (int i = 0; i < results.Length; i++)
		{
			if (results[i] == null) continue;	
			if (results[i].Value.HasDropBomb)
			{
				var pos = results[i].Value.Position;
				Vector2Int position = Game.Round(pos);
				var cell = GetGameBoard().GetCell(position.x, position.y);
				if (cell == CellStates.None)
				{
					GetGameBoard().SetCell(position, CellStates.Bomb);
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
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
			_gameBoard[i] = CellStatesHelper.GetRandomCellStates();
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
		int x = (int)positionX;
		int z = (int)positionZ;
		return PositionHasExploded(x, z);
	}

	public bool PositionHasExploded(int positionX, int positionZ)
	{
		return _gameBoard.PositionHasExploded(positionX, positionZ);
	}

	public Vector2Int[] GetPossibleDirection(Vector2 position)
	{
		List<Vector2Int> possibleMove = new List<Vector2Int>(5);
		int x = (int)(position.x);
		int y = (int)(position.y);
		Vector2Int Center = new Vector2Int(x, y);
		Vector2Int Up = Center + Vector2Int.up;
		Vector2Int Right = Center + Vector2Int.right;
		Vector2Int Down = Center + Vector2Int.down;
		Vector2Int Left = Center + Vector2Int.left;

		if(_gameBoard.GetCell(Center) == CellStates.None) possibleMove.Add(Center);

		if(Up.x >= 0 && Up.x < _gameBoard.Width && Up.y >= 0 && Up.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Up) == CellStates.None) possibleMove.Add(Up);
		}
		if(Right.x >= 0 && Right.x < _gameBoard.Width && Right.y >= 0 && Right.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Right) == CellStates.None) possibleMove.Add(Right);
		}
		if(Down.x >= 0 && Down.x < _gameBoard.Width && Down.y >= 0 && Down.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Down) == CellStates.None) possibleMove.Add(Down);
		}
		if(Left.x >= 0 && Left.x < _gameBoard.Width && Left.y >= 0 && Left.y < _gameBoard.Height)
		{
            if(_gameBoard.GetCell(Left) == CellStates.None) possibleMove.Add(Left);
		}

		Assert.IsTrue(possibleMove.Count > 0);

		return possibleMove.ToArray();
	}
	
	// 0 None, 1 up, 2 right, 3 down, 4 left
	public bool[] GetPossibleActions(Vector2 position)
	{
		bool[] possibleMove = new bool[5];
		int x = Round(position.x);
		int y = Round(position.y);
		Vector2Int Center = new Vector2Int(x, y);
		Vector2Int Up = new Vector2Int(x,Round(position.y + 0.5f)); //.5f is player radius
		Vector2Int Right = new Vector2Int(Round(position.x + 0.5f),y);
		Vector2Int Down = new Vector2Int(x,Round(position.y - 0.5f));
		Vector2Int Left = new Vector2Int(Round(position.x - 0.5f),y);
		
		if(_gameBoard.GetCell(Center) == CellStates.None) possibleMove[0] = true;

		if(Up.x >= 0 && Up.x < _gameBoard.Width && Up.y >= 0 && Up.y < _gameBoard.Height)
		{
			if(_gameBoard.GetCell(Up) == CellStates.None) possibleMove[1] = true;
		}
		if(Right.x >= 0 && Right.x < _gameBoard.Width && Right.y >= 0 && Right.y < _gameBoard.Height)
		{
			if(_gameBoard.GetCell(Right) == CellStates.None) possibleMove[2] = true;
		}
		if(Down.x >= 0 && Down.x < _gameBoard.Width && Down.y >= 0 && Down.y < _gameBoard.Height)
		{
			if (_gameBoard.GetCell(Down) == CellStates.None) possibleMove[3] = true;
		}
		if(Left.x >= 0 && Left.x < _gameBoard.Width && Left.y >= 0 && Left.y < _gameBoard.Height)
		{
			if(_gameBoard.GetCell(Left) == CellStates.None) possibleMove[4] = true;
		}

		Assert.IsTrue(possibleMove.Length > 0);

		return possibleMove;
	}

	public bool IsPositionValidToMove(int x, int y)
	{
		if(x >= 0 && x < _gameBoard.Width && y >= 0 && y < _gameBoard.Height)
		{
			return (_gameBoard.GetCell(x, y) == CellStates.None);
		}

		return false;
	}

	private int Round(float input)
	{
		return Mathf.RoundToInt(input);
	}
}
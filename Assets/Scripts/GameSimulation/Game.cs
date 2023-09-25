using System;
using UnityEngine;

[Serializable]
public class Game
{
	[SerializeField]
	private GameBoard _gameBoard;

	public Game(IGameParameters parameters)
	{
		this._gameBoard = new GameBoard(parameters);
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

}
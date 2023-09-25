using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class GameBoard : IReadOnlyList<CellStates>
{
	[OdinSerialize, ShowInInspector]
	private IGameParameters gameParameters;
	[SerializeField, ReadOnly]
	private CellStates[] gameboard;
	private Nullable<float>[] bombTimers;
	private List<int> bombs;

	public int Width => gameParameters.Width;
	public int Height => gameParameters.Height;

	public GameBoard(GameBoard board)
	{
		this.gameParameters = board.gameParameters;
		this.gameboard = new CellStates[board.gameboard.Length];
		for (int i = 0; i < board.gameboard.Length; i++)
		{
			this.gameboard[i] = board.gameboard[i];
		}
		this.bombTimers = new float?[board.bombTimers.Length];
		for (int i = 0; i < board.bombTimers.Length; i++)
		{
			this.bombTimers[i] = board.bombTimers[i];
		}
		this.bombs = new List<int>(board.bombs.Count);
		for (int i = 0; i < board.bombs.Count; i++)
		{
			this.bombs.Add(board.bombs[i]);
		}
	}

	public GameBoard(IGameParameters parameters)
	{
		this.gameParameters = parameters;
		this.gameboard = new CellStates[parameters.Width * parameters.Height];
        this.bombTimers = new float?[parameters.Width * parameters.Height];
		this.bombs = new List<int>(2);
	}

	#region Implement IReadOnlyList

	public CellStates GetCell(int index)
	{
		Assert.IsTrue(index >= 0 && index < gameboard.Length, $"The index must be between [0, {Count}[.");
		return gameboard[index];
	}

	public CellStates GetCell(Vector2Int position)
	{
		return GetCell(position.x, position.y);
	}

	public CellStates GetCell(int x, int y)
	{
		int index = x + (y * Width);
		return GetCell(index);
	}

	public void SetCell(int index, CellStates cell)
	{
		Assert.IsTrue(index >= 0 && index < gameboard.Length, $"The index must be between [0, {Count}[.");
		if (gameboard[index] == CellStates.Bomb)
		{
			bombTimers[bombs[index]] = null;
			bombs.Remove(index);
		}

		gameboard[index] = cell;

		if (cell == CellStates.Bomb)
		{
			bombs.Add(index);
			bombTimers[index] = gameParameters.BombTimer;
		}
	}

	public void SetCell(int x, int y, CellStates cell)
	{
		int index = x + (y * Width);
		SetCell(index, cell);
	}
	
	public void SetCell(Vector2Int position, CellStates cell)
	{
		SetCell(position.x, position.y, cell);
	}
	public IEnumerator<CellStates> GetEnumerator()
	{
		return gameboard.AsEnumerable().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return gameboard.GetEnumerator();
	}

	public int Count => gameboard.Length;

	public CellStates this[int index]
	{
		get => GetCell(index);
		set => SetCell(index, value);
	}

	#endregion

	private Vector2Int GetPosition(int index)
	{
		return new Vector2Int(index % Width, index / Width);
	}

	private int GetIndex(Vector2Int position)
	{
		return GetIndex(position.x, position.y);
	}
	private int GetIndex(int x, int y)
	{
		return x + (y * Width);
	}

	public void Update(float ts)
	{
		for (int i = 0; i < bombs.Count; i++)
		{
			bombTimers[bombs[i]] -= ts;
		}
	}

	public bool PositionHasExploded(int x, int y)
	{
		int bombRadius = gameParameters.BombRadius;
		for (int i = 0; i < bombs.Count; i++)
		{
			Vector2Int bombPosition = GetPosition(bombs[i]);
			Assert.IsTrue(bombTimers[bombs[i]].HasValue, "The timer is supposed to exist.");
			if(bombTimers[bombs[i]] > 0) continue;
			if (bombPosition.x != x && bombPosition.y != y) continue;

			if (bombPosition.x == x && (bombPosition.y - bombRadius < y && bombPosition.y + bombRadius > y))
			{
				return true;
			}
			
			if (bombPosition.y == y && (bombPosition.x - bombRadius < x && bombPosition.x + bombRadius > x))
			{
				return true;
			}
		}

		return false;
	}
}
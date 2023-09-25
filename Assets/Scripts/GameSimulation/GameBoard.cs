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
	private List<int> bombIndexes;
	private List<int> explodedBombIndexes;

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
		this.bombIndexes = new List<int>(board.bombIndexes.Count);
		for (int i = 0; i < board.bombIndexes.Count; i++)
		{
			this.bombIndexes.Add(board.bombIndexes[i]);
		}
		this.explodedBombIndexes = new List<int>(board.explodedBombIndexes.Count);
		for (int i = 0; i < board.explodedBombIndexes.Count; i++)
		{
			this.explodedBombIndexes.Add(board.explodedBombIndexes[i]);
		}
	}

	public GameBoard(IGameParameters parameters)
	{
		this.gameParameters = parameters;
		this.gameboard = new CellStates[parameters.Width * parameters.Height];
        this.bombTimers = new float?[parameters.Width * parameters.Height];
		this.bombIndexes = new List<int>(2);
		this.explodedBombIndexes = new List<int>(2);
	}

	private void ExplodeCell(Vector2Int pos)
	{
		int index = GetIndex(pos);
		if (gameboard[index] == CellStates.Bomb)
		{
			if(!explodedBombIndexes.Contains(GetIndex(pos))) Detonate(GetPosition(index));
		}
		else
		{
			SetCell(index, CellStates.None);
		}
	}

	private void Detonate(Vector2Int center)
	{
		if (!explodedBombIndexes.Contains(GetIndex(center))) explodedBombIndexes.Add(GetIndex(center));
		else return;

		for (int j = 0; j < 2; j++)
		{
			for (int k = -gameParameters.BombRadius; k <= gameParameters.BombRadius; k++)
			{
				Vector2Int explodedPos = center;
				explodedPos[j] += k;
				if(explodedPos.x >= 0 && explodedPos.x < Width && explodedPos.y >= 0 && explodedPos.y < Height) ExplodeCell(explodedPos);
			}
		}
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
			bombTimers[index] = null;
			bombIndexes.Remove(index);
		}

		gameboard[index] = cell;

		if (cell == CellStates.Bomb)
		{
			bombIndexes.Add(index);
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
		for (int i = bombIndexes.Count - 1; i >= 0; i--)
		{
			bombTimers[bombIndexes[i]] -= ts;
			if (bombTimers[bombIndexes[i]] <= 0)
			{
				Detonate(GetPosition(bombIndexes[i]));
			}

			if (bombTimers[bombIndexes[i]] <= -gameParameters.BombExplosionTimer)
			{
				SetCell(bombIndexes[i], CellStates.None);
			}
		}
	}

	public bool PositionHasExploded(int x, int y)
	{
		int bombRadius = gameParameters.BombRadius;
		for (int i = 0; i < bombIndexes.Count; i++)
		{
			Vector2Int bombPosition = GetPosition(bombIndexes[i]);
			Assert.IsTrue(bombTimers[bombIndexes[i]].HasValue, "The timer is supposed to exist.");
			if(bombTimers[bombIndexes[i]] > 0) continue;
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
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

	public int Width => gameParameters.Width;
	public int Height => gameParameters.Height;

	public GameBoard(GameBoard board)
	{
		this.gameParameters = board.gameParameters;
		this.gameboard = board.gameboard;
	}
	public GameBoard(IGameParameters parameters)
	{
		this.gameParameters = parameters;
		this.gameboard = new CellStates[parameters.Width * parameters.Height];
	}

	#region Implement IReadOnlyList

	public CellStates GetCell(int index)
	{
		Assert.IsTrue(index >= 0 && index < gameboard.Length, $"The index must be between [0, {Count}[.");
		return gameboard[index];
	}

	public CellStates GetCell(int x, int y)
	{
		int index = x + (y * Width);
		return GetCell(index);
	}

	public void SetCell(int index, CellStates cell)
	{
		Assert.IsTrue(index >= 0 && index < gameboard.Length, $"The index must be between [0, {Count}[.");
		gameboard[index] = cell;
	}

	public void SetCell(int x, int y, CellStates cell)
	{
		int index = x + (y * Width);
		SetCell(index, cell);
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
}
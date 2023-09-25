using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class GameRenderer : MonoBehaviour
{
	[ShowInInspector, ReadOnly]
	private Game game => GameManager.Instance.GetCurrentGame();

	[SerializeField] private Transform boardParent;
	[SerializeField] private float height = 0;
	[SerializeField, Required] private GameObject none;
	[SerializeField, Required] private GameObject bomb;
	[SerializeField, Required] private GameObject wall;

	private GameObject[] renderBoard;

	private void Start()
	{
		InitBoard();
	}

	[Button]
	private void InitBoard()
	{
		var board = game.GetGameBoard();
		var w = board.Width;
		var h = board.Height;
		boardParent.position = new Vector3(-((float)w)*0.5f, height, -((float)h)*0.5f);
		renderBoard = new GameObject[board.Count];
		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				
				Vector3 position = new Vector3(x, 0, y);
				var cell = board.GetCell(x, y);
				var srcObj = GetAssociatedGameObject(cell);
				var instance = Instantiate(srcObj, boardParent, false);
				renderBoard[x + y * w] = instance;
				instance.transform.localPosition = position;

			}
		}
	}

	private GameObject GetAssociatedGameObject(CellStates cell)
	{
		return cell switch
		{
			CellStates.None => none,
			CellStates.Bomb => bomb,
			CellStates.Wall => wall,
			_ => throw new ArgumentException($"The argument {cell} is not valid for the {nameof(GetAssociatedGameObject)} function."),
		};
	}
}
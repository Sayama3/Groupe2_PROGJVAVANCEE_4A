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

	private GameBoard currentGameBoard;
	private GameObject[] renderBoard;
	private GameObject[] players;

	private void Start()
	{
		InitBoard();
		InitPlayer();
	}

	[Button]
	private void InitBoard()
	{
        //TODO: Destroy Existing.
		var board = game.GetGameBoard();
		currentGameBoard = board;
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

	private void InitPlayer()
	{
        //TODO: Destroy Existing.
		var instance = PlayerManager.Instance;
		players = new GameObject[instance.players.Count];
		for (int i = 0; i < instance.players.Count; i++)
		{
			var player = instance.players[i];
			Vector3 position = player.Position;
			Quaternion rotation = player.Rotation;
			GameObject src = player.PrefabSource;
			players[i] = Instantiate(src, boardParent, false);
			var playerTransform = players[i].transform;
			playerTransform.localPosition = position;
			playerTransform.localRotation = rotation;
		}
	}

	private void Update()
	{
		UpdateBoard();
		UpdatePlayers();
	}

	private void UpdateBoard()
	{
		var board = game.GetGameBoard();
		var w = board.Width;
		var h = board.Height;
		//Update board.
		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				int index = x + y * w;
				var cell = board.GetCell(index);
				var currentCell = currentGameBoard.GetCell(index);
				if (currentCell != cell)
				{
					Destroy(renderBoard[index]);
					Vector3 position = new Vector3(x, 0, y);
					var srcObj = GetAssociatedGameObject(cell);
					var instance = Instantiate(srcObj, boardParent, false);
					renderBoard[x + y * w] = instance;
					instance.transform.localPosition = position;
				}
			}
		}
	}

	private void UpdatePlayers()
	{
		//Update Player positino and rotation.
		var instance = PlayerManager.Instance;
		Assert.AreEqual(instance.players.Count, players.Length);
		for (int i = 0; i < players.Length; i++)
		{
			var player = instance.players[i];
			Vector3 position = player.Position;
			Quaternion rotation = player.Rotation;

			var playerTransform = players[i].transform;
			playerTransform.localPosition = position;
			playerTransform.localRotation = rotation;
			if (game.PositionHasExploded(position.x, position.z))
			{
				//TODO: Player i death. continue to see if there is more.
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
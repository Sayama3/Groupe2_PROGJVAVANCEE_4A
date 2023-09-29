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
	[SerializeField, Required] private CellObject CellObject;

	private GameBoard currentGameBoard;
	private CellObject[] renderBoard;
	private GameObject[] players;
	private Animator[] animators;
	private bool needToInit = true;
	private void Start()
	{
		GameManager.Instance.OnGameStart += StartEverything;
		GameManager.Instance.OnGameEnd += EndGame;
		if(GameManager.GameIsOn()) StartEverything();
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameStart -= StartEverything;
		GameManager.Instance.OnGameStart -= EndGame;
	}

	private void EndGame()
	{
		UpdateBoard();
		UpdatePlayers();
	}

	private void StartEverything()
	{
		// DestroyPreviousBoard();
		InitBoard();
		InitPlayer();
	}

	private void DestroyPreviousBoard()
	{
		if(renderBoard is { Length: > 0 })
		{
			for (int i = renderBoard.Length - 1; i >= 0; i--)
			{
				if(renderBoard[i] == null) continue;
				Destroy(renderBoard[i].gameObject);
				renderBoard[i] = null;
			}
		}

		if (players is { Length: > 0 })
		{
			for (int i = players.Length - 1; i >= 0; i--)
			{
				if(players[i] == null) continue;
				Destroy(players[i].gameObject);
				players[i] = null;
			}
		}
	}
	
	private void InitBoard()
	{
		currentGameBoard = game.GetCopyGameBoard();
		var w = currentGameBoard.Width;
		var h = currentGameBoard.Height;
		boardParent.position = new Vector3(-((float)w)*0.5f, height, -((float)h)*0.5f);
		renderBoard = new CellObject[currentGameBoard.Count];
		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				Vector3 position = new Vector3(x, 0, y);
				var instance = Instantiate(CellObject, boardParent, false);
				renderBoard[x + y * w] = instance;
				instance.transform.localPosition = position;
			}
		}
	}

	private void InitPlayer()
	{
		var instance = PlayerManager.Instance;
		players = new GameObject[instance.players.Count];
		animators = new Animator[players.Length];
		for (int i = 0; i < instance.players.Count; i++)
		{
			var player = instance.players[i];
			Vector3 position = new Vector3(player.Position.x, 0, player.Position.y);
			GameObject src = player.PrefabSource;
			players[i] = Instantiate(src, boardParent, false);
			animators[i] = players[i].GetComponentInChildren<Animator>();
			var playerTransform = players[i].transform;
			playerTransform.localPosition = position;
		}
	}

	private void Update()
	{
		if(GameManager.GameIsOn())
		{
			if (needToInit)
			{
				for (int i = 0; i < renderBoard.Length; i++)
				{
					renderBoard[i].SetCell(currentGameBoard.GetCell(i));
				}

				needToInit = false;
			}
			UpdateBoard();
			UpdatePlayers();
		}
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
				CellStates cell = board.GetCell(index);
				CellStates currentCell = currentGameBoard.GetCell(index);
				if (currentCell != cell)
				{
					renderBoard[index].SetCell(cell);
				}

				if (cell == CellStates.None)
				{
					renderBoard[index].SetCell(cell, board.PositionHasExploded(x, y));
				}
			}
		}
	}

	private void UpdatePlayers()
	{
		//Update Player position and rotation.
		var gamePlayers = GameManager.Instance.GetPlayers();
		Assert.AreEqual(gamePlayers.Length, players.Length);
		for (int i = 0; i < players.Length; i++)
		{
			if (gamePlayers[i] == null)
			{
				if(players[i] != null)
				{
					players[i].SetActive(false);
					players[i] = null;
				}
				continue;
			}
			var player = gamePlayers[i];
			
			Vector3 lastPosition = new Vector3(player.LastPosition.x, 0, player.LastPosition.y);
			Vector3 position = new Vector3(player.Position.x, 0, player.Position.y);

			var playerTransform = players[i].transform;
			playerTransform.localPosition = position;

			if (!(player.LastPosition == player.Position))
			{
				animators[i].SetBool("Walking", true);
				playerTransform.forward = position - lastPosition;
			}
			else
			{
				animators[i].SetBool("Walking", false);
			}
		}
	}

}
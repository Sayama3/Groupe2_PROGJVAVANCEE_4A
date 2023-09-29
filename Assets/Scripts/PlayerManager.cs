using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	#region Singleton

	private static PlayerManager instance;

	private static PlayerManager GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<PlayerManager>();
			if (instance == null)
			{
				var obj = new GameObject(nameof(PlayerManager), typeof(PlayerManager));
				instance = obj.GetComponent<PlayerManager>();
			}
		}
		return instance;
	}

	public static PlayerManager Instance => GetInstance();

	public void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this);
		instance = this;
		InitSelf();
	}

	#endregion

	public List<IPlayerController> players = new ();
	[SerializeField] private GameObject humanPrefab;
	[SerializeField] private GameObject iaPrefab;
	[SerializeField] private GameObject randomPrefab;

	private void InitSelf()
	{
		players.Clear();
		int humanCount = 0;
		if (MenuToGame.Instance != null)
		{
			foreach (PlayerType type in MenuToGame.Instance.playerTypes)
			{
				switch (type)
				{
					case PlayerType.None:
						break;
					case PlayerType.Human:
						switch (humanCount)
						{
							case 0:
								players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.One));
								break;
							case 1:
								players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.Two));
								break;
							case 2:
								players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.Three));
								break;
							case 3:
								players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.Four));
								break;
						}
						humanCount++;
						break;
					case PlayerType.Random:
						players.Add(new RandomPlayerController(randomPrefab));
						break;
					case PlayerType.MCTS:
						players.Add(new MCTSPlayerController(iaPrefab));
						break;
				}
			}
		}
		else
		{
			players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.One));
			players.Add(new HumanPlayerController(humanPrefab, HumanPlayerIndex.Two));
			players.Add(new RandomPlayerController(randomPrefab));
		}
	}

	// public PlayerUpdateResult[] UpdatePlayers(float dt, ref Game currentGame)
	// {
	// 	PlayerUpdateResult[] results = new PlayerUpdateResult[players.Count];
	// 	for (int i = 0; i < players.Count; i++)
	// 	{
	// 		var copy = new Game(currentGame);
	// 		IPlayerController player = players[i];
	// 		results[i] = player.Update(dt, currentGame);
	// 	}
	//
	// 	return results;
	// }

	public static void Init()
	{
		Instance.InitSelf();
	}
}

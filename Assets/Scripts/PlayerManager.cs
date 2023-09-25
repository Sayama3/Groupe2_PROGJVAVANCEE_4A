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

	private void InitSelf()
	{
		
	}

	public PlayerUpdateResult[] UpdatePlayers(float dt, ref Game currentGame)
	{
		PlayerUpdateResult[] results = new PlayerUpdateResult[players.Count];
		for (int i = 0; i < players.Count; i++)
		{
			var copy = new Game(currentGame);
			IPlayerController player = players[i];
			// results[i] = player.Update(dt, currentGame);
		}

		return results;
	}
}
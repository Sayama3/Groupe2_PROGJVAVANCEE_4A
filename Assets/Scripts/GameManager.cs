using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class GameManager : SerializedMonoBehaviour
{
    #region Singleton

    private static GameManager instance;

    private static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();
            if (instance == null)
            {
                var obj = new GameObject(nameof(GameManager), typeof(GameManager));
                instance = obj.GetComponent<GameManager>();
            }
        }
        return instance;
    }
    public static GameManager Instance => GetInstance();
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

    [SerializeField]
    private ScriptableGameParameters gameparameters;
    [ShowInInspector, ReadOnly]
    private Game game;

    [Button]
    private void InitSelf()
    {
        game = new Game(gameparameters);
        game.Fill(CellStates.Wall);
        SetupPlayers();
    }

    private void SetupPlayers()
    {
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            int randomX = Random.Range(1, game.Width - 1);
            int randomY = Random.Range(1, game.Height - 1);

            PlayerManager.Instance.players[i].Position = new Vector2(randomX, randomY);
            game.GetGameBoard().SetCell(randomX, randomY, CellStates.None);
            game.GetGameBoard().SetCell(randomX+1, randomY, CellStates.None);
            game.GetGameBoard().SetCell(randomX-1, randomY, CellStates.None);
            game.GetGameBoard().SetCell(randomX, randomY+1, CellStates.None);
            game.GetGameBoard().SetCell(randomX, randomY-1, CellStates.None);
            game.GetGameBoard().SetCell(randomX+1, randomY-1, CellStates.None);
            game.GetGameBoard().SetCell(randomX-1, randomY+1, CellStates.None);
        }
        // TODO: Add player position ?
        // TODO: Add empty spots around player
    }

    public Game GetCurrentGame()
    {
        return game;
    }
    public ScriptableGameParameters GetCurrentGameParams()
    {
        return gameparameters;
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        UpdatePlayers(dt);
        UpdateGame(dt);
        CheckPlayersDeath();
    }

    private void UpdatePlayers(float dt)
    {
        var results = PlayerManager.Instance.UpdatePlayers(dt, ref game);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].HasDropBomb)
            {
                var pos = results[i].Position;
                Vector2Int position = new Vector2Int((int)pos.x, (int)pos.y);
                var cell = game.GetGameBoard().GetCell(position.x, position.y);
                if (cell == CellStates.None)
                {
                    game.GetGameBoard().SetCell(position, CellStates.Bomb);
                }
            }
        }
    }

    private void UpdateGame(float dt)
    {
        game.Update(dt);
    }

    private void CheckPlayersDeath()
    {
        bool[] playerDead = new bool[PlayerManager.Instance.players.Count];
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            var player = PlayerManager.Instance.players[i];
            playerDead[i] = game.PositionHasExploded(player.Position.x, player.Position.y);
        }

        if (playerDead.Length > 0 && playerDead.Any( p => p))
        {
            Debug.LogWarning("Someone is dead.");
            //TODO: triggerthe errors.
        }
    }
}

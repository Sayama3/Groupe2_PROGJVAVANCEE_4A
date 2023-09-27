using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    }

    #endregion

    public static bool Pause
    {
        get => Instance.isPaused;
        set
        {
            if(value != Instance.isPaused)
            {
                Instance.isPaused = value;
                Instance.OnPause?.Invoke(Instance.isPaused);
            }
        }
    }
    public static void StartGame() { Instance.PrivateStartGame();}
    public static bool GameIsOn() => Instance.gameIsOn;

    private bool isPaused = false;
    private bool gameIsOn = false;
    
    [SerializeField]
    private ScriptableGameParameters gameparameters;

    [ShowInInspector, ReadOnly]
    private Game game;

    public Action<bool> OnPause;
    public Action OnGameStart;
    public Action OnGameEnd;
    public Action OnGameTie;
    public Action<IPlayerController, int> OnPlayerWin;
    public Action<IPlayerController, int> OnPlayerLoose;

    private IPlayerController[] players;


    [Button]
    private void PrivateStartGame()
    {
        game = new Game(gameparameters);
        game.Fill(CellStates.Wall);
        SetupPlayers();
        gameIsOn = true;
        OnGameStart?.Invoke();
    }

    private void SetupPlayers()
    {
        players = new IPlayerController[PlayerManager.Instance.players.Count];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = PlayerManager.Instance.players[i];
            
            int randomX = Random.Range(1, game.Width - 1);
            int randomY = Random.Range(1, game.Height - 1);

            players[i].Id = i;
            players[i].Position = new Vector2(randomX, randomY);
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
        if(isPaused || !gameIsOn) return;
        var dt = Time.deltaTime;
        UpdatePlayers(dt);
        UpdateGame(dt);
        CheckPlayersDeath();
    }

    private void UpdatePlayers(float dt)
    {
        var results = UpdatePlayers(dt, ref game);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] == null) continue;
            if (results[i].Value.HasDropBomb)
            {
                var pos = results[i].Value.Position;
                Vector2Int position = Game.Round(pos);
                var cell = game.GetGameBoard().GetCell(position.x, position.y);
                if (cell == CellStates.None)
                {
                    game.GetGameBoard().SetCell(position, CellStates.Bomb);
                }
            }
        }
        game.UpdatePlayers(results);
    }
    
    public PlayerUpdateResult?[] UpdatePlayers(float dt, ref Game currentGame)
    {
        PlayerUpdateResult?[] results = new PlayerUpdateResult?[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                results[i] = null;
                continue;
            }
            var copy = new Game(currentGame);
            results[i] = players[i].Update(dt, currentGame);
        }

        return results;
    }

    private void UpdateGame(float dt)
    {
        game.Update(dt);
    }

    private void CheckPlayersDeath()
    {
        bool[] playerDead = new bool[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];
            if (player == null)
            {
                playerDead[i] = true;
                continue;
            }
            playerDead[i] = game.PositionHasExploded(player.Position.x, player.Position.y);
        }

        int numberPlayerDead = playerDead.Count(p => p);
        if (numberPlayerDead == playerDead.Length)
        {
            for (int i = 0; i < playerDead.Length; i++)
            {
                if (players[i] == null) continue;
                OnPlayerLoose?.Invoke(players[i], i);
                players[i] = null;
            }

            OnGameTie?.Invoke();
            gameIsOn = false;
            OnGameEnd?.Invoke();
        }
        else if (numberPlayerDead == playerDead.Length-1)
        {
            for (int i = 0; i < playerDead.Length; i++)
            {
                if (players[i] == null) continue;
                if (playerDead[i])
                {
                    OnPlayerLoose?.Invoke(players[i], i);
                    players[i] = null;
                }
                else
                {
                    OnPlayerWin?.Invoke(players[i], i);
                }
            }
            gameIsOn = false;
            OnGameEnd?.Invoke();
        }
        else if (numberPlayerDead > 0)
        {
            for (int i = 0; i < playerDead.Length; i++)
            {
                if (players[i] == null) continue;
                if (playerDead[i])
                {
                    OnPlayerLoose?.Invoke(players[i], i);
                    players[i] = null;
                }
            }
        }
    }

    public IPlayerController[] GetPlayers()
    {
        var p = new IPlayerController[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            p[i] = players[i];
        }

        return p;
    }
}

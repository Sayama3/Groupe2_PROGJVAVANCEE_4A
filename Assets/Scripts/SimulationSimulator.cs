using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class SimulationSimulator : MonoBehaviour
{
    
    // Current action, only one.
    [ShowInInspector, ReadOnly]
    private MCTSAction action = MCTSAction.Bomb;
    [ShowInInspector, ReadOnly]
    private Vector2?[] players;
    [ShowInInspector, ReadOnly]
    private readonly int PlayerCount = 2;
    [ShowInInspector, ReadOnly]
    private int currentPlayer = 0;
    [ShowInInspector, ReadOnly]
    private int playerTurn = 0;
    [ShowInInspector, ReadOnly]
    private Game copyGame;
    private Vector2 Position;
    [ShowInInspector, ReadOnly]
    private int turnCount;
    [SerializeField] private ScriptableGameParameters parameters;
    private PlayerUpdateResult?[] results;
    [ShowInInspector, ReadOnly]
    private bool gameEnd = false;
    [SerializeField] private CellObject CellObject;
    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;
    [SerializeField] private Transform boardParent;

    private CellObject[] renderBoard; 
    private GameObject playerInstanceOne;
    private GameObject playerInstanceTwo;

    private float dt;
    // Start is called before the first frame update
    [Button]
    void Init()
    {
        copyGame = new Game(parameters);
        players = new []{(Vector2?)new Vector2(1,1), (Vector2?)new Vector2(10,10)};
        Position = new Vector2(1,1);
        int currentPlayerTurn = 0;
        dt = MCTSHelper.SimulationDeltaTime;
        results = new PlayerUpdateResult?[PlayerCount];
        gameEnd = false;
        copyGame.Fill(CellStates.Wall);
        for (int i = 0; i < 2; i++)
        {
            copyGame.SetCell(players[i].Value.x, players[i].Value.y, CellStates.None);
            copyGame.SetCell(players[i].Value.x+1, players[i].Value.y, CellStates.None);
            copyGame.SetCell(players[i].Value.x-1, players[i].Value.y, CellStates.None);
            copyGame.SetCell(players[i].Value.x, players[i].Value.y+1, CellStates.None);
            copyGame.SetCell(players[i].Value.x, players[i].Value.y-1, CellStates.None);
            copyGame.SetCell(players[i].Value.x+1, players[i].Value.y-1, CellStates.None);
            copyGame.SetCell(players[i].Value.x-1, players[i].Value.y+1, CellStates.None);
        }
        InitBoard();
        InitPlayer();
        copyGame.GetGameBoard().SetCell((int)Position.x, (int)Position.y, CellStates.Bomb);
    }

    private void InitBoard()
    {
        var currentGameBoard = copyGame.GetGameBoard();
        var w = currentGameBoard.Width;
        var h = currentGameBoard.Height;
        boardParent.position = new Vector3(-((float)w)*0.5f, 0.0f, -((float)h)*0.5f);
        renderBoard = new CellObject[currentGameBoard.Count];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                var cell = currentGameBoard.GetCell(x, y);
                var instance = Instantiate(CellObject, boardParent, false);
                renderBoard[x + y * w] = instance;
                instance.transform.localPosition = position;
                instance.SetCell(currentGameBoard.GetCell(x,y));
            }
        }
    }

    private void InitPlayer()
    {
        {
            var player = players[0].Value;
            Vector3 position = new Vector3(player.x, 0, player.y);
            // Quaternion rotation = player.;
            playerInstanceOne = Instantiate(playerOne, boardParent, false);
            var playerTransform = playerInstanceOne.transform;
            playerTransform.localPosition = position;
            // playerTransform.localRotation = rotation;
        }
        {
            var player = players[1].Value;
            Vector3 position = new Vector3(player.x, 0, player.y);
            // Quaternion rotation = player.;
            playerInstanceTwo = Instantiate(playerTwo, boardParent, false);
            var playerTransform = playerInstanceTwo.transform;
            playerTransform.localPosition = position;
            // playerTransform.localRotation = rotation;
        }
    }
    private void UpdateBoard()
    {
        var board = copyGame.GetGameBoard();
        var w = board.Width;
        var h = board.Height;
        //Update board.
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int index = x + y * w;
                CellStates cell = board.GetCell(index);
                renderBoard[index].SetCell(cell, board.PositionHasExploded(x, y));
            }
        }
    }

    private void UpdatePlayers()
    {
        {
            Vector3 position = new Vector3(players[0].Value.x, 0, players[0].Value.y);
            playerInstanceOne.transform.localPosition = position;
        }
        {
            Vector3 position = new Vector3(players[1].Value.x, 0, players[1].Value.y);
            playerInstanceTwo.transform.localPosition = position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SimulateNextFrame();
            UpdateBoard();
            UpdatePlayers();
        }
    }

    private void SimulateNextFrame()
    {
        for (int player = 0; player < PlayerCount; player++)
        {
            if(!players[player].HasValue)
            {
                results[player] = null;
                continue;
            }

            var action = copyGame.GenerateAllPossibleRandomPlayerAction(players[player].Value);
            results[player] = action.ChooseRandomAction().GetPlayerUpdateResult(players[player].Value, dt);
            players[player] = results[player].Value.Position;
        }
        copyGame.UpdatePlayers(results);
        copyGame.Update(dt);

        Assert.IsTrue(players[currentPlayer].HasValue);

        for (int j = 0; j < PlayerCount; j++)
        {
            if (!players[j].HasValue) continue;
            bool playerIsDead = copyGame.PositionHasExploded(players[j].Value);
            if (playerIsDead)
            {
                Debug.Log($"Player {j} is dead.");
                players[j] = null;
                gameEnd |= players.Count(p => !p.HasValue) >= PlayerCount - 1;
            }
        }
    }
}

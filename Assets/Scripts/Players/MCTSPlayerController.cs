using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MCTSPlayerController : APlayerController
{
    private const int numberOfTests = 4;
    private const int numberOfSimulations = 8;

    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        PlayerUpdateResult res = new PlayerUpdateResult();
        res.Position = Position;
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        
        for (int i = 0; i < numberOfTests; i++)
        {
            GameBoard newGame = Expand(copyGame);
            int simulationScore = Simulate(newGame);
            BackPropagation(newGame,simulationScore,numberOfSimulations);
        }
        
        return res;
    }

    private GameBoard Expand(Game game)
    {
        Vector3 expandedPosition = Position;
        GameBoard expandedBoard = game.GetGameBoard();
        
        // Choose random move and play it
        PlayAction(FindRandomPossibleAction(game), expandedPosition, expandedBoard);
        
        // Other player does random thing
        
        
        // Continuer jusqu'à victoire/défaire

        return expandedBoard;
    }

    private int Simulate(GameBoard game)
    {
        int numberWin = 0;
        
        for (int i = 0; i < numberOfSimulations; i++)
        {
            // if win
            numberWin++;
        }
        
        return numberWin;
    }

    private void BackPropagation(GameBoard newGame, int numberVictory, int numberSimulation)
    {
        // ???
    }
    
    private GameActions FindRandomPossibleAction(Game game)
    {
        List<GameActions> allPossibleActions = new List<GameActions>();
        GameBoard currentBoard = game.GetCopyGameBoard();

        int Xrounded = Mathf.RoundToInt(Position.x);
        int Yrounded = Mathf.RoundToInt(Position.y);
        
        allPossibleActions.Add(GameActions.None);
        if (currentBoard.GetCell(Xrounded, Yrounded) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.Bomb);
        }
        if (currentBoard.GetCell(Xrounded - 1, Yrounded) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveLeft);
        }
        if (currentBoard.GetCell(Xrounded + 1, Yrounded) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveRight);
        }
        if (currentBoard.GetCell(Xrounded, Yrounded - 1) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveDown);
        }
        if (currentBoard.GetCell(Xrounded, Yrounded + 1) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveUp);
        }

        return allPossibleActions[Random.Range(0, allPossibleActions.Count)];
    }

    private void PlayAction(GameActions action, Vector3 position, GameBoard board)
    {
        switch (action)
        {
            case GameActions.None:
                return;
                break;
            case GameActions.MoveUp:
                position.z += 1; // change to speed
                break;
            case GameActions.MoveDown:
                position.z -= 1;
                break;
            case GameActions.MoveLeft:
                position.x -= 1;
                break;
            case GameActions.MoveRight:
                position.x += 1;
                break;
            case GameActions.Bomb:
                
                break;
        }
    }
}
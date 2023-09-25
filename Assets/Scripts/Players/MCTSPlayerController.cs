using System.Collections.Generic;
using UnityEngine;

public class MCTSPlayerController : APlayerController
{
    private const int numberOfTests = 4;
    private const int numberOfSimulations = 8;

    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        PlayerUpdateResult res = new PlayerUpdateResult();
        res.Position = Position;
        
        for (int i = 0; i < numberOfTests; i++)
        {
            Game newGame = Expand(copyGame);
            int simulationScore = Simulate(newGame);
            BackPropagation(newGame,simulationScore,numberOfSimulations);
        }

        
        
        return res;
    }

    private Game Expand(Game game)
    {
        // Choose possible move
        GameActions action = FindPossibleAction(game);
        // Other player does random thing
        // Continuer jusqu'à victoire/défaire

        
        
        return game;
    }

    private int Simulate(Game game)
    {
        int numberWin = 0;
        
        for (int i = 0; i < numberOfSimulations; i++)
        {
            // if win
            numberWin++;
        }
        
        return numberWin;
    }

    private void BackPropagation(Game newGame, int numberVictory, int numberSimulation)
    {
        // ???
    }
    
    private GameActions FindPossibleAction(Game game)
    {
        List<GameActions> allPossibleActions = new List<GameActions>(); // Might be useless to make a list
        GameBoard currentBoard = game.GetCopyGameBoard();
        
        allPossibleActions.Add(GameActions.None);
        if (game.GetCopyGameBoard().GetCell((int)Position.x, (int)Position.z) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.Bomb);
        }

        if (game.GetCopyGameBoard().GetCell((int)Position.x - 1, (int)Position.z) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveLeft);
        }
        if (game.GetCopyGameBoard().GetCell((int)Position.x + 1, (int)Position.z) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveRight);
        }
        if (game.GetCopyGameBoard().GetCell((int)Position.x, (int)Position.z - 1) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveDown);
        }
        if (game.GetCopyGameBoard().GetCell((int)Position.x, (int)Position.z + 1) == CellStates.None)
        {
            allPossibleActions.Add(GameActions.MoveUp);
        }

        return allPossibleActions[Random.Range(0, allPossibleActions.Count)];
    }
}
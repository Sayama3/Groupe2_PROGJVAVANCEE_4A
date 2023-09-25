using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class MCTSPlayerController : APlayerController
{
    private static int NumberOfTests => GameManager.Instance.GetCurrentGameParams().NumberOfTests;
    private static int NumberOfSimulations => GameManager.Instance.GetCurrentGameParams().NumberOfSimulations;
    private static float PlayerSpeed => GameManager.Instance.GetCurrentGameParams().Speed;

    public MCTSPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        
        
        return new PlayerUpdateResult() { HasDropBomb = false, Position = Position };
    }

    private static MCTSPlayerAction GenerateRandomPlayerAction(float dt, Game copyGame, Vector2 position)
    {
        Vector2? target = null;
        var action = Helpers.GetRandomEnum<GameActions>();
        switch (action)
        {
            case GameActions.None:
            {
                return new MCTSPlayerAction { GameAction = action, Position = position, HasDropBomb = false };
            }
            case GameActions.Move:
            {
                Vector2Int direction = copyGame.GetPossibleDirection(position).GetRandom();
                target = direction;
            }
                break;
            case GameActions.Bomb:
                return new MCTSPlayerAction { GameAction = action, Position = position, HasDropBomb = true};
        }

        if (target.HasValue)
        {
            position = Vector2.MoveTowards(position, target.Value, PlayerSpeed * dt);
            if (target == position)
            {
                target = null;
            }
        }

        return new MCTSPlayerAction { GameAction = action, Position = position, HasDropBomb = false };
    }

    private static MCTSPlayerAction[] GenerateAllPossibleRandomPlayerAction(float dt, Game copyGame, Vector2 position)
    {
        List<MCTSPlayerAction> playerActions = new List<MCTSPlayerAction>(6);
        // None Action
        playerActions.Add(new MCTSPlayerAction { GameAction = GameActions.None, Position = position, HasDropBomb = false });

        // Movement Actions
        var directions = copyGame.GetPossibleDirection(position);
        for (int i = 0; i < directions.Length; i++)
        {
            playerActions.Add(new MCTSPlayerAction { 
                GameAction = GameActions.Move, 
                Position = Vector2.MoveTowards(position, directions[i], PlayerSpeed * dt), 
                HasDropBomb = false
            });
        }

        // Bomb Action
        if (copyGame.GetCell(position) == CellStates.None)
        {
            playerActions.Add(new MCTSPlayerAction { GameAction = GameActions.Bomb, Position = position, HasDropBomb = true });
        }

        return playerActions.ToArray();
    }

}
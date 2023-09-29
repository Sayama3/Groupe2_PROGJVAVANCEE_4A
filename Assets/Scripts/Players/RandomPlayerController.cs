using System;
using UnityEngine;

public class RandomPlayerController : APlayerController
{
    
    private Vector2? target = null;
    public RandomPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }

    // public override PlayerUpdateResult Update(float dt, Game copyGame)
    // {
    //     GameActions action = GameActions.None;
    //     if (!target.HasValue)
    //     {
    //         action = Helpers.GetRandomEnum<GameActions>();
    //         switch (action)
    //         {
    //             case GameActions.None:
    //             {
    //                 return new PlayerUpdateResult { Position = Position, HasDropBomb = false };
    //             }
    //             case GameActions.Move:
    //             {
    //                 Vector2Int direction = copyGame.GetPossiblePositions(Position).GetRandom();
    //                 target = direction;
    //             }
    //                 break;
    //             case GameActions.Bomb:
    //                 return new PlayerUpdateResult{Position = Position, HasDropBomb = true};
    //         }
    //     }
    //
    //     if (target.HasValue)
    //     {
    //         Position = Vector2.MoveTowards(Position, target.Value, GameManager.Instance.GetCurrentGameParams().Speed * dt);
    //         if (target == Position)
    //         {
    //             target = null;
    //         }
    //     }
    //
    //     return new PlayerUpdateResult { Position = Position, HasDropBomb = false };
    // }

    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        target = null;
        var action = Helpers.GetRandomEnum<GameActions>();
        switch (action)
        {
            case GameActions.None:
            {
                return new PlayerUpdateResult { Position = Position, HasDropBomb = false };
            }
            case GameActions.Move:
            {
                Vector2Int direction = copyGame.GetPossiblePositions(Position).GetRandom();
                target = direction;
                Forward = new Vector3(direction.x, 0, direction.y);
            }
                break;
            case GameActions.Bomb:
                return new PlayerUpdateResult{Position = Position, HasDropBomb = true};
        }

        if (target.HasValue)
        {
            Position = Vector2.MoveTowards(Position, target.Value, GameManager.Instance.GetCurrentGameParams().Speed * dt);
            if (target == Position)
            {
                target = null;
            }
        }

        return new PlayerUpdateResult { Position = Position, HasDropBomb = false };
    }
}
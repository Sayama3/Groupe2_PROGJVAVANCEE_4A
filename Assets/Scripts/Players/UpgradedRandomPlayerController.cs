using System;
using UnityEngine;

public class UpgradedRandomPlayerController : APlayerController
{
    
    private Vector2? target = null;
    public UpgradedRandomPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }

    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        GameActions action = GameActions.None;
        if (!target.HasValue)
        {
            action = Helpers.GetRandomEnum<GameActions>();
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
                }
                    break;
                case GameActions.Bomb:
                    return new PlayerUpdateResult{Position = Position, HasDropBomb = true};
            }
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
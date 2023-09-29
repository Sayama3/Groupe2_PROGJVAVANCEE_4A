using UnityEngine;

public class RandomPlayerController : APlayerController
{
    
    private Vector2? target = null;
    public RandomPlayerController(GameObject prefab)
    {
        PrefabSource = prefab;
    }

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
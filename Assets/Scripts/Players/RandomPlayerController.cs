using UnityEngine;

public class RandomPlayerController : APlayerController
{
    public RandomPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        int randomAction;
        PlayerUpdateResult res = new PlayerUpdateResult();
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var Position = this.Position;
        bool hasDroppedBomb = false;
        
        bool[] possibleActions = copyGame.GetPossibleActions(Position);

        Back:
        randomAction = Random.Range(0, 5);
        if (possibleActions[randomAction])
        {
            switch (randomAction)
            {
                case 0:
                    res.HasDropBomb = true;
                    break;
                case 1:
                    Position.x -= speed * dt;
                    break;
                case 2:
                    Position.x += speed * dt;
                    break;
                case 3:
                    Position.y -= speed * dt;
                    break;
                case 4:
                    Position.y += speed * dt;
                    break;
            }
        }
        else
        {
            // Y'a sûrement mieux
            goto Back;
        }

        this.Position = Position;
        return new PlayerUpdateResult {HasDropBomb = hasDroppedBomb, Position = Position};
    }
}
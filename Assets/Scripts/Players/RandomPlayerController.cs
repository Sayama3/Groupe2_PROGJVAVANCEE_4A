using UnityEngine;

public class RandomPlayerController : APlayerController
{
    public RandomPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        int randomAction = Random.Range(0, 5);

        PlayerUpdateResult res = new PlayerUpdateResult();
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var Position = this.Position;
        bool hasDroppedBomb = false;
        
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
        
        this.Position = Position;
        return new PlayerUpdateResult {HasDropBomb = hasDroppedBomb, Position = Position};
    }
}
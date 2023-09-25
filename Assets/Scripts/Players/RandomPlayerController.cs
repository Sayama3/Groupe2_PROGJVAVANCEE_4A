using UnityEngine;

public class RandomPlayerController : APlayerController
{
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        int randomAction = Random.Range(0, 5);

        PlayerUpdateResult res = new PlayerUpdateResult();
        res.Position = Position;
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        
        switch (randomAction)
        {
            case 0:
                res.HasDropBomb = true;
                break;
            case 1:
                res.Position.x -= speed * dt;
                break;
            case 2:
                res.Position.x += speed * dt;
                break;
            case 3:
                res.Position.z -= speed * dt;
                break;
            case 4:
                res.Position.z += speed * dt;
                break;
        }
        
        return res;
    }
}
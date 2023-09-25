using UnityEngine;

public class HumanPlayerController : APlayerController
{
    public override PlayerUpdateResult Update(float dt, Game currentGame)
    {
        PlayerUpdateResult res = new PlayerUpdateResult();
        res.Position = Position;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            res.HasDropBomb = true;
        }
        else if(horizontalInput < 0f)
        {
            res.Position.x -= speed * dt;
        }
        else if(horizontalInput > 0f)
        {
            res.Position.x += speed * dt;
        }
        else if(verticalInput < 0f)
        {
            res.Position.z -= speed * dt;
        }
        else if(verticalInput > 0f)
        {
            res.Position.z += speed * dt;
        }

        return res;
    }
}
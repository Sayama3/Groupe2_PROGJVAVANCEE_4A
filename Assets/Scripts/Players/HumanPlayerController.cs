using UnityEngine;

public class HumanPlayerController : APlayerController
{
    public override PlayerUpdateResult Update(float dt, Game currentGame)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var Position = this.Position;
        
        bool hasDroppedBomb = Input.GetKeyDown(KeyCode.Space);
        if (!hasDroppedBomb)
        {
            if (horizontalInput < 0f)
            {
                Position.x -= speed * dt;
            }
            else if (horizontalInput > 0f)
            {
                Position.x += speed * dt;
            }
            else if (verticalInput < 0f)
            {
                Position.y -= speed * dt;
            }
            else if (verticalInput > 0f)
            {
                Position.y += speed * dt;
            }
        }

        this.Position = Position;
        return new PlayerUpdateResult {HasDropBomb = hasDroppedBomb, Position = Position};
    }
}
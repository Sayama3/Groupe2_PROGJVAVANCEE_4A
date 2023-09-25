using UnityEngine;

public class HumanPlayerController : APlayerController
{
    public HumanPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game currentGame)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var Position = this.Position;
        
        bool hasDroppedBomb = Input.GetKeyDown(KeyCode.Space);
        
        if (!hasDroppedBomb)
        {
            bool[] possibleActions = currentGame.GetPossibleActions(Position);
            if (verticalInput > 0 && possibleActions[1])
            {
                Position.y += speed * dt;
            }
            else if (horizontalInput > 0 && possibleActions[2])
            {
                Position.x += speed * dt;
            }
            else if (verticalInput < 0 && possibleActions[3])
            {
                Position.y -= speed * dt;
            }
            else if (horizontalInput < 0 && possibleActions[4])
            {
                Position.x -= speed * dt;
            }
        }

        this.Position = Position;
        return new PlayerUpdateResult {HasDropBomb = hasDroppedBomb, Position = Position};
    }
}
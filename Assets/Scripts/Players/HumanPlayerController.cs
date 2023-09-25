using UnityEngine;

public class HumanPlayerController : APlayerController
{
    public HumanPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var Position = this.Position;
        
        bool hasDroppedBomb = Input.GetKeyDown(KeyCode.Space);
        
        if (!hasDroppedBomb)
        {
            bool[] possibleActions = copyGame.GetPossibleActionsV2(Position);
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
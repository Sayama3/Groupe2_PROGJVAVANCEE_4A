using UnityEngine;

public class HumanPlayerController : APlayerController
{
    public HumanPlayerController(GameObject prefab)
    {
        PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var position = Position;
        
        bool hasDroppedBomb = Input.GetKeyDown(KeyCode.Space);
        
        if (!hasDroppedBomb)
        {
            bool[] possibleActions = copyGame.GetPossibleActions(position);
            if (verticalInput > 0 && possibleActions[1])
            {
                position.y += speed * dt;
            }
            else if (horizontalInput > 0 && possibleActions[2])
            {
                position.x += speed * dt;
            }
            else if (verticalInput < 0 && possibleActions[3])
            {
                position.y -= speed * dt;
            }
            else if (horizontalInput < 0 && possibleActions[4])
            {
                position.x -= speed * dt;
            }
        }

        Position = position;
        return new PlayerUpdateResult {HasDropBomb = hasDroppedBomb, Position = position};
    }
}
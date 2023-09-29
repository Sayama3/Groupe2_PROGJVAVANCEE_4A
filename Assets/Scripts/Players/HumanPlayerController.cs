using UnityEngine;

public enum HumanPlayerIndex : int
{
	One = 0,
	Two = 1,
	Three = 2,
	Four = 3,
}

public class HumanPlayerController : APlayerController
{
	private HumanPlayerIndex index;
	public HumanPlayerController(GameObject prefab, HumanPlayerIndex playerIndex)
	{
		PrefabSource = prefab;
		index = playerIndex;
	}
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
		float horizontalInput = GetHorizontal();
		float verticalInput = GetVertical();
        float speed = GameManager.Instance.GetCurrentGameParams().Speed;
        var position = Position;

		bool hasDroppedBomb = PressDropBomb();
        
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

	private float GetHorizontal()
	{
		return index switch
		{
            HumanPlayerIndex.One => Input.GetAxisRaw("Horizontal1"),
            HumanPlayerIndex.Two => Input.GetAxisRaw("Horizontal2"),
            HumanPlayerIndex.Three => Input.GetAxisRaw("Horizontal3"),
            HumanPlayerIndex.Four => Input.GetAxisRaw("Horizontal4"),
		};
	}
	private float GetVertical()
	{
		return index switch
		{
            HumanPlayerIndex.One => Input.GetAxisRaw("Vertical1"),
            HumanPlayerIndex.Two => Input.GetAxisRaw("Vertical2"),
            HumanPlayerIndex.Three => Input.GetAxisRaw("Vertical3"),
            HumanPlayerIndex.Four => Input.GetAxisRaw("Vertical4"),
		};
	}

	private bool PressDropBomb()
	{
		return index switch
		{
			HumanPlayerIndex.One => Input.GetButtonDown("Bomb1"),
			HumanPlayerIndex.Two => Input.GetButtonDown("Bomb2"),
			HumanPlayerIndex.Three => Input.GetButtonDown("Bomb3"),
			HumanPlayerIndex.Four => Input.GetButtonDown("Bomb4"),
		};
	}
}
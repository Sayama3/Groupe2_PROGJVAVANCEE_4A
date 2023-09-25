using UnityEngine;



public enum NCTSAction
{
	None = 1 << 0,
	MoveUp = 1 << 1,
	MoveRight = 1 << 2,
	MoveDown = 1 << 3,
	MoveLeft = 1 << 4,
	Bomb = 1 << 5,
}
public struct MCTSPlayerAction
{
	public GameActions GameAction;
	public Vector2 Position;
	public bool HasDropBomb;
}

using System;

[Flags]
public enum MCTSAction
{
	None = 1 << 0,
	MoveUp = 1 << 1,
	MoveRight = 1 << 2,
	MoveDown = 1 << 3,
	MoveLeft = 1 << 4,
	Bomb = 1 << 5,
}
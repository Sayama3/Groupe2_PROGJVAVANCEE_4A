using System;
using System.Collections.Generic;
using UnityEngine;

public static class MCTSHelper
{
	public static int NumberOfTests => GameManager.Instance.GetCurrentGameParams().NumberOfTests;
	public static int NumberOfSimulations => GameManager.Instance.GetCurrentGameParams().NumberOfSimulations;
	public static float PlayerSpeed => GameManager.Instance.GetCurrentGameParams().Speed;
	public static float SimulationDeltaTime => 0.1f;
	public static float ExploreThreshold => 0.8f;
	public static int NumberOfFramePerSimulation => 50;
	public static MCTSAction GenerateAllPossibleRandomPlayerAction(this Game game, Vector2 position)
	{
		MCTSAction actions = MCTSAction.None;

		// Movement Actions
		var directions = game.GetPossibleActions(position);
		if (directions[1]) actions &= MCTSAction.MoveUp;
		if (directions[2]) actions &= MCTSAction.MoveRight;
		if (directions[3]) actions &= MCTSAction.MoveDown;
		if (directions[4]) actions &= MCTSAction.MoveLeft;

		// Bomb Action
		if (game.GetCell(position) == CellStates.None) actions &= MCTSAction.Bomb;

		return actions;
	}

	public static MCTSPlayerAction DoRandomAction(this MCTSAction action, Vector2 position, float dt)
	{
		MCTSAction randomAction = ChooseRandomAction(action);

		return randomAction switch
		{
			MCTSAction.None => new MCTSPlayerAction {GameAction = GameActions.None, Position = position, HasDropBomb = false},
			MCTSAction.MoveUp => new MCTSPlayerAction {GameAction = GameActions.Move, Position = position + GetDirection(MCTSAction.MoveUp) * dt, HasDropBomb = false},
			MCTSAction.MoveRight => new MCTSPlayerAction {GameAction = GameActions.Move, Position = position + GetDirection(MCTSAction.MoveRight) * dt, HasDropBomb = false},
			MCTSAction.MoveDown => new MCTSPlayerAction {GameAction = GameActions.Move, Position = position + GetDirection(MCTSAction.MoveDown) * dt, HasDropBomb = false},
			MCTSAction.MoveLeft => new MCTSPlayerAction {GameAction = GameActions.Move, Position = position + GetDirection(MCTSAction.MoveLeft) * dt, HasDropBomb = false},
			MCTSAction.Bomb => new MCTSPlayerAction {GameAction = GameActions.Bomb, Position = position, HasDropBomb = true},
			_ => throw new ArgumentOutOfRangeException()
		};
	}
	public static MCTSAction ChooseRandomAction(this MCTSAction action)
	{
		List<MCTSAction> actions = new List<MCTSAction>(6);
		if(action.HasFlag(MCTSAction.None)) actions.Add(MCTSAction.None);
		if(action.HasFlag(MCTSAction.MoveUp)) actions.Add(MCTSAction.MoveUp);
		if(action.HasFlag(MCTSAction.MoveRight)) actions.Add(MCTSAction.MoveRight);
		if(action.HasFlag(MCTSAction.MoveDown)) actions.Add(MCTSAction.MoveDown);
		if(action.HasFlag(MCTSAction.MoveLeft)) actions.Add(MCTSAction.MoveLeft);
		if(action.HasFlag(MCTSAction.Bomb)) actions.Add(MCTSAction.Bomb);

		return actions.GetRandom();
	}

	public static Vector2 GetDirection(this MCTSAction action)
	{
		return action switch
		{
			MCTSAction.None => Vector2.zero,
			MCTSAction.MoveUp => Vector2.up,
			MCTSAction.MoveRight => Vector2.right,
			MCTSAction.MoveDown => Vector2.down,
			MCTSAction.MoveLeft => Vector2.left,
			MCTSAction.Bomb => Vector2.zero,
			_ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
		};
	}

	public static PlayerUpdateResult GetPlayerUpdateResult(this MCTSAction action, Vector2 player, float dt)
	{
		PlayerUpdateResult result = new PlayerUpdateResult { Position = player, HasDropBomb = action == MCTSAction.Bomb };

		switch (action)
		{
			case MCTSAction.MoveUp: result.Position += Vector2.up * dt; break; 
			case MCTSAction.MoveRight: result.Position += Vector2.right * dt; break; 
			case MCTSAction.MoveDown: result.Position += Vector2.down * dt; break; 
			case MCTSAction.MoveLeft: result.Position += Vector2.left * dt; break; 
		}

		return result;
	}
}
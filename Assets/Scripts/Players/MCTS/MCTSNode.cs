using System.Collections.Generic;
using UnityEngine;

public class MCTSNode
{
	public MCTSNode(Game copyGame, Vector2[] players, int playerTurn, MCTSPlayerAction playerAction) {
        this.action = playerAction;
	}
	public MCTSNode(Game copyGame, Vector2[] players, int playerTurn, MCTSPlayerAction playerAction, ref MCTSNode parent) {
        this.action = playerAction;
		this.parent = parent;
	}
	
	// Current action, only one.
	public MCTSPlayerAction action;
	public Vector2[] players;
	public int playerTurn;
	public Game game;

	// Tree.
	public MCTSNode parent = null;
	public List<MCTSNode> childrens = new ();

	public void Simulate(int numberOfSimulation, float dt)
	{
		game.UpdatePlayers(new []{new PlayerUpdateResult() {Position = action.Position, HasDropBomb = action.HasDropBomb}});
		game.PositionHasExploded(Game.Round(action.Position));
		var actions = MCTSHelper.GenerateAllPossibleRandomPlayerAction(dt, copyGame, Position);
		MCTSNode[] tests = new MCTSNode[MCTSHelper.NumberOfTests];
		for (int i = 0; i < tests.Length; i++)
		{
			tests[i] = new MCTSNode(new Game(copyGame), MCTSHelper.DoRandomAction(actions, Position, dt));
			tests[i].Simulate(MCTSHelper.NumberOfSimulations, MCTSHelper.SimulationDeltaTime);
		}
        
		//TODO: choose the better one.
		childrens
	}
}
using System.Collections.Generic;
using UnityEngine;

public class MCTSNode
{
	public MCTSNode(Game copyGame, Vector2[] players,  int currentPlayer, int playerTurn, MCTSPlayerAction playerAction) {
        this.action = playerAction;
        this.players = players;
        this.currentPlayer = currentPlayer;
        this.playerTurn = playerTurn;
	}
	public MCTSNode(Game copyGame, Vector2[] players, int currentPlayer, int playerTurn, MCTSPlayerAction playerAction, MCTSNode parent) {
        this.action = playerAction;
        this.players = players;
        this.currentPlayer = currentPlayer;
        this.playerTurn = playerTurn;
		this.parent = parent;
	}
	
	// Current action, only one.
	public MCTSPlayerAction action;
	public Vector2[] players;
	public int currentPlayer;
	public int playerTurn;
	public Game game;
	public Vector2 Position;

	// Tree.
	public MCTSNode parent = null;
	public List<MCTSNode> childrens = new ();

	public void Simulate(int numberOfSimulation, float dt)
	{
		game.UpdatePlayers(new []{new PlayerUpdateResult() {Position = action.Position, HasDropBomb = action.HasDropBomb} as PlayerUpdateResult?});
		bool playerLost = game.PositionHasExploded(action.Position);
		var actions = MCTSHelper.GenerateAllPossibleRandomPlayerAction(dt, game, Position);
		MCTSNode[] tests = new MCTSNode[MCTSHelper.NumberOfTests];
		for (int i = 0; i < tests.Length; i++)
		{
			tests[i] = new MCTSNode(new Game(game), players, currentPlayer, playerTurn+1%players.Length, MCTSHelper.DoRandomAction(actions, Position, dt), this);
			tests[i].Simulate(MCTSHelper.NumberOfSimulations, MCTSHelper.SimulationDeltaTime);
		}
        
		//TODO: choose the better one.
	}
}
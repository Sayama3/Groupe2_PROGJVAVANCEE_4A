using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class MCTSNode
{
	public MCTSNode(Game copyGame, Vector2?[] players,  int currentPlayer, int turnCount = 0) {
		this.game = copyGame;
		this.action = MCTSAction.None;
		this.players = players;
        this.PlayerCount = players.Length;
		this.currentPlayer = currentPlayer;
		this.playerTurn = currentPlayer;
        Assert.IsTrue(players[currentPlayer].HasValue);
        this.turnCount = turnCount;
        childrens = new ();

		Assert.IsTrue(players[playerTurn].HasValue, "players[playerTurn].HasValue");
		Assert.IsTrue(players[currentPlayer].HasValue);

		var actions = game.GenerateAllPossibleRandomPlayerAction(players[currentPlayer].Value);
		if(actions.HasFlag(MCTSAction.None)) childrens.Add((currentPlayer, MCTSAction.None), null);
		if(actions.HasFlag(MCTSAction.Bomb)) childrens.Add((currentPlayer, MCTSAction.Bomb), null);
		if(actions.HasFlag(MCTSAction.MoveUp)) childrens.Add((currentPlayer, MCTSAction.MoveUp), null);
		if(actions.HasFlag(MCTSAction.MoveRight)) childrens.Add((currentPlayer, MCTSAction.MoveRight), null);
		if(actions.HasFlag(MCTSAction.MoveDown)) childrens.Add((currentPlayer, MCTSAction.MoveDown), null);
		if(actions.HasFlag(MCTSAction.MoveLeft)) childrens.Add((currentPlayer, MCTSAction.MoveLeft), null);	
	}

	public MCTSNode(Game copyGame, Vector2?[] players, int currentPlayer, int playerTurn, MCTSAction action, MCTSNode parent, int turnCount) {
		this.game = copyGame;
		this.action = action;
		this.players = players;
        this.PlayerCount = players.Length;
		this.currentPlayer = currentPlayer;
		this.playerTurn = playerTurn;
		this.parent = parent;
        this.turnCount = turnCount;
        childrens = new ();

		Assert.IsTrue(players[playerTurn].HasValue, "players[playerTurn].HasValue");
        Assert.IsTrue(players[currentPlayer].HasValue);

		var resultAction = action.GetPlayerUpdateResult(players[playerTurn].Value, MCTSHelper.SimulationDeltaTime);
		this.players[playerTurn] = resultAction.Position;
		this.game.UpdatePlayer(resultAction);
		this.game.Update(MCTSHelper.SimulationDeltaTime);

		for (int i = 0; i < PlayerCount; i++)
		{
            if(players[i] == null) continue;
			var actions = game.GenerateAllPossibleRandomPlayerAction(players[i].Value);
			if(actions.HasFlag(MCTSAction.None)) childrens.Add((i, MCTSAction.None), null);
			if(actions.HasFlag(MCTSAction.Bomb)) childrens.Add((i, MCTSAction.Bomb), null);
			if(actions.HasFlag(MCTSAction.MoveUp)) childrens.Add((i, MCTSAction.MoveUp), null);
			if(actions.HasFlag(MCTSAction.MoveRight)) childrens.Add((i, MCTSAction.MoveRight), null);
			if(actions.HasFlag(MCTSAction.MoveDown)) childrens.Add((i, MCTSAction.MoveDown), null);
			if(actions.HasFlag(MCTSAction.MoveLeft)) childrens.Add((i, MCTSAction.MoveLeft), null);	
		}
	}
	
	// Current action, only one.
	private MCTSAction action;
	private Vector2?[] players;
	private readonly int PlayerCount;
	private int currentPlayer;
	private int playerTurn;
	private Game game;
	private Vector2 Position;
	private int turnCount;

	// Tree.
	public MCTSNode parent = null;
	public Dictionary<(int, MCTSAction), MCTSNode> childrens = new ();

	// Win / Loose
	public float WinScore {get; private set;}
	public float LooseScore {get; private set;}
	public int ExplorationCount { get; private set; }

	public MCTSNode ExpandAction()
	{
		Assert.IsTrue(IsLeaf(), "IsLeaf()");
		var action = childrens.Where(c => c.Value == null).GetRandom().Key;
		
		return childrens[action] = new MCTSNode(new Game(game), (Vector2?[])players.Clone(), currentPlayer, action.Item1, action.Item2,this, turnCount + 1);
	}

	public void SimulateAction()
	{
		var copyGame = new Game(game);
		Vector2?[] players = (Vector2?[])this.players.Clone();
		int currentPlayerTurn = playerTurn;
		float dt = MCTSHelper.SimulationDeltaTime;
		PlayerUpdateResult?[] results = new PlayerUpdateResult?[PlayerCount];

		bool gameEnd = false;
		while(!gameEnd)
		{
			for (int player = 0; player < PlayerCount; player++)
			{
				
				if(!players[player].HasValue)
				{
					results[player] = null;
					continue;
				}

				var action = copyGame.GenerateAllPossibleRandomPlayerAction(players[player].Value);
				results[player] = action.ChooseRandomAction().GetPlayerUpdateResult(players[player].Value, dt);
				players[player] = results[player].Value.Position;
			}
			copyGame.UpdatePlayers(results);
			copyGame.Update(dt);

			Assert.IsTrue(players[currentPlayer].HasValue);

			for (int j = 0; j < PlayerCount; j++)
			{
				if (!players[j].HasValue) continue;
				bool playerIsDead = copyGame.PositionHasExploded(players[j].Value);
				if (playerIsDead)
				{
					players[j] = null;
					gameEnd |= players.Count(p => !p.HasValue) >= PlayerCount - 1;
				}
			}

			gameEnd |= !players[currentPlayer].HasValue;

		}

		if (!players[currentPlayer].HasValue)
		{
			LooseScore ++;
		}
		else
		{
			WinScore ++;
		}
	}

	public void Backpropagate()
	{
		if (parent == null) return;
		ExplorationCount += 1;
		parent.RecurseAddScore(WinScore, LooseScore);
	}

	private void RecurseAddScore(float win, float loose)
	{
		this.WinScore += win;
		this.LooseScore += loose;
		ExplorationCount += 1;
		this.Score = GetScore();
		if(parent != null) parent.RecurseAddScore(win, loose);
	}

	public float Score { get; private set; }

	private MCTSNode Explore()
	{
		return childrens.Where(c => c.Value != null && c.Value.IsLeaf()).Select(c => c.Value).GetRandom();
	}

	private MCTSNode Exploit()
	{
		var filteredChildren = childrens.Where(c => c.Value != null && c.Value.IsLeaf());
		MCTSNode best = null;
		foreach (var valuePair in filteredChildren)
		{
			if (best == null)
			{
				best = valuePair.Value;
			}
			else
			{
				if (valuePair.Value.GetScore() > best.GetScore())
				{
					best = valuePair.Value;
				}
			}
		}

		return best;
	}

	public float GetScore()
	{
		if (ExplorationCount == 0) return float.NegativeInfinity;

		return (WinScore-LooseScore) / (float) ExplorationCount;
	}

	private bool AnyNewPlayerIsDead()
	{
		return players.Any(p => p != null && game.PositionHasExploded(p.Value));
	}

	private bool?[] GetListDeadPlayer()
	{
		return players.Select(p => p.HasValue ? (bool?)game.PositionHasExploded(p.Value) : (bool?)null).ToArray();
	}

	private bool CurrentPlayerDead()
	{
		return players[currentPlayer] == null || game.PositionHasExploded(players[currentPlayer].Value);
	}

	public bool IsLeaf()
	{
		foreach (var child in childrens)
		{
			if (child.Value == null)
			{
				return true;
			}
		}

		return false;
	}

	public MCTSAction GetBestAction()
	{
		var filteredChildren = childrens.Where(c => c.Value != null && c.Key.Item1 == currentPlayer);
		MCTSNode bestNode = null;
		MCTSAction best = MCTSAction.None;
		
		foreach (var valuePair in filteredChildren)
		{
			if (bestNode == null)
			{
				best = valuePair.Key.Item2;
				bestNode = valuePair.Value;
			}
			else
			{
				if (valuePair.Value.GetScore() > bestNode.GetScore())
				{
					best = valuePair.Key.Item2;
					bestNode = valuePair.Value;
				}
			}
		}

		return best;
	}
}
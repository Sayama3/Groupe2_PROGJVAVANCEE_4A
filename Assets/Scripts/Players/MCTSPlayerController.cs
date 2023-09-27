using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;



public class MCTSPlayerController : APlayerController
{

    public MCTSPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        int playerIndex = PlayerManager.Instance.players.IndexOf(this);
        var actions = MCTSHelper.GenerateAllPossibleRandomPlayerAction(dt, copyGame, Position);
        MCTSNode[] tests = new MCTSNode[MCTSHelper.NumberOfTests];
        for (int i = 0; i < tests.Length; i++)
        {
            tests[i] = new MCTSNode(new Game(copyGame), PlayerManager.Instance.players.Select(p => p.Position).ToArray(), playerIndex, playerIndex, MCTSHelper.DoRandomAction(actions, Position, dt));
            tests[i].Simulate(MCTSHelper.NumberOfSimulations, MCTSHelper.SimulationDeltaTime);
            //TODO: backpropagate.
        }

        //TODO: choose the better one.

        return new PlayerUpdateResult() { HasDropBomb = false, Position = Position };
    }


}
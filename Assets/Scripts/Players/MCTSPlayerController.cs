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

    private MCTSNode Select(ref List<MCTSNode> nodes)
    {
        
        float value = UnityEngine.Random.value;
        if (value > MCTSHelper.ExploreThreshold)
        {
            return Exploit(ref nodes);
        }
        else
        {
            return Explore(ref nodes);
        }
    }
    
    
    private MCTSNode Explore(ref List<MCTSNode> nodes)
    {
        return nodes.GetRandom();
    }

    private MCTSNode Exploit(ref List<MCTSNode> nodes)
    {
        int best = 0;

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].GetScore() > nodes[best].GetScore())
            {
                best = i;
            }
        }

        return nodes[best];
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        int playerIndex = PlayerManager.Instance.players.IndexOf(this);

        // var actions = MCTSHelper.GenerateAllPossibleRandomPlayerAction(dt, copyGame, Position);
        // MCTSNode[] tests = new MCTSNode[MCTSHelper.NumberOfTests];
        // for (int i = 0; i < tests.Length; i++)
        // {
        //     tests[i] = new MCTSNode(new Game(copyGame), PlayerManager.Instance.players.Select(p => (Vector2?)p.Position).ToArray(), playerIndex, playerIndex, actions);
        //     tests[i].Simulate(MCTSHelper.NumberOfSimulations, MCTSHelper.SimulationDeltaTime);
        //     //TODO: backpropagate.
        // }
        //
        // //TODO: choose the better one.
        //
        // return new PlayerUpdateResult() { HasDropBomb = false, Position = Position };
        var rootNode = new MCTSNode(new Game(copyGame), PlayerManager.Instance.players.Select(p => p != null ? (Vector2?)p.Position : (Vector2?)null).ToArray(), playerIndex);
        List<MCTSNode> leafs = new List<MCTSNode>(4096) {rootNode};
        
        for (int i = 0; i < MCTSHelper.NumberOfTests; i++)
        {
            var selected = Select(ref leafs);
            var newNode = selected.ExpandAction();
            newNode.SimulateAction();
            newNode.Backpropagate();
            CheckNodes(ref leafs, ref newNode);
        }

        return rootNode.GetBestAction().GetPlayerUpdateResult(Position, dt);
    }

    private void CheckNodes(ref List<MCTSNode> leafs, ref MCTSNode newNode)
    {
        if (newNode.IsLeaf())
        {
            leafs.Add(newNode);
        }
        else
        {
            ref MCTSNode parent = ref newNode.parent;
            while (!parent.IsLeaf())
            {
                leafs.Remove(parent);
                if (parent.parent == null) break;
                parent = parent.parent;
            }
        }
    }
}
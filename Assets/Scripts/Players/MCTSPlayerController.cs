using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;



public class MCTSPlayerController : APlayerController
{

    public MCTSPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }

    private float researchValue;

    private MCTSNode Select(ref List<MCTSNode> nodes)
    {
        Assert.IsTrue(nodes.TrueForAll(l => l.IsLeaf()), "leafs.TrueForAll(l => l.IsLeaf())");
        float value = UnityEngine.Random.value;
        if (value > ((MCTSHelper.ExploreMaxThreshold-MCTSHelper.ExploreMinThreshold) * researchValue) + MCTSHelper.ExploreMinThreshold)
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

        var rootNode = new MCTSNode(new Game(copyGame), PlayerManager.Instance.players.Select(p => p != null ? (Vector2?)p.Position : (Vector2?)null).ToArray(), playerIndex);
        Assert.IsTrue(rootNode.IsLeaf(), "rootNode.IsLeaf()");
        List<MCTSNode> leafs = new List<MCTSNode>(MCTSHelper.NumberOfTests + 1) {rootNode};
        
        for (int i = 0; i < MCTSHelper.NumberOfTests; i++)
        {
            researchValue = Mathf.Max((MCTSHelper.NumberOfTests - i) / (float)MCTSHelper.NumberOfTests, 0);
            var selected = Select(ref leafs);
            Assert.IsTrue(selected.IsLeaf());
            var newNode = selected.ExpandAction();
            for (int j = 0; j < MCTSHelper.NumberOfSimulations; j++)
            {
                newNode.SimulateAction();
            }
            newNode.Backpropagate();
            CheckNodes(ref leafs, ref newNode);
        }

        var ac = rootNode.GetBestAction().GetPlayerUpdateResult(Position, dt);
        Position = Vector2.MoveTowards(Position, ac.Position, GameManager.Instance.GetCurrentGameParams().Speed * dt);
        ac.Position = Position;
        return ac;
    }

    private void CheckNodes(ref List<MCTSNode> leafs, ref MCTSNode newNode)
    {
        if (newNode.IsLeaf())
        {
            leafs.Add(newNode);
        }

        ref MCTSNode parent = ref newNode.parent;
        if (!parent.IsLeaf())
        {
            leafs.Remove(parent);
        }

        Assert.IsTrue(leafs.TrueForAll(l => l.IsLeaf()), "leafs.TrueForAll(l => l.IsLeaf())");
    }
}
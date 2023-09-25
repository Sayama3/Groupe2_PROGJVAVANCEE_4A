using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MCTSPlayerController : APlayerController
{
    private const int numberOfTests = 4;
    private const int numberOfSimulations = 8;

    public MCTSPlayerController(GameObject prefab)
    {
        this.PrefabSource = prefab;
    }
    
    public override PlayerUpdateResult Update(float dt, Game copyGame)
    {
        return new PlayerUpdateResult() { HasDropBomb = false, Position = Position };
    }

}
﻿using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "GameParameters", menuName = "ScriptableObjects/GameParameters")]
[HideMonoScript]
public class ScriptableGameParameters : SerializedScriptableObject, IGameParameters
{
	[OdinSerialize, ShowInInspector, InlineProperty, HideLabel]
	private GameParameters _defaultParameters = new GameParameters(15, 15);

	public int Width => _defaultParameters.Width;
	public int Height => _defaultParameters.Height;
	public float Speed => _defaultParameters.Speed;
	public int BombRadius => _defaultParameters.BombRadius;
	public float BombTimer => _defaultParameters.BombTimer;
	public float BombExplosionTimer => _defaultParameters.BombExplosionTimer;
	public int NumberOfTests => _defaultParameters.NumberOfTests;
	public float SimulationDeltaTime => _defaultParameters.SimulationDeltaTime;

	public float ExploreMaxThreshold => _defaultParameters.ExploreMaxThreshold;
	public float ExploreMinThreshold => _defaultParameters.ExploreMinThreshold;

	public int NumberOfSimulations => _defaultParameters.NumberOfSimulations;
}
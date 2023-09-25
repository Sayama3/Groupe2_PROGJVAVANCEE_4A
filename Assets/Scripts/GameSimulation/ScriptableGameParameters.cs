using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "GameParameters", menuName = "ScriptableObjects/GameParameters")]
[HideMonoScript]
public class ScriptableGameParameters : SerializedScriptableObject, IGameParameters
{
	[SerializeField, InlineProperty, HideLabel]
	private GameParameters _defaultParameters = new GameParameters(1, 1);

	public GameParameters GetParameters()
	{
		return _defaultParameters;
	}

	public int Width => _defaultParameters.Width;

	public int Height => _defaultParameters.Height;
	public float Speed => _defaultParameters.Speed;
	public int BombRadius => _defaultParameters.BombRadius;

	public float BombTimer => _defaultParameters.BombTimer;
}
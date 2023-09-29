using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public struct GameParameters: IGameParameters
{
	public GameParameters(int width, int height)
	{
		Width = width;
		Height = height;
		Speed = 1;
		BombRadius = 2;
		BombTimer = 1.5f;
		BombExplosionTimer = 1.5f;
		NumberOfTests = 4;
		SimulationDeltaTime = 0.1f;
		ExploreMinThreshold = 0.05f;
		ExploreMaxThreshold = 0.5f;
		NumberOfSimulations = 50;
	}
	[OdinSerialize, ShowInInspector, MinValue(1), HorizontalGroup("GameSize")]
	public int Width {get; set;}

	[OdinSerialize, ShowInInspector, MinValue(1), HorizontalGroup("GameSize")]
	public int Height {get; set;}
	
	[OdinSerialize, ShowInInspector]
	public float Speed {get; set;}

	[OdinSerialize, ShowInInspector]
    public int BombRadius { get; set; }

	[OdinSerialize, ShowInInspector]
    public float BombTimer { get; set; }

	[OdinSerialize, ShowInInspector]
    public float BombExplosionTimer { get; set; }

	[OdinSerialize, ShowInInspector]
	public int NumberOfTests { get; set; }

    [OdinSerialize, ShowInInspector]
	public float SimulationDeltaTime { get; set; }

    [OdinSerialize, ShowInInspector, PropertyRange(0,1)]
	public float ExploreMaxThreshold { get; set; }

    [OdinSerialize, ShowInInspector, PropertyRange(0,1)]
	public float ExploreMinThreshold { get; set; }

    [OdinSerialize, ShowInInspector]
	public int NumberOfSimulations { get; set; }
}
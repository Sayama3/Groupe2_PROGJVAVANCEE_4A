using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public struct GameParameters: IGameParameters
{
	public GameParameters(int width, int height)
	{
		this.Width = width;
		this.Height = height;
		this.Speed = 1;
		this.BombRadius = 2;
		this.BombTimer = 1.5f;
		this.BombExplosionTimer = 1.5f;
		this.NumberOfTests = 4;
		this.SimulationDeltaTime = 0.1f;
		this.ExploreMinThreshold = 0.05f;
		this.ExploreMaxThreshold = 0.5f;
		this.NumberOfSimulations = 50;
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
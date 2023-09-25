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
}
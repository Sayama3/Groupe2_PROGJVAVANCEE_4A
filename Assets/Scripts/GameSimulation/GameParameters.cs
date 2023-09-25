using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public struct GameParameters: IGameParameters
{
	public GameParameters(int width, int height, float speed)
	{
		this.Width = width;
		this.Height = height;
		this.Speed = speed;
	}
	
	[OdinSerialize, ShowInInspector, MinValue(1), HorizontalGroup("GameSize")]
	public int Width {get; set;}

	[OdinSerialize, ShowInInspector, MinValue(1), HorizontalGroup("GameSize")]
	public int Height {get; set;}
	
	[OdinSerialize, ShowInInspector]
	public float Speed {get; set;}
}
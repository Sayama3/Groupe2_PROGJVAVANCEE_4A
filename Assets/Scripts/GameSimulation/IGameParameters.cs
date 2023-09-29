﻿public interface IGameParameters
{
	public int Width { get; }
	public int Height { get; }
	public float Speed { get; }
	public int BombRadius { get; }
	public float BombTimer { get; }
	public float BombExplosionTimer { get; }
	public int NumberOfTests { get; }
	public float SimulationDeltaTime { get; }
	public float ExploreThreshold { get; }
	public int NumberOfFramePerSimulation { get; }
}
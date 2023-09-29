using UnityEngine;

public interface IPlayerController
{
    public PlayerUpdateResult Update(float dt, Game copyGame);
    public int Id { get; set; }
    public Vector2 LastPosition { get; set; }
    public Vector2 Position { get; set; }
    public GameObject PrefabSource { get; }
}
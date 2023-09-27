using UnityEngine;

public interface IPlayerController
{
    public PlayerUpdateResult Update(float dt, Game copyGame);
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Quaternion Rotation { get; }
    public GameObject PrefabSource { get; }
}
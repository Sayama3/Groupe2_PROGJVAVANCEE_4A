using UnityEngine;

public interface IPlayerController
{
    public PlayerUpdateResult Update(float dt, Game copyGame);
    public Vector2 Position { get; set; }
    public Quaternion Rotation { get; }
    public GameObject PrefabSource { get; }
}
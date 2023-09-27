using UnityEngine;

public abstract class APlayerController : IPlayerController
{
    public abstract PlayerUpdateResult Update(float dt, Game copyGame);
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Quaternion Rotation { get; protected set; }
    public GameObject PrefabSource { get; protected set; }
    
}
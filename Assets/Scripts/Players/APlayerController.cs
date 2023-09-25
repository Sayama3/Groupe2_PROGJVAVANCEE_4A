using UnityEngine;

public abstract class APlayerController : IPlayerController
{

    public abstract PlayerUpdateResult Update(float dt, Game copyGame);
    public Vector3 Position { get; protected set; }
    public Quaternion Rotation { get; protected set; }
    public GameObject PrefabSource { get; protected set; }
    
}
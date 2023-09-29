using UnityEngine;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Init();
        GameManager.StartGame();
    }
}

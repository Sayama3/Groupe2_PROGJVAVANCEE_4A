using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToGame : MonoBehaviour
{
    public PlayerType[] playerTypes = new PlayerType[4];
    
    #region Singleton

    private static MenuToGame instance;

    private static MenuToGame GetInstance(bool createIfNull)
    {
        if (instance == null)
        {
            instance = FindObjectOfType<MenuToGame>();
            if (createIfNull && instance == null)
            {
                var obj = new GameObject(nameof(MenuToGame), typeof(MenuToGame));
                instance = obj.GetComponent<MenuToGame>();
            }
        }
        return instance;
    }
    public static MenuToGame Instance => GetInstance(false);
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
        instance = this;
    }

    #endregion

    private void Start()
    {
        SceneManager.sceneLoaded += ResetPlayers;
    }

    private void ResetPlayers(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainMenu") return;
        for(int i = 0; i < playerTypes.Length; i++)
        {
            playerTypes[i] = PlayerType.None;
        }
    }

    
}
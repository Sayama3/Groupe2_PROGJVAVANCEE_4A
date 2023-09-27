using UnityEngine;

public class MenuToGame : MonoBehaviour
{
    public PlayerType[] playerTypes = new PlayerType[4];
    
    #region Singleton

    private static MenuToGame instance;

    private static MenuToGame GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<MenuToGame>();
            if (instance == null)
            {
                var obj = new GameObject(nameof(MenuToGame), typeof(MenuToGame));
                instance = obj.GetComponent<MenuToGame>();
            }
        }
        return instance;
    }
    public static MenuToGame Instance => GetInstance();
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
}
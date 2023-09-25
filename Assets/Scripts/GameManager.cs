using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class GameManager : SerializedMonoBehaviour
{
    #region Singleton

    private static GameManager instance;

    private static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();
            if (instance == null)
            {
                var obj = new GameObject(nameof(GameManager), typeof(GameManager));
                instance = obj.GetComponent<GameManager>();
            }
        }
        return instance;
    }
    public static GameManager Instance => GetInstance();
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        InitSelf();
    }

    #endregion

    [SerializeField]
    private ScriptableGameParameters gameparameters;
    [ShowInInspector, ReadOnly]
    private Game game;

    [Button]
    private void InitSelf()
    {
        game = new Game(gameparameters);
        game.Fill(CellStates.Wall);
        game.FillRandom();
    }

    public Game GetCurrentGame()
    {
        return game;
    }
    public ScriptableGameParameters GetCurrentGameParams()
    {
        return gameparameters;
    }
}

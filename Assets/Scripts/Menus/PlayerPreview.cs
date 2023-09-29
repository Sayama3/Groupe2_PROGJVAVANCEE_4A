using System;
using TMPro;
using UnityEngine;

public class PlayerPreview : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    
    [SerializeField] private GameObject[] players;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { ShowPlayer();});
    }

    public void ShowPlayer()
    {
        foreach (GameObject gameObject in players)
        {
            gameObject.SetActive(false);
        }

        players[dropdown.value].SetActive(true);
        
        MenuToGame.Instance.playerTypes[playerIndex] = (PlayerType)dropdown.value;
    }
}
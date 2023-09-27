using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    [SerializeField, Required] private GameObject none;
    [SerializeField, Required] private GameObject explodedCell;
    [SerializeField, Required] private GameObject bomb;
    [SerializeField, Required] private GameObject wall;
    [SerializeField, ReadOnly] private CellStates cell;
    [SerializeField, ReadOnly] private bool exploded;

    private void Start()
    {
        DisableAll();
    }

    public void SetCell(CellStates cellState, bool exploded = false)
    {
        this.cell = cellState;
        this.exploded = exploded;
        DisableAll();
        switch (cellState)
        {
            case CellStates.None: 
            {
                if(exploded) explodedCell.SetActive(true);
                else none.SetActive(true);
                break;
            }
            case CellStates.Bomb: 
            {
                bomb.SetActive(true);
                break;
            }
            case CellStates.Wall: 
            {
                wall.SetActive(true);
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(cellState), cellState, null);
        }
    }

    private void DisableAll()
    {
        none.SetActive(false);
        explodedCell.SetActive(false);
        bomb.SetActive(false);
        wall.SetActive(false);
    }
}

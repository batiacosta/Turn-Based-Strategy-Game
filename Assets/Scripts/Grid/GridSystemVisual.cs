using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance;
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
    private int _height;
    private int _width;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _height = LevelGrid.Instance.GetHeight();
        _width = LevelGrid.Instance.GetWidth();
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[_width,_height];
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSistemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                _gridSystemVisualSingleArray[x, z] =
                    gridSistemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        HideAllGridPositions();
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                _gridSystemVisualSingleArray[x,z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        HideAllGridPositions();
        ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
    }
}

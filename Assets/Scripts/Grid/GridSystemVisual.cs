using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance;
    
    
    public enum GridVisualType
    {
        White, Blue, Red, Yellow, RedSoft
    }
    [Serializable] public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

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

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
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

    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial((gridVisualType)));
        }
    }

    private void UpdateGridVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        HideAllGridPositions();
        GridVisualType gridVisualType;
        switch (selectedAction)
        {
            case MoveAction moveAction: 
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction: 
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction: 
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(unit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            default: 
                gridVisualType = GridVisualType.White;
                break;
        }
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        
        Debug.Log($"Not found GridVisualTypeMaterial for GridVisualType {gridVisualType}");
        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private Vector3 _targetPosition;
    private Unit _unit;
    

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        Transform currentTransform = transform;
        Vector3 position = currentTransform.position;
        var deltaPositionTarget = Vector3.Distance(position, _targetPosition);
        var stoppingDistance = 0.1f;
        
        if (deltaPositionTarget > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - position).normalized;
            float moveSpeed = 4f;
            float rotationSpeed = 10f;
            transform.forward = Vector3.Lerp(currentTransform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            unitAnimator.SetBool(IsWalking, false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    
}

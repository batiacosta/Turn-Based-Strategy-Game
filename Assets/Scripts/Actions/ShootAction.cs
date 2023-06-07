using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootAction : BaseAction
{
    
    [SerializeField] private int maxShootDistance = 7;
    
    private enum State
    {
        Aiming,
        Shooting,
        Coolff
    }
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;

    private void Start()
    {
        _stateTimer = 3;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        switch (_state)
        {
            case State.Aiming:
                float rotationSpeed = 2f;
                Transform currentTransform = transform;
                transform.forward = Vector3.Lerp(currentTransform.forward, _targetUnit.transform.position, Time.deltaTime * rotationSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.Coolff:
                break;
        }

        _stateTimer -= Time.deltaTime;

        if (_stateTimer <= 0)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        _targetUnit.Damage();
    }

    private void NextState()
    {
        float stateTimer;
        
        switch (_state)
        {
            case State.Aiming: 
                _state = State.Shooting;
                stateTimer = 0.1f;
                _stateTimer = stateTimer;
                break;
            case State.Shooting: 
                _state = State.Coolff;
                stateTimer = 0.5f;
                _stateTimer = stateTimer;
                break;
            case State.Coolff:
                ActionComplete();
                break;
        } ;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _canShootBullet = true;
        _state = State.Aiming;
        var stateTimer = 1f;
        _stateTimer = stateTimer;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {   //  No units are on this grid position
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == _unit.IsEnemy())
                {// Continue if they are on the same team
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
                
            }
        }
        return validGridPositionList;
    }
}

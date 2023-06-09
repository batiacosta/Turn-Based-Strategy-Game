using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    private const int MaxActionPointsMax = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;
    
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private HealthSystem _healthSystem;
    private int _actionPoints = MaxActionPointsMax;
    
    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (_gridPosition != newGridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
            
        }
    }

    public MoveAction GetMoveAction() => _moveAction;
    public SpinAction GetSpinAction() => _spinAction;
    public GridPosition GetGridPosition() => _gridPosition;
    public BaseAction[] GetBaseActionArray() => _baseActionArray;

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs e)
    {
        bool isEnemyTurn = IsEnemy() && !TurnSystem.Instance.IsPlayerTurn();
        bool isPlayerTurn = !IsEnemy() && TurnSystem.Instance.IsPlayerTurn();
        if (isEnemyTurn || isPlayerTurn)
        {
            _actionPoints = MaxActionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    

    public int GetActionPoints()
    {
        return _actionPoints;
    }

    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}

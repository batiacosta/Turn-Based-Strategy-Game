using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private Vector3 _targetPosition;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private GridPosition _gridPosition;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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
        
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (_gridPosition != newGridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}

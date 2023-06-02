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

    private void Update()
    {
        Vector3 position = transform.position;
        var deltaPositionTarget = Vector3.Distance(position, _targetPosition);
        var stoppingDistance = 0.1f;
        
        if (deltaPositionTarget > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            unitAnimator.SetBool(IsWalking, false);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        var delta = Vector3.Distance(transform.position, _targetPosition);
        if (delta < 0.5f)
        {
            
        }
        _targetPosition = targetPosition;
    }
}

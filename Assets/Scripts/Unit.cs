using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;
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
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
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

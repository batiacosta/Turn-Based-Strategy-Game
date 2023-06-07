using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
    private Vector3 _targetPosition;
    private void Update()
    {
        if (_targetPosition == null)
        {
            return;
        }

        var position = transform.position;
        float distanceBeforeMoving = Vector3.Distance(position, _targetPosition);
        float moveSpeed = 200f;
        Vector3 moveDir = (_targetPosition - position).normalized;
        transform.position += moveDir * Time.deltaTime* moveSpeed;
        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            trailRenderer.transform.parent = null;
            Instantiate(bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private const float MinFollowOffset = 2f;
    private const float MaxFollowOffset = 12f;
    private Vector3 _targetFollowOffset;
    private CinemachineTransposer _cinemachineTransposer;

    private void Start()
    {
        _cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleZoom();
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y += zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y -= zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowOffset, MaxFollowOffset);
        float zoomSpeed = 5f;
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
            Time.deltaTime * zoomSpeed);
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        var rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        var movementSpeed = 5f;
        Transform controllerTransform = transform;
        Vector3 moveVector = controllerTransform.forward * inputMoveDirection.z +
                             controllerTransform.right * inputMoveDirection.x;
        transform.position += moveVector * movementSpeed * Time.deltaTime;
    }
}

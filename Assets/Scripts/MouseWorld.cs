using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld _instance;
    
    [SerializeField] private LayerMask mousePlayerMask;

    private void Awake()
    {
        _instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _instance.mousePlayerMask);
        return raycastHit.point;
    }
}

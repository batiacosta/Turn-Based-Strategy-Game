using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro gridText;
    
    private object _gridObject;
    
    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }

    protected virtual void Update()
    {
        gridText.text = _gridObject.ToString();
    }
}

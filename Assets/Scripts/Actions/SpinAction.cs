using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    

    private void Start()
    {
        _totalSpinAmount = 0;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        float spinAddAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360)
        {
            _isActive = false;
            _OnActionComplete();
        }
    }

    public void Spin(Action onActionComplete )
    {
        _OnActionComplete = onActionComplete;
        _totalSpinAmount = 0f;
        _isActive = true;
    }
}

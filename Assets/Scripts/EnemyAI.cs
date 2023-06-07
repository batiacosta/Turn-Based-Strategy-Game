using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;

    private void Start()
    {
        _timer = 2;
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs e)
    {
        _timer = 2;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0 )
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}

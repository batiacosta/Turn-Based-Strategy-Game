using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;

    public event EventHandler OnTurnNumberChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private int _turnNumber = 1;

    public void NextTurn()
    {
        _turnNumber++;
        OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() => _turnNumber;

}

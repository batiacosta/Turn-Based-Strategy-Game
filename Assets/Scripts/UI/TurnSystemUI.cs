using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTurnNumber;
    [SerializeField] private Button endTurnButton;

    private void Start()
    {
        endTurnButton.onClick.AddListener(NextTurn);
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
        UpdateTurnNumberVisual(TurnSystem.Instance.GetTurnNumber());
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs e)
    {
        int turnNumber = TurnSystem.Instance.GetTurnNumber();
        UpdateTurnNumberVisual(turnNumber);
    }

    private void UpdateTurnNumberVisual(int turnNumber)
    {
        textTurnNumber.text = $"TURN {turnNumber}";
    }

    private void NextTurn()
    {
        TurnSystem.Instance.NextTurn();
    }
}

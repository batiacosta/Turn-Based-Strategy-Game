using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textButton;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction _baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        _baseAction = baseAction;
        textButton.text = _baseAction.GetActinName().ToUpper();
        button.onClick.AddListener(() => { 
            UnitActionSystem.Instance.SetSelectedAction(_baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedBaseAction == _baseAction)
        {
            selectedGameObject.gameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.gameObject.SetActive(false);
        }
    }
    

}

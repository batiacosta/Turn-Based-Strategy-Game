
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        UpdateVisuals(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        UpdateVisuals(isBusy);
    }

    private void UpdateVisuals(bool isBusy)
    {
        gameObject.gameObject.SetActive(isBusy);
    }
}

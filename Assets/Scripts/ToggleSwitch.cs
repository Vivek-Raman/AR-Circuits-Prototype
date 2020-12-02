using System;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private ManageLightState bulb = null;

    private const float TIME = 1.5f;
    private bool isSwitchClosed = false;

    private void Awake()
    {
        OpenSwitch();
    }

    private void OnEnable()
    {
        TouchInput.Instance.tapAction += OnTap;
    }

    private void OnDisable()
    {
        TouchInput.Instance.tapAction -= OnTap;
    }

    public void OnTap()
    {
        if (isSwitchClosed)
        {
            OpenSwitch();
        }
        else
        {
            CloseSwitch();
        }
        isSwitchClosed = !isSwitchClosed;
    }
    private void OnTap(RaycastHit res)
    {
        OnTap();
    }

    private void OpenSwitch()
    {
        LeanTween.rotateY(this.gameObject, 150f, TIME).setEaseSpring();
        Invoke(nameof(ManageBulb), 0.2f);
    }

    private void CloseSwitch()
    {
        LeanTween.rotateY(this.gameObject, 90f, TIME).setEaseSpring();
        Invoke(nameof(ManageBulb), TIME - 0.2f);
    }

    private void ManageBulb()
    {
        bulb.EvaluateState(isSwitchClosed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Display : MonoBehaviour
{
    protected bool _isShowing;

    protected virtual void Awake()
    {
        _isShowing = transform.GetChild(0).gameObject.activeSelf;
    }

    public bool IsShowing
    {
        get { return _isShowing; }
    }

    public virtual void Show()
    {
        if (IsShowing)
            return;
        _isShowing = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        if (!IsShowing)
            return;
        _isShowing = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ToggleVisibility()
    {

        if (IsShowing)
        {
            Hide();
            return;
        }
        Show();
    }
}

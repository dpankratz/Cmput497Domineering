﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaysManager : MonoBehaviour
{
    [SerializeField] private Display _defaultDisplay;
    [SerializeField] internal GameDisplay GameDisplay;
    [SerializeField] internal SplashScreenDisplay SplashDisplay;
    [SerializeField] internal SettingsDisplay SettingsDisplay;


    public static DisplaysManager instance;

    public Display CurrentDisplay
    {
        get { return _currentDisplay; }
    }

    private Display _currentDisplay;

    private void Awake()
    {
        instance = this;
        _currentDisplay = _defaultDisplay;
    }

    public void ShowDisplay(Display display)
    {
        _currentDisplay.Hide();
        display.Show();
        _currentDisplay = display;
    }
}

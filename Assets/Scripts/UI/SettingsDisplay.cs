using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDisplay : Display
{

    [SerializeField] private Dropdown _playerOneDropdown;
    [SerializeField] private Dropdown _playerTwoDropdown;

    public void MainMenu()
    {
        DisplaysManager.instance.ShowDisplay(DisplaysManager.instance.SplashDisplay);
    }

    public void PlayerOneSelection(Int32 choice)
    {
        Settings.AgentOne = (AgentType) choice;
    }

    public void PlayerTwoSelection(Int32 choice)
    {        
        Settings.AgentTwo = (AgentType)choice;
    }

    public override void Show()
    {
        base.Show();
        _playerOneDropdown.value = (int)Settings.AgentOne;
        _playerTwoDropdown.value = (int)Settings.AgentTwo;
    }

    public override void Hide()
    {
        base.Hide();
        SerializationManager.instance.Serialize();
    }
}


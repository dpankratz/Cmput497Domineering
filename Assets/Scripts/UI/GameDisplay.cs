using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDisplay : Display
{
    [SerializeField] private Text[] _playerOneName;
    [SerializeField] private Text[] _playerTwoName;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameManager _gameManager;

    public override void Show()
    {
        base.Show();
        foreach (var text in _playerOneName)
            text.text = Enum.GetName(typeof(AgentType), _gameManager.AgentOne.Type);
        foreach (var text in _playerTwoName)
            text.text = Enum.GetName(typeof(AgentType), _gameManager.AgentTwo.Type);
        _mainCamera.enabled = true;
    }

    public void MainMenu()
    {
        DisplaysManager.instance.ShowDisplay(DisplaysManager.instance.SplashDisplay);
        _mainCamera.enabled = false;
    }
}

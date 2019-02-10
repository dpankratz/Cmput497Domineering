using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenDisplay : Display
{

    [SerializeField] private Agent[] _zeroPlayer;
    [SerializeField] private Agent[] _onePlayer;
    [SerializeField] private Agent[] _twoPlayer;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Camera _rotatingCamera;
    [SerializeField] private Camera _mainCamera;

    public void ZeroPlayer()
    {
        Debug.Log("Zero player");
        _gameManager.ResetGameWithNewAgents(_zeroPlayer);
        Hide();
    }

    public void OnePlayer()
    {
        Debug.Log("One player");
        _gameManager.ResetGameWithNewAgents(_onePlayer);
        Hide();
    }

    public void TwoPlayer()
    {
        Debug.Log("Two player");
        _gameManager.ResetGameWithNewAgents(_twoPlayer);
        Hide();
    }

    public override void Show()
    {
        _rotatingCamera.enabled = true;
        _mainCamera.enabled = false;
        base.Show();
    }

    public override void Hide()
    {
        _rotatingCamera.enabled = false;
        _mainCamera.enabled = true;
        base.Hide();
    }
}

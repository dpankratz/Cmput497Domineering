using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{

    [SerializeField] private SplashScreenDisplay _splashScreenDisplay;        
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Agent[] _randomAgents;

    void Start()
    {
        ShowSplashScreen();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            ShowSplashScreen();
            
    }


    private void ShowSplashScreen()
    {
        _splashScreenDisplay.Show();
        _gameManager.ResetGameWithNewAgents(_randomAgents);
    }

}

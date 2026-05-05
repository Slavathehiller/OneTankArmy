using Assets.Scripts.SceneNavigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Portal : MonoBehaviour
{
    [SerializeField] 
    private string _sceneToLoadName;

    [SerializeField]
    private string _startPointName;

    [Inject]
    private ISceneNavigator _sceneNavigator;
    public void LoadNextScene()
    {
        _sceneNavigator.GoingFromAnotherScene = true;
        _sceneNavigator.StartPointName = _startPointName;
        SceneManager.LoadScene(_sceneToLoadName);
    }
}

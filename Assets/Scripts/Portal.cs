using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string _sceneToLoadName;
    public void LoadNextScene()
    {
        SceneManager.LoadScene(_sceneToLoadName);
    }
}

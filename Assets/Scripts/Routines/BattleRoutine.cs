using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

public class BattleRoutine : MonoBehaviour
{
    [Inject]
    ISceneAssetFactory _sceneAssetFactory;
    void Start()
    {
       // var flea = _sceneAssetFactory.CreateAsset<BoomFlea>();
       // flea.transform.position = new Vector3(-4, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
            Application.Quit();
    }

}

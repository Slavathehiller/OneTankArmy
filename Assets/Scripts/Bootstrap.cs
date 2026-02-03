using Assets.Scripts.Factories;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        PrefabsPath.InitPathes();
        SceneManager.LoadScene(Scenes.BATTLE_SCENE);
    }
}

using Assets.Scripts.Factories;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        PrefabsPath.InitPathes();
        RegisterGlobalConverters.Register();
        SceneManager.LoadScene(Scenes.MAIN_MENU);
    }
}

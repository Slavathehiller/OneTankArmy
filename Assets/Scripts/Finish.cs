using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private GameObject _finishWindow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<TankController>(out _))
            _finishWindow.SetActive(true);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(Scenes.BATTLE_SCENE);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

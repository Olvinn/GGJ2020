using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Widget _loading;

    public void StartGame()
    {
        _loading.Show();
        Invoke("LoadGame", 1);
    }

    public void ExitGame()
    {
        _loading.Show();
        Invoke("Quit", 1);
    }

    void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    void Quit()
    {
        Application.Quit();
    }
}

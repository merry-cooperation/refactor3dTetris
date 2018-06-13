using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quite()
    {
        Application.Quit();
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject scores;
    GameObject menu;
    GameObject pause_menu;

    private void Awake()
    {
        scores = transform.GetChild(2).gameObject;
        menu = transform.GetChild(1).gameObject;
        pause_menu = transform.GetChild(0).gameObject;
    }

    public void Menu()
    {
        scores.SetActive(false);
        pause_menu.SetActive(false);
        menu.SetActive(true);
    }

    public void Quite()
    {
        Application.Quit();
    }

}

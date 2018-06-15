using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // this script should be attached to Canvas
    private GameObject scores;
    private GameObject menu;
    private GameObject pause_menu;
    private GameObject scoreText;
    private void Awake()
    {
        scores = transform.GetChild(2).gameObject;
        menu = transform.GetChild(1).gameObject;
        pause_menu = transform.GetChild(0).gameObject;
        scoreText = transform.GetChild(3).gameObject;
    }

    public void Menu()
    {
        Camera.main.transform.Translate(Vector3.right * 100);
        scoreText.SetActive(false);
        scores.SetActive(false);
        pause_menu.SetActive(false);
        menu.SetActive(true);
    }

    public void Quite()
    {
        Application.Quit();
    }

}

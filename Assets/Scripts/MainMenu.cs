using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    GameObject scores;
    GameObject menu;

    private void Awake()
    {
        scores = transform.GetChild(0).gameObject;
        menu = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            scores.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quite()
    {
        Application.Quit();
    }

    public void HightScores()
    {
        menu.SetActive(false);
        scores.SetActive(true);
    }
}

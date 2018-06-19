using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public string filename = "Scores.txt";
    GameObject scores;
    GameObject menu;
    public Text first;
    public Text second;
    public Text third;
    public Text fourth;
    public Text fifth;

    public Text first_s;
    public Text second_s;
    public Text third_s;
    public Text fourth_s;
    public Text fifth_s;

    string[] table = new string[10];

    private void Awake()
    {
        scores = transform.GetChild(1).gameObject;
        menu = transform.GetChild(0).gameObject;
        Update_score();
        Time.timeScale = 0;
    }

    void Update_score()
    {
        table = File.ReadAllLines(filename);

        first.text = "" + table[0];
        second.text = "" + table[1];
        third.text = "" + table[2];
        fourth.text = "" + table[3];
        fifth.text = "" + table[4];

        first_s.text = "" + table[5];
        second_s.text = "" + table[6];
        third_s.text = "" + table[7];
        fourth_s.text = "" + table[8];
        fifth_s.text = "" + table[9];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && scores.activeSelf == true)
        {
            scores.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void StartGame()
    {
        scores.SetActive(false);
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Quite()
    {
        Application.Quit();
    }

    public void HightScores()
    {
        menu.SetActive(false);
        scores.SetActive(true);
        Update_score();
    }
}

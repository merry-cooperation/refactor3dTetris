using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameLostScript : MonoBehaviour
{
    public string filename = "Scores.txt";
    string[] table = new string[10];
    GameObject scores;
    GameObject pause_menu;
    GameObject name_request;
    List<Text> names_scores = new List<Text>(10);
    Text name_win;

    private void Awake()
    {
        name_request = transform.GetChild(2).gameObject;
        scores = transform.GetChild(1).gameObject;
        pause_menu = transform.GetChild(0).gameObject;
        name_win = name_request.transform.GetChild(1).gameObject.GetComponent<Text>();
        GameManager.GameLostEvent += new GameManager.GameLostHandler(Gamelost);
        update_score();
    }

    void update_score()
    {
        table = File.ReadAllLines(filename);
        for (int i = 0; i < 10; i++)
        {
            names_scores.Add(scores.transform.GetChild(i).gameObject.GetComponent<Text>());
            names_scores[i].text = "" + table[i];
        }
    }

    void add_score()
    {
        if (GameManager.score_palyer > System.Convert.ToInt32(table[5]))
        {
            table[5] = System.Convert.ToString(GameManager.score_palyer);
            table[0] = name_win.text;
        }
        else if (GameManager.score_palyer > System.Convert.ToInt32(table[6]))
        {
            table[6] = System.Convert.ToString(GameManager.score_palyer);
            table[1] = name_win.text;
        }
        else if (GameManager.score_palyer > System.Convert.ToInt32(table[7]))
        {
            table[7] = System.Convert.ToString(GameManager.score_palyer);
            table[2] = name_win.text;
        }
        else if (GameManager.score_palyer > System.Convert.ToInt32(table[8]))
        {
            table[8] = System.Convert.ToString(GameManager.score_palyer);
            table[3] = name_win.text;
        }
        else if (GameManager.score_palyer > System.Convert.ToInt32(table[9]))
        {
            table[9] = System.Convert.ToString(GameManager.score_palyer);
            table[4] = name_win.text;
        }

        StreamWriter sw = new StreamWriter(filename);
        for (int i = 0; i < 10; i++)
        {
            sw.WriteLine(table[i]);
        }
        sw.Close();
    }

    void Gamelost()
    {
        pause_menu.SetActive(false);
        if (GameManager.score_palyer > System.Convert.ToInt32(table[9]))
        {
            name_request.SetActive(true);
        }
        else
        {
            scores.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        GameManager.GameLostEvent -= Gamelost;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && scores.activeSelf == true)
        {
            scores.SetActive(false);
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Return) && name_request.activeSelf == true)
        {
            add_score();
            update_score();
            name_request.SetActive(false);
            scores.SetActive(true);
        }
    }

    public void Quite()
    {
        Application.Quit();
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

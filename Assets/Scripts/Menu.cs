using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string filename = "Scores.txt";
    GameObject scores;
    GameObject menu;
    GameObject control;
    List<Text> names_scores = new List<Text>(10);
    string[] table = new string[10];

    private void Awake()
    {
        control = transform.GetChild(2).gameObject;
        scores = transform.GetChild(1).gameObject;
        menu = transform.GetChild(0).gameObject;
        Update_score();
    }

    void Update_score()
    {

        for (int i = 0; i < 10; i++)
        {
            names_scores.Add(scores.transform.GetChild(i).gameObject.GetComponent<Text>());
        }

        if (File.Exists(filename) == true)
        {
            table = File.ReadAllLines(filename);
            for (int i = 0; i < 10; i++)
            {
                names_scores[i].text = "" + table[i];
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                table[i] = names_scores[i].text;
            }

            StreamWriter sw = new StreamWriter(filename);
            for (int i = 0; i < 10; i++)
            {
                sw.WriteLine(table[i]);
            }
            sw.Close();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (scores.activeSelf == true)
            {
                scores.SetActive(false);
                menu.SetActive(true);
            }
            else if(control.activeSelf == true)
            {
                control.SetActive(false);
                menu.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        scores.SetActive(false);
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
        Update_score();
    }

    public void Control()
    {
        menu.SetActive(false);
        control.SetActive(true);
    }
}

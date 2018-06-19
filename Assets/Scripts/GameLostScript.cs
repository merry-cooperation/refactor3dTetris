using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLostScript : MonoBehaviour
{
    public string filename = "Scores.txt";
    string[] table = new string[10];
    int[] point = new int[5];
    string player_name;
    GameObject scores;
    GameObject pause_menu;
    public GameObject name_request;
    public Text name_win;
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

    private void Awake()
    {
        name_request = transform.GetChild(2).gameObject;
        scores = transform.GetChild(1).gameObject;
        pause_menu = transform.GetChild(0).gameObject;
        GameManager.GameLostEvent += new GameManager.GameLostHandler(Gamelost);
        update_score();
    }

    void update_score()
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

        point[0] = System.Convert.ToInt32(table[5]);
        point[1] = System.Convert.ToInt32(table[6]);
        point[2] = System.Convert.ToInt32(table[7]);
        point[3] = System.Convert.ToInt32(table[8]);
        point[4] = System.Convert.ToInt32(table[9]);
    }

    void add_score()
    {
        player_name = name_win.text;
        if (GameManager.score_palyer > point[0])
        {
            table[5] = System.Convert.ToString(GameManager.score_palyer);
            table[0] = player_name;
        }
        else if (GameManager.score_palyer > point[1])
        {
            table[6] = System.Convert.ToString(GameManager.score_palyer);
            table[1] = player_name;
        }
        else if (GameManager.score_palyer > point[2])
        {
            table[7] = System.Convert.ToString(GameManager.score_palyer);
            table[2] = player_name;
        }
        else if (GameManager.score_palyer > point[3])
        {
            table[8] = System.Convert.ToString(GameManager.score_palyer);
            table[3] = player_name;
        }
        else if (GameManager.score_palyer > point[4])
        {
            table[9] = System.Convert.ToString(GameManager.score_palyer);
            table[4] = player_name;
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
        if (GameManager.score_palyer > point[4])
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
        Time.timeScale = 0;
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string filename = "Scores.txt";
    string[] table = new string[10];
    GameObject scores;
    GameObject menu;
    GameObject pause_menu;
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
        scores = transform.GetChild(2).gameObject;
        menu = transform.GetChild(1).gameObject;
        pause_menu = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && scores.activeSelf == true)
        {
            scores.SetActive(false);
            menu.SetActive(true);
        }
    }


    //void OnGUI()
    //{
    //    playerName = GUI.TextField(new Rect(100, 100, 100, 20), playerName);
    //    if (Input.GetKeyDown(KeyCode.Q))
    //        return;
    //}

    //void OnGUI()
    //{
    //    SceneManager.LoadScene(0);
    //    playerName = GUI.TextField(new Rect(100, 100, 100, 20), playerName);
    //    //OnDestroy();
    //}

    //private void OnDestroy()
    //{
    //    GameManager.GameLostEvent -= HandleGameLost;
    //}

    public void StartGame()
    {
        scores.SetActive(false);
        menu.SetActive(false);
        pause_menu.SetActive(false);
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
        pause_menu.SetActive(false);
        scores.SetActive(true);

        table = File.ReadAllLines(filename);
        StreamWriter sw = new StreamWriter(filename);

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

        sw.WriteLine(table[0]);
        sw.WriteLine(table[1]);
        sw.WriteLine(table[2]);
        sw.WriteLine(table[3]);
        sw.WriteLine(table[4]);
        sw.WriteLine(table[5]);
        sw.WriteLine(table[6]);
        sw.WriteLine(table[7]);
        sw.WriteLine(table[8]);
        sw.WriteLine(table[9]);
        sw.Close();


        // StreamReader st = new StreamReader(filename);
        //if(st!=null)
        //{
        //    a[0] = System.Convert.ToString(st.ReadLine());
        //    a[1] = System.Convert.ToString(st.ReadLine());
        //    a[2] = System.Convert.ToString(st.ReadLine());
        //    a[3] = System.Convert.ToString(st.ReadLine());
        //    a[4] = System.Convert.ToString(st.ReadLine());
        //    a[5] = System.Convert.ToString(st.ReadLine());
        //    a[6] = System.Convert.ToString(st.ReadLine());
        //    a[7] = System.Convert.ToString(st.ReadLine());
        //    a[8] = System.Convert.ToString(st.ReadLine());
        //    a[9] = System.Convert.ToString(st.ReadLine());

        //    first.text = "" + a[0];
        //    second.text = "" + a[1];
        //}
        // sw.Close();
    }
}

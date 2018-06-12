using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    //public class PlayerInfo
    //{
    //    public string Name;
    //    public int Score;

    //    public void SetFromCsv(string csvStr)
    //    {
    //        string[] fields = csvStr.Split(';');
    //        Name = fields[0];
    //        Score = int.Parse(fields[1]);
    //    }

    //    public string GetCsvString()
    //    {
    //        return string.Format("{0};{1}", Name, Score);
    //    }
    //}
    //private const int n = 5;
    //List<PlayerInfo> recordTable = new List<PlayerInfo>(n);
    public string filename = "Scores.txt";
    string[] a = new string[10];
    GameObject scores;
    GameObject menu;
    //private Text text;
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

        a = File.ReadAllLines(filename);
        StreamWriter sw = new StreamWriter(filename);


        first.text = "" + a[0];
        second.text = "" + a[1];
        third.text = "" + a[2];
        fourth.text = "" + a[3];
        fifth.text = "" + a[4];

        first_s.text = "" + a[5];
        second_s.text = "" + a[6];
        third_s.text = "" + a[7];
        fourth_s.text = "" + a[8];
        fifth_s.text = "" + a[9];

        sw.WriteLine(a[0]);
        sw.WriteLine(a[1]);
        sw.WriteLine(a[2]);
        sw.WriteLine(a[3]);
        sw.WriteLine(a[4]);
        sw.WriteLine(a[5]);
        sw.WriteLine(a[6]);
        sw.WriteLine(a[7]);
        sw.WriteLine(a[8]);
        sw.WriteLine(a[9]);
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public delegate void GameLostHandler();
    public static event GameLostHandler GameLostEvent;

    public delegate void GameTickHandler(); //ссылка на функцию
    public static event GameTickHandler GameTickEvent;

    public delegate void RecolourHandler();
    public static event RecolourHandler RecolourEvent;

    public delegate void LayersClearedHandler(List<int> layers);
    public static event LayersClearedHandler LayersClearedEvent;

    public delegate void MoveHandler(Vector3 translation);
    public static event MoveHandler MoveEvent;

    public delegate void RotationHandler(Vector3 rotation);
    public static event RotationHandler RotationEvent;

    public GameObject cubePrefab;
    public GameObject l_shaped;
    public GameObject t_shaped;
    public GameObject straight;
    public GameObject Big_cube;
    public GameObject z_shaped;
    public GameObject fixedCube;
    public GameObject right_screw;
    public GameObject left_screw;
    public GameObject branch;


    private int[] points = new int[4];
    public Sprite on;
    public Sprite off;
    Image theImage;
    public GameObject pause_menu;
    public GameObject soung;

    public Text score;
    public static int score_palyer;

    private AudioSource music;

    public float basicPeriod;
    private float period;
    private float progress;

    private enum State { Play, Pause, Lost };
    private State state;

    private const int SIZEX = 5;
    private const int SIZEY = 10;
    private const int FULLY = 14;
    private const int SIZEZ = 5;

    private GameObject[,,] well;
    private GameObject activeTetracube;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        state = State.Play;
        theImage = GameObject.Find("Music").GetComponent<Image>();
        theImage.sprite = on;
    }
    // Use this for initialization
    void Start()
    {
        music = Camera.main.GetComponent<AudioSource>();

        well = new GameObject[SIZEX, FULLY, SIZEZ];
        progress = 0.0f;
        period = basicPeriod;
        points[0] = 100;
        points[1] = 300;
        points[2] = 500;
        points[3] = 2000;
        score_palyer = 0;
        score.text = "Score " + score_palyer;

        ActiveTetrominoControl.StopFallEvent += new ActiveTetrominoControl.StopFallHandler(HandleStopFall);
        GameLostEvent += new GameLostHandler(HandleGameLost);
        state = State.Play;
        activeTetracube = null;
        theImage.sprite = on;
    }

    private void OnDestroy()
    {
        ActiveTetrominoControl.StopFallEvent -= HandleStopFall;
        GameLostEvent -= HandleGameLost;
    }

    private void HandleGameLost()
    {
        //Debug.Log("Game Lost");
        music.Stop();
        state = State.Lost;
    }

    public bool PositionValid(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);
        if (x >= 0 && x < SIZEX && y >= 0 && y < FULLY && z >= 0 && z < SIZEZ &&
            well[x, y, z] == null)
        {
            return true;
        }
        return false;
    }

    private void HandleStopFall(Transform[] transforms)
    {
        activeTetracube = null;
        foreach (Transform t in transforms)
        {

            int x = Mathf.RoundToInt(t.position.x);
            int y = Mathf.RoundToInt(t.position.y);
            int z = Mathf.RoundToInt(t.position.z);

            if (y >= SIZEY)
            {
                if (GameLostEvent != null)
                {
                    GameLostEvent();
                }
                return;
            }
            //Debug.Log("HandleStopFall: x = " + t.position.x + " y = " + t.position.y + " z = " + t.position.z);
            //Debug.Log("HandleStopFall: cube at x = " + x + " y = " + y + " z = " + z);
            well[x, y, z] = Instantiate(fixedCube, t.position, Quaternion.identity);
        }

        CheckLayers();

    }

    private void CheckLayers()
    {
        List<int> filledLayers = new List<int>();

        for (int y = SIZEY - 1; y >= 0; y--)
        {
            bool layerFilled = true;
            for (int z = 0; z < SIZEZ && layerFilled; z++)
            {
                for (int x = 0; x < SIZEX && layerFilled; x++)
                {
                    if (well[x, y, z] == null)
                    {
                        layerFilled = false;
                    }
                }
            }
            if (layerFilled)
            {
                filledLayers.Add(y);
            }
        }
        if (filledLayers.Count > 0)
        {
            score_palyer += points[filledLayers.Count - 1];
            score.text = "Score " + score_palyer;
            ClearLayers(filledLayers);
        }
    }
    private void ClearLayers(List<int> filledLayers)
    {
        foreach (int y in filledLayers)
        {
            for (int x = 0; x < SIZEX; x++)
            {
                for (int z = 0; z < SIZEZ; z++)
                {
                    well[x, y, z].SetActive(false);
                    Destroy(well[x, y, z]);
                    well[x, y, z] = null;
                }
            }

            for (int layer = y; layer < SIZEY - 1; layer++)
            {
                for (int x = 0; x < SIZEX; x++)
                {
                    for (int z = 0; z < SIZEZ; z++)
                    {
                        well[x, layer, z] = well[x, layer + 1, z];
                    }
                }
            }
        }
        if (RecolourEvent != null)
        {
            RecolourEvent();
        }
        // slight pause
        progress -= 0.5f;
        if (LayersClearedEvent != null)
        {
            LayersClearedEvent(filledLayers);
        }

    }

    private void SpawnRandomTetromino()
    {
        int x, z;
        switch (Random.Range(0, 8))
        {
            case 0:
                {
                    x = Random.Range(0, 4);
                    z = Random.Range(0, 4);
                    activeTetracube = Instantiate(Big_cube, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 1:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    activeTetracube = Instantiate(t_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 2:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    activeTetracube = Instantiate(l_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 3:
                {
                    x = Random.Range(0, 2);
                    z = Random.Range(0, 5);
                    activeTetracube = Instantiate(straight, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 4:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    activeTetracube = Instantiate(z_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 5:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 3);
                    activeTetracube = Instantiate(right_screw, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 6:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 3);
                    activeTetracube = Instantiate(left_screw, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 7:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 3);
                    activeTetracube = Instantiate(branch, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
        }
    }
    private void NextTetracube()
    {
        SpawnRandomTetromino();
        //activeTetracube = Instantiate(cubePrefab, new Vector3(1, FULLY - 2, 1), Quaternion.identity);
        //activeTetracube = Instantiate(straight, new Vector3(0, FULLY - 3, 0), Quaternion.Euler(0, 0, 0));
    }
    void Update()
    {
        if (progress >= period)
        {
            progress -= period;

            if (state == State.Play)
            {
                if (activeTetracube == null)
                {
                    NextTetracube();
                }
                if (GameTickEvent != null)
                {
                    // Debug.Log("Game Tick");

                    GameTickEvent();
                    if (RecolourEvent != null)
                    {
                        RecolourEvent();
                    }
                }
            }
        }
        else
        {
            progress += Time.deltaTime;
        }

        if (state == State.Play)
        {
            GetInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.Play)
            {
                state = State.Pause;
                pause_menu.SetActive(true);
            }
            else if (state == State.Pause && pause_menu.activeSelf == true)
            {
                state = State.Play;
                pause_menu.SetActive(false);
            }
        }
    }

    private void GetInput()
    {
        Vector3 translation = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.S))
        {
            translation.z = 1;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            translation.z = -1;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            translation.x = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            translation.x = -1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation.x = 90;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            rotation.y = 90;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            rotation.z = 90;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            period = basicPeriod / 5;
        }
        else
        {
            period = basicPeriod;
        }

        if (translation != Vector3.zero && MoveEvent != null)
        {
            //Debug.Log("MoveEvent");
            MoveEvent(translation);
            if (RecolourEvent != null)
            {
                RecolourEvent();
            }
        }

        if (rotation != Vector3.zero && RotationEvent != null)
        {
            //Debug.Log("RotationEvent");
            RotationEvent(rotation);
            if (RecolourEvent != null)
            {
                RecolourEvent();
            }
        }
    }

    public void OnOff()
    {
        if (theImage.sprite == off)
        {
            theImage.sprite = on;
            music.Play();
        }
        else
        {
            theImage.sprite = off;
            music.Pause();
        }
    }
}

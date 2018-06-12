using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public delegate void MoveHandler(Vector3 translation, Vector3 rotation);
    public static event MoveHandler MoveEvent;

    public GameObject cubePrefab;
    public GameObject l_shaped;
    public GameObject t_shaped;
    public GameObject straight;
    public GameObject Big_cube;
    public GameObject z_shaped;
    public GameObject fixedCube;

    public float period;
    private float progress;

    private enum State { Play, Pause, Lost };
    private State state;

    private const int SIZEX = 5;
    private const int SIZEY = 10;
    private const int FULLY = 14;
    private const int SIZEZ = 5;

    private GameObject[,,] well;

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
    }
    // Use this for initialization
    void Start()
    {
        well = new GameObject[SIZEX, FULLY, SIZEZ];
        progress = 0.0f;

        ActiveTetrominoControl.StopFallEvent += new ActiveTetrominoControl.StopFallHandler(HandleStopFall);
        GameLostEvent += new GameLostHandler(HandleGameLost);

    }

    private void OnDestroy()
    {
        ActiveTetrominoControl.StopFallEvent -= HandleStopFall;
        GameLostEvent -= HandleGameLost;
    }

    private void HandleGameLost()
    {
        Debug.Log("Game Lost");
        state = State.Lost;
    }

    public bool PositionValid(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if (x >= 0 && x < SIZEX && y >= 0 && y < FULLY && z >= 0 && z < SIZEZ &&
            well[x, y, z] == null)
        {
            return true;
        }
        return false;
    }

    private void HandleStopFall(Transform[] transforms)
    {
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
        
        //SpawnRandomTetromino();
        Instantiate(cubePrefab, new Vector3(1, FULLY - 2, 1), Quaternion.identity);
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
                        if (well[x, layer, z] != null)
                        {
                            FixedCube cube = well[x, layer, z].GetComponent<FixedCube>();
                            if (cube != null)
                            {
                                cube.FallDown();
                            }
                        }

                    }
                }
            }
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
        switch (Random.Range(0, 5))
        {
            case 0:
                {
                    x = Random.Range(0, 4);
                    z = Random.Range(0, 4);
                    Instantiate(Big_cube, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 1:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    Instantiate(t_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 2:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    Instantiate(l_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 3:
                {
                    x = Random.Range(0, 2);
                    z = Random.Range(0, 5);
                    Instantiate(straight, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
            case 4:
                {
                    x = Random.Range(0, 3);
                    z = Random.Range(0, 5);
                    Instantiate(z_shaped, new Vector3(x, FULLY - 2, z), Quaternion.identity);
                    break;
                }
        }
    }

    void Update()
    {
        if (progress >= period)
        {
            progress -= period;

            if (state == State.Play && GameTickEvent != null)
            {
                GameTickEvent();
                if (RecolourEvent != null)
                {
                    RecolourEvent();
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

        if ((translation != Vector3.zero || rotation != Vector3.zero) && MoveEvent != null)
        {
            MoveEvent(translation, rotation);
            if (RecolourEvent != null)
            {
                RecolourEvent();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public delegate void GameTickHandler();
    public event GameTickHandler GameTickEvent;
    public delegate void MoveXHandler(bool right);
    public event MoveXHandler MoveXEvent;
    public delegate void MoveZHandler(bool up);
    public event MoveZHandler MoveZEvent;

    public GameObject cubePrefab;

    public float period;
    private float progress;

    public enum Piece { Cube };

    private enum Cell { Space, Piece };
    private Cell[,,] glass;
    private const int SIZEX = 5;
    private const int SIZEY = 10;
    private const int SIZEZ = 5;
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
    }
    // Use this for initialization
    void Start()
    {
        glass = new Cell[SIZEX, SIZEY, SIZEZ];
        progress = 0.0f;

        FallingObj.StopFallEvent += new FallingObj.StopFallHandler(StopFallHandler);
    }

    public bool PositionValid(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if (x >= 0 && x < SIZEX && y >= 0 && y < SIZEY && z >= 0 && z < SIZEZ &&
            glass[x, y, z] == Cell.Space)
        {
            return true;
        }
        return false;
    }

    void StopFallHandler(Vector3 pos)
    {
        Instantiate(cubePrefab, new Vector3(2, 9, 2), Quaternion.identity);
        glass[(int)pos.x, (int)pos.y, (int)pos.z] = Cell.Piece;
    }

    // Update is called once per frame
    void Update()
    {
        if (progress >= period)
        {
            progress -= period;
            if (GameTickEvent != null)
            {
                GameTickEvent();
            }

        }
        else
        {
            progress += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (MoveZEvent != null)
            {
                MoveZEvent(true);
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (MoveZEvent != null)
            {
                MoveZEvent(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (MoveZEvent != null)
            {

                MoveXEvent(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (MoveZEvent != null)
            {
                MoveXEvent(false);
            }
        }
    }

}

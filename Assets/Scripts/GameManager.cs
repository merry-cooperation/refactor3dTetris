﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public delegate void GameTickHandler();
    public static event GameTickHandler GameTickEvent;

    public delegate void MoveEventHandler(Vector3 translation);
    public static event MoveEventHandler MoveEvent;

    public GameObject cubePrefab;
    public GameObject l_shaped;
    public GameObject t_shaped;
    public GameObject straight;
    public GameObject Big_cube;

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

        FallingObj.StopFallEvent += new FallingObj.StopFallHandler(HandleStopFall);
    }
    private void OnDestroy()
    {
        FallingObj.StopFallEvent -= HandleStopFall;
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

    void HandleStopFall(Transform[] transforms)
    {
        foreach (Transform t in transforms)
        {
            glass[(int)t.position.x, (int)t.position.y, (int)t.position.z] = Cell.Piece;
        }
        Instantiate(cubePrefab, new Vector3(2, 9, 2), Quaternion.identity);
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

        Vector3 translation = GetInput();
        if (MoveEvent != null)
        {
            MoveEvent(translation);
        }
    }

    private Vector3 GetInput()
    {
        Vector3 translation = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            translation.z = 1;

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            translation.z = -1;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            translation.x = 1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            translation.x = -1;
        }
        return translation;
    }
}

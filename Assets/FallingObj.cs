using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObj : MonoBehaviour
{
    public delegate void StopFallHandler(Vector3 pos);
    public static event StopFallHandler StopFallEvent;

    void Start()
    {
        GameManager.instance.MoveXEvent += new GameManager.MoveXHandler(HandleMoveX);
        GameManager.instance.MoveZEvent += new GameManager.MoveZHandler(HandleMoveZ);
        GameManager.instance.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
    }

    private void HandleMoveX(bool right)
    {
        if (right)
        {
            transform.Translate(1, 0, 0);
        }
        else
        {
            transform.Translate(-1, 0, 0);
        }
    }

    private void HandleMoveZ(bool up)
    {
        if (up)
        {
            transform.Translate(0, 0, 1);
        }
        else
        {
            transform.Translate(0, 0, -1);
        }
    }

    void HandleGameTick()
    {
        if (transform.position.y >= 1)
        {
            transform.Translate(0, -1, 0);

        }
        else
        {

            if (StopFallEvent != null)
            {
                StopFallEvent(transform.position);
            }

            GameManager.instance.MoveXEvent -= HandleMoveX;
            GameManager.instance.MoveZEvent -= HandleMoveZ;
            GameManager.instance.GameTickEvent -= HandleGameTick;
            Destroy(this);
        }

    }
}

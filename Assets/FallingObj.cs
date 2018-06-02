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
        Vector3 oldPos = transform.position;
        if (right)
        {
            transform.Translate(1, 0, 0);
        }
        else
        {
            transform.Translate(-1, 0, 0);
        }
        if (!GameManager.instance.PositionValid(transform.position))
        {
            transform.position = oldPos;
        }
    }

    private void HandleMoveZ(bool up)
    {
        Vector3 oldPos = transform.position;
        if (up)
        {
            transform.Translate(0, 0, 1);
        }
        else
        {
            transform.Translate(0, 0, -1);
        }
        if (!GameManager.instance.PositionValid(transform.position))
        {
            transform.position = oldPos;
        }
    }

    void HandleGameTick()
    {
        Vector3 oldPos = transform.position;
        transform.Translate(0, -1, 0);
        if (!GameManager.instance.PositionValid(transform.position))
        {
            transform.position = oldPos;
            if (StopFallEvent != null)
            {
                StopFallEvent(transform.position);
            }
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = true;
            GameManager.instance.MoveXEvent -= HandleMoveX;
            GameManager.instance.MoveZEvent -= HandleMoveZ;
            GameManager.instance.GameTickEvent -= HandleGameTick;
            Destroy(this);
        }
    }
}

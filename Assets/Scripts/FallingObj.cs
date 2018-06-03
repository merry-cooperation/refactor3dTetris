using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObj : MonoBehaviour
{
    public delegate void StopFallHandler(Vector3 pos);
    public static event StopFallHandler StopFallEvent;

    private void Awake()
    {
        GameManager.MoveEvent += new GameManager.MoveEventHandler(HandleMove);
        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
    }
    private void OnDestroy()
    {
        GameManager.MoveEvent -= HandleMove;
        GameManager.GameTickEvent -= HandleGameTick;
    }
    private void HandleMove(Vector3 translation)
    {
        Vector3 oldPos = transform.position;
        transform.Translate(translation);
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

            Destroy(this);
        }
    }
}

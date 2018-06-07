using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObj : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private void Awake()
    {
        int n = transform.childCount;
        childTransform = new Transform[n];
        childGameObj = new GameObject[n];
        for (int i = 0; i < n; i++)
        {
            childTransform[i] = transform.GetChild(i);
            childGameObj[i] = childTransform[i].gameObject;
        }

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
        foreach (Transform piecePart in childTransform)
        {
            if (!GameManager.instance.PositionValid(piecePart.position))
            {
                transform.position = oldPos;
                break;
            }
        }

    }

    void HandleGameTick()
    {
        Vector3 oldPos = transform.position;
        transform.Translate(0, -1, 0);
        foreach(Transform piecePart in childTransform)
        {
            if (!GameManager.instance.PositionValid(piecePart.position))
            {
                transform.position = oldPos;
                
                foreach(var child in childGameObj)
                {
                    MeshRenderer mr = child.GetComponent<MeshRenderer>();
                    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    mr.receiveShadows = true;
                    //child.AddComponent<InactiveCube>();
                }

                if (StopFallEvent != null)
                {
                    StopFallEvent(childTransform);
                }
                Destroy(this);
                break;
            }
        }
        
    }
}

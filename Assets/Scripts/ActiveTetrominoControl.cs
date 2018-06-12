using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTetrominoControl : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.down };

    private static int layerFixedCube = 1 << 22;
    private static int layerWell = 1 << 20;
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

        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
        GameManager.MoveEvent += new GameManager.MoveHandler(HandleMove);
    }

    private void OnDestroy()
    {
        GameManager.GameTickEvent -= HandleGameTick;
        GameManager.MoveEvent -= HandleMove;
    }

    private void HandleMove(Vector3 translation, Vector3 rotation)
    {
        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        transform.Translate(translation, Space.World);
        transform.Rotate(rotation, Space.World);

        foreach (Transform tetrominoPart in childTransform)
        {
            if (!GameManager.instance.PositionValid(tetrominoPart.position))
            {
                transform.position = oldPos;
                transform.rotation = oldRot;
                break;
            }
        }
        RaycastRecolor();
    }

    void HandleGameTick()
    {
        //Debug.Log("Game tick");
        Vector3 oldPos = transform.position;
        transform.Translate(0, -1, 0, Space.World);

        bool posValid = true;
        foreach (Transform piecePart in childTransform)
        {
            if (!GameManager.instance.PositionValid(piecePart.position))
            {
                posValid = false;
                break;
            }
        }

        if (posValid)
        {
            RaycastRecolor();
        }
        else
        {
            transform.position = oldPos;

            if (StopFallEvent != null)
            {
                StopFallEvent(childTransform);
            }

            Destroy(gameObject);
        }

    }

    private void RaycastRecolor()
    {
        foreach (Transform t in childTransform)
        {
            foreach (var dir in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(t.position, dir, out hit, Mathf.Infinity, layerWell))
                {
                    IHighlightable obj = hit.transform.gameObject.GetComponent<IHighlightable>();
                    if (obj != null)
                    {
                        if (dir == Vector3.down)
                        {
                            obj.HighlightOn(Rainbow.colors.Length - 1);
                        }
                        else if (dir == Vector3.forward || dir == Vector3.back)
                        {
                            obj.HighlightOn(Mathf.RoundToInt(t.position.z));
                        }
                        else
                        {
                            obj.HighlightOn(Mathf.RoundToInt(t.position.x));
                        }
                    }
                }

                if (Physics.Raycast(t.position, dir, out hit, Mathf.Infinity, layerFixedCube))
                {
                    IHighlightable obj = hit.transform.gameObject.GetComponent<IHighlightable>();
                    if (obj != null)
                    {
                        obj.HighlightOn(1);
                    }
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTetrominoControl : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private List<IHighlightable> highlighted;

    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.down };

    private static int layermask = 1 << 20 | 1 << 22;
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

        highlighted = new List<IHighlightable>();

        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
        GameManager.MoveEvent += new GameManager.MoveEventHandler(HandleMove);
    }

    private void OnDestroy()
    {
        GameManager.GameTickEvent -= HandleGameTick;
        GameManager.MoveEvent -= HandleMove;
        foreach (var obj in highlighted)
        {
            if (obj != null)
            {
                obj.HighlightOff();
            }
        }
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
                return;
            }
        }
        RaycastRecolor();
    }

    void HandleGameTick()
    {
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

            //foreach (var child in childGameObj)
            //{
            //    MeshRenderer mr = child.GetComponent<MeshRenderer>();
            //    mr.material = inactiveMaterial;
            //}

            foreach (var obj in highlighted)
            {
                obj.HighlightOff();
            }
            highlighted.Clear();
            if (StopFallEvent != null)
            {
                StopFallEvent(childTransform);
            }


            Destroy(gameObject);
        }

    }

    private void RaycastRecolor()
    {
        foreach (var obj in highlighted)
        {
            obj.HighlightOff();
        }
        highlighted.Clear();

        foreach (Transform t in childTransform)
        {
            foreach (var dir in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(t.position, dir, out hit, Mathf.Infinity, layermask))
                {
                    IHighlightable obj = hit.transform.gameObject.GetComponent<IHighlightable>();
                    if (obj != null)
                    {
                        highlighted.Add(obj);
                        obj.HighlightOn();
                    }
                }
            }

        }
    }

}

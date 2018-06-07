using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTetrominoControl : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    public Material inactiveMaterial;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private List<Highlighter> recolored;

    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.down };

    private int layermask;
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

        recolored = new List<Highlighter>();

        // tetrominoes
        layermask = 1 << 21;
        // well
        layermask |= 1 << 20;
        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
        GameManager.MoveEvent += new GameManager.MoveEventHandler(HandleMove);
    }
    private void OnDestroy()
    {
        GameManager.GameTickEvent -= HandleGameTick;
        GameManager.MoveEvent -= HandleMove;
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
                return;
            }
        }
        RaycastRecolor();
    }

    void HandleGameTick()
    {
        Vector3 oldPos = transform.position;
        transform.Translate(0, -1, 0);

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

            foreach (var child in childGameObj)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                mr.material = inactiveMaterial;
            }

            if (StopFallEvent != null)
            {
                StopFallEvent(childTransform);
            }
            Destroy(this);
        }

    }

    private void RaycastRecolor()
    {
        foreach (var obj in recolored)
        {
            obj.SetBasicColor();
        }
        recolored.Clear();

        foreach (Transform t in childTransform)
        {
            foreach(var dir in directions)
            {
                RaycastHit hit;
                if (Physics.Raycast(t.position, dir, out hit, Mathf.Infinity, layermask))
                {
                    Highlighter highlighter = hit.transform.gameObject.GetComponent<Highlighter>();
                    if (highlighter != null)
                    {
                        recolored.Add(highlighter);
                        highlighter.SetHighlight();
                    }
                }
            }

        }
    }

}

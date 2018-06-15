using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTetrominoControl : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private static Vector3[] raycastDirections = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.down };
    private static Vector3[] rotationFixDirections = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.up };

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
        GameManager.RotationEvent += new GameManager.RotationHandler(HandleRotate);
    }

    private void OnDisable()
    {
        GameManager.GameTickEvent -= HandleGameTick;
        GameManager.MoveEvent -= HandleMove;
        GameManager.RotationEvent -= HandleRotate;
    }

    private void HandleMove(Vector3 translation)
    {
        Vector3 oldPos = transform.position;

        transform.Translate(translation, Space.World);

        if (!PosIsValid())
        {
            transform.position = oldPos;
        }

        RaycastRecolor();
    }

    private void HandleRotate(Vector3 rotation)
    {
        Quaternion oldRot = transform.rotation;
        transform.Rotate(rotation, Space.World);
        
        int countInvalidPos = 0;

        foreach (Transform tetrominoPart in childTransform)
        {
            if (!GameManager.instance.PositionValid(tetrominoPart.position))
            {
                countInvalidPos++;
            }
            if (countInvalidPos > 1)
            {
                transform.rotation = oldRot;
                break;
            }
        }

        if (countInvalidPos == 1)
        {
            // if only one cube in a figure went outside bounds, try to fix position
            Vector3 oldPos = transform.position;
            bool rotationSuccessful = false;
            foreach (Vector3 direction in rotationFixDirections)
            {
                transform.Translate(direction, Space.World);
                if (PosIsValid())
                {
                    rotationSuccessful = true;
                    break;
                }
                else
                {
                    transform.position = oldPos;
                }
            }
            if (!rotationSuccessful)
            {
                transform.rotation = oldRot;
            }
        }

        RaycastRecolor();
    }

    private bool PosIsValid()
    {
        foreach (Transform tetrominoPart in childTransform)
        {
            if (!GameManager.instance.PositionValid(tetrominoPart.position))
            {
                return false;
            }
        }
        return true;
    }


    private void HandleGameTick()
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
            gameObject.SetActive(false);
            Destroy(gameObject);
            if (StopFallEvent != null)
            {
                StopFallEvent(childTransform);
            }
        }

    }

    private void RaycastRecolor()
    {
        foreach (Transform t in childTransform)
        {
            foreach (var dir in raycastDirections)
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

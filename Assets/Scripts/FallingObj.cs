﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObj : MonoBehaviour
{
    public delegate void StopFallHandler(Transform[] transforms);
    public static event StopFallHandler StopFallEvent;

    private Transform[] childTransform;
    private GameObject[] childGameObj;

    private List<MeshRenderer> recolored;
    private Color basic;
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

        basic = childGameObj[0].GetComponent<MeshRenderer>().material.color;
        recolored = new List<MeshRenderer>();

        // tetrominoes
        layermask = 1 << 21;
        //glass
        layermask |= 1 << 20;

        GameManager.MoveEvent += new GameManager.MoveEventHandler(HandleMove);
        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
    }
    private void OnDestroy()
    {
        GameManager.MoveEvent -= HandleMove;
        GameManager.GameTickEvent -= HandleGameTick;
    }

    private void Update()
    {
        DebugRaycast();
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
        // DoRaycast();
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

            // DoRaycast();
        }
        else
        {
            transform.position = oldPos;

            foreach (var child in childGameObj)
            {
                //MeshRenderer mr = child.GetComponent<MeshRenderer>();
                //mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                //mr.material.color = new Color(1f, 0f, 0f, 0.3f);
                //mr.receiveShadows = true;
            }

            foreach (var cubeMesh in recolored)
            {
                cubeMesh.material.color = basic;
            }

            if (StopFallEvent != null)
            {
                StopFallEvent(childTransform);
            }
            Destroy(this);
        }

    }

    private void DoRaycast()
    {
        foreach (Transform t in childTransform)
        {
            Ray ray = new Ray(t.position, t.TransformDirection(Vector3.forward));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                ///Debug.Log("hit");
                MeshRenderer mr = hit.transform.gameObject.GetComponent<MeshRenderer>();
                recolored.Add(mr);
                mr.material.color = new Color(1f, 1f, 1f, 0.1f);
                //Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.DrawRay(t.position, t.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            }
            else
            {
                //Debug.Log("not hit");
                //Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * 1000, Color.black);
                //Debug.DrawRay(t.position, t.TransformDirection(Vector3.right) * 1000, Color.gray);
            }
        }
    }

    private void DebugRaycast()
    {
        //foreach (Transform t in childTransform)
        //{

        //    RaycastHit hit;
        //    if (Physics.Raycast(t.position, t.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layermask))
        //    {
        //        Debug.Log("forward hit" + hit.collider.tag);
        //        Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

        //    }

        //    if (Physics.Raycast(t.position, t.TransformDirection(Vector3.right), out hit, Mathf.Infinity, layermask))
        //    {
        //        Debug.Log("right hit" + hit.collider.tag);
        //        Debug.DrawRay(t.position, t.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
        //    }

        //    if (Physics.Raycast(t.position, t.TransformDirection(Vector3.left), out hit, Mathf.Infinity, layermask))
        //    {
        //        Debug.Log("left hit" + hit.collider.tag);
        //        Debug.DrawRay(t.position, t.TransformDirection(Vector3.left) * hit.distance, Color.cyan);
        //    }

        //    if (Physics.Raycast(t.position, t.TransformDirection(Vector3.back), out hit, Mathf.Infinity, layermask))
        //    {
        //        Debug.Log("back hit" + hit.collider.tag);
        //        Debug.DrawRay(t.position, t.TransformDirection(Vector3.back) * hit.distance, Color.green);
        //    }
        //}



        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layermask))
        {
            Debug.Log("forward hit" + hit.collider.tag);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, layermask))
        {
            Debug.Log("right hit" + hit.collider.tag);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity, layermask))
        {
            Debug.Log("left hit" + hit.collider.tag);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.cyan);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity, layermask))
        {
            Debug.Log("back hit" + hit.collider.tag);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.green);
        }

    }
}

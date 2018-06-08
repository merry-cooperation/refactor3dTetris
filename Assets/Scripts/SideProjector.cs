using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideProjector : MonoBehaviour
{
    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left };
    private static int layerMask = 1 << 20;
    private List<Highlighter> recolored;
    private void Awake()
    {
        recolored = new List<Highlighter>();     
    }
    private void Start()
    {
        RaycastRecolor();
    }
    private void OnDestroy()
    {
        foreach (var obj in recolored)
        {
            obj.SetBasicColor();
        }
    }
    private void RaycastRecolor()
    {
        if (recolored.Count > 0)
        {
            foreach (var obj in recolored)
            {
                obj.SetBasicColor();
            }
            recolored.Clear();
        }

        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
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

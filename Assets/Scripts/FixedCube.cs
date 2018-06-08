using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCube : MonoBehaviour
{
    public float duration = 0.5f;

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
            if (obj != null)
            {
                obj.SetBasicColor();
            }
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

    public void FallDown()
    {
        StartCoroutine(FallAnim());
    }

    private IEnumerator FallAnim()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        float progress = 0.0f;
        float rate = 1.0f / duration;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, progress));
            RaycastRecolor();
            yield return null;
        }
    }
}

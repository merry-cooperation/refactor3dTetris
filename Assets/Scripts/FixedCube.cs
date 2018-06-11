using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCube : MonoBehaviour, IHighlightable
{
    public float fallDuration = 0.5f;

    public Material highlightMat;

    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left };
    private static int layerMask = 1 << 20;

    private List<IHighlightable> recolored;
    private MeshRenderer meshRenderer;
    private Material basicMaterial;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        basicMaterial = meshRenderer.material;
        recolored = new List<IHighlightable>();
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
                obj.HighlightOff();
            }
        }
    }

    private void RaycastRecolor()
    {
        if (recolored.Count > 0)
        {
            foreach (var obj in recolored)
            {
                obj.HighlightOff();
            }
            recolored.Clear();
        }

        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
            {
                IHighlightable obj = hit.transform.gameObject.GetComponent<IHighlightable>();
                if (obj != null)
                {
                    recolored.Add(obj);
                    obj.HighlightOn();
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
        float rate = 1.0f / fallDuration;
        while (progress < 1.0f)
        {
            progress += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, progress));
            RaycastRecolor();
            yield return null;
        }
    }

    public void HighlightOn()
    {
        meshRenderer.material = highlightMat;
    }
    public void HighlightOff()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = basicMaterial;
        }
    }
}

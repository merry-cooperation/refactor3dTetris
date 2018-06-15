using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCube : MonoBehaviour, IHighlightable
{
    public float fallDuration = 0.5f;
    public Material highlightMat;

    private static Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left };
    private static int layerWell = 1 << 20;

    private MeshRenderer meshRenderer;
    private Material basicMaterial;
    private int highlighted;
    private void Awake()
    {
        highlighted = -1;
        meshRenderer = GetComponent<MeshRenderer>();
        basicMaterial = meshRenderer.material;
        //ActiveTetrominoControl.StopFallEvent += new ActiveTetrominoControl.StopFallHandler(HandleStopFall);
        GameManager.MoveEvent += new GameManager.MoveHandler(HandleMove);
        GameManager.RotationEvent += new GameManager.RotationHandler(HandleRotation);
        GameManager.GameTickEvent += new GameManager.GameTickHandler(HandleGameTick);
        GameManager.RecolourEvent += new GameManager.RecolourHandler(HandleRecolour);
        RaycastRecolor();
    }

    private void OnDisable()
    {
        //Debug.Log("OnDisable");
        //ActiveTetrominoControl.StopFallEvent -= HandleStopFall;
        GameManager.GameTickEvent -= HandleGameTick;
        GameManager.MoveEvent -= HandleMove;
        GameManager.RotationEvent -= HandleRotation;
        GameManager.RecolourEvent -= HandleRecolour;

    }

    private void HandleGameTick()
    {
        RaycastRecolor();
    }
    private void HandleMove(Vector3 translation)
    {
        RaycastRecolor();
    }
    private void HandleRotation(Vector3 rotation)
    {
        RaycastRecolor();
    }
    private void HandleStopFall(Transform[] transforms)
    {
        RaycastRecolor();
    }

    private void RaycastRecolor()
    {
        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerWell))
            {
                IHighlightable obj = hit.transform.gameObject.GetComponent<IHighlightable>();
                if (obj != null)
                {
                    if (dir == Vector3.forward || dir == Vector3.back)
                    {
                        obj.HighlightOn(Mathf.RoundToInt(transform.position.z));
                    }
                    else
                    {
                        obj.HighlightOn(Mathf.RoundToInt(transform.position.x));
                    }
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

            yield return null;
        }
        RaycastRecolor();
    }

    public void HighlightOn(int highlight)
    {
        highlighted = highlight;
    }

    private void HandleRecolour()
    {
        if (highlighted > 0)
        {
            meshRenderer.material = highlightMat;
        }
        else
        {
            meshRenderer.material = basicMaterial;
        }
        highlighted = -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public Color highlight;

    private Color basic;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        basic = meshRenderer.material.color;
    }
    public void SetHighlight()
    {
        meshRenderer.material.color = highlight;
    }
    public void SetBasicColor()
    {
        meshRenderer.material.color = basic;
    }
}

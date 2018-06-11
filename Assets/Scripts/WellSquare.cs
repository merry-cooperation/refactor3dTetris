using UnityEngine;

public class WellSquare : MonoBehaviour, IHighlightable
{
    public Material highlightMaterial;
    private Material basic;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        basic = meshRenderer.material;
    }
    public void HighlightOn()
    {
        meshRenderer.material = highlightMaterial;
    }
    public void HighlightOff()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = basic;
        }
    }
}
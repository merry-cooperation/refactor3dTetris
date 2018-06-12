using UnityEngine;

public class WellSquare : MonoBehaviour, IHighlightable
{
    public Material highlightMaterial;
    private Material basic;
    private MeshRenderer meshRenderer;
    private int currentDepth;
    private void Awake()
    {
        currentDepth = -1;
        meshRenderer = GetComponent<MeshRenderer>();
        basic = meshRenderer.material;
        GameManager.RecolourEvent += new GameManager.RecolourHandler(HandleRecolour);
    }
    private void OnDestroy()
    {
        GameManager.RecolourEvent -= HandleRecolour;
    }
    public void HighlightOn(int depth)
    {
        if (depth > currentDepth)
        {
            currentDepth = depth;
        }
    }
    private void HandleRecolour()
    {
        //Debug.Log("recolour");
        if (currentDepth > -1 && currentDepth < Rainbow.colors.Length)
        {
            meshRenderer.material = highlightMaterial;
            meshRenderer.material.color = Rainbow.colors[currentDepth];
        }
        else
        {
            meshRenderer.material = basic;
        }
        currentDepth = -1;
    }
}
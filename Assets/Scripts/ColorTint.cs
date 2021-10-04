using UnityEngine;

public class ColorTint : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color color = Color.black;
    public float blend = 0.5f;
    
    void OnDrawGizmos()
    {
        Gizmos.color = color * blend;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 8f);
    }
    
    void Awake()
    {
        for (var i = 0; i < meshRenderer.materials.Length; i++)
        {
            meshRenderer.materials[i] = Instantiate(meshRenderer.materials[i]);
        }

        ApplyTint();
    }

    public void ApplyTint()
    {
        for (var i = 0; i < meshRenderer.materials.Length; i++)
        {
            meshRenderer.materials[i].SetColor("TintColor", color);
            meshRenderer.materials[i].SetFloat("TintBlend", blend);
        }
    }
}


using UnityEngine;

public class ZFightingHack : MonoBehaviour
{
    void Start()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        var vectors = new [] { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        var direction = 0;

        // Instead of fixing z-fighting across all the levels, we perform this very evil hack of 
        // nudging all of the graphics a tiny bit. AAA work right here. Hire me.
        
        foreach (var renderer in renderers)
        {
            renderer.gameObject.transform.position += vectors[direction] * 0.001f;
            direction = (direction + 1) % 6;
        }
    }
}

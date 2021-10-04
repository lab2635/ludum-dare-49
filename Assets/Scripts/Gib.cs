using UnityEngine;

public class Gib : MonoBehaviour
{
    private float time = 0f;

    [Header("Audio")] public AudioClip[] sfx;

    void Start()
    {
        var body = GetComponent<Rigidbody>();
        body.constraints |= RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        time += Time.deltaTime;
        
        if (time >= 5f || transform.position.y <= -15f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        const float collisionImpactThreshold = 8f;

        var magnitude = other.impulse.magnitude;
        
        if (magnitude >= collisionImpactThreshold )
        {
            var impactMaxRange = collisionImpactThreshold * collisionImpactThreshold;
            var volume = Mathf.Clamp01(other.impulse.magnitude / impactMaxRange) + 0.05f;
            GameManager.Instance.sfx.PlayRandomSoundEffect(volume, sfx);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class HorseAnimation : MonoBehaviour
{
    public GameObject explosionRadiusObject;

    [Serializable]
    public class GibInfo
    {
        public int count;
        public GameObject prefab;
        public bool scale;
    }
    
    private Animator animator;
    private Rigidbody rigidBody;
    
    private float earFlickTime;
    private float earFlickDelay;

    private Vector3 startPos;
    private Quaternion startRotation;

    public GibInfo[] gibs;
    public bool debug = false;

    public GameObject meshRenderer;
    public ParticleSystem explosionParticleSystem;
    
    [Header("Audio")]
    public AudioClip explosionSfx;
    public AudioClip neighSfx;
    public AudioClip impactSfx;
    public AudioClip impactAltSfx;

    private void Start()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        earFlickTime = 0f;
        earFlickDelay = 2f;
    }

    void OnExplosionFinished()
    {
        Explode();
    }

    public void StartExplosion()
    {
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        var delay = Random.Range(0f, 0.5f) + 0.25f;
        yield return new WaitForSeconds(delay);
        GameManager.Instance.sfx.PlaySoundEffect(neighSfx, 0.5f);
        animator.SetTrigger("Explode");
    }
    
    private void Explode()
    {
        Vector3 pos = transform.position + Vector3.up * 3f;
        
        StartCoroutine(PlayExplosionAnimation());
        
        foreach (var gib in gibs)
        {
            for (var i = 0; i < gib.count; i++)
            {
                var instance = Instantiate(gib.prefab, pos, Quaternion.identity);
                var transform = instance.transform;
                var dx = Random.Range(-3f, 3f);
                var offset = new Vector3(dx, 0f, dx);
                
                transform.position += offset;

                if (gib.scale)
                {
                    transform.localScale = Vector3.one * Random.Range(0.25f, 1.0f);
                }
                
                var body = instance.GetComponent<Rigidbody>();
                body.AddTorque(Random.onUnitSphere * 100f, ForceMode.Impulse);
                body.AddExplosionForce(50f, pos, 10f, 0f, ForceMode.Impulse);
            }
        }

        this.explosionRadiusObject.SetActive(true);
    }

    private IEnumerator PlayExplosionAnimation()
    {
        meshRenderer.SetActive(false);
        GameManager.Instance.sfx.PlaySoundEffect(explosionSfx);
        explosionParticleSystem.Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void Update()
    {
        earFlickTime += Time.deltaTime;

        if (earFlickTime >= earFlickDelay)
        {
            earFlickTime = 0f;
            earFlickDelay = Random.Range(2f, 10f);
            animator.SetTrigger("EarFlick");
        }

        if (!debug) return;
        
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     rigidBody.AddForce(transform.right * -10f, ForceMode.VelocityChange);
        // }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // animator.SetTrigger("Explode");
            StartExplosion();
        }
        
        //
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     transform.SetPositionAndRotation(startPos, startRotation);
        //     rigidBody.ResetInertiaTensor();
        //     rigidBody.angularVelocity = Vector3.zero;
        //     rigidBody.velocity = Vector3.zero;
        // }
        //
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Hit(Vector3.up);
        // }
    }

    private void Hit(Vector3 impact)
    {
        impact = impact.normalized;
        
        // NOTE: these are translated from Blender coordinate system
        // Unity Y -> Blender Z
        // Unity Z -> Blender Y
        // Unity X -> Blender X
        
        animator.SetFloat("HitZ+", Mathf.Clamp(impact.y, 0.0f, 1.0f));
        animator.SetFloat("HitZ-", Mathf.Clamp(-impact.y, 0.0f, 1.0f));
        animator.SetFloat("HitY+", Mathf.Clamp(impact.z, 0.0f, 1.0f));
        animator.SetFloat("HitY-", Mathf.Clamp(-impact.z, 0.0f, 1.0f));
        animator.SetFloat("HitX+", Mathf.Clamp(impact.x, 0.0f, 1.0f));
        animator.SetFloat("HitX-", Mathf.Clamp(-impact.x, 0.0f, 1.0f));
        
        animator.SetTrigger("Hit");
    }

    private void OnCollisionEnter(Collision other)
    {
        var impactForce = other.relativeVelocity.magnitude;
        
        if (impactForce >= 0.5f)
        {
            const float maxImpactForce = 50f;
            float impactAmount = Mathf.Clamp01(0.1f + impactForce / maxImpactForce);
            GameManager.Instance.sfx.PlayRandomSoundEffect(impactAmount * 0.5f, impactSfx, impactAltSfx);
        }
        
        if (impactForce >= 10f)
        {
            // TODO: replace with impact vector transformed from global space to local space
            Hit(Vector3.up);
        }
    }
}

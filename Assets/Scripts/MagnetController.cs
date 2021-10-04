using UnityEngine;

public class MagnetController : ActuatorBase
{
    public Animator animator;
    
    [Header("Detection")]
    public float detectionRadius = 1f;
    public float strength = 100f;
    public LayerMask detectionLayerMask;

    [SerializeField] 
    private bool magnetActive;
    private RaycastHit[] hits;
    private Collider[] collisions;
    
    public bool IsMagnetActive => magnetActive;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Start()
    {
        hits = new RaycastHit[4];
        collisions = new Collider[4];

        var detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.radius = detectionRadius;

        SetMagnetStatus(magnetActive);
    }

    void Update()
    {
        if (GameManager.Instance.isDebugMode && Input.GetKeyDown(KeyCode.M))
        {
            SetMagnetStatus(!magnetActive);
        }
    }

    public void SetMagnetStatus(bool status)
    {
        magnetActive = status;
        animator.SetBool("Active", magnetActive);
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particleSystems)
        {
            if (status)
            {
                system.Play();
            }
            else
            {
                system.Stop();
            }
        }
    }


    private void FixedUpdate()
    {
        var colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, collisions, detectionLayerMask);

        if (colliderCount <= 0) return;
        
        for (var j = 0; j < colliderCount; j++)
        {
            var collider = collisions[j];
            var behavior = collider.GetComponent<HorseBehavior>();
            
            if (behavior.HasActiveMagnet(this)) continue;
            
            var direction = Vector3.Normalize(collider.transform.position - transform.position);
            var hitCount = Physics.SphereCastNonAlloc(transform.position, 0.5f, direction, hits, detectionRadius, detectionLayerMask);
        
            // ensure target is within line of sight
            
            // NOTE: does not check that raycast target is the horse discovered by sphere overlap; 
            // fix this if we have problems.
            
            if (hitCount > 0 && magnetActive)
            {
                behavior.SetActiveMagnet(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            var behavior = other.GetComponent<HorseBehavior>();
            behavior.SetActiveMagnet(null);
        }
    }

    public override void Actuate()
    {
        this.SetMagnetStatus(true);
    }

    public override void ResetActuator()
    {
        this.SetMagnetStatus(false);
    }
}

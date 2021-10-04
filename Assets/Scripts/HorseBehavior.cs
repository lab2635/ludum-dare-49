using Unity.VisualScripting;
using UnityEngine;

public class HorseBehavior : MonoBehaviour
{
    public GameObject explosion;

    private MagnetController activeMagnet;
    private bool stuck;
    
    public bool Active
    {
        get;
        private set;
    }

    public bool HasActiveMagnet(MagnetController magnet)
    {
        return activeMagnet == magnet;
    }

    // Start is called before the first frame update
    void Start()
    {
        Active = true;
        GameManager.Instance.HorsesCreated++;
    }

    void FixedUpdate()
    {
        if (activeMagnet != null && activeMagnet.IsMagnetActive && !stuck)
        {
            var body = GetComponent<Rigidbody>();
            var direction = Vector3.Normalize(activeMagnet.transform.position - transform.position);
            var distance = Vector3.Distance(activeMagnet.transform.position, transform.position);
            
            body.AddForce(direction * activeMagnet.strength * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if (stuck && activeMagnet.IsMagnetActive == false)
        {
            DetachFromActiveMagnet();
        }
    }

    public void Explode()
    {
        if (Active == false) return;
        
        // Explode nearby
        //this.explosion.SetActive(true);
        //Active = false;
        Active = false;

        // Game over
        var animation = GetComponent<HorseAnimation>();
        animation.StartExplosion();
    }

    private void DetachFromActiveMagnet()
    {
        if (!stuck) return;
        
        var joint = GetComponent<FixedJoint>();
        Destroy(joint);
        stuck = false;
    }
    
    public void SetActiveMagnet(MagnetController magnetController)
    {
        activeMagnet = magnetController;
        
        if (magnetController == null)
        {
            Destroy(GetComponent<FixedJoint>());
        }
    }

    private void AttachToActiveMagnet()
    {
        if (activeMagnet == null || stuck) return;
        
        // If we touch the magnet, weld to the magnet with a fixed joint
        
        var body = GetComponent<Rigidbody>();
        var newJoint = body.AddComponent<FixedJoint>();
        newJoint.connectedBody = activeMagnet.GetComponent<Rigidbody>();
        stuck = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (activeMagnet != null && collision.gameObject == activeMagnet.gameObject)
        {
            AttachToActiveMagnet();
        }
        
        if (collision.gameObject.CompareTag("Horse"))
        {
            // explode!
            Explode();
            //this.gameObject.transform.Find("Mesh").gameObject.SetActive(false);
        }
    }
}

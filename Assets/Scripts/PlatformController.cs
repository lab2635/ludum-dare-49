using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformController : MonoBehaviour
{
    public GameObject level;
    public GameObject hinge;
    private bool isBumping;
    private float timeElapsed;

    private float lastImpactSfxTime = 0f;
    private float accelerationTime = 0f;
    private float rotation;
    private float previousDirection = 0f;
    
    private Rigidbody levelBody => level.GetComponent<Rigidbody>();
    private ParticleSystem[] leftParticleSystems;
    private ParticleSystem[] rightParticleSystems;

    private void Start()
    {
        GameObject leftEmitter = this.hinge.transform.Find("Left Emitter").gameObject;
        GameObject rightEmitter = this.hinge.transform.Find("Right Emitter").gameObject;
        this.leftParticleSystems = leftEmitter.GetComponentsInChildren<ParticleSystem>();
        this.rightParticleSystems = rightEmitter.GetComponentsInChildren<ParticleSystem>();
        this.SetParticleSystems(leftParticleSystems, false);
        this.SetParticleSystems(rightParticleSystems, false);
    }

    private void RotatePlatform(float direction)
    {
        if (direction != previousDirection)
        {
            accelerationTime = 0;
            GlobalVariables.Instance.tiltAngle = 0;
        }
        
        var rb = levelBody;
        var angle = GlobalVariables.Instance.tiltSpeed;
        var minAngle = GlobalVariables.Instance.minTiltAngle;
        var maxAngle = GlobalVariables.Instance.maxTiltAngle;

        var curve = GlobalVariables.Instance.tiltAccelerationCurve;
        var value = curve.Evaluate(accelerationTime);

        angle *= value;
        GlobalVariables.Instance.tiltAngle = angle;

        var targetRotation = rotation + angle * direction * Time.fixedDeltaTime;

        if (targetRotation <= minAngle || targetRotation >= maxAngle)
        {
            if (Time.time - lastImpactSfxTime >= 1f)
            {
                lastImpactSfxTime = Time.time;
                GameManager.Instance.sfx.PlaySoundEffect(
                    GlobalVariables.Instance.platformImpactSfx,
                    GlobalVariables.Instance.platformImpactVolume);
            }

            GameManager.Instance.sfx.StopAudioLoop();
            return;
        }

        rotation = Mathf.Clamp(targetRotation, minAngle, maxAngle);

        GameManager.Instance.sfx.PlayRandomLoopClip(GlobalVariables.Instance.platformRotateVolume);

        var origin = hinge.transform.position;
        var deltaRotation = Quaternion.AngleAxis(angle * direction * Time.fixedDeltaTime, Vector3.forward);

        rb.MovePosition(deltaRotation * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * deltaRotation);

        accelerationTime += Time.fixedDeltaTime;
        previousDirection = direction;
    }

    private void HandleRotationParticles(float direction)
    {
        //if (GlobalVariables.Instance.tiltAngle == 0)
        //{
        //    this.SetParticleSystems(leftParticleSystems, false);
        //    this.SetParticleSystems(rightParticleSystems, false);
        //    return;
        //}

        if (direction == previousDirection) return;

        // tilting left
        if (direction > 0)
        {
            this.SetParticleSystems(leftParticleSystems, true);
            this.SetParticleSystems(rightParticleSystems, false);
        }
        else if (direction < 0)
        {
            this.SetParticleSystems(leftParticleSystems, false);
            this.SetParticleSystems(rightParticleSystems, true);
        }   
    }

    private void SetParticleSystems(ParticleSystem[] particleSystems, bool enablePlaying)
    {
        foreach (ParticleSystem system in particleSystems)
        {
            if (!system.isPlaying && enablePlaying)
            {
                system.Play();
            }
            else if (!enablePlaying)
            {
                system.Stop();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.ReloadLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !this.isBumping)
        {
            this.isBumping = true;
            Camera.main.gameObject.GetComponent<CameraShake>().Bump();

            var bumpableObjects = GameObject.FindGameObjectsWithTag("Horse");
            
            foreach (GameObject bumpableObject in bumpableObjects)
            {
                var temp = bumpableObject.GetComponent<Rigidbody>();
                temp.AddForce(Vector3.up * GlobalVariables.Instance.bumpForce, ForceMode.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        var rotating = false;
        
        if (Input.GetKey(KeyCode.D))
        {
            rotating = true;
            this.HandleRotationParticles(-1f);
            RotatePlatform(-1f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotating = true;
            this.HandleRotationParticles(1f);
            RotatePlatform(1f);
        }
        else
        {
            GameManager.Instance.sfx.StopAudioLoop();
        }

        if (!rotating)
        {
            accelerationTime = 0;
            GlobalVariables.Instance.tiltAngle = 0;
        }

        if (this.isBumping)
        {
            this.timeElapsed += Time.fixedDeltaTime;

            if (this.timeElapsed > GlobalVariables.Instance.bumpCooldown)
            {
                this.isBumping = false;
                this.timeElapsed = 0;
            }
        }
    }
}

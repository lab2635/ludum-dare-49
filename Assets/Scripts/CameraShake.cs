using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform bumpOrigin;
    public Transform bumpDestination;
    public float bumpUpSpeed;
    public float bumpReturnSpeed;
    public float bumpUpDuration;
    public float bumpReturnDuration;

    private float timeElapsed;

    private BumpStatus bumpStatus;

    private enum BumpStatus
    {
        NotBumping,
        BumpingUp,
        Returning
    }

    // Start is called before the first frame update
    void Start()
    {
        this.bumpStatus = BumpStatus.NotBumping;
        this.bumpOrigin.position = this.transform.position;
        this.bumpDestination.position = new Vector3(this.bumpOrigin.position.x, this.bumpOrigin.position.y - 1, this.bumpOrigin.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.bumpStatus == BumpStatus.BumpingUp)
        {
            this.BumpUp();
        }
        else if (this.bumpStatus == BumpStatus.Returning)
        {
            this.BumpReturn();
        }
    }

    public void Bump()
    {
        this.bumpStatus = BumpStatus.BumpingUp;
    }

    void BumpUp()
    {
        if (timeElapsed < bumpUpDuration)
        {
            float step = this.bumpUpSpeed * Time.deltaTime; // calculate distance to move
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.bumpDestination.position, step);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            this.bumpStatus = BumpStatus.Returning;
            this.timeElapsed = 0;
        }
    }

    void BumpReturn()
    {
        if (timeElapsed < bumpReturnDuration)
        {
            float step = this.bumpReturnSpeed * Time.deltaTime; // calculate distance to move
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.bumpOrigin.position, step);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            this.bumpStatus = BumpStatus.NotBumping;
            this.timeElapsed = 0;
        }
    }
}

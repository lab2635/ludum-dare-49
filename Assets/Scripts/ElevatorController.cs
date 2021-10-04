using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : ActuatorBase
{
    public Transform topPosition;
    public Transform bottomPosition;
    public float springSpeed; //200 for elevator, 400 for catapult
    public float dampenSpeed; //50 for elevator, 10 for catapult
    public float maxForce; //100 for elevator, 2000 for catapult
    public bool isCatapult;

    public ElevatorPosition activatedPosition;
    public ElevatorPosition deactivatedPosition;
    public ElevatorPosition startingPosition;

    public enum ElevatorPosition
    {
        Top,
        Bottom,
    }

    private ConfigurableJoint joint;
    private float topY;
    private float bottomY;

    // Start is called before the first frame update
    void Start()
    {
        this.joint = GetComponent<ConfigurableJoint>();

        // connectedAnchor -> joint's position in world space
        // anchor -> joint offset relative to the level

        topY = joint.connectedAnchor.y - topPosition.transform.position.y + joint.anchor.y;
        bottomY = joint.connectedAnchor.y - bottomPosition.transform.position.y + joint.anchor.y;

        JointDrive newYDrive = new JointDrive();
        newYDrive.positionSpring = this.springSpeed;
        newYDrive.positionDamper = this.dampenSpeed;
        newYDrive.maximumForce = this.maxForce;
        this.joint.yDrive = newYDrive;

        if (this.startingPosition == ElevatorPosition.Bottom)
        {
            this.joint.targetPosition = new Vector3(0, bottomY, 0);
        }
        else if (this.startingPosition == ElevatorPosition.Top)
        {
            this.joint.targetPosition = new Vector3(0, topY, 0);
        }
    }

    public override void Actuate()
    {
        if (this.activatedPosition == ElevatorPosition.Bottom)
        {
            this.joint.targetPosition = new Vector3(0, bottomY, 0);
        }
        else if (this.activatedPosition == ElevatorPosition.Top)
        {
            this.joint.targetPosition = new Vector3(0, topY, 0);
        }

        if (this.isCatapult)
        {
            StartCoroutine(this.WaitToReset());
        }
    }

    public override void ResetActuator()
    {
        if (this.deactivatedPosition == ElevatorPosition.Bottom)
        {
            this.joint.targetPosition = new Vector3(0, bottomY, 0);
        }
        else if (this.deactivatedPosition == ElevatorPosition.Top)
        {
            this.joint.targetPosition = new Vector3(0, topY, 0);
        }
    }

    private IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(0.3f);
        this.ResetActuator();
    }
}

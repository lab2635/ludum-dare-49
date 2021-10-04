using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class DoorController : ActuatorBase
{
    public DoorStatus actuateAction;
    public DoorStatus resetAction;
    public DoorStatus currentStatus;
    public enum DoorStatus
    {
        Closed,
        Open,
    }

    public HingeJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        if (this.currentStatus == DoorStatus.Closed)
        {
            this.joint.useMotor = false;
        }
        else if (this.currentStatus == DoorStatus.Open)
        {
            this.joint.useMotor = true;
        }
    }

    public override void Actuate()
    {
        if (this.actuateAction == DoorStatus.Closed)
        {
            this.currentStatus = DoorStatus.Closed;
            this.joint.useMotor = false;
        }
        else if (this.actuateAction == DoorStatus.Open)
        {
            this.currentStatus = DoorStatus.Open;
            this.joint.useMotor = true;
        }
    }

    public override void ResetActuator()
    {
        if (this.resetAction == DoorStatus.Closed)
        {
            this.currentStatus = DoorStatus.Closed;
            this.joint.useMotor = false;
        }
        else if (this.resetAction == DoorStatus.Open)
        {
            this.currentStatus = DoorStatus.Open;
            this.joint.useMotor = true;
        }
    }
}

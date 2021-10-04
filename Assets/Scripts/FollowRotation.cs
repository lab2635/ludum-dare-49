using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    //private GameObject levelHinge;
    //private float previousDirection;
    //private float rotation;
    //private Rigidbody[] rbs => this.gameObject.GetComponentsInChildren<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody levelRigidbody = GameObject.FindGameObjectWithTag("LevelRigidbody").GetComponent<Rigidbody>();
        ConfigurableJoint[] childConfigJoints = this.gameObject.GetComponentsInChildren<ConfigurableJoint>();
        FixedJoint[] childFixedJoints = this.gameObject.GetComponentsInChildren<FixedJoint>();

        foreach (ConfigurableJoint configJoint in childConfigJoints)
        {
            configJoint.connectedBody = levelRigidbody;
        }

        foreach (FixedJoint fixedJoint in childFixedJoints)
        {
            fixedJoint.connectedBody = levelRigidbody;
        }

        //this.levelHinge = GameObject.FindGameObjectWithTag("LevelHinge");
        //this.previousDirection = 1;
    }

    //private void FixedUpdate()
    //{
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        BuildingRotation(-1f);
    //    }
    //    else if (Input.GetKey(KeyCode.A))
    //    {
    //        BuildingRotation(1f);
    //    }
    //}

    //private void BuildingRotation(float direction)
    //{
    //    float angle = GlobalVariables.Instance.tiltAngle;
    //    var minAngle = GlobalVariables.Instance.minTiltAngle;
    //    var maxAngle = GlobalVariables.Instance.maxTiltAngle;

    //    if (this.previousDirection != direction) angle = 0;
    //    this.previousDirection = direction;

    //    if (angle == 0) return;

    //    rotation += angle * direction * Time.fixedDeltaTime;
    //    rotation = Mathf.Clamp(rotation, minAngle, maxAngle);

    //    if (rotation <= minAngle && direction < 0) return;
    //    if (rotation >= maxAngle && direction > 0) return;

    //    var origin = levelHinge.transform.position;
    //    var q = Quaternion.AngleAxis(angle * direction * Time.fixedDeltaTime, Vector3.forward);

    //    foreach (Rigidbody rb in this.rbs)
    //    {
    //        if (rb.gameObject.name != "Door")
    //        {
    //            rb.MovePosition(q * (rb.transform.position - origin) + origin);
    //            rb.MoveRotation(rb.transform.rotation * q);
    //        }
    //    }
    //}
}

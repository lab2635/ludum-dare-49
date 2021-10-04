using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActuatorBase : MonoBehaviour
{
    public abstract void Actuate();
    public abstract void ResetActuator();
}

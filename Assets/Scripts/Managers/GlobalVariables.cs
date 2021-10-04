using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class GlobalVariables : SingletonBehaviour<GlobalVariables>
{
    public float bumpCooldown;
    public float bumpForce;
    public float maxTiltAngle;
    public float minTiltAngle;
    public float tiltSpeed;
    public float tiltAngle;
    public Vector3 defaultCameraPosition;
    public AnimationCurve tiltAccelerationCurve;
    public float cameraMaxY;
    public float cameraMinY;

    [Header("Audio")]
    public AudioClip[] buildingRotateSfx;
    public AudioClip platformRotateSfx;
    public float platformRotateVolume = 0.5f;
    public AudioClip platformImpactSfx;
    public float platformImpactVolume = 0.5f;
}

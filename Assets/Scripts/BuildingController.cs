using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingController : MonoBehaviour
{
    public GameObject levelParts;

    private Vector3 currentAngle;
    private bool isBumping;
    private GameObject[] bumpableObjects;
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        this.currentAngle = this.levelParts.transform.eulerAngles;
        //this.currentAngle = this.transform.eulerAngles;
        this.isBumping = false;
        this.bumpableObjects = GameObject.FindGameObjectsWithTag("Horse");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //currentAngle = this.levelParts.transform.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(
                currentAngle.x,
                currentAngle.y,
                Mathf.Clamp(currentAngle.z + GlobalVariables.Instance.tiltSpeed, GlobalVariables.Instance.minTiltAngle, GlobalVariables.Instance.maxTiltAngle));
            this.levelParts.transform.rotation = Quaternion.RotateTowards(
                this.levelParts.transform.rotation, targetRotation, Time.deltaTime * GlobalVariables.Instance.tiltSpeed);

            //this.transform.rotation = Quaternion.RotateTowards(
            //    this.transform.rotation, targetRotation, Time.deltaTime * GlobalVariables.Instance.tiltSpeed);

            //this.levelParts.GetComponent<Rigidbody>().AddTorque(this.levelParts.transform.forward * GlobalVariables.Instance.tiltSpeed);
            //this.levelParts.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //this.levelParts.GetComponent<Rigidbody>().AddTorque(new Vector3(0,0,-1) * GlobalVariables.Instance.tiltSpeed);

            //targetRotation = Quaternion.Euler(currentAngle.x, currentAngle.y, currentAngle.z + GlobalVariables.Instance.tiltSpeed);
            //targetRotation = Quaternion.Euler(new Vector3(0,0,-1).normalized * GlobalVariables.Instance.tiltSpeed);
            //this.levelParts.GetComponent<Rigidbody>().MoveRotation(this.levelParts.GetComponent<Rigidbody>().rotation * targetRotation);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Quaternion targetRotation = Quaternion.Euler(
                currentAngle.x,
                currentAngle.y,
                Mathf.Clamp(currentAngle.z - GlobalVariables.Instance.tiltSpeed, GlobalVariables.Instance.minTiltAngle, GlobalVariables.Instance.maxTiltAngle));
            this.levelParts.transform.rotation = Quaternion.RotateTowards(
                this.levelParts.transform.rotation, targetRotation, Time.deltaTime * GlobalVariables.Instance.tiltSpeed);
            //this.transform.rotation = Quaternion.RotateTowards(
            //    this.transform.rotation, targetRotation, Time.deltaTime * GlobalVariables.Instance.tiltSpeed);

            //this.levelParts.GetComponent<Rigidbody>().AddTorque(this.levelParts.transform.forward * GlobalVariables.Instance.tiltSpeed * -1);

            //targetRotation = Quaternion.Euler(currentAngle.x, currentAngle.y, currentAngle.z - GlobalVariables.Instance.tiltSpeed);
            //this.levelParts.GetComponent<Rigidbody>().MoveRotation(this.levelParts.transform.rotation * targetRotation);

            //targetRotation = Quaternion.Euler(0, 0, GlobalVariables.Instance.tiltSpeed * Time.fixedDeltaTime);
            //this.levelParts.GetComponent<Rigidbody>().MoveRotation(this.levelParts.GetComponent<Rigidbody>().rotation * targetRotation);
        }

        //if (this.levelParts.transform.rotation.eulerAngles.z < GlobalVariables.Instance.maxTiltAngle)
        //{
        //    this.levelParts.transform.rotation = Quaternion.Euler(0, 0, GlobalVariables.Instance.maxTiltAngle);
        //}
        //else if (this.levelParts.transform.rotation.eulerAngles.z < 360 + GlobalVariables.Instance.minTiltAngle
        //    && this.levelParts.transform.rotation.eulerAngles.z > GlobalVariables.Instance.maxTiltAngle)
        //{
        //    this.levelParts.transform.rotation = Quaternion.Euler(0, 0, GlobalVariables.Instance.minTiltAngle);
        //}

        if (Input.GetKeyDown(KeyCode.Space) && !this.isBumping)
        {
            this.isBumping = true;
            Camera.main.gameObject.GetComponent<CameraShake>().Bump();

            foreach (GameObject bumpableObject in this.bumpableObjects)
            {
                var temp = bumpableObject.GetComponent<Rigidbody>();
                temp.AddForce(Vector3.up * GlobalVariables.Instance.bumpForce, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
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

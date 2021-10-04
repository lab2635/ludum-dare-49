using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPanController : MonoBehaviour
{
    public float cameraSpeed = 1f;

    private bool panningUp;
    private bool panningDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool IsOnTitleScreen()
    {
        var scene = SceneManager.GetActiveScene().name;
        return scene == "TitleScreen";
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !IsOnTitleScreen())
        {
            panningUp = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) && !IsOnTitleScreen())
        {
            panningDown = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            panningUp = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            panningDown = false;
        }
    }

    private void FixedUpdate()
    {
        GameObject cameraObj = Camera.main.gameObject.transform.parent.gameObject;
        Vector3 currentPos = cameraObj.transform.localPosition;
        if (panningUp && currentPos.y < GlobalVariables.Instance.cameraMaxY)
        {
            cameraObj.transform.position += new Vector3(0, cameraSpeed * Time.fixedDeltaTime, 0);
        }
        else if (panningDown && currentPos.y > GlobalVariables.Instance.cameraMinY)
        {
            cameraObj.transform.position -= new Vector3(0, cameraSpeed * Time.fixedDeltaTime, 0);
        }
    }
}

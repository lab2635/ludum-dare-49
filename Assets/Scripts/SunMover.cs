using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SunMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(
            this.transform.eulerAngles.x,
            -244 + 4 * SceneManager.GetActiveScene().buildIndex,
            this.transform.eulerAngles.z);
    }
}

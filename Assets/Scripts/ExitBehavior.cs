using System.Collections;
using UnityEngine;

public class ExitBehavior : MonoBehaviour
{
    public AudioClip exitSound;
    public Transform emitterLocation;
    public GameObject exitParticleEmitterPrefab;
    
    private int remainingHorses = -1;

    // Start is called before the first frame update
    void Start()
    {
        this.remainingHorses = GameObject.FindGameObjectsWithTag("Horse").Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Horse"))
        {
            // Dont allow dead horses to count as a success
            var behavior = other.gameObject.GetComponent<HorseBehavior>();
            if (behavior.Active == false) return;
            
            GameObject emitter = GameObject.Instantiate(exitParticleEmitterPrefab);
            emitter.transform.position = other.transform.position;

            other.gameObject.SetActive(false);
            remainingHorses--;
            GameManager.Instance.sfx.PlaySoundEffect(exitSound, 0.5f);

            if (remainingHorses == 0)
            {
                var menuObject = GameObject.FindWithTag("MainMenu");
                var menu = menuObject.GetComponent<MainMenu>();
                menu.ShowLevelCompletePopup();
            }
        }
    }
}

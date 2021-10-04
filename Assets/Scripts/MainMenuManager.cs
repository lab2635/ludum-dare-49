using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject endButton;
    public GameObject[] CreditsInExplosionOrder; // Asimir, Zyr, Euix, Rein
    public HorseBehavior AsimirHorse;
    public GameObject titleScreenCanvas;

    public void LevelSelect()
    {
        titleScreenCanvas.SetActive(false);

        MainMenu.Instance.ShowLevelSelect();
        MainMenu.Instance.PopupClosed += OnPopupClosed;
    }

    private void OnPopupClosed(object sender, string popupName)
    {
        MainMenu.Instance.PopupClosed -= OnPopupClosed;
        
        if (popupName == "LevelSelect")
        {
            titleScreenCanvas.SetActive(true);
        }
    }
    
    
    public void StartGame()
    {
        // Remove the start button
        titleScreenCanvas.SetActive(false);

        // Drop the horse
        MagnetController magnet = GameObject.FindGameObjectWithTag("MainMenuMagnet")?.GetComponent<MagnetController>();
        if (magnet)
        {
            magnet.SetMagnetStatus(false);
        }

        // Wait a bit and then start the game
        StartCoroutine(DelayedStart(4.5f));
    }

    public void EndGame()
    {
        endButton.SetActive(false);

        // blow up asimir
        AsimirHorse.Explode();
        StartCoroutine(RemoveCredit(0));
    }

    private IEnumerator RemoveCredit(int creditIndex)
    {
        yield return new WaitForSeconds(1f);
        CreditsInExplosionOrder[creditIndex].SetActive(false);

        if (creditIndex == CreditsInExplosionOrder.Length - 1)
        {
            StartCoroutine(DelayedEnd(2));
        }
        else
        {
            StartCoroutine(RemoveCredit(++creditIndex));
        }
    }

    private IEnumerator DelayedStart(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("LoreScene");
    }

    private IEnumerator DelayedEnd(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("TitleScene");
    }
}

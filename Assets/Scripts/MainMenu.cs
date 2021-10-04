using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject levelCompletePopup;
    public GameObject menuPopup;
    public GameObject resetButton;
    public GameObject menuButton;
    public GameObject lorePage;
    public Image logoImage;
    public Image overlayImage;

    [Header("Audio")]
    public AudioClip unlockSfx;
    public AudioClip gameStartSfx;

    private bool cheatsEnabled;
    private float checkTimer;
    
    public static MainMenu Instance => GameObject.FindWithTag("MainMenu").GetComponent<MainMenu>();

    public event EventHandler<string> PopupClosed;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        overlayImage.CrossFadeAlpha(0.0f, 0.0f, true);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        var logoPosition = new Vector3(0f, 6f, 0f);

        logoImage.gameObject.SetActive(true);
        logoImage.DOFade(0.0f, 0.0f);
        logoImage.DOFade(1.0f, 3.0f);
        logoImage.transform.DOLocalMove(logoPosition, 2.0f).OnStepComplete(() =>
        {
            GameManager.Instance.sfx.PlaySoundEffect(gameStartSfx, 0.25f);
        });
        logoImage.DOFade(0.0f, 2.0f).SetDelay(4.0f).OnStepComplete(() =>
        {
            logoImage.gameObject.SetActive(false);
        });

        DOTween.Play(logoImage);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        menuButton.SetActive(true);
        overlayImage.CrossFadeAlpha(1f, 0f, true);
        overlayImage.CrossFadeAlpha(0f, 2f, true);

        if (scene.name == "LoreScene")
        {
            lorePage.SetActive(true);
            menuButton.SetActive(false);
        }
    }
    
    private void OnSceneUnloaded(Scene scene)
    {
        // do nothing
    }
    
    public void NotifyPopupClosed(string popupName)
    {
        PopupClosed?.Invoke(this, popupName);
        menuButton.SetActive(true);
    }
    
    public void ShowLevelCompletePopup()
    {
        menuButton.SetActive(false);
        ShowScreen(levelCompletePopup);
    }

    public void ShowLevelSelect()
    {
        menuButton.SetActive(false);
        ShowScreen(levelSelect);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))
        {
            if (menuPopup.activeSelf) return;
            ShowScreen(menuPopup);
        }

        if (Input.GetKeyDown(KeyCode.Backslash) && !cheatsEnabled)
        {
            cheatsEnabled = true;
            GameManager.Instance.sfx.PlaySoundEffect(unlockSfx);
            GameManager.Instance.UnlockAllLevels();
        }

        if (GameManager.Instance.isDebugMode && Input.GetKeyDown(KeyCode.Tab))
        {
            ShowLevelCompletePopup();
        }   

        checkTimer += Time.deltaTime;
        
        if (checkTimer >= 5f)
        {
            checkTimer = 0f;
            ShowResetButton();
        }
    }

    private void ShowScreen(GameObject screen)
    {
        screen.SetActive(true);
    }
    
    public void ShowMenuPopup()
    {
        menuPopup.SetActive(true);
        menuButton.SetActive(false);
    }
    
    public void ShowResetButton()
    {
        resetButton.SetActive(true);
    }

    public void ResetLevel()
    {
        resetButton.SetActive(false);
        GameManager.Instance.ReloadLevel();
    }

    public void ClickLore()
    {
        lorePage.SetActive(false);
        SceneManager.LoadScene("Level1");
    }
}

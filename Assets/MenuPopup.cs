using UnityEngine;

public class MenuPopup : MonoBehaviour
{
    public void OnTitleScreenClicked()
    {
        Close();
        GameManager.Instance.ShowTitleScreen();
    }

    public void OnLevelSelectClicked()
    {
        Close();
        MainMenu.Instance.ShowLevelSelect();
    }

    public void OnExitClicked()
    {
        Close();
        Application.Quit();
    }

    public void OnRestartClicked()
    {
        Close();
        GameManager.Instance.ReloadLevel();
    }

    public void Close()
    {
        MainMenu.Instance.NotifyPopupClosed("MenuPopup");
        gameObject.SetActive(false);
    }
}

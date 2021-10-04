using UnityEngine;

public class MenuLevelSelect : MonoBehaviour
{
    public void OnClickClose()
    {
        gameObject.SetActive(false);
        MainMenu.Instance.NotifyPopupClosed("LevelSelect");
    }
}

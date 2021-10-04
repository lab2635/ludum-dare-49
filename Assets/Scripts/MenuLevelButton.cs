using System;
using TMPro;
using UnityEngine;

public class MenuLevelButton : MonoBehaviour
{
    public int levelNumber;
    public Sprite completeSprite;
    public GameObject menuObject;
    
    [Header("Audio")]
    public AudioClip clickSfx;
    public AudioClip negativeSfx;

    private string levelName;

    private void OnEnable()
    {
        levelName = string.Format("Level{0}", levelNumber);

        var label = GetComponentInChildren<TMP_Text>();
        label.text = levelNumber.ToString();

        if (IsCompleted())
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            image.sprite = completeSprite;
        }
    }

    private bool IsCompleted()
    {
        return GameManager.Instance.IsLevelComplete(levelName);
    }

    public void OnButtonClicked()
    {
        if (IsCompleted())
        {
            menuObject.SetActive(false);
            GameManager.Instance.sfx.PlaySoundEffect(clickSfx);
            GameManager.Instance.StartLevel(levelName);
        }
        else
        {
            GameManager.Instance.sfx.PlaySoundEffect(negativeSfx);
        }
    }
}

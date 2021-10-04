using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip clickSfx;
    
    public void OnButtonClick()
    {
        GameManager.Instance.sfx.PlaySoundEffect(clickSfx);
    }
}

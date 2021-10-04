using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : SingletonBehaviour<BGMPlayer>
{
    public AudioClip[] bgmList;
    public AudioSource audioSource;

    public bool playBGM;

    // Start is called before the first frame update
    void Start()
    {
        this.playBGM = false;
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        while (this.playBGM && this.audioSource && this.bgmList.Length > 0)
        {
            int index = Random.Range(0, this.bgmList.Length - 1);
            audioSource.PlayOneShot(bgmList[index]);
            while (audioSource.isPlaying)
                yield return null;
        }
    }

    public void PlayBGM()
    {
        this.playBGM = true;
        StartCoroutine(PlayMusic());
    }

    public void StopBGM()
    {
        this.playBGM = false;
    }
}

using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffects : MonoBehaviour
{
    private AudioSource source;
    private AudioSource loopSource;
    private float loopFadeTime;
    private bool loopFade;
    private RandomAudioClip randomLoopClip;

    void Start()
    {
        source = CreateAudioSource();
        loopSource = CreateAudioSource();
        loopSource.loop = false;
        randomLoopClip = new RandomAudioClip(GlobalVariables.Instance.buildingRotateSfx);
    }

    AudioSource CreateAudioSource()
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        return audioSource;
    }

    private void Update()
    {
        const float loopFadeDuration = 0.25f;

        if (!loopFade) return;
        
        loopFadeTime += Time.deltaTime;
        loopSource.volume = 1f - (loopFadeTime / loopFadeDuration);

        if (loopFadeTime >= loopFadeDuration)
        {
            StopAudioLoop(false);
        }
    }

    public void PlayRandomLoopClip(float volume = 1f)
    {
        if (loopSource.isPlaying) return;

        loopFade = false;
        loopFadeTime = 0f;
        loopSource.Stop();
        loopSource.volume = volume;
        loopSource.clip = randomLoopClip.GetRandomAudioClip();
        loopSource.time = 0f;
        loopSource.Play();
    }

    public void PlayAudioLoop(AudioClip clip, float volume = 1f)
    {
        if (loopSource.isPlaying) return;

        loopFade = false;
        loopFadeTime = 0f;
        loopSource.Stop();
        loopSource.volume = volume;
        loopSource.clip = clip;
        loopSource.time = 0f;
        loopSource.Play();
    }

    public void StopAudioLoop(bool fade = true)
    {
        switch (fade)
        {
            case true when !loopFade:
                loopFade = true;
                loopFadeTime = 0f;
                break;
            case false:
                loopFade = false;
                loopFadeTime = 0f;
                loopSource.Stop();
                break;
        }
    }
    
    public void PlayRandomSoundEffect(float volumeScale, params AudioClip[] clips)
    {
        var value = Random.Range(0f, clips.Length);
        var index = Mathf.FloorToInt(value);
        var clip = clips[index];
        PlaySoundEffect(clip, volumeScale);
    }
    
    public void PlaySoundEffect(AudioClip clip, float volumeScale = 1f)
    {
        source.PlayOneShot(clip, volumeScale);
    }
}

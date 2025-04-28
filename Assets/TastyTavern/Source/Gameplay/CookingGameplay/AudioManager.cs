using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // audio sources
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    // SFX
    [SerializeField] private AudioClip frySFX;
    [SerializeField] private AudioClip boilSFX;
    [SerializeField] private AudioClip ovenSFX;
    [SerializeField] private AudioClip grillSFX;
    [SerializeField] private AudioClip cutSFX;
    [SerializeField] private AudioClip mixSFX;
    [SerializeField] private AudioClip deepFrySFX;
    [SerializeField] private AudioClip satisfiedSFX;
    [SerializeField] private AudioClip dissatisfiedSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip mainThemeSFX;
    [SerializeField] private AudioClip forestThemeSFX;
    [SerializeField] private AudioClip oceanThemeSFX;
    [SerializeField] private AudioClip cavesThemeSFX;


    private Dictionary<string, AudioClip> sfxMap;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxMap = new Dictionary<string, AudioClip>
        {
            
        };

        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string sfxName)
    {
        if (sfxMap.TryGetValue(sfxName.ToLower(), out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxName}' not found in AudioPlayer.");
        }
    }
}
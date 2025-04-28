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
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip forestThemeLoop;
    [SerializeField] private AudioClip oceanThemeLoop;
    [SerializeField] private AudioClip oceanThemeBeginning;
    [SerializeField] private AudioClip cavesThemeLoop;
    [SerializeField] private AudioClip cavesThemeBeginning;
    [SerializeField] private AudioClip backgroundAudioLoop;
    [SerializeField] private AudioClip order;


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
        DontDestroyOnLoad(this);

        sfxMap = new Dictionary<string, AudioClip>
        {
            {"Frying", frySFX},
            {"Boiling", boilSFX},
            {"Oven", ovenSFX},
            {"Grilling", grillSFX},
            {"Cutting", cutSFX},
            {"Mixing", mixSFX},
            {"Deep Frying", deepFrySFX},
            {"Satisfied", satisfiedSFX},
            {"Dissatisfied", dissatisfiedSFX},
            {"ButtonClick", buttonClickSFX},
            {"ForestThemeLoop", forestThemeLoop},
            {"OceanThemeLoop", oceanThemeLoop},
            {"OceanThemeBeginning", oceanThemeBeginning},
            {"CavesTheme", cavesThemeLoop},
            {"CavesThemeBeginning", cavesThemeBeginning},
            {"MainTheme", mainTheme},
            {"Background", backgroundAudioLoop},
        };
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource != null && !bgmSource.isPlaying && clip != null)
        {
            bgmSource.loop = true;
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
    
    public void PlayBGM(string bgmName)
    {
        if (sfxMap.TryGetValue(bgmName, out AudioClip clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{bgmName}' not found in AudioPlayer.");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string sfxName)
    {
        if (sfxMap.TryGetValue(sfxName, out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxName}' not found in AudioPlayer.");
        }
    }
    
    
}
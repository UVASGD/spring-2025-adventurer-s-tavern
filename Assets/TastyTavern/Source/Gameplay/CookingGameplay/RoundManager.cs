using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoundManager : MonoBehaviour
{

    private int _day;
    private int _moneyAccumulatedThisRound;
    private Enum _grade;
    private int _customersServed;
    
    /// <summary>
    /// A dict of served orders, whose values represent if the order was cooked correctly or incorrectly
    /// </summary>
    private Dictionary<Order, bool> _servedOrders = new Dictionary<Order, bool>();

    [SerializeField] private int customersToPass;
    
    [SerializeField] private CookingUIEventChannel eventChannel;
    
    public BiomeData currentBiome;
    
    [SerializeField] private PlayerManager playerManager;
    
    [SerializeField] private OrderManager orderManager;
    
    private bool isIntroPlaying = true;
    private AudioManager audioManager;
    private AudioSource audioSource;
    private string introClipName;
    private string loopClipName;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _day = 0;
        _customersServed = 0;
        
        currentBiome = playerManager.currentBiome;
        
        audioManager = AudioManager.Instance;
        audioSource = audioManager.bgmSource;

        switch (currentBiome.Name)
        {
            case "Riko Wilds":
                introClipName = "ForestThemeLoop";
                loopClipName = "ForestThemeLoop";
                break;
            case "Pipawpwa Waves":
                introClipName = "OceanThemeBeginning";
                loopClipName = "OceanThemeLoop";
                break;
            case "Mungtown Caves":
                introClipName = "CavesThemeBeginning";
                loopClipName = "CavesThemeLoop";
                break;
        }

        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: customerstopass will probably increase for each level
        if (_customersServed >= customersToPass)
        {
            FinishDay();
            _customersServed = 0; 
        }
        if (isIntroPlaying && !audioSource.isPlaying)
        {
            PlayLoop();
        }
        
    }

    private void OnEnable()
    {
        eventChannel.OnDayStarted += StartNewDay;
        eventChannel.OnChangePlayerMoney += ChangeMoney;
        eventChannel.OnSubmitOrder += IncrementCustomersServed;
    }

    private void OnDisable()
    {
        eventChannel.OnDayStarted -= StartNewDay;
        eventChannel.OnChangePlayerMoney -= ChangeMoney;
        eventChannel.OnSubmitOrder -= IncrementCustomersServed;
    }
    
    private void PlayIntro()
    {
        isIntroPlaying = true;
        audioSource.loop = false; // don't loop the intro
        audioManager.PlaySFX(introClipName);
    }

    private void PlayLoop()
    {
        isIntroPlaying = false;
        audioSource.loop = true; // now loop
        audioManager.PlayBGM(loopClipName);
    }

    // Don't need this int input for now, but it's here just in case
    private void StartNewDay(int x)
    {
        _day++;
        _moneyAccumulatedThisRound = 0;
        _customersServed = 0;
    }
    
    private void FinishDay()
    {
        eventChannel.RaiseOnDayFinished(_day);
        
        playerManager.moneyAccumulatedThisRound = _moneyAccumulatedThisRound;
        playerManager.customersServed = _customersServed;
        
        audioManager.bgmSource.Stop();
        audioManager.sfxSource.Stop();
        
        Debug.Log("Day Finished");
        playerManager.SavePlayer();
        SceneManager.LoadScene("EndOfDayView");
    }

    private void ChangeMoney(int deltaMoney)
    {
        _moneyAccumulatedThisRound += deltaMoney;
    }

    private void IncrementCustomersServed(Order order)
    {
        _customersServed++;
    }
}
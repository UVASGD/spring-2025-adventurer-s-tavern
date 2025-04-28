using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIMenusProcesses : MonoBehaviour
{
    public UIDocument postGameUI;
    public UIDocument shopMenuUI;
    public UIDocument biomesMenuUI;

    public VisualElement postGameUIroot;
    public VisualElement shopMenuUIroot;
    public VisualElement biomesMenuUIroot;

    public VisualElement statsPanel;

    public Button NextDayButton;
    public Button ShopButton;
    public Button BiomesButton;
    public Button ShopBackButton;

    public Button SelectForestButton;
    public Button SelectCavesButton;
    public Button SelectOceanButton;
    public Button ExitBiomeMenuButton;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private GameObject shopKeepsObj;
    
    private AudioManager _audioManager;
    private bool isIntroPlaying = true;
    private AudioSource audioSource;
    private string introClipName;
    private string loopClipName;


    void Awake()
    {
        postGameUIroot = postGameUI.GetComponent<UIDocument>().rootVisualElement;
        shopMenuUIroot = shopMenuUI.GetComponent<UIDocument>().rootVisualElement;
        biomesMenuUIroot = biomesMenuUI.GetComponent<UIDocument>().rootVisualElement;
        
        statsPanel = postGameUIroot.Q<VisualElement>("StatsPanel");
        
        
        NextDayButton = postGameUIroot.Q<Button>("Continue");
        ShopButton = postGameUIroot.Q<Button>("Shop");
        BiomesButton = postGameUIroot.Q<Button>("Biomes");
        ShopBackButton = shopMenuUIroot.Q<Button>("BackButton");

        SelectForestButton = biomesMenuUIroot.Q<Button>("ForestSelect");
        SelectCavesButton = biomesMenuUIroot.Q<Button>("CavesSelect");
        SelectOceanButton = biomesMenuUIroot.Q<Button>("OceanSelect");
        ExitBiomeMenuButton = biomesMenuUIroot.Q<Button>("ExitBiomeMenu");
        
        playerManager.LoadPlayer();
        
        _audioManager = AudioManager.Instance;


        // TODO: Add shop menu exit button and subscribe to SwitchToPostGameMenu

        // TODO: Read a JSON File that stores which biomes are unlocked and disable those panels
    }

    void Start()
    {
        ShopButton.clicked += SwitchToShopMenu;
        BiomesButton.clicked += SwitchToBiomeMenu;
        NextDayButton.clicked += GoToNextDay;

        SelectForestButton.clicked += SelectBiome1;
        SelectCavesButton.clicked += SelectBiome2;
        SelectOceanButton.clicked += SelectBiome3;

        ExitBiomeMenuButton.clicked += SwitchToPostGameMenu;

        ShopBackButton.clicked += SwitchToPostGameMenu;
        
        _audioManager.PlayBGM("MainTheme");
        
        SwitchToPostGameMenu();
        GenerateStats();
        RefreshBiomeButtons();
    }

    private void RefreshBiomeButtons()
    {
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[0]])
            SelectForestButton.SetEnabled(false);
        else
            SelectForestButton.SetEnabled(true);
        
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[1]])
            SelectOceanButton.SetEnabled(false);
        else
            SelectOceanButton.SetEnabled(true);
        
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[2]])
            SelectCavesButton.SetEnabled(false);
        else
            SelectCavesButton.SetEnabled(true);
    }
    
    private void GenerateStats()
    {
        Label statsLabel = statsPanel.ElementAt(0) as Label;
        statsLabel.text = "Biome Played: " + playerManager.currentBiome.Name + 
            "\nMoney Earned Today: " + playerManager.moneyAccumulatedThisRound
            + "\nCustomers Served Today: " + playerManager.customersServed;
    }
    
    private void SwitchToPostGameMenu()
    {
        _audioManager.PlaySFX("ButtonClick");
        postGameUIroot.visible = true;
        postGameUI.sortingOrder = 1;
        shopMenuUIroot.visible = false;
        shopMenuUI.sortingOrder = 0;
        biomesMenuUIroot.visible = false;
        biomesMenuUI.sortingOrder = 0;
        shopKeepsObj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        shopKeepsObj.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        shopKeepsObj.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SwitchToShopMenu()
    {
        _audioManager.PlaySFX("ButtonClick");
        postGameUIroot.visible = false;
        postGameUI.sortingOrder = 0;
        shopMenuUIroot.visible = true;
        shopMenuUI.sortingOrder = 1;
        biomesMenuUIroot.visible = false;
        biomesMenuUI.sortingOrder = 0;
        shopManager.refreshShopView();
        
        switch (playerManager.currentBiome.Name)
        {
            case "Riko Wilds":
                shopKeepsObj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case "Pipawpwa Waves":
                shopKeepsObj.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case "Mungtown Caves":
                shopKeepsObj.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                break;
        }
        
    }

    private void SwitchToBiomeMenu()
    {
        _audioManager.PlaySFX("ButtonClick");
        RefreshBiomeButtons();
        postGameUIroot.visible = false;
        postGameUI.sortingOrder = 0;
        shopMenuUIroot.visible = false;
        shopMenuUI.sortingOrder = 0;
        biomesMenuUIroot.visible = true;
        biomesMenuUI.sortingOrder = 1;
    }
    private void SelectBiome1()
    {
        SwitchToBiome(1);
    }
    private void SelectBiome2()
    {
        SwitchToBiome(2);
    }
    private void SelectBiome3()
    {
        SwitchToBiome(3);
    }

    private void SwitchToBiome(int b) 
    {
        switch(b)
        {
            case 1:
                playerManager.currentBiome = playerManager.allBiome[0];
                break;
            case 2:
                playerManager.currentBiome = playerManager.allBiome[1];
                break;
            case 3:
                playerManager.currentBiome = playerManager.allBiome[2];
                break;
            default:
                break;
            
        }
        playerManager.SavePlayer();// PLEASE SET BIOME TO SWITCH TO :)
        SwitchToPostGameMenu();
    }
    
    private void GoToNextDay()
    {
        _audioManager.bgmSource.Stop();
        _audioManager.sfxSource.Stop();
        playerManager.SavePlayer();
        SceneManager.LoadScene("SpriteStackingTest");

    }
}

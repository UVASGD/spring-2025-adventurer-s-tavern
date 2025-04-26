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

    public Button NextDayButton;
    public Button ShopButton;
    public Button BiomesButton;

    public Button SelectForestButton;
    public Button SelectCavesButton;
    public Button SelectOceanButton;
    public Button ExitBiomeMenuButton;

    [SerializeField] private PlayerManager playerManager;

    void Awake()
    {
        postGameUIroot = postGameUI.GetComponent<UIDocument>().rootVisualElement;
        shopMenuUIroot = shopMenuUI.GetComponent<UIDocument>().rootVisualElement;
        biomesMenuUIroot = biomesMenuUI.GetComponent<UIDocument>().rootVisualElement;

        SwitchToPostGameMenu();

        NextDayButton = postGameUIroot.Q<Button>("Continue");
        ShopButton = postGameUIroot.Q<Button>("Shop");
        BiomesButton = postGameUIroot.Q<Button>("Biomes");

        SelectForestButton = biomesMenuUIroot.Q<Button>("ForestSelect");
        SelectCavesButton = biomesMenuUIroot.Q<Button>("CavesSelect");
        SelectOceanButton = biomesMenuUIroot.Q<Button>("OceanSelect");
        ExitBiomeMenuButton = biomesMenuUIroot.Q<Button>("ExitBiomeMenu");
        // TODO: Add shop menu exit button and subscribe to SwitchToPostGameMenu

        // TODO: Read a JSON File that stores which biomes are unlocked and disable those panels

        ShopButton.clicked += SwitchToShopMenu;
        BiomesButton.clicked += SwitchToBiomeMenu;

        SelectForestButton.clicked += SelectBiome1;
        SelectCavesButton.clicked += SelectBiome2;
        SelectOceanButton.clicked += SelectBiome3;

        ExitBiomeMenuButton.clicked += SwitchToPostGameMenu;
    }

    void Start()
    {
        playerManager.LoadPlayer();
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[0]])
        {
            SelectForestButton.SetEnabled(false);
        }
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[1]])
        {
            SelectCavesButton.SetEnabled(false);
        }
        if (!playerManager.BiomeUnlocked[playerManager.allBiome[2]])
        {
            SelectOceanButton.SetEnabled(false);
        }
    }
    
    private void SwitchToPostGameMenu()
    {
        postGameUIroot.visible = true;
        shopMenuUIroot.visible = false;
        biomesMenuUIroot.visible = false;
    }

    private void SwitchToShopMenu()
    {
        postGameUIroot.visible = false;
        shopMenuUIroot.visible = true;
        biomesMenuUIroot.visible = false;
    }

    private void SwitchToBiomeMenu()
    {
        postGameUIroot.visible = false;
        shopMenuUIroot.visible = false;
        biomesMenuUIroot.visible = true;
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
}

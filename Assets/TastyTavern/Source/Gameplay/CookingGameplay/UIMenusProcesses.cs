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

    public VisualElement Biome1ButtonContainer;
    public VisualElement Biome2ButtonContainer;
    public VisualElement Biome3ButtonContainer;
    void Awake()
    {
        postGameUIroot = postGameUI.GetComponent<UIDocument>().rootVisualElement;
        shopMenuUIroot = shopMenuUI.GetComponent<UIDocument>().rootVisualElement;
        biomesMenuUIroot = biomesMenuUI.GetComponent<UIDocument>().rootVisualElement;
        postGameUI.enabled = true;
        shopMenuUI.enabled = false;
        biomesMenuUI.enabled = false;

        Biome1ButtonContainer = biomesMenuUIroot.Q<VisualElement>("Biome1ButtonPanel");
        Biome2ButtonContainer = biomesMenuUIroot.Q<VisualElement>("Biome2ButtonPanel");
        Biome3ButtonContainer = biomesMenuUIroot.Q<VisualElement>("Biome3ButtonPanel");

        Biome1ButtonContainer.Clear();
        Biome2ButtonContainer.Clear();
        Biome3ButtonContainer.Clear();
    }

    private void LoadBiomeSelectionUI()
    {
        Biome1ButtonContainer.Clear();
        Biome2ButtonContainer.Clear();
        Biome3ButtonContainer.Clear();
        GenerateBiomeButtons();

    }

    private void GenerateBiomeButtons()
    {
        Button Biome1Button = new Button();
        Button Biome2Button = new Button();
        Button Biome3Button = new Button();

        Biome1Button.AddToClassList("Select-button");
        Biome2Button.AddToClassList("Select-button");
        Biome3Button.AddToClassList("Select-button");

        Biome1ButtonContainer.Add(Biome1Button);
        Biome2ButtonContainer.Add(Biome2Button);
        Biome3ButtonContainer.Add(Biome3Button);

        Biome1Button.clicked += SelectBiome1;
        Biome2Button.clicked += SelectBiome2;
        Biome3Button.clicked += SelectBiome3;
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
                // Save biome switching info to a JSON file
                break;
            case 2:
                // Save biome switching info to a JSON file

                break;
            case 3:
                // Save biome switching info to a JSON file

                break;
            default:
                break;
            
        }
        SceneManager.LoadScene("TestScene A 2");
    }
}

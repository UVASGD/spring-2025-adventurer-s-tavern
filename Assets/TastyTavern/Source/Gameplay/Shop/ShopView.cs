using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopView : MonoBehaviour
{

    [SerializeField]
    private UIDocument document;

    private VisualElement root;
    private Button ingredientsBtn;
    private Button recipesBtn;
    private Button equipmentBtn;
    private Button biomesBtn;
    private VisualElement ingredientsPage;
    private VisualElement recipesPage;
    private VisualElement equipmentPage;
    private VisualElement biomesPage;

    private VisualElement playerMoneyText;
    private VisualElement backButton;

    // Tracking current visuals
    private Button currentButton;
    private VisualElement currentPage;

    // Action for when a page is clicked, in <newly selected page, old page> format
    // public Action<VisualElement,VisualElement> OnPageClicked;


    void Awake()
    {
        root = document.rootVisualElement;

        // Retrieve all category buttons and pages from the UI
        ingredientsBtn = (Button)root.Q<VisualElement>("IngredientsBtn");
        recipesBtn = (Button)root.Q<VisualElement>("RecipesBtn");   
        equipmentBtn = (Button)root.Q<VisualElement>("EquipmentBtn");
        biomesBtn = (Button)root.Q<VisualElement>("BiomesBtn");
        ingredientsPage = root.Q<VisualElement>("IngredientsPage");
        recipesPage = root.Q<VisualElement>("RecipesPage");
        equipmentPage = root.Q<VisualElement>("EquipmentPage");
        biomesPage = root.Q<VisualElement>("BiomesPage");

        // Retrieving other UI (player money and back button)
        playerMoneyText = root.Q<VisualElement>("PlayerMoney");
        backButton = root.Q<VisualElement>("BackButton");

        // Set default page
        currentPage = ingredientsPage;
        currentButton = ingredientsBtn;
    }

    void OnEnable()
    {
        ingredientsBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Ingredients");
        recipesBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Recipes");
        equipmentBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Equipment");
        biomesBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Biomes");
    }

    void OnDisable()
    {
        ingredientsBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        recipesBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        equipmentBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        biomesBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
    }
    
    // Handles page switching
    void OnPageClicked(ClickEvent evt, string pageName)
    {
        // Hide current page and remove selected button style
        currentPage.style.display = DisplayStyle.None;  
        currentButton?.RemoveFromClassList("page-btn-selected");
        currentButton?.AddToClassList("page-btn");

        // Update current page and button
        switch (pageName)
        {
            case "Ingredients":
                currentButton = ingredientsBtn;
                currentPage = ingredientsPage;
                break;
            case "Recipes":
                currentButton = recipesBtn;
                currentPage = recipesPage;
                break;
            case "Equipment":
                currentButton = equipmentBtn;
                currentPage = equipmentPage;
                break;
            case "Biomes":
                currentButton = biomesBtn;
                currentPage = biomesPage;
                break;
            default:
                Debug.LogError("Unknown page name: " + pageName);
                break;
        }
        // Update styles
        currentButton.AddToClassList("page-btn-selected");
        currentPage.style.display = DisplayStyle.Flex;

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopView : MonoBehaviour
{

    [SerializeField]
    private UIDocument document;

    [SerializeField]
    private ShopManager shopManager;

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
    private VisualElement shopPageContainer;

    // Tracking current visuals
    private Button currentButton;
    private VisualElement currentPage;

    private List<Button> itemButtons;

    // Action for when a page is clicked, in <newly selected page, old page> format

    void Awake()
    {
        ////////////// Assigning major UI document elements //////////////
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
        shopPageContainer = root.Q<VisualElement>("ShopPageContainer");

        // Instantiate all shop items based on the current biome
        // GenerateShopItems(ingredientsPage, shopManager.CurrentShopData.IngredientItems);
        // GenerateShopItems(recipesPage, shopManager.CurrentShopData.RecipeItems);
        // GenerateShopItems(equipmentPage, shopManager.CurrentShopData.EquipmentItems);
        // GenerateShopItems(biomesPage, shopManager.CurrentShopData.BiomeItems);

        // Setting default page
        currentPage = ingredientsPage;
        currentButton = ingredientsBtn;

        // FOR TESTING I WILL ONLY SET UP INGREDIENTS BUTTONS FOR SUBSCRIPTION
        itemButtons = ingredientsPage.Query<Button>().ToList();

    }

    void OnEnable()
    {
        ingredientsBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Ingredients");
        recipesBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Recipes");
        equipmentBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Equipment");
        biomesBtn.RegisterCallback<ClickEvent, string>(OnPageClicked, "Biomes");

        // Subscribe all shop items
        foreach (Button itemBtn in itemButtons)
        {
            itemBtn.RegisterCallback<ClickEvent, Button>(OnBuyButtonClicked, itemBtn);   
        }
    }

    void OnDisable()
    {
        ingredientsBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        recipesBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        equipmentBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);
        biomesBtn.UnregisterCallback<ClickEvent, string>(OnPageClicked);

        foreach (Button itemBtn in itemButtons)
        {
            itemBtn.UnregisterCallback<ClickEvent, Button>(OnBuyButtonClicked);   
        }
    }

    // Generates shop items based on the current biome in the page (a ScrollView element)
    void GenerateShopItems(VisualElement page, List<ShopItem> shopItems)
    {
        // Clear existing items in the page
        page.Clear();

        // Create a cell for each item and add it to the page
        foreach (ShopItem item in shopItems)
        {
            VisualElement shopItemCell = new VisualElement();
            // Set data source for the button
            //itemButton.SetHierarchicalDataSourceContext(new HierarchicalDataSourceContext(item));

            page.Add(shopItemCell);
        }
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

    void OnBuyButtonClicked(ClickEvent evt, Button itemBtn)
    {
        ShopItem shopItem = itemBtn.GetHierarchicalDataSourceContext().dataSource as ShopItem;

        Debug.Log(shopItem);
        // Debug.Log("Item clicked of price: " + shopItem.Price);
        // Debug.Log("Item clicked of type: " + shopItem.Type);
    }
}

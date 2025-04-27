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

    [SerializeField]
    private VisualTreeAsset shopItemTemplate;

    private VisualElement root;
    private Button ingredientsBtn;
    private Button recipesBtn;
    private Button equipmentBtn;
    private Button biomesBtn;
    private VisualElement ingredientsPage;
    private VisualElement recipesPage;
    private VisualElement equipmentPage;
    private VisualElement biomesPage;

    private Label playerMoneyText;
    private VisualElement backButton;
    private VisualElement shopPageContainer;

    // Tracking current visuals
    private Button currentButton;
    private VisualElement currentPage;

    private List<Button> activeItemButtons;

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
        playerMoneyText = root.Q<Label>("PlayerMoney");
        
        backButton = root.Q<VisualElement>("BackButton");
        shopPageContainer = root.Q<VisualElement>("ShopPageContainer");

        // Setting default page
        currentPage = ingredientsPage;
        currentButton = ingredientsBtn;

        // FOR TESTING I WILL ONLY SET UP INGREDIENTS BUTTONS FOR SUBSCRIPTION
        // activeItemButtons = ingredientsPage.Query<Button>().ToList();

    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        // Subscribe to page button clicks
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

        // not sure if i have to unregister all item buttons here because they will be destroyed. (?)
    }
    
    public void GenerateAllShopItems(){
        GenerateShopItems(ingredientsPage, shopManager.currentShopData.IngredientItems);
        GenerateShopItems(recipesPage, shopManager.currentShopData.RecipeItems);
        GenerateShopItems(equipmentPage, shopManager.currentShopData.EquipmentItems);
        GenerateShopItems(biomesPage, shopManager.currentShopData.BiomeItems);
    }
    // Generates shop items based on the current biome in the page (a ScrollView element)
    void GenerateShopItems(VisualElement page, List<ShopItem> shopItems)
    {
        Debug.Log(shopItems.Count + " items in the shop");
        Debug.Log(shopItems[0].Name + " is the first item in the shop");
        
        page.Clear();

        // Create a cell for each item, set data source and add it to the page
        foreach (ShopItem item in shopItems)
        {
            TemplateContainer shopItemContainer = shopItemTemplate.Instantiate();
            VisualElement shopItemCell = shopItemContainer.Q<VisualElement>("ShopItem");

            shopItemCell.dataSource = item;

            // not bothering with data binding for now, just setting the information directly
            shopItemCell.Q<Label>("ItemName").text = item.Name;
            shopItemCell.Q<Label>("ItemPrice").text = item.Price.ToString();
            shopItemCell.Q<Label>("ItemDescription").text = item.Description;
            shopItemCell.Q<VisualElement>("ItemIcon").style.backgroundImage = item.Icon.texture;
            Button buyButton = shopItemCell.Q<Button>("BuyButton");

            // checking if item is bought or not, if not, register the buying callback
            if (shopManager.IsItemPurchased(item)){
                buyButton.text = "Owned";
                buyButton.SetEnabled(false);
            } else {
                buyButton.text = "Buy";
                buyButton.RegisterCallback<ClickEvent, Button>(OnBuyButtonClicked, buyButton);
            }
            
            page.Add(shopItemCell); 
        }
    }

    public void SetPlayerMoneyText(int money)
    {
        playerMoneyText.text = money.ToString();
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
        Debug.Log("Item clicked of price: " + shopItem.Price);
        Debug.Log("Item clicked of type: " + shopItem.Type);
        Debug.Log("Item clicked of data: " + shopItem.Data.Name);

        bool bought = shopManager.BuyItem(shopItem);
        if (bought) {
            itemBtn.text = "Purchased";
            itemBtn.SetEnabled(false);
        }
    }
}

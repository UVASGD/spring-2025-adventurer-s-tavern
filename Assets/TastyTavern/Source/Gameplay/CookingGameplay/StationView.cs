using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class StationView : MonoBehaviour {

    [SerializeField] string PanelName { get; set; }

    [SerializeField]
    protected UIDocument document;

    public VisualElement root;
    public VisualElement ingredientSlotContainer;
    public VisualElement actionSlotContainer;
    public VisualElement stationWorkspaceContainer;
    public VisualElement nextStationContainer;
    public VisualElement orderContainer;
    public VisualElement orderSlot1;
    public VisualElement orderSlot0;
    public VisualElement orderSlot2;
    public VisualElement barAndStationContainer;
    public VisualElement sidePanelContainer;
    public VisualElement RecipeContainer;
    public VisualElement storeButtonContainer;
    public VisualElement trashButtonContainer;


    public VisualElement stationTop;

    public Image stationBG;

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    private void OnEnable()
    {
        cookingUIEventChannel.OnLoadStationView += LoadStationView;
        cookingUIEventChannel.OnRefreshStationWorkspace += RefreshStationWorkspace;
        cookingUIEventChannel.OnRefreshIngredientsPanel += RefreshIngredientsPanel;
        cookingUIEventChannel.OnGenerateOrderButton += GenerateOrderButton;
        cookingUIEventChannel.OnDeselectOrder += CloseStationPanels;
        cookingUIEventChannel.OnDeleteOrderButton += DeleteOrderButton;
    }

    private void OnDisable() 
    {
        cookingUIEventChannel.OnLoadStationView -= LoadStationView;
        cookingUIEventChannel.OnRefreshStationWorkspace -= RefreshStationWorkspace;
        cookingUIEventChannel.OnRefreshIngredientsPanel -= RefreshIngredientsPanel;
        cookingUIEventChannel.OnGenerateOrderButton -= GenerateOrderButton;
        cookingUIEventChannel.OnDeselectOrder -= CloseStationPanels;
        cookingUIEventChannel.OnDeleteOrderButton -= DeleteOrderButton;
    }

    private void Awake(){
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        Debug.Log("root is" + root);
        ingredientSlotContainer = root.Q<VisualElement>("IngredientSlotContainer"); //already style?
        actionSlotContainer = root.Q<VisualElement>("ActionSlotContainer");
        stationWorkspaceContainer = root.Q<VisualElement>("StationWorkspaceContainer");
        nextStationContainer = root.Q<VisualElement>("NextStation");
        orderContainer = root.Q<VisualElement>("TopContainer");
        orderSlot0 = root.Q<VisualElement>("OrderSlot0");
        orderSlot1 = root.Q<VisualElement>("OrderSlot1");
        orderSlot2 = root.Q<VisualElement>("OrderSlot2");
        barAndStationContainer = root.Q<VisualElement>("BarAndStation");
        sidePanelContainer = root.Q<VisualElement>("SidePanel");
        RecipeContainer = root.Q<VisualElement>("RecipePanel");
        storeButtonContainer = root.Q<VisualElement>("StoreButton");
        trashButtonContainer = root.Q<VisualElement>("TrashButton");
        actionSlotContainer.Clear();
        ingredientSlotContainer.Clear();
        stationWorkspaceContainer.Clear();
        nextStationContainer.Clear();
        trashButtonContainer.Clear();
        RecipeContainer.Clear();
        orderSlot0.Clear(); // Probably just want slots, not order container
        orderSlot1.Clear();
        orderSlot2.Clear();
        sidePanelContainer.visible = false;
        barAndStationContainer.visible = false;
    }

    // ***May be easier to have a simple button instead, not attached to station data, go back up to order
    // combine with initialize?
    private void LoadStationView(Station station){
        Debug.Log("View recieved loading request from event channel");
        Debug.Log("Load Station view");
        actionSlotContainer.Clear();
        ingredientSlotContainer.Clear();
        stationWorkspaceContainer.Clear();
        RecipeContainer.Clear();
        nextStationContainer.Clear();
        storeButtonContainer.Clear();
        trashButtonContainer.Clear();
        if (station.Data.StationType == StationType.Serving){
            GenerateServeButton(); // last station only generates serve button
        } else {
            GenerateNextStationButton();
            GenerateActionButton(station.Data.ActionData); 
        }
        GenerateIngredientButtons(station.StockIngredients);
        GenerateStationBackground(station);
        GenerateOrderInstructions(station.StockIngredients);
        GenerateStoreButton();
        GenerateTrashButton();
        sidePanelContainer.visible = true;
        barAndStationContainer.visible = true;
    }

    private void GenerateOrderInstructions(List<Ingredient> stockIngredients)
    {
        Label orderInstructions = new();
        var instructions = "";
        Debug.Log("Generating order instructions");
        foreach (var ingredient in stockIngredients)
        {
            
            if (ingredient.Properties.Count > 0)
                instructions += ingredient.Properties[^1] + " " + ingredient.Data.Name + "\n";
            else
                instructions += ingredient.Data.Name + "\n";
        }

        orderInstructions.text = instructions;
        orderInstructions.AddToClassList("action-label");
        RecipeContainer.Add(orderInstructions);
    }

    // A simple styled button with 
    private void GenerateNextStationButton(){
        Button nextButton = new();
        nextButton.AddToClassList("unity-text-label");
        nextButton.AddToClassList("unity-button");
        nextButton.AddToClassList("button");
        nextButton.AddToClassList("next-station-button");
        nextButton.text = "Next Station";
        nextStationContainer.Add(nextButton);
        nextButton.clicked += OnNextStation;
    }

    

    private void GenerateServeButton(){
        Button serveButton = new();
        serveButton.AddToClassList("button");
        serveButton.AddToClassList("next-station-button"); // TODO: consolidate generic styles
        serveButton.text = "Serve Order";
        nextStationContainer.Add(serveButton); // change container name
        serveButton.clicked += OnServeOrder;
    }

    private void GenerateActionButton(ActionData actionData){
        ActionButton actionButton = new(actionData);
        Debug.Log($"Slot created for {actionButton.Data.Name}");
        actionSlotContainer.Add(actionButton);
        actionButton.OnClickButton += OnAddProperty;
    }

    // ONLY happens when new order is added to order manager
    private void GenerateOrderButton(Order order){
        Debug.Log("Generating order button");
        OrderButton orderButton = new(order);
        if (order.Customer.Data.CustomerSpotIdx == 0){
            orderSlot0.Add(orderButton);
        } else if (order.Customer.Data.CustomerSpotIdx == 1){
            orderSlot1.Add(orderButton);
        } else {
            orderSlot2.Add(orderButton);
        }
        orderButton.OnClickButton += OnSelectOrder;
    }

    private void GenerateIngredientButtons(List<Ingredient> ingredients){
        foreach(Ingredient ingredient in ingredients){
            IngredientButton ingredientButton = new(ingredient);
            Debug.Log("Slot created for " + ingredientButton.Ingredient.Data.Name);
            ingredientSlotContainer.Add(ingredientButton);
            ingredientButton.OnClickButton += OnAddIngredient;
        }
    }

    private void GenerateStationBackground(Station station){
        stationBG = new(){ image = station.Data.Background.texture }; // change this to just equipment, not counter
        stationWorkspaceContainer.Add(stationBG);
        stationTop = stationBG;
    }

    private void GenerateStoreButton(){
        Button storeButton = new();
        storeButton.AddToClassList("unity-text-label");
        storeButton.AddToClassList("unity-button");
        storeButton.AddToClassList("button");
        storeButton.AddToClassList("store-button");
        storeButton.text = "Store";
        storeButtonContainer.Add(storeButton);
        storeButton.clicked += OnStoreIngredient;
    }
    
    private void GenerateTrashButton()
    {
        Button trashButton = new();
        trashButton.AddToClassList("unity-text-label");
        trashButton.AddToClassList("unity-button");
        trashButton.AddToClassList("button");
        trashButton.AddToClassList("trash-button");
        trashButton.text = "Trash";
        trashButtonContainer.Add(trashButton);
        trashButton.clicked += OnTrashOrderFood;
    }

    private void DeleteOrderButton(int orderIdx){
        if (orderIdx == 0){
            orderSlot0.Clear();
        } else if (orderIdx == 1){
            orderSlot1.Clear();
        } else {
            orderSlot2.Clear();
        }
    }

    private void OnAddIngredient(IngredientButton ingredientButton ) {
        cookingUIEventChannel.RaiseOnAddIngredient(ingredientButton.Ingredient); // adds ingredient, calls refresh
        ingredientButton.SetEnabled(false);
        ingredientButton.RemoveFromClassList("button");
    }
    
    private void OnStoreIngredient() {
        cookingUIEventChannel.RaiseOnStoreIngredient();
    }

    private void OnTrashOrderFood()
    {
        cookingUIEventChannel.RaiseOnTrashCurrentOrderFood();
    }

    private void OnAddProperty(ActionButton actionButton){
        cookingUIEventChannel.RaiseOnAddProperty(actionButton.Data); // Property enum actionProperty
    }
    
    // This button is not DataButton, does not pass button data
    private void OnNextStation(){
        cookingUIEventChannel.RaiseOnChangeNextStation();
    }

    private void OnServeOrder(){
        cookingUIEventChannel.RaiseOnSubmitOrder(null); // passes in null because StationView doesn't have access to the current order. There is a null check, don't worry. 
        CloseStationPanels();
    }

    private void OnSelectOrder(OrderButton orderButton){
        cookingUIEventChannel.RaiseOnSelectOrder(orderButton.Order);
        LoadStationView(orderButton.Order.Station);
    }

    // could consolidate into helper, hiding and showing station elements
    private void CloseStationPanels(){
        sidePanelContainer.visible = false;
        barAndStationContainer.visible = false;
    }

    // TODO: change method of determining sprites
    private void AddToStationWorkspace(Ingredient ingredient){
        Sprite sprite;
        if( ingredient.Properties.Contains(Property.Cut) && ingredient.Properties.Contains(Property.Cooked)){
            sprite = ingredient.Data.Sprites[3];
        } else if (ingredient.Properties.Contains(Property.Cut)){
            sprite = ingredient.Data.Sprites[2];
        } else {
            sprite = ingredient.Data.Sprites[1];
        }
        Image icon = new(){ image = sprite.texture };
        stationTop.Add(icon);
        stationTop = icon; // update new top of stack
    }

    private void RefreshStationWorkspace(Station station){
        stationBG.Clear();
        stationTop = stationBG;
        foreach (var ingredient in station.ActiveIngredients){
            AddToStationWorkspace(ingredient);
        }
    }

    private void RefreshIngredientsPanel(Station station){
        ingredientSlotContainer.Clear();
        GenerateIngredientButtons(station.StockIngredients);
    }

}

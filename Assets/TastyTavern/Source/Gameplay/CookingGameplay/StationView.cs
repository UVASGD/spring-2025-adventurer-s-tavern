using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
//using Mono.Cecil.Cil;
//using NUnit.Framework.Internal.Commands;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StationView : MonoBehaviour {

    [SerializeField] private ProgressBar progressBar;
    private VisualElement progressBarContainer1;
    private VisualElement progressBarContainer2;
    private VisualElement progressBarContainer3;

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
    
    private AudioManager audioManager;

    private void OnEnable()
    {
        cookingUIEventChannel.OnLoadStationView += LoadStationView;
        // cookingUIEventChannel.OnRefreshStationWorkspace += RefreshStationWorkspace;
        cookingUIEventChannel.OnRefreshIngredientsPanel += RefreshIngredientsPanel;
        cookingUIEventChannel.OnGenerateOrderButton += GenerateOrderButton;
        cookingUIEventChannel.OnDeselectOrder += CloseStationPanels;
        cookingUIEventChannel.OnDeleteOrderButton += DeleteOrderButton;
        cookingUIEventChannel.OnAddProperty += StartProgress;
    }

    private void OnDisable() 
    {
        cookingUIEventChannel.OnLoadStationView -= LoadStationView;
        // cookingUIEventChannel.OnRefreshStationWorkspace -= RefreshStationWorkspace;
        cookingUIEventChannel.OnRefreshIngredientsPanel -= RefreshIngredientsPanel;
        cookingUIEventChannel.OnGenerateOrderButton -= GenerateOrderButton;
        cookingUIEventChannel.OnDeselectOrder -= CloseStationPanels;
        cookingUIEventChannel.OnDeleteOrderButton -= DeleteOrderButton;
        cookingUIEventChannel.OnAddProperty -= StartProgress;
    }

    public System.Action OnProgressComplete;
    private void Awake(){
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        
        ingredientSlotContainer = root.Q<VisualElement>("IngredientSlotContainer"); //already style?
        actionSlotContainer = root.Q<VisualElement>("ActionSlotContainer");
        stationWorkspaceContainer = root.Q<VisualElement>("StationWorkspaceContainer");
        nextStationContainer = root.Q<VisualElement>("NextStation");
        orderContainer = root.Q<VisualElement>("TopContainer");
        orderSlot0 = root.Q<VisualElement>("OrderSlot0");
        orderSlot1 = root.Q<VisualElement>("OrderSlot1");
        orderSlot2 = root.Q<VisualElement>("OrderSlot2");
        progressBarContainer1 = root.Q<VisualElement>("ProgressBarContainer1");
        progressBarContainer2 = root.Q<VisualElement>("ProgressBarContainer2");
        progressBarContainer3 = root.Q<VisualElement>("ProgressBarContainer3");
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
        
        audioManager = AudioManager.Instance;
    }

    // ***May be easier to have a simple button instead, not attached to station data, go back up to order
    // combine with initialize?
    private void LoadStationView(Station station){
        actionSlotContainer.Clear();
        ingredientSlotContainer.Clear();
        // stationWorkspaceContainer.Clear();
        RecipeContainer.Clear();
        nextStationContainer.Clear();
        storeButtonContainer.Clear();
        trashButtonContainer.Clear();
        if (station.Data != null) {
            if (station.Data.StationType == StationType.Serving)
            {
                GenerateServeButton(); // last station only generates serve and assemble button
                GenerateAssembleButton();
            }
            else
            {
                GenerateNextStationButton();
                GenerateActionButton(station.Data.ActionData);
            }
        }
        else
        {
            Debug.LogError("Station data is null!");
        }
        GenerateIngredientButtons(station);
        // GenerateStationBackground(station);
        GenerateOrderInstructions(station);
        GenerateStoreButton();
        GenerateTrashButton();
        sidePanelContainer.visible = true;
        barAndStationContainer.visible = true;
    }

    private void GenerateAssembleButton()
    {
        Button assembleButton = new();
        assembleButton.AddToClassList("action-button");
        assembleButton.AddToClassList("button");
        assembleButton.text = "Assemble";
        actionSlotContainer.Add(assembleButton);
        assembleButton.clicked += OnAssembleOrder;
    }

    private void GenerateOrderInstructions(Station station)
    {
        List<IngredientData> ingredients = station.OrderManager.currentOrder.Recipe.CorrectStockSequence[station.OrderManager.currentOrder.StationIdx].CorrectIngredients;
        Label orderInstructions = new();
        var instructions = "";
        switch (station.Data.StationType)
        {
            case StationType.Serving:
                instructions += "Assemble: ";
                break;
            case StationType.Grill:
                instructions += "Grill: ";
                break;
            case StationType.Oven:
                instructions += "Bake: ";
                break;
            case StationType.Pan:
                instructions += "Fry: ";
                break;
            case StationType.CuttingBoard:
                instructions += "Cut: ";
                break;
            case StationType.DeepFryer:
                instructions += "Deep Fry: ";
                break;
            case StationType.MixingBowl:
                instructions += "Mix: ";
                break;
            default:
                instructions += "Uknnown: ";
                break;
        }
        
        foreach (IngredientData ingredient in ingredients)
            instructions += "\n" +
                            "\t- " + ingredient.Name + "\n";

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
    
    private void StartProgress(ActionData action)
    {
        // StartCoroutine(UpdateProgressBar(action.ActionTime));
    }

    // private IEnumerator UpdateProgressBar(float duration){
    //     float elapsed = 0f;

    //     while (elapsed < duration){
    //         elapsed += Time.deltaTime;
    //         float percentage = Mathf.Clamp01(elapsed / duration) * 100f;
    //         progressBar.progress = percentage;
    //         yield return null;
    //     }
    //     progressBar.progress = 100f;

    //     progressBar.OnProgressComplete?.Invoke();
    // }

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
        actionSlotContainer.Add(actionButton);
        actionButton.OnClickButton += OnAddProperty;
    }

    // ONLY happens when new order is added to order manager
    private void GenerateOrderButton(Order order)
    {
        OrderButton orderButton;
        if (order.Customer.Data.CustomerSpotIdx == 0){
            orderButton = new(order, progressBarContainer1);
            orderSlot0.Add(orderButton);
            orderButton.OnClickButton += OnSelectOrder;
        } else if (order.Customer.Data.CustomerSpotIdx == 1){
            orderButton = new(order, progressBarContainer2);
            orderSlot1.Add(orderButton);
            orderButton.OnClickButton += OnSelectOrder;
        } else {
            orderButton = new(order, progressBarContainer3);
            orderSlot2.Add(orderButton);
            orderButton.OnClickButton += OnSelectOrder;
        }
    }

    private void GenerateIngredientButtons(Station station)
    {
        List<Ingredient> ingredients = station.StockIngredients;
        if (station.Data.StationType != StationType.Serving)
            foreach(Ingredient ingredient in ingredients){
                IngredientButton ingredientButton = new(ingredient);
                ingredientSlotContainer.Add(ingredientButton);
                ingredientButton.OnClickButton += OnAddIngredient;
            }
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
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnAddIngredient(ingredientButton.Ingredient); // adds ingredient, calls refresh
        ingredientButton.SetEnabled(false);
        ingredientButton.RemoveFromClassList("button");
    }
    
    private void OnAssembleOrder()
    {
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnAssembleOrder();
    }
    
    private void OnStoreIngredient() {
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnStoreIngredient();
    }

    private void OnTrashOrderFood()
    {
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnTrashCurrentOrderFood();
    }

    private void OnAddProperty(ActionButton actionButton){
        //audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnAddProperty(actionButton.Data); // Property enum actionProperty
    }
    
    // This button is not DataButton, does not pass button data
    private void OnNextStation(){
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnChangeNextStation();
    }

    private void OnServeOrder(){
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnSubmitOrder(null); // passes in null because StationView doesn't have access to the current order. There is a null check, don't worry. 
        CloseStationPanels();
    }

    private void OnSelectOrder(OrderButton orderButton){
        audioManager.PlaySFX("ButtonClick");
        cookingUIEventChannel.RaiseOnSelectOrder(orderButton.Order);
    }

    // could consolidate into helper, hiding and showing station elements
    private void CloseStationPanels(){
        sidePanelContainer.visible = false;
        barAndStationContainer.visible = false;
    }

    private void RefreshIngredientsPanel(Station station){
        ingredientSlotContainer.Clear();
        GenerateIngredientButtons(station);
    }

}

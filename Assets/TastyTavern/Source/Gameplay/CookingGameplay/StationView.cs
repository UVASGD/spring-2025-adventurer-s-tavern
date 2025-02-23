using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: Organize references and use Event Channels
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

    public VisualElement stationTop;

    public Image stationBG;

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    public IngredientData basilisk;
    public IngredientData punchPepper;

    private void OnEnable()
    {
        cookingUIEventChannel.OnLoadStationView += LoadStationView;
        cookingUIEventChannel.OnRefreshStationView += RefreshStationView;
    }

    private void OnDisable() 
    {
        cookingUIEventChannel.OnLoadStationView -= LoadStationView;
        cookingUIEventChannel.OnRefreshStationView -= RefreshStationView;
    }

    private void Awake(){
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        Debug.Log("root is" + ingredientSlotContainer);
        ingredientSlotContainer = root.Q<VisualElement>("IngredientSlotContainer"); //already style?
        actionSlotContainer = root.Q<VisualElement>("ActionSlotContainer");
        stationWorkspaceContainer = root.Q<VisualElement>("StationWorkspaceContainer");
        nextStationContainer = root.Q<VisualElement>("BottomContainer");
        orderContainer = root.Q<VisualElement>("TopContainer");
    }

    private void Start(){
        // List<Ingredient> dummyIngredients = new()
        // {
        //     basilisk.Create(),
        //     punchPepper.Create()
        // };
        // InitializeView(dummyIngredients);
        // stationWorkspaceContainer.Clear();
    }
    // ***May be easier to have a simple button instead, not attached to station data, go back up to order
    // combine with initialize?
    private void LoadStationView(Station station){
        Debug.Log("View recieved loading request from event channel");
        InitializeView(station,station.Data.ActionData,station.StockIngredients);
    }

    // ingredients --> live ingredients in the station storage/stock
    // TODO: Change params to just use station
    public void InitializeView(Station station, ActionData actionData,List<Ingredient> ingredients){
        Debug.Log("Initializing Station view");
        actionSlotContainer.Clear();
        ingredientSlotContainer.Clear();
        stationWorkspaceContainer.Clear();
        nextStationContainer.Clear(); // may not have to clear anymore when hiding station panels, station button stays constant.
        GenerateNextStationButton();
        GenerateActionButton(actionData);
        GenerateIngredientButtons(ingredients);
        GenerateStationBackground(station);
        // make visible the parent elements for station menus (everything except order tabs)
    }

    // A simple styled button with 
    private void GenerateNextStationButton(){
        Button nextButton = new();
        nextButton.AddToClassList("button");
        nextStationContainer.Add(nextButton);
        nextButton.clicked += OnNextStation;
    }

    private void GenerateActionButton(ActionData actionData){
        ActionButton actionButton = new(actionData);
        Debug.Log($"Slot created for {actionButton.Data.Name}");
        actionSlotContainer.Add(actionButton);
        actionButton.OnClickButton += OnAddProperty;
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
        stationBG = new(){ image = station.Data.Background.texture };
        stationWorkspaceContainer.Add(stationBG);
        stationTop = stationBG;
    }

    private void OnAddIngredient(IngredientButton ingredientButton ) {
        cookingUIEventChannel.RaiseOnAddIngredient(ingredientButton.Ingredient); // adds ingredient, calls refresh
        ingredientButton.SetEnabled(false);
        ingredientButton.RemoveFromClassList("button");
    }
    
    private void OnAddProperty(ActionButton actionButton){

        cookingUIEventChannel.RaiseOnAddProperty(actionButton.Data); // Property enum actionProperty
    }
    
    private void OnNextStation(){
        cookingUIEventChannel.RaiseOnChangeNextStation();
    }

    private void AddToStationWorkspace(Ingredient ingredient){
        Sprite sprite;
        if( ingredient.Properties.Contains(Property.Cut) && ingredient.Properties.Contains(Property.Cooked) ){
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

    private void RefreshStationView(Station station){
        stationBG.Clear();
        stationTop = stationBG;
        foreach (var ingredient in station.ActiveIngredients){
            AddToStationWorkspace(ingredient);
        }
        Debug.Log(station.ActiveIngredients[0]);
    }

}

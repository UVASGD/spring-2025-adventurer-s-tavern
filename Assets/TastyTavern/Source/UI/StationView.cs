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
    public Slot[] Slots;

    [SerializeField] string PanelName { get; set; }

    [SerializeField]
    protected UIDocument document;

    public VisualElement root;
    public VisualElement ingredientSlotContainer;
    public VisualElement actionSlotContainer;
    public VisualElement stationWorkspaceContainer;
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

    // add action item in param
    // ingredients --> live ingredients in the station storage/stock
    // TODO: Change params to just use station
    public void InitializeView(Station station, ActionData actionData,List<Ingredient> ingredients){
        Debug.Log("Initializing Station view");

        actionSlotContainer.Clear();
        ActionSlot actionSlot = new(actionData);
        Debug.Log($"Slot created for {actionSlot.ActionData.Name}");
        actionSlot.AddToClassList("action-slot");
        actionSlot.AddToClassList("slot");
        actionSlotContainer.Add(actionSlot);
        actionSlot.OnClickAction += OnAddProperty;
        // actionSlot.visible = false;

        ingredientSlotContainer.Clear();
        foreach(Ingredient ingredient in ingredients){
            Slot slot = new(ingredient);
            Debug.Log("Slot created for " + slot.Ingredient.Data.Name);
            slot.AddToClassList("ingredient-slot"); // make helper methods?
            slot.AddToClassList("slot");
            ingredientSlotContainer.Add(slot);
            slot.OnClickIngredient += OnAddIngredient;
        }

        // bring back stationWorkspaceContainer
        // if (stationWorkspaceContainer.visible == false){
        //     stationWorkspaceContainer.visible = true;
        // }
        stationBG = new(){ image = station.Data.Background.texture };
        stationWorkspaceContainer.Add(stationBG);
        stationTop = stationBG;
    }

    private void LoadStationView(Station station){
        Debug.Log("View recieved loading request from event channel");
        InitializeView(station,station.Data.ActionData,station.StockIngredients);
    }

    private void OnAddIngredient(Slot slot) {
        cookingUIEventChannel.RaiseOnAddIngredient(slot.Ingredient); // adds ingredient, calls refresh
        slot.SetEnabled(false);
        slot.RemoveFromClassList("slot");
    }

    private void OnAddProperty(ActionSlot actionSlot){
        cookingUIEventChannel.RaiseOnAddProperty(actionSlot.ActionData.Property); // Property enum actionProperty
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
    }

}

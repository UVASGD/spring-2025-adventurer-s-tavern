// using System.Collections;
// using System.ComponentModel.Design.Serialization;
// using UnityEngine;
// using UnityEngine.UIElements;

// // TODO: Organize references and use Event Channels
// public class MenuControlOld : MenuView
// {

//     public VisualElement ui;

//     public VisualTreeAsset ingredientButtonTemplate;

//     public VisualElement ingredientsContainer;
//     public VisualElement actionContainer;

//     // for now 
//     public Button onionButton;
//     public Button actionButton;
    
//     public IngredientData onionData;

//     [SerializeField]
//     private CookingUIEventChannel cookingUIEventChannel;


//     private void Awake()
//     {
//         ui = GetComponent<UIDocument>().rootVisualElement;
//         onionButton = ui.Q<Button>("onion");
//         actionButton = ui.Q<Button>("Knife");
//         ingredientsContainer = ui.Q<VisualElement>("IngredientsContainer");
//         actionContainer = ui.Q<VisualElement>("ActionContainer");

//         OnLoadStation();
//     }

//     private void OnEnable()
//     {
//         onionButton.clicked += OnOnionClicked;
//         actionButton.clicked += OnActionClicked;
//     }

//     private void OnDisable()
//     {
//         onionButton.clicked -= OnOnionClicked;
//         actionButton.clicked -= OnActionClicked;
//     }

//     // ********GENERALIZE******Eventually, will not have ingredient specific
//     // will BIND ingredients, one function for all ingredients
//     private void OnOnionClicked()
//     {
//         cookingUIEventChannel.RaiseOnAddIngredient(onionData);
//     }

//     //********GENERALIZE******Eventually
//     private void OnActionClicked()
//     {
//         cookingUIEventChannel.RaiseOnAddProperty(Property.Cut); // Property enum actionProperty
//     }

//     //Can make a new order with correct numbered slot?
//     private void OrderSelected()
//     {
//         cookingUIEventChannel.RaiseOpenOrder(); // send through the related order 
//     }

//     // add station type var
//     private void OnLoadStation()
//     {
//         // load appropriate menu
//         // for item in stock/storage, instantiate button.
//         Debug.Log("making new button");
//         TemplateContainer ingredientButtonContainer = ingredientButtonTemplate.Instantiate();
//         ingredientsContainer.Add(ingredientButtonContainer);
        
//     }

//     public override IEnumerator InitializeView(){
//         yield return null; // initialize tree, set slots? Called on every station swap?
//         Slots = new Slot[6];
//         root = document.rootVisualElement;
//         root.Clear();

//         // generating children, frame, header, slotscontainer
//         // new slot for every class.

//         yield return null;
//     }

// }

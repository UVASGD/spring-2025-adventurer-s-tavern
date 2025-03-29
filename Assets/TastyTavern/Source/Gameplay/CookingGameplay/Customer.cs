using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [field: SerializeField]
    public CustomerData Data { get; private set; }

    [field: SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [field: SerializeField]
    public MenuManager MenuManager { get; set; }
    
    [field: SerializeField]
    public float RemainingPatience { get; set; }


    // ONLY FOR DEBUGGING PURPOSES
    [field: SerializeField]
    public string orderName;

    private void Start()
    {
        RemainingPatience = Data.Patience;
    }

    private void Update()
    {
        RemainingPatience -= Time.deltaTime;
        if (RemainingPatience <= 0)
        {
            cookingUIEventChannel.RaiseOnSubmitOrder(Data.Order);
        }
        
        if (Data.Order != null)
        {
            orderName = Data.Order.Recipe.Name;
        }
    }

    public void Initialize(CustomerData data)
    {
        Data = data;
        Data.Order = GenerateOrder();
        // Debug info or appearance setup (e.g., sprites, dialogue)
        Debug.Log($"Customer {Data.Name} initialized with patience {Data.Patience}.");
        PlaceCustomerOrder(Data.Order);
    }

    private Order GenerateOrder()
    { 
        // recipe.CorrectStockSequence[^1].CorrectIngredients -> list of all ingredients in the recipe
        // recipe.CorrectStockSequence[^1].CorrectPropertiesPerIngredient[0->n].Properties -> all the properties that each ingredient(0 to n) needs to have by the end of the order; Sort of a 3D array.
        // CorrectStockSequence -> The correct stocks of ingredients & properties for each station; CorrectPropertiesPerIngredient -> Each ingredient has a list of properties that it needs to have by the end (cut, etc.)
        // Properties -> the list of properties of one ingredient
        return new Order(this, MenuManager.GetRandomRecipeFromMenu(), cookingUIEventChannel);
    }

    private void PlaceCustomerOrder(Order order)
    {
        Debug.Log($"Order for {Data.Name} has been placed.");
        cookingUIEventChannel?.RaiseOpenOrder(order);
        // Order placed logic (UI, update the order list, etc.)
        // When CurrentOrderManager is placed in the scene (as of now it isn't yet), access that somehow and then update its private allOrders list with the new order
    }

    // public void CompleteCustomerOrder(bool isSatisfied) // maybe this will be called by the Station UI, or maybe the UI will have its own function. If the station UI has its own way of calling the event, then this function is useless. 
    // {
    //     Debug.Log($"Customer {Data.Name} is {(isSatisfied ? "satisfied" : "dissatisfied")}.");
    //     // Customer says satisfied or dissatisfied dialogue -> customer is dismissed -> related UI is updated -> allOrders list is updated -> money is received -> etc. Perhaps this could be an event instead if needed
    //     cookingUIEventChannel?.RaiseOnSubmitOrder();
    // }
}

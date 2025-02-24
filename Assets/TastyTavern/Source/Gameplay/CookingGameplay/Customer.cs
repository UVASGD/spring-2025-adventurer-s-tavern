using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [field: SerializeField]
    public CustomerData Data { get; private set; }

    [field: SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [field: SerializeField]
    public MenuManager MenuManager { get; set; }


    // ONLY FOR DEBUGGING PURPOSES
    [field: SerializeField]
    public string orderName;

    private void Update()
    {
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
        System.Random rand = new System.Random();
        RecipeData recipe = MenuManager.GetRandomRecipeFromMenu();

        // Customization logic
        Dictionary<IngredientData, List<Property>> selectedIngredients = new Dictionary<IngredientData, List<Property>>();

        for (int i = 0; i < recipe.Ingredients.Count; i++)
        {
            selectedIngredients.Add(recipe.Ingredients[i], recipe.Properties[i].Properties);
        }

        /**if (rand.Next(0, 4) == 3) // 1/4 chance
        {
            selectedIngredients.Remove(selectedIngredients.ElementAt(rand.Next(0, selectedIngredients.Count - 1)).Key);
        }**/

        return new Order(this, recipe, selectedIngredients);
    }

    private void PlaceCustomerOrder(Order order)
    {
        Debug.Log($"Order for {Data.Name} has been placed.");
        cookingUIEventChannel?.RaiseOpenOrder(order);
        // Order placed logic (UI, update the order list, etc.)
        // When CurrentOrderManager is placed in the scene (as of now it isn't yet), access that somehow and then update its private allOrders list with the new order
    }

    public void CompleteCustomerOrder(bool isSatisfied) // maybe this will be called by the Station UI, or maybe the UI will have its own function. If the station UI has its own way of calling the event, then this function is useless. 
    {
        Debug.Log($"Customer {Data.Name} is {(isSatisfied ? "satisfied" : "dissatisfied")}.");
        // Customer says satisfied or dissatisfied dialogue -> customer is dismissed -> related UI is updated -> allOrders list is updated -> money is received -> etc. Perhaps this could be an event instead if needed
        cookingUIEventChannel?.RaiseOnSubmitOrder(this);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [field: SerializeField]
    public CustomerData Data { get; private set; }

    [field: SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    public void Initialize(CustomerData data)
    {
        Data = data;

        // Set the customer reference in the order
        if (Data.Order != null)
        {
            Data.Order.Customer = this;
        }

        // Debug info or appearance setup (e.g., sprites, dialogue)
        Debug.Log($"Customer {Data.Name} initialized with patience {Data.Patience}.");
        PlaceCustomerOrder(Data.Order);
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
        cookingUIEventChannel?.RaiseOnSubmitOrder(Data.Order);
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEditor;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [SerializeField]
    private CustomerController customerController;

    [SerializeField]
    private Order currentOrder; 

    [SerializeField]
    private List<Order> activeOrders = new();

    [SerializeField] private Dictionary<Order, float> servedOrders = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ASSUMING SET ORDER AND STATION FOR NOW
        if (currentOrder != null){
            foreach( var i in currentOrder.Station.ActiveIngredients)
            {
                Debug.Log("station has " + i.Data.Name);
            }
        }
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnCreateOrder += AddOrder;
        cookingUIEventChannel.OnSubmitOrder += SubmitOrder;
        cookingUIEventChannel.OnAddProperty += StartAddProperty;
        cookingUIEventChannel.OnSelectOrder += SelectOrder;
        cookingUIEventChannel.OnChangeNextStation += ChangeNextStation;
        cookingUIEventChannel.OnTrashCurrentOrderFood += OnTrashCurrentOrderFood;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnCreateOrder -= AddOrder;
        cookingUIEventChannel.OnSubmitOrder -= SubmitOrder;
        cookingUIEventChannel.OnAddProperty -= StartAddProperty;
        cookingUIEventChannel.OnSelectOrder -= SelectOrder;
        cookingUIEventChannel.OnChangeNextStation -= ChangeNextStation;
        cookingUIEventChannel.OnTrashCurrentOrderFood -= OnTrashCurrentOrderFood;
    }

    // Add Property starts here because it needs to kick off a coroutine
    private void StartAddProperty(ActionData actionData)
    {
        StartCoroutine(ExecuteAddProperty(actionData));
    }

    private IEnumerator ExecuteAddProperty(ActionData actionData)
    {
        progressBar.StartProgress(actionData.ActionTime);
        yield return new WaitForSeconds(actionData.ActionTime);
        
        // apply property to ingredients in station
        List<Ingredient> ingredients = currentOrder.Station.ApplyProperty(actionData);
        
        // apply property to ingredients in the SelectedIngredients of the current order
        foreach (Ingredient processedIngredient in ingredients)
        {
            foreach (IngredientData orderIngredientData in currentOrder.CurrentIngredients.Keys)
            {
                if (processedIngredient.Data.Name.Equals(orderIngredientData.Name))
                {
                    currentOrder.CurrentIngredients[orderIngredientData].Add(actionData.Property);// Might be unfinished. look later. 
                }
            }
        }
    }

    /// <summary>
    /// Changes the current order to the newly selected order.
    /// </summary>
    /// <param name="orderData"></param>
    private void SelectOrder(Order selectedOrder)
    {
        if (selectedOrder == currentOrder){ // for now, click again to deselect TODO: close X button
            DeselectOrder();
            return;
        }
        Debug.Log("Selected Order " + selectedOrder);
        currentOrder = selectedOrder;
        currentOrder.Station.Subscribe();
    }

    private void DeselectOrder()
    {
        if (currentOrder != null) currentOrder.Station.Unsubscribe();
        currentOrder = null;
        cookingUIEventChannel.RaiseOnDeselectOrder();
    }
    
    private void OnTrashCurrentOrderFood()
    {
        int index = currentOrder.StationIdx;
        Debug.Log(activeOrders.IndexOf(currentOrder) + " order's " + index + " station trashed");
        currentOrder.ResetStation();
        DeselectOrder();
        activeOrders[index] = currentOrder;
    }
    
    public void AddOrder(Order order)
    {
        order.cookingUIEventChannel = cookingUIEventChannel; // pass event channel to order
        activeOrders.Add(order);
        cookingUIEventChannel.RaiseOnGenerateOrderButton(order);
    }

    public void SubmitOrder(Order order)
    {
        if (order == null)
        {
            SubmitOrder(currentOrder);
        }
        else
        {
            activeOrders.Remove(order);
            cookingUIEventChannel.RaiseOnRemoveCustomer(order.Customer.Data.CustomerSpotIdx);
            float correctness = order.IsCorrect();
            Debug.Log(correctness);
            if (correctness == 1.0f)
            {
                Debug.Log("Order is correct");
            }
            else
            {
                Debug.Log("Order is incorrect");
            }
            Customer c = order.Customer;
            servedOrders[order] = correctness * (c.RemainingPatience / c.Data.Patience);
        }
    }

    public Dictionary<Order, float> GetServedOrders()
    {
        return servedOrders;
    }
    
    // Pass event channel trigger to order
    public void ChangeNextStation(){
        currentOrder.ChangeStation();
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [SerializeField]
    private CustomerController customerController;

    [SerializeField]
    private Order currentOrder; 

    [SerializeField]
    private List<Order> allOrders = new(); 

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
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnCreateOrder -= AddOrder;
        cookingUIEventChannel.OnSubmitOrder -= SubmitOrder;
        cookingUIEventChannel.OnAddProperty -= StartAddProperty;
        cookingUIEventChannel.OnSelectOrder -= SelectOrder;
        cookingUIEventChannel.OnChangeNextStation -= ChangeNextStation;
    }

    // Add Property starts here because it needs to kick off a coroutine
    private void StartAddProperty(ActionData actionData)
    {
        StartCoroutine(ExecuteAddProperty(actionData));
    }

    private IEnumerator ExecuteAddProperty(ActionData actionData)
    {
        yield return new WaitForSeconds(actionData.ActionTime);

        currentOrder.Station.ApplyProperty(actionData);
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
        if (currentOrder != null) currentOrder.Station.Unsubscribe();
        currentOrder = selectedOrder;
    }

    private void DeselectOrder()
    {
        if (currentOrder != null) currentOrder.Station.Unsubscribe();
        currentOrder = null;
        cookingUIEventChannel.RaiseOnDeselectOrder();
    }

    public void AddOrder(Order order)
    {
        order.cookingUIEventChannel = cookingUIEventChannel; // pass event channel to order
        allOrders.Add(order);
        cookingUIEventChannel.RaiseOnGenerateOrderButton(order);
    }

    public void SubmitOrder()
    {
        allOrders.Remove(currentOrder);
        cookingUIEventChannel.RaiseOnRemoveCustomer(currentOrder.Customer.Data.CustomerSpotIdx);
        if (currentOrder.isCorrect()){
            Debug.Log("Order is correct");
        } else {
            Debug.Log("Order is incorrect");
        }
            // other things can happen here like money? etc. like playerMoney += order.Recipe.Price; or something like that
    }

    // Pass event channel trigger to order
    public void ChangeNextStation(){
        currentOrder.ChangeStation();
    }
}

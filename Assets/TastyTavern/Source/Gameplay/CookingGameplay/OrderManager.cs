using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [SerializeField]
    private CustomerController customerController;

    [SerializeField]
    private Order currentOrder; 

    [SerializeField]
    private List<Order> allOrders; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ASSUMING SET ORDER AND STATION FOR NOW
        foreach( var i in currentOrder.CurrentStation.ActiveIngredients)
        {
            Debug.Log("station has " + i.Data.Name);
        }
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnOpenOrder += CreateOrder;
        cookingUIEventChannel.OnSubmitOrder += SubmitOrder;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnOpenOrder -= CreateOrder;
        cookingUIEventChannel.OnSubmitOrder -= SubmitOrder;
    }


    /// <summary>
    /// Changes the current order to the newly selected order.
    /// </summary>
    /// <param name="orderData"></param>
    private void SelectOrder(Order selectedOrder)
    {
        Debug.Log("Selected Order " + selectedOrder);
        currentOrder = selectedOrder;
        LoadStation(currentOrder.CurrentStation.Data);
    }

    private void LoadStation(StationData station)
    {
        Debug.Log("Loading Station " + station.StationType);
        // all station logic is updated on station object
        // update menu where? how does it know the data
    }

    public void CreateOrder(Order order)
    {
        allOrders.Add(order);
    }
    public void SubmitOrder(Order order)
    {
        allOrders.Remove(order);
        if (order.isCorrect())
            Debug.Log("Order is correct");
            // other things can happen here like money? etc. like playerMoney += order.Recipe.Price; or something like that
    }
}

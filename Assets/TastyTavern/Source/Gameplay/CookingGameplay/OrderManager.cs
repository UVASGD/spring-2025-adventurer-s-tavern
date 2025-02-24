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
        cookingUIEventChannel.OnOpenOrder += AddOrder;
        cookingUIEventChannel.OnSubmitOrder += SubmitOrder;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnOpenOrder -= AddOrder;
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
    }

    // private void LoadStation(StationData station)
    // {
    //     Debug.Log("Loading Station " + station.StationType);
    //     // all station logic is updated on station object
    //     // update menu where? how does it know the data
    // }

    public void AddOrder(Order order)
    {
        order.cookingUIEventChannel = cookingUIEventChannel;
        allOrders.Add(order);
    }
    public void SubmitOrder(Customer customer)
    {
        allOrders.Remove(customer.Data.Order);
        if (customer.Data.Order.isCorrect())
            Debug.Log("Order is correct");
            // other things can happen here like money? etc. like playerMoney += order.Recipe.Price; or something like that

    }

    // Pass event channel trigger to order
    public void OnChangeStation(){
        currentOrder.ChangeNextStation();
    }
}

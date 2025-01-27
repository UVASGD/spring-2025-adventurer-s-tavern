using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderManager2 : MonoBehaviour
{
    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel; // separate event channels

    [SerializeField]
    private Order currentOrder; 

    [SerializeField]
    private List<Order> allOrders; 
    
    [SerializeField]
    private Station currentStation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ASSUMING SET ORDER AND STATION FOR NOW
        foreach( var i in currentStation.ActiveIngredients)
        {
            Debug.Log("station has " + i.Data.Name);
        }
        // LoadStation(currentStation);
    }


    /// Changes the current order to the newly selected order.
    private void SelectOrder(Order selectedOrder)
    {
        Debug.Log("Selected Order " + selectedOrder);
        currentOrder = selectedOrder;
        currentStation = currentOrder.CurrentStation;
        // LoadStation(currentStation);
    }

    // private void LoadStation(Station station)
    // {
    //     Debug.Log("Loading Station " + station.Data.StationType);
    //     // all station logic is updated on station object
    //     // update menu where? how does it know the data
    //     cookingUIEventChannel.RaiseLoadStationView(station);
    // }
}

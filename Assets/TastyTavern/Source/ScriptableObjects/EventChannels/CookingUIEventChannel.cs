using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event Channel to communicate any actions on the Cooking UI to the in-progress Order
/// </summary>
[CreateAssetMenu(fileName = "CookingUIEventChannel", menuName = "ScriptableObjects/EventChannels/OrderEventChannel", order = 0)]
public class CookingUIEventChannel : ScriptableObject {
    
    /// <summary> Callback when an ingredient is added to the working station of the current order </summary>
    public Action<Ingredient> OnAddIngredient;

    /// <summary> Callback when a property is added to the ingredients on the working station of the current order </summary>
    public Action<Property> OnAddProperty;

    public Action<Station> OnLoadStationView;

    public Action<Station> OnRefreshStationView; // TODO: Split refactor refresh station to be part of load

    /// <summary> Callback when an order is opened in the UI </summary>
    public Action<Order> OnOpenOrder;

    /// Callback when an order is submitted in the UI by the player
    public Action<Order> OnSubmitOrder;

    public void RaiseOnAddIngredient(Ingredient ingredient){
        Debug.Log("Raise on " + ingredient.Data.Name + " broadcasted from event channel.");
        OnAddIngredient?.Invoke(ingredient);
    }

    public void RaiseOnAddProperty(Property actionProperty){
        Debug.Log("Raise adding " + actionProperty + " broadcasted from event channel.");
        OnAddProperty?.Invoke(actionProperty);
    }

    public void RaiseOpenOrder(Order Order)
    {
        OnOpenOrder?.Invoke(Order);
    }

    public void RaiseOnSubmitOrder(Order Order)
    {
        OnSubmitOrder?.Invoke(Order);
    }
    public void RaiseOnLoadStationView(Station station){
        Debug.Log("Raise loading " + station.Data.StationType + " broadcasted from event channel.");
        OnLoadStationView?.Invoke(station);
    }

    public void RaiseOnRefreshStationView(Station station){
        OnRefreshStationView?.Invoke(station);
    }

}
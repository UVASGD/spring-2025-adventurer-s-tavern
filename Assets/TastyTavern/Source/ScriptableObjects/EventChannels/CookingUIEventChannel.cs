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
    public Action<ActionData> OnAddProperty;

    public Action<Station> OnLoadStationView;

    public Action<Station> OnRefreshStationWorkspace; // TODO: Split refactor refresh station to be part of load

    public Action<Station> OnRefreshIngredientsPanel;

    /// <summary> TODO: Liam rename, customer makes order--> added to manager </summary>
    public Action<Order> OnCreateOrder;

    public Action<Order> OnSelectOrder;

    public Action<Order> OnGenerateOrderButton;

    /// Callback when an order is submitted in the UI by the player
    public Action OnSubmitOrder;

    /// <summary> Callback when a customer is removed </summary>
    public Action<int> OnRemoveCustomer;

    /// </summary> Callback when the player's money is changed, either by being paid or paying for items </summary>
    public Action<float> OnChangePlayerMoney;

    /// Callback when player wants to go to the next station
    public Action OnChangeNextStation;
    
    public event Action<int> OnDeleteOrderButton;
    
    public event Action OnDeselectOrder;

    public Action OnStoreIngredient;
    
    public void RaiseOnAddIngredient(Ingredient ingredient){
        Debug.Log("Raise adding " + ingredient.Data.Name + " ingredient broadcasted from event channel.");
        OnAddIngredient?.Invoke(ingredient);
    }
    
    public void RaiseOnAddProperty(ActionData actionData){
        Debug.Log("Raise adding " + actionData + " property broadcasted from event channel.");
        OnAddProperty?.Invoke(actionData); 
    }
    
    public void RaiseOpenOrder(Order Order)
    {
        OnCreateOrder?.Invoke(Order);
    }

    public void RaiseOnSubmitOrder()
    {
        OnSubmitOrder?.Invoke();
    }

    public void RaiseOnRemoveCustomer(int idx)
    {
        Debug.Log("Raise removing customer at spot " + idx + " broadcasted from event channel.");
        OnRemoveCustomer?.Invoke(idx);
    }

    public void RaiseOnLoadStationView(Station station){
        Debug.Log("Raise loading " + station.Data.StationType + " broadcasted from event channel.");
        OnLoadStationView?.Invoke(station);
    }

    public void RaiseOnGenerateOrderButton(Order order){
        OnGenerateOrderButton?.Invoke(order);
    }

    public void RaiseOnSelectOrder(Order order)
    {
        OnSelectOrder?.Invoke(order);
    }

    public void RaiseOnRefreshStationWorkspace(Station station){
        OnRefreshStationWorkspace?.Invoke(station);
    }

    public void RaiseOnRefreshIngredientsPanel(Station station){
        OnRefreshIngredientsPanel?.Invoke(station);
    }

    public void RaiseOnChangeNextStation(){
        OnChangeNextStation?.Invoke();
    }

    public void RaiseOnChangePlayerMoney(float money)
    {
        OnChangePlayerMoney?.Invoke(money);
    }


    public void RaiseOnDeselectOrder()
    {
        OnDeselectOrder?.Invoke();
    }

    public void RaiseOnDeleteOrderButton(int obj)
    {
        OnDeleteOrderButton?.Invoke(obj);
    }

    public void RaiseOnStoreIngredient() 
    {
        OnStoreIngredient?.Invoke();
    }
}
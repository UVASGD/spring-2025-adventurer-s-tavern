using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using System;
using System.Linq;

/// Stock ingreityEngine;

/// <summary>
/// This station is a live tracker of the current station and ingredient process of the current order. 
/// It will rodients for each proper station are added on instantiation/station change.
public class Station {

    public StationData Data { get; set; }
    
    public List<Ingredient> StockIngredients { get; set; } = new List<Ingredient>();

    public List<Ingredient> ActiveIngredients { get; set; } = new List<Ingredient>();

    public List<Ingredient> StoredIngredients { get; set; } = new List<Ingredient>();

    public List<List<Ingredient>> AllIngredients { get; set; }

    public CookingUIEventChannel cookingUIEventChannel { get; set; }
    
    public OrderManager OrderManager { get; set; }
    
    public Station(StationData data, List<IngredientData> stock, CookingUIEventChannel ev){
        this.Data = data;
        cookingUIEventChannel = ev;
        foreach (var ingredientData in stock){
            StockIngredients.Add(ingredientData.Create());
        }
        AllIngredients = new(){
            StockIngredients,
            ActiveIngredients,
            StoredIngredients
        };
    }

    public void Subscribe()
    {
        cookingUIEventChannel.OnAddIngredient += AddIngredient;
        cookingUIEventChannel.OnStoreIngredient += StoreActiveIngredients;
    }

    public void Unsubscribe(){
        cookingUIEventChannel.OnAddIngredient -= AddIngredient;
        cookingUIEventChannel.OnStoreIngredient -= StoreActiveIngredients;
    }

    /// Adds ingredient to current active workspace (from stock)
    private void AddIngredient(Ingredient ingredient)
    {
        AddToActive(ingredient);
        cookingUIEventChannel.RaiseOnRefreshStationWorkspace(this);
    }
    
    /// Applies a property to all active ingredients on the station if they don't already have it
    
    public List<Ingredient> ApplyProperty(ActionData actionData)
    {
        foreach (var ingredient in ActiveIngredients)
        {
            if(!ingredient.Properties.Contains(actionData.Property)){
                ingredient.Properties.Add(actionData.Property);
            }
        }
        cookingUIEventChannel.RaiseOnRefreshStationWorkspace(this);
        return ActiveIngredients;
    }

    // Change data, move new Stock and ingredients in Active and Stored to Stock
    public void ProgressStation(StationData data, List<IngredientData> stock)
    {
        Debug.Log("Station changed to " + data.StationType + "in Station.cs");
        this.Data = data;

        //StockIngredients.Clear();
        foreach (var ingredient in stock)
        {
            StockIngredients.Add(ingredient.Create());
        }

        StockIngredients.AddRange(StoredIngredients);
        StockIngredients.AddRange(ActiveIngredients);
        ActiveIngredients.Clear();
        StoredIngredients.Clear();
        cookingUIEventChannel.RaiseOnLoadStationView(this); // refreshes all panels
        // also refresh order instructions
    }

    /// Adds ingredient to current active workspace (from stock)
    /// TODO: Animation into storage?
    public void AddToActive(Ingredient ingredient){
        if (Data.StationType == StationType.CuttingBoard){
            StoreActiveIngredients();
        }
        Debug.Log(ingredient.Data.Name + " added to " + Data.StationType + " in Station.cs");
        ActiveIngredients.Add(ingredient);
        StockIngredients.Remove(ingredient);
    }

    // "SET ASIDE" FUNCTION
    public void StoreActiveIngredients(){
        StoredIngredients.AddRange(ActiveIngredients);
        ActiveIngredients.Clear();
        // TODO: Play little store animation. Transform will be a long parabola and shrink into a little box or chest icon. 
        // Chest icon can do a little shake when the food visually reaches it
    }
    
}


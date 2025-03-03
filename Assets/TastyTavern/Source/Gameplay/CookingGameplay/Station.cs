using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using System;
/// Stock ingreityEngine;

/// <summary>
/// This station is a live tracker of the current station and ingredient process of the current order. 
/// It will rodients for each proper station are added on instantiation/station change.
public class Station {

    public StationData Data { get; set; }
    
    [field: SerializeField]
    public List<Ingredient> StockIngredients { get; set; } = new List<Ingredient>();

    [field: SerializeField]
    public List<Ingredient> ActiveIngredients { get; set; } = new List<Ingredient>();

    [field: SerializeField]
    public List<Ingredient> StoredIngredients { get; set; } = new List<Ingredient>();

    public List<List<Ingredient>> AllIngredients { get; set; }

    [field: SerializeField]
    public CookingUIEventChannel cookingUIEventChannel { get; set; }

    public Station(StationData data, List<IngredientData> stock, CookingUIEventChannel ev){
        this.Data = data;
        cookingUIEventChannel = ev;
        cookingUIEventChannel.OnAddIngredient += AddIngredient;
        foreach (var ingredientData in stock){
            StockIngredients.Add(ingredientData.Create());
        }
        AllIngredients = new(){
            StockIngredients,
            ActiveIngredients,
            StoredIngredients
        };
    }

    public void Unsubscribe(){
        cookingUIEventChannel.OnAddIngredient -= AddIngredient;
    }

    /// Adds ingredient to current active workspace (from stock)
    private void AddIngredient(Ingredient ingredient)
    {
        AddToActive(ingredient);
        cookingUIEventChannel.RaiseOnRefreshStationView(this);
    }
    
    /// Applies a property to all active ingredients on the station if they don't already have it
    
    public void ApplyProperty(ActionData actionData)
    {
        foreach (var ingredient in ActiveIngredients)
        {
            if(!ingredient.Properties.Contains(actionData.Property)){
                ingredient.Properties.Add(actionData.Property);
            }
        }
        cookingUIEventChannel.RaiseOnRefreshStationView(this);
    }

    // Change data, move new Stock and ingredients in Active and Stored to Stock
    public void ChangeStation(StationData data, List<IngredientData> stock){
        Debug.Log("Station changed to "+ data.StationType + "in Station.cs");
        this.Data = data;

        StockIngredients.Clear();
        foreach (var ingredient in stock){
            StockIngredients.Add(ingredient.Create());
        }
        StockIngredients.AddRange(StoredIngredients);
        StockIngredients.AddRange(ActiveIngredients);
        ActiveIngredients.Clear();
        StoredIngredients.Clear();
        cookingUIEventChannel.RaiseOnRefreshStationView(this);
    }

    /// Adds ingredient to current active workspace (from stock)
    /// TODO: Animation into storage?
    public void AddToActive(Ingredient ingredient){
        if (Data.StationType == StationType.CuttingBoard){
            StoreActiveIngredients();
        }
        ActiveIngredients.Add(ingredient);
        StockIngredients.Remove(ingredient);
    }

    // "SET ASIDE" FUNCTION
    public void StoreActiveIngredients(){
        StoredIngredients.AddRange(ActiveIngredients);
        ActiveIngredients.Clear();
    }
    
}


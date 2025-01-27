using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
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

    public Station(StationData data, List<IngredientData> stock){
        this.Data = data;
        foreach (var ingredientData in stock){
            StockIngredients.Add(ingredientData.Create());
        }
        AllIngredients = new(){
            StockIngredients,
            ActiveIngredients,
            StoredIngredients
        };
    }

    // Change data, move new Stock and ingredients in Active and Stored to Stock
    public void ChangeStation(StationData data, List<IngredientData> stock){
        this.Data = data;

        StockIngredients.Clear();
        foreach (var ingredientData in stock){
            StockIngredients.Add(ingredientData.Create());
        }
        StockIngredients.AddRange(StoredIngredients);
        StockIngredients.AddRange(ActiveIngredients);
        ActiveIngredients.Clear();
        StoredIngredients.Clear();
    }

    /// Adds ingredient to current active workspace (from stock)
    /// If the station type is cutting board, store active when a new ingredient is added
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


using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    // One station per order, start off on first station data
    [SerializeField]
    private Station station;

    // TESTING data
    public StationData stationData;
    public List<IngredientData> testStock;

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    public StationController(){
    }

    private void Awake(){
        // open stay on order awake
        this.station = new(stationData,testStock);
        LoadStation();
    }

    // handle other logic

    // subscribe to view evt, model evt, refresh
    // REFRESH VIEW HERE 7:34

    private void OnEnable()
    {
        cookingUIEventChannel.OnAddIngredient += AddIngredient;
        cookingUIEventChannel.OnAddProperty += AddProperty;
    }

    private void OnDisable() 
    {
        cookingUIEventChannel.OnAddIngredient -= AddIngredient;
        cookingUIEventChannel.OnAddProperty -= AddProperty;
    }

    /// Adds ingredient to current active workspace (from stock)
    private void AddIngredient(Ingredient ingredient)
    {
        station.AddToActive(ingredient);
        cookingUIEventChannel.RaiseOnRefreshStationView(station);
    }

    /// Applies a property to all active ingredients on the station if they don't already have it
    private void AddProperty(Property actionProperty)
    {
        foreach (var ingredient in station.ActiveIngredients)
        {
            if(!ingredient.Properties.Contains(actionProperty)){
                ingredient.Properties.Add(actionProperty);
            }
        }
        cookingUIEventChannel.RaiseOnRefreshStationView(station);
    }

    private void LoadStation()
    {
        Debug.Log("Loading Station " + station.Data.StationType);
        cookingUIEventChannel.RaiseOnLoadStationView(station);
    }

}
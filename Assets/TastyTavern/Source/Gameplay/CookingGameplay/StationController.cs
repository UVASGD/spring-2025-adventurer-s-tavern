using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
        cookingUIEventChannel.RaiseOnLoadStationView(this.station);
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnAddIngredient += AddIngredient;
        cookingUIEventChannel.OnAddProperty += StartAddProperty;
    }

    private void OnDisable() 
    {
        cookingUIEventChannel.OnAddIngredient -= AddIngredient;
        cookingUIEventChannel.OnAddProperty -= StartAddProperty;
    }

    // all the below functions can be moved to station, but can't call coroutine from monobehavior?

    /// Adds ingredient to current active workspace (from stock)
    private void AddIngredient(Ingredient ingredient)
    {
        station.AddToActive(ingredient);
        cookingUIEventChannel.RaiseOnRefreshStationView(station);
    }

    private void StartAddProperty(ActionData actionData){
        StartCoroutine(ExecuteAddProperty(actionData));
    }

    private IEnumerator ExecuteAddProperty(ActionData actionData){
        yield return StartCoroutine(WaitBeforeApplyingProperty(actionData.ActionTime));

        ApplyProperty(actionData);
    }  

    private IEnumerator WaitBeforeApplyingProperty(float waitTime){
        yield return new WaitForSeconds(waitTime);
    }
    
    /// Applies a property to all active ingredients on the station if they don't already have it
    
    private void ApplyProperty(ActionData actionData)
    {
        foreach (var ingredient in station.ActiveIngredients)
        {
            if(!ingredient.Properties.Contains(actionData.Property)){
                ingredient.Properties.Add(actionData.Property);
            }
        }
        cookingUIEventChannel.RaiseOnRefreshStationView(station);
    }

  /**
    private void LoadStation()
    {
        Debug.Log("Loading Station " + station.Data.StationType);
        cookingUIEventChannel.RaiseOnLoadStationView(station);
        station.SwitchStation(station.Data, station.StockIngredients);
    }**/
}
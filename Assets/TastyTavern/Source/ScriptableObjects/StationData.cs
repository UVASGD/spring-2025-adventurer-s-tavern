using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StationData", menuName = "ScriptableObjects/StationData", order = 0)]
public class StationData : BuyableData 
{

    // Gameplay information
    [field: SerializeField]
    public StationType StationType { get; set; }

    [field: SerializeField]
    public Sprite[] Sprites { get; set; } // 0: Equipment Top, 1: Equipment Bottom, 2: Background

    [field: SerializeField]
    public float ProcessingTime { get; set; }

    [field: SerializeField]
    public ActionData ActionData { get; set; } 

    // Factory method to make instance of Station
    public Station Create(List<IngredientData> stock, CookingUIEventChannel cookingUIEventChannel, OrderManager manager)
    {
        return new Station(this, stock, cookingUIEventChannel, manager);
    }

}

public enum StationType
{
    CuttingBoard,
    Pan,
    Pot,
    Serving,
    MixingBowl,
    Grill,
    Oven,
    DeepFryer,
}
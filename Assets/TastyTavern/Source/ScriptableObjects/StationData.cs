using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StationData", menuName = "ScriptableObjects/StationData", order = 0)]
public class StationData : ScriptableObject 
{

    [field: SerializeField]
    public StationType StationType { get; set; }

    [field: SerializeField]
    public Sprite Background { get; set; }

    [field: SerializeField]
    public ActionData ActionData { get; set; } // Make Action into Scriptable object too?

    // Factory method to make instance of Station
    public Station Create(List<IngredientData> stock){
        return new Station(this, stock);
    }

}

public enum StationType
{
    CuttingBoard,
    Pan,
    Pot,
    Serving,
}
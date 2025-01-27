using System.Collections.Generic;
using UnityEngine;

public class Ingredient {

    public IngredientData Data { get; set; }

    // Properties added during gameplay. Empty = raw and uncut
    [field: SerializeField]
    public List<Property> Properties { get; set; } = new List<Property>();

    public Ingredient(IngredientData data){
        this.Data = data;
    }

    public override string ToString(){
        string toString = "";
        foreach(var prop in Properties){
            toString += prop + " ";
        }
        toString += Data.Name; 
        return toString;
    }
}

public enum Property{
    Cut,
    Cooked,
    Stewed,
    Salted,
    Peppered,
}
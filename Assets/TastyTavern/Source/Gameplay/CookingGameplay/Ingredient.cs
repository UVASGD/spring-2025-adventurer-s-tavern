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

    // Return the current sprite to be used in the workspace depending on the property
    public Sprite GetCurrentSprite(){
        Sprite sprite = Data.Sprites[0]; // initial is raw, uncut
        if (Properties.Contains(Property.Cut) && Properties.Contains(Property.Battered)){
            sprite = Data.Sprites[3];
        } else if (Properties.Contains(Property.Battered)){
            sprite = Data.Sprites[2];
        } else if (Properties.Contains(Property.Cut)){
            sprite = Data.Sprites[1]; 
        }
        return sprite;
    }
    
}

public enum Property{
    Cut,
    Cooked,
    Battered,
    Boiled,
    DeepFried,
    Mixed,
    Grilled,
    Baked,
}
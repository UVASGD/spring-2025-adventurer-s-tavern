using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject 
{

    [field: SerializeField]
    public List<ShopItem> IngredientItems { get; set; } 

    [field: SerializeField]
    public List<ShopItem> RecipeItems { get; set; } 

    [field: SerializeField]
    public List<ShopItem> EquipmentItems { get; set; } 

    [field: SerializeField]
    public List<ShopItem> BiomeItems { get; set; }  
    
}

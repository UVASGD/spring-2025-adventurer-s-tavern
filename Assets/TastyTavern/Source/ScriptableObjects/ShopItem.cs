using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem")]
public class ShopItem : ScriptableObject 
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public ItemType Type { get; set; }

    // This can be IngredientData, RecipeData, StationData, or BiomeData

    [field: SerializeField]
    public int Price { get; set; } 

    [field: SerializeField]
    public string Description { get; set; }

    [field: SerializeField]
    public Sprite Icon { get; set; } 

    // [field: SerializeField]
    // public bool Purchased { get; set; } = false; //default false?
    
    [field: SerializeField]
    public BuyableData Data { get; set; } 
}

public enum ItemType
{
    Ingredient,
    Recipe,
    Equipment,
    Biome
} 
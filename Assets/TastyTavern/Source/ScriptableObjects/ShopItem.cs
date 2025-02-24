using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem")]
public class ShopItem : ScriptableObject 
{
    //public GameObject entityToSpawn;

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Description { get; set; }

    [field: SerializeField]
    public int Price { get; set; }

    [field: SerializeField]
    public Sprite Icon { get; set; }

    [field: SerializeField]
    public bool Purchased { get; set; }

    public enum ItemType
    {
        Ingredient,
        Recipe,
        Equipment,
        Biome
    } 
    [field: SerializeField]
    public ItemType Type { get; set; }

}

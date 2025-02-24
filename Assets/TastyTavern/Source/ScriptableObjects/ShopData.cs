using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "ScriptableObjects/Shop")]
public class ShopData : ScriptableObject 
{

    [field: SerializeField]
    public List<ShopItem> ShopItems { get; } = new List<ShopItem>();
    
}

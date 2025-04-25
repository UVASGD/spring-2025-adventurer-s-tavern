using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuyableData : ScriptableObject 
{
    [field: SerializeField]
    public string Name { get; set; } 

    [field: SerializeField]
    public int Price { get; set; } 

    [field: SerializeField]
    public string Description { get; set; }
    
}

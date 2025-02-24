using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CustomerData{ // change to class?
    
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public OrderManager OrderManager { get; set; }

    [field: SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    // This will be the eyes, clothes, hair for a character that's randomized
    [field: SerializeField]
    public List<Sprite> Appearance { get; set; } = new List<Sprite>();

    // expressions for neutral, satisfied, and disappointed
    [field: SerializeField]
    public List<Sprite> Faces { get; set; } = new List<Sprite>();

    [field: SerializeField]
    public List<string> Dialogue { get; set; } = new List<string>();// tentative Dialogue[0] = greet, [1] = satisfied, [2] = unsatisfied, [3] = star npc dialogue?

    [field: SerializeField]
    public int Patience { get; set; }

    [field: SerializeField]
    public Order Order { get; set; }

    [field: SerializeField]
    public BiomeData Biome { get; set; } // biome... scriptable object or enum

    public CustomerData(string name, List<Sprite> appearance, List<Sprite> faces, List<string> dialogue, int patience, BiomeData biome)
    {
        Name = name;
        Appearance = appearance;
        Faces = faces;
        Dialogue = dialogue;
        Patience = patience;
        Biome = biome;
        Order = null; // setting later
    }
}

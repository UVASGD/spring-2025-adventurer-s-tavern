using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CustomerData : MonoBehaviour{ // change to class?
    
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
        Order = GenerateOrder();
    }

    private Order GenerateOrder()
    {
        System.Random rand = new System.Random();
        RecipeData recipe = new RecipeData(); // Temporary, replace with a centralized menu system

        // Customization logic
        Dictionary<IngredientData, List<Property>> selectedIngredients = recipe.SelectedIngredients;
        if (rand.Next(0, 4) == 3) // 1/4 chance
        {
            selectedIngredients.Remove(selectedIngredients.ElementAt(rand.Next(0, selectedIngredients.Count - 1)).Key);
        }

        return new Order(null, recipe, selectedIngredients); // Order's "Customer" field will be set later.
    }
}

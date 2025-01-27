using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    // [field: SerializeField]
    // public int OrderSlot { get; set; }

    // the customer that ordered the order
    [field: SerializeField]
    public Customer Customer { get; set; }

    // The recipe of the order
    [field: SerializeField]
    public RecipeData Recipe { get; set; }

    // The exact ingredients of the order. The order may be slightly customized by the customer to exclude certain ingredients. 
    [field: SerializeField]
    public Dictionary<IngredientData, List<Property>> SelectedIngredients { get; set; } = new Dictionary<IngredientData, List<Property>>(); 

    // Whether the order is served or not
    [field: SerializeField]
    public bool Served { get; set; }

    [field: SerializeField]
    public Station CurrentStation { get; set; }

    public Order(Customer Customer, RecipeData recipe, Dictionary<IngredientData, List<Property>> SelectedIngredients)
    {
        Served = false;
        this.Customer = Customer;
        Recipe = recipe;
        this.SelectedIngredients = SelectedIngredients;
        CurrentStation = null; // This needs to be whatever the default station is... I don't know yet
    }

    public bool isCorrect()
    {
        foreach (Ingredient ingredient in CurrentStation.ActiveIngredients)
        {
            if (SelectedIngredients.Keys.Contains(ingredient.Data) && AreListsEqual(SelectedIngredients[ingredient.Data], ingredient.Properties)) return false;
        }
        return true;
    }

    bool AreListsEqual(List<Property> list1, List<Property> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        list1.Sort();
        list2.Sort();

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }
}

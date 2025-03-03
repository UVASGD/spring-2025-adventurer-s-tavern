using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order
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
    public Station Station { get; set; }

    [field: SerializeField]
    public int StationIdx { get; set; }

    [field: SerializeField]
    public CookingUIEventChannel cookingUIEventChannel { get; set; }

    public Order(Customer Customer, RecipeData recipe, Dictionary<IngredientData, List<Property>> SelectedIngredients, CookingUIEventChannel cookingUIEventChannel)
    {
        Served = false;
        this.Customer = Customer;
        Recipe = recipe;
        this.SelectedIngredients = SelectedIngredients;
        StationIdx = 0;
        this.cookingUIEventChannel = cookingUIEventChannel;
        Station = Recipe.StationSequence[0].Create(Recipe.InitialStockSequence[StationIdx].InitialStock, cookingUIEventChannel);

    }

    // Order manager triggers station change
    // for now assuming button is not present at last station
    public void ChangeStation(){
        StationIdx++;
        Station.ProgressStation(Recipe.StationSequence[StationIdx],Recipe.InitialStockSequence[StationIdx].InitialStock);
    }

    public bool isCorrect()
    {
        foreach (Ingredient ingredient in Station.ActiveIngredients)
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

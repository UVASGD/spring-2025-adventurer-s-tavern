using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public List<RecipeData> Menu = new List<RecipeData>();

    public void AddToMenu(RecipeData recipe)
    {
        Menu.Add(recipe);
    }

    public void RemoveFromMenu(RecipeData recipe)
    {
        Menu.Remove(recipe);
    }

    public RecipeData GetRandomRecipeFromMenu()
    {
        return Menu[new System.Random().Next(Menu.Count)];
    }
}

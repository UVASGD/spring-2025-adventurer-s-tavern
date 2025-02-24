using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<RecipeData> Menu = new List<RecipeData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToMenu(RecipeData recipe)
    {
        Menu.Add(recipe);
    }

    public void RemoveFromMenu(RecipeData recipe)
    {
        Menu.Remove(recipe);
    }
}

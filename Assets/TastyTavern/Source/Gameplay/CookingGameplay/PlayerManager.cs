using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float money;

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    //All Ingredients, Equipment, Recipes, and Biome scriptable objects get placed in their respective lists
    public List<IngredientData> allIngredient = new List<IngredientData>();
    public List<ActionData> allEquipment = new List<ActionData>();
    public List<RecipeData> allRecipe = new List<RecipeData>();
    public List<BiomeData> allBiome = new List<BiomeData>();

    //All the scriptable objects get placed into their correct dictionaries, all bools are initially set to false
    private Dictionary<IngredientData, bool> IngredientUnlocked = new Dictionary<IngredientData, bool>();
    private Dictionary<ActionData, bool> ActionUnlocked = new Dictionary<ActionData, bool>();
    private Dictionary<RecipeData, bool> RecipeUnlocked = new Dictionary<RecipeData, bool>();
    private Dictionary<BiomeData, bool> BiomeUnlocked = new Dictionary<BiomeData, bool>();

    void Start()
    {
        //Enter the Ingredients
        for (int i = 0; i < allIngredient.Count; i++)
        {
            IngredientUnlocked.Add(allIngredient[i], false);
        }
        //Enter the Equipment/Actions
        for (int i = 0; i < allEquipment.Count; i++)
        {
            ActionUnlocked.Add(allEquipment[i], false);
        }
        //Enter the Recipes
        for (int i = 0; i < allRecipe.Count; i++)
        {
            RecipeUnlocked.Add(allRecipe[i], false);
        }
        //Enter the Biomes
        for (int i = 0; i < allBiome.Count; i++)
        {
            BiomeUnlocked.Add(allBiome[i], false);
        }
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnChangePlayerMoney += changeMoney;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnChangePlayerMoney -= changeMoney;
    }

    public void changeMoney(float deltaMoney)
    {
        if (deltaMoney < 0 && deltaMoney > money)
        {
            Debug.Log("Not enough money!!");
        }
        else
        {
            money += deltaMoney;
        }
    }
}

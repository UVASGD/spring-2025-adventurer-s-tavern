
using System;
using System.Collections.Generic;
using System.IO;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public int money { get; set; }

    [SerializeField] public MenuManager MenuManager;
    
    public BiomeData currentBiome { get; set; }
    
    // End of day statistics info to send to UIMenusProcesses in the End of day view
    public int moneyAccumulatedThisRound { get; set; }
    public int customersServed { get; set; }

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    //All Ingredients, Equipment, Recipes, and Biome scriptable objects get placed in their respective lists
    public List<IngredientData> allIngredient = new();
    public List<StationData> allEquipment = new();
    public List<RecipeData> allRecipe = new();
    public List<BiomeData> allBiome = new();
    
    public List<BuyableData> initiallyUnlockedItems = new();

    //All the scriptable objects get placed into their correct dictionaries, all bools are initially set to false
    public Dictionary<IngredientData, bool> IngredientUnlocked = new();
    public Dictionary<StationData, bool> StationUnlocked = new();
    public Dictionary<RecipeData, bool> RecipeUnlocked = new();
    public Dictionary<BiomeData, bool> BiomeUnlocked = new();

    void Awake()
    {
        if (!LoadPlayer())
        {
            //Enter the Ingredients
            for (int i = 0; i < allIngredient.Count; i++)
            {
                bool x = false;
                for (int j = 0; j < initiallyUnlockedItems.Count; j++)
                {
                    if (initiallyUnlockedItems[j].Name == allIngredient[i].Name)
                    {
                        x = true;
                        break;
                    }
                }
                IngredientUnlocked.Add(allIngredient[i], x);
            }

            //Enter the Equipment/Stations
            for (int i = 0; i < allEquipment.Count; i++)
            {
                bool x = false;
                for (int j = 0; j < initiallyUnlockedItems.Count; j++)
                {
                    if (initiallyUnlockedItems[j].Name == allEquipment[i].Name)
                    {
                        x = true;
                        break;
                    }
                }
                StationUnlocked.Add(allEquipment[i], x);
            }

            //Enter the Recipes
            for (int i = 0; i < allRecipe.Count; i++)
            {
                bool x = false;
                for (int j = 0; j < initiallyUnlockedItems.Count; j++)
                {
                    if (initiallyUnlockedItems[j].Name == allRecipe[i].Name)
                    {
                        x = true;
                        break;
                    }
                }
                RecipeUnlocked.Add(allRecipe[i], x);
            }

            //Enter the Biomes
            for (int i = 0; i < allBiome.Count; i++)
            {
                bool x = false;
                for (int j = 0; j < initiallyUnlockedItems.Count; j++)
                {
                    if (initiallyUnlockedItems[j].Name == allBiome[i].Name)
                    {
                        x = true;
                        break;
                    }
                }
                BiomeUnlocked.Add(allBiome[i], x);
            }
            money = 300;
            currentBiome = allBiome[0]; //temporary...
            
        }
        // Putting stuff in menu!!!!!!! 
        if (MenuManager != null)
        {
            MenuManager.ForestMenu.Clear();
            MenuManager.OceanMenu.Clear();
            MenuManager.CavesMenu.Clear();

            foreach (var key in RecipeUnlocked.Keys)
            {
                if (RecipeUnlocked[key])
                    if (key.Biome.Name == "Riko Wilds")
                        MenuManager.ForestMenu.Add(key);
                    else if (key.Biome.Name == "Nipawpwa Waves")
                        MenuManager.OceanMenu.Add(key);
                    else
                        MenuManager.CavesMenu.Add(key);
            }
        }
        
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnChangePlayerMoney += ChangeMoney;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnChangePlayerMoney -= ChangeMoney;
    }

    public void ChangeMoney(int deltaMoney)
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
    public void AddItemToInventory(ShopItem item)
    {
        Debug.Log("Item " + item.Name + " added of type " + item.Type);
        switch (item.Type)
        {
            case ItemType.Ingredient:
                foreach (var i in allIngredient)
                    if (i.Name == item.Name) { IngredientUnlocked[i] = true; break; }
                break;

            case ItemType.Equipment:
                foreach (var e in allEquipment)
                    if (e.Name == item.Name) { StationUnlocked[e] = true; break; }
                break;

            case ItemType.Recipe:
                foreach (var r in allRecipe)
                    if (r.Name == item.Name) { RecipeUnlocked[r] = true; break; }
                break;

            case ItemType.Biome:
                foreach (var b in allBiome)
                    if (b.Name == item.Name) {  BiomeUnlocked[b] = true; break; }
                break;
        }
    }

    
    [Serializable]
    private class PlayerData : ISerializationCallbackReceiver
    {
        public int money;
        public BiomeData currBiomeName;
        
        public int moneyAccumulatedThisRound;
        public int customersServed;

        [NonSerialized] public Dictionary<IngredientData, bool> IngredientUnlocked = new();
        [NonSerialized] public Dictionary<StationData, bool> StationUnlocked = new();
        [NonSerialized] public Dictionary<RecipeData, bool> RecipeUnlocked = new();
        [NonSerialized] public Dictionary<BiomeData, bool> BiomeUnlocked = new();

        [SerializeField] private List<IngredientData> ingredientKeys = new();
        [SerializeField] private List<bool> ingredientValues = new();

        [SerializeField] private List<StationData> stationKeys = new();
        [SerializeField] private List<bool> stationValues = new();

        [SerializeField] private List<RecipeData> recipeKeys = new();
        [SerializeField] private List<bool> recipeValues = new();

        [SerializeField] private List<BiomeData> biomeKeys = new();
        [SerializeField] private List<bool> biomeValues = new();

        public void OnBeforeSerialize()
        {
            ingredientKeys.Clear(); ingredientValues.Clear();
            foreach (var kv in IngredientUnlocked)
            {
                ingredientKeys.Add(kv.Key);
                ingredientValues.Add(kv.Value);
            }

            stationKeys.Clear(); stationValues.Clear();
            foreach (var kv in StationUnlocked)
            {
                stationKeys.Add(kv.Key);
                stationValues.Add(kv.Value);
            }

            recipeKeys.Clear(); recipeValues.Clear();
            foreach (var kv in RecipeUnlocked)
            {
                recipeKeys.Add(kv.Key);
                recipeValues.Add(kv.Value);
            }

            biomeKeys.Clear(); biomeValues.Clear();
            foreach (var kv in BiomeUnlocked)
            {
                biomeKeys.Add(kv.Key);
                biomeValues.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            IngredientUnlocked = new();
            for (int i = 0; i < Math.Min(ingredientKeys.Count, ingredientValues.Count); i++)
                IngredientUnlocked[ingredientKeys[i]] = ingredientValues[i];

            StationUnlocked = new();
            for (int i = 0; i < Math.Min(stationKeys.Count, stationValues.Count); i++)
                StationUnlocked[stationKeys[i]] = stationValues[i];

            RecipeUnlocked = new();
            for (int i = 0; i < Math.Min(recipeKeys.Count, recipeValues.Count); i++)
                RecipeUnlocked[recipeKeys[i]] = recipeValues[i];

            BiomeUnlocked = new();
            for (int i = 0; i < Math.Min(biomeKeys.Count, biomeValues.Count); i++)
                BiomeUnlocked[biomeKeys[i]] = biomeValues[i];
        }
    }
    
    private PlayerData CreatePlayerDataFromManager()
    {
        var data = new PlayerData
        {
            money = this.money,
            currBiomeName = this.currentBiome,
            moneyAccumulatedThisRound = this.moneyAccumulatedThisRound,
            customersServed = this.customersServed,
        };

        foreach (var kv in IngredientUnlocked)
            data.IngredientUnlocked[kv.Key] = kv.Value;

        foreach (var kv in StationUnlocked)
            data.StationUnlocked[kv.Key] = kv.Value;

        foreach (var kv in RecipeUnlocked)
            data.RecipeUnlocked[kv.Key] = kv.Value;

        foreach (var kv in BiomeUnlocked)
            data.BiomeUnlocked[kv.Key] = kv.Value;

        return data;
    }

    private void ApplyPlayerDataToManager(PlayerData data)
    {
        money = data.money;
        currentBiome = data.currBiomeName;
        moneyAccumulatedThisRound = data.moneyAccumulatedThisRound;
        customersServed = data.customersServed;

        foreach (var ingredient in allIngredient)
            IngredientUnlocked[ingredient] = false;
        foreach (var equipment in allEquipment)
            StationUnlocked[equipment] = false;
        foreach (var recipe in allRecipe)
            RecipeUnlocked[recipe] = false;
        foreach (var biome in allBiome)
            BiomeUnlocked[biome] = false;

        foreach (var ingredientData in data.IngredientUnlocked.Keys)
        {
            var obj = allIngredient.Find(i => i == ingredientData);
            if (obj != null)
                IngredientUnlocked[obj] = data.IngredientUnlocked[ingredientData];
        }

        foreach (var stationData in data.StationUnlocked.Keys)
        {
            var obj = allEquipment.Find(e => e == stationData);
            if (obj != null)
                StationUnlocked[obj] = data.StationUnlocked[stationData];
        }

        foreach (var recipeData in data.RecipeUnlocked.Keys)
        {
            var obj = allRecipe.Find(r => r == recipeData);
            if (obj != null)
                RecipeUnlocked[obj] = data.RecipeUnlocked[recipeData];
        }

        foreach (var biomeData in data.BiomeUnlocked.Keys)
        {
            var obj = allBiome.Find(b => b == biomeData);
            if (obj != null)
                BiomeUnlocked[obj] = data.BiomeUnlocked[biomeData];
        }
    }

    public void SavePlayer(string filename = "player_save.json")
    {
        var data = CreatePlayerDataFromManager();
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, filename), json);
        Debug.Log("Player saved to " + Path.Combine(Application.persistentDataPath, filename));
    }

    public bool LoadPlayer(string filename = "player_save.json")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(path))
        {
            return false;
        }

        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<PlayerData>(json);
        ApplyPlayerDataToManager(data);
        Debug.Log("Player loaded from " + path);
        return true;
    }
}

using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour
{
    public List<RecipeData> ForestMenu = new();
    public List<RecipeData> OceanMenu = new();
    public List<RecipeData> CavesMenu = new();

    public RecipeData GetRandomRecipeFromMenu(BiomeData biome)
    {
        switch (biome.Name)
        {
            case "Riko Wilds":
                return ForestMenu[new System.Random().Next(ForestMenu.Count)];
            case "Nipawpwa Waves":
                return OceanMenu[new System.Random().Next(OceanMenu.Count)];
            case "Mungtown Caves":
                return CavesMenu[new System.Random().Next(CavesMenu.Count)];
        }

        throw new IndexOutOfRangeException();
    }
}

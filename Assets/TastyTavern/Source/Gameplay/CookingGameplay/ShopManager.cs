/* Shop: - A class that represents a shop in the game. It has an inventory of items that can be bought and sold, and prices for each item.
        - The player can buy items from the shop if they have enough money.
        - Once a user completes a purchase, the listing for that item is updated to say something like 'in kitchen/stock'.
        - Once a user buys an item, they do not need to buy it again. The item is available for use in the kitchen. (Buying = unlocking)
        - Items PC can buy: Ingredients, Recipes, Kitchen Upgrades (Equipment), Biomes

        Notes:
        - Item state: lock/unlock
        - Make a shop scriptable object so UI will just need to reference it
        
        - Shop will have a list of items, each with a name, price, status, icon, and description
        - Separate script for shop functions (buying, adding to player inventory, changing status of items in shop)
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public ShopData currentShopData; // DETERMINED IN GAMEPLAY

    // Shops, set in the editor: (Forest, Ocean, Caves)
    [SerializeField]
    private List<ShopData> allShops;

    [SerializeField]
    private PlayerManager playerManager; // NEED TO CONNECT

    public BiomeData currentBiome; 

    void Start()
    {
        currentBiome = playerManager.currentBiome; // Get the current biome from the PlayerManager
        if (currentBiome.Name == "Riko Wilds"){
            currentShopData = allShops[0];
        } else if (currentBiome.Name == "Nipawpwa Waves"){
            currentShopData = allShops[1];
        } else if (currentBiome.Name == "Mungtown Caverns"){
            currentShopData = allShops[2];
        } else {
            Debug.Log("Biome not found!");
        }
    }


    /* Player can buy an item under the following conditions
        1. They have enough gold
        2. The item is not already purchased
        3. Display only stuff for biome chosen
        4. Recipies cannot be bought unless all ingredients are unlocked
    If the player meets these conditions, the item is marked as purchased and the player's gold is reduced by the item's price
    public bool BuyItem(ShopItem item, ref int playerGold) 
    */
    public bool BuyItem(ShopItem item)
    {
        if (playerManager.money >= item.Price)
        {
            playerManager.money -= item.Price;
            Debug.Log($"You bought {item.Data.Name} for {item.Price} gold!");
            playerManager.AddItemToInventory(item);
            return true;
        }
        else
        {
            Debug.Log("You don't have enough gold to buy that item!");
            return false;
        }
    }

    public bool IsItemPurchased(ShopItem item)
    {
        if (item.Type == ItemType.Recipe && playerManager.RecipeUnlocked[(RecipeData)item.Data] == true)
        {
            return true;
        }
        else if (item.Type == ItemType.Ingredient && playerManager.IngredientUnlocked[(IngredientData)item.Data] == true)
        {
            return true;
        }
        else if (item.Type == ItemType.Equipment && playerManager.StationUnlocked[(StationData)item.Data] == true)
        {
            return true;
        }
        else if (item.Type == ItemType.Biome && playerManager.BiomeUnlocked[(BiomeData)item.Data] == true)
        {
            return true;
        } else {
            return false;
        }
    }
}
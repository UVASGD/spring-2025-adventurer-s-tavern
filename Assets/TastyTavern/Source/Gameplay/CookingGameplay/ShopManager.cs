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

    [SerializeField]
    public ShopData CurrentShopData { get; set; } // RECIEVED IN GAMEPLAY

    private PlayerManager playerManager;

    [SerializeField]
    public BiomeData currentBiome; // TESTING ONLY

    void Awake()
    {
        // if (currentBiome.Name == "Forest"){
        //     CurrentShopData = Resources.Load<ShopData>("ScriptableObjects/Shop/PlainsShop");
        //     Debug.Log("No biome found!");
        // } else {
        //     Debug.Log("Biome not found!");
        // }
    
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
            if (item.Type == ItemType.Recipe && playerManager.RecipeUnlocked[(RecipeData)item.Data] == true)
            {
                Debug.Log("You already bought that item!");
                return false;
            }
            else if (item.Type == ItemType.Ingredient && playerManager.IngredientUnlocked[(IngredientData)item.Data] == true)
            {
                Debug.Log("You already bought that item!");
                return false;
            }
            else if (item.Type == ItemType.Equipment && playerManager.StationUnlocked[(StationData)item.Data] == true)
            {
                Debug.Log("You already bought that item!");
                return false;
            }
            else if (item.Type == ItemType.Biome && playerManager.BiomeUnlocked[(BiomeData)item.Data] == true)
            {
                Debug.Log("You already bought that item!");
                return false;
            } else {
                playerManager.money -= item.Price;
                Debug.Log($"You bought {item.Data.Name} for {item.Price} gold!");
                playerManager.AddItemToInventory(item);
                return true;
            }
        }
        else
        {
            Debug.Log("You don't have enough gold to buy that item!");
            return false;
        }
    }
}
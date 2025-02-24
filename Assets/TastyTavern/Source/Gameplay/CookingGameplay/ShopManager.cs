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

public class ShopManager
{
    private Dictionary<string, int> inventory;
    // Has: ingredients, recipies, equipment, biomes
    // private Dictionary<string, int> prices; -> each object has its own price field

    public ShopManager()
    {
        inventory = new Dictionary<string, int>(); // list of arrays where each array = item [name, price, status, icon, description]
        //prices = new Dictionary<string, int>();
    }

    public void AddItem(string itemName, int quantity, int price)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += quantity;
        }
        else
        {
            inventory[itemName] = quantity;
            //prices[itemName] = price;
        }
    }

    // public bool BuyItem(string itemName, int quantity, ref int playerGold)
    // {
    //     if (inventory.ContainsKey(itemName) && inventory[itemName] >= quantity)
    //     {
    //         int totalCost = prices[itemName] * quantity;
    //         if (playerGold >= totalCost)
    //         {
    //             inventory[itemName] -= quantity;
    //             playerGold -= totalCost;
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // public void DisplayInventory()
    // {
    //     Console.WriteLine("Shop Inventory:");
    //     foreach (var item in inventory)
    //     {
    //         Console.WriteLine($"{item.Key}: {item.Value} available at {prices[item.Key]} gold each");
    //     }
    // }
}
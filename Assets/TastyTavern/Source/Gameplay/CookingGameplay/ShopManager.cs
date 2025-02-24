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
    private List<ShopItem> inventory;
    public ShopManager()
    {
        inventory = ShopData.ShopItems;
    }

    /* Player can buy an item under the following conditions
        1. They have enough gold
        2. The item is not already purchased
    If the player meets these conditions, the item is marked as purchased and the player's gold is reduced by the item's price
    public bool BuyItem(ShopItem item, ref int playerGold) 
    */
    public bool BuyItem(ShopItem item, ref int playerGold)
    {
        playerGold = PlayerManager.Money;
        if (playerGold >= item.Price)
        {
            if (item.Purchased = true)
            {
                Debug.Log("You already bought that item!");
                return false;
            } else {
                playerGold -= item.Price;
                item.Purchased = true;
                Debug.Log($"You bought {item.Name} for {item.Price} gold!");
                //how to update information in player?

                return true;
            }
        }
        else
        {
            Debug.Log("You don't have enough gold to buy that item!");
            return false;
        }
    }

    // public void DisplayInventory()
    // {
    //     Debug.Log("Shop Inventory:");
    //     foreach (var item in inventory)
    //     {
    //         Debug.Log($"{item.Name}: {item.Description}, {item.Price} gold, Purchased: {item.Purchased}");
    //     }
    // }
}
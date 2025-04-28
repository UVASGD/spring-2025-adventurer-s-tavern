using System.Collections.Generic;
using UnityEngine;

public class StationWorkspace : MonoBehaviour
{
    // The slots where ingredients are placed in the workspace
    [field: SerializeField]
    private List<SpriteRenderer> _slots { get; set; }

    // The station data associated with this workspace
    [field: SerializeField]
    public StationData stationData { get; set; } 

    void Start()
    {
        ClearWorkspace();
    }

    // single slot: uncut -> cut, cut -> cut_battered, uncut -> uncut_battered
    public void AddToWorkspace(Ingredient ingredient)
    {
        if (stationData.StationType == StationType.CuttingBoard || stationData.StationType == StationType.MixingBowl)
        {
            AddIngredientToWorkspace(ingredient, true);
        } else {
            AddIngredientToWorkspace(ingredient, false);
        }
    }

    public void AddIngredientToWorkspace(Ingredient ingredient, bool isSingleSlot = false)
    { 
        Sprite ingredientSprite = ingredient.GetCurrentSprite();

        // Check for conditions for single slot + replacing (cutting + battering(bowl))
        // Also used with CUT and BATTERED
        if (isSingleSlot && _slots[0].gameObject.activeSelf)
        {
            _slots[0].sprite = ingredientSprite;
        } else {
            // Find the first deactivated slot and activate it with the sprite
            foreach (SpriteRenderer slot in _slots)
            {
                if (!slot.gameObject.activeSelf)
                {
                    slot.gameObject.SetActive(true);
                    slot.sprite = ingredientSprite;
                    break; 
                }
            }
        }
    }


    public void ClearWorkspace()
    {
        foreach (SpriteRenderer slot in _slots)
        {
            slot.gameObject.SetActive(false);
            slot.sprite = null; 
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class StationWorkspace : MonoBehaviour
{
    [field: SerializeField]
    public List<SpriteRenderer> _slots { get; set; }

    void Start()
    {
        ClearWorkspace();
    }

    public void AddToWorkspace(Ingredient ingredient, bool isSingleSlot = false)
    {
        Texture ingredientTexture = ingredient.GetCurrentSprite().texture;

        // Check for conditions for single slot + replacing (cutting + battering(bowl))
        // Also used with CUT and BATTERED
        if (isSingleSlot && _slots[0].gameObject.activeSelf)
        {
            _slots[0].sprite = ingredientTexture;
        } else {

            // TODO: Check for serving station condition

            // Find the first deactivated slot and activate it with the sprite
            foreach (SpriteRenderer slot in _slots)
            {
                if (!slot.gameObject.activeSelf)
                {
                    slot.gameObject.SetActive(true);
                    slot.sprite = ingredientTexture;
                    break; 
                }
            }
        }
    }

    // Modify first slot: 
    // uncut -> cut, cut -> cut_battered, uncut -> uncut_battered
    public void ApplyPropertyUpdate(Ingredient ingredient){
        AddToWorkspace(ingredient, true); 
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
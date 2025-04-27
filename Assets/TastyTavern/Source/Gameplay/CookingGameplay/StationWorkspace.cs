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

        // Get the texture of the ingredient, see IngredientData for more
        Texture2D ingredientTexture = ingredient.Data.Sprites[0].texture; // initial is raw, uncut
        if (ingredient.Properties.Contains(Property.Cut) && ingredient.Properties.Contains(Property.Battered)){
            sprite = ingredient.Data.Sprites[3];
        } else if (ingredient.Properties.Contains(Property.Battered)){
            sprite = ingredient.Data.Sprites[2];
        } else if (ingredient.Properties.Contains(Property.Cut)){
            sprite = ingredient.Data.Sprites[1]; 
        }

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

    void ClearWorkspace()
    {
        foreach (SpriteRenderer slot in _slots)
        {
            slot.gameObject.SetActive(false);
            slot.sprite = null; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
using System.Collections.Generic;
using UnityEngine;

public class StationWorkspace : MonoBehaviour
{
    //TODO: Add specific station tops: Burner, Grill, Fryer, etc.

    // Equipment sprites, the top overlays ingredients, while bottom layers behind them. Set in the inspector.
    [field: SerializeField]
    public SpriteRenderer _equipmentTop { get; set; }

    [field: SerializeField]
    public SpriteRenderer _equipmentBottom { get; set; }

    [field: SerializeField]
    public SpriteRenderer _cuttingBoardSlot { get; set; }

    [field: SerializeField]
    public List<SpriteRenderer> _panSlots { get; set; }

    [field: SerializeField]
    public List<SpriteRenderer> _grillSlots { get; set; }

    [field: SerializeField]
    public List<SpriteRenderer> _potSlots { get; set; }

    [field: SerializeField]
    public List<SpriteRenderer> _mixingBowlSlots { get; set; }

    [field: SerializeField]
    public GameObject _fryerSlot { get; set; }

    // This script is attached to the Station Workspace prefab

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddToStationWorkspace(Ingredient ingredient){
        // only add to active station type


        // Add ingredient to workspace
        // GameObject ingredientObject = new GameObject(ingredient.ToString());
        // ingredientObject.transform.SetParent(_ingredients.transform);
        // ingredientObject.transform.localPosition = Vector3.zero;
        // ingredientObject.AddComponent<SpriteRenderer>().sprite = ingredient.Data.Sprites[0].texture;
        // ingredientObject.AddComponent<BoxCollider2D>();
        // ingredientObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void AddToWorkspace(Ingredient ingredient){
        // adding to slot
        // Add sprite to first deactivated slot
        Sprite cur_sprite = ingredient.GetCurrentSprite();

        for (int i = 0; i < _panSlots.Count; i++)
        {
            if (!_panSlots[i].enabled)
            {
                _panSlots[i].enabled = true;
                _panSlots[i].sprite = cur_sprite;
                return;
            }
        }
    }
}

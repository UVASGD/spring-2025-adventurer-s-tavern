using System.Collections.Generic;
using UnityEngine;

public class StationWorkspaceController : MonoBehaviour
{
    //TODO: Add specific station tops: Burner, Grill, Fryer, etc.

    // Equipment sprites, the top overlays ingredients, while bottom layers behind them. Set in the inspector.
    [field: SerializeField]
    public SpriteRenderer _EquipmentTop { get; set; }

    [field: SerializeField]
    public SpriteRenderer _EquipmentBottom { get; set; }

    [field: SerializeField]
    public SpriteRenderer _StationBackground { get; set; }

    [field: SerializeField]
    public StationWorkspace _CurrentWorkspace { get; set; }

    [field: SerializeField]
    public StationWorkspace[] _Workspaces { get; set; }

    [field: SerializeField]
    public CookingUIEventChannel cookingUIEventChannel { get; set; }

    // This script is attached to the Station Workspace prefab
    private void Start()
    {
        _CurrentWorkspace = null; //no workspace yet.
        _EquipmentTop.sprite = null; //no equipment yet.
        _EquipmentBottom.sprite = null; //no equipment yet.
        _StationBackground.sprite = null; //no background yet.
    }

    void OnEnable()
    {
        // Subscribe to events
        cookingUIEventChannel.OnLoadStationView += RotateStation;
        cookingUIEventChannel.OnUpdateWorkspace += UpdateStationWorkspace;
        cookingUIEventChannel.OnWorkspaceAssemble += AssembleOrder;
        cookingUIEventChannel.OnStoreIngredient += ClearActiveWorkspace;
        cookingUIEventChannel.OnSubmitOrder += CloseWorkspace;
    }

    void OnDisable()
    {
        // Unsubscribe from events
        cookingUIEventChannel.OnLoadStationView -= RotateStation;
        cookingUIEventChannel.OnUpdateWorkspace -= UpdateStationWorkspace;
        cookingUIEventChannel.OnWorkspaceAssemble -= AssembleOrder;
        cookingUIEventChannel.OnStoreIngredient -= ClearActiveWorkspace;
        cookingUIEventChannel.OnSubmitOrder -= CloseWorkspace;
    }

    private void RotateStation(Station station){
        // TODO: swap station background + equipment sprites
        if (_CurrentWorkspace != null){
            _CurrentWorkspace.ClearWorkspace();
        }

        _StationBackground.sprite = station.Data.Sprites[0];
        _EquipmentTop.sprite = station.Data.Sprites[1];
        _EquipmentBottom.sprite = station?.Data.Sprites[2];

        switch (station.Data.StationType){
            case StationType.CuttingBoard:
                _CurrentWorkspace = _Workspaces[0];
                break;
            case StationType.Pan:
                _CurrentWorkspace = _Workspaces[1];
                break;
            case StationType.Grill:
                _CurrentWorkspace = _Workspaces[2];
                break;
            case StationType.MixingBowl:
                _CurrentWorkspace = _Workspaces[3];
                break;
            case StationType.DeepFryer:
                _CurrentWorkspace = _Workspaces[4];
                break;
            case StationType.Oven:
                _CurrentWorkspace = _Workspaces[5];
                break;
            case StationType.Pot:
                _CurrentWorkspace = _Workspaces[6];
                break;
            case StationType.Serving:
                _CurrentWorkspace = _Workspaces[7];
                break;
        }

        _CurrentWorkspace.stationData = station.Data;
    }

    // can probably director call in subscription
    private void UpdateStationWorkspace(Ingredient ingredient){
        _CurrentWorkspace.AddToWorkspace(ingredient);
    }

    private void AssembleOrder(Sprite sprite){
        _CurrentWorkspace.AssembleOrder(sprite);
    }

    private void ClearActiveWorkspace(){
        _CurrentWorkspace.ClearWorkspace();
    }

    private void CloseWorkspace(Order order){
        _CurrentWorkspace.ClearWorkspace();
        _EquipmentTop.sprite = null;
        _EquipmentBottom.sprite = null;
        _StationBackground.sprite = null;
    }
}

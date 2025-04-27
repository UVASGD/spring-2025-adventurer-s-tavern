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
    public List<StationWorkspace> _Workspaces { get; set; }

    // This script is attached to the Station Workspace prefab

    private void RotateStation(Station station){
        // TODO: swap station background + equipment sprites
        _CurrentWorkspace.ClearWorkspace();
        _EquipmentTop.sprite = station.Data.StationSprites[0];
        _EquipmentBottom.sprite = station.Data.StationSprites[1];
        _StationBackground.sprite = station.Data.StationSprites[2];

        switch (station.Data.StationType){
            case StationType.CuttingBoard:
                _CurrentWorkspace = station.Data.StationWorkspaces[0];
                break;
            case StationType.Pan:
                _CurrentWorkspace = station.Data.StationWorkspaces[1];
                break;
            case StationType.Grill:
                _CurrentWorkspace = station.Data.StationWorkspaces[2];
                break;
            case StationType.MixingBowl:
                _CurrentWorkspace = station.Data.StationWorkspaces[3];
                break;
            case StationType.DeepFryer:
                _CurrentWorkspace = station.Data.StationWorkspaces[4];
                break;
            case StationType.Oven:
                _CurrentWorkspace = station.Data.StationWorkspaces[5];
                break;:
            case StationType.Pot:
                _CurrentWorkspace = station.Data.StationWorkspaces[6];
                break;
            case StationType.Serving:
                _CurrentWorkspace = station.Data.StationWorkspaces[7];
                break;
        }
    }

    private void AddToStationWorkspace(Ingredient ingredient, Station station){
        if (station.Data.StationType == StationType.CuttingBoard || station.Data.StationType == StationType.MixingBowl){
            _CurrentWorkspace.AddToWorkspace(ingredient,true); // single slot
        } else {
            _CurrentWorkspace.AddToWorkspace(ingredient); // multiple slots
        }
    }
}

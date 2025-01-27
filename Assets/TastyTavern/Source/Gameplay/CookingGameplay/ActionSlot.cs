using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionSlot : Button {
    public Image Icon;
    public Label Label = new();

    public ActionData ActionData { get; set; }

    public event Action<ActionSlot> OnClickAction = delegate { };

    public ActionSlot(ActionData actionData)
    {
        ActionData = actionData;

        Icon = new()
        {
            image = ActionData.Sprite.texture
        };

        Label.text = ActionData.Name;

        Icon.AddToClassList("action-icon");
        Label.AddToClassList("action-label");
        this.Add(Icon);
        this.Add(Label);

        this.clicked += OnClick;
    }

    private void OnClick() {
        Debug.Log("Clicked " + ActionData.Name + " Action");
        OnClickAction.Invoke(this);
    }

}


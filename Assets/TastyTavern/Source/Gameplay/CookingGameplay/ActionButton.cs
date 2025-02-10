using System;
using UnityEngine;
using UnityEngine.UIElements;

// Inherits helper classes and returns itself on OnClick() with OnClickButton delegate
public class ActionButton : DataButton {

    public ActionData Data { get; set; }

    public event Action<ActionButton> OnClickButton = delegate { };

    public ActionButton(ActionData data) 
    {
        Data = data;
        Icon = new(){ image = Data.Sprite.texture };
        Label.text = Data.Name;

        AddStyles();
        AttachIcon();
        AttachLabel();
    }

    protected override void OnClick(){
        OnClickButton.Invoke(this);
    }
    
    protected override void AddStyles(){
        Icon.AddToClassList("action-icon");
        Label.AddToClassList("action-label");
    }

}

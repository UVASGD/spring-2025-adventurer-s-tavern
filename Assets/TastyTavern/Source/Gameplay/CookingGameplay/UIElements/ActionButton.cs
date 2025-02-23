using System;
using UnityEngine;
using UnityEngine.UIElements;

// Inherits helper classes and returns itself on OnClick() with OnClickButton delegate
public class ActionButton : DataButton {

    public ActionData Data { get; private set; }

    public event Action<ActionButton> OnClickButton = delegate { };

    public ActionButton(ActionData data) 
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));

    if (Data.Sprite != null){
        Icon = new() { image = Data.Sprite.texture };
    }
    else{
        Debug.LogWarning("ActionButton: Data.Sprite is null!");
    }




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
        this.AddToClassList("action-button");
        this.AddToClassList("button");
    }

}

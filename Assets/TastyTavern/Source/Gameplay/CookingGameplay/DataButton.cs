using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DataButton : Button {

    public Label Label = new();
    public Image Icon;

    public DataButton()
    {
        this.clicked += OnClick;
    }

    protected void AttachLabel(){
        this.Add(Label);
    }

    protected void AttachIcon(){
        this.Add(Icon);
    }

    // in concrete child, invoke a delegate
    protected abstract void OnClick();
    
    protected abstract void AddStyles();

}

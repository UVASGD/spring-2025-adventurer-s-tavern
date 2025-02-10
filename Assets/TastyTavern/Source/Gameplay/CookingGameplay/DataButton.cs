using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DataButton : Button {

    public Label Label = new();
    public Image Icon;

    // This delegate is used to forward OnClick to StationView, where unique logic is applied via subscription
    public event Action<DataButton> OnClickButton = delegate { };

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

    protected abstract void OnClick();
    
    protected abstract void AddStyles();

}

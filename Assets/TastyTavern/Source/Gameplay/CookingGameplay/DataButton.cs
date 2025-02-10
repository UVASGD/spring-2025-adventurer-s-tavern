using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DataButton : Button {
    
    public Label Label = new();

    public ScriptableObject Data { get; set; }

    // This delegate is used to forward OnClick to StationView, where unique logic is applied via subscription
    public event Action<DataButton> OnClickButton = delegate { };

    public DataButton(ScriptableObject data)
    {
        Data = data;
        this.clicked += OnClick;
    }

    protected void AttachLabel(){
        this.Add(Label);
    }

    private void OnClick(){
        OnClickButton.Invoke(this);
    }
    
    protected abstract void AddStyles();

}

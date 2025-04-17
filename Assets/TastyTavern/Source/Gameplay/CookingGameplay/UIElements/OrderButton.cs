using System;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderButton : DataButton {

    public Order Order { get; private set; }
    public ProgressBar ProgressBar { get; private set; }
    private VisualElement progressBarContainer {get; set;}
    public event Action<OrderButton> OnClickButton = delegate { };

    public OrderButton(Order order, VisualElement progressBarContainer) 
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        Label.text = Order.Recipe.Name;
        ProgressBar = progressBarContainer.Q<ProgressBar>("ProgressBar");
        
        /*

         if (Order.Sprite != null)
        {Icon = new() { image = Order.Sprite.texture };}
        else
        {Debug.LogWarning("OrderButton: Order.Sprite is null!");}

         Label.text = Order.Name;

        */

        AddStyles();
        // AttachIcon(); for now
        AttachLabel();
        progressBarContainer.Add(ProgressBar);
        this.Add(progressBarContainer);
    }

    protected override void OnClick(){
        OnClickButton.Invoke(this);
    }
    
    protected override void AddStyles(){
        // Icon.AddToClassList("order-icon");
        Label.AddToClassList("order-label");
        this.AddToClassList("order-button");
        this.AddToClassList("button");
    }
    
    

}

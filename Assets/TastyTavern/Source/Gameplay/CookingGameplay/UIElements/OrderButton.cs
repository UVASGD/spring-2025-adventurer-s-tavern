using System;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderButton : DataButton {

    

    public Order Order { get; private set; }

    public event Action<OrderButton> OnClickButton = delegate { };

    public OrderButton(Order order) 
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));

        /*

         if (Order.Sprite != null)
        {
             Icon = new() { image = Order.Sprite.texture };
        }
        else
        {
            Debug.LogWarning("OrderButton: Order.Sprite is null!");
        }

         Label.text = Order.Name;

        */

        AddStyles();
        AttachIcon();
        AttachLabel();
    }

    protected override void OnClick(){
        OnClickButton.Invoke(this);
    }
    
    protected override void AddStyles(){
        Icon.AddToClassList("order-icon");
        Label.AddToClassList("order-label");
        this.AddToClassList("order-button");
        this.AddToClassList("button");
    }

}

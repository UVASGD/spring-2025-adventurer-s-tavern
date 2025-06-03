using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderButton : DataButton {

    public Order Order { get; private set; }
    // public ProgressBar PatienceBar { get; private set; }
    private VisualElement progressBarContainer {get; set;} 
    public event Action<OrderButton> OnClickButton = delegate { };

    public OrderButton(Order order, VisualElement progressBarContainer) 
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));

        // PatienceBar = new ProgressBar
        // {
        //     title = "Patience",
        //     lowValue = 0f,
        //     highValue = 100f,
        //     value = 0f,
        // };

        // Icon = new() { image = Order.Recipe.DoneIcon.texture };
        Label.text = Order.Recipe.Name;
        AddStyles();
        AttachLabel();
        // AttachIcon(); 
        
        
        // SerializedObject so = new SerializedObject(order.Customer);
        // SerializedProperty sp = so.FindProperty("Health.CurrentHealth");

        // PatienceBar.Bind(so);
        // // PatienceBar.BindProperty(sp);
        // //PatienceBar.TrackPropertyValue(sp, //callback );
        // progressBarContainer.Add(PatienceBar);
        // this.Add(progressBarContainer);
        this.AddToClassList("order-button");
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

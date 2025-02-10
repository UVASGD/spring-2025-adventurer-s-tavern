using System;
using UnityEngine;
using UnityEngine.UIElements;

// Inherits helper classes and returns itself on OnClick() with OnClickButton delegate
public class IngredientButton : DataButton {

    public Ingredient Ingredient { get; set; }

    // In StationView, each generated IngredientButton subscribes OnAddIngredient to this
    public event Action<IngredientButton> OnClickButton = delegate { };

    public IngredientButton(Ingredient ingredient) 
    {
        Ingredient = ingredient;
        Icon = new(){ image = Ingredient.Data.Sprites[0].texture };
        Label.text = Ingredient.Data.Name;

        AddStyles();
        AttachIcon();
        AttachLabel();
    }

    protected override void OnClick(){
        OnClickButton.Invoke(this);
    }
    
    protected override void AddStyles(){
        Icon.AddToClassList("ingredient-icon");
        Label.AddToClassList("ingredient-label");
        this.AddToClassList("ingredient-slot"); // move to button class?
        this.AddToClassList("slot");
    }

}

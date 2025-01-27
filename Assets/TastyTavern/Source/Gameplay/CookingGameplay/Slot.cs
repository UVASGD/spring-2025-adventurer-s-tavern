using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot : Button {
    public Image Icon;
    public Label Label = new();

    public Ingredient Ingredient { get; set; }

    public event Action<Slot> OnClickIngredient = delegate { };

    public Slot(Ingredient ingredient)
    {
        Ingredient = ingredient;

        Icon = new()
        {
            image = Ingredient.Data.Sprites[0].texture
        };

        Label.text = Ingredient.Data.Name;

        Icon.AddToClassList("ingredient-icon");
        Label.AddToClassList("ingredient-label");
        this.Add(Icon);
        this.Add(Label);

        this.clicked += OnClick;
    }

    private void OnClick() {
        Debug.Log("Clicked " + Ingredient.Data.Name + " Ingredient");
        OnClickIngredient.Invoke(this);
    }

}


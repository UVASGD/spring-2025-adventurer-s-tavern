using System;
using System.Collections.Generic;
using System.Linq;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEditor;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [SerializeField]
    private ProgressBar progressBar;  // Reference to ProgressBar


    [SerializeField]
    private CustomerController customerController;

    [SerializeField]
    public Order currentOrder; 

    [SerializeField]
    private List<Order> activeOrders = new();
    
    [SerializeField] public PlayerManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnCreateOrder += AddOrder;
        cookingUIEventChannel.OnSubmitOrder += SubmitOrder;
        cookingUIEventChannel.OnAddProperty += StartAddProperty;
        cookingUIEventChannel.OnSelectOrder += SelectOrder;
        cookingUIEventChannel.OnChangeNextStation += ChangeNextStation;
        cookingUIEventChannel.OnTrashCurrentOrderFood += OnTrashCurrentOrderFood;
        cookingUIEventChannel.OnAssembleOrder += SetOrderAsAssembled;
        //TEMP - I think you need to be in order to determine this, so prolly keep this?
        cookingUIEventChannel.OnAssembleOrder += DetermineWorkspaceAssemble;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnCreateOrder -= AddOrder;
        cookingUIEventChannel.OnSubmitOrder -= SubmitOrder;
        cookingUIEventChannel.OnAddProperty -= StartAddProperty;
        cookingUIEventChannel.OnSelectOrder -= SelectOrder;
        cookingUIEventChannel.OnChangeNextStation -= ChangeNextStation;
        cookingUIEventChannel.OnTrashCurrentOrderFood -= OnTrashCurrentOrderFood;
        cookingUIEventChannel.OnAssembleOrder -= SetOrderAsAssembled;
        //TEMP
        cookingUIEventChannel.OnAssembleOrder -= DetermineWorkspaceAssemble;
    }

    private void SetOrderAsAssembled()
    {
        currentOrder.isAssembled = true;
    }

    // Add Property starts here because it needs to kick off a coroutine
    private void StartAddProperty(ActionData actionData)
    {
        StartCoroutine(ExecuteAddProperty(actionData));
    }

    private IEnumerator ExecuteAddProperty(ActionData actionData)
    {
        //progressBar.StartProgress(actionData.ActionTime);
        yield return new WaitForSeconds(actionData.ActionTime);
        
        // apply property to ingredients in station
        List<Ingredient> ingredients = currentOrder.Station.ApplyProperty(actionData);
        
        // apply property to ingredients in the SelectedIngredients of the current order
        foreach (Ingredient processedIngredient in ingredients)
        {
            foreach (IngredientData orderIngredientData in currentOrder.CurrentIngredients.Keys)
            {
                if (processedIngredient.Data.Name.Equals(orderIngredientData.Name))
                {
                    currentOrder.CurrentIngredients[orderIngredientData].Add(actionData.Property);// Might be unfinished. look later. 
                }
            }
        }
    }

    /// <summary>
    /// Changes the current order to the newly selected order.
    /// </summary>
    /// <param name="orderData"></param>
    private void SelectOrder(Order selectedOrder)
    {
        if (selectedOrder == currentOrder){ // for now, click again to deselect TODO: close X button
            DeselectOrder();
            return;
        }
        currentOrder = selectedOrder;
        currentOrder.Station.Subscribe();
    }

    private void DeselectOrder()
    {
        if (currentOrder != null) currentOrder.Station.Unsubscribe();
        currentOrder = null;
        cookingUIEventChannel.RaiseOnDeselectOrder();
    }
    
    private void OnTrashCurrentOrderFood()
    {
        int index = currentOrder.StationIdx;
        currentOrder.ResetStation();
    }
    
    public void AddOrder(Order order)
    {
        order.cookingUIEventChannel = cookingUIEventChannel; // pass event channel to order
        activeOrders.Add(order);
        cookingUIEventChannel.RaiseOnGenerateOrderButton(order);
    }

    public void SubmitOrder(Order order)
    {
        if (order == null)
        {
            SubmitOrder(currentOrder);
        }
        else
        {
            activeOrders.Remove(order);
            cookingUIEventChannel.RaiseOnRemoveCustomer(order.Customer.Data.CustomerSpotIdx);
            float incorrectness = order.IsCorrect();
            Debug.Log(incorrectness);
            if (incorrectness == 0)
            {
                Debug.Log("Order is correct");
            }
            else
            {
                Debug.Log("Order is incorrect");
            }
            Customer c = order.Customer;
            
            // pay the player
            float incorrectnessPenalty = incorrectness / (incorrectness + 1);
            float timeUsed = (c.Data.Patience - c.RemainingPatience) / c.Data.Patience;
            
            // food must be assembled AND not raw
            bool isAssembled = currentOrder.isAssembled;
            bool noRawFood = currentOrder.CurrentIngredients.Count == 0 || currentOrder.CurrentIngredients.All(kvp => kvp.Value.Count == 0);
            
            float score = (1 - incorrectnessPenalty) * (1 - timeUsed);
            score = noRawFood || !isAssembled ? 0 : score;

            int deltaMoney = (int)Math.Round(currentOrder.Recipe.RecipeValue * score);
            playerManager.ChangeMoney(deltaMoney);
            cookingUIEventChannel.RaiseOnChangePlayerMoney(deltaMoney);
            Debug.Log(deltaMoney);
        }
    }
    
    // Pass event channel trigger to order
    public void ChangeNextStation(){
        currentOrder.ChangeStation();
    }

    public void DetermineWorkspaceAssemble()
    {
        // check if the order is correct here, then send the right sprite
        cookingUIEventChannel.RaiseOnWorkspaceAssemble(currentOrder.Recipe.DoneIcon);
    }
}

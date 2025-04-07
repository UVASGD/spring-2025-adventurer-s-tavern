using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class RoundManager : MonoBehaviour
{

    private int _day;
    private float _moneyAccumulatedThisRound;
    private Enum _grade;
    private int _customersServed;
    
    /// <summary>
    /// A dict of served orders, whose values represent if the order was cooked correctly or incorrectly
    /// </summary>
    private Dictionary<Order, bool> _servedOrders = new Dictionary<Order, bool>();

    [SerializeField] private int customersToPass;
    
    [SerializeField] private CookingUIEventChannel eventChannel;
    
    [SerializeField] public BiomeData currentBiome;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _day = 0;
        _customersServed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: customerstopass will probably increase for each level
        if (_customersServed >= customersToPass)
        {
            FinishDay();
            _customersServed = 0; 
        }
    }

    private void OnEnable()
    {
        eventChannel.OnDayStarted += StartNewDay;
        eventChannel.OnChangePlayerMoney += ChangeMoney;
        eventChannel.OnSubmitOrder += IncrementCustomersServed;
    }

    private void OnDisable()
    {
        eventChannel.OnDayStarted -= StartNewDay;
        eventChannel.OnChangePlayerMoney -= ChangeMoney;
        eventChannel.OnSubmitOrder -= IncrementCustomersServed;
    }

    // Don't need this int input for now, but it's here just in case
    private void StartNewDay(int x)
    {
        _day++;
    }
    
    private void FinishDay()
    {
        eventChannel.RaiseOnDayFinished(_day);
        Debug.Log("Day Finished");
    }

    private void ChangeMoney(float deltaMoney)
    {
        _moneyAccumulatedThisRound += deltaMoney;
    }

    private void CalculateGrade()
    {
        
    }

    private void IncrementCustomersServed(Order order)
    {
        _customersServed++;
    }
}

public enum Grade
{
    S,
    A,
    B,
    C,
    D,
    F,
}
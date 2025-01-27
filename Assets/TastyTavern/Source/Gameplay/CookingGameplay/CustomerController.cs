using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    /// TODO: 
    /// Spawn Customers at a certain rate per timeframe -- Depend on maybe level? (difficulty)
    /// Customer will generate order
    /// Customer will move into game view
    /// On completion, customer
    /// Keep track of customer list --> Slot number (slot 1, 2, 3)
    /// When customer comes in, slide from right
    /// When customer leaves, slides to left off screen
    /// Don't worry about changing appearance rn, just have one sprite
    /// </summary>

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject[] customers = new GameObject[3];

    [SerializeField]
    private CookingUIEventChannel cookingUIEventChannel;

    [SerializeField]
    private BiomeData selectedBiome;

    [SerializeField]
    private int Difficulty;
    private double nextSpawnTime = 0.0;
    public float baseDelay = 0f; // Base delay in seconds for difficulty = 1.
    public float randomOffset = 0f; // Maximum random offset added or subtracted from the delay.;
    public GameObject customerPrefab;

    [SerializeField]
    private List<Transform> CustomerSpots;

    void Start()
    {
        
    }

    private void Update()
    {
        // Check if it's time to create a new customer.
        if (Time.time >= nextSpawnTime)
        {
            CreateCustomer();
            ScheduleNextCustomer();
        }
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnSubmitOrder += RemoveCustomer;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnSubmitOrder -= RemoveCustomer;
    }

    private void ScheduleNextCustomer()
    {
        // Calculate the delay based on difficulty and randomness.
        float adjustedDelay = Mathf.Max(0.1f, baseDelay / Difficulty); // Ensure delay is never below 0.1 seconds.
        float randomDelay = adjustedDelay + Random.Range(-randomOffset, randomOffset);

        // Schedule the next spawn time.
        nextSpawnTime = Time.time + randomDelay;
    }

    public bool CreateCustomer()
    {
        // Inefficiency: For loop will always be running. Technically it's O(1) every frame since the length of the customers list is a constant 3, but still. 
        // Could be optimized to only run when a spot in customers is null, but I can't use customers.Length b/c it is always 3 since I set it that way. 
        for (int i = 0; i < CustomerSpots.Count; i++)
        {
            if (customers[i] == null)
            {
                Debug.Log($"Found empty customer spot in {i}");
                // Create the CustomerData
                CustomerData data = new CustomerData(
                    name: "Customer " + Random.Range(1, 100),
                    appearance: new List<Sprite>(), // Replace with actual sprites
                    faces: new List<Sprite>(), // Replace with actual face sprites
                    dialogue: new List<string> { "Hello!", "Thanks!", "Oh no!" },
                    patience: Random.Range(50, 100),
                    biome: selectedBiome // Replace with the current biome
                );

                // Instantiate prefab and initialize
                GameObject customerObj = Instantiate(customerPrefab, CustomerSpots[i].position, Quaternion.identity);
                Customer customerScript = customerObj.GetComponent<Customer>();
                customerScript.Initialize(data);

                // Track the customer
                customers[i] = customerObj;

                return true;
            }
        }
        
        return false;
    }

    public void RemoveCustomer(Order order)
    {
        Customer c = order.Customer;
        for (int i = 0; i < customers.Length; i++) 
        {
            if (customers[i].Equals(c))
            {
                customers[i] = null;
                Destroy(customers[i]);
            }
        }
    }
}

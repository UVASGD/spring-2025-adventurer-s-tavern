//using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
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

    private BiomeData selectedBiome;

    [SerializeField]
    private int Difficulty;
    private double nextSpawnTime = 0.0;
    public float baseDelay; // Base delay in seconds for difficulty = 1.
    public float randomOffset; // Maximum random offset added or subtracted from the delay.;
    public GameObject customerPrefab;
    public int min_patience = 80; // Minimum patience level for customers.
    public int max_patience = 160; // Maximum patience level for customers.

    //TEMPORARY until customer sprites corrected
    public List<Sprite> customerSprites;
    public int spriteIdx = 0;

    [SerializeField]
    private List<Transform> CustomerSpots;

    [SerializeField]
    private MenuManager MenuManager;

    [SerializeField] 
    private OrderManager OrderManager;
    
    private bool isSpawning = false;


    void Start()
    {
        selectedBiome = OrderManager.playerManager.currentBiome;
    }

    private void Update()
    {
        // Check if it's time to create a new customer.
        for (int i = 0; i < CustomerSpots.Count; i++)
        {
            if (customers[i] == null && !isSpawning)
            {
                StartCoroutine(SpawnCustomerWithDelay(i));
            }
        }
        
    }

    private void OnEnable()
    {
        cookingUIEventChannel.OnRemoveCustomer += RemoveCustomer;
    }

    private void OnDisable()
    {
        cookingUIEventChannel.OnRemoveCustomer -= RemoveCustomer;
    }
    private IEnumerator SpawnCustomerWithDelay(int spotIndex)
    {
        isSpawning = true;
        // Calculate the delay based on difficulty and randomness.
        float adjustedDelay = Mathf.Max(0.1f, baseDelay / Difficulty); // Ensure delay is never below 0.1 seconds.
        float randomDelay = adjustedDelay + Random.Range(-randomOffset, randomOffset);
        
        yield return new WaitForSeconds(adjustedDelay);
        
        // Double-check the spot is still empty before spawning.
        if (customers[spotIndex] == null)
        {
            CreateCustomer();
        }
        
        // Optionally, you might call ScheduleNextCustomer after creating a customer.
        isSpawning = false;
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
                    patience: Random.Range(min_patience, max_patience),
                    biome: selectedBiome, // Replace with the current biome
                    customerSpotIdx: i
                );

                // Instantiate prefab and initialize
                GameObject customerObj = Instantiate(customerPrefab, CustomerSpots[i].position, Quaternion.identity);
                customerObj.GetComponent<SpriteRenderer>().sprite = customerSprites[Random.Range(0, customerSprites.Count)];

                Customer customerScript = customerObj.GetComponent<Customer>();
                customerScript.MenuManager = MenuManager;
                customerScript.OrderManager = OrderManager;
                customerScript.Initialize(data);

                // Track the customer
                customers[i] = customerObj;

                return true;
            }
        }
        
        return false;
    }

    public void RemoveCustomer(int customeridx)
    {
        customers[customeridx].SetActive(false);
        Destroy(customers[customeridx]);
        customers[customeridx] = null;
        cookingUIEventChannel.RaiseOnDeleteOrderButton(customeridx);
    }
}

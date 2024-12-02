using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Reference to the customer prefab
    public Transform spawnPoint; // Point where the customer will be spawned

    private void Start()
    {
        SpawnCustomer(); // Spawn the first customer at the start
    }

    public void SpawnCustomer()
    {
        // Instantiate the customer prefab at the spawn point
        GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

        // Optionally, you can add a script to the customer to handle its destruction
        // For example, you can add a listener to the customer's destruction event
        Customer customerScript = customer.GetComponent<Customer>();
        if (customerScript != null)
        {
            customerScript.OnCustomerDestroyed += HandleCustomerDestroyed;
        }
    }

    private void HandleCustomerDestroyed()
    {
        // Start the coroutine to wait and spawn a new customer
        StartCoroutine(WaitAndSpawnCustomer());
    }

    private IEnumerator WaitAndSpawnCustomer()
    {
        // Wait for a random time between 1 and 2 seconds
        float waitTime = Random.Range(1f, 2f);
        yield return new WaitForSeconds(waitTime);

        // Spawn a new customer
        SpawnCustomer();
    }
}

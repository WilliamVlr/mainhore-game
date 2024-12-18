using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeCustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform customerSpawnPoint;
    public RectTransform customerPanelSpawnPoint;
    public GameObject parentCustomerLayer;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnCustomer();
        StartCoroutine(WaitAndSpawnCustomer(1f, 5f));
    }

    private void SpawnCustomer()
    {
        //Instantiate customer prefab
        GameObject customer = Instantiate(customerPrefab, customerSpawnPoint.position, Quaternion.identity);
        customer.transform.SetParent(parentCustomerLayer.transform);

        Transform customerTransform = customer.transform.GetComponent<Transform>();
        customerTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        Transform customerPanelTr = customer.transform.GetChild(0).transform.GetChild(0);

        RectTransform rectTransformPanel = customerPanelTr.GetComponent<RectTransform>();
        rectTransformPanel.anchoredPosition = customerPanelSpawnPoint.anchoredPosition;

        CustomerCafe customerScript = customer.GetComponent<CustomerCafe>();
        if(customerScript != null )
        {
            customerScript.OnCustomerDestroyed += HandleCustomerDestroyed;
        }
    }

    private void HandleCustomerDestroyed()
    {
        // Start the coroutine to wait and spawn a new customer
        StartCoroutine(WaitAndSpawnCustomer(1f, 3f));
    }

    private IEnumerator WaitAndSpawnCustomer(float upper, float lower)
    {
        // Wait for a random time between lower bound and upper bound
        float waitTime = Random.Range(lower, upper);
        yield return new WaitForSeconds(waitTime);

        // Spawn a new customer
        SpawnCustomer();
    }
}

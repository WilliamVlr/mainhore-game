using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Furniture Behaviors/WeightedDrop")]
public class WeightedDropBehavior_SO : DropBehavior_SO
{
    public override void HandleDrop(GameObject furniture)
    {
        // Enable gravity to make the furniture fall
        Rigidbody2D rb = furniture.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            //Debug.Log($"{furniture.name} is now falling to the ground!");
        }
        else
        {
            Debug.LogWarning("No Rigidbody2D found on furniture!");
        }
    }
}

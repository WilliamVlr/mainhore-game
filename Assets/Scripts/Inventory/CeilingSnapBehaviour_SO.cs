using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Furniture Behaviors/CeilingSnap")]
public class CeilingSnapBehavior_SO : DropBehavior_SO
{
    public override void HandleDrop(GameObject furniture)
    {
        // Snap the furniture to the ceiling
        Vector3 position = furniture.transform.position;
        //position.y = Camera.main.orthographicSize - 3; // Example: Top of the camera view
        position.y = 5 - 3; // Example: Top of the camera view
        furniture.transform.position = position;
        //Debug.Log($"{furniture.name} snapped to the ceiling!");
    }
}

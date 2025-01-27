using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Furniture Behaviors/WallSnap")]
public class WallSnapBehavior_SO : DropBehavior_SO
{
    public override void HandleDrop(GameObject furniture)
    {
        // Snap to the wall if near the floor
        Vector3 position = furniture.transform.position;
        if (position.y < -3) // Example threshold for the floor
        {
            position.y = -3; // Snap to wall height
        }
        furniture.transform.position = position;
        Debug.Log($"{furniture.name} snapped to the wall!");
    }
}

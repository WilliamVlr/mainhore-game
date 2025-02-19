using UnityEngine;
using UnityEngine.UI;  // For UI elements

public class ApproachAndTriggerChat : MonoBehaviour
{
    public Transform objectB;         // Reference to Object B
    public float speed = 2f;          // Movement speed of Object A
    public float triggerDistance = 2f; // Distance at which the chatbox appears
    public GameObject chatboxPanel;   // The chatbox UI panel
    public Text chatText;             // The text component for displaying messages

    private bool chatTriggered = false; // To prevent triggering chat multiple times

    void Start()
    {
        // Ensure the chatbox is hidden at the start
        if (chatboxPanel != null)
            chatboxPanel.SetActive(false);
    }

    void Update()
    {
        // Check distance between Object A and Object B
        float distance = Vector3.Distance(transform.position, objectB.position);

        if (distance > triggerDistance && !chatTriggered)
        {
            // Move Object A toward Object B
            transform.position = Vector3.MoveTowards(
                transform.position,
                objectB.position,
                speed * Time.deltaTime
            );
        }
        else if (!chatTriggered)
        {
            // Trigger chatbox when distance is reached
            TriggerChatbox();
        }
    }

    void TriggerChatbox()
    {
        if (chatboxPanel != null)
        {
            chatboxPanel.SetActive(true);
            chatText.text = "Hey there! You finally reached me. Let's chat!";  // Customize this message
            chatTriggered = true;
        }
    }
}

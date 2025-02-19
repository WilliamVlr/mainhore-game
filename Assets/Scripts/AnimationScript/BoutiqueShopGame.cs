using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoutiqueShopManager : MonoBehaviour
{
    [Header("Player and Cashier Settings")]
    public Transform playerAvatar;
    public Transform cashierWoman;
    public float triggerDistance = 3f;

    [Header("UI Elements")]
    public GameObject chatBubble;
    public GameObject startButton; // Reference to 'shop_btn'

    [Header("Spotlight Settings")]
    public Animator spotlightAnimator;

    [Header("Curtain and Character Settings")]
    public Animator[] curtainAnimators; // Animators for the three curtains
    public GameObject[] costumeCharacters; // The three costume character GameObjects

    [Header("Equip Button")]
    public GameObject equipButton; // Button for equipping the costume

    private int selectedIndex;
    private bool interactionTriggered = false;

    void Update()
    {
        CheckPlayerDistance();
    }

    // Check if player is close enough to trigger chatbox and button
    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(playerAvatar.position, cashierWoman.position);
        if (distance <= triggerDistance && !interactionTriggered)
        {
            TriggerChatAndButton();
            interactionTriggered = true;
        }
    }

    // Display chat bubble and "Start Randomize Costume" button
    void TriggerChatAndButton()
    {
        chatBubble.SetActive(true);
        startButton.SetActive(true);
    }

    // Called by Start Button (shop_btn) when pressed
    public void StartRandomization()
    {
        StartCoroutine(HandleRandomizationProcess());
    }

    // Coroutine to manage the entire randomization and reveal process
    IEnumerator HandleRandomizationProcess()
    {
        startButton.SetActive(false);
        TriggerSpotlightAnimation();
        yield return new WaitForSeconds(2.5f); // Wait for spotlight animation

        RandomizeCostume();
        yield return new WaitForSeconds(1.5f); // Wait for curtain animation

        ShowSelectedCharacter();
    }

    // Trigger the spotlight animation
    void TriggerSpotlightAnimation()
    {
        if (spotlightAnimator != null)
            spotlightAnimator.SetTrigger("StartShow");
    }

    // Randomly select a costume and trigger curtain reveal
    void RandomizeCostume()
    {
        selectedIndex = Random.Range(0, costumeCharacters.Length);
        curtainAnimators[selectedIndex].SetTrigger("Open");
    }

    // Reveal the selected character and show equip button
    void ShowSelectedCharacter()
    {
        costumeCharacters[selectedIndex].SetActive(true);
        equipButton.SetActive(true);
    }

    // Equip the selected costume (linked to Equip Button)
    public void EquipSelectedCostume()
    {
        Debug.Log("Player equipped costume: " + costumeCharacters[selectedIndex].name);
        equipButton.SetActive(false);
        ApplyCostumeToPlayer();
    }

    // Additional method for applying the selected costume to the player
    void ApplyCostumeToPlayer()
    {
        // Placeholder for actual costume application logic
        // Implement character customization logic here
        Debug.Log("Costume applied to player successfully.");
    }

    // Reset the shop state if needed
    public void ResetShop()
    {
        interactionTriggered = false;
        chatBubble.SetActive(false);
        startButton.SetActive(false);
        equipButton.SetActive(false);
        foreach (var character in costumeCharacters)
        {
            character.SetActive(false);
        }
        foreach (var animator in curtainAnimators)
        {
            animator.SetTrigger("Close");
        }
        Debug.Log("Shop reset to initial state.");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject virus;
    GameObject timerObject;
    public GameObject pauseInterface;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    private List<Vector2> spawnedPositions = new List<Vector2>();

    public float minDistance;

    int spawned = 0;
    public int spawnCount;
    public int _virusDestroyed = 0;

    public Text timer;
    //VirusController _virusController;
    
    // Start is called before the first frame update
    void Start()
    {
        if(pauseInterface)
        {
            //Debug.Log("ada");
        }
        else
        {
            //Debug.Log("gada");
        }
    }
    private void Awake()
    {
        timerObject = GameObject.FindWithTag("preRoundTimer");
    }
    void Update()
    {
        Text timerText = timerObject.GetComponent<Text>();
        if(timerText.text == "GO!" && spawned == 0)
        {
            spawnObject();
        }

        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos, 0f); // Adjust radius as needed

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Virus") && timerText.text != "0") // Ensure you only target objects tagged "Virus"
                {
                    Clickable clickable = hitCollider.GetComponent<Clickable>();
                    if (clickable != null && !clickable.isClicked) // Check if it hasn't already been clicked
                    {
                        clickable.isClicked = true; // Mark as clicked

                        VirusAnimator virusAnimator = hitCollider.GetComponent<VirusAnimator>();
                        if (virusAnimator != null && !pauseInterface.activeSelf && timer.text != "00")
                        {
                            virusAnimator.PlayAnimator(); // Play animation
                            _virusDestroyed++;
                            Debug.Log(_virusDestroyed);
                            StartCoroutine(DestroyAfterAnimation(hitCollider.gameObject, virusAnimator)); // Delay destruction
                        }
                    }
                }
            }
        }

        IEnumerator DestroyAfterAnimation(GameObject target, VirusAnimator virusAnimator)
        {
            Animator animator = target.GetComponent<Animator>();

            if (animator != null)
            {
                // We need to wait for the current animation to finish, so let's track when the animation ends
                bool isPlaying = true;

                // Loop to check if the animation is still playing
                while (isPlaying)
                {
                    // Check if the animator is playing any animation
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    isPlaying = stateInfo.normalizedTime < 1f; // Wait until the animation completes (normalizedTime reaches 1)

                    // Optional: You can add a small delay to prevent excessive checks
                    yield return null;
                }
            }

            // Destroy the target after the animation finishes
            //Debug.Log(virusDestroyed);
            Destroy(target);
        }
    }

    

    // Update is called once per frame
    void spawnObject()
    {
        int maxAttempts = 10 * spawnCount;  // Limit attempts to prevent infinite loops
        int attempts = 0;

        // Try spawning objects until we reach the spawn count or exceed max attempts
        while (spawned < spawnCount && attempts < maxAttempts)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            attempts++;

            // If the position is valid (no overlap), spawn the object
            if (IsPositionValid(randomPosition))
            {
                spawnedPositions.Add(randomPosition);
                Instantiate(virus, randomPosition, Quaternion.identity);
                spawned++;
            }
        }

        // Optionally, print a message if spawn attempts exceeded max attempts
        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Max spawn attempts reached!");
        }
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            if (Vector2.Distance(spawnedPosition, position) < minDistance)
            {
                return false; // Position is too close to an existing one
            }
        }
        return true; // Position is valid
    }
}

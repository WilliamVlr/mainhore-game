using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject virus;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject pauseInterface;

    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;

    [SerializeField] private List<Vector2> spawnedPositions = new List<Vector2>();
    [SerializeField] private float minDistance;

    [SerializeField] private int spawnCount;
    private int spawned = 0;
    [SerializeField] private int _virusDestroyed = 0;
    [SerializeField] private float virusSpeed;

    private TextMeshProUGUI timerText;

    public int SpawnCount => spawnCount;
    public int VirusDestroyed => _virusDestroyed;

    void Start()
    {
        Camera cam = Camera.main;
        Vector3 maxPosition = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        Vector3 minPosition = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        spawnAreaMin = new Vector2(minPosition.x + 1.5f, minPosition.y + 1f);
        spawnAreaMax = new Vector2(maxPosition.x - 1.5f, maxPosition.y - 1f);

        timerText = timerObject.GetComponent<TextMeshProUGUI>();
    }

    void Awake()
    {
        
    }

    void Update()
    {
        //Debug.Log(virusSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Virus"))
            {
                //Debug.Log("Clicked");
                HandleVirusClick(hit.collider.gameObject);
            }
        }
    }

    void HandleVirusClick(GameObject virusObject)
    {
        if (timerText.text == "00" || pauseInterface.activeSelf)
        {
            //Debug.Log("1");
            return;
        }

        Clickable clickable = virusObject.GetComponent<Clickable>();
        if (clickable == null || clickable.isClicked)
        {
            //Debug.Log("2");
            return;
        }

        clickable.isClicked = true;
        VirusAnimator virusAnimator = virusObject.GetComponent<VirusAnimator>();
        //Debug.Log("3");

        if (virusAnimator != null)
        {
            //Debug.Log("4");
            virusAnimator.PlayAnimator();
            _virusDestroyed++;
            StartCoroutine(DestroyAfterAnimation(virusObject));
        }
    }

    IEnumerator DestroyAfterAnimation(GameObject target)
    {
        Animator animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }
        Destroy(target);
    }

    public void SpawnObjects()
    {
        int attempts = 0;
        float dynamicMinDistance = minDistance;

        while (spawned < spawnCount && attempts < spawnCount * 10)
        {
            Vector2 randomPosition = GetRandomPosition();
            attempts++;

            if (IsPositionValid(randomPosition, dynamicMinDistance))
            {
                spawnedPositions.Add(randomPosition);
                GameObject virusNew = Instantiate(virus, randomPosition, Quaternion.identity);
                VirusMove virusMove = virusNew.GetComponent<VirusMove>();
                virusMove.speed = virusSpeed;
                spawned++;
                attempts = 0;
            }

            if (attempts >= spawnCount * 5)
            {
                dynamicMinDistance *= 0.9f;
            }
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
    }

    bool IsPositionValid(Vector2 position, float minDist)
    {
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            if (Vector2.Distance(spawnedPosition, position) < minDist)
            {
                return false;
            }
        }
        return true;
    }

    public void SetLevel1()
    {
        spawnCount = 15;
        virusSpeed = 5f;
    }
    public void SetLevel2()
    {
        spawnCount = 20;
        virusSpeed = 8f;
    }
    public void SetLevel3()
    {
        spawnCount = 25;
        virusSpeed = 10f;
    }
}

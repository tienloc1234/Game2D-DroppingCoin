using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapAmbushPoint : MonoBehaviour
{
    [Header("Trap")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Timing")]
    [SerializeField] private float activationDelay = 0.15f;
    [SerializeField] private float riseDuration = 0.3f;
    [SerializeField] private float dangerDelay = 0.05f;

    [Header("Animation")]
    [SerializeField] private float riseDistance = 0.7f;

    private bool hasTriggered;
    private BoxCollider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        hasTriggered = true;

        // Không cho vùng này kích hoạt lần thứ hai
        triggerCollider.enabled = false;

        StartCoroutine(SpawnTrapRoutine());
    }

    private IEnumerator SpawnTrapRoutine()
    {
        if (activationDelay > 0f)
        {
            yield return new WaitForSeconds(activationDelay);
        }

        if (trapPrefab == null)
        {
            Debug.LogWarning(
                $"Trap Prefab chưa được gán tại {gameObject.name}.",
                this
            );

            yield break;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning(
                $"Spawn Point chưa được gán tại {gameObject.name}.",
                this
            );

            yield break;
        }

        Vector3 finalPosition = spawnPoint.position;
        Vector3 hiddenPosition =
            finalPosition - Vector3.up * riseDistance;

        GameObject spawnedTrap = Instantiate(
            trapPrefab,
            hiddenPosition,
            spawnPoint.rotation
        );

        // Tắt collider trong lúc trap đang trồi lên
        Collider2D[] trapColliders =
            spawnedTrap.GetComponentsInChildren<Collider2D>(true);

        foreach (Collider2D trapCollider in trapColliders)
        {
            trapCollider.enabled = false;
        }

        float elapsedTime = 0f;

        while (elapsedTime < riseDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / riseDuration
            );

            // Làm chuyển động trồi lên mượt hơn
            float smoothProgress = Mathf.SmoothStep(
                0f,
                1f,
                progress
            );

            spawnedTrap.transform.position = Vector3.Lerp(
                hiddenPosition,
                finalPosition,
                smoothProgress
            );

            yield return null;
        }

        spawnedTrap.transform.position = finalPosition;

        // Cho Player một khoảng thời gian rất ngắn để nhìn thấy trap
        if (dangerDelay > 0f)
        {
            yield return new WaitForSeconds(dangerDelay);
        }

        foreach (Collider2D trapCollider in trapColliders)
        {
            trapCollider.enabled = true;
        }

        // Xóa vùng kích hoạt sau khi đã sinh trap
        Destroy(gameObject);
    }
}
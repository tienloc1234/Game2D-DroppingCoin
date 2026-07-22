using System.Collections;
using UnityEngine;

public class FakeCoin : MonoBehaviour
{
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private float teleportCooldown = 0.5f;


    private Collider2D coinCollider;

    private bool isTeleporting;

    void Awake()
    {

        coinCollider = GetComponent<Collider2D>();


    }

    public void TriggerTrap()
    {
        if (isTeleporting || teleportPoints == null || teleportPoints.Length == 0)
            return;

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        isTeleporting = true;

        if (coinCollider != null)
            coinCollider.enabled = false;
        Transform destination = GetRandomDestination();
        if (destination != null)
            transform.position = destination.position;

        yield return new WaitForSeconds(teleportCooldown);

        if (coinCollider != null)
            coinCollider.enabled = true;
        isTeleporting = false;
    }
    private Transform GetRandomDestination()
    {
        return teleportPoints[Random.Range(0, teleportPoints.Length)];
    }   
}

using System.Collections;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject potionPrefab;
    [Range(0f, 1f)] public float potionChance = 0.3f;
    public Sprite usedSprite;
    public float popHeight = 0.6f;
    public float popDuration = 0.25f;

    private bool used = false;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (used || !collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                Activate();
                break;
            }
        }
    }

    void Activate()
    {
        used = true;
        if (usedSprite != null) sr.sprite = usedSprite;

        GameObject prefabToSpawn = (Random.value < potionChance && potionPrefab != null)
            ? potionPrefab
            : coinPrefab;

        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject item = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        StartCoroutine(PopUp(item));
    }

    IEnumerator PopUp(GameObject item)
    {
        Vector3 start = item.transform.position;
        Vector3 peak = start + Vector3.up * popHeight;
        float t = 0f;

        while (t < popDuration)
        {
            //if (item == null) yield break;

            t += Time.deltaTime;
            item.transform.position = Vector3.Lerp(start, peak, t / popDuration);
            yield return null;

        //Rigidbody2D coinRb = item.GetComponent<Rigidbody2D>();
        //if (coinRb != null)
        //{
        //    coinRb.bodyType = RigidbodyType2D.Dynamic;
        //    coinRb.gravityScale = 0.5f;
        }
    }
}
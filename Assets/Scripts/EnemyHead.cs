using UnityEngine;

public class EnemyHead : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().Die();
        }
        }
    }

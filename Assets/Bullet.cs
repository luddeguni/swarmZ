using UnityEngine;

public class EnemyBullet01 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
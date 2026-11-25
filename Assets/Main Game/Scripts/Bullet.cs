using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    void Start()
    {        
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //prevent bullet being deleted when colliding with player and room dectector hitbox
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Room Detector"))
        {
            return;
        }
        //if colliding with an enemy
        else if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

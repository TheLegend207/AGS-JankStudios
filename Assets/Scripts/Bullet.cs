using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //destroy bullet after 1 second(to be adjusted) to prevent unwanted/glitched bullets from staying in game
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //if colliding with an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            //when enemy is implemented change "destroy enemy" to enemy takes dmg
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        //prevent bullet being deleted when colliding with player
        //(aka when spawning, so we dont have to force the shoot point forwards to avoid bullet deletion)
        else if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        else
        {
            //destroys projectile when hitting anything else
            Destroy(gameObject);
        }
    }
}

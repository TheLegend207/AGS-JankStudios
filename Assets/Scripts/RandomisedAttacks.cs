using System.Collections;
using UnityEngine;

public class RandomisedAttacks : MonoBehaviour
{
    //public GameObject projectilePrefab;
    //public Transform shootPoint;
    //public float projectileSpeed;

    public bool readyToDrawCard;
    public bool readyToShoot;
    public bool cardShot;
    public int generatedCardNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        cardShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToDrawCard == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("Card Drawn");
                DrawCard();
                readyToShoot = true;
                readyToDrawCard = false;
                StartCoroutine(CardHandler());
            }
        }
        else if (readyToShoot == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cardShot = true;
                Shoot();
                print("Card Shot");
                readyToDrawCard = true;
                readyToShoot = false;
            }
        }
        else
            print("error, draw and shoot are both unready");
    }
    void DrawCard()
    {
        generatedCardNumber = Random.Range(1 , 4);
        print("Card number is " + generatedCardNumber);
    }
    IEnumerator CardHandler()
    {
        yield return new WaitForSeconds(2f);
        if (cardShot == false)
        {
            print("Card discarded");
            readyToDrawCard = true;
            readyToShoot = false;
        }
    }
    void Shoot()
    {
        //GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        //Rigidbody rb = projectile.GetComponent<Rigidbody>();
        //rb.linearVelocity = shootPoint.forward * projectileSpeed;
    }
}

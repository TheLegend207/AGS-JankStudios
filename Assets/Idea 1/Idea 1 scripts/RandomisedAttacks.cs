using System.Collections;
using UnityEngine;

public class RandomisedAttacks : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed;
    public PlayerController playerController;

    [Header("Card Handling")]
    public bool readyToDrawCard;
    public bool readyToShoot;
    public int generatedCardNumber;
    public GameObject card1Indicator;
    public GameObject card2Indicator;
    public GameObject card3indicator;
    public GameObject Indicator1;
    public GameObject Indicator2;
    public GameObject Indicator3;
    public float timeElapsed;
    public float timeBeforeDiscard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        card1Indicator.SetActive(false);
        card2Indicator.SetActive(false);
        card3indicator.SetActive(false);
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToDrawCard == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("Drawn Card");
                DrawCard();
                readyToShoot = true;
                readyToDrawCard = false;
                StartCoroutine(CardHandler());
            }
        }
        else if (readyToShoot == true && generatedCardNumber == 1 || readyToShoot == true && generatedCardNumber == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                print("Card Shot");
                ResetHand();
            }
        }
        else if (readyToShoot == true && generatedCardNumber == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Heal();
                print("Healed");
                ResetHand();
            }
        }
        else
            print("error, draw and shoot are both unready");
    }
    void DrawCard()
    {
        generatedCardNumber = Random.Range(1 , 4);
        print("Card number is " + generatedCardNumber);
        switch (generatedCardNumber)
        {
            case 1:
                card1Indicator.SetActive(true);
                Indicator1.SetActive(true);
                break;
            case 2:
                card2Indicator.SetActive(true);
                Indicator2.SetActive(true);
                break;
            case 3:
                card3indicator.SetActive(true);
                Indicator3.SetActive(true);
                break;
        }
    }
    IEnumerator CardHandler()
    {
        print("Discard Timer Started");
        timeElapsed = 0f;
        while (timeElapsed < timeBeforeDiscard)
        {
            if (readyToDrawCard == true)
            {
                print("Discard Timer Disabled");
                yield break;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        print("Card Discarded");
        ResetHand();
    }
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = shootPoint.forward * projectileSpeed;
    }
    void ResetHand()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        card1Indicator.SetActive(false);
        card2Indicator.SetActive(false);
        card3indicator.SetActive(false);
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
    }
    void Heal()
    {
        playerController.TakeDamage(-10);
    }
}

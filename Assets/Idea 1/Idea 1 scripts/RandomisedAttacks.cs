using System.Collections;
using UnityEditor.Experimental.GraphView;
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
    public GameObject cardRedIndicator;
    public GameObject cardGreenIndicator;
    public GameObject cardYellowindicator;
    public GameObject Indicator1;
    public GameObject Indicator2;
    public GameObject Indicator3;
    public float timeElapsed;
    public float timeBeforeDiscard;
    public Material cardRed;
    public Material cardGreen;
    public Material cardYellow;
    public Material cardEmpty;
    public ArmSleeveHandler armSleeveHandler;
    public int numberKeyPressed;
    public bool cardIsEmpty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        cardRedIndicator.SetActive(false);
        cardGreenIndicator.SetActive(false);
        cardYellowindicator.SetActive(false);
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
                generatedCardNumber = Random.Range(1, 4);
                //print("Card number is " + generatedCardNumber);
                //print("Drawn Card");
                DrawCard();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                numberKeyPressed = 1;
                drawFromArm();
                DrawCard();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                numberKeyPressed = 2;
                drawFromArm();
                DrawCard();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                numberKeyPressed = 3;
                drawFromArm();
                DrawCard();
            }
        }
        else if (readyToShoot == true && generatedCardNumber == 1 || readyToShoot == true && generatedCardNumber == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                print("Card Used");
                ResetHand();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                updateCard1();
                print("Replaced Card 1");
                ResetHand();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                updateCard2();
                print("Replaced Card 2");
                ResetHand();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                updateCard3();
                print("Replaced Card 3");
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
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                updateCard1();
                print("Replaced Card 1");
                ResetHand();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                updateCard2();
                print("Replaced Card 2");
                ResetHand();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                updateCard3();
                print("Replaced Card 3");
                ResetHand();
            }
        }
        else
            print("error, draw and shoot are both unready");
    }
    void DrawCard()
    {
        print(generatedCardNumber);
        switch (generatedCardNumber)
        {
            case 1:
                cardRedIndicator.SetActive(true);
                Indicator1.SetActive(true);
                break;
            case 2:
                cardGreenIndicator.SetActive(true);
                Indicator2.SetActive(true);
                break;
            case 3:
                cardYellowindicator.SetActive(true);
                Indicator3.SetActive(true);
                break;
        }
        readyToShoot = true;
        readyToDrawCard = false;
        StartCoroutine(CardHandler());
    }
    IEnumerator CardHandler()
    {
        //print("Discard Timer Started");
        timeElapsed = 0f;
        while (timeElapsed < timeBeforeDiscard)
        {
            if (readyToDrawCard == true)
            {
                //print("Discard Timer Disabled");
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
        cardRedIndicator.SetActive(false);
        cardGreenIndicator.SetActive(false);
        cardYellowindicator.SetActive(false);
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
    }
    void Heal()
    {
        playerController.TakeDamage(-10);
    }

    void updateCard1()
    {
        switch (generatedCardNumber)
        {
            case 1:
                armSleeveHandler.cardHolder1 = cardRed;
                break;
            case 2:
                armSleeveHandler.cardHolder1 = cardGreen;
                break;
            case 3:
                armSleeveHandler.cardHolder1 = cardYellow;
                break;
        }
        armSleeveHandler.updateSleeveCards();
    }
    void updateCard2()
    {
        switch (generatedCardNumber)
        {
            case 1:
                armSleeveHandler.cardHolder2 = cardRed;
                break;
            case 2:
                armSleeveHandler.cardHolder2 = cardGreen;
                break;
            case 3:
                armSleeveHandler.cardHolder2 = cardYellow;
                break;
        }
        armSleeveHandler.updateSleeveCards();
    }
    void updateCard3()
    {
        switch (generatedCardNumber)
        {
            case 1:
                armSleeveHandler.cardHolder3 = cardRed;
                break;
            case 2:
                armSleeveHandler.cardHolder3 = cardGreen;
                break;
            case 3:
                armSleeveHandler.cardHolder3 = cardYellow;
                break;
        }
        armSleeveHandler.updateSleeveCards();
    }
    void drawFromArm()
    {
        if (numberKeyPressed < 1 || numberKeyPressed > 3) 
            return;
        switch (numberKeyPressed)
        {
            case 1:
                    if (armSleeveHandler.cardHolder1 == cardRed)
                    {
                        generatedCardNumber = 1;
                        armSleeveHandler.cardHolder1 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder1 == cardGreen)
                    {
                        generatedCardNumber = 2;
                        armSleeveHandler.cardHolder1 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder1 == cardYellow)
                    {
                        generatedCardNumber = 3;
                        armSleeveHandler.cardHolder1 = cardEmpty;
                        break;
                    }
                    else
                        break;
            case 2:
                    if (armSleeveHandler.cardHolder2 == cardRed)
                    {
                        generatedCardNumber = 1;
                        armSleeveHandler.cardHolder2 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder2 == cardGreen)
                    {
                        generatedCardNumber = 2;
                        armSleeveHandler.cardHolder2 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder2 == cardYellow)
                    {
                        generatedCardNumber = 3;
                        armSleeveHandler.cardHolder2 = cardEmpty;
                        break;
                    }
                    else
                        break;
            case 3:
                    if (armSleeveHandler.cardHolder3 == cardRed)
                    {
                        generatedCardNumber = 1;
                        armSleeveHandler.cardHolder3 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder3 == cardGreen)
                    {
                        generatedCardNumber = 2;
                        armSleeveHandler.cardHolder3 = cardEmpty;
                        break;
                    }
                    else if (armSleeveHandler.cardHolder3 == cardYellow)
                    {
                        generatedCardNumber = 3;
                        armSleeveHandler.cardHolder3 = cardEmpty;
                        break;
                    }
                    else
                        break;
        }
        armSleeveHandler.updateSleeveCards();
    }
}

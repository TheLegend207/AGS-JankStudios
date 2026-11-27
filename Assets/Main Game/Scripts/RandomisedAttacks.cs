using System.Collections;
using TMPro;
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
    public GameObject Indicator1;
    public GameObject Indicator2;
    public GameObject Indicator3;
    public GameObject Indicator4;
    public GameObject Indicator5;
    public GameObject Indicator6;
    public Material cardRed;
    public Material cardGreen;
    public Material cardYellow;
    public Material cardEmpty;
    public ArmSleeveHandler armSleeveHandler;
    public int numberKeyPressed;
    public bool cardIsEmpty;
    public string[] cards = { "1", "2", "3", "4", "5", "6"};
    public int drawRandomCard;
    public string selectedCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Mana system")]
    public int maxMana = 100;
    public int currentMana;
    public float manaRegen;
    public float manaRegenTimer;
    public TMP_Text manaText;
    void Start()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
        Indicator4.SetActive(false);
        Indicator5.SetActive(false);
        Indicator6.SetActive(false);
        currentMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        manaRegenTimer += Time.deltaTime;
        if(manaRegenTimer > manaRegen)
        {
            manaRegenTimer -= manaRegen;
            currentMana += 1;
            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }
            UpdateManaUI();
        }
        if (readyToDrawCard == true && currentMana >10)
        {
            if (Input.GetMouseButtonDown(0))
            {
                drawRandomCard = Random.Range(0, cards.Length);
                string selectedCard = cards[drawRandomCard];
                Debug.Log(selectedCard);
                switch(selectedCard)
                {
                    case "1":
                        cardtest();
                        Indicator1.SetActive(true);
                        break;
                    case "2":
                        cardtest();
                        Indicator2.SetActive(true); 
                        break;
                    case "3":
                        cardtest();
                        Indicator3.SetActive(true);
                        break;
                    case "4":
                        cardtest();
                        Indicator4.SetActive(true);
                        break;
                    case "5":
                        cardtest();
                        Indicator5.SetActive(true);
                        break;
                    case "6":
                        cardtest();
                        Indicator6.SetActive(true);
                        break;
                }

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
                Indicator1.SetActive(true);
                break;
            case 2:
                Indicator2.SetActive(true);
                break;
            case 3:
                Indicator3.SetActive(true);
                break;
        }
        readyToShoot = true;
        readyToDrawCard = false;
    }
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = shootPoint.forward * projectileSpeed;
        currentMana -= 10;
        UpdateManaUI();
    }
    void ResetHand()
    {
        readyToDrawCard = true;
        readyToShoot = false;
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
    }
    void Heal()
    {
        playerController.TakeDamage(-10);
        currentMana -= 10;
        UpdateManaUI();
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
    private void UpdateManaUI()
    {
        if (manaText != null)
        {
            manaText.text = "" + currentMana;
        }
    }
    void cardtest()
    {
        Indicator1.SetActive(false);
        Indicator2.SetActive(false);
        Indicator3.SetActive(false);
        Indicator4.SetActive(false);
        Indicator5.SetActive(false);
        Indicator6.SetActive(false);
    }
}

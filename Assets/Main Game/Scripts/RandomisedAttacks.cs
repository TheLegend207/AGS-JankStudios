using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    public bool readyToUseCard;
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
        readyToUseCard = false;
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
        //Mana regen system using delta time (float) converted into mana (int)
        //manaRegenTimer = time elapsed, manaRegen = time to regen 1 mana
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= manaRegen)
        {
            manaRegenTimer = 0f;
            currentMana += 1;
            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }
            UpdateManaUI();
        }
        //Randomised card drawing system using array
        //  [MAY HAVE TO COMBINE 2 FUNCTIONS MENTIONED ABOVE]
        //  [TO CHANGE "1-6" TO CARD NAMES AFTER TESTING]
        //If player can draw a card (card handler) and has sufficient mana (mana system)
        if (readyToDrawCard == true && currentMana > 10)
        {
            if (Input.GetMouseButtonDown(0))
            {
                readyToDrawCard = false;
                readyToUseCard = true;
                currentMana -= 10;
                drawRandomCard = Random.Range(0, cards.Length);
                selectedCard = cards[drawRandomCard];
                Debug.Log(selectedCard); //can remove after
                UpdateManaUI();
                switch (selectedCard)
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
            //Drawing cards from armsleeve system
            //  [TEMPORARILY DISABLED FOR REWRITING]
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                numberKeyPressed = 1;
                drawFromArm();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                numberKeyPressed = 2;
                drawFromArm();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                numberKeyPressed = 3;
                drawFromArm();
            }
        }
        //Card use system using switch case
        else if (readyToUseCard == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cardtest();
                print("clicked");
                //Different effect based on which card is drawn (selectedCard)
                //  [TO CHANGE SHOOT FUNCTION TO INDIVIDUAL CARD EFFECTS]
                switch (selectedCard)
                {
                    case "1":
                        print("clicked1");
                        Shoot();
                        break;
                    case "2":
                        print("clicked2");
                        Shoot();
                        break;
                    case "3":
                        print("clicked3");
                        Shoot();
                        break;
                    case "4":
                        print("clicked4");
                        Shoot();
                        break;
                    case "5":
                        print("clicked5");
                        Shoot();
                        break;
                    case "6":
                        print("clicked6");
                        Shoot();
                        break;
                }
            }

            //Store cards in armsleeve system
            //  [TEMPORARILY DISABLED FOR REWRITING]
            else if (readyToUseCard == true && generatedCardNumber == 1 || readyToUseCard == true && generatedCardNumber == 3)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
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
            else if (readyToUseCard == true && generatedCardNumber == 2)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
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
        }
        //Basic shoot function
        //  [TO BE REPALCED WITH INDIVIDUAL CARD EFFECT FUNCTIONS]
        void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = shootPoint.forward * projectileSpeed;
            readyToDrawCard = true;
            readyToUseCard = false;
        }
        //Basic heal function
        // [TEMPORARILY DISABLED FOR REWRITING]
        void Heal()
        {
            playerController.TakeDamage(-10);
            UpdateManaUI();
        }
        //  [TEMPORARILY DISABLED FOR REWRITING]
        void ResetHand()
        {
            readyToDrawCard = true;
            readyToUseCard = false;
            Indicator1.SetActive(false);
            Indicator2.SetActive(false);
            Indicator3.SetActive(false);
        }
        //  [TEMPORARILY DISABLED FOR REWRITING]
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
        //  [TEMPORARILY DISABLED FOR REWRITING]
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
        //  [TEMPORARILY DISABLED FOR REWRITING]
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
        //  [TEMPORARILY DISABLED FOR REWRITING]
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
        }
    }
    //Update visual mana indicator on screen
    private void UpdateManaUI()
    {
        if (manaText != null)
        {
            manaText.text = "" + currentMana;
        }
    }
    // [TEST FUNCTION]
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

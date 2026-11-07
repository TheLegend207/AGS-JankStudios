using System;
using UnityEngine;
using UnityEngine.Animations;

public class ArmSleeveHandler : MonoBehaviour
{
    [Header("Materials")]
    public Material leather;
    public Material metal;
    public Material cardHolder3;
    public Material cardHolder2;
    public Material cardHolder1;

    private SkinnedMeshRenderer meshRenderer;

    public RandomisedAttacks randomisedAttacks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        cardHolder1 = randomisedAttacks.cardRed;
        cardHolder2 = randomisedAttacks.cardGreen;
        cardHolder3 = randomisedAttacks.cardYellow;

        Material[] sleeveMaterials = new Material[5];

        sleeveMaterials[0] = leather;
        sleeveMaterials[1] = metal;
        sleeveMaterials[2] = cardHolder3;
        sleeveMaterials[3] = cardHolder2;
        sleeveMaterials[4] = cardHolder1;

        meshRenderer.materials = sleeveMaterials;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void updateSleeveCards()
    {
        Material[] sleeveMaterials = new Material[5];

        sleeveMaterials[0] = leather;
        sleeveMaterials[1] = metal;
        sleeveMaterials[2] = cardHolder3;
        sleeveMaterials[3] = cardHolder2;
        sleeveMaterials[4] = cardHolder1;

        meshRenderer.materials = sleeveMaterials;
    }
}

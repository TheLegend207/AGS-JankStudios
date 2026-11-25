using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private Animator anim;
    public Rigidbody rb;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        anim.SetFloat("speed", 10f);


    }
}

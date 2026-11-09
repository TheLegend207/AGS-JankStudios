using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = GetComponent<Rigidbody>().linearVelocity.magnitude;
        anim.SetFloat("speed", currentSpeed);


        if (Input.GetKeyDown(KeyCode.Space)) // Temporary manual test button
        {
            anim.SetTrigger("skele attack");
        }
    }
}

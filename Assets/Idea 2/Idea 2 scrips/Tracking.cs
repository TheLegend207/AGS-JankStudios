using UnityEngine;

public class TrackByTag : MonoBehaviour
{
    public string targetTag = "Player";
    public float rotationSpeed = 5.0f;
    public bool smoothRotate = true;

    void Update()
    {

        GameObject target = GameObject.FindGameObjectWithTag(targetTag);

        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            if (smoothRotate)
            {

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.LookAt(target.transform);
            }
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class TowerMerging : MonoBehaviour
{
    public GameObject mergedTower;
    private bool merged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        merged = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        print("objects collided");
        //compares if the other object has the same tag
        if (other.gameObject.CompareTag(gameObject.tag))
        {
            print("objects are same tag");
            // gets the script for the other merging object, variable is named otherObject
            TowerMerging otherObject = other.gameObject.GetComponent<TowerMerging>();
            //checking if the merged bools are false
            if (otherObject != null && !merged && !otherObject.merged)
            {
                print("merge is possible");
                //set merged bools for both objects to true
                merged = true;
                otherObject.merged = true;
                //calculate transform for position of spawning merged tower
                //spawn position is center of both objects transforms
                Vector3 spawnPosition =
                    (transform.position + other.transform.position) / 2f;
                //spawn merged object
                Instantiate(mergedTower,spawnPosition, Quaternion.identity);
                print("merged tower spawned");
                //remove the 2 objects that merged
                Destroy(gameObject);
                Destroy(other.gameObject);

            }
        }
    }
}

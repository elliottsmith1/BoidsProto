using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour {

    public GameObject[] boids;

    // Use this for initialization
    void Start () {
                
        boids = GameObject.FindGameObjectsWithTag("Boid");

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 v1;
        Vector3 v2;
        Vector3 v3;

        boids = GameObject.FindGameObjectsWithTag("Boid");

        for (int i = 0; i < boids.Length; i++)
        {
            Vector3 velocity = new Vector3(0, 0, 0);
            //boids[i].transform.Translate(Vector3.forward * Time.deltaTime * speed);

            v1 = ruleOne(boids[i]);
            v2 = ruleTwo(boids[i]);
            v3 = ruleThree(boids[i]);

            boids[i].transform.Translate(velocity + v1 + v2 + v3);
            boids[i].transform.position = boids[i].transform.position + velocity;

        }

    }

    Vector3 ruleOne(GameObject b)
    {
        Vector3 pcJ = new Vector3(0, 0, 0);

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                pcJ = pcJ + boids[i].transform.position;
            }
        }

        pcJ = pcJ / (boids.Length - 1);

        return (pcJ - b.transform.position) / 100;

    }

    Vector3 ruleTwo(GameObject b)
    {
        Vector3 c = new Vector3(0, 0, 0);
        float minDistance = 1.0f;

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                if (((boids[i].transform.position.x - b.transform.position.x) < minDistance) && ((boids[i].transform.position.y - b.transform.position.y) < minDistance) && ((boids[i].transform.position.z - b.transform.position.z) < minDistance))
                {
                    c = c - (boids[i].transform.position - b.transform.position);
                }
            }
        }

        return c;
    }

    Vector3 ruleThree(GameObject b)
    {
        Vector3 pvJ = new Vector3(0, 0, 0);
        float speeds = 0.0f;

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                speeds = boids[i].GetComponent<BoidScript>().speed;

                pvJ = (pvJ + new Vector3(speeds, speeds, speeds));
            }
        }

        pvJ = (pvJ / (boids.Length - 1));

        speeds = b.GetComponent<BoidScript>().speed;

        return (pvJ - new Vector3(speeds, speeds, speeds)) / 8;
    }
}

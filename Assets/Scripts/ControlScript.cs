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
        Vector3 v4;

        boids = GameObject.FindGameObjectsWithTag("Boid");

        for (int i = 0; i < boids.Length; i++)
        {
            Vector3 velocity = new Vector3(0, 0, 0);

            v1 = ruleOne(boids[i]);
            v2 = ruleTwo(boids[i]);
            v3 = ruleThree(boids[i]);
            v4 = boundingBox(boids[i]);

            boids[i].transform.Translate(velocity + v1 + v2 + v3 + v4);
            boids[i].transform.position = boids[i].transform.position + velocity;

        }

    }

    Vector3 ruleOne(GameObject b)
    {
        Vector3 pcJ = new Vector3(0, 0, 0);
        int counter = 0;

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                if (checkDistance(b, boids[i]))
                {
                    pcJ = pcJ + boids[i].transform.position;
                    counter++;
                }
            }
        }

        if (counter != 0)
        {
            pcJ = pcJ / (counter);

            return (pcJ - b.transform.position) / 100;
        }

        else
        {
            return new Vector3(0, 0, 0);
        }

    }

    Vector3 ruleTwo(GameObject b)
    {
        Vector3 c = new Vector3(0, 0, 0);
        float minDistance = 0.7f;

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                if (checkDistance(b, boids[i]))
                {
                    if ((boids[i].transform.position.x - b.transform.position.x) < minDistance) 
                    {
                        c.x = c.x - (boids[i].transform.position.x - b.transform.position.x) / 100;
                    }

                    if ((boids[i].transform.position.z - b.transform.position.z) < minDistance)
                    {
                        c.z = c.z - (boids[i].transform.position.z - b.transform.position.z) / 100;
                    }
                }
            }
        }

        return c;
    }

    Vector3 ruleThree(GameObject b)
    {
        Vector3 pvJ = new Vector3(0, 0, 0);
        float speeds = 0.0f;
        int counter = 0;

        for (int i = 0; i < boids.Length; i++)
        {
            if (boids[i] != b)
            {
                if (checkDistance(b, boids[i]))
                {
                    speeds = boids[i].GetComponent<BoidScript>().speed;

                    pvJ = (pvJ + new Vector3(speeds, speeds, speeds));
                    counter++;
                }
            }
        }

        if (counter != 0)
        {
            pvJ = (pvJ / (boids.Length - 1));

            speeds = b.GetComponent<BoidScript>().speed;

            return (pvJ - new Vector3(speeds, speeds, speeds)) / 8;
        }

        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    Vector3 boundingBox(GameObject b)
    {
        int Xmin = 1, Xmax = 11, Ymin = -1, Ymax = 1, Zmin = 1, Zmax = 11;
        Vector3 v = new Vector3(0, 0, 0);

        if (b.transform.position.x < Xmin)
        {
            v.x = 0.5f;
        }

        else if (b.transform.position.x > Xmax)
        { 
            v.x = -0.5f;

        }
        if (b.transform.position.y < Ymin)
        {
            v.y = 0.1f;
        }

        else if (b.transform.position.y > Ymax)
        {
            v.y = -0.1f;
        }

        if (b.transform.position.z < Zmin)
        {
            v.z = 0.5f;
        }

        else if (b.transform.position.z > Zmax)
        {
            v.z = -0.5f;
        }

        return v;
    }

    bool checkDistance(GameObject b, GameObject c)
    {
        int minDistance = 3;

        // if (((b.transform.position.x - c.transform.position.x) < minDistance) || ((b.transform.position.z - c.transform.position.z) < minDistance))       
        if (Vector3.Distance(b.transform.position, c.transform.position) < minDistance)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}

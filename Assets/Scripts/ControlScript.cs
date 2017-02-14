using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {

    public List<BoidScript> boids;
    public GameObject redBoid;
    public GameObject blueBoid;

    // Use this for initialization
    void Start () {

    }

    public void addBoid(BoidScript b)
    {
        boids.Add(b);
    }

    // Update is called once per frame
    void Update() {

        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].GetComponent<BoidScript>().runBoid(boids);
        }

        if (Input.GetKeyDown(KeyCode.B))
        { 
            Instantiate(blueBoid, new Vector3(12, 0, 12), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(redBoid, new Vector3(12, 0, 12), Quaternion.identity);
        }
    }
}

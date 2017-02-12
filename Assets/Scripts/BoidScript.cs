using UnityEngine;
using System.Collections;

public class BoidScript : MonoBehaviour {

    public float speed = 1.0f; 

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
       // transform.position = transform.position * speed;
    }
}

using UnityEngine;
using System.Collections;

public class BoidScript : MonoBehaviour {

    GameObject controller;
    ControlScript cntrl;

    public float speed = 1.0f; 

	// Use this for initialization
	void Start () {

        //controller = GameObject.Find("BoidController");
       // cntrl = controller.GetComponent<ControlScript>();


    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}

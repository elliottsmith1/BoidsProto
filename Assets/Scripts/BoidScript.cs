using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidScript : MonoBehaviour {

    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    float r;
    float maxforce;    // Maximum steering force
    float maxspeed;    // Maximum speed

    ControlScript controlScript;

    // Use this for initialization
    void Start () {

        GameObject.FindGameObjectWithTag("Control").GetComponent<ControlScript>().addBoid(this);

        acceleration = new Vector3(0, 0);

        // Leaving the code temporarily this way so that this example runs in JS
        float angle = Random.Range(0.1f, 359.0f);
        velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        position = transform.position;
        r = 2.0f;
        maxspeed = 0.1f;
        maxforce = 0.03f;
    }

    public void runBoid(List<BoidScript> boids)
    {
        flock(boids);
        updateBoid();
    }

    void applyForce(Vector3 force)
    {
        // We could add mass here if we want A = F / M
        acceleration += force;
    }

    // We accumulate a new acceleration each time based on three rules
    void flock(List<BoidScript> boids)
    {
        Vector3 sep = separate(boids);   // Separation
        Vector3 ali = align(boids);      // Alignment
        Vector3 coh = cohesion(boids);   // Cohesion
        Vector3 boun = new Vector3(0, 0, 0);

        for (int i = 0; i < boids.Count; i++)
        {
            boun = boundingBox(boids[i]);
        }
        
        // Arbitrarily weight these forces
        sep *= 1.5f;
        ali *= 1.0f;
        coh *= 1.0f;
        boun *= 1.0f;
        // Add the force vectors to acceleration
        applyForce(boun);
        applyForce(sep);
        applyForce(ali);
        applyForce(coh);
    }

    // Method to update position
    void updateBoid()
    {
        // Update velocity
        velocity += acceleration;
        // Limit speed
        velocity = Vector3.ClampMagnitude(velocity, maxspeed);
        transform.position += velocity;
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    Vector3 seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position);  // A vector pointing from the position to the target
                                                          // Scale to maximum speed
        desired.Normalize();
        desired *= maxspeed;

        // Steering = Desired minus Velocity
        Vector3 steer = (desired - velocity);
        steer = Vector3.ClampMagnitude(steer, maxforce);  // Limit to maximum steering force
        return steer;
    }

    Vector3 boundingBox(BoidScript b)
    {
        int Xmin = 1, Xmax = 25, Ymin = -1, Ymax = 1, Zmin = 1, Zmax = 25;
        Vector3 v = new Vector3(0, 0, 0);

            if (b.transform.position.x < Xmin)
            {
                v.x += 0.5f;
            }

            else if (b.transform.position.x > Xmax)
            {
                v.x += -0.5f;

            }
            if (b.transform.position.y < Ymin)
            {
                v.y += 0.1f;
            }

            else if (b.transform.position.y > Ymax)
            {
                v.y += -0.1f;
            }

            if (b.transform.position.z < Zmin)
            {
                v.z += 0.5f;
            }

            else if (b.transform.position.z > Zmax)
            {
                v.z += -0.5f;
            }

        return v;
    }

    // Separation
    // Method checks for nearby boids and steers away
    Vector3 separate(List<BoidScript> boids)
    {
        float desiredseparation = 1.0f;
        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        for (int i = 0; i < boids.Count; i++)
        {
            if (checkColour(this, boids[i]))
            {
                float d = Vector3.Distance(transform.position, boids[i].transform.position);
                // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
                if ((d > 0) && (d < desiredseparation))
                {
                    // Calculate vector pointing away from neighbor
                    Vector3 diff = (transform.position - boids[i].transform.position);
                    diff.Normalize();
                    diff /= d;        // Weight by distance
                    steer += diff;
                    count++;            // Keep track of how many
                }
            }
        }
        // Average -- divide by how many
        if (count > 0)
        {
            steer /= ((float)count);
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0)
        {
            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= maxspeed;
            steer -= velocity;
            steer = Vector3.ClampMagnitude(steer, maxforce);
        }
        return steer;
    }

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    Vector3 align(List<BoidScript> boids)
    {
        float neighbordist = 2.5f;
        Vector3 sum = new Vector3(0, 0);
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            if (checkColour(this, boids[i]))
            {
                float d = Vector3.Distance(transform.position, boids[i].transform.position);
                if ((d > 0) && (d < neighbordist))
                {
                    sum += boids[i].velocity;
                    count++;
                }
            }
        }
        if (count > 0)
        {
            sum /= ((float)count);

            // Implement Reynolds: Steering = Desired - Velocity
            sum.Normalize();
            sum *= maxspeed;
            Vector3 steer = (sum - velocity);
            steer = Vector3.ClampMagnitude(steer, maxforce);
            return steer;
        }
        else
        {
            return new Vector3(0, 0);
        }
    }

    // Cohesion
    // For the average position (i.e. center) of all nearby boids, calculate steering vector towards that position
    Vector3 cohesion(List<BoidScript> boids)
    {
        float neighbordist = 2.5f;
        Vector3 sum = new Vector3(0, 0);   // Start with empty vector to accumulate all positions
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            if (checkColour(this, boids[i]))
            {
                float d = Vector3.Distance(transform.position, boids[i].transform.position);
                if ((d > 0) && (d < neighbordist))
                {
                    sum += boids[i].transform.position; // Add position
                    count++;
                }
            }
        }

        if (count > 0)
        {
            sum /= count;
            return seek(sum);  // Steer towards the position
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    bool checkColour(BoidScript b, BoidScript c)
    {
        if (b.gameObject.tag == c.gameObject.tag)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}

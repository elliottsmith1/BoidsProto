using UnityEngine;
using System.Collections;

public class BoidScript : MonoBehaviour {

    public float speed = 1.0f;
    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    float r;
    float maxforce;    // Maximum steering force
    float maxspeed;    // Maximum speed

    ControlScript controlScript;

    // Use this for initialization
    void Start () {
        acceleration = new Vector3(0, 0);

        // This is a new Vector3 method not yet implemented in JS
        // velocity = Vector3.random2D();

        // Leaving the code temporarily this way so that this example runs in JS
        float angle = random(TWO_PI);
        velocity = new Vector3(cos(angle), sin(angle));

        position = new Vector3(x, y);
        r = 2.0;
        maxspeed = 2;
        maxforce = 0.03;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

    }

    public void run(GameObject b)
    {
        flock(b);
        update();
    }

    Boid(float x, float y)
    {
        
    }

    void applyForce(Vector3 force)
    {
        // We could add mass here if we want A = F / M
        acceleration += force;
    }

    // We accumulate a new acceleration each time based on three rules
    void flock(ArrayList<Boid> boids)
    {
        Vector3 sep = separate(boids);   // Separation
        Vector3 ali = align(boids);      // Alignment
        Vector3 coh = cohesion(boids);   // Cohesion
                                         // Arbitrarily weight these forces
        sep *= 1.5f;
        ali *= 1.0f;
        coh *= 1.0f;
        // Add the force vectors to acceleration
        applyForce(sep);
        applyForce(ali);
        applyForce(coh);
    }

    // Method to update position
    void update()
    {
        // Update velocity
        velocity += acceleration;
        // Limit speed
        velocity = Vector3.ClampMagnitude(velocity, maxspeed);
        position += velocity;
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    Vector3 seek(Vector3 target)
    {
        Vector3 desired = (target - position);  // A vector pointing from the position to the target
                                                          // Scale to maximum speed
        desired.Normalize();
        desired *= maxspeed;

        // Above two lines of code below could be condensed with new Vector3 setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector3 steer = (desired - velocity);
        steer = Vector3.ClampMagnitude(steer, maxforce);  // Limit to maximum steering force
        return steer;
    }

    // Separation
    // Method checks for nearby boids and steers away
    Vector3 separate(ArrayList<Boid> boids)
    {
        float desiredseparation = 25.0f;
        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        for (Boid other : boids)
        {
            float d = Vector3.Distance(position, other.position);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < desiredseparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = (position - other.position);
                diff.Normalize();
                diff /= d;        // Weight by distance
                steer += diff;
                count++;            // Keep track of how many
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
            // First two lines of code below could be condensed with new Vector3 setMag() method
            // Not using this method until Processing.js catches up
            // steer.setMag(maxspeed);

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
    Vector3 align(ArrayList<Boid> boids)
    {
        float neighbordist = 50;
        Vector3 sum = new Vector3(0, 0);
        int count = 0;
        for (Boid other : boids)
        {
            float d = Vector3.Distance(position, other.position);
            if ((d > 0) && (d < neighbordist))
            {
                sum += other.velocity;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= ((float)count);
            // First two lines of code below could be condensed with new Vector3 setMag() method
            // Not using this method until Processing.js catches up
            // sum.setMag(maxspeed);

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
    Vector3 cohesion(ArrayList<Boid> boids)
    {
        float neighbordist = 50;
        Vector3 sum = new Vector3(0, 0);   // Start with empty vector to accumulate all positions
        int count = 0;
        for (Boid other : boids)
        {
            float d = Vector3.Distance(position, other.position);
            if ((d > 0) && (d < neighbordist))
            {
                sum += other.position; // Add position
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            return seek(sum);  // Steer towards the position
        }
        else
        {
            return new Vector3(0, 0);
        }
    }
}
}

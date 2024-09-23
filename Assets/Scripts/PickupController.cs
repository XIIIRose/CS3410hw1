//      Autumn Rose
//      22 September 2024
//      This script controls the movement and speed of the projectiles.git

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    // variables
    private Rigidbody2D rbPickup2d;
    public float speed;
    private Vector2 movement;
    private float moveHorizontal;
    private float moveVertical;
    private float bounceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        // connect rbPickup2d to the Pickup rigidbody
        rbPickup2d = GetComponent<Rigidbody2D>();
        // generate random direction, and initialize velocity
        moveHorizontal = Random.Range(-10.0f, 10.0f);
        moveVertical = Random.Range(-10.0f, 10.0f);
        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        rbPickup2d.velocity = movement * speed;
        // bounce multiplier increased for projectiles hitting walls
        bounceMultiplier = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // keep the projectile moving in its intended direction
        rbPickup2d.velocity = movement * speed;
    }

    // this function increases the speed of the projectiles when called by PlayerController
    public void IncreaseSpeed(float increaseAmt)
    {
        // increase the speed of the projectiles and adjust velocity accordingly
        speed += increaseAmt;
        rbPickup2d.velocity = movement * speed;
    }

    // projectile collision controller
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the projectile collides with the perimeter of the background
        if (collision.gameObject.tag == "Background")
        {
            // bounce the projectile off the wall it has hit
            movement = Vector2.Reflect(movement, collision.contacts[0].normal).normalized * bounceMultiplier;
            rbPickup2d.velocity = movement * speed;

            // Debug.Log("Velocity after collision: " + rbPickup2d.velocity);
            // Debug.Log("Direction: " + movement);
        }
    }
}

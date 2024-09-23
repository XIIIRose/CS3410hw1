//      Autumn Rose
//      22 September 2024
//      This script is attached the the player game object and controls the player movement, timer, text displayed, restart button, 
//      and the speed increase during the last third of the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    // argument list
    private Rigidbody2D rb2d;
    public float speed;
    float timer;
    int seconds;
    public Text winText;
    public Text countdownText;
    public Text gameOverText;
    public Button restartButton;
    private Boolean gameLost;
    public PickupController[] projectiles;
    private Boolean speedIncreased;

    // Start is called before the first frame update
    void Start()
    {
        // connect to Player rigidbody
        rb2d = GetComponent<Rigidbody2D>();

        // initialize timer and countdownText
        timer = 60.0f; // changed from timer = 0.0f;
        countdownText.text = "Timer: " + timer.ToString();
        
        // initialize win/loss text, reset button, and boolean event variables
        winText.text = "";
        gameOverText.text = "";
        restartButton.gameObject.SetActive(false);
        gameLost = false;
        speedIncreased = false;

        // connect to all projectiles
        projectiles = FindObjectsOfType<PickupController>();
    }

    // Update is called once per frame
    // FixedUpdate is in sync with physics engine
    void FixedUpdate()
    {
        // keep the player moving in the specified direction from the keyboard, at the input/calculated speed
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity = movement * speed;

        // increase timer by time passed since last frame, convert to remaining seconds, and display until 0 is reached
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime; // decrease timer
        }
        seconds = Mathf.Max(0, (int)timer);
        countdownText.text = "Timer: " + seconds.ToString();

        // if 60s passed and the player has not been hit, the game is won
        if (timer <= 0.0f && !gameLost)
        {
            // display win text
            winText.text = "You win!";
            // show reset button
            restartButton.gameObject.SetActive(true);
            // timer will no longer increase
            timer = 0.0f;
        }

        // at 20s mark, the speed of the player and projectiles will increase by ~87%
        if (timer <= 20.0f && !speedIncreased)
        {
            // mark speed as being increased, so it will only happen once
            speedIncreased = true;
            // increase player speed and every projectile
            IncreasePlayerSpeed(2.8f);
            foreach (PickupController projectile in projectiles)
            {
                projectile.IncreaseSpeed(2.8f);
            }
        }
    }

    // function for behavior upon the player colliding with something
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if the "other" is a Pickup...
        if (other.gameObject.CompareTag("Pickup"))
        {
            // then check timer - if timer > 0s, then it is game over
            if (timer > 0.0f)
            {
                // game over signified and text and restart button displayed
                gameLost = true;
                gameOverText.text = "GAME OVER";
                restartButton.gameObject.SetActive(true);
            }
            // Debug.Log("Collision: " + other.gameObject.name);
        }
    }

    // function to increase player speed by amount passed
    public void IncreasePlayerSpeed(float amt)
    {
        speed += amt;
    }

    // restarts the scene when the restart button is pressed
    public void OnRestartButtonPress()
    {
        // Debug.Log("Restart button clicked");
        SceneManager.LoadScene("SampleScene");
    }
}

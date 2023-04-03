using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    public float speed;  // The speed of the enemy
    public Rigidbody2D rb;  // The Rigidbody2D component attached to the enemy
    public int health = 100;  // The starting health of the enemy
    public GameObject deathEffect;  // The particle effect that plays when the enemy is destroyed
    private PlayerCombat player;  // A reference to the player's combat script
    public int damage;  // The amount of damage the enemy deals to the player


    // This method is called when the enemy takes damage
    public void TakeDamages(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
        }
    }

    // This method is called when the enemy is destroyed
    private void Destroy()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (player != null)
        {
            player.AddScore(2);
        }
    }

    // This method is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerCombat>(); 

        
        StartCoroutine(RandomizeMovement());  // Start the coroutine that randomizes the enemy's movement
    }


    private IEnumerator RandomizeMovement()
    {
        while (true)
        {
            // Randomize direction and speed
            Vector2 direction = Vector2.down;
            float angle = Random.Range(-45f, 45f);
            direction = Quaternion.Euler(0, 0, angle) * direction;
            float speed = Random.Range(0.2f, 1f) * this.speed;

            // Lerp towards new direction and speed over 1 second
            float t = 0f;
            Vector2 initialVelocity = rb.velocity;
            while (t < 1f)
            {
                rb.velocity = Vector2.Lerp(initialVelocity, direction * speed, t);
                t += Time.deltaTime;
                yield return null;
            }

            // Wait for a random amount of time before randomizing again
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    // This function is called when the object collides with another object with a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            // Randomize direction and speed
            Vector2 direction = Vector2.down;
            float angle = Random.Range(-45f, 45f);
            direction = Quaternion.Euler(0, 0, angle) * direction;
            float speed = Random.Range(0.2f, 3f) * this.speed;

            // Lerp towards new direction and speed over 1 second
            float t = 0f;
            Vector2 initialVelocity = rb.velocity;
            while (t < 1f)
            {
                rb.velocity = Vector2.Lerp(initialVelocity, direction * speed, t);
                t += Time.deltaTime;
            }

            // Randomize again after lerping to the new direction
            float randomWaitTime = Random.Range(0.5f, 2f);
            Invoke(nameof(RandomizeMovement), randomWaitTime);
        }
        if (other.CompareTag("Health_Border"))
        {
            Destroy(gameObject);
        }
        PlayerScript player = other.GetComponent<PlayerScript>();
        if (player != null)
        {
            // Deal damage to the player and destroy this object
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    // This function is called when the object becomes invisible
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}



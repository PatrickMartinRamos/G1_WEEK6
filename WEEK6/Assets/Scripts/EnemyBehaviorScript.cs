using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public int health = 100;
    //public GameObject impactEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        Destroy(gameObject);     
    }

    void Start()
    {
        // Calculate direction vector based on spawn point angle
        Vector2 direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.down;
        rb.velocity = direction * speed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
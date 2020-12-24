using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject projectile;
    public float speed;
    Rigidbody2D rb;

    void Start()
    {
        
        InvokeRepeating("Shoot", 0.5f, 1.0f);
    }

    void Shoot()
    {
        Instantiate(projectile, transform.position + new Vector3(0, -2, 0), transform.rotation);
    }
}

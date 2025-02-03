using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class FireController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0F;

    [SerializeField]
    float lifeTime = 3.0F;

    Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

    void FixedUpdate() 
    {
        _rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy (gameObject);
    }
}

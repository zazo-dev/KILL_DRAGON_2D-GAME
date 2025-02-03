using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float velocidadBala = 5f;
    [SerializeField]
    [Range(0.1F, 1.0F)]
    float firerate = 0.3F;
    float _fireTimer;

    [SerializeField]
    float maxHealth = 500.0f; 
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0.0F)
        {
            Shoot();
            _fireTimer = firerate;
        }
    }

    void Shoot()
    {
        // Acá se instancia la bala desde el prefab en la posición del punto de disparo escogido
        GameObject bala = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);

        // Se accede al componente Rigidbody2D de la bala y se le aplica velocidad
        Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
        if (rbBala != null)
        {
            rbBala.velocity = puntoDisparo.right * velocidadBala;
        }
    }

    public void TakeDamage(float damage)
    {
        //Código para daño al enemigo
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}

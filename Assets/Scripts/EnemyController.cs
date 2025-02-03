using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100.0f;
    private float currentHealth;
    
    

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // Se aplica daño al enemigo
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Vector2 contactPoint = other.GetContact(0).normal;

            if (contactPoint.y < -0.9F)
            {
                Character2DController.Instance.Rebound();
                Destroy(gameObject);
            }
            else
            {
                HealthController controller = other.collider.GetComponent<HealthController>();
                if (controller != null)
                {
                    float damage = 10.0f; 
                    // Se aplicar daño al jugador
                    controller.TakeDamage(damage, contactPoint);

                    // Se elimina al enemigo luego de infligir daño al jugador
                    TakeDamage(damage); 
                }

            }
           
            }
        }
    }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlataformaMortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Se obtiene el componente HealthController del jugador y se llama al método HandlePlayerDeath
            HealthController playerHealth = other.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.HandlePlayerDeath();
                MostrarMenuGameOver();
            }
        }
    }
    private void MostrarMenuGameOver()
    {
        // Se busca el objeto con el script MenuGameOver y activa el menú Game Over
        MenuGameOver menuGameOver = FindObjectOfType<MenuGameOver>();
        if (menuGameOver != null)
        {
            menuGameOver.ActivarMenu(null, null);
        }
    }
}
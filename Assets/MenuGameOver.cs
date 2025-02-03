using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    [SerializeField] private GameObject menuGameOver;

    private HealthController healthController;
    private AudioSource _gameOver;

    private void Start()
    {
        healthController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        healthController.MuerteJugador += ActivarMenu;
        _gameOver = GetComponent<AudioSource>();
    }

    public void ActivarMenu(object sender, EventArgs e)
    {
        menuGameOver.SetActive(true);
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}

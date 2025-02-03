using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void CargaEscena(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void CargaEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

}

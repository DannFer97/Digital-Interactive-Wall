using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

[System.Serializable]
public class ImagenEscenaPair
{
    public Sprite imagen;
    public string nombreEscena;
    
}


public class MenuDeslizante : MonoBehaviour
{
    public Image imagenJuego;
    public Sprite[] imagenes;
    public ImagenEscenaPair[] imagenEscenaPairs;  // Relación entre imágenes y escenas.
    public GameObject Icono;
    public GameObject Siguiente;
    public GameObject Atras;
    //public GameObject AudioMgr;
    //public GameObject SerialMgr;
    public SerialManager puerto;
    public AudioClip menuBackgroundMusic;

    private bool canExecute = true; // Variable de control
    private float cooldownTime = 0.5f; // Tiempo de enfriamiento en segundos
    private int cont =0;


    public int indiceActual;

    private void Start()
    {
        indiceActual = 0;
        ActualizarImagen();
        // Reproducir la música de fondo del menú
        BackgroundMusic.Instance.PlayBackgroundMusic(menuBackgroundMusic);
        //AudioMgr.SetActive(true);
        //SerialMgr.SetActive(true);
    }

    public void SiguienteImagen()
    {
        if (canExecute)
        {
            indiceActual++;
            if (indiceActual >= imagenes.Length)
            {
                indiceActual = 0;
            }
            ActualizarImagen();
            // Desactiva la variable de control y establece el temporizador
            canExecute = false;
            Invoke("ResetCooldown", cooldownTime);
        }
        else {

        }
        
    }

    public void ImagenAnterior()
    {
        if (canExecute)
        {
            indiceActual--;
            if (indiceActual < 0)
            {
                indiceActual = imagenes.Length - 1;
            }
            ActualizarImagen();
            //cont++;
            //Debug.Log("contador: "+cont);
            // Desactiva la variable de control y establece el temporizador
            canExecute = false;
            Invoke("ResetCooldown", cooldownTime);
        }
        
    }

    // Función para restablecer la variable de control
    private void ResetCooldown()
    {
        canExecute = true;
        Debug.Log("Espera "+cooldownTime+" segundos");
    }

    public void ActualizarImagen()
    {
        imagenJuego.sprite = imagenes[indiceActual];
    }

    public void CargarEscenaCorrespondiente()
    {
        //AudioMgr.SetActive(false);
        //SerialMgr.SetActive(false);
        //puerto.CerrarPuerto();
        if (canExecute)
        {
            Debug.Log("se cerro puerto: Menu");
            
            string nombreEscena = imagenEscenaPairs[indiceActual].nombreEscena;
            SceneManager.LoadScene(nombreEscena);
            // Desactiva la variable de control y establece el temporizador
            canExecute = false;
            Invoke("ResetCooldown", cooldownTime);
        }
        else{

        }
        
    }

    
}

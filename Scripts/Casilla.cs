using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Casilla : MonoBehaviour
{
    public Button casilla;
    public TextMeshProUGUI textoCasilla;
    //public string playerside;
    public AudioSource sonidocasilla;
    public AudioClip seleccionCasilla;
    private bool canExecute = true; // Variable de control
    public float cooldownTime = 1.5f; // Tiempo de enfriamiento en segundos
    private int cont=0;

    private ControladorTresEnRaya controladorjuego;

    public void SetSpace(){
        if (canExecute)
        {
            if (controladorjuego.side==false)
            {
                textoCasilla.text=controladorjuego.GetPlayerSide();
                casilla.interactable=false;
                controladorjuego.EndTurn();
                sonidocasilla.clip=seleccionCasilla;
                sonidocasilla.Play();
                //cont++;
                //Debug.Log("contador: "+cont);

            // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            }
            
        }
        else{

        }
        
    }

    // Función para restablecer la variable de control
    private void ResetCooldown()
    {
        canExecute = true;
    }

    public void SetGameControllerReference(ControladorTresEnRaya controlador)
    {
        controladorjuego=controlador;
    }


}

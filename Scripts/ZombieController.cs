using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    //public Animator animatorZombieDeath;
    private void OnMouseDown()
    {
        Debug.Log("Se dio click");
        
        ;
        // Obtener la referencia al script ZoombiesShooter en el objeto raíz del juego
        ZoombiesShooter zoombiesShooter = GameObject.FindObjectOfType<ZoombiesShooter>();

        if (zoombiesShooter != null)
        {
            //Animator animatorZombieDeath = activeZombie.GetComponent<Animator>(); 
            zoombiesShooter.EliminateZombie(); // Llamar a la función EliminateZombie del script ZoombiesShooter
        }
    }
}

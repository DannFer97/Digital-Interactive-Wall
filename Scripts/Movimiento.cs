using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Movimiento : MonoBehaviour
{
    private float velocidadMinima = 1.3f; // Velocidad mínima de movimiento del bloque
    private float velocidadMaxima = 1.5f; // Velocidad máxima de movimiento del bloque
    private Vector3 direccionMovimiento; // Dirección de movimiento actual del bloque
    private float velocidadMovimiento; // Velocidad actual de movimiento del bloque
    public GameObject textoMovimiento; // referencia a el texto
    public GameObject[] bloques;
    private Rigidbody rb;
    //public Text primerdigito;



    // Start is called before the first frame update
    void Start()
    { 
    // Inicializar dirección de movimiento aleatoria
    direccionMovimiento = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    //direccionMovimiento = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    //Debug.Log("1: "+direccionMovimiento);
    textoMovimiento.transform.position=new Vector3(direccionMovimiento.x,direccionMovimiento.y,direccionMovimiento.z+0.1f);
    //Debug.Log("2: "+textoMovimiento.transform.position);
    // Inicializar velocidad de movimiento aleatoria
    velocidadMovimiento = Random.Range(velocidadMinima, velocidadMaxima);
    rb = GetComponent<Rigidbody>();
    // Invocar repetidamente la función CambiarDireccionMovimiento cada 3 segundos
        InvokeRepeating("CambiarDireccionMovimiento", 0f, 5f);
    
    }
    // Cambiar la dirección de movimiento de manera aleatoria
    private void CambiarDireccionMovimiento()
    {
        direccionMovimiento = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
    
    // Verificar si el bloque ha chocado contra las paredes
    if (transform.position.y >= 7.5f || transform.position.y <= 0.740f)
        {
            direccionMovimiento.y *= -1; // Invertir la dirección en el eje Y
            //textoMovimiento.transform.position = new Vector3(textoMovimiento.transform.position.x, direccionMovimiento.y, textoMovimiento.transform.position.z);
        }

        if (transform.position.x >= 16.26f || transform.position.x <= -14.5f)
        {
            direccionMovimiento.x *= -1; // Invertir la dirección en el eje X
            //  textoMovimiento.transform.position = new Vector3(direccionMovimiento.x, textoMovimiento.transform.position.y, textoMovimiento.transform.position.z);
        }//*/
        // Calcular la velocidad en el eje X y Y
        float velocidadX = direccionMovimiento.x * velocidadMovimiento;
        float velocidadY = direccionMovimiento.y * velocidadMovimiento;

        // Aplicar la velocidad al Rigidbody
        Vector3 movimiento = new Vector3(velocidadX, velocidadY, 0f);
        rb.velocity = movimiento;

        // Limitar el movimiento dentro de los límites establecidos
        float posicionX = Mathf.Clamp(transform.position.x, -14.5f, 16.26f);
        float posicionY = Mathf.Clamp(transform.position.y, 0.740f, 7.5f);
        transform.position = new Vector3(posicionX, posicionY, transform.position.z);
        textoMovimiento.transform.position =new Vector3(transform.position.x,transform.position.y, transform.position.z+0.1f );
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si la colisión es con otro bloque
        if (collision.gameObject.CompareTag("Bloque"))
        {
            Debug.Log("Choco");
            // Cambiar la dirección de movimiento de ambos bloques
        //CambiarDireccion();
        //collision.gameObject.GetComponent<Movimiento>().CambiarDireccion();
        CambiarDireccionMovimiento();
        }
    }

    private void CambiarDireccion()
    {
        // Invertir la dirección en los ejes X y Y
        direccionMovimiento = new Vector3(-direccionMovimiento.x, -direccionMovimiento.y, 0f);
    }

}
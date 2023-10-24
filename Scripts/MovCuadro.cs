using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovCuadro : MonoBehaviour
{
    private float velocidadMinima = 0.7f; // Velocidad mínima de movimiento del bloque
    private float velocidadMaxima = 1f; // Velocidad máxima de movimiento del bloque
    private Vector3 direccionMovimiento; // Dirección de movimiento actual del bloque
    private float velocidadMovimiento; // Velocidad actual de movimiento del bloque
    //public GameObject textoMovimiento; // referencia a el texto
    //public GameObject[] bloques;
    private Rigidbody rb;
    public GameObject cuadro;

    void Start()
    { 
    // Inicializar dirección de movimiento aleatoria
    direccionMovimiento = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    //direccionMovimiento = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    //Debug.Log("1: "+direccionMovimiento);
    
    //Debug.Log("2: "+textoMovimiento.transform.position);
    // Inicializar velocidad de movimiento aleatoria
    velocidadMovimiento = Random.Range(velocidadMinima, velocidadMaxima);
    velocidadMovimiento=1.5f;
    rb = GetComponent<Rigidbody>();
    // Invocar repetidamente la función CambiarDireccionMovimiento cada 3 segundos
        InvokeRepeating("CambiarDireccionMovimiento", 0f, 3f);
    
    }
    // Cambiar la dirección de movimiento de manera aleatoria
    private void CambiarDireccionMovimiento()
    {
        direccionMovimiento = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
    //transform.position += direccionMovimiento * velocidadMovimiento * Time.deltaTime;
    // Verificar si el bloque ha chocado contra las paredes
    if (transform.position.y >= 12.9f || transform.position.y <= 11f)
        {
            direccionMovimiento.y *= -1;
            //Debug.Log("cambia direccion y");
             // Invertir la dirección en el eje Y
            //textoMovimiento.transform.position = new Vector3(textoMovimiento.transform.position.x, direccionMovimiento.y, textoMovimiento.transform.position.z);
        }

        if (transform.position.x >= 28.6f || transform.position.x <= 18f)
        {
            direccionMovimiento.x *= -1; // Invertir la dirección en el eje X
            //Debug.Log("cambia direccion x");
            //  textoMovimiento.transform.position = new Vector3(direccionMovimiento.x, textoMovimiento.transform.position.y, textoMovimiento.transform.position.z);
        }//*/
        // Calcular la velocidad en el eje X y Y
        float velocidadX = direccionMovimiento.x * velocidadMovimiento;
        float velocidadY = direccionMovimiento.y * velocidadMovimiento;

        // Aplicar la velocidad al Rigidbody
        Vector3 movimiento = new Vector3(velocidadX, velocidadY, 0f);
        rb.velocity = movimiento;

        // Limitar el movimiento dentro de los límites establecidos
        float posicionX = Mathf.Clamp(transform.position.x, 18f, 28.6f);
        //Debug.Log("direccion x: "+posicionX);
        float posicionY = Mathf.Clamp(transform.position.y, 11f, 12.9f);
        //Debug.Log("direccion x: "+posicionY);
        transform.position = new Vector3(posicionX, posicionY, transform.position.z);
        //textoMovimiento.transform.position =new Vector3(transform.position.x,transform.position.y, transform.position.z+0.1f );
        
    }


}

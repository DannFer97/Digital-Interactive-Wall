using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rueda : MonoBehaviour
{
    public GameObject rueda; // Referencia al objeto de la ruleta de la fortuna

    private bool giroActivo = false;
    public Button Inicio;
    public Button Menu;
    public float velocidadGiro;
    public float detenerGiro;
    public float Angulo;
    private Rigidbody2D rbody;
    public int division;
    public bool esperarTiempo = false;
    public float duracionEspera = 2f; // Tiempo de espera en segundos
    public float tiempoEsperado;
    public AudioSource sonidoRespuesta;
    public AudioClip gira;
    public AudioClip ruedaGameBackgroundMusic;

    //public SerialManager puerto;
    
    

    /* private void Awake(){
        DontDestroyOnLoad(AudioMgr);
    }*/
    private void Start()
    {
        // Desactivar la ruleta al inicio
        //rueda.SetActive(false);
        //AudioMgr.SetActive(true);
        Time.timeScale = 1f;
        rueda.transform.eulerAngles = new Vector3(270f, 0f, 0f);
        BackgroundMusic.Instance.PlayBackgroundMusic(ruedaGameBackgroundMusic);
        //Debug.Log("aun no se da  click"+giroActivo);
        rbody = GetComponent<Rigidbody2D>();
    }

        // Si el botón de inicio es presionado y el giro no está activo, iniciar el giro
    private void Update()
    {
        if (giroActivo==true)
        {
            rueda.transform.Rotate(0,velocidadGiro,0);
            velocidadGiro -= detenerGiro;
        }
        if (velocidadGiro<=0)
        {
            velocidadGiro=0;
            giroActivo=false;
            //sonidoRespuesta.Stop();  
            Vector3 rotacion = rueda.transform.eulerAngles;  
            float Angulo=rbody.rotation;
            //Debug.Log("Rotacion es: "+Angulo);
            //int division=ObtenerDivisionActual(Angulo);
            //
            if ((Angulo > 57.92 && Angulo<90)||((Angulo>-131.43 && Angulo<-92.26)))
            {
                division=0;  
            }
        //Suma
            else if ((Angulo>24.53 && Angulo<57.02)||((Angulo>-160.81 && Angulo<-132.53)))
            {
                division=1;  
            }
        //Division
            else if ((Angulo>-49.66 && Angulo<-12.24)||((Angulo>121.17 && Angulo<153.55)))
            {
                division=3;
            }
        //Multiplicación
            else if((Angulo>89.11 && Angulo<120.27)||((Angulo>-89.99 && Angulo<-50.76)))
            {
                division=4;
            }
            else
            {
                division=2;
            }
            esperarTiempo = true;
            tiempoEsperado = duracionEspera;

            //Debug.Log("Division: "+division); 
            
            if (esperarTiempo && Time.time >= tiempoEsperado)
            {
                esperarTiempo = false;
                CargarMinijuego(division);
                tiempoEsperado=0;
            }                    
        }
            

    } //=> Inicio.onClick.AddListener(IniciarGiro);

    public void IniciarGiro()
    {
        velocidadGiro=Random.Range(1.000f,2.000f);
        detenerGiro=Random.Range(0.003f,0.009f );
        giroActivo=true;
        sonidoRespuesta.clip=gira;
        sonidoRespuesta.Play(); 
       // Debug.Log("Se dio click"+giroActivo);
        
    }


private int ObtenerDivisionActual(float rotacion)
    {
        // Implementa el código para determinar en qué división se detuvo la ruleta
        // Puedes usar la rotación actual de la ruleta o generar un número aleatorio
        
        int division =0;
        //Resta
        if ((rotacion>57.92 && rotacion<90)||((rotacion>-131.43 && rotacion<-92.26)))
        {
            division=0;
            
        }
        //Suma
        else if ((rotacion>24.53 && rotacion<57.02)||((rotacion>-160.81 && rotacion<-132.53)))
        {
            division=1;
            
        }
        //COmbinada
        else if ((rotacion>-11.21 && rotacion<23.57)||((rotacion>-170.84 && rotacion<154.5)))
        {
            division=2;
        }
        //Division
        else if ((rotacion>-49.66 && rotacion<-12.24)||((rotacion>121.17 && rotacion<153.55)))
        {
            division=3;
            
        }
        //Multiplicación
        else if((rotacion>89.11 && rotacion<120.27)||((rotacion>-89.99 && rotacion<-50.76)))
        {
            division=4;
            
        }
        else
        {
            division=2;
        }
        return division;
       //Debug.Log("La division es: "+ division);


        
    }

    private void CargarMinijuego(int division)
    {
    // Implementa el código para cargar la escena del minijuego según la división seleccionada

    // Ejemplo: Si la división es 0, cargar el minijuego de suma
    // Guardar la división seleccionada en PlayerPrefs
    PlayerPrefs.SetInt("DivisionSeleccionada", division);
    //puerto.CerrarPuerto();
    Debug.Log("puerto cerrado: Rueda");
    // Cargar la escena del minijuego
    SceneManager.LoadScene("Minijuego");

    }
    public void RegresarMenu(){
        //puerto.CerrarPuerto();
        Debug.Log("puerto cerrado: Rueda");
        //AudioMgr.SetActive(false);
        SceneManager.LoadScene("Menu");
        
        //AudioMgr.SetActive(true);
        
    }
}  
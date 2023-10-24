using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Calcular : MonoBehaviour
{
    public TextMeshProUGUI primerDigito, segundoDigito,signo,Alt1,Alt2,Alt3, respuesta;
    public Animator animatorRespuesta,animatorFin,animatorCheckSi;
    //public GameObject cubp; //referencia a la operación principal   
    int primerValor,segundoValor,aux,aux2,resultado,alternativa1, alternativa2, puntaje;
    private string varsigno,OpTemp; 
    public Sprite spriteSi, spriteNo, spriteTransparente;
    public GameObject check1, check2, check3;
    public GameObject Alternativa1, Alternativa2, Alternativa3, botonAtras;
    public float duracionEspera = 1f; // Tiempo de espera en segundos
    public GameObject estrellaGameObject;
    public Transform estrella;
    public SerialManager puerto;

    public AudioSource sonidoRespuesta;
    public AudioClip correcto, incorrecto, felicidades;
    public AudioClip minijuegoGameBackgroundMusic;
    public GameObject felicitaciones,retroceder;

    private bool canExecute = true; // Variable de control
    public float cooldownTime = 0.5f; // Tiempo de enfriamiento en segundos

    // Start is called before the first frame update
    void Start()
    {
        int division = PlayerPrefs.GetInt("DivisionSeleccionada");
        felicitaciones.SetActive(false);
        retroceder.SetActive(false);
        estrellaGameObject.SetActive(false);
        BackgroundMusic.Instance.PlayBackgroundMusic(minijuegoGameBackgroundMusic);
        

        puntaje=0;
        
        

    // Llamar a la función correspondiente según la división
    switch (division)
    {
        case 0:
            RestaFn();
            break;
        case 1:
            SumaFn();
            break;
        case 2:
            SceneManager.LoadScene("FortuneWheel");
            break;
        case 3:
            DivisionFn();
            break;
        case 4:
            MultiplicacionFn();
            break;
    }
    }
    public void SumaFn (){
        //Debug.Log("Es una suma");
        OpTemp="suma";
        Calcula(OpTemp);
    }
    public void RestaFn (){
        OpTemp="resta";
        Calcula(OpTemp);
        //Debug.Log("Es una resta");
    }
    public void MultiplicacionFn (){
        OpTemp="multiplicacion";
        Calcula(OpTemp);
        //Debug.Log("Es una multiplicacion");
    }
    public void DivisionFn (){
        OpTemp="division";
        Calcula(OpTemp);
        //Debug.Log("Es una division");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)){
            Calcula("suma");
        }
        if (Input.GetKeyDown(KeyCode.R)){
            Calcula("resta");
        }
        if (Input.GetKeyDown(KeyCode.M)){
            Calcula("multiplicacion");
        }
        if (Input.GetKeyDown(KeyCode.D)){
            Calcula("division");
        }
    }


    public void Calcula (string operacion)
    {
        ResetValores();
        respuesta.text="= ?";
        primerValor=Random.Range(1,10);
        segundoValor=Random.Range(1,10);
        if (primerValor-segundoValor<0){
            aux=segundoValor;
            segundoValor=primerValor;
            primerValor=aux;
        }

        if(operacion =="suma"){
            resultado=primerValor+segundoValor;
            varsigno="suma";
        }
        if(operacion =="resta"){
            resultado=primerValor-segundoValor;
            varsigno="resta";
        }
        if(operacion =="multiplicacion"){
            resultado=primerValor*segundoValor;
            varsigno="multiplicacion";
        }
        if(operacion =="division"){
            resultado=primerValor/segundoValor;
            varsigno="division";
        }
        //Debug.Log("primer valor: "+primerValor+"segundo Valor: "+segundoValor+"= "+resultado);

        primerDigito.text=primerValor.ToString();
        segundoDigito.text=segundoValor.ToString();
        if (varsigno=="suma"){
            signo.text="+";
        }
        if (varsigno=="resta"){
            signo.text="-";
        }
        if (varsigno=="multiplicacion"){
            signo.text="x";
        }
        if (varsigno=="division"){
            signo.text="÷";
        }
        //Primerra Alternativa
        aux2=Random.Range(2,20);
        while(aux2==resultado){
            aux2=Random.Range(2,20);
        }
        alternativa1=aux2;
        //Primerra Alternativa
        aux2=Random.Range(2,20);
        while(aux2==resultado||aux2==alternativa1){
            aux2=Random.Range(2,20);
        }
        alternativa2=aux2;
        //Debug.Log("alternativa1: "+alternativa1);
        //Debug.Log("alternativa2: "+alternativa2);
        //Orden respuestas
        aux2=Random.Range(1,7);
        switch(aux2){
            case 1:
            Alt1.text=resultado.ToString();
            Alt2.text=alternativa1.ToString();
            Alt3.text=alternativa2.ToString();
            break;
            case 2:
            Alt1.text=resultado.ToString();
            Alt2.text=alternativa2.ToString();
            Alt3.text=alternativa1.ToString();
            break;
            case 3:
            Alt1.text=alternativa1.ToString();
            Alt2.text=resultado.ToString();
            Alt3.text=alternativa2.ToString();
            break;
            case 4:
            Alt1.text=alternativa1.ToString();
            Alt2.text=alternativa2.ToString();
            Alt3.text=resultado.ToString();
            break;
            case 5:
            Alt1.text=alternativa2.ToString();
            Alt2.text=resultado.ToString();
            Alt3.text=alternativa1.ToString();
            break;
            case 6:
            Alt1.text=alternativa2.ToString();
            Alt2.text=alternativa1.ToString();
            Alt3.text=resultado.ToString();
            break;
            
        }
        /* Debug.Log("empieza");
        Debug.Log("Alternativa 1:  "+ Alt1.text);
        Debug.Log("Alternativa 2:  "+ Alt2.text);
        Debug.Log("Alternativa 3:  "+ Alt3.text);
        Debug.Log("resultado "+ resultado.ToString());*/


        
    } 
    public void Alt1_accion(){
        if (canExecute)
        {
            if (Alt1.text==resultado.ToString()){
                check1.GetComponent<Image>().sprite=spriteSi;
                //Debug.Log("accion alt1, check si");
                respuesta.text="= "+resultado;
                animatorRespuesta= GameObject.Find("Resultado").GetComponent<Animator>();
                animatorCheckSi= GameObject.Find("check1").GetComponent<Animator>();
                animatorRespuesta.Play("RespuestaCorrecta");
                animatorCheckSi.Play("CheckSi");
                sonidoRespuesta.clip=correcto;
                sonidoRespuesta.Play();
                Invoke("Score",duracionEspera);
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            }else {
                check1.GetComponent<Image>().sprite=spriteNo;
                respuesta.text="= ?";
                //Debug.Log("accion alt1, check no");
                sonidoRespuesta.clip=incorrecto;
                sonidoRespuesta.Play();
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            }
        } else{

        }

    }
    public void Alt2_accion()
    {
        if (canExecute)
        {         
            if (Alt2.text==resultado.ToString()){
                check2.GetComponent<Image>().sprite=spriteSi;
                //Debug.Log("accion alt2, check si");
                respuesta.text="= "+resultado;
                animatorRespuesta= GameObject.Find("Resultado").GetComponent<Animator>();
                animatorCheckSi= GameObject.Find("check2").GetComponent<Animator>();
                animatorRespuesta.Play("RespuestaCorrecta");
                animatorCheckSi.Play("CheckSi");
                sonidoRespuesta.clip=correcto;
                sonidoRespuesta.Play();
                Invoke("Score",duracionEspera);
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            } else {
                check2.GetComponent<Image>().sprite=spriteNo;
                //Debug.Log("accion alt2, check no");
                respuesta.text="= ?";
                sonidoRespuesta.clip=incorrecto;
                sonidoRespuesta.Play();
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            }
        }
    }
    public void Alt3_accion()
    {
        if (canExecute)
        {
            if (Alt3.text==resultado.ToString()){
                check3.GetComponent<Image>().sprite=spriteSi;
                //Debug.Log("accion alt3, check si");
                respuesta.text="= "+resultado;
                animatorRespuesta= GameObject.Find("Resultado").GetComponent<Animator>();
                animatorCheckSi= GameObject.Find("check3").GetComponent<Animator>();
                animatorRespuesta.Play("RespuestaCorrecta");
                animatorCheckSi.Play("CheckSi");
                sonidoRespuesta.clip=correcto;
                sonidoRespuesta.Play();
                Invoke("Score",duracionEspera);
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            } else {
                check3.GetComponent<Image>().sprite=spriteNo;
                //Debug.Log("accion alt3, check no");
                respuesta.text="= ?";
                sonidoRespuesta.clip=incorrecto;
                sonidoRespuesta.Play();
                // Desactiva la variable de control y establece el temporizador
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
            }
        }
    }
    public void ResetValores (){
        check1.GetComponent<Image>().sprite=spriteTransparente;
        check2.GetComponent<Image>().sprite=spriteTransparente;
        check3.GetComponent<Image>().sprite=spriteTransparente;
    }
    private void Score(){

        
        if (puntaje==0){
            Debug.Log("puntaje: "+puntaje);
            Calcula(varsigno);
            estrellaGameObject.SetActive(true);
            puntaje +=1;

        }

        else if (puntaje==3){
            felicitaciones.SetActive(true);
            animatorFin= GameObject.Find("Fin").GetComponent<Animator>();
            animatorFin.Play("Final");
            sonidoRespuesta.clip=felicidades;
            sonidoRespuesta.Play();
            estrella.transform.localScale=new Vector3(0.5f,0.5f,1f);
            Instantiate(estrella, new Vector3(-16.1f+(puntaje+1)*1f, 16.14f, 14f),  Quaternion.Euler(0f, -180f, 0f));
            
            puntaje=0;
            
            
            StartCoroutine(EsperarAnimacion());
            retroceder.SetActive(true);
        }

        else {
        Debug.Log("puntaje: "+puntaje);
        Calcula(varsigno);
        estrella.transform.localScale=new Vector3(0.5f,0.5f,1f);
        Instantiate(estrella, new Vector3(-16.1f+(puntaje+1)*1f, 16.14f, 14f), Quaternion.Euler(0f, -180f, 0f));
        
        puntaje +=1;
        }
    IEnumerator EsperarAnimacion()
    {
        // Obtener el estado de la animación actual
        AnimatorStateInfo stateInfo = animatorFin.GetCurrentAnimatorStateInfo(0);
        // Calcular el tiempo de espera restando 0.5 segundos a la duración de la animación
        float tiempoEspera = stateInfo.length;

        // Esperar la duración de la animación
        yield return new WaitForSeconds(tiempoEspera);
        ResetValores();
        // Pausar el tiempo en el juego
        Time.timeScale = 0f;
    }
        
    }public void Retroceder()
    {
        //puerto.CerrarPuerto();
        Debug.Log("puerto cerrado: minijuego");
        //Time.timeScale = 1f;
        SceneManager.LoadScene("FortuneWheel");
        // Reanudar el tiempo en el juego
        
    }
    // Función para restablecer la variable de control
    private void ResetCooldown()
    {
        canExecute = true;
    }

    
}

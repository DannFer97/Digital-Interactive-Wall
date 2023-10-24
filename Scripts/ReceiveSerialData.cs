using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using TMPro;

public class ReceiveSerialData : MonoBehaviour
{
    public CoordenadasPantalla coordenadas;
    public ZoombiesShooter zombieEliminate;
    public Calcular minijuego;
    public Atras minijuegoAtras;
    public Rueda rueda;
    public Casilla casilla;
    public ControladorTresEnRaya tresEnRaya;
    public MenuDeslizante menu;
    private bool shouldWait = false;
    //private static   SerialPort puerto;

    //public SerialManager serialManager;

    private bool canExecute = true; // Variable de control
    public float cooldownTime = 1f; // Tiempo de enfriamiento en segundos

    public string ultimacoordenada="";
    public TextMeshProUGUI nuevacoordenada;
   // private float waitTime = 5f; // Tiempo de espera en segundos
   // private float lastReceivedTime = 0f; // Hora en que se recibió la última coordenada válida


    //private bool zombieEnRango = false; // Variable adicional para realizar un seguimiento del estado del rango
    
    //public string[] dataArray;
    // Start is called before the first frame update
    void Start()
    {
        SerialManager.WhenReceiveDataCall += EscucharCoordenadas;
    }
    private void OnDestroy()
    {
        SerialManager.WhenReceiveDataCall -= EscucharCoordenadas;
    }
    void EscucharCoordenadas(string incomingString)
    {
         if (shouldWait) // Si deberíamos esperar, salimos temprano
        {
            return;
        }
        
        if (CoordenadaEsValida(incomingString)) // Verificar si la coordenada es válida
        {
            shouldWait = true; // Activar la espera
            StartCoroutine(WaitAndContinueListening()); // Iniciar la corutina para esperar y continuar
        }
    
       // lastReceivedTime = Time.time; // Actualizar el tiempo de la última coordenada válida
        Scene currentScene = SceneManager.GetActiveScene();
        string escenaActiva = currentScene.name;
        string coordenadas = incomingString;
        Debug.Log("coor: "+coordenadas);
        nuevacoordenada.text=coordenadas.ToString();

        
        //string coordenadas = incomingString;
         // Obtener la posición en base a la coordenada
        Vector3 posicion = this.coordenadas.ObtenerPosicion(coordenadas);

        
        
    
        switch(escenaActiva){
            case "ZombisShooter":

            Vector3 posicionZombie=zombieEliminate.activeZombie.transform.position;
            Vector3 posicionbotonAtras1=zombieEliminate.botonAtras.transform.position;

            // Obtener el tamaño del collider del zombie
            Collider2D zombieCollider = zombieEliminate.activeZombie.GetComponent<Collider2D>();
            Collider2D botonAtrasCollider1=zombieEliminate.botonAtras.GetComponent<Collider2D>();

            Vector2 zombieSize = zombieCollider.bounds.size;
            Vector2 botonAtrasSize1 = botonAtrasCollider1.bounds.size;
            

            //Debug.Log("ZOMBIESIZE: "+zombieSize);
        
            Debug.Log("coordenadas: "+posicion);
            //Debug.Log("posicion zombie activo: "+posicionZombie);
            Vector3 localscale= (zombieEliminate.activeZombie.transform.localScale);
            //Debug.Log("escala: "+localscale);
            // Obtener la posición del centro del zombie
            Vector3 zombieCenter = zombieCollider.bounds.center;
            //Debug.Log("zobie center: "+zombieCenter);
            bool dentroDelRango = false;
            bool dentroDelRangoBotonAtras1 = false;
        
            // Verificar si la posición dada está dentro del rango ocupado por el zombie
            dentroDelRango = posicion.x >= zombieCenter.x - (zombieSize.x)/(2f) &&
                        posicion.x <= zombieCenter.x + (zombieSize.x)/(2f) &&
                        posicion.y >= zombieCenter.y - (zombieSize.y)/(2f) &&
                        posicion.y <= zombieCenter.y + (zombieSize.y)/(2f);
            
            dentroDelRangoBotonAtras1= posicion.x >= posicionbotonAtras1.x - (botonAtrasSize1.x)/(2f) &&
                        posicion.x <= posicionbotonAtras1.x + (botonAtrasSize1.x)/(2f) &&
                        posicion.y >= posicionbotonAtras1.y - (botonAtrasSize1.y)/(2f) &&
                        posicion.y <= posicionbotonAtras1.y + (botonAtrasSize1.y)/(2f);

                /*dentroDelRango = posicion.x >= posicionZombie.x - 0.55f &&
                        posicion.x <= posicionZombie.x + 0.55f &&
                        posicion.y >= posicionZombie.y - 2.0f &&
                        posicion.y <= posicionZombie.y + 2.0f;    */    
            if (dentroDelRango==true)
            {
                Debug.Log("elimina..");
                zombieEliminate.EliminateZombie();
                //serialManager.puerto.DiscardInBuffer();

               // dentroDelRango=true;
                posicion=Vector3.zero;
            }
            else if (dentroDelRangoBotonAtras1==true)
            {   
                posicion=Vector3.zero;
                zombieEliminate.RetrocederMenu();
            }
            break;
            case "Minijuego":
            Vector3 posicionAlt1=minijuego.Alternativa1.transform.position;
            Vector3 posicionAlt2=minijuego.Alternativa2.transform.position;
            Vector3 posicionAlt3=minijuego.Alternativa3.transform.position;
            Vector3 posicionBotonAtras=minijuego.botonAtras.transform.position;
            Debug.Log("posicion: "+posicion );
            //Debug.Log(posicionAlt1=minijuego.Alternativa1.transform.position);

            Collider alt1Collider = minijuego.Alternativa1.GetComponent<BoxCollider>();
            Collider alt2Collider = minijuego.Alternativa2.GetComponent<BoxCollider>();
            Collider alt3Collider = minijuego.Alternativa3.GetComponent<BoxCollider>();
            Collider2D botonAtrasCollider=minijuego.botonAtras.GetComponent<Collider2D>();
            Vector2 BotonAtrasSize = botonAtrasCollider.bounds.size;
            //Debug.Log("sizeboton: "+BotonAtrasSize);
            Vector3 Botonlocalscale= (minijuego.botonAtras.transform.localScale);
            //Debug.Log("scale boton: "+Botonlocalscale);

            /*Debug.Log("alternativa1: "+alt1Collider.bounds.size);
            Debug.Log("alternativa2: "+alt2Collider.bounds.size);
            Debug.Log("alternativa3: "+alt3Collider.bounds.size);*/
            bool dentroDelRangoAlt1 = false;
            bool dentroDelRangoAlt2 = false;
            bool dentroDelRangoAlt3 = false;
            bool dentroDelRangoBotonAtras=false;

            bool dentroDelRangoTotal = (posicion == Vector3.zero);

            if (dentroDelRangoTotal==false){
            dentroDelRangoAlt1 = posicion.x <= posicionAlt1.x + 2.5f &&
                        posicion.x >= posicionAlt1.x - 2.5f &&
                        posicion.y >= posicionAlt1.y - 2.5f &&
                        posicion.y <= posicionAlt1.y + 2.5f;
            dentroDelRangoAlt2 = posicion.x <= posicionAlt2.x + 2.5f &&
                        posicion.x >= posicionAlt2.x - 2.5f &&
                        posicion.y >= posicionAlt2.y - 2.5f &&
                        posicion.y <= posicionAlt2.y + 2.5f;
            dentroDelRangoAlt3 = posicion.x <= posicionAlt3.x + 2.5f &&
                        posicion.x >= posicionAlt3.x - 2.5f &&
                        posicion.y >= posicionAlt3.y - 2.5f &&
                        posicion.y <= posicionAlt3.y + 2.5f;
            dentroDelRangoBotonAtras = posicion.x >= posicionBotonAtras.x - (BotonAtrasSize.x)/(Botonlocalscale.x*2f) &&
                        posicion.x <= posicionBotonAtras.x + (BotonAtrasSize.x)/(Botonlocalscale.x*2f) &&
                        posicion.y >= posicionBotonAtras.y - (BotonAtrasSize.y)/(Botonlocalscale.y*2f) &&
                        posicion.y <= posicionBotonAtras.y + (BotonAtrasSize.y)/(Botonlocalscale.y*2f);
            }
            

            Debug.Log("alt1: "+dentroDelRangoAlt1+" alt2: "+dentroDelRangoAlt2+" alt3: "+dentroDelRangoAlt3);              
            
            if (dentroDelRangoAlt1==true)
            {
                Debug.Log("ALternativa 1"+dentroDelRangoAlt1);

                minijuego.Alt1_accion();
                //StartCoroutine(EsperarTresSegundos());

                dentroDelRangoAlt1=true;
               // posicion=Vector3.zero;
                
            } 
            else if (dentroDelRangoAlt2==true)
                {
                    Debug.Log("Alternativa2"+dentroDelRangoAlt2);
                    minijuego.Alt2_accion();
                    //StartCoroutine(EsperarTresSegundos());

                    dentroDelRangoAlt2=true;
                    //posicion=Vector3.zero;
                
                } 
            else if (dentroDelRangoAlt3==true)
                    {
                        Debug.Log("Alternativa3"+dentroDelRangoAlt3);
                        minijuego.Alt3_accion();
                        //StartCoroutine(EsperarTresSegundos());
                        dentroDelRangoAlt3=true;
                        //posicion=Vector3.zero;
                    }  
            if (dentroDelRangoBotonAtras==true){
                Debug.Log("Atras. ");
                minijuego.Retroceder();
            }    

            break;
            case "FortuneWheel":
            Vector3 posicionBotonInicio=rueda.Inicio.transform.position;
            Vector3 posicionBotonMenu=rueda.Menu.transform.position;
            Debug.Log("coordenadas: "+posicion);
            Collider2D botonInicioCollider=rueda.Inicio.GetComponent<Collider2D>();
            Collider2D botonMenuCollider=rueda.Menu.GetComponent<Collider2D>();

            Vector2 BotonInicioSize = botonInicioCollider.bounds.size;
            Vector2 BotonMenuSize= botonMenuCollider.bounds.size;
           // Debug.Log("sizeboton: "+BotonInicioSize);
            Vector3 BotonIniciolocalscale= (rueda.Inicio.transform.localScale);
            Vector3 BotonMenulocalscale=(rueda.Menu.transform.localScale);
            //Debug.Log("scale boton: "+BotonIniciolocalscale);
            bool dentroDelRangoTotalrueda = (posicion == Vector3.zero);
            bool dentroDelRangoBotonInicio=false;
            bool dentroDelRangoBotonMenu=false;

            if(dentroDelRangoTotalrueda==false){
                dentroDelRangoBotonInicio = posicion.x >= posicionBotonInicio.x - (BotonInicioSize.x)/(BotonIniciolocalscale.x*2f) &&
                        posicion.x <= posicionBotonInicio.x + (BotonInicioSize.x)/(BotonIniciolocalscale.x*2f) &&
                        posicion.y >= posicionBotonInicio.y - (BotonInicioSize.y)/(BotonIniciolocalscale.y*2f) &&
                        posicion.y <= posicionBotonInicio.y + (BotonInicioSize.y)/(BotonIniciolocalscale.y*2f);
                dentroDelRangoBotonMenu = posicion.x >= posicionBotonMenu.x - (BotonMenuSize.x)/(BotonMenulocalscale.x*2f) &&
                        posicion.x <= posicionBotonMenu.x + (BotonMenuSize.x)/(BotonMenulocalscale.x*2f) &&
                        posicion.y >= posicionBotonMenu.y - (BotonMenuSize.y)/(BotonMenulocalscale.y*2f) &&
                        posicion.y <= posicionBotonMenu.y + (BotonMenuSize.y)/(BotonMenulocalscale.y*2f);

            }

            if (dentroDelRangoBotonInicio==true)
            {
                rueda.IniciarGiro();
            }
            if (dentroDelRangoBotonMenu==true)
            {
                rueda.RegresarMenu();
            }
            

            

            break;
            case "TresEnRaya":

            Debug.Log("coordenadas: "+posicion);

            List<Vector3> posicionesCasillas = new List<Vector3>();
            Vector3 posicionBotonRestart=tresEnRaya.botonRestart.transform.position;
            Vector3 posicionBotonPlayerX=tresEnRaya.botonPlayerX.transform.position;
            Vector3 posicionBotonPlayerO=tresEnRaya.botonPLayerO.transform.position;

            Vector3 posicionBotonMenu1=tresEnRaya.botonMenu.transform.position;

            foreach (GameObject casilla in tresEnRaya.casillas)
            {
                posicionesCasillas.Add(casilla.transform.position);
            }

            List<Collider2D> collidersCasillas = new List<Collider2D>();

            foreach (GameObject casilla in tresEnRaya.casillas)
            {
                Collider2D collider = casilla.GetComponent<Collider2D>();
    
                if (collider != null)
                {
                    collidersCasillas.Add(collider);
                }
            }

            Collider2D colliderBotonRestart=tresEnRaya.botonRestart.GetComponent<Collider2D>();
            Collider2D colliderBotonPlayerX=tresEnRaya.botonPlayerX.GetComponent<Collider2D>();
            Collider2D colliderBotonPlayerO=tresEnRaya.botonPLayerO.GetComponent<Collider2D>();
            Collider2D botonMenuCollider1=tresEnRaya.botonMenu.GetComponent<Collider2D>();

            Vector2 BotonRestartSize = colliderBotonRestart.bounds.size;
            Vector2 BotonPlayerXtSize = colliderBotonPlayerX.bounds.size;
            Vector2 BotonPlayerOSize = colliderBotonPlayerO.bounds.size;
            Vector2 BotonMenuSize1= botonMenuCollider1.bounds.size;
            //Debug.Log("sizeboton: "+BotonAtrasSize);
            Vector3 BotonRestartlocalscale= tresEnRaya.botonRestart.transform.localScale;
            Vector3 BotonPlayerXlocalscale= tresEnRaya.botonPlayerX.transform.localScale;
            Vector3 BotonPlayerOlocalscale= tresEnRaya.botonPLayerO.transform.localScale;
            Vector3 BotonMenulocalscale1=tresEnRaya.botonMenu.transform.localScale;
            //Debug.Log("scale boton: "+Botonlocalscale);

            //bool dentroDelRangoTotalFondo = (posicion == Vector3.zero);
            bool dentroDelRangoCasilla=false;
            bool dentroDelRangoStart=false;
            bool dentroDelRangoPlayerX=false;
            bool dentroDelRangoPlayerO=false;
            bool dentroDelRangoBotonMenu1=false;

            bool dentroDelRango1;
            //bool dentroDelRango2;
            Casilla casillaSeleccionada = null;
            
                foreach (Collider2D colliderCasilla in collidersCasillas)
                {
                Vector3 posicionCasilla = colliderCasilla.transform.position;
                Vector2 casillaSize = colliderCasilla.bounds.size;
                Vector3 casillaLocalScale = colliderCasilla.transform.localScale;

                dentroDelRango1 = posicion.x >= posicionCasilla.x - (casillaSize.x / (casillaLocalScale.x * 2f)) &&
                              posicion.x <= posicionCasilla.x + (casillaSize.x / (casillaLocalScale.x * 2f)) &&
                              posicion.y >= posicionCasilla.y - (casillaSize.y / (casillaLocalScale.y * 2f)) &&
                              posicion.y <= posicionCasilla.y + (casillaSize.y / (casillaLocalScale.y * 2f));

                if (dentroDelRango1)
                    {
                        dentroDelRangoCasilla = true;
                        Debug.Log("Entra1");
                        casillaSeleccionada = colliderCasilla.GetComponentInParent<Casilla>();
                        //casilla.SetSpace();
                        break; // Salir del bucle si se encuentra una casilla dentro del rango
                    }
                }

                dentroDelRangoStart = posicion.x >= posicionBotonRestart.x - (BotonRestartSize.x)/(BotonRestartlocalscale.x*2f) &&
                        posicion.x <= posicionBotonRestart.x + (BotonRestartSize.x)/(BotonRestartlocalscale.x*2f) &&
                        posicion.y >= posicionBotonRestart.y - (BotonRestartSize.y)/(BotonRestartlocalscale.y*2f) &&
                        posicion.y <= posicionBotonRestart.y + (BotonRestartSize.y)/(BotonRestartlocalscale.y*2f);
                dentroDelRangoPlayerX = posicion.x >= posicionBotonPlayerX.x - (BotonPlayerXtSize.x)/(BotonPlayerXlocalscale.x*2f) &&
                        posicion.x <= posicionBotonPlayerX.x + (BotonPlayerXtSize.x)/(BotonPlayerXlocalscale.x*2f) &&
                        posicion.y >= posicionBotonPlayerX.y - (BotonPlayerXtSize.y)/(BotonPlayerXlocalscale.y*2f) &&
                        posicion.y <= posicionBotonPlayerX.y + (BotonPlayerXtSize.y)/(BotonPlayerXlocalscale.y*2f);
                dentroDelRangoPlayerO = posicion.x >= posicionBotonPlayerO.x - (BotonPlayerOSize.x)/(BotonPlayerOlocalscale.x*2f) &&
                        posicion.x <= posicionBotonPlayerO.x + (BotonPlayerOSize.x)/(BotonPlayerOlocalscale.x*2f) &&
                        posicion.y >= posicionBotonPlayerO.y - (BotonPlayerOSize.y)/(BotonPlayerOlocalscale.y*2f) &&
                        posicion.y <= posicionBotonPlayerO.y + (BotonPlayerOSize.y)/(BotonPlayerOlocalscale.y*2f);
                dentroDelRangoBotonMenu1 = posicion.x >= posicionBotonMenu1.x - (BotonMenuSize1.x)/(BotonMenulocalscale1.x*2f) &&
                        posicion.x <= posicionBotonMenu1.x + (BotonMenuSize1.x)/(BotonMenulocalscale1.x*2f) &&
                        posicion.y >= posicionBotonMenu1.y - (BotonMenuSize1.y)/(BotonMenulocalscale1.y*2f) &&
                        posicion.y <= posicionBotonMenu1.y + (BotonMenuSize1.y)/(BotonMenulocalscale1.y*2f);

                
            
        if(dentroDelRangoCasilla==true)
            {
                casillaSeleccionada.SetSpace();
            }
        else if(dentroDelRangoStart==true) 
        {
            tresEnRaya.RestartGame();
        }
        else if(dentroDelRangoPlayerX==true) 
        {
            tresEnRaya.SetStaringSide("X");
        }
        else if(dentroDelRangoPlayerO==true) 
        {
            tresEnRaya.SetStaringSide("O");
        }

        
        break;
        case "Menu":

        Vector3 posicionSiguiente=menu.Siguiente.transform.position;
        Vector3 posicionAtras=menu.Atras.transform.position;
        Vector3 posicionIcono=menu.Icono.transform.position;
        //Debug.Log("posicion icono: "+posicionIcono);
            // Obtener el tamaño del collider del zombie
        Collider2D iconoCollider = menu.Icono.GetComponent<Collider2D>();
        Vector2 iconoSize = iconoCollider.bounds.size;
        //Debug.Log("posicion size: "+iconoSize);
        Collider2D SiguienteCollider = menu.Atras.GetComponent<Collider2D>();
        Vector2 SiguienteSize = SiguienteCollider.bounds.size;
        Collider2D AtrasCollider = menu.Siguiente.GetComponent<Collider2D>();
        Vector2 AtrasSize = AtrasCollider.bounds.size;
            

           // Debug.Log("ZOMBIESIZE: "+zombieSize);
        
        Debug.Log("coordenadas: "+posicion);
            //Debug.Log("posicion zombie activo: "+posicionZombie);
        Vector3 iconoLocalscale= (menu.Icono.transform.localScale);
        Vector3 SiguienteLocalscale= (menu.Siguiente.transform.localScale);
        Vector3 AtrasLocalscale= (menu.Atras.transform.localScale);

            //Debug.Log("escala: "+localscale);
            // Obtener la posición del centro del zombie
            //Vector3 zombieCenter = zombieCollider.bounds.center;
            //Debug.Log("zobie center: "+zombieCenter);
            bool dentroDelRangoIcono = false;
            bool dentroDelRangoSiguiente = false;
            bool dentroDelRangoAtras = false;

        dentroDelRangoIcono = posicion.x >= 7.03f &&
                        posicion.x <= 21.28f &&
                        posicion.y >= -22.49f &&
                        posicion.y <= -12.95f;
            /*bool x2= posicion.x >= posicionIcono.x - 7.22f;
            bool x3= posicion.x <= posicionIcono.x + 7.22f;
            bool y2=posicion.y >= posicionIcono.y -4.85f;
            bool y3=posicion.y <= posicionIcono.y + 0.69f;
        Debug.Log("X-= "+x2);
        Debug.Log("X+= "+x3);
        Debug.Log("Y-= "+y2);
        Debug.Log("Y+= "+y3);*/
        dentroDelRangoSiguiente = posicion.x >= posicionSiguiente.x - (SiguienteSize.x)/(SiguienteLocalscale.x*2f) &&
                        posicion.x <= posicionSiguiente.x + (SiguienteSize.x)/(SiguienteLocalscale.x*2f) &&
                        posicion.y >= posicionSiguiente.y - (SiguienteSize.y)/(SiguienteLocalscale.y*2f) &&
                        posicion.y <= posicionSiguiente.y + (SiguienteSize.y)/(SiguienteLocalscale.y*2f);
        dentroDelRangoAtras = posicion.x >= posicionAtras.x - (AtrasSize.x)/(AtrasLocalscale.x*2f) &&
                        posicion.x <= posicionAtras.x + (AtrasSize.x)/(AtrasLocalscale.x*2f) &&
                        posicion.y >= posicionAtras.y - (AtrasSize.y)/(AtrasLocalscale.y*2f) &&
                        posicion.y <= posicionAtras.y + (AtrasSize.y)/(AtrasLocalscale.y*2f);
        
        if (dentroDelRangoSiguiente==true)
        {
            //if (canExecute){
                menu.SiguienteImagen();
                //serialManager.puerto.DiscardInBuffer();
                // Desactiva la variable de control y establece el temporizador
                //canExecute = false;
                //Invoke("ResetCooldown", cooldownTime);
           // }
            
        }
        else if (dentroDelRangoAtras==true)
        {
            //if (canExecute)
            //{
                menu.ImagenAnterior();
            //}
            
        }
        else if (dentroDelRangoIcono==true)
        {
            
            menu.CargarEscenaCorrespondiente();
            //Debug.Log("dentro");
            
            
        }
        
        break;
        }
        

        
    }

    private void ResetCooldown()
    {
        canExecute = true;
    }

    IEnumerator WaitAndContinueListening()
    {
        yield return new WaitForSeconds(1f); // Esperar 5 segundos
        shouldWait = false; // Desactivar la espera
    }

    bool CoordenadaEsValida(string coordenada)
    {
        CoordenadasPantalla coordenadasPantalla = GetComponent<CoordenadasPantalla>();

        // Usa la función ObtenerPosicion para validar la coordenada
        Vector3 posicion = coordenadasPantalla.ObtenerPosicion(coordenada);

        // Si la posición obtenida es igual a Vector3.zero, la coordenada no es válida
        return posicion != Vector3.zero;
    }

    /*private IEnumerator EsperarTresSegundos()
    {
        yield return new WaitForSeconds(3f);

        // Continuar con la lógica después de esperar 3 segundos
    }*/

    
}

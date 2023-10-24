using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting; // Agrega este using para usar OrderByDescending


public class SerialManager : MonoBehaviour
{
    private static SerialManager instance;
    private bool abort;
    private static   SerialPort puerto;
    private Thread serialThread;
    private SynchronizationContext mainThread;
    private char incomingChar;
    private string incomingString;

    public static bool IsWaiting { get; private set; } = false;
    private float waitingTime = 5f; // Tiempo de espera en segundos
    private float waitStartTime;
    private bool canExecute = true; // Variable de control
    private float cooldownTime = 1f; // Tiempo de enfriamiento en segundos

    public delegate void SerialEvent(string incomingString);
    public static event SerialEvent WhenReceiveDataCall;
    private float lastPercentReceivedTime;
    private string datos;
    public List<string> receivedData = new List<string>();
    // Declara una variable para mantener la coordenada anterior
    private float processingDelay = 0.1f;
    private bool hasProcceded=false;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Cierra el puerto serial cuando se descargue una escena
        if (puerto != null && puerto.IsOpen)
        {
            puerto.Close();
            Debug.Log("Puerto serial cerrado al cambiar de escena");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verificar si el puerto serial no está abierto antes de intentar abrirlo
        if (puerto == null || !puerto.IsOpen)
        {
            puerto = new SerialPort("COM3", 115200);
            puerto.Open();
            Debug.Log("Puerto serial abierto");
            puerto.DiscardInBuffer();
            puerto.DiscardOutBuffer();
            puerto.ReadTimeout = 300;
            mainThread = SynchronizationContext.Current;
            //waitStartTime= Time.time;
            serialThread = new Thread(Receive);
            if (puerto.IsOpen)
            {
                serialThread.Start();
            }
        }
    }
    
    void Receive(){
        
        while (true){
            if (abort){
                serialThread.Abort();
                break;
            }
            try {
                incomingChar = (char)puerto.ReadChar();
                //Debug.Log("IncomingChar: "+incomingChar);
            }
            catch (Exception  ){ }
            
            if(!incomingChar.Equals('%')){
                incomingString += incomingChar;
                
                //Debug.Log("Incoming String: "+incomingString);
                
            } 
            else 
            {
                
                
                    mainThread.Send((object state) =>
                    {
                         
                        if (WhenReceiveDataCall != null)
                    {
                    WhenReceiveDataCall(incomingString);
                    
                        if (incomingString !="" && !incomingString.Contains('Z') && !incomingString.Contains('X'))
                        {
                            receivedData.Add(incomingString);
                            //Debug.Log("elementos: "+receivedData.Count);
                            Debug.Log("elementos: " +receivedData.Count);
                            waitStartTime = Time.time;
                            
                        }
                        
                        
                        
                    // Agregar la cadena de caracteres sin el '%' a la lista
                    // Si se ha acumulado suficientes elementos y ha pasado el tiempo necesario, procesa los datos
                    //if (receivedData.Count > 1  && Time.time - waitStartTime >= processingDelay) 
                    //{
                    //Debug.Log("ENTRA");
                    //Debug.Log("time: "+Time.time);
                    //Debug.Log("waitStartTime: "+waitStartTime);
                       // if (receivedData.Count < 500  )
                        //{
                        // Si la lista tiene elementos, enviar el primero
                       /* if (receivedData.Count > 10)
                        {
                            WhenReceiveDataCall(receivedData[1]);
                            //receivedData.RemoveAt(0); // Eliminar el elemento enviado
                            
                            //receivedData.Clear();
                        //}*/
                        /*if (receivedData.Count >5){

                        // Contar la frecuencia de cada elemento en la lista
                            Dictionary<string, int> frequency = new Dictionary<string, int>();
                            foreach (string data in receivedData)
                            {
                                if (frequency.ContainsKey(data))
                                frequency[data]++;
                                else
                                frequency[data] = 1;
                            }

                            // Encontrar el elemento con la frecuencia más alta
                            string mostRepeatedData = null;
                            //Debug.Log("mas repetido: "+mostRepeatedData);
                            int maxFrequency = 0;
                            //string mostRepeatedData = receivedData[0];
                            //int maxFrequency = frequency[mostRepeatedData];
                            foreach (var pair in frequency)
                            {
                                if (pair.Value > maxFrequency)
                                {
                                    maxFrequency = pair.Value;
                                    mostRepeatedData = pair.Key;
                                }
                            }

                            // Enviar el elemento más repetido
                            WhenReceiveDataCall(mostRepeatedData);
                            hasProcceded=true;
                            Debug.Log("ENVIA: "+mostRepeatedData);
                            receivedData.Clear();
                            waitStartTime= Time.time;
                            //Thread.Sleep(1000);
                        }
                        
                    
                
                    //}
                    
            
                    
                    

                    /*if (receivedData.Count>5 && hasProcceded){
                        receivedData.Clear();
                        hasProcceded=false;
                    }*/
                        
                        
                    }
                    else
                    {
                        receivedData.Clear();
                    }
                    
               // }
            }, null);

                incomingString = "";
                //receivedData.Clear();
                // Si se ha acumulado suficientes elementos y ha pasado el tiempo necesario, establece la hora de espera
            
                
                
            }
        }
    }

    private void OnApplicationQuit() {
        abort=true;
        puerto.DiscardInBuffer();
        puerto.DiscardOutBuffer();
        puerto.Close();
        Debug.Log("se cerró el puerto al salir");
    }

    /*public static void SendInfo (string infoToSend){
        puerto.Write(infoToSend);
    }*/
    void LimpiarBuffer()
    {
        puerto.DiscardInBuffer();
        puerto.DiscardOutBuffer();
        Debug.Log("Se limpio el buffer");
    }

    void Update() {
    if (IsWaiting && Time.time - waitStartTime >= waitingTime) {
        IsWaiting = false;
    }

    }
    // Función para encontrar el elemento más repetido en el diccionario
    private string FindMostRepeatedData(Dictionary<string, int> frequency) {
        string mostRepeatedData = null;
        int maxFrequency = 0;

        foreach (var pair in frequency) {
            if (pair.Value > maxFrequency) {
                maxFrequency = pair.Value;
                mostRepeatedData = pair.Key;
            }
        }

        return mostRepeatedData;
    }

}
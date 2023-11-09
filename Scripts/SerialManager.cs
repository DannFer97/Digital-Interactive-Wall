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
    private string count;

    public static bool IsWaiting { get; private set; } = false;
    private float waitingTime = 5f; // Tiempo de espera en segundos
    private float waitStartTime;

    public delegate void SerialEvent(string incomingString);
    public static event SerialEvent WhenReceiveDataCall;
    public List<string> receivedData = new List<string>();
    public List<string> lastTenReceived = new List<string>();
    // Declara una variable para mantener la coordenada anterior
    private float processingDelay = 1f;
    private bool hasProcceded = false;
    private string lastSentData = "";
    private int cont=0;
    private int receivedCount = 0;
    private int maxReceivedCount = 10; // Cambia este valor al número deseado
    

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
            puerto = new SerialPort("COM10", 115200);
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
    
    void Receive()
    {
        while (true)
        {
            if (abort)
            {
                serialThread.Abort();
                break;
            }
            try
            {
                incomingChar = (char)puerto.ReadChar();
            }
            catch (Exception)
            {
            }

            if (!incomingChar.Equals('%'))
            {
                incomingString += incomingChar;
            }
            else
            {
                mainThread.Send((object state) =>
                {
                    if (WhenReceiveDataCall != null)
                    {
                        //WhenReceiveDataCall(incomingString);

                        if (incomingString != "" && !incomingString.Contains('Z') && !incomingString.Contains('X'))
                        {
                            receivedData.Add(incomingString);
                            lastTenReceived.Add(incomingString);
                            receivedCount++;

                            if (receivedCount >= maxReceivedCount)
                            {
                                receivedCount = 0; // Reinicia el contador
                                receivedData.Clear();
                                waitStartTime = Time.time;
                                puerto.DiscardInBuffer();

                                // Procesa los últimos 10 elementos
                                if (lastTenReceived.Count > 0)
                                {
                                    Dictionary<string, int> frequency = new Dictionary<string, int>();

                                    // Contar las ocurrencias de cada elemento
                                    foreach (string data in lastTenReceived)
                                    {
                                        if (frequency.ContainsKey(data))
                                            frequency[data]++;
                                        else
                                            frequency[data] = 1;
                                    }

                                    string mostRepeatedData = null;
                                    int maxFrequency = 0;

                                    // Encontrar el elemento más repetido
                                    foreach (var pair in frequency)
                                    {
                                        if (pair.Value > maxFrequency)
                                        {
                                            maxFrequency = pair.Value;
                                            mostRepeatedData = pair.Key;
                                        }
                                        //Debug.Log("most repeated data: "+mostRepeatedData);
                                    }
                                    //Debug.Log("most repeated data: "+mostRepeatedData);
                                    WhenReceiveDataCall(mostRepeatedData);
                                }

                                lastTenReceived.Clear();
                            }
                        }
                        else
                        {
                            receivedData.Clear();
                        }
                    }
                }, null);

                incomingString = "";
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

}
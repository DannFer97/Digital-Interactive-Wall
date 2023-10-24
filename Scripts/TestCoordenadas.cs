using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using System.Threading;

public class TestCoordenadas : MonoBehaviour
{
    public List<string> allCoordinates = new List<string>();
    private int repetitions = 1;
    private int testIterations = 5;
    private Dictionary<string, List<bool>> validationResults = new Dictionary<string, List<bool>>();
    private string filePath;
    private int testCount = 0;
    private int currentCoordinateIndex = 0;
    private string currentCoordinate;
    private int validCount = 0;
    private int invalidCount = -1;
    private bool waitingForValidation = false;
    private int aux=0;
    private bool pasa;

    public string receivedCoordinate;
    public string coor;
    private int numreceive;
    public TextMeshProUGUI Coordenada;
    public TextMeshProUGUI ContadorTest;
    public TextMeshProUGUI Cooordenadarecibida;
    public SerialManager serialmanager; 

    private bool clearReceivedData = false;
    private float clearReceivedDataTimer = 0.5f; // Tiempo en segundos antes de borrar receivedData
    private float lastCoordinateReceivedTime;

    public int COLUMNA;
    public char FILA;

    void Start()
    {
        // Llena la lista de coordenadas desde A1 hasta N26
        for (char row = FILA; row <= FILA; row++)
        {
            for (int col = COLUMNA; col <= COLUMNA; col++)
            {
                if (col<10){
                    allCoordinates.Add(row +"0"+ col.ToString());
                }
                else {
                    allCoordinates.Add(row + col.ToString());
                }
                
            }
        }

        // Define la ruta del archivo TXT
        filePath = "C:/Users/USUARIO/Documents/Data/CoordinateResults"+FILA+COLUMNA+".txt";
        SerialManager.WhenReceiveDataCall += EscucharCoordenadas;
        ContadorTest.text="Mapeos Validados: 0";
        // Inicia la prueba
        StartTest();
    }
    private void OnDestroy()
    {
        SerialManager.WhenReceiveDataCall -= EscucharCoordenadas;
    }

    void StartTest()
    {
        // Si hemos completado todas las repeticiones, terminamos la prueba
        if (testCount == testIterations)
        {
            ShowResults();
            CalculateAccuracy();
            SaveResultsToTXT();
            return;
        }

        // Obtenemos la coordenada actual
        currentCoordinate = allCoordinates[currentCoordinateIndex];
        Debug.Log("Validando coordenada: " + currentCoordinate);
        Coordenada.text="Validando coordenada: "+currentCoordinate.ToString();

        // Reiniciamos los contadores
        aux=0;
        validCount = 0;
        invalidCount = -1;
        numreceive=0;
        waitingForValidation = true;
    }

    void Update()
    {
        // Si se presiona la tecla "S", se considera una coordenada no válida
        if (Input.GetKeyDown(KeyCode.S))
        {
            pasa=true;
            Debug.Log("se pulso s");
            ///waitingForValidation = false;
            serialmanager.receivedData.Clear();
            
        }

        // Verifica si debes borrar receivedData después de un tiempo
    
    }

    void EscucharCoordenadas(string incomingString)
    {
        
        receivedCoordinate = incomingString;
        coor=receivedCoordinate;
        Cooordenadarecibida.text="coodenada recibida: "+coor.ToString();
        
        if (waitingForValidation)
        {
            
            if (receivedCoordinate != "")
            {
                //numreceive++;
                //Debug.Log("numreceive: "+numreceive);
                Debug.Log("recibe: "+receivedCoordinate);
                //if (receivedCoordinate == currentCoordinate)
                  //  {

                    /*validCount++;
                    Debug.Log("VALID COUNT: " + validCount);
                    bool cumple=(receivedCoordinate == currentCoordinate);
                    Debug.Log("cumple: "+cumple);
                    //Debug.Log("recibe: "+receivedCoordinate);
                    Debug.Log("current: "+currentCoordinate);*/
                    StartCoroutine(ValidateCoordinate());
                   // serialmanager.receivedData.Clear();
                   // Activa la limpieza de receivedData
                    
                    

                
               // }
            }
        }
    

    IEnumerator ValidateCoordinate()
    {
        
            //while (numreceive <= repetitions)
            //{
                
                //vuelve:
                yield return null;
            
                
           // }
        
        // Registramos el resultado de validación para esta coordenada
        if (!validationResults.ContainsKey(currentCoordinate))
        {
            validationResults[currentCoordinate] = new List<bool>();
        }
        validationResults[currentCoordinate].Add(coor==currentCoordinate);

        // Pasamos a la siguiente coordenada
        currentCoordinateIndex++;

        if (currentCoordinateIndex >= allCoordinates.Count)
        {
            currentCoordinateIndex = 0;
            testCount++;
            ContadorTest.text="Mapeos Validados: "+testCount.ToString();
            //StartTest();
            
        }
        //serialmanager.receivedData.Clear();
        //Thread.Sleep(1000);

        StartTest();
        //serialmanager.receivedData.Clear();
    }
}


    void CalculateAccuracy()
    {
        Dictionary<string, float> accuracyPercentage = new Dictionary<string, float>();

        foreach (var kvp in validationResults)
        {
            float accuracy = kvp.Value.Count(result => result) / (float)kvp.Value.Count * 100f;
            accuracyPercentage[kvp.Key] = accuracy;
        }

        Debug.Log("Accuracy Calculado:");
        foreach (var kvp in accuracyPercentage)
        {
            Debug.Log("Coordenada: " + kvp.Key + ", Porcentaje de Exactitud: " + kvp.Value + "%");
        }
    }



    void ShowResults()
    {
        Debug.Log("Resultados de Validación:");
        foreach (var kvp in validationResults)
        {
            int validCount = kvp.Value.Count(result => result);
            int invalidCount = kvp.Value.Count - validCount;
            Debug.Log("Coordenada: " + kvp.Key + ", Válidas: " + validCount + ", Inválidas: " + invalidCount);
        }
    }

    void SaveResultsToTXT()
    {
        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine("Resultados de Validación:");
        foreach (var kvp in validationResults)
        {
            int validCount = kvp.Value.Count(result => result);
            int invalidCount = kvp.Value.Count - validCount;
            writer.WriteLine("Coordenada: " + kvp.Key + ", Válidas: " + validCount + ", Inválidas: " + invalidCount);
        }

        writer.WriteLine("Accuracy Calculado:");
        foreach (var kvp in validationResults)
        {
            float accuracy = kvp.Value.Count(result => result) / (float)kvp.Value.Count * 100f;
            writer.WriteLine("Coordenada: " + kvp.Key + ", Porcentaje de Exactitud: " + accuracy + "%");
        }

        writer.Close();

        Debug.Log("Resultados guardados en " + filePath);
    }

}
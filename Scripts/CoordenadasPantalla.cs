using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordenadasPantalla : MonoBehaviour
{
    // Tamaño de la cuadrícula de coordenadas
    private int filas = 15;
    private int columnas = 27;

    private Camera mainCamera;
    private Vector3 bottomLeft;
    private Vector3 topRight;

    // Mapa para asociar letras a filas
    private string letras = "ABCDEFGHIJKLMN";
    void Start() 
    {
        mainCamera = Camera.main;
        bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.farClipPlane));
        //Debug.Log("bottomleft"+bottomLeft);
        topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.farClipPlane));  
        //Debug.Log("topright"+topRight);  
    }

    // Función para obtener la posición en base a la letra y el número
    public Vector3 ObtenerPosicion(string coordenada)
    {
        // Validar la longitud de la coordenada
        //Debug.Log("tamaño: "+coordenada.Length);
        if (coordenada.Length < 2 || coordenada.Length > 3  )
        {
            Debug.LogError("Coordenada inválida1: " + coordenada);
            return Vector3.zero;
        }

        // Obtener la letra y el número de la coordenada
        char letra = coordenada[0];
        int numero;
        
        if (coordenada.Length == 2)
        {
            numero = int.Parse(coordenada[1].ToString());
        }
        else
        {
            numero = int.Parse(coordenada.Substring(1));
        }

        // Validar la letra y el número
        if (!letras.Contains(letra.ToString()) || numero < 1 || numero  > columnas)
        {
            Debug.LogError("Coordenada inválida2: " + coordenada);
            return Vector3.zero;
        }

        /*// Calcular la posición en base a la letra y el número
        float x = (numero - 0.5f) * (Screen.width / filas);
        float y = (letras.IndexOf(letra) + 0.5f) * (Screen.height / columnas);
        Debug.Log("ancho: "+Screen.width);
        Debug.Log("largo: "+Screen.height);

        // Convertir la posición de pantalla a posición en el mundo relativa a la cámara principal
        Camera mainCamera = Camera.main;
        Vector3 screenPosition = new Vector3(x, y, mainCamera.nearClipPlane);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;*/
        // Calcular la posición en base a la letra y el número
        float cellWidth = (topRight.x - bottomLeft.x) / columnas;
        float cellHeight = (topRight.y - bottomLeft.y) / filas;

        //float x = bottomLeft.x + ((numero - 1) * cellWidth) + (cellWidth / 2f);
        //float y = bottomLeft.y + (letras.IndexOf(letra) * cellHeight) + (cellHeight / 2f);
        float x = bottomLeft.x + ((numero - 1) * cellWidth) + (cellWidth);
        float y = bottomLeft.y + (letras.IndexOf(letra) * cellHeight) + (cellHeight);

        return new Vector3(x, y, 15.67f);

        

        
    }
}


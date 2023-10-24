using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CapturarImagen : MonoBehaviour
{
    public Camera camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaptureAndSaveImage();
        }
    }

    private void CaptureAndSaveImage()
    {
        // Crear una nueva textura con las dimensiones de la pantalla
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Renderizar la vista de la cámara en un RenderTexture
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();

        // Leer los píxeles del RenderTexture y aplicarlos a la textura
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Limpiar y liberar recursos
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convertir la imagen en formato PNG y guardarla como archivo
        byte[] pngData = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes("Captura.png", pngData);

        Debug.Log("Imagen capturada y guardada como Captura.png");
    }
}


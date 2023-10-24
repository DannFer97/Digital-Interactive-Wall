using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Atras : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Retroceder()
    {
        SceneManager.LoadScene("FortuneWheel");
        // Reanudar el tiempo en el juego
        Time.timeScale = 1f;
    }
}

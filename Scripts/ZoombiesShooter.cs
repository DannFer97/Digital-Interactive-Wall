using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ZoombiesShooter : MonoBehaviour
{
    public List<GameObject> spawnPoints; // Lista de puntos de aparición de los zombies
    public GameObject zombiePrefab; // Prefab del zombie
    public GameObject activeZombie; // Zombie activo actualmente
    public GameObject botonAtras;
    public AudioClip zombieGameBackgroundMusic;
    //public GameObject SerialMgr;
    public SerialManager puerto;

    public ZoombiesShooter zombieEliminate;
    
    public Animator animatorZombieDeath, animatorFin;
    public AudioSource sonidoZombie;
    public AudioClip zombiegruñido, zombieEliminado, felicidades;
    public TextMeshProUGUI scoreText;
    int score;
    public float duracionEspera = 1f; // Tiempo de espera en segundos
    public GameObject felicitaciones,retroceder;
    private bool canExecute = true; // Variable de control
    public float cooldownTime = 0.5f; // Tiempo de enfriamiento en segundos
    private bool spawn=false;
    //public ControladorZombies controladorZombies; // Referencia al script ControladorZombies
    


    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllSpawnPoints(); // Desactivar todos los puntos de aparición al inicio
        SpawnRandomZombie(); // Activar un zombie aleatorio al inicio
        scoreText.text=score.ToString();
        felicitaciones.SetActive(false);
        retroceder.SetActive(false);
        BackgroundMusic.Instance.PlayBackgroundMusic(zombieGameBackgroundMusic);
        Time.timeScale = 1f;
        //AudioMgr.SetActive(true);
        //Debug.Log("activo audiomanager");
        //SerialMgr.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        //animatorZombieDeath = activeZombie.GetComponent<Animator>();
        if (Input.GetKeyDown(KeyCode.S))
        {
            EliminateZombie();
        }
    }

    // Desactivar todos los puntos de aparición
    private void DeactivateAllSpawnPoints()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.SetActive(false);
        }
    }

    // Desactivar el zombie activo actual
    private void DesactivateActiveZombie()
    {
        if (activeZombie != null)
        {
            activeZombie.SetActive(false);
        }
    }

    // Activar un zombie aleatorio en un punto de aparición
    private void SpawnRandomZombie()
    {
        if (canExecute)
        {
            // Desactiva la variable de control y establece el temporizador
            canExecute = false;
            Invoke("ResetCooldown", cooldownTime);
            int randomIndex = Random.Range(0, spawnPoints.Count);
            GameObject randomSpawnPoint = spawnPoints[randomIndex];

            if (activeZombie != null)
            {
                activeZombie.SetActive(false);
            }

            randomSpawnPoint.SetActive(true);
            activeZombie = randomSpawnPoint;
            Collider2D zombieCollider = activeZombie.GetComponent<Collider2D>();
            zombieCollider.enabled=true;

        // Establecer el estado del zombie
            Animator animatorZombieDeath = activeZombie.GetComponent<Animator>();
        //animatorZombieDeath.SetBool("IsStanding", isStanding);
        //animatorZombieDeath.SetBool("IsDead", false);
        // Agregar esta línea para reiniciar el parámetro "IsDead"
        //Debug.Log("Is Deaad"+animatorZombieDeath.GetBool("IsDead") );
            sonidoZombie.clip=zombiegruñido;
            sonidoZombie.Play();
        // Agregar el zombie activo a la lista de zombies activos en el ControladorZombies
        //controladorZombies.AgregarZombieActivo(activeZombie);

        }
        else{

        }
            

    }

    public void EliminateZombie()
    {
        if (activeZombie != null)
    {
        if (canExecute)
        {
            Collider2D zombieCollider = activeZombie.GetComponent<Collider2D>();

        // Verificar si el collider del zombie está habilitado
            if (zombieCollider.enabled)
            {
                animatorZombieDeath = activeZombie.GetComponent<Animator>();
                sonidoZombie.clip = zombieEliminado;
                sonidoZombie.Play();
            //animatorZombieDeath.SetBool("IsDead", true);
                animatorZombieDeath.Play("death_01");
                Score();

            // Desactivar el collider del zombie
                zombieCollider.enabled = false;
                canExecute = false;
                Invoke("ResetCooldown", cooldownTime);
                StartCoroutine(DeactivateZombieAfterDelay());
                
        }
        }
        
    }
    }

    private IEnumerator DeactivateZombieAfterDelay()
    {
        // Esperar un breve tiempo para permitir que la animación de muerte se reproduzca
        yield return new WaitForSeconds(animatorZombieDeath.GetCurrentAnimatorStateInfo(0).length);
        DesactivateActiveZombie();
        activeZombie = null;
        //if (!spawn)
        //{
            SpawnRandomZombie();
        //}
        
    }
    private void Score()
    {
        if (score == 5)
        {
            scoreText.text = 10f.ToString();
            felicitaciones.SetActive(true);
            animatorFin = felicitaciones.GetComponent<Animator>();
            //animatorFin.Play("zombieFelicitaciones");
            felicitaciones.SetActive(true);
            sonidoZombie.clip = felicidades;
            sonidoZombie.Play();
            score = 0;
            spawn=true;
            StartCoroutine(EsperarAnimacion());
            retroceder.SetActive(true);
            
        }
        else
        {
            //Debug.Log("puntaje: " + score);
            score += 1;
            scoreText.text = score.ToString();
        }
    }

    private IEnumerator EsperarAnimacion()
    {
        // Obtener el estado de la animación actual
        AnimatorStateInfo stateInfo = animatorFin.GetCurrentAnimatorStateInfo(0);
        // Calcular el tiempo de espera restando 0.5 segundos a la duración de la animación
        float tiempoEspera = stateInfo.length;

        // Esperar la duración de la animación
        yield return new WaitForSeconds(tiempoEspera);
        DesactivateActiveZombie();
        // Pausar el tiempo en el juego
        Time.timeScale = 0f;
    }

    public void RetrocederMenu()

    {
        
        //AudioMgr.SetActive(false);
        //Debug.Log("desactivo audiomanager");
        //SerialMgr.SetActive(false);
        //puerto.CerrarPuerto();

        SceneManager.LoadScene("Menu");
        
    }

// Función para restablecer la variable de control
    private void ResetCooldown()
    {
        canExecute = true;
        Debug.Log("Espera "+cooldownTime+" segundos");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

[System.Serializable]
public class player 
{
    public Image panel;
    public TextMeshProUGUI text;   
    public Button boton;  
}
    [System.Serializable]
public class playerColor 
{
    public Color panelColor;
    public Color textColor;     
}

public class ControladorTresEnRaya : MonoBehaviour
{
    public TextMeshProUGUI [] listaCasilla;
    private string playerSide;
    
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private int moveCount;

    public GameObject botonRestart;
    public GameObject botonMenu;

    public player playerX;
    public player playerO;
    public playerColor activePlayerColor;
    public playerColor inactivePlayercolor;

    public GameObject startInfo;

    public AudioSource sonidoControlador;
    public AudioClip seleccionControlador, sonidoVictoria, sonidoEmpate;

    public List<GameObject> casillas;
    public List<GameObject> botones;

    
    public GameObject botonPlayerX;
    public GameObject botonPLayerO;

    public bool side;




    void Awake() 
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceButtons();
        //playerSide="X";
        moveCount=0;
        botonRestart.SetActive(false);
        botonMenu.SetActive(false);
        //startInfo.SetActive(true);
        //SetPlayersColors(playerX,playerO);
        side=true;
    }

    void SetGameControllerReferenceButtons(){

            for (int i=0; i <listaCasilla.Length; i++)
            {
                listaCasilla[i].GetComponentInParent<Casilla>().SetGameControllerReference(this);
            }
        
    }

    public void SetStaringSide(string staringSide)
    {
        if (side==true)
        {
            playerSide= staringSide;
            if (playerSide=="X")
            {   
                sonidoControlador.clip=seleccionControlador;
                sonidoControlador.Play();

                SetPlayersColors(playerX,playerO);
                side =false;
                
            }
            else
            {
                SetPlayersColors(playerO,playerX);
                sonidoControlador.clip=seleccionControlador;
                sonidoControlador.Play();
                side =false;
            }
            StartGame();
        }
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayersButton(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn ()
    {
        moveCount++;
        if (listaCasilla[0].text==playerSide && listaCasilla[1].text==playerSide && listaCasilla[2].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[3].text==playerSide && listaCasilla[4].text==playerSide && listaCasilla[5].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[6].text==playerSide && listaCasilla[7].text==playerSide && listaCasilla[8].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[0].text==playerSide && listaCasilla[3].text==playerSide && listaCasilla[6].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[1].text==playerSide && listaCasilla[4].text==playerSide && listaCasilla[7].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[2].text==playerSide && listaCasilla[5].text==playerSide && listaCasilla[8].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[2].text==playerSide && listaCasilla[4].text==playerSide && listaCasilla[6].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (listaCasilla[0].text==playerSide && listaCasilla[4].text==playerSide && listaCasilla[8].text==playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount>=9)
        {
            
            GameOver("Empate");
        }
        else 
        {
            ChangeSides();
        }
        
    }

    void SetPlayersColors(player newPlayer, player Oldplayer)
    {
            newPlayer.panel.color=activePlayerColor.panelColor;
            newPlayer.text.color=activePlayerColor.textColor;
            Oldplayer.panel.color=inactivePlayercolor.panelColor;
            Oldplayer.text.color=inactivePlayercolor.textColor;
        
    }

    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false); 
        if (winningPlayer=="Empate")
        {
            SetGameOverText("Empate! ");
            SetPlayersColorsInactive();
            sonidoControlador.clip=sonidoEmpate;
            sonidoControlador.Play();
            
        } 
        else
        {
            SetGameOverText("Ganó: "+ playerSide);
            sonidoControlador.clip=sonidoVictoria;
            sonidoControlador.Play();
            
        }
        
        botonRestart.SetActive(true);
        botonMenu.SetActive(true);
    }

    void ChangeSides()
    {
        playerSide =(playerSide=="X")? "O":"X";
        if (playerSide=="X")
        {
            SetPlayersColors(playerX,playerO);
        }
        else
        {
            SetPlayersColors(playerO,playerX);
        }
    }

    void SetGameOverText(string value)
    {

            gameOverPanel.SetActive(true);
            gameOverText.text = value;
        
    }

    public void RestartGame()
    {
            moveCount=0;
            gameOverPanel.SetActive(false);
            botonRestart.SetActive(false);
            SetPlayersButton(true);
            SetPlayersColorsInactive();
            startInfo.SetActive(true);
            sonidoControlador.clip=seleccionControlador;
            sonidoControlador.Play();

            botonMenu.SetActive(false);
            side=true;

            for (int i=0; i <listaCasilla.Length;i++ )
            {
                listaCasilla[i].text="";
                //Debug.Log("hols");
            } 
            //SetPlayersColors(playerX,playerO);
        
    }
    void SetBoardInteractable(bool toogle)
    {
            for (int i=0; i <listaCasilla.Length;i++ )
            {
                listaCasilla[i].GetComponentInParent<Button>().interactable=toogle;
                
            }
        
    }

    void SetPlayersButton (bool toogle)
    {
            playerX.boton.interactable=toogle;
            playerO.boton.interactable=toogle;          

        
    }

    void SetPlayersColorsInactive()
    {
            playerX.panel.color=inactivePlayercolor.panelColor;
            playerX.text.color=inactivePlayercolor.textColor;
            playerO.panel.color=inactivePlayercolor.panelColor;
            playerO.text.color=inactivePlayercolor.textColor;
            

        
    }
    public void RetrocederMenu()

    {
        
        //AudioMgr.SetActive(false);
        //Debug.Log("desactivo audiomanager");
        //SerialMgr.SetActive(false);
        //puerto.CerrarPuerto();

        SceneManager.LoadScene("Menu");
        
    }


    

}

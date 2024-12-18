using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;//tener una instancia estatica para poder tener acceso a la clase desde cualquier script
    //guardar los prefabricados en la escena para su intanciacion desde codigo 
    public GameObject wallPrefab,floorPrefab,teleportPrefab,heroPrefab;//... prefabricados principales
    public GameObject doorPrefab,keyPrefab;//...prefabricados especiales
    public GameObject trapPrefab; // prefabricado principal de las trampas
    public GameObject sueloAuxForMinho; // prefabricado auxiliar de suelo para cuando Minho destruya la tierra
    public TextMeshProUGUI player1NameT,player1NameTs; //nombre del jugador 1, texto y sombra
    public TextMeshProUGUI player2NameT,player2NameTs; //nombre del jugador 2, texto y sombra
    public TextMeshProUGUI player1Energy,player1Energys; //energia del jugador 1, texto y sombra
    public TextMeshProUGUI player2Energy,player2Energys; //energia del jugador 1, texto y sombra
    public TextMeshProUGUI player1Money,player1Moneys; //dinero acumulado del jugador 1 y su sombra
    public TextMeshProUGUI player2Money,player2Moneys; //dinero acumulado del jugador 2 y su sombra
    public static Sprite clikedObjectFija; // imagen de objeto clickeado fija para cuando se pase de turno
    public static string player1Name; //nombre del player 1 a montar en la escena 
    public static string player2Name; //nombre del player 2 a montar en la escena 
    public static List<string> player1Heros; //lista de heroes del player 1 
    public static List<string> player2Heros; //lista de heores del player 2
    public List<Trap> traps; //guardar los scriptables para su puesta en escena(trampas)
    public List<Hero> heros; //guardar los scriptables para su puesta en escena(heroes)
    public GameObject maze; //guardar el objeto padre de todos los objetos(Padre de la matriz)
    public UnityEngine.UI.Image currentPlayer1Image; //para mayor legibilidad a la hora de saber a quien le toca jugar 
    public UnityEngine.UI.Image currentPlayer2Image; // ...
    // public UnityEngine.UI.Image auxImageForDestruction; // imagen auxiliar 
    public Button applyEffectPlayer1; //boton de aplicar el efecto del jugador 1
    public Button applyEffectPlayer2; //boton de aplicar el efecto del jugador 2
    public GameObject currentObjectClickedForMinhoEffect;//guardar el objeto clickeado para el efecto de minho
    public GameObject clickedHero; //objeto clickeado en la escena por si se activa el efecto
    public bool currentPlayer;//valor booleano para representar los juadore(false para player 1) y (true para player 2)
    public List<GameObject> herosPlayer1; //rellenar una vez instanciados los heroes en la escena para el sistema de turnos
    public List<GameObject> herosPlayer2; //rellenar una vez instanciados los heroes en la escena para el sistema de turnos
    public AudioSource colectedSound;//guardar el audio source de objeto coleccionado para cuando se coleccione algo 
    public AudioSource itemPickedup; //guardar el audio source de objeto coleccionado para las llaves
    public static List<int> keysColectedPlayer1;//guardar la cantidad de llaver que tenga el jugador 1
    public static List<int> keysColectedPlayer2;//guardar la cantidad de llaver que tenga el jugador 2
    public static bool haveHability; //booleano para verificar si se puede activar la habilidad de un lider o si esta muy drogado producto al veneno
    public static int tommyenfriando; //entero para controlar el tiempo que lleva enfriandose el heroe
    public static int gallyEnfriando; // ...
    public static int terezaEnfriando; // ... 
    public static int sartenEnfriando; //...
    public static int minhoEnfriando; //...
    public static int newtEnfriando; //...
    public static int winConditionForPLayers; //condicion de vistoria para los jugadores 
    public static int counterOfRounds; //entero para llevar la cantidad de turnos que han jugado los jugadores para instanciar trampas y comidas en un momento determinado
    private static int numberOfRounds;//entero para guardar la cantidad de turnos que se jugaran hasta la nueva instanciacion de trampas e items
    
    //ejecutar antes de cualquier frame en el juego 
    private void Awake()
    {
        // Verificar si ya existe una instancia
        if (instancia == null)
        {
            instancia = this; // Asignar la instancia
            DontDestroyOnLoad(gameObject); //no destruir en nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Destruir el duplicado
        }
        herosPlayer1 = new List<GameObject>();//inicializar las listas para evitar errores de referencia
        herosPlayer2 = new List<GameObject>();//...
    }
   
    void Start()  //ejecutar en el inicio de la escena 
    {
        winConditionForPLayers = player1Heros.Count * player1Heros.Count * 2;//inicializar la condicion de victoria
        numberOfRounds = player1Heros.Count * 5;//inicializar la variable de actulaizacion de tablero acrode la cantidad de hereos que se tengan
        clikedObjectFija = null; //inicializar la imagen de objeto clickeado
        counterOfRounds = 0; //inicializar la cantidad de turnos jugados en el pimer momento del juego
        currentPlayer = false; //inicia el primer jugador
        currentObjectClickedForMinhoEffect = null;
        MazeGenerator.Starting();//inicializar el laberinto una vez se cargue la escena
        player1NameT.text = player1Name;//llevar el nombre del player 1 a la escena
        player1NameTs.text = player1Name;//..sombra
        player2NameT.text = player2Name;//llevar el nombre del player 2 a la escena 
        player2NameTs.text = player2Name;//..sombra
        keysColectedPlayer1 = new List<int>(); //inicializar las listas de llaves colectadas
        keysColectedPlayer2 = new List<int>(); // ...

        MazeGenerator.GenerateHeros();//inicializar los heroes correspondientes a cada jugador 
        MazeGenerator.GenerateTeleports(0,1,false);//inicializar los teletransportadores al incio y final de laberinto 
        MazeGenerator.GenerateTeleports(16,17,true);//inicializar los teletransportadores al incio y final de laberinto 
        ObtainHeros(); //guardar los heroes en sus listas correspondientes para el sistema de turnos 
        PrepareGame(); //prepar el laberinto para el jugador 1
        MazeGenerator.PrepareMoney(6);
        MazeGenerator.PrepareDoorsAndKeys(6);

        MazeGenerator.PrepareTraps(herosPlayer1.Count*12,true); //instanciar las trampas de manera random en el laberinto 
        // DeadMove.GenerateZombie();/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //iniciar los valores de enfriamiento de los respectivos heroes
        gallyEnfriando = 0;
        tommyenfriando = 0;
        newtEnfriando = 0;
        terezaEnfriando = 0;
        minhoEnfriando = 0;
        sartenEnfriando = 0;
    }
    private void ObtainHeros() //rellenar los objetos instanciados en la escena en el primer momento
    {
        for (int i = 1; i < 18 ; i++) //iterar por la fila 1
        {
            if(GameManager.instancia.maze.transform.GetChild(1).GetChild(i).childCount > 1) //obtener los objetos donde se instanciaron los clones
            {
                //agregar los objetos a la respectiva lista
                herosPlayer1.Add(GameManager.instancia.maze.transform.GetChild(1).GetChild(i).GetChild(1).gameObject);
            }
        }
        for (int i = 1; i < 18 ; i++) //iterar por la fila 15
        {
            if(GameManager.instancia.maze.transform.GetChild(15).GetChild(i).childCount > 1) //obtener los objetos donde se instanciaron los clones
            {
                //agregar los objetos a la respectiva lista
                herosPlayer2.Add(GameManager.instancia.maze.transform.GetChild(15).GetChild(i).GetChild(1).gameObject);
            }
        }
    }
    public void PrepareGame() //preparar el sistema de turnos al inicio del juego 
    {
        Effects.RestTime();//restar el tiempo de enfriamiento de las habilidades de los heroes 
        haveHability = true;//tiene la habilidad siempre que se pasa de turno
        NPCMove.Newt = false;
        NPCMove.Gally = false;
        counterOfRounds += 1;
        this.clickedHero = null;

        if(!currentPlayer) //el caso de que le toca al jugador 1
        {
            for (int i = 0; i < herosPlayer2.Count ; i++) //desactivar la propiedad NPCMove de los objetos del jugador 2
            {
                herosPlayer2[i].GetComponent<NPCMove>().enabled = false;
            }
            for (int i = 0; i < herosPlayer1.Count ; i++) //activar la propiedad NPCMove de los objetos del jugador 1
            {
                herosPlayer1[i].GetComponent<NPCMove>().enabled = true;
            }
            currentPlayer1Image.enabled = true; //activar el indicador de luz verde del jugador 1
            applyEffectPlayer1.gameObject.SetActive(true);//activar el boton de aplicar efecto del jugador 1
            currentPlayer2Image.enabled = false;//desactivar el indicador de luz verde del jugador 2
            applyEffectPlayer2.gameObject.SetActive(false);//descativar el boton de aplicar efecto del jugador 2
        }
        else //el caso de que le toca al jugador 2
        {
            for (int i = 0; i < herosPlayer1.Count ; i++) //desactivar la propiedad NPCMove de los objetos del jugador 1
            {
                herosPlayer1[i].GetComponent<NPCMove>().enabled = false;
            }
            for (int i = 0; i < herosPlayer2.Count ; i++) //activar la propiedad NPCMove de los objetos del jugador 2
            {
                herosPlayer2[i].GetComponent<NPCMove>().enabled = true;
            }
            currentPlayer1Image.enabled = false;//desactivar el indicador de luz verde del jugador 1
            applyEffectPlayer1.gameObject.SetActive(false);//desactivar el boton de aplicar efecto del juador 1
            currentPlayer2Image.enabled = true;//activar el indicador de luz verde del jugador 2
            applyEffectPlayer2.gameObject.SetActive(true);//activar el boton de aplicar efecto del jugador 2
        }
        NPCMove.n = 0;//restablecer el valor a cero para que el otro jugador tambien se pueda mover 
        if(NPCMove.clikedObjectImage is null) return; //evitar errores de referencia con la imagen de clicked objet de la escena
        NPCMove.clikedObjectImage.sprite = GameManager.clikedObjectFija; //cambiar la imagen a la imagen por default
    }
    private void Update() //verificar en cada frame la cantidad de turnos jugados 
    {
        if(counterOfRounds == 16) //si se tienen 10 turnos jugados se prepararan 10 nuevos objetos en la escena(trampas e items)
        {
            MazeGenerator.PrepareTraps(10,false); //si se llego a 10 turnos crear 10 trampas e items
            MazeGenerator.PrepareMoney(4); //preparar 4 dolares mas para que haya mas juego
        }
        if(Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(1);
    }
}
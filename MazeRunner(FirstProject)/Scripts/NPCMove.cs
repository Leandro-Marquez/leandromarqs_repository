using UnityEngine;
using UnityEngine.EventSystems;

public class NPCMove : MonoBehaviour , IPointerDownHandler
{
    private Hero currentHero; //guardar el heroe actual
    private int x;//cordenada x del heroe actual
    private int y;//coordenada y del heroe actual
    public static bool seMovio;
    private bool [,] maze; //guardar el laberinto completo de la escena
    private bool [,] posibleMoves; //guardar las posiciones accesibles acorde a cada heroe;
    public void Start()
    {
        maze = new bool[17,19];
        posibleMoves = new bool[17,19];
        currentHero = null;
        seMovio = false;
    }
    public void OnPointerDown(PointerEventData eventData) //cuando se hace click 
    {
        GameObject objetoClickeado = eventData.pointerCurrentRaycast.gameObject; //guardar el objeto clickeado
        Hero clickedHero = objetoClickeado.GetComponent<HeroVisual>().hero; //guardar el componente hero 
        if(clickedHero is not null ) //verificar si el componente hero no es nulo osea que es un heroe
        {
            currentHero = clickedHero;//actualizar el heroe actual con el heroe clickeado
            Debug.Log(currentHero.name);
            UpdatePosition();//actualizar la posicion con la posicion del heroe clickeado
        }
    }
    private void UpdatePosition() //actualizar la posicion del heroe clickeado
    {
        //actualir matriz a partir del laberinto en la escena
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                //verificar que sea una celda con al menos un objeto que no sea hierba o pared 
                if(GameManager.instancia.maze.transform.GetChild(i).GetChild(j).childCount > 1)
                {
                    //en caso de que dicho objeto no tenga el componente hero visual seria una trampa u algo asi  
                    if(GameManager.instancia.maze.transform.GetChild(i).GetChild(j).GetChild(1).GetComponent<HeroVisual>() is null) continue;
                    //en el caso de que lo tenga verificar que tenga el nombre del current hero
                    else if(GameManager.instancia.maze.transform.GetChild(i).GetChild(j).GetChild(1).GetComponent<HeroVisual>().hero.name == currentHero.name)
                    {
                        //actualizar las coordenadas con sus respectivas
                        x = i;
                        y = j;
                        return; //una vez se encontro, por eficiencia retornar
                    }
                }
            }
        }
    }
    private void DetectPressedKeys()//detectar que tecla se presiona 
    {
        if (Input.GetKeyDown(KeyCode.W)) MoveW(); //Detectar si se presiona la tecla W
        if (Input.GetKeyDown(KeyCode.A)) MoveA(); // Detectar si se presiona la tecla A
        if (Input.GetKeyDown(KeyCode.S)) MoveS(); // Detectar si se presiona la tecla S
        if (Input.GetKeyDown(KeyCode.D)) MoveD(); // Detectar si se presiona la tecla D
    }
    private void MoveW() //mover hacia arriba 
    {
        if(currentHero is null) return;//verificar que no se haga nada si no hay heroe alguno seleccionado
        if(!seMovio) //si no ha habido movimiento necesita actualizarce el laberinto e invalidad el componente a los demas heroes 
        {
            UpdatePosibleMoves(); // marcar las celdas accesibles
            InvalidOperationsWithOthers();//invalidar el la habilidad de movimiento a los otros Heroes
            UpdateMatrix();//actualizar el laberinto de la escena en la mascara booleana para tenerlo a nivel de codigo 
            seMovio = true;//si llamo al metodo es que hubo movimiento
        }
        if(currentHero is null) return;//verificar que no se haga nada si no hay heroe alguno seleccionado
        int currentIndex = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).childCount-1;//inidice del heroe actual en la gerarquia
        if(x-1 >= 0 && !maze[x-1,y] && posibleMoves[x-1,y])//si esta en los rangos de la matriz y si se puede mover hacia alli
        {
            GameObject aux = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).GetChild(currentIndex).gameObject;//guardar el heroe para moverlo despues
            aux.transform.SetParent(GameManager.instancia.maze.transform.GetChild(x-1).transform.GetChild(y).transform);//darle su padre correspondiente en la gerarquia 
            aux.transform.localPosition = Vector3.zero;//colocarle lasc coordenadas 0,0,0 para evitar troques
            x-=1;//actualizar la posicion
        }
    }
    private void MoveA()//mover a la izquierda
    {
        if(currentHero is null) return;//verificar que no se haga nada si no hay heroe alguno seleccionado
        if(!seMovio)//si no ha habido movimiento necesita actualizarce el laberinto e invalidad el componente a los demas heroes 
        {
            UpdatePosibleMoves(); // marcar las celdas accesibles 
            InvalidOperationsWithOthers();//invalidar el la habilidad de movimiento a los otros Heroes
            UpdateMatrix();//actualizar el laberinto de la escena en la mascara booleana para tenerlo a nivel de codigo 
            seMovio = true;//si llamo al metodo es que hubo movimiento
        }
        int currentIndex = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).childCount-1;//inidice del heroe actual en la gerarquia
        if(y-1 >= 0 && !maze[x,y-1] && posibleMoves[x,y-1])//si esta en los rangos de la matriz y si se puede mover hacia alli
        {
            GameObject aux = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).GetChild(currentIndex).gameObject;//guardar el heroe para moverlo despues
            aux.transform.SetParent(GameManager.instancia.maze.transform.GetChild(x).transform.GetChild(y-1).transform);//darle su padre correspondiente en la gerarquia 
            aux.transform.localPosition = Vector3.zero;//colocarle lasc coordenadas 0,0,0 para evitar troques
            y-=1;//actualizar la posicion
        }
    }
    private void MoveS()//mover hacia abajo
    {
        if(currentHero is null) return;//verificar que no se haga nada si no hay heroe alguno seleccionado
        if(!seMovio)//si no ha habido movimiento necesita actualizarce el laberinto e invalidad el componente a los demas heroes 
        {
            UpdatePosibleMoves(); // marcar las celdas accesibles 
            InvalidOperationsWithOthers();//invalidar el la habilidad de movimiento a los otros Heroes
            UpdateMatrix();//actualizar el laberinto de la escena en la mascara booleana para tenerlo a nivel de codigo 
            seMovio = true;//si llamo al metodo es que hubo movimiento
        }
        int currentIndex = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).childCount-1;//inidice del heroe actual en la gerarquia
        if(x+1 < 17 && !maze[x+1,y] && posibleMoves[x+1,y]) //si esta en los rangos de la matriz y si se puede mover hacia alli
        {
            GameObject aux = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).GetChild(currentIndex).gameObject;//guardar el heroe para moverlo despues
            aux.transform.SetParent(GameManager.instancia.maze.transform.GetChild(x+1).transform.GetChild(y).transform);//darle su padre correspondiente en la gerarquia 
            aux.transform.localPosition = Vector3.zero;//colocarle lasc coordenadas 0,0,0 para evitar troques
            x+=1;//actualizar la posicion
        }
    }
    private void MoveD() //mover a la derecha
    {
        if(currentHero is null) return; //verificar que no se haga nada si no hay heroe alguno seleccionado
        if(!seMovio) //si no ha habido movimiento necesita actualizarce el laberinto e invalidad el componente a los demas heroes 
        {
            UpdatePosibleMoves(); // marcar las celdas accesibles 
            InvalidOperationsWithOthers();//invalidar el la habilidad de movimiento a los otros Heroes
            UpdateMatrix();//actualizar el laberinto de la escena en la mascara booleana para tenerlo a nivel de codigo 
            seMovio = true;//si llamo al metodo es que hubo movimiento
        }
        int currentIndex = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).childCount-1; //inidice del heroe actual en la gerarquia
        if(y+1 < 19 && !maze[x,y+1] && posibleMoves[x,y+1]) //si esta en los rangos de la matriz y si se puede mover hacia alli
        {
            GameObject aux = GameManager.instancia.maze.transform.GetChild(x).GetChild(y).GetChild(currentIndex).gameObject; //guardar el heroe para moverlo despues
            aux.transform.SetParent(GameManager.instancia.maze.transform.GetChild(x).transform.GetChild(y+1).transform); //darle su padre correspondiente en la gerarquia 
            aux.transform.localPosition = Vector3.zero; //colocarle lasc coordenadas 0,0,0 para evitar troques
            y += 1;//actualizar la posicion
        }
    }
    private void UpdatePosibleMoves() //marcar toda celda accesible para el heroe
    {
        posibleMoves = new bool[17,19]; //reiniciar los valores de la matriz para evitar confusiones
        int xpos = x; //guardar la posicion incial para evitar trabajar con los campos de la clase
        int ypos = y; // ...
        DFS(xpos,ypos,currentHero.speed); //lamar a marcar cada ceda alcanzable por el current hero
    }
    private void DFS(int xpos , int ypos , int moves) //visitar todas las casillas accesibles desde la posicion del heroe
    {
        if(moves == 0) return; //el caso de que no pueda seguir caminando debido a la velocidad
        posibleMoves[xpos,ypos] = true; //marcar la posicion pertinente
        //direcciones    der izq  arr abj
        int [] dfilas = { 0 , 0 , -1 , 1 };
        int [] dcolus = { 1 ,-1 ,  0 , 0 };
        for (int i = 0; i < dfilas.Length ; i++) //iterar por las direcciones
        {
            int nuevafila = xpos + dfilas[i]; 
            int nuevacolumna = ypos + dcolus[i];
            if(nuevafila >= 0 && nuevafila < 17 && nuevacolumna >= 0 && nuevacolumna < 19) //verificar que este en los rangos de la matriz
            {
                if(!maze[nuevafila,nuevacolumna])//verificar que no sea una pared y no se haya visitado
                {
                    posibleMoves[nuevafila,nuevacolumna] = true; //marcar los valores
                    DFS(nuevafila,nuevacolumna, moves - 1);//llamar con la nueva posicion 
                }
            }
        }
    }
    private void UpdateMatrix()//guardar en maze el laberinto de la escena en el frame exacto!!
    {
        //inicializr ambas matrices 
        maze = new bool[17,19];
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                //si tiene un solo hijo o bien es una hierba o es una pared y si el hijo tiene la tarjeta entonces solamente podria ser una pared 
                if(GameManager.instancia.maze.transform.GetChild(i).transform.GetChild(j).transform.GetChild(0).transform.tag == "wall")
                {
                    maze[i,j] = true; //marcar la celda correspondiente en true en el laberinto booleano 
                }
            }
        }
    }
    private void InvalidOperationsWithOthers() //una vez se presione cualquier tecla, osea que te muevas con un heroe se invalidan los movimientos con los demas 
    {
        if(!GameManager.instancia.currentPlayer) // en caso de que sea el jugador 1
        {
            for (int i = 0; i < GameManager.instancia.herosPlayer1.Count ; i++)
            {
                if(GameManager.instancia.herosPlayer1[i].GetComponent<HeroVisual>().hero.name != currentHero.name) // si tiene nombre distinto al current hero se le desactiva el componente
                {
                    GameManager.instancia.herosPlayer1[i].GetComponent<NPCMove>().enabled = false;
                }
            }
        }
        else // en caso de que sea el jugador 2
        {
            for (int i = 0; i < GameManager.instancia.herosPlayer2.Count ; i++)
            {
                if(GameManager.instancia.herosPlayer2[i].GetComponent<HeroVisual>().hero.name != currentHero.name) // si tiene nombre distinto al current hero se le desactiva el componente
                {
                    GameManager.instancia.herosPlayer2[i].GetComponent<NPCMove>().enabled = false;
                }
            }
        }
    }
    public void OnPassButtonPressed()
    {
        if(GameManager.instancia.currentPlayer) GameManager.instancia.currentPlayer = false;
        else GameManager.instancia.currentPlayer = true;
        GameManager.instancia.PrepareGame();
        currentHero = null;
        seMovio = false;
    }
    void Update() //detectar teclas presionadas en cada frame
    {
       DetectPressedKeys();
    }
}
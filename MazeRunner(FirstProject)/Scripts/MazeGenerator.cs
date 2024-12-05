using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class MazeGenerator
{
    public static void Starting()//preparar la escena del juego(laberinto)
    {
        GenerateMaze();//generar el primer laberinto
        maze[0,1] = false;
        maze[16,17] = false;
        while(!IsValid()) //verificar si el laberinto es valido
        {
            maze = new bool[n,m];//volver a inicializar la matriz 
            GenerateMaze();//generar otro laberinto
            maze[0,1] = false;
            maze[16,17] = false;
        }
        PrintMaze();//una vez el laberinto esta listo se imprime en la escena 
    }
    
    public static bool [,] maze; //matriz estatica para trabajar por detraz 
    public static int n = 17; //filas 
    public static int m = 19; //columnas

    // Método para generar el laberinto
    private static void GenerateMaze()
    {
        maze = new bool[n, m]; //inicializar la matriz 
        CreateMaze(1, 1); //crear el laberinto a partir de la posicion 1,1 para dar margen a las paredes
    }

    private static void CreateMaze(int x, int y) //crear el laberinto a nivel de codigo
    {
        maze[x, y] = true; // Marca la celda como visitada

        // Direcciones posibles: derecha, abajo, izquierda, arriba
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(1, 0), // Derecha
            new Vector2Int(0, 1), // Abajo
            new Vector2Int(-1, 0), // Izquierda
            new Vector2Int(0, -1) // Arriba
        };
        // Mezclar direcciones para aleatoriedad
        for (int i = 0; i < directions.Count; i++)
        {
            //utilizar simple swapeo por burbuja 
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        //iterar por todas las direcciones
        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x * 2;
            int newY = y + direction.y * 2;

            if (IsInBounds(newX, newY) && !maze[newX, newY]) //verificar si esta dentro de los rangos de la matriz y no esta visitada 
            {
                // Marca la celda intermedia como visitada
                maze[x + direction.x, y + direction.y] = true;
                CreateMaze(newX, newY);
            }
        }
    }
    public static bool IsValid() //verificar si se tiene un laberinto valido osea uno donde haya salida
    {
        return Lee(maze , 0 , 1); //se retorna el metodo Lee
    }
    public static bool Lee (bool [,] a , int fila, int columna) //algoritmo de lee, eficiente para veificar si el laberinto tiene salida o no 
    {
        int [,] costos = new int [a.GetLength(0),a.GetLength(1)]; //matriz de costos a cada celda
        costos[fila,columna] = 1; //marcar la casilla inicial con costo 1
        //                 der abaj izq arribita
        int [] filas =    { 0 , 1 , 0 , -1 };
        int [] columnas = { 1 , 0 , -1 , 0 };
        bool marlon; //booleano para verificar si se alcanzo una casilla valida 
        do
        {
            marlon = false; //de momento no se ha alcanzado casilla valida alguna 
            for(int x = 0 ; x < a.GetLength(0) ; x++)
            {
                for(int y = 0 ; y < a.GetLength(1) ; y++)
                {
                    if(a[x,y] == true) continue;//si la casilla es un obstaculo continuar iterando
                    if(costos[x,y] == 0) continue; //si no se ha podido llegar a la celda continuar iterando
                    for(int d = 0 ; d < filas.Length ; d++) //iterar por los arrays direccionales
                    {
                        int Factual = x + filas[d]; //nueva fila en la direccion correspondiente
                        int Cactual = y + columnas[d]; //nueva columna en la direccion correspondiente
                        //verificar si esta dentro de los rangos de la matriz, no se haya visitado y no sea un obstaculo 
                        if(IsInBounds(Factual , Cactual) && !a[Factual,Cactual] && costos[Factual,Cactual] == 0)
                        {
                            costos[Factual,Cactual] = costos[x,y] + 1; //marcar el costo de llegar hasta aqui
                            marlon = true; //se llego a una celda valida 
                        }
                    }
                }
            }
        }
        while(marlon); //mientras se llegue a una celda valida en cada iteracion del while, se sigue ejecutando 
        if(costos[16,17] != 0) return true; //el caso de q tenga un costo llegar a dicha casilla es q se pudo llegar 
        return false; // no hay costo en la casilla de salida por tanto no hay salida 
    }

    private static bool IsInBounds(int x, int y) //verificar si esta dentro de los rangos de la matriz 
    {
        return x >= 0 && x < n && y >= 0 && y < m;
    }
    
    private static void PrintMaze() //instancias los objetos en la escena a partir del laberinto guardado en maze
    {
        for (int y = 0; y < n; y++)
        {
            for (int x = 0; x < m; x++)
            {
                if((y == 0 && x == 1 ) || (y == 16 && x == 17)) continue;
                if(!maze[y,x]) //si esta en falso significa que es un obstaculo
                {
                    //guardar en un objeto una instancia del prefab correspondiente guardado en el GameManager 
                    GameObject pared = GameObject.Instantiate(GameManager.instancia.wallPrefab,GameManager.instancia.maze.transform.GetChild(y).GetChild(x).transform);
                    pared.transform.localPosition = new Vector3(0,0,0);//reescribir la posicion del objeto al centro de la escena
                    pared.transform.SetParent(GameManager.instancia.maze.transform.GetChild(y).GetChild(x).transform); //llevarlo a su respectiva posicion
                }
                else //si esta en true es una celda valida
                {
                    //guardar en un objeto una instancia del prefab correspondiente guardado en el GameManager 
                    GameObject pared = GameObject.Instantiate(GameManager.instancia.floorPrefab,GameManager.instancia.maze.transform.GetChild(y).GetChild(x).transform);
                    pared.transform.localPosition = new Vector3(0,0,0); //reescribir la posicion del objeto al centro de la escena 
                    pared.transform.SetParent(GameManager.instancia.maze.transform.GetChild(y).GetChild(x).transform); //llevarlo a su respectiva posicion 
                }
            }
        }
    }
    public static void GenerateTeleports(int x , int y) //generar los teleports para elinicio y el final del laberinto 
    {
        //instanciar el teleport en su respectiva posicion
        GameObject teleport  = GameObject.Instantiate(GameManager.instancia.teleportPrefab,GameManager.instancia.maze.transform.GetChild(x).GetChild(y).transform);
        teleport.transform.localPosition = new Vector3(0,0,0);//reescribir las nuevas coordenadas del teleport
        teleport.transform.SetParent(GameManager.instancia.maze.transform.GetChild(x).GetChild(y).transform);//darle el padre correspondiente al teleport coprrespondiente
    }
    public static void GenerateHeros() // Genera los héroes correspondientes a cada uno en sus respectivas casillas 
    {
        if(GameManager.player1Heros.Count == 0 || GameManager.player2Heros.Count == 0) return; //verificar que no se haya seleccionado heroe alguno para evitar errores de referencias 
        
        List<(int x, int y)> position1 = GetPositions(GameManager.player1Heros.Count, 1); // Posiciones de colocación de los héroes del jugador 1
        List<(int x, int y)> position2 = GetPositions(GameManager.player2Heros.Count, 2); // Posiciones de colocación de los héroes del jugador 2

        for (int i = 0; i < position1.Count ; i++) //iterar por las posiciones de colocacion 
        {
            //instanciar el heroe en su respectiva posicion 
            GameObject game = GameObject.Instantiate(GameManager.instancia.heroPrefab, GameManager.instancia.maze.transform.GetChild(position1[i].x).GetChild(position1[i].y).transform);
            HeroVisual Scriptable = game.GetComponent<HeroVisual>();//obtener el componente visual del heroe para imprimirlo 
            Scriptable.hero = GetHero(GameManager.player1Heros[i]);//obtener el scriptable object y asignarselo al visual
            Scriptable.InitializeHero();//inicializar el heroe en el visual 

            //instanciar el heroe en su respectiva posicion 
            GameObject game1 = GameObject.Instantiate(GameManager.instancia.heroPrefab, GameManager.instancia.maze.transform.GetChild(position2[i].x).GetChild(position2[i].y).transform);
            HeroVisual Scriptable1 = game1.GetComponent<HeroVisual>();
            Scriptable1.hero = GetHero(GameManager.player2Heros[i]);//obtener el componente visual del heroe para imprimirlo 
            Scriptable1.InitializeHero();//inicializar el heroe en el visual 
        }
    }
    //obtener el Scriptable Object a partir del nombre del heroe que se le pase 
    private static Hero GetHero(string hero)
    {
        string nuevo = hero.Trim(); //eliminar los caracteres vacios del inicio
        for (int i = 0; i < GameManager.instancia.heros.Count ; i++)
        {
            GameManager.instancia.heros[i].name.Trim(); //eliminar los caracteres vacios del inicio
            if(GameManager.instancia.heros[i].name == nuevo) //si coincide con el que se quiere proceder a retornar 
            {
                Hero hero1 = GameManager.instancia.heros[i]; //guardar una instancia del heroe para retornarlo 
                return hero1;
            }
        }
        return null;
    }

    //obtener los valores para la colocacion de los heroes
    private static List<(int x , int y)> GetPositions(int cantidad, int currentPLayer)
    {
        List<(int x , int y)> values = new List<(int x, int y)>(); //guardar las posiciones en las que se colocaran los lideres 
        if(currentPLayer == 1) //si llama para el jugador 1 
        {
            for (int i = 1; i < maze.GetLength(1) ; i++) //iterar por toda la primera fila del laberinto para colocar los heroes lo mas proximo posible 
            {
                if(maze[1,i] && values.Count < cantidad) values.Add((1,i)); //si no es un obstaculo y no se ha llegado a la cantidad necesaria 
                if(values.Count >= cantidad) return values; //si ya tenemos las celdas necesarias retornar 
            }
        }
        else // si se llama para el jugador 2
        {
            for (int i = maze.GetLength(1)-2; i >= 0 ; i--) //iterar por toda la primera fila del laberinto para colocar los heroes lo mas proximo posible 
            {
                if(maze[15,i] && values.Count < cantidad) values.Add((15,i)); //si no es un obstaculo y no se ha llegado a la cantidad necesaria 
                if(values.Count >= cantidad) return values; //si ya tenemos las celdas necesarias retornar 
            }
        }
        return values;
    }
}
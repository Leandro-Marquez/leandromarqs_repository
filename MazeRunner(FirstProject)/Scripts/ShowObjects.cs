using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShowObjects : MonoBehaviour, IPointerEnterHandler
{
    //propiedad booleana para saber que tipo de objeto tenemos en cada momento
    public bool isTeleport; //propiedad booleana para saber si se trata de uno de los teleports que son salidas del laberinto
    public Image targetImage; // La imagen del objeto destino
    //textos y sus respectivas sombras para el HeroInformation(nombre, habilidad, tiempo de enfriamiento etc etc)
    public TextMeshProUGUI names,namess,hability,habilitys,coolingTime,coolingTimes,speed,speeds,life,lifes;
    private Hero CurrentHero;//guardar el heroe actual para tener acceso a sus propiedades y luego mostrarlas 
    private void Start()
    {
        //instanciar la imagen una vez se cargue la escena
        targetImage = GameObject.Find("TargetImage").GetComponent<Image>();
        //intancias los text mesh pro para evitar errores de referencia
        names = GameObject.Find("NAME").GetComponent<TextMeshProUGUI>(); //...
        namess = GameObject.Find("NAMES").GetComponent<TextMeshProUGUI>(); //...
        hability = GameObject.Find("HABILITY").GetComponent<TextMeshProUGUI>(); //...
        habilitys = GameObject.Find("HABILITYS").GetComponent<TextMeshProUGUI>(); //...
        coolingTime = GameObject.Find("COOLINGTIME").GetComponent<TextMeshProUGUI>(); //...
        coolingTimes = GameObject.Find("COOLINGTIMES").GetComponent<TextMeshProUGUI>(); //...
        speed = GameObject.Find("SPEED").GetComponent<TextMeshProUGUI>(); //...
        speeds = GameObject.Find("SPEEDS").GetComponent<TextMeshProUGUI>(); //...
        life = GameObject.Find("LIFE").GetComponent<TextMeshProUGUI>(); //...
        lifes = GameObject.Find("LIFES").GetComponent<TextMeshProUGUI>(); //...
    }

    public void OnPointerEnter(PointerEventData eventData)//entrando en el puntero se llama a esta funcion 
    {
        if(GetComponent<HeroVisual>() is not null) //si el objeto tiene el componente hero visual es un heroe logicamente 
        {
            CurrentHero = GetComponent<HeroVisual>().hero; //actualizar el heroe actual para mostrar su informacion 
            Sprite TemporalImage = GetComponent<HeroVisual>().hero.heroPhoto;//guardar el sprite del objeto a nivel de codigo
            if (TemporalImage is not null)//verificar que el compoente sprite no sea nulo para evitar errores de referencia 
            {
                Sprite sourceImage = GetComponent<HeroVisual>().hero.heroPhoto;
                if (sourceImage != null) //si la imagen no es nula(sirve )
                {
                    targetImage.sprite = sourceImage;//se asigna la imagen del objeto correspondiente 
                    Encender(0);//se encienden los correspondientes componentes de texto y se apagan los pertinentes 
                    ShowHeroInformation(eventData);//se muestra la informacion pertinente del correspondiente objeto 

                }
            }
        }
        else if(tag == "extras") //si tiene la tarjeta de extras no es un heroe se maneja aparte 
        {
            Sprite temporalImage = GetComponent<Image>().sprite; //guardar el sprite del objeto a nivel de codigo
            if(temporalImage is not null) //verificar que el compoente sprite no sea nulo para evitar errores de referencia 
            {
                targetImage.sprite = temporalImage; //se asigna la imagen del objeto correspondiente 
                Encender(1); //se encienden los correspondientes componentes de texto y se apagan los pertinentes 
                ShowObjectInformation(eventData);//se muestra la informacion pertinente del correspondiente objeto 
            }
        }
        else if(tag == "puerta") //si tiene la tarjeta de extras no es un heroe se maneja aparte 
        {
            Sprite temporalImage = GetComponent<Image>().sprite; //guardar el sprite del objeto a nivel de codigo
            if(temporalImage is not null) //verificar que el compoente sprite no sea nulo para evitar errores de referencia 
            {
                targetImage.sprite = temporalImage; //se asigna la imagen del objeto correspondiente 
                Encender(1); //se encienden los correspondientes componentes de texto y se apagan los pertinentes 
                PrintDoor();//se muestra la informacion pertinente del correspondiente objeto 
            }
        }
        else if(tag == "llave") //si tiene la tarjeta de extras no es un heroe se maneja aparte 
        {
            Sprite temporalImage = GetComponent<Image>().sprite; //guardar el sprite del objeto a nivel de codigo
            if(temporalImage is not null) //verificar que el compoente sprite no sea nulo para evitar errores de referencia 
            {
                targetImage.sprite = temporalImage; //se asigna la imagen del objeto correspondiente 
                Encender(1); //se encienden los correspondientes componentes de texto y se apagan los pertinentes 
                PrintKey();//se muestra la informacion pertinente del correspondiente objeto 
            }
        }
        
    }
    public void Encender(int current) //apagar y encender los componentes de texto segun sea necesario
    {
        if(current == 0) //si es un heroe(0) encender los textos de los heroes y apagar los de los heroes 
        {
            //encender los textos correspondientes
            names.enabled = true;//...
            namess.enabled = true;//...
            hability.enabled = true;//...
            habilitys.enabled = true;//...
            coolingTime.enabled = true;//...
            coolingTimes.enabled = true;//...
            speed.enabled = true;//...
            speeds.enabled = true;//...
            life.enabled = true;//...
            lifes.enabled = true;//...
        }
        else if(current == 1)// si es un objeto(1) encender los componentes de texto pertinentes y apagar los inncesesarios
        {
            //apagar los textos que sobran solo se utilizan el name y el hability
            coolingTime.enabled = false;
            coolingTimes.enabled = false;
            speed.enabled = false;
            speeds.enabled = false;
            life.enabled = false;
            lifes.enabled = false;
        }
    }
    private void ShowHeroInformation(PointerEventData eventData) //mostrar la informacion del heroe
    {
        //reiniciar los valores de los campos de texto de la escena
        names.text = "";//...
        namess.text = "";//...
        hability.text = "";//...
        habilitys.text = "";//...
        coolingTime.text = "";//...
        coolingTimes.text = "";//...
        speed.text = "";//...
        speeds.text = "";//...
        life.text = "";//...
        lifes.text = "";//...
        //actualizar
        names.text = "NAME: " + CurrentHero.name;//...
        namess.text = "NAME: " + CurrentHero.name;//...
        hability.text = "ABILITY: " + CurrentHero.hability;//...
        habilitys.text = "ABILITY: " + CurrentHero.hability;//...
        coolingTime.text = "COOLINGTIME: " + CurrentHero.coolingTime;//...
        coolingTimes.text = "COOLINGTIME: " + CurrentHero.coolingTime; //...
        speed.text = "SPEED: " + CurrentHero.speed;//...
        speeds.text = "SPEED: " + CurrentHero.speed;//...
        life.text = "LIFE: " + CurrentHero.life;//...
        lifes.text = "LIFE: " + CurrentHero.life;//...
    }
    private void ShowObjectInformation(PointerEventData eventData) //mostrar la informacion del objeto
    {
        //reiniciar los valores de los campos de texto de la escena
        names.text = "";//...
        namess.text = "";//...
        hability.text = "";//...
        habilitys.text = "";//...
        //se hace algo similar a un switch case verificando cada caso para proporcionar la informacion respecto al objeto correspondiente
        if(isTeleport)//si esta propiedad esta en true significa que es un teleport de los que representan la salida del laberinto 
        {
            PrintTeleport();//imprimir su respectiva informacion en su metodo
            return;//retornar para no seguir mostrando informacion inapropiada
        }
        //este es el caso de que sea otro tipo de objeto como una trampa o un item 
        string name = GetComponent<TrapVisual>().name; //obtener el nombre
        string description = GetComponent<TrapVisual>().description;//obtener la descripcion 
        names.text = $"NAME : {name}";//imprimir el nombre 
        namess.text = $"NAME : {name}";//sombre
        hability.text = description;//imprimir la descripcion 
        habilitys.text = description;//sombra
    }
    private void PrintTeleport() //el caso de que represente un teleport de inicio y salida 
    {
        names.text = "NAME: TELEPORT"; //impirmir el nombre de teleport 
        namess.text = "NAME: TELEPORT"; //sombra
        //impirmir lo que hace y su sombra
        hability.text = $"MAZE EXIT, {GameManager.player1Heros.Count * GameManager.player1Heros.Count * 2} units of energy is required for using it";//...
        habilitys.text = $"MAZE EXIT, {GameManager.player1Heros.Count * GameManager.player1Heros.Count * 2} units of energy is required for using it";//...
    }
    private void PrintDoor() //el caso de que represente un teleport de inicio y salida 
    {
        names.text = "NAME: MAGIC DOOR"; //impirmir el nombre de teleport 
        namess.text = "NAME: MAGIC DOOR"; //sombra
        //impirmir lo que hace y su sombra
        hability.text = $"If you don't have a magical key you cant pass through it";//...
        habilitys.text = $"If you don't have a magical key you cant pass through it";//...
    }
    private void PrintKey() //el caso de que represente un teleport de inicio y salida 
    {
        names.text = "NAME: MAGIC KEY"; //impirmir el nombre de teleport 
        namess.text = "NAME: MAGIC KEY"; //sombra
        //impirmir lo que hace y su sombra
        hability.text = $"This key can open any magical door if you have it ";//...
        habilitys.text = $"This key can open any magical door if you have it ";//...
    }
}
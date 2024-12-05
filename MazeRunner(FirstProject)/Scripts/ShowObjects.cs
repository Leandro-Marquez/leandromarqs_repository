using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowObjects : MonoBehaviour, IPointerEnterHandler
{
    //propiedad booleana para saber que tipo de objeto tenemos en cada momento 
    public bool isTeleport,isBomb,isBeer,isCofre,isFuego,isHueco,isMuerto,isPizza,isPosima,isRock,isVenum,isLife,isKey,isDoor;
    public Image targetImage; // La imagen del objeto destino
    public TextMeshProUGUI names,namess,hability,habilitys,coolingTime,coolingTimes,speed,speeds,life,lifes;
    private Hero CurrentHero;//guardar el heroe actual para tener acceso a sus propiedades y luego mostrarlas 
    private void Start()
    {
        //instanciar la imagen una vez se cargue la escena
        targetImage = GameObject.Find("TargetImage").GetComponent<Image>();
        //intancias los text mesh pro para evitar errores de referencia
        names = GameObject.Find("NAME").GetComponent<TextMeshProUGUI>(); 
        namess = GameObject.Find("NAMES").GetComponent<TextMeshProUGUI>(); 
        hability = GameObject.Find("HABILITY").GetComponent<TextMeshProUGUI>(); 
        habilitys = GameObject.Find("HABILITYS").GetComponent<TextMeshProUGUI>(); 
        coolingTime = GameObject.Find("COOLINGTIME").GetComponent<TextMeshProUGUI>(); 
        coolingTimes = GameObject.Find("COOLINGTIMES").GetComponent<TextMeshProUGUI>(); 
        speed = GameObject.Find("SPEED").GetComponent<TextMeshProUGUI>(); 
        speeds = GameObject.Find("SPEEDS").GetComponent<TextMeshProUGUI>(); 
        life = GameObject.Find("LIFE").GetComponent<TextMeshProUGUI>(); 
        lifes = GameObject.Find("LIFES").GetComponent<TextMeshProUGUI>(); 
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
                if (sourceImage != null)
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
    }
    private void Encender(int current) //apagar y encender los componentes de texto segun sea necesario
    {
        if(current == 0) //si es un heroe encender los textos de los heroes y apagar los de los heroes 
        {
            //encender los textos correspondientes
            names.gameObject.SetActive(true);
            namess.gameObject.SetActive(true);
            hability.gameObject.SetActive(true);
            habilitys.gameObject.SetActive(true);
            coolingTime.gameObject.SetActive(true);
            coolingTimes.gameObject.SetActive(true);
            speed.gameObject.SetActive(true);
            speeds.gameObject.SetActive(true);
            life.gameObject.SetActive(true);
            lifes.gameObject.SetActive(true);
        }
        else if(current == 1)// si es un objeto encender los componentes de texto pertinentes y apagar los inncesesarios
        {
            coolingTime.gameObject.SetActive(false);
            coolingTimes.gameObject.SetActive(false);
            speed.gameObject.SetActive(false);
            speeds.gameObject.SetActive(false);
            life.gameObject.SetActive(false);
            lifes.gameObject.SetActive(false);
        }
    }
    private void ShowHeroInformation(PointerEventData eventData) //mostrar la informacion del heroe
    {
        //reiniciar los valores de los campos de texto de la escena
        names.text = "";
        namess.text = "";
        hability.text = "";
        habilitys.text = "";
        coolingTime.text = "";
        coolingTimes.text = "";
        speed.text = "";
        speeds.text = "";
        life.text = "";
        lifes.text = "";
        //actualizar
        names.text = "NAME: " + CurrentHero.name;
        namess.text = "NAME: " + CurrentHero.name;
        hability.text = "HABILITY: " + CurrentHero.hability;
        habilitys.text = "HABILITY: " + CurrentHero.hability;
        coolingTime.text = "COOLINGTIME: " + CurrentHero.coolingTime;
        coolingTimes.text = "COOLINGTIME: " + CurrentHero.coolingTime; 
        speed.text = "SPEED: " + CurrentHero.speed;
        speeds.text = "SPEED: " + CurrentHero.speed;
        life.text = "LIFE: " + CurrentHero.life;
        lifes.text = "LIFE: " + CurrentHero.life;
    }
    private void ShowObjectInformation(PointerEventData eventData) //mostrar la informacion del objeto
    {
        //reiniciar los valores de los campos de texto de la escena
        names.text = "";
        namess.text = "";
        hability.text = "";
        habilitys.text = "";
        //se hace algo similar a un switch case verificando cada caso para proporcionar la informacion respecto al objeto correspondiente
        if(isTeleport)
        {
            PrintTeleport();
        }
        else if(isBomb)
        {

        }
        else if(isBomb)
        {

        }
        else if(isBeer)
        {

        }
        else if(isCofre)
        {

        }        
        else if(isFuego)
        {

        }        
        else if(isHueco)
        {

        }        
        else if(isMuerto)
        {

        }        
        else if(isPizza)
        {

        }        
        else if(isPosima)
        {

        }        
        else if(isRock)
        {

        }        
        else if(isVenum)
        {

        }        
        else if(isLife)
        {

        }        
        else if(isKey)
        {

        }
        else if(isDoor)
        {

        }
    }
    private void PrintTeleport()
    {
        names.text = "NAME: TELEPORT";
        namess.text = "NAME: TELEPORT";
        hability.text = $"MAZE EXIT, {GameManager.player1Heros.Count * GameManager.player1Heros.Count} in total of energy is required ";
        habilitys.text = $"MAZE EXIT, {GameManager.player1Heros.Count * GameManager.player1Heros.Count} in total of energy is required ";
    }
}
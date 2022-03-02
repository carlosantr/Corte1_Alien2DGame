using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    Rigidbody2D myBody; //Físicas del animal
    CircleCollider2D coll; //Collider del animal
    [SerializeField] float speed; //Velocidad inicial del animal
    float minX, minY, maxY, maxX; //Límites de la pantalla
    float Dir; //Dirección inicial del animal (derecha o izquierda)
    int Vida = 3;//Valor de la vida inicial
    [SerializeField] GameObject VidaCompleta;//Prefab Vida completa
    [SerializeField] GameObject VidaMedia;//Prefab vida media
    [SerializeField] GameObject VidaIncompleta;//Prefab vida incompleta
    [SerializeField] GameObject Bala;//Prefab vida incompleta
    [SerializeField] GameObject BalaCargada;//Prefab vida incompleta

    // Start is called before the first frame update
    void Start()
    {
       Vector2 esqInfIzq = Camera.main.ViewportToWorldPoint(new Vector2(0,0));//Definición de límites de la pantalla
       Vector2 esqSupDer = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
       minX = esqInfIzq.x;
       minY = esqInfIzq.y;
       maxX = esqSupDer.x;
       maxY = esqSupDer.y;    
    
       myBody = GetComponent<Rigidbody2D>();//COmponentes Físicas 2D del animal
       coll = GetComponent<CircleCollider2D>();//Componentes de la colisión con el animal

       
       Dir = Random.Range(-1.0f,1.0f);
       if(Dir<=0){ //Dirección del animal (Izquierda o derecha)
           Dir=-1;
       } else if(Dir>0){
           Dir=1;
       }

        VidaCompleta = Instantiate(VidaCompleta, new Vector2(transform.position.x, transform.position.y + 1.0f), transform.rotation);//Imagen de vida inicial (Vida=3=Completa)
        Bala.gameObject.name ="Bala";//Dando nombre a las balas normales
        BalaCargada.gameObject.name ="BalaCargada";//Dando nombre a las balas cargadas
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(transform.position.x<=minX+coll.radius || transform.position.x>=maxX-coll.radius){//Cambio de dirección y pequeño aumento de velocidad al tocar bordes (contacto del collider del animal con el borde de la pantalla)
            speed = speed * -1.01f; 
        }    
        if(speed>=10||speed<=-10){//Definiendo velocidad máxima que se puede alcanzar (cuando el juego dura mucho tiempo)
            speed=5;
        }
        myBody.velocity = new Vector2(Dir * speed, myBody.velocity.y);//Movimiento del animal (se tiene en cuenta dirección [derecha-izquierda] y valores de velocidad dados [variable serializada])
        
        if(Vida==3){//Seguimiento del movimiento de imagenes de vida arriba a los animales
            VidaCompleta.transform.position = new Vector2(transform.position.x, transform.position.y + 1.0f);
        }else if(Vida==2){
            VidaMedia.transform.position = new Vector2(transform.position.x, transform.position.y + 1.0f);
        }else if(Vida==1){
            VidaIncompleta.transform.position = new Vector2(transform.position.x, transform.position.y + 1.0f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)//Destruyendo y generando imagenes de vida actual, y destruyendo animal si se acaba la vida (Para balas normales y cargadas)
    {

        if (collision.gameObject.name == "Bala(Clone)") {//Balas normales (se pone "(Clone)", pues así las instancia el programa)
            Vida = Vida - 1;//Balas normales quitan 1 punto de vida
            if (Vida == 2)
            {//Instanciando y destruyendo íconos de vida en la parte superior de animales
                VidaMedia = Instantiate(VidaMedia, new Vector2(transform.position.x, transform.position.y + 1.0f), VidaCompleta.transform.rotation);
                Object.Destroy(VidaCompleta, 0.0f);
            } else if (Vida == 1) {
                VidaIncompleta = Instantiate(VidaIncompleta, new Vector2(transform.position.x, transform.position.y + 1.0f), VidaMedia.transform.rotation);
                Object.Destroy(VidaMedia, 0.0f);
            } else if (Vida == 0) {
                Object.Destroy(VidaIncompleta, 0.0f);
                Object.Destroy(gameObject, 0.0f);//Destrucción del animal
            }
        } else if (collision.gameObject.name == "BalaCargada(Clone)")//Balas cargadas (se pone "(Clone)", pues así las instancia el programa)
        {
            if (Vida == 3)
            {//Destruyendo los íconos de vida y animales directamente, pues la bala cargada quita todas las vidas en una sola colisión
                Object.Destroy(VidaCompleta, 0.0f);
                Object.Destroy(gameObject, 0.0f);
            }
            else if (Vida == 2)
            {
                Object.Destroy(VidaMedia, 0.0f);
                Object.Destroy(gameObject, 0.0f);
            }
            else if (Vida == 1)
            {
                Object.Destroy(VidaIncompleta, 0.0f);
                Object.Destroy(gameObject, 0.0f);
            }
        }
    }
}

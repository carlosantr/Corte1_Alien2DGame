using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;//Velocidad de la nave extraterrestre
    float minX, minY, maxY, maxX;//Límites de la pantalla
    [SerializeField] GameObject bala;//Prefab de bala normal
    [SerializeField] GameObject balaCargada;//Prefab de bala cargada
    [SerializeField] float FireRate;//Definición de FireRate (tiempo entre bala y bala [normales])
    BoxCollider2D coll;//Collider de la nave
    float NextBullet =0.0f;//Variable para indicar el tiempo que ha pasado entre bala y bala
    float TipoBala = 1;//Bala normal (1) y Bala Cargada (-1)
    float PressedTime;//Tiempo que se ha estado oprimiendo "Space" para cargar la bala
    float Carga = 0;//Cuando Carga=0, no se ha cargado completamente la bala, cuando Carga=1, se cargó completamente la bala
    Vector2 PosiBala;//Posición de instanciado de las balas (bajo la nave)

    // Start is called before the first frame update
    void Start()
    {
       Vector2 esqInfIzq = Camera.main.ViewportToWorldPoint(new Vector2(0,0));//Definición de límites de pantalla
       Vector2 esqSupDer = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
       minX = esqInfIzq.x;
       minY = esqInfIzq.y;
       maxX = esqSupDer.x;
       maxY = esqSupDer.y;

        coll = GetComponent<BoxCollider2D>();//Agregando componentes del collider de la nave
    }

    // Update is called once per frame
    void Update()
    {
        float dirH = Input.GetAxis("Horizontal");//Dirección de las flechas "<" y ">"
        float dirV = Input.GetAxis("Vertical");//Dirección de las flechas "arriba" y "abajo"

        transform.Translate(new Vector2(dirH * speed * Time.deltaTime, dirV * speed * Time.deltaTime));//Movimiento de la nave, teniendo en cuenta la dirección y velocidad definidas

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX + (coll.size.x / 2.0f), maxX - (coll.size.x / 2.0f)), Mathf.Clamp(transform.position.y, 0.5f + (coll.size.y / 2.0f), maxY - (coll.size.y / 2.0f)));//Límites de la nave con la pantalla

        if (Input.GetKeyDown(KeyCode.Tab))//Cambio de tipo de bala, con la tecla "Tab"
        {
            TipoBala = TipoBala * -1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && TipoBala == 1 && Time.time >= NextBullet)//Lanzamiento de bala normal
        {
            NextBullet = Time.time + FireRate;//Tiempo que pasa después de un lanzamiento
            PosiBala = new Vector2(transform.position.x, transform.position.y - 1.2f);//Posición de la bala
            Instantiate(bala, PosiBala, transform.rotation);//Instanciado de la bala normal
        }
        if (Input.GetKey(KeyCode.Space) && TipoBala == -1)//Lanzamiento de bala cargada
        {
            PressedTime = PressedTime + Time.deltaTime;//Cálculo de tiempo que se ha estado oprimiento la tecla "Space" para cargar bala [Time.deltaTime es el tiempo entre frames]
            if (PressedTime >= 3)
            {
                    Carga = 1;//Bala cargada
                    PosiBala = new Vector2(transform.position.x, transform.position.y - 1.4f);//Posición de la bala cargada
            }
        }else if (Carga == 1)//Cuando bala está cargada (Carga=1)
        {
            Instantiate(balaCargada, PosiBala, transform.rotation);//Instanciado de la bala cargada
            Carga = 0.0f;//Reinicio de valores (bala ya no cargada)
            PressedTime = 0.0f;//Reinicio de contador de tiempo que se ha oprimido "Space"
        }

    }

    private void FixedUpdate()
    {

    }



}

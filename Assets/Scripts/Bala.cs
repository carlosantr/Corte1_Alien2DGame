using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] GameObject bullet;//Prefab de bala a destruir
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D (Collision2D collision)//Destrucción de la bala al chocar con alguna superficie (suelo o animal)
    { 
    Object.Destroy(bullet,0.0f);//Destrucción de bala
    }


}

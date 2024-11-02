using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerTomarObjetos : MonoBehaviour
{
    public bool action;
    public bool isObjectNextYou;
    public GameObject objectTaken;

    public Vector3 original_scale;

    GameObject padre;

    private void Awake()
    {
        padre = GameObject.Find("Personaje");
    }

    // Start is called before the first frame update
    void Start()
    {
        action = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!action && isObjectNextYou)
            { //mod postclase ... solo se puede tomar si estas cerca .. evita que se inicie la toma antes de estar cerca 
                action = true;
            }
            else
            {
                action = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (objectTaken == null)
        { //(mod postclase) solo actualiza la variable si no estamos cargando a algun objeto
            if (other.gameObject.CompareTag("TakenObject"))
            {
                //modificado por corrutina... ->
                isObjectNextYou = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject temporal = other.gameObject;

        if (temporal.CompareTag("TakenObject"))
        {
            if (isObjectNextYou)
            {
                if (objectTaken == null)
                { //si el objeto se debe tomar
                    if (action)
                    { //modificado postclase (no se ha tomado el objeto)
                        objectTaken = temporal;  //guarda la instancia al objeto tomado

                        Vector3 aux = temporal.transform.localScale;
                        original_scale = new Vector3(aux.x, aux.y, aux.z); //respalda la escala original

                        objectTaken.transform.SetParent(padre.transform); //cambia de padre
                        Rigidbody rb = objectTaken.GetComponent<Rigidbody>(); //obtiene el rigidbody del objeto tomado
                        rb.isKinematic = true; //activa la funcionalidad kinematica
                        rb.useGravity = false; //desactiva la gravedad del objeto tomado para que no caiga de las manos
                        objectTaken.transform.position = transform.position; //posiciona al obj tomado en las manos del personaje
                        objectTaken.transform.rotation = transform.rotation; //posiciona al obj tomado en la rotacion de las manos del personaje            
                        objectTaken.transform.localScale = transform.localScale;
                    }
                }
                else
                { //si no lo estoy tomando ya
                    if (!action)
                    { // pero ya se habia estado tomando (es decir, se solto)            
                        objectTaken.transform.SetParent(null); //cambia de padre
                        Rigidbody rb = objectTaken.GetComponent<Rigidbody>(); //obtiene el rigidbody del objeto tomado
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        objectTaken.transform.localScale = original_scale; //recupera la escala original

                        objectTaken = null;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectTaken == null)
        { //(mod postclase) solo actualiza la variable si no estamos cargando a algun objeto
            if (other.gameObject.CompareTag("TakenObject"))
            {
                isObjectNextYou = false;
            }
        }
    }



}

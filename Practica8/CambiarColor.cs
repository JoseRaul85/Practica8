using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CambiarColor : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectsList;
    [SerializeField] TextMeshProUGUI colorText;
    public List<string> coloresDisponibles = new List<string> { "Rojo", "Amarillo", "Verde" };
    public int numeroAleatorio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Personaje") && coloresDisponibles.Count > 0)
        {
            numeroAleatorio = Random.Range(0, coloresDisponibles.Count);
            colorText.text = coloresDisponibles[numeroAleatorio];
            coloresDisponibles.RemoveAt(numeroAleatorio);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Rojo") || other.gameObject.CompareTag("Verde") || other.gameObject.CompareTag("Amarillo"))
        {
            
            Vector3 posicionOriginal = other.transform.position;

            
            if (other.gameObject.CompareTag(colorText.text))
            {
                Destroy(other.gameObject); 
            }
            else
            {
                other.transform.position = posicionOriginal; 
            }
        }
    }
}

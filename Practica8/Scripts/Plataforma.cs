using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plataforma : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public string requiredColor;
    private GameObject player;
    private List<string> availableColors = new List<string> { "Rojo", "Amarillo", "Verde" };

    private void Start()
    {
        player = GameObject.Find("Personaje");
        SetRandomColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Personaje"))
        {
            messageText.text = "Es necesario el color: " + requiredColor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Personaje"))
        {
            HandlerTomarObjetos handler = player.GetComponentInChildren<HandlerTomarObjetos>();
            if (handler != null && handler.objectTaken != null)
            {
                string objectColor = handler.objectTaken.name;
                if (objectColor.Equals(requiredColor, System.StringComparison.OrdinalIgnoreCase))
                {
                    Destroy(handler.objectTaken);
                    handler.objectTaken = null;
                    messageText.text = "Cubo correcto. Cubo " + requiredColor + " destruido.";
                    availableColors.Remove(requiredColor);

                    if (availableColors.Count > 0)
                    {
                        SetRandomColor();
                        messageText.text += " Lleva el cubo de color " + requiredColor;
                    }
                    else
                    {
                        messageText.text += " Todos los cubos han sido recogidos";
                    }
                }
                else
                {
                    OriginalPosition originalPosition = handler.objectTaken.GetComponent<OriginalPosition>();
                    if (originalPosition != null)
                    {
                        handler.objectTaken.transform.SetParent(null);
                        Rigidbody rb = handler.objectTaken.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                            rb.useGravity = true;
                        }
                        handler.objectTaken.transform.position = originalPosition.originalPosition;
                    }

                    handler.objectTaken = null;
                    messageText.text = "Cubo incorrecto. Regresa con el correcto";
                }
            }
        }
    }

    private void SetRandomColor()
    {
        int index = Random.Range(0, availableColors.Count);
        requiredColor = availableColors[index];
    }
}

    public class OriginalPosition : MonoBehaviour
    {
    public Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }
}

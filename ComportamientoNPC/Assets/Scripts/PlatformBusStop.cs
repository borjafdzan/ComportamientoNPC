using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBusStop : MonoBehaviour
{
    public Transform posicionInicial;
    public Transform posicionFinal;
    public float tiempoViaje = 2;
    private bool activo = true;
    void Start()
    {
        if (posicionFinal == null)
        {
            Debug.Log("No se ha establecido una posicion inciial");
        }
        if (posicionFinal == null)
        {
            Debug.Log("No se ha establecido una posicion Final");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            activo = !activo;
            this.transform.position = posicionInicial.position;
        }
        if (activo)
        {
            float t = Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time / tiempoViaje, 1));

            Vector3 posicion = Vector3.Lerp(posicionInicial.position, posicionFinal.position, t);
            transform.position = posicion;
        }
    }
}

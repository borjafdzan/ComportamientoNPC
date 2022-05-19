using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public GameObject Jugador;
    public Transform Casa;
    public LayerMask mascaraJugador;
    public float RadioVision = 15;
    NavMeshAgent agenteNavegacion;

    Collider colisionador;
    // Start is called before the first frame update
    void Start()
    {
        this.agenteNavegacion = GetComponent<NavMeshAgent>();

        agenteNavegacion.destination = Jugador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ComprobarRadio());
        if (ComprobarRadio())
        {
            this.agenteNavegacion.destination = Jugador.transform.position;
        }
        else
        {
            this.agenteNavegacion.destination = Casa.position;
        }
    }

    private bool ComprobarRadio()
    {
        RaycastHit[] JugadoresEnObjetivo = Physics.SphereCastAll(this.transform.position, RadioVision, this.transform.forward, RadioVision, mascaraJugador);
        if (JugadoresEnObjetivo.Length > 0)
        {

            return true;
        }
        else
        {

            return false;
        }
    }
}

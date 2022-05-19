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
    public float anguloLimite = 30;
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
            if (ComprobarSiNoHayObstaculos(JugadoresEnObjetivo[0].transform.gameObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {

            return false;
        }
    }

    private bool ComprobarSiNoHayObstaculos(GameObject objetivoDetectado)
    {
        Vector3 direccion = this.transform.position - objetivoDetectado.transform.position;
        direccion = direccion.normalized;
        direccion = -direccion;
        //Subimos la posicion del raycast
        RaycastHit[] objetivos = Physics.RaycastAll(this.transform.position + Vector3.up, direccion, 10, -1);
        //En el caso de que el primer objeto que vea el raycast del minion sea el jugador
        foreach (RaycastHit golpe in objetivos)
        {
            if (golpe.transform.gameObject.tag == "Player")
            {
                //Comprobamos el angulo
                if (ComprobarAngulo(direccion))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private bool ComprobarAngulo(Vector3 direccion)
    {
        float angulo = Vector3.Angle(this.transform.forward, direccion);
        if (angulo <= anguloLimite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

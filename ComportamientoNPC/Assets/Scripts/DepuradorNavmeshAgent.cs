using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


[RequireComponent(typeof(NavMeshAgent))]
public class DepuradorNavmeshAgent : MonoBehaviour
{
    public Color colorCaminoDepurado = Color.black;

    public bool velocidad;
    public bool velocidadDeseada;
    public bool camino;
    [SerializeField]
    NavMeshAgent agente;
    // Start is called before the first frame update
    void Start()
    {
        this.agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        if (velocidad){
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + this.agente.velocity);
        }
        if (velocidadDeseada){
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + this.agente.desiredVelocity);
        }

        if (camino){
            Gizmos.color = colorCaminoDepurado;
            NavMeshPath camino = this.agente.path;
            Vector3 esquinaAnterior = transform.position;
            foreach(Vector3 esquina in camino.corners){
                Gizmos.DrawLine(esquinaAnterior, esquina);
                Gizmos.DrawSphere(esquina, 0.1f);

                esquinaAnterior = esquina;
            }
        }
    }
}

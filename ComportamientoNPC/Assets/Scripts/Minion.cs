using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public GameObject Jugador;
    private NavMeshAgent agenteNavegacion;
    // Start is called before the first frame update
    void Start()
    {
        this.agenteNavegacion = GetComponent<NavMeshAgent>();
        
        agenteNavegacion.destination = Jugador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        agenteNavegacion.destination = Jugador.transform.position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum TipoVision
{
    Circular,
    CircularConObstaculos,
    CircularConObstaculosYAngulos
}
public class Minion : MonoBehaviour
{
    public GameObject Jugador;
    public Transform Casa;
    public LayerMask mascaraJugador;
    public float RadioVision = 15;
    public float AngulosVision = 40;
    NavMeshAgent agenteNavegacion;
    public TipoVision tipoVision;
    GameObject targetObjetivo;

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
        if (ComprobarVision())
        {
            this.agenteNavegacion.destination = Jugador.transform.position;
        }
        else
        {
            this.agenteNavegacion.destination = Casa.position;
        }
    }

    private bool ComprobarVision()
    {
        switch (tipoVision)
        {
            case TipoVision.Circular:
                return ComprobarRadio();
            case TipoVision.CircularConObstaculos:
                return ComprobarCircularConObstaculos();
            case TipoVision.CircularConObstaculosYAngulos:
                return ComprobarConAngulo();
        }
        return false;
    }

    private bool ComprobarRadio()
    {
        RaycastHit[] JugadoresEnObjetivo = Physics.SphereCastAll(this.transform.position, RadioVision, this.transform.forward, RadioVision, mascaraJugador);
        if (JugadoresEnObjetivo.Length > 0)
        {
            targetObjetivo = JugadoresEnObjetivo[0].transform.gameObject;
            return true;
        }
        return false;
    }

    private bool ComprobarCircularConObstaculos()
    {
        if (ComprobarRadio())
            if (ComprobarSiNoHayObstaculos(Jugador))
                return true;
        return false;
    }

    private bool ComprobarConAngulo()
    {
        if (ComprobarRadio())
            if (ComprobarSiNoHayObstaculos(Jugador))
                if (ComprobarRango())
                    return true;
        return false;
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
                return true;
            return false;
        }
        return false;
    }

    private bool ComprobarRango()
    {
        Vector3 direccion = this.transform.position - Jugador.transform.position;
        direccion = direccion.normalized;
        direccion = -direccion;
        //Cogemos el angulo entre el vector frente del jugador y el objetivo
        float angulo = Vector3.Angle(this.transform.forward, direccion);
        if (angulo <= AngulosVision)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

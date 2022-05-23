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

public enum EstadosJugador
{
    Esperar,
    Perseguir,
    Volver
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
    public EstadosJugador estado;

    Collider colisionador;
    // Start is called before the first frame update
    void Start()
    {
        this.agenteNavegacion = GetComponent<NavMeshAgent>();

        agenteNavegacion.destination = Jugador.transform.position;
        estado = EstadosJugador.Esperar;
    }

    private void ConfigurarEstado(EstadosJugador nuevoEstado)
    {
        if (nuevoEstado != estado)
        {
            estado = nuevoEstado;
            InterfazUsuario.instancia.ConfigurarEtiqueta(estado.ToString());
            switch (estado)
            {
                case EstadosJugador.Esperar:
                    agenteNavegacion.destination = Casa.position;
                    break;
                case EstadosJugador.Perseguir:
                    agenteNavegacion.destination = Jugador.transform.position;
                    Debug.Log("Se cambia el estado a perseguir");
                    break;
                case EstadosJugador.Volver:
                    agenteNavegacion.destination = Casa.position;
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        bool estaEnVision = ComprobarVision();
        Debug.Log(estaEnVision);
        switch (estado)
        {
            case EstadosJugador.Esperar:
                if (estaEnVision)
                    ConfigurarEstado(EstadosJugador.Perseguir);
                break;
            case EstadosJugador.Volver:
                if (estaEnVision)
                    ConfigurarEstado(EstadosJugador.Perseguir);
                else if (agenteNavegacion.remainingDistance <= agenteNavegacion.stoppingDistance)
                    ConfigurarEstado(EstadosJugador.Esperar);
                break;
            case EstadosJugador.Perseguir:
                if (!estaEnVision)
                    ConfigurarEstado(EstadosJugador.Volver);
                else 
                    agenteNavegacion.destination = Jugador.transform.position;
                break;
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
        RaycastHit[] objetivos = Physics.RaycastAll(this.transform.position + Vector3.up, direccion, RadioVision, -1);
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
        float angulo = Vector3.SignedAngle(this.transform.forward, direccion, this.transform.up);
        //Vector3 productoCruzado = Vector3.Cross(this.transform.forward, direccion);
        Debug.Log(angulo);
        if (angulo <= AngulosVision / 2 && angulo >= -AngulosVision / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

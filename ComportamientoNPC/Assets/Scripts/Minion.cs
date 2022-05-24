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
    Patrullar,
    Perseguir,
    Volver
}
public class Minion : MonoBehaviour
{
    //Campos relacionados con la patrulla
    public Transform[] posicionesMoverse;
    private int indicePatrulla;
    private bool isInDestino = false;
    public GameObject Jugador;
    public Transform Casa;
    public LayerMask mascaraJugador;
    public float RadioVision = 15;
    public float AngulosVision = 40;
    NavMeshAgent agenteNavegacion;
    public TipoVision tipoVision;
    GameObject targetObjetivo;
    public EstadosJugador estado;
    Transform rotacionSegEsperar;

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
                    //agenteNavegacion.destination = Casa.position;
                    agenteNavegacion.destination = transform.position;
                    rotacionSegEsperar = this.transform;
                    break;

                case EstadosJugador.Patrullar:
                    agenteNavegacion.destination = posicionesMoverse[indicePatrulla].position;
                    break;
                case EstadosJugador.Perseguir:
                    agenteNavegacion.destination = Jugador.transform.position;
                    //Debug.Log("Se cambia el estado a perseguir");
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
        switch (estado)
        {
            case EstadosJugador.Esperar:
                if (estaEnVision)
                    ConfigurarEstado(EstadosJugador.Perseguir);
                    /*
                else
                    EjecutarEsperar();
                    */
                break;
            case EstadosJugador.Patrullar:
                if (estaEnVision)
                    ConfigurarEstado(EstadosJugador.Perseguir);
                else
                    Patrullar();
                break;
            case EstadosJugador.Volver:
                if (estaEnVision)
                    ConfigurarEstado(EstadosJugador.Perseguir);
                else if (agenteNavegacion.remainingDistance <= agenteNavegacion.stoppingDistance)
                    //ConfigurarEstado(EstadosJugador.Esperar);
                    ConfigurarEstado(EstadosJugador.Patrullar);
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
        DepurarAngulo();
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
        //Debug.Log(angulo);
        if (angulo <= AngulosVision / 2 && angulo >= -AngulosVision / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DepurarAngulo(){
        Debug.Log("Esta funcionando");
        Vector3 direccionIzquierda =  Quaternion.AngleAxis(-AngulosVision/2, Vector3.up) * this.transform.forward;
        Vector3 direccionDerecha = Quaternion.AngleAxis(AngulosVision, Vector3.up) * this.transform.forward;
        Debug.DrawRay(this.transform.position + Vector3.up, direccionDerecha + Vector3.up * RadioVision, Color.cyan);
        Debug.DrawRay(this.transform.position + Vector3.up, direccionIzquierda + Vector3.up * RadioVision, Color.cyan);
    }

    private void EjecutarEsperar(){
        float valorRotacion = Mathf.PingPong(Time.time * 4, 60);
        Debug.Log(valorRotacion);
        valorRotacion -= 30;
        transform.rotation = Quaternion.Euler(0, this.rotacionSegEsperar.rotation.y + valorRotacion, 0);
    }

    //Funcionalidad Patrullar
    private void Patrullar()
    {
        if (AlcanzoDestino())
        {
            SiguientePosicion();
        }
    }
    private bool AlcanzoDestino()
    {
        if (Vector3.Distance(this.transform.position, posicionesMoverse[indicePatrulla].position) < 0.5f)
        {
            return true;
        }
        return false;
    }
    private void SiguientePosicion()
    {
        this.indicePatrulla++;
        if (indicePatrulla >= this.posicionesMoverse.Length)
        {
            this.indicePatrulla = 0;
        }

        ConfigurarEstado(EstadosJugador.Esperar);
        Invoke("AcabarEspera", 5);

    }


    private void AcabarEspera()
    {
        ConfigurarEstado(EstadosJugador.Patrullar);
        this.agenteNavegacion.destination = posicionesMoverse[indicePatrulla].position;
        //Debug.Log("La siguiente posicion es " + this.indicePatrulla);
    }
}

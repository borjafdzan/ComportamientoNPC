using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InterfazUsuario : MonoBehaviour
{
    public static InterfazUsuario instancia;
    public Text textoEstado;
    public Minion NPC;
    // Start is called before the first frame update
    void Start()
    {
        instancia = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickCircular(){
        NPC.tipoVision = TipoVision.Circular;
    }

    public void OnClickObstaculos(){
        NPC.tipoVision = TipoVision.CircularConObstaculos;
    }

    public void OnClickAngulos(){
        NPC.tipoVision = TipoVision.CircularConObstaculosYAngulos;
    }

    public void ConfigurarEtiqueta(string texto){
        textoEstado.text = texto;
    }
}

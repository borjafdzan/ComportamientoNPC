using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfazUsuario : MonoBehaviour
{
    public Minion NPC;
    // Start is called before the first frame update
    void Start()
    {
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparnInimigo : MonoBehaviour
{
    [SerializeField]
    private GameObject[] PontoSpawns;
    [SerializeField]
    private GameObject Inimigo;
    private int Qtpontos = 0;
    public int NumerodeInimigos = 1;
    private Vector3 pontSpawn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void spawnarInimigos()
    {
     
        for (int ini = 0; ini < NumerodeInimigos; ini++)
        {
            pontSpawn = new Vector3(PontoSpawns[Qtpontos].transform.rotation.x + 0.5f, PontoSpawns[Qtpontos].transform.rotation.y, PontoSpawns[Qtpontos].transform.rotation.z);
            if (Qtpontos >= PontoSpawns.Length)
            {
                Qtpontos = 0;
            }
            Instantiate(Inimigo, PontoSpawns[Qtpontos].transform.position, PontoSpawns[Qtpontos].transform.rotation);

            Qtpontos++;
        }
        Camera_Control.canCont.StopCam = true;
        Controle_cena.c_cena.ChecarAi();
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            spawnarInimigos();
        }
    }
}

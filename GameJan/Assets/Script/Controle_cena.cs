using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_cena : MonoBehaviour
{
    [SerializeField]
    private int Valor_Personagem = 0;
    [SerializeField]
    private GameObject player_1, player_2;//prefab para Instanciar
    private GameObject C_change; // presonagen Instanciado
    [SerializeField]
    private GameObject[] Inimigos;
    private NPC[] npc;
    bool posic = true;
    void Start()
    {
        instaciar(Valor_Personagem);// Spawna O Personagem q vai comecar
        ChecarAi();
    }  
    void instaciar(int p)
    {       
        if (player_1 && p == 0) C_change = Instantiate(player_1,transform.position,transform.rotation);
        if (player_2 && p == 1) C_change = Instantiate(player_1, transform.position, transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && Valor_Personagem != 0)
        {
            Change_Char(0);
            Valor_Personagem = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Valor_Personagem != 1)
        {
            Change_Char(1);
            Valor_Personagem = 1;
        }
    }
    void Change_Char(int p)
    {
        Vector3 localSpaw;
        Vector3 RotateSpaw;

        localSpaw = C_change.transform.position;
        RotateSpaw = C_change.transform.eulerAngles;
        Destroy(C_change.gameObject);
        if (player_1 && p == 0)
        {
            C_change = Instantiate(player_1, localSpaw, transform.rotation);
            C_change.transform.eulerAngles = RotateSpaw;
        }       
        if (player_2 && p == 1)
        {
            C_change = Instantiate(player_2, localSpaw, transform.rotation);
            C_change.transform.eulerAngles = RotateSpaw;
        }
    }

    void ChecarAi()// procura todos os inimigos e coloca na Array
    {
        Inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        npc = new NPC[Inimigos.Length];
        PosicionarAi();

    }
    void PosicionarAi()
    {      
       
        for (int p = 0; p < Inimigos.Length; p++)
        {
            npc[p] = Inimigos[p].GetComponent<NPC>();
            posic = !posic;
            
            if(posic)
            {
                npc[p].Side_right = true;
            }
            if (!posic)
            {
                npc[p].Side_right = false;
            }
        }        
    }
}

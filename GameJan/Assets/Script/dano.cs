using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dano : MonoBehaviour
{

    public bool Inimigo = false;
    [SerializeField]
    private bool Projetil = false;
    [SerializeField]
    private float VelocidadeProjetil = 3.0f;
    private float direcao = 0;
    [SerializeField]
    private bool DesativaAoAtigir = true;
    NPC npc;
    Player player;
    [SerializeField]
    float dano_ataque = 10.0f;
    public bool Derrubar = false;
    public bool CorpoEletrico = false;
    void Start()
    {
        if (Projetil)
        {
            Destroy(gameObject,0.75f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Projetil)
        {
            transform.Translate((direcao/2), direcao, VelocidadeProjetil);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Inimigo)
        {
            if (other.gameObject.CompareTag("Inimigo"))
            {
                npc = other.GetComponent<NPC>();
                if (npc.Tipo_tinimigo == 3 && Projetil)
                {
                    direcao = 3.0f;
                }
                if (!Projetil || npc.Tipo_tinimigo != 3)
                {
                    npc.hit(dano_ataque);
                    if (Derrubar)
                    {
                        npc.cair();
                    }
                    if (DesativaAoAtigir)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
        if (Inimigo)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                if(CorpoEletrico)
                {
                    player.cair(dano_ataque);
                }
                if (!CorpoEletrico)
                {
                    player.hit(dano_ataque);
                }
                if (DesativaAoAtigir)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

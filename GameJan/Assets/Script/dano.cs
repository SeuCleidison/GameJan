using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dano : MonoBehaviour
{

    public bool Inimigo = false;
    [SerializeField]
    private bool Projetil = false;
    [SerializeField]
    private float VelocidadeProjetil = 5.0f;
    [SerializeField]
    private bool DesativaAoAtigir = true;
    NPC npc;
    Player player;
    float dano_ataque = 10.0f;
    
    void Start()
    {
        if (Projetil)
        {
            Destroy(gameObject,1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Projetil)
        {
            transform.Translate(0, 0, VelocidadeProjetil);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!Inimigo)
        {
            if (other.gameObject.CompareTag("Inimigo"))
            {
                npc = other.GetComponent<NPC>();
                npc.hit(dano_ataque);
                if(DesativaAoAtigir)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        if (Inimigo)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                player.hit(dano_ataque);
                if (DesativaAoAtigir)
                {
                    gameObject.SetActive(false);
                }
            }
        }

    }
}

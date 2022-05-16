using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NPC : MonoBehaviour
{
    public float Life = 100.0f, LifeMax = 100.0f,Target_distance = 1000; 
    private Animator Npc_Anim;
    private NavMeshAgent nave;
    private GameObject Player;
    private float SpeedMove;
    private Rigidbody rig;
    [SerializeField]
    private CapsuleCollider capColl;
    #region Var de Combate
    [SerializeField]
    private GameObject Mao_dir, Mao_Esq, perna_dir, perna_Esq;
    private bool dead = false, Look_To_Player = false;
    [SerializeField]
    private bool Side_right;
    private bool caido = false;
    Vector3 frontPon,BackPos;
    #endregion
    void Start()
    {
        Npc_Anim = GetComponent<Animator>();
        nave = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Npc_Anim.SetBool("vivo", true);
        SpeedMove = nave.speed;
        rig = GetComponent<Rigidbody>();
        caido = false;
        destivaAtaq();
        Ataque_Fim();
    }
    void Update()
    {
       if(Player && nave  && !dead)
        {
            Movimento();
        }
       if (!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
       if (Life <= 0 && dead == false)
        {
            Npc_Anim.SetBool("vivo", false);
            Morto();
            dead = true;
        }
    }
    void Movimento()
    {
        Target_distance = nave.remainingDistance;
        if (Player)
        {
            frontPon = new Vector3(Player.transform.position.x-1, Player.transform.position.y, Player.transform.position.z);
        }
        if (Player)
        {
            BackPos = new Vector3(Player.transform.position.x + 1, Player.transform.position.y, Player.transform.position.z);
        }
        if (!dead)
        {
            if (Target_distance > nave.stoppingDistance)
            {
                Npc_Anim.SetFloat("Movimento", 1.0f);
                Ataque_Fim();
            }
            if (Target_distance <= nave.stoppingDistance)
            {
                Npc_Anim.SetFloat("Movimento", 0f);
                Ataque();
                Rotate_To_Target();
            }
            if (Look_To_Player)
            {
                Quaternion targetRotation;
                targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                targetRotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, targetRotation.eulerAngles.y, transform.eulerAngles.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            }
        }
        if (Side_right)
        {
            nave.destination = frontPon;
        }
        if (!Side_right)
        {
            nave.destination = BackPos;
        }
        if (Life <=0 && dead == false)
        {            
            Npc_Anim.SetBool("vivo", false);
            Morto();
            dead = true;
        }
        ProcuraLado();
    }
    public void cair()
    {
        Npc_Anim.SetBool("Cair", true);
    }
    void noChaor()
    {
        caido = true;
        if(capColl) capColl.enabled = false;
        nave.speed = 0;
        Npc_Anim.SetBool("NoChao", true);        
    }
    void levantar()
    {
       
        caido = false;
        Npc_Anim.SetBool("Cair", false);
        Npc_Anim.SetBool("NoChao", false);
        nave.speed = SpeedMove;
        if (capColl) capColl.enabled = true;
    }
    void ProcuraLado()//Escole O lado pra atacar direito ou esquerdo
    {
        
        float dist_Fron = Vector3.Distance(frontPon, transform.position);
        float dist_Back = Vector3.Distance(BackPos, transform.position);
        if(dist_Back < dist_Fron)
        {
            Side_right = false;
        }
        if (dist_Back > dist_Fron)
        {
            Side_right = true;
        }

    }
    #region funçoues de Combate
    void Ataque()
    {      
        Npc_Anim.SetBool("Ataq", true);    
    }  
    void Rotate_To_Target() //Força o NPC a Girar Para o Player
    {
        Look_To_Player = true;
    }
    void End_rotate() //Parar de forçar o NPC a Olhar
    {
        Look_To_Player = false;
    }
    void Ataque_Inicio()
    {
        Npc_Anim.SetBool("Ataq", true);
    }
    void Ataque_Fim()
    {
        Npc_Anim.SetBool("Ataq", false);
    }  

    void AtivaAtaque_Mao_Dir()
    {
        //Ativa o colisor MãoDireita
        if (Mao_dir) Mao_dir.SetActive(true);
    }
    void destivaAtaq()// destiva Colisores dos Ataques
    {
       if (Mao_dir) Mao_dir.SetActive(false);
       if (Mao_Esq) Mao_Esq.SetActive(false);
       if (perna_dir) perna_dir.SetActive(false);
       if (perna_Esq) perna_Esq.SetActive(false);
    }
    public void hit(float dano = 0)
    {
        int hitLocal = Random.Range(0, 3);
        Npc_Anim.SetBool("Hit", true);
        Npc_Anim.SetInteger("AreaHit", hitLocal);
        nave.speed = 0;
        StartCoroutine(recuperar());
        Life -= dano;
    }
    void Endhit()
    {
        Npc_Anim.SetBool("Hit", false);
        End_rotate();
    }
    void Move_Npc() //Retorna o Valor normal da Velocidade 
    {
        nave.speed = SpeedMove;
    }
    void PausarMove() //Faz o NPC parar o movimento quando ataca por exemplo
    {
        nave.speed = 0;
        StartCoroutine(recuperar());
    }
    IEnumerator recuperar()
    {
        yield return new WaitForSeconds(1.0f);
        Npc_Anim.SetBool("Hit", false);
        nave.speed = SpeedMove;
    }
    void Morto()
    {
        Npc_Anim.SetBool("died", true);
        StopAllCoroutines();
        nave.speed = 0;
        if (capColl) capColl.enabled = false;
        rig.isKinematic = true;
        transform.tag = "Morto";
    }
    void Morrer_Event_FIX() // Evita que o NPC fique preso em Loop de morte
    {
        Npc_Anim.SetBool("died", false);
        nave.enabled = false;
    }
    #endregion
}


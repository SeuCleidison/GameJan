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
    [SerializeField] // prefab da Bala
    private GameObject shoot,PontaArma;
    private float SpeedMove;
    private Rigidbody rig;
    [SerializeField]
    Renderer[] CorOriginal; // verifica a Cor Original;
    Renderer[] CorAlterar;// A cor do Dano a Cor Original;
    Vector4[] cores; 
    [SerializeField]
    private CapsuleCollider capColl; 
    public int Tipo_tinimigo = 0;// 0 == inimigo Comum , 1 == inimigo Rifle ,2 = Brute , 3 = Shild , 4 = Eletrico , 5 = Boss
    private int novoPosicao = 0;
    #region Var de Combate
    [SerializeField]
    private GameObject Mao_dir, Mao_Esq, perna_dir, perna_Esq;
    private bool dead = false, Look_To_Player = false;
    [HideInInspector]
    public bool Side_right;
    [SerializeField]//temp
    private bool caido = false, Move_on_Ataque = true;
    Vector3 frontPon,BackPos;
    Vector3[] evade = new Vector3[5];
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
        StartCoroutine(GetCor());
    }
    IEnumerator GetCor()
    {
        yield return new WaitForEndOfFrame();       
       if(CorOriginal.Length >0)
        {
            CorAlterar = new Renderer[CorOriginal.Length];
            cores = new Vector4[CorOriginal.Length];
            for (int c = 0;c < CorOriginal.Length ; c++)
            {
                CorAlterar[c] = CorOriginal[c];
                cores[c] = CorOriginal[c].material.color;
            }
        }
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
    #region Animacoes Nao Combate
    void Movimento()
    {
        Target_distance = nave.remainingDistance; // BackPos = Poicao a Esquerda da tela  , frontPon  = Poicao a Direita da tela
       // se Tipo_tinimigo tem valor igual a 1 Ele vai atacar a distancia Diferemde de 1 Corpo a corpo
        if (Player)
        {
           if (Tipo_tinimigo != 1) frontPon = new Vector3(Player.transform.position.x-1, Player.transform.position.y, Player.transform.position.z);
           if (Tipo_tinimigo == 1) frontPon = new Vector3(Player.transform.position.x - 3, Player.transform.position.y, Player.transform.position.z);

        }
        if (Player)
        {
            if (Tipo_tinimigo != 1) BackPos = new Vector3(Player.transform.position.x + 1, Player.transform.position.y, Player.transform.position.z);
            if (Tipo_tinimigo == 1) BackPos = new Vector3(Player.transform.position.x + 3, Player.transform.position.y, Player.transform.position.z);

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
                if (Move_on_Ataque == true)
                {
                    Ataque();
                }
                Rotate_To_Target();               
            }        
            if (Look_To_Player &&  caido == false) // rotaciona O NPC para a direcao do Player
            {
                Quaternion targetRotation;
                targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                targetRotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, targetRotation.eulerAngles.y, transform.eulerAngles.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            }
            if (novoPosicao > 0) //Faz o npc Se mover em um ponto aleatorio 
            {
                Evade();
            }

        }
        if (Move_on_Ataque == true)//Mover em Direcao para o ataque
        {
            if (Side_right)
            {
                nave.destination = frontPon; // Direita do Alvo
            }
            if (!Side_right)
            {
                nave.destination = BackPos;// Esquerda do Alvo
            }           
        }
        if (Life <= 0 && dead == false)
        {
            Npc_Anim.SetBool("vivo", false);
            Morto();
        }
        ProcuraLado();
    }   
    void Shoot()
    {
        Instantiate(shoot, PontaArma.transform.position, transform.rotation);
    }
    public void cair()
    {
        int hitBrut = Random.Range(0, 5);//Chamce do Bruto ter Animacao de queda
        if (Tipo_tinimigo != 2 && Tipo_tinimigo != 5)
        {
            Move_on_Ataque = false;
            Npc_Anim.SetBool("Cair", true);

            End_rotate();
            nave.speed = 0;
        }
        if (Tipo_tinimigo == 2 || Tipo_tinimigo == 5)
        {
            if (hitBrut >= 4)
            {
                Move_on_Ataque = false;
                Npc_Anim.SetBool("Cair", true);

                End_rotate();
                nave.speed = 0;
            }
        }
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
        StartCoroutine(PausaMove());
    }
    #endregion
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
    IEnumerator PausaMove ()// Faz uma Pequena Pausa no movimento
    {
        int NumeroDeVoltas = 0;//Sorteia o Numero de Pausas Antes de atacar novamente;
        NumeroDeVoltas = Random.Range(2, 6);
        yield return new WaitForSeconds(0.5f);       
        novoPosicao = Random.Range(1, 5);
        evade[1] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2.3f);
        evade[2] = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 2.3f);
        evade[3] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 2.3f);
        evade[4] = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 2.3f);
             
        for(int v =0;v < NumeroDeVoltas;v++)
        {
            yield return new WaitForSeconds(0.5f);
            novoPosicao = Random.Range(1, 5);
            evade[1] = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2.3f);
            evade[2] = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 2.3f);
            evade[3] = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 2.3f);
            evade[4] = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 2.3f);
        }
    } 
    void Evade()
    {       
       nave.destination = evade[novoPosicao];  
       StartCoroutine(BackToAtaque());
       
    }
    IEnumerator BackToAtaque()// retorna O movimento de ataque
    {
        yield return new WaitForSeconds(2.0f);
        if (Tipo_tinimigo != 2 && Tipo_tinimigo != 5)
        {
            yield return new WaitForSeconds(6.0f);
        }
        novoPosicao = 0;
        Move_on_Ataque = true;
    }    
    #region funçoues de Combate
    void Ataque()
    {      
        Npc_Anim.SetBool("Ataq", true);    
    }  
    void Rotate_To_Target() //Força o NPC a Girar Para o Player
    {
        if (caido == true) Look_To_Player = false;
        if (caido == false) Look_To_Player = true;
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
        if (caido == false)
        {
            int hitLocal = Random.Range(0, 3);//Mecanica Não Aplicada
            int hitBrut = Random.Range(0, 4);//Chamce do Bruto ter Animacao de Hit

            if (Tipo_tinimigo != 2 && Tipo_tinimigo != 5)
            {
                Npc_Anim.SetBool("Hit", true);
                nave.speed = 0;
            }
            if (Tipo_tinimigo == 2 || Tipo_tinimigo == 5)
            {
                if (hitBrut >= 3) Npc_Anim.SetBool("Hit", true);
            }
            Npc_Anim.SetInteger("AreaHit", hitLocal);
            StartCoroutine(recuperar());
            Life -= dano;
            CorHit();
        }
    }
    void CorHit()// ao ser atingido Altera a cor
    {
        for (int c = 0; c < CorOriginal.Length; c++)
        {
            CorOriginal[c].material.color = new Vector4(1, 0, 0, 1);
        }
        StartCoroutine(restaurarCor());
    }
    IEnumerator restaurarCor()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        for (int c = 0; c < CorOriginal.Length; c++)
        {      
            CorOriginal[c].material.color =  cores[c];
        }
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
    }
    IEnumerator recuperar()
    {
        yield return new WaitForSeconds(1.0f);
        Npc_Anim.SetBool("Hit", false);      
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
    void Deaf_check_Fix() // previnir Bug de quando o NPC Morre
    {
        if(Life <= 0)
        {
            Npc_Anim.SetBool("died", true);
        }
    }
    void Morrer_Event_FIX() // Evita que o NPC fique preso em Loop de morte
    {
        dead = true;
        Npc_Anim.SetBool("died", false);
        nave.enabled = false;
        for (int c = 0; c < CorOriginal.Length; c++)
        {
            CorOriginal[c].material.color = cores[c];
        }
        Destroy(gameObject, 10.0f);
    }
    #endregion
}


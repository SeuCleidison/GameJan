using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private Animator anin;
    private CharacterController character_Controlhe;
    [SerializeField]
    private int Char_ID;
    #region Var Movimeto
    [SerializeField]
    private float speed, junpSpeed;
    private float YSpeed, originalStepOff;
    float horizontalInput;
    float verticalInput;
    float magnitude;
    Vector3 movementDirection;
    Vector3 velocity;
    Vector3 V_rotatao;
    [SerializeField]
    private CapsuleCollider cap;
    #endregion

    #region varLutas
    [SerializeField]
    private float Life = 100.0f, lifeMax = 100.0f;
    private bool combo;
    private bool BlockJunp = false;
    private bool BlockSpin = false;
    private bool dead = false;
    [SerializeField]
    private GameObject Shoot; //tiro do Payer 2
    private float VelNormal;
    [SerializeField]
    GameObject MaoDireita, MaoEsqueda, Pe_direito, Pe_esquerdo;
    float cadenciaTiro = 1;// apenas Para Player2
    #endregion


    private void Awake()
    {
        transform.tag = "Player";
    }
    void Start()
    {
        character_Controlhe = GetComponent<CharacterController>();
        anin = GetComponent<Animator>();
        originalStepOff = character_Controlhe.stepOffset;
        VelNormal = speed;
        BlockJunp = false;
        FimAtaque();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            Movimentacao();
            Animacoes();
            Atacar();
        }
        if(Life <= 0 && dead == false)
        {
            anin.SetBool("vivo", false);
            Morto();
            dead = true;
        }
    }
    void Movimentacao()
    {

        horizontalInput = (Input.GetAxis("Horizontal") * speed);
        verticalInput = (Input.GetAxis("Vertical") * speed);
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        magnitude = Mathf.Clamp01(movementDirection.magnitude) * junpSpeed;
        if (horizontalInput < 0 && BlockSpin == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 270.0f, 0)), 20.0f * Time.deltaTime);
        }
        if (horizontalInput > 0 && BlockSpin == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 90.0f, 0)), 20.0f * Time.deltaTime);
        }
        if (verticalInput > 0 && BlockSpin == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0.0f, 0)), 20.0f * Time.deltaTime);
        }
        if (verticalInput < 0 && BlockSpin == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 180.0f, 0)), 20.0f * Time.deltaTime);
        }
        YSpeed += Physics.gravity.y * Time.deltaTime * 2;
        if (character_Controlhe.isGrounded)
         {
                character_Controlhe.stepOffset = originalStepOff;
                YSpeed = -0.75f;
             if (Input.GetKeyDown(KeyCode.Space) && BlockJunp == false)
                {
                    YSpeed = junpSpeed;
                }
         }
            else
            {
                character_Controlhe.stepOffset = 0;
            }
            velocity = movementDirection * magnitude;
            velocity.y = YSpeed;
            character_Controlhe.Move(velocity * Time.deltaTime);
        }
    void Animacoes()
    {
        anin.SetFloat("Movimentacao",(Mathf.Abs(horizontalInput )+ Mathf.Abs(verticalInput)));
        anin.SetBool("Ground", character_Controlhe.isGrounded);
        //Movimentacao  Ground
    }
   
    void Change_Char(int c)
    {
        Char_ID = c;
    }

    #region void de Combate

    void Atacar()
    {
        //Comando_Player_1
        if (Char_ID == 0)
        {
            if (Input.GetMouseButton(0))
            {
                BlockJunp = true;
                anin.SetBool("Atak", true);
            }
            if (Input.GetMouseButton(0) && combo == true)
            {
                anin.SetTrigger("Next_ataq");
                combo = false;
            }
        }
        
        if(cadenciaTiro < 0.6f)
        {
            cadenciaTiro += 2.0f * Time.deltaTime;
        }
        //Comando_Player_2
        if( Char_ID == 1)
        { 
            if (Input.GetMouseButton(0) && cadenciaTiro >= 0.5f)
            {
                Instantiate(Shoot, MaoDireita.transform.position, transform.rotation);
                cadenciaTiro = 0;
            }
        }
    }
    void AtaqueStatus()
    {        
        speed = (VelNormal / 3);       
    }
    void AtaqueMaoDireita()
    {
        if(MaoDireita)
        {
            MaoDireita.SetActive(true);
        }    
        if (MaoEsqueda) MaoEsqueda.SetActive(false);
        if (Pe_direito) Pe_direito.SetActive(false);
        if (Pe_esquerdo) Pe_esquerdo.SetActive(false);
    }
    void AtaqueMaoEsqueda()
    {
        if (MaoEsqueda)
        {
            MaoEsqueda.SetActive(true);
        }
        if (MaoDireita) MaoDireita.SetActive(false);
        if (Pe_direito) Pe_direito.SetActive(false);
        if (Pe_esquerdo) Pe_esquerdo.SetActive(false);
    }   
    void AtaqueChuteDireito()
    {
        if (Pe_direito)
        {
            Pe_direito.SetActive(true);
        }
        if (MaoDireita) MaoDireita.SetActive(false);
        if (MaoEsqueda) MaoEsqueda.SetActive(false);
        if (Pe_esquerdo) Pe_esquerdo.SetActive(false);
    }
    void AtaqueChuteesquerdo()
    {
        if (Pe_esquerdo)
        {
            Pe_esquerdo.SetActive(true);
        }
        if (MaoDireita) MaoDireita.SetActive(false);
        if (MaoEsqueda) MaoEsqueda.SetActive(false);
        if (Pe_direito) Pe_direito.SetActive(false);    
    }
    void FimAtaque()// Desativa os Colisores de Ataque 
    {
        MaoDireita.SetActive(false);
        MaoEsqueda.SetActive(false);
        Pe_direito.SetActive(false);
        Pe_esquerdo.SetActive(false);
    } 
    void combo_ataque()
    {
        combo = true;
    }
    void end_combo()
    {
        combo = false;
    }
    void endAtaque()
    {
        BlockSpin = false;
        end_combo();
        anin.SetBool("Atak", false);
        Arero_fim();
        FimAtaque();
    }
    void NomalSpeed()
    {
        BlockSpin = false;
        BlockJunp = false;
        speed = VelNormal;
    }
    void Arero()
    {
        anin.SetBool("AeroAtaq", true);
    }
    void Arero_fim()
    {
        anin.SetBool("AeroAtaq", false);
    }
    void DestravarTravaSpin()
    {
        BlockSpin = false;
    }
    void TravaSpin()
    {
        BlockSpin = true;
    }
    public void hit(float dano = 0)
    {
        FimAtaque();
        Life -= dano;
        anin.SetBool("Hit", true);
    }
    void Endhit()
    {
        anin.SetBool("Hit", false);
    }
    void Morto()
    {   
        if (cap) cap.enabled = false;
        anin.SetBool("died", true);
        anin.SetBool("vivo", true);
        StopAllCoroutines();     
        transform.tag = "Morto";
    }

    void Morrer_Event_FIX() // Evita que o NPC fique preso em Loop de morte
    {
        anin.SetBool("died", false);
    }
    #endregion

    
}




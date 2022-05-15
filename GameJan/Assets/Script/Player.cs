using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private Animator anin;
    private CharacterController character_Controlhe;

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
    #endregion
    #region varLutas
    private bool combo;
    private bool BlockJunp = false;
    private bool BlockSpin = false;
    private float VelNormal;
    [SerializeField]
    GameObject MaoDireita, MaoEsqueda, Pe_direito, Pe_esquerdo;
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
        Movimentacao();
        Animacoes();
        Atacar();
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
    #region voiddeCombate
    void Atacar()
    {
        if(Input.GetMouseButton(0))
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
    }
    void AtaqueMaoEsqueda()
    {
        if (MaoDireita)
        {
            MaoEsqueda.SetActive(true);
        }
    }
   
    void AtaqueChuteDireito()
    {
        if (MaoDireita)
        {
            Pe_direito.SetActive(true);
        } 
    }
    void AtaqueChuteesquerdo()
    {
        if (MaoDireita)
        {
            Pe_esquerdo.SetActive(true);
        }
    }
    void FimAtaque()
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
    #endregion
}




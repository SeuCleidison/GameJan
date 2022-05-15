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
    private Animator Npc_Anim;
    private NavMeshAgent nave;
    private GameObject Player;
    private float SpeedMove;
    private Rigidbody rig;
    void Start()
    {
        Npc_Anim = GetComponent<Animator>();
        nave = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        SpeedMove = nave.speed;
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
       if(Player)
        {
            Movimento();
        }

        if (!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void Movimento()
    {
        nave.destination = Player.transform.position;
        if(nave.remainingDistance > nave.stoppingDistance)
        {
            Npc_Anim.SetFloat("Movimento",1.0f);
        }
        if (nave.remainingDistance <= nave.stoppingDistance)
        {
            Npc_Anim.SetFloat("Movimento", 0f);
        }
    }
    #region fucouesdeCombate
    public void hit()
    {
        int hitLocal = Random.Range(0, 3);
        Npc_Anim.SetBool("Hit", true);
        Npc_Anim.SetInteger("AreaHit", hitLocal);
        nave.speed = 0;
        StartCoroutine(recuperar());
      
    }
    void Endhit()
    {
        Npc_Anim.SetBool("Hit", false); 
    }
    IEnumerator recuperar()
    {
        yield return new WaitForSeconds(1.0f);
        Npc_Anim.SetBool("Hit", false);
        nave.speed = SpeedMove;
    }
    #endregion
}


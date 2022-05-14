using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class NPC : MonoBehaviour
{
    [SerializeField]
    private GameObject Cabeca;
    [SerializeField]
    private GameObject Peito;
    [SerializeField]
    private GameObject Virilha;
    [SerializeField]
    private GameObject Pernas;
    private Animator Npc_Anim;
    void Start()
    {
        Npc_Anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit()
    {
        int hitLocal = Random.Range(0, 3);
        Npc_Anim.SetBool("Hit", true);
        Npc_Anim.SetInteger("AreaHit", hitLocal);
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
    }
}


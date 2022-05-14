using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dano : MonoBehaviour
{
    // Start is called before the first frame update
    NPC npc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject.CompareTag ("Inimigo"))
        {
            npc = other.GetComponent<NPC>();
            npc.hit();
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_cena : MonoBehaviour
{
    [SerializeField]
    private GameObject player_1;
    void Start()
    {
        instaciar();
    }
    void instaciar()
    {
        if(player_1)Instantiate(player_1,transform.position,transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

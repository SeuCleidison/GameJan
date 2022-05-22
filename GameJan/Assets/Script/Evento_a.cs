using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evento_a : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Player plaey;
        if (other.gameObject.CompareTag("Player"))
        {
            plaey = other.GetComponent<Player>();
            plaey.AbreDialogo = true;
            Camera_Control.canCont.cena = true;
            MenuMae.mae.AbreDial();
            Destroy(gameObject, 0.1f);
        }
    }

}

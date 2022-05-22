using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSimple : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float SpeedUp = 5.0f,TempoMover = 5.0f;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TempoMover > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + SpeedUp * Time.deltaTime, transform.position.z);
            TempoMover -= 1 * Time.deltaTime;
        }
    }
    public void CarregarCena(string id)//Play
    {
        Time.timeScale = 1.0f;
        LOAD.loadScena(id);
    }
}

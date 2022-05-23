using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroi : MonoBehaviour
{
    [SerializeField]
    private float Tempo = 5.0f;
    [SerializeField]
    private float Habilitar = 5.0f;
    [SerializeField]
    private GameObject Obj;
    void Start()
    {
        Destroy(gameObject, Tempo);
        StartCoroutine(habilitar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator habilitar()
    {
        yield return new WaitForSeconds(Habilitar);
        if(Obj)
        {
            Obj.SetActive(true);
        }
    }
}

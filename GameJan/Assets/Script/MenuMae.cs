using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuMae : MonoBehaviour
{
    static public MenuMae mae;
   [SerializeField]
    private bool PauseGame = false;
    [SerializeField]
    private Image BarraLife_p1, BarraLife_peq1, BarraLife_p2, BarraLife_peq2; 
     public float Life_P1 = 100.0f,Life_P2 = 100.0f;
     private float partialLife_P1 = 1.0f;
     private float partialLife_P2 = 1.0f;
    [SerializeField]
    private Mask M_1, M_2;
    public int ID_char;
    private void Awake()
    {
        mae = this;
    }
    void Start()
    {      
        Time.timeScale = 1.0f;
        partialLife_P1 = BarraLife_p1.fillAmount / Life_P1;
        partialLife_P2 = BarraLife_p2.fillAmount / Life_P2;
        AttMask(0);
    }
    // Update is called once per frame
    void Update()
    {

        BarraLife_p1.fillAmount = partialLife_P1 * Life_P1;
        BarraLife_p2.fillAmount = partialLife_P2 * Life_P2;
        BarraLife_peq1.fillAmount = partialLife_P1 * Life_P1;
        BarraLife_peq2.fillAmount = partialLife_P2 * Life_P2;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame = !PauseGame;
            Pause();          
        }
    }
    public void AttMask(int m = 0)
    {
        if(m == 0)
        {
            M_1.enabled = true;
            M_2.enabled = false;
        }
        if (m == 1)
        {
            M_1.enabled = false;
            M_2.enabled = true;
        }
    }
    void Pause()
    {
        if (PauseGame) Time.timeScale = 0.0f;
        if (!PauseGame) Time.timeScale = 1.0f;
    }
}

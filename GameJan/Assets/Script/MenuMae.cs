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
    [SerializeField]
    private GameObject Dialg, Foto_1, Foto_2;
    [SerializeField]
    Text Dialogo;
    [SerializeField]
    string Mensage;
    [SerializeField]
    private int VlMensage;
    [SerializeField]
    private string Cena;
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
        if(Life_P1 <= 0 || Life_P1 <= 1)
        {
            gameOver();
        }
    }
    private void gameOver()
    {
        Time.timeScale = 1.0f;
        LOAD.loadScena(Cena);
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
    public void AbreDial()
    {
      
        StartCoroutine(statDialg());     
    }
    IEnumerator statDialg()
    {
        yield return new WaitForSeconds(3.0f);
        Dialg.SetActive(true);
        Dialogo_Level();
       
    }
    public void Dialogo_Level()
    {
        Dialg.SetActive(true);
        VlMensage ++;
        Dialogo.text = Mensage;

         if (VlMensage == 1 )
        {
            Mensage = "_ Quem é Voce?";
            Dialogo.text = Mensage;           
            Foto_1.SetActive(true);
            Foto_2.SetActive(false);
        }
        if (VlMensage == 2)
        {       
            Mensage = "_ Eu Sou Você, Mas de um universo paralelo. ";
            Dialogo.text = Mensage;
            Foto_1.SetActive(false);
            Foto_2.SetActive(true);
        }
        if (VlMensage == 3)
        {          
            Mensage = "_ Eu Sou Você, Mas de um universo paralelo. ";
            Dialogo.text = Mensage;
            Foto_1.SetActive(false);
            Foto_2.SetActive(true);
        }
        if (VlMensage == 4)
        {           
            Mensage = "_ Esses Terroristas Causaram o caos e varias mortes em MightCyty do meu Universo .  ";
            Dialogo.text = Mensage;
            Foto_1.SetActive(false);
            Foto_2.SetActive(true);
        }
        if (VlMensage == 5)
        {            
            Mensage = "_ Eu vou ajudar você a salvar sua  cidade e levarei eles a para enfrentarem a justiça por seus Crimes.";
            Dialogo.text = Mensage;
            Foto_1.SetActive(false);
            Foto_2.SetActive(true);
        }
        if(VlMensage == 6)
        {
            Dialogo.text = Mensage;
            Dialg.SetActive(false);
            Camera_Control.canCont.cena = false;
            StartCoroutine(Cena2());
        }

    }
    IEnumerator Cena2()
    {
        yield return new WaitForSeconds(5.0f);
        Time.timeScale = 1.0f;
        LOAD.loadScena("Area2");

    }
    void Pause()
    {
        if (PauseGame) Time.timeScale = 0.0f;
        if (!PauseGame) Time.timeScale = 1.0f;
    }
}

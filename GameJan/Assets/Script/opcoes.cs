using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class opcoes : MonoBehaviour
{
    #region variaveis Grafica
    private Resolution[] resolucoesSuportadas; // lista das Resolucoes Suportadas;
    [SerializeField]
    private Dropdown resol_Drop; // Dropdown da Resolucao
    [SerializeField]
    private Dropdown Qualidade; // Dropdown da Qualidade
    [SerializeField]
    private Slider Master_Son; // Volume audio
    private int resolucaoSalveIndex;
    private bool telaCheiaAtivada;
    #endregion
    [SerializeField]
    bool isTitle = false;
    [SerializeField]
    private GameObject Bt_Title, BtOpicoes;
    void Awake()
    {
        resolucoesSuportadas = Screen.resolutions;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("RESOLUCAO"))
        {
            int numResoluc = PlayerPrefs.GetInt("RESOLUCAO");
            if (resolucoesSuportadas.Length <= numResoluc)
            {
                PlayerPrefs.DeleteKey("RESOLUCAO");
            }
        }

        if (PlayerPrefs.HasKey("RESOLUCAO"))
        {
            resolucaoSalveIndex = PlayerPrefs.GetInt("RESOLUCAO");
            Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
            resol_Drop.value = resolucaoSalveIndex;
        }
        else
        {
            resolucaoSalveIndex = (resolucoesSuportadas.Length - 1);
            Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
            PlayerPrefs.SetInt("RESOLUCAO", resolucaoSalveIndex);
            resol_Drop.value = resolucaoSalveIndex;
        }
        ChecarResolucoes();
        Qualidades_void();
        if(isTitle == false && Bt_Title)
        {
            Bt_Title.SetActive(false);
        }
    }
    #region Ajustes Graficos e Som
    void ChecarResolucoes()
    {
        Resolution[] resolucoesSuportadas = Screen.resolutions;
        resol_Drop.options.Clear();
        for (int y = 0; y < resolucoesSuportadas.Length; y++)
        {
            resol_Drop.options.Add(new Dropdown.OptionData() { text = resolucoesSuportadas[y].width + "x" + resolucoesSuportadas[y].height });
        }
        resol_Drop.captionText.text = "Resolucao";
    }
    public void Qualidades_void()
    {

        QualitySettings.SetQualityLevel(Qualidade.value);
    }
    #endregion
    #region Funcoes Titulo
    public void CarregarCena(string id)//Play
    {
        Time.timeScale = 1.0f;
        LOAD.loadScena(id);
    }
    public void Opicoes(bool b)
    {
        if (BtOpicoes)
        {
            BtOpicoes.SetActive(b);
        }
        else
            Debug.Log("Setar Menu Opcoes");

    }
    public void FecharJogo()
    {
        Application.Quit();
    }
    #endregion
    private void Update()
    {
        if (isTitle == false)
        {
            if (Time.timeScale == 0.0f)
            {
                BtOpicoes.SetActive(true);
            }
            if (Time.timeScale == 1.0f)
            {
                BtOpicoes.SetActive(false);
            }
        }
    }
}

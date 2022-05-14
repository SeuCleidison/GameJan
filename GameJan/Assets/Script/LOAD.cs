using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LOAD : MonoBehaviour
{
    [SerializeField]
    private Text percent;
    [SerializeField]
    private Image foreground;
    // Start is called before the first frame update
    void Start()
    {
        string scenaNome = PlayerPrefs.GetString("Scene_to_load", "Titulo");
        PlayerPrefs.SetString("Scene_to_load", "Titulo");
        PlayerPrefs.Save();
        percent.text = "0%";
        foreground.fillAmount = 0;
        StartCoroutine(LoadSceneAsync(scenaNome));
        // MenuMae.MaeMenu.pausar = false;
    }
    public static void loadScena(string scenaNome)
    {
        PlayerPrefs.SetString("Scene_to_load", scenaNome);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Load");
    }
    IEnumerator LoadSceneAsync(string scenaNome)
    {
        yield return new WaitForSeconds(0.5f);
        System.GC.Collect();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenaNome);
        while (!asyncLoad.isDone)
        {
            percent.text = (asyncLoad.progress * 100).ToString("n0") + "%";
            foreground.fillAmount = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
            // yield return null;
        }
    }
}

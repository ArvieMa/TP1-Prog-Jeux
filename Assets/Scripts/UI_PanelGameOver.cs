using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UI_PanelGameOver : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void Quit()
    {
        //MKéthode de contournement - A corriger au moment de la release
        if (Application.isEditor)   
            EditorApplication.ExitPlaymode();
         else
            Application.Quit();
    }
}

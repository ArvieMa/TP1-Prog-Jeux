using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;



    public HUDController HudController;
    public GameObject PanelGameOver;

    //Autres elements d'interfaces seront ajouter ici.




    private void Awake()
    {
        instance = this;
    }


}

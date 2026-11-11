using UnityEngine;

public class HP : MonoBehaviour
{
    private int currentHP;
    
    public void TakeDamage(int Damage)
    {
        currentHP = UIManager.instance.HudController.setHealthBar(Damage);
        if (currentHP <= 0) die();

    }
    private void die()
    {

        gameObject.GetComponent<PlayerController>().notifyDeath();
        gameObject.GetComponent<PlayerController>().enabled = false;

        UIManager.instance.PanelGameOver.SetActive(true);




    }
}

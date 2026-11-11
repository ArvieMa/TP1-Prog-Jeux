using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;
    public TMP_Text Score;

    public int maxHealth = 100;
    public int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        setHealthBar(0);

    }
    public void setScore(int points)
    {

        Score.text = "Score:" + points;
    }

    public int setHealthBar(int damage)
    {
        currentHealth -= damage;
        
        healthBarValueText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthBarSlider.value = currentHealth;
        healthBarSlider.maxValue = maxHealth;

        return currentHealth;
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }
}

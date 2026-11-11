using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;

    public int maxHealth = 100;
    public int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarValueText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthBarSlider.value = currentHealth;
        healthBarSlider.maxValue = maxHealth;
    }
}

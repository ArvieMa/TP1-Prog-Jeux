using UnityEngine;

public class Mimic : MonoBehaviour
{
    public int damage = 50;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTrigger2D - " +collision.gameObject.name);
        collision.GetComponent<HP>()?.TakeDamage(damage);

        Destroy(gameObject);
    }
}

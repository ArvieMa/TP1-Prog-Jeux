using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ChestScript : MonoBehaviour
{
    private int points = 100;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Score>()?.increaseScore(points);

        Destroy(gameObject);
    }


}

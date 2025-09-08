using UnityEngine;

public class Key : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    bool hasCollided = false;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            if(!hasCollided)
            {
                hasCollided = true;
                Debug.Log("Key collected!");
                gameManager.KeyCollected();
                Destroy(gameObject);
            }
        }
    }
}

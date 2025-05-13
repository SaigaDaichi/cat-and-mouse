using UnityEngine;

public class Cheese : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("チーーーーーーーーーーーズ！");
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;


public class CatArm : MonoBehaviour, IBlocker
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ねーーーーーーーーーーーーーーーーーーーこ");
        }

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.moveLimit--;
            Debug.Log("moveLimit 減らしたよ！今の残り: " + player.moveLimit);
        }
    }

    public void OnBlocked(PlayerController player)
    {
        if (player != null)
        {
            player.moveLimit--;
            player.moveLimit--;
            Debug.Log("moveLimit 減らしたよ！今の残り: " + player.moveLimit);
        }
    }

    //腕部分に噛まれた時
    public void Damage()
    {
        if(GetComponentInParent<CatWhite>())
        {
            StartCoroutine(GetComponentInParent<CatWhite>().Damage());
        }
    }
}

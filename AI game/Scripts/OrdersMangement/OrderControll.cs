using UnityEngine;

public class OrderControll : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isDelivering = false;

    [Header("Order")]
    public int order=1;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDelivering)
        {
            player playerScript = collision.GetComponent<player>();
            if (playerScript != null && playerScript.Pizza > 0)
            {
                StartCoroutine(DeliverPizzaAfterDelay(playerScript));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        isDelivering = false;
    }

    private System.Collections.IEnumerator DeliverPizzaAfterDelay(player playerScript)
    {
        isDelivering = true;

        yield return new WaitForSeconds(2f); // wait for 2 seconds

        if (playerScript.Pizza >= 0)
        {
            if(playerScript.Pizza>order)
            {
                playerScript.DeliveredPizza+=order;
                playerScript.Pizza-=order;
                order = 0;
                

            }
            else
            {
                playerScript.DeliveredPizza+=playerScript.Pizza;
                order-=playerScript.Pizza;
                playerScript.Pizza=0;

                // Set alpha to 0 (invisible)
                
                
            }
            if (order == 0)
            {
                this.gameObject.SetActive( false);
            }

        }
        else
        { 
            this.gameObject.SetActive(false);
        }
        playerScript.extra += 1;
        isDelivering = false;
    }
}

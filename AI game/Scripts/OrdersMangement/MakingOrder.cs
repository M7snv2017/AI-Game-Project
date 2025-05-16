using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(SpriteRenderer))]
public class MakingOrder : MonoBehaviour
{

    [Header("Sprites")]
    public Sprite rawSprite;     // Raw uncooked pizza sprite
    public Sprite readySprite;   // Fully cooked sprite

    [Header("Cooking Settings")]
    public float cookTime = 2f;          // Time to cook per pizza
    public float startCook = 1f;
    public int cookRate = 1;             // Number of pizzas cooked per session
    public orderManager or;

    [Header("State Flags")]
    public bool playerIsHere = false;
    public bool isCooking = false;
    public bool isReady = false;


    private SpriteRenderer spriteRenderer;
    private player playerScript;
    private float currentCookProgress = 0f; // From 0 to 1
    private Coroutine cookingCoroutine;
    private readonly Color startColor = new Color(0.1f, 0.1f, 0.1f, 0.6f); // Semi-dark and hidden
    private readonly Color endColor = new Color(1f, 1f, 1f, 1f); // Fully visible

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = rawSprite;
        spriteRenderer.color = startColor;
    }

    private void Update()
    {
        if (isCooking && !isReady)
        {
            float step = Time.deltaTime / cookTime;
            currentCookProgress = Mathf.Clamp01(currentCookProgress + step);

            // Update color based on progress
            spriteRenderer.color = Color.Lerp(startColor, endColor, currentCookProgress);

            if (currentCookProgress >= 1f)
            {
                CompleteCooking();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerScript = collision.GetComponent<player>();
        playerIsHere = true;

        if (!isCooking && !isReady)
        {
            StartCooking();
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsHere = false;

        StopAllCoroutines(); // Cancel delayed restart
        ResetCookingState();
    }

    private void StartCooking()
    {
        isCooking = true;
        currentCookProgress = 0f;
        spriteRenderer.sprite = rawSprite;
        spriteRenderer.color = startColor;
    }

    private void CompleteCooking()
    {
        isCooking = false;
        isReady = true;
        spriteRenderer.sprite = readySprite;
        spriteRenderer.color = endColor;
        Debug.Log("Pizza is ready!");
        if(playerIsHere)
        {
            GivePizzaToPlayer();
        }
    }

    private void GivePizzaToPlayer()
    {
        if (playerScript == null) return;

        playerScript.Pizza +=  (cookRate+or.currentPhase*15/10);
        isReady = false;

        cookingCoroutine = StartCoroutine(DelayedRestart());
        
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(startCook);
        ResetCookingState();
        cookingCoroutine = null;

        if (playerIsHere)
        {
            StartCooking();
        }
    }

    private void ResetCookingState()
    {
        isCooking = false;
        isReady = false;
        currentCookProgress = 0f;
        spriteRenderer.sprite = rawSprite;
        spriteRenderer.color = startColor;
        cookingCoroutine = null;
        Debug.Log("Player left. Cooking reset.");
    }
}

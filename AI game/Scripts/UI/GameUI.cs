using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI pizzaText;
    public TextMeshProUGUI deliveredText;
    public TextMeshProUGUI timerText;
    public GameObject extraObject;
    public TextMeshProUGUI extraText;

    public orderManager orderManager;
    public player playerScript;

    void Update()
    {
        phaseText.text = $"Phase: {orderManager.currentPhase}";
        pizzaText.text = $"{playerScript.Pizza}";
        deliveredText.text = $"{playerScript.DeliveredPizza}";
        timerText.text = $"{orderManager.phaseTimer:F2}s";

        if (playerScript.extra == 3)
        {
            extraText.text = $"+ {orderManager.extra:F2}s";
            extraObject.SetActive(true);
            Invoke(nameof(UnActive), 2f);
        }
    }
    void UnActive()
    {
        extraObject.SetActive(false);
    }
}

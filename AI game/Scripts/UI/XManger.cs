using UnityEngine;
using UnityEngine.SceneManagement;

public class XManger : MonoBehaviour
{
    public GameObject[] x;
    public orderManager or;
    public int pointer = -1;
    public player playerScript;
    void Update()
    {
        if (or.phaseMissedPoints >= 3)
        {
            Time.timeScale = 0f;
            playerScript.moveSpeed = 0;
            x[2].SetActive(true);
            if (Input.anyKey)
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 30f;
                playerScript.moveSpeed = 10;

            }
            //stop the game to start again
        }
        else if (or.phaseMissedPoints < 4 && or.phaseMissedPoints > 0) 
        {
            for (int i = 0; i < or.phaseMissedPoints; i++)
            {
                x[i].SetActive(true);
            }
        }else
        {
            for (int i = 0; i < 3; i++)
            {
                x[i].SetActive(false);
            }
        }
            

    }
}

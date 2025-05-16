using System;
using System.Collections.Generic;
using UnityEngine;

public class orderManager : MonoBehaviour
{
    [Header("Phase Settings")]
    public float phaseTime = 30f;
    public int initialPhasePoints = 2;
    public int ordersPerPoint = 1;

    [Header("Points")]
    public List<GameObject> points; // GameObjects that will hold orders
    public player playerScript;

    [Header("Debug")]
    public float phaseTimer;
    public int currentPhase = 0;
    public bool isPhaseActive = false;
    public int remainPoints = 0;
    public int phaseMissedPoints = 0;
    public List<GameObject> currentActivePoints = new List<GameObject>();
    public float extra;

    void Start()
    {
        StartPhase(initialPhasePoints);
    }

    void Update()
    {
        extra = 5f;
        if (extra < currentPhase) 
        {
            extra = ((float)Math.Pow(currentPhase,(float)(extra / 10))) / 2;
        }

        if (isPhaseActive)
        {
            if(playerScript.extra==3)
            {
                
                Debug.Log("extra time was " + phaseTimer + " to " + ( extra + phaseTimer));
                phaseTimer += extra;
                playerScript.extra = 0;

                phaseTimer -= Time.deltaTime;
            }
            else
            {
                phaseTimer -= Time.deltaTime;
            }
                

            if (phaseTimer <= 0)
            {
                EndCurrentPhase();
            }
            
        }
        
    }

    void StartPhase(int pointCount)
    {
        phaseTimer = phaseTime+currentPhase*phaseTime/phaseTime/6;
        isPhaseActive = true;
        currentPhase++;

        int activated = 0;
        currentActivePoints.Clear();

        List<int> indices = new List<int>();
        for (int i = 0; i < points.Count; i++) indices.Add(i);

        while (activated < pointCount && indices.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, indices.Count);
            GameObject point = points[indices[randIndex]];
            indices.RemoveAt(randIndex);

            if (!point.activeSelf)
            {
                point.SetActive(true);
            }

            OrderControll orderCtrl = point.GetComponent<OrderControll>();
            if (orderCtrl != null)
            {
                orderCtrl.order = ordersPerPoint;
                
                orderCtrl.enabled = true;
            }

            currentActivePoints.Add(point);
            activated++;
        }
    }

    void EndCurrentPhase()
    {
        foreach (GameObject point in currentActivePoints)
        {
            if (point.activeSelf)
            {
                remainPoints++;
                point.SetActive(false);
            }
            
        }
        if(remainPoints>0)
        {
            Debug.LogWarning("You missed " + remainPoints);
            phaseMissedPoints += 1;
        }
        else
        {
            if(phaseMissedPoints>0)
                phaseMissedPoints -= 1;
        }
        isPhaseActive = false;

        // Increase difficulty
        int nextPointCount = Mathf.Min(initialPhasePoints + currentPhase, points.Count);
        int nextOrderPerPoint = Mathf.Min(ordersPerPoint + (currentPhase / 2), 5);

        Invoke(nameof(StartNextPhase), 2f);
    }

    void StartNextPhase()
    {
        remainPoints = 0;
        StartPhase(Mathf.Min(initialPhasePoints + currentPhase, points.Count));
        ordersPerPoint++;
    }


    
}

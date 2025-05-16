using UnityEngine;

public class Goal : MonoBehaviour
{
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();  // Corrected: no "This.tranform"
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;  // If you're working in 2D, set Z to 0
            tr.position = mouseWorldPos;
        }
    }
}

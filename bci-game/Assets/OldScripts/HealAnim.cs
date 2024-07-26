using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAnim : MonoBehaviour
{
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        y = BaseY();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.y = Mathf.PingPong(Time.time * 0.5f, 0.3f) + y;
        transform.position = currentPos;
    }

    float BaseY()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Platform"))
            {
                float baseY = collider.gameObject.transform.position.y + 1f;
                return baseY;
            }
        }
        return -1;
    }
}

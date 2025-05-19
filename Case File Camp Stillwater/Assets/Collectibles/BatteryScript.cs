using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Battery picked up");
            GameManager.Instance.RechargeLight();
            Destroy(gameObject);
        }
    }
}

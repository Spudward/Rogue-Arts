using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehaviour : MonoBehaviour
{
    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Unit>().myCondition = Unit.Conditions.STUNNED;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public int dummy_Health = 100;

    [SerializeField]private Animator dummyAnims;

    public void TakeDamage(int damageAmount) { 
        dummy_Health -= damageAmount;
        if (dummy_Health < 0)
        {
            Die();
        }
    }
    private void Die() {
        Destroy(gameObject);
    }
}

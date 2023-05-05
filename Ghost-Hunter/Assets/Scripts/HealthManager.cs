using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public GameObject healthUI;
    public float invulnerableTime;
    
    private int currentHealth;
    private Image[] hearts;
    private bool invulnerable;

    private void Start()
    {
        currentHealth = maxHealth;
        hearts = healthUI.GetComponentsInChildren<Image>();
    }

    public void DecreaseHealth()
    {
        if (invulnerable) return;
        if (currentHealth == 1)
        {
            GameManager.GameOver();
        }

        
        StartCoroutine(InvulnerableTime());
        
        currentHealth--;
        for (int i = currentHealth; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Animator>().Play("LooseHP");
        }
        
    }

    IEnumerator InvulnerableTime()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }
}

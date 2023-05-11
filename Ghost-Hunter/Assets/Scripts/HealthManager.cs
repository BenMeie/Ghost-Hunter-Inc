using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private SpriteRenderer playerSprite;

    private void Start()
    {
        currentHealth = maxHealth;
        hearts = healthUI.GetComponentsInChildren<Image>();

        playerSprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }

    public void DecreaseHealth()
    {
        if (invulnerable) return;
        if (currentHealth == 1)
        {
            
            Debug.Log("Player died");
            GameManager.GameOver();
        }

        
        StartCoroutine(InvulnerableTime());
        
        currentHealth--;
        for (int i = currentHealth; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Animator>().Play("LooseHP");
        }


        playerSprite.DOColor(new Color(1, 0, 0), 0.2f);
        playerSprite.DOColor(new Color(1, 1, 1), 0.2f).SetDelay(0.3f);
        DOTween.Play(playerSprite);
        
    }

    IEnumerator InvulnerableTime()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScareController : MonoBehaviour
{
    public HealthManager healthManager;
    //doing this directly might not be the cleanest way
    //might make sense to route both of these through the gameMangaer
    public PostProcessing postProcessing; 

    public float alertRadius;
    public float warningRadius;
    public float deathRadius;
    public float distanceFromGhost;

    private GameObject ghost;
    private Ghost ghostScript;

    private void Start()
    {
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        ghostScript = ghost.GetComponent<Ghost>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostScript.updating)
        {
            FindDistanceFromGhost();
            if (distanceFromGhost < deathRadius)
            {
                Kill();
            }
            else if (distanceFromGhost < warningRadius)
            {
                Warn();
            }
            else if (distanceFromGhost < alertRadius)
            {
                Alert();
            }
            else
            {
                ClearAll();
            }
        }
    }

    private void FindDistanceFromGhost()
    {
        distanceFromGhost = Vector3.Distance(transform.position, ghost.transform.position);
        postProcessing.UpdateEffects(distanceFromGhost);
    }

    private void Alert()
    {
    }

    private void Warn()
    {
    }

    private void Kill()
    {
        //print("player big dead");
        healthManager.DecreaseHealth();
        postProcessing.PlayerDamaged();
    }

    private void ClearAll()
    {
        
    }

    private void OnDrawGizmos()
    {
        // var currPosition = transform.position;
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(currPosition, deathRadius);
        //
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(currPosition, warningRadius);
        //
        // Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(currPosition, alertRadius);
    }
}

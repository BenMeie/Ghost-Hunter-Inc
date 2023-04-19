using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform target;
    public GameObject ghostSprite;
    public int angerLevel = 0;

    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        ghostSprite.transform.rotation = Quaternion.identity;
    }

    public void IncreaseAnger()
    {
        IncreaseAnger(1);
    }

    public void IncreaseAnger(int by)
    {
        angerLevel += by;
    }
    
}

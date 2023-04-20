using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform target;
    public GameObject ghostSprite;
    public int angerLevel = 0;
    public bool updating = true;

    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        UpdateDifficulty();
        StartCoroutine(UpdateTarget());
    }

    // Update is called once per frame
    void Update()
    {
        ghostSprite.transform.rotation = Quaternion.identity;
    }

    public void IncreaseAnger(int by = 1)
    {
        angerLevel += by;
        UpdateDifficulty();
    }

    public void DecreaseAnger(int by = 1)
    {
        angerLevel -= by;
        UpdateDifficulty();
    }

    public void SetAnger(int to)
    {
        angerLevel = to;
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        switch (angerLevel)
        {
            case <5:
                agent.acceleration = 2.1f;
                agent.speed = 1.2f;
                agent.stoppingDistance = 3;
                break;
            case <10:
                agent.acceleration = 2.2f;
                agent.speed = 1.4f;
                agent.stoppingDistance = 3;
                break;
            case <15:
                agent.acceleration = 2.3f;
                agent.speed = 1.6f;
                agent.stoppingDistance = 2;
                break;
            case <20 :
                agent.acceleration = 2.4f;
                agent.speed = 1.8f;
                agent.stoppingDistance = 0;
                break;
            case <25 :
                agent.acceleration = 2.5f;
                agent.speed = 2f;
                break;
        }
    }

    IEnumerator UpdateTarget()
    {
        while (updating)
        {
            yield return new WaitForSeconds(0.5f);
            agent.SetDestination(target.position);
        }
    }
}

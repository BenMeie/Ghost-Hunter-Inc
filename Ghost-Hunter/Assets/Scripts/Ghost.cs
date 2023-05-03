using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Ghost : MonoBehaviour
{
    public GameManager gameManager;

    public Transform target;
    [FormerlySerializedAs("ghostSprite")] public GameObject spriteObject;
    public int angerLevel = 1;
    public int minAnger = 1;
    public bool updating = true;

    private NavMeshAgent agent;
    private Animator animController;
    private SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        animController = spriteObject.GetComponent<Animator>();
        sprite = spriteObject.GetComponent<SpriteRenderer>();
        
        UpdateDifficulty();
        StartCoroutine(UpdateTarget());
        StartCoroutine(Calm());
    }

    // Update is called once per frame
    void Update()
    {
        spriteObject.transform.rotation = Quaternion.identity;
        if (agent.velocity.magnitude > 1.0f)
        {
            animController.Play("Run");
        }
        else if (agent.velocity.magnitude > 0.0f)
        {
            animController.Play("Walk");
        }
        else
        {
            animController.Play("Idle");
        }

        if (agent.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else if (agent.velocity.x > 0)
        {
            sprite.flipX = false;
        }
    }

    void FoundMemento(int id)
    {
        IncreaseMinAnger();
    }

    public void IncreaseMinAnger(int by = 5)
    {
        minAnger += 5;
        if (angerLevel < minAnger)
        {
            SetAnger(minAnger);  
        }
    }

    public void IncreaseAnger(int by = 1)
    {
        angerLevel += by;
        UpdateDifficulty();
    }

    public void DecreaseAnger(int by = 1)
    {
        if (angerLevel <= minAnger) return;
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
        StopCoroutine(UpdateTarget());
        StartCoroutine(UpdateTarget());
    }

    IEnumerator UpdateTarget()
    {
        while (updating)
        {
            Vector3 targetPosition = target.position;
            Vector3 currentPosition = transform.position;
            yield return new WaitForSeconds(10.0f/angerLevel);
            int followChance = Random.Range(1, 11) * angerLevel;
            if (followChance > 60)
            {
                agent.SetDestination(targetPosition);
            }
            else if (followChance < 30 && angerLevel < 20)
            {
                agent.SetDestination(new Vector3(targetPosition.x + Random.Range(-10.0f, 10.0f),targetPosition.y + Random.Range(-10.0f, 10.0f), currentPosition.z));
            }
        }
    }

    IEnumerator Calm()
    {
        while (updating)
        {
            yield return new WaitForSeconds(1);
            DecreaseAnger();
        }
    }
}

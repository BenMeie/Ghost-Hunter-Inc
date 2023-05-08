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
    public bool stunned;

    public AudioClip[] ambientSounds;
    public AudioClip[] angerSounds;

    private NavMeshAgent agent;
    private Animator animController;
    private SpriteRenderer sprite;
    private AudioSource audioSource;
    private float angerTimer;
    private bool canGetAngry = true;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(target.position.x + Random.Range(-10.0f, 10.0f),target.position.y + Random.Range(-10.0f, 10.0f), target.position.z));
        animController = spriteObject.GetComponent<Animator>();
        sprite = spriteObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        angerTimer = 0.4f;
            
        UpdateDifficulty();
        StartCoroutine(UpdateTarget());
        StartCoroutine(Calm());
        StartCoroutine(playSound());
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
        if (!canGetAngry) return;
        StartCoroutine(angerDelay());
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
                agent.speed = 1.5f;
                agent.stoppingDistance = 3;
                break;
            case <10:
                agent.acceleration = 2.2f;
                agent.speed = 2.0f;
                agent.stoppingDistance = 3;
                break;
            case <15:
                agent.acceleration = 5f;
                agent.speed = 2.5f;
                agent.stoppingDistance = 2;
                break;
            case <20 :
                agent.acceleration = 10f;
                agent.speed = 3.0f;
                agent.stoppingDistance = 0;
                break;
            case <25 :
                agent.acceleration = 10f;
                agent.speed = 3.5f;
                break;
            default:
                agent.acceleration = 15f;
                agent.speed = 4.0f;
                break;
        }
        StopCoroutine(UpdateTarget());
        StartCoroutine(UpdateTarget());
    }

    public void Stun()
    {
        StopCoroutine(StunTimer());
        StartCoroutine(StunTimer());
    }

    IEnumerator UpdateTarget()
    {
        while (updating && !stunned)
        {
            Vector3 targetPosition = target.position;
            Vector3 currentPosition = transform.position;
            yield return new WaitForSeconds(8.0f/angerLevel);
            int followChance = Random.Range(1, 11) * angerLevel;
            if (followChance > 60)
            {
                agent.SetDestination(targetPosition);
            }
            else if (followChance < 30 && angerLevel < 20)
            {
                agent.SetDestination(new Vector3(targetPosition.x + Random.Range(-15.0f, 15.0f),targetPosition.y + Random.Range(-15.0f, 15.0f), currentPosition.z));
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

    IEnumerator StunTimer()
    {
        stunned = true;
        agent.speed = 0;
        agent.velocity = new Vector3();
        yield return new WaitForSeconds(2);
        stunned = false;
        UpdateDifficulty();
    }

    IEnumerator playSound()
    {
        while (updating)
        {
            yield return new WaitForSeconds(Random.Range(20, 50f));
            if (angerLevel > 15)
            {
                audioSource.clip = angerSounds[Random.Range(0, angerSounds.Length)];
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
            else
            {
                audioSource.clip = ambientSounds[Random.Range(0, ambientSounds.Length)];
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
        }
    }

    IEnumerator angerDelay()
    {
        canGetAngry = false;
        yield return new WaitForSeconds(angerTimer);
        canGetAngry = true;
    }
}

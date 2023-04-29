using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("General")]

    public GameManager gameManager;

    public Camera cam;
    Vector2 mousePos;

    private Vector2 lightPosition2D;
    private Vector2 lookDir;

    [Header("Flashlight")]
    public Light2D flashlight;
    private bool flashlightOn = true;
    private float battery = 100f;
    public float depleteRate = 1f;
    public float rechargeRate = 5f;

    //[Header("Mementos")]
    //public float mementoCheckingDistance = 1f;
    //public GameObject[] mementos;

    private GameObject ritual;
    private Ghost ghost;

    public delegate void MementoFound(int id);

    //public static event MementoFound onMementoFound;    
    
    // Start is called before the first frame update
    void Start()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        lightPosition2D = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        
        StartCoroutine(CheckLightCollision(lookDir));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0)){
            ToggleFlashlight();
        }

        //checking if there's anything interactable where the player is looking
        if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 1f))
        {
            if (hitInfo.collider.gameObject.CompareTag("Memento"))
            {
                gameManager.ShowInteractable(hitInfo.transform.position);

                if (Input.GetKeyDown("e"))
                {
                    //might not be the best way to do this
                    Memento memento = hitInfo.collider.GetComponent<Memento>();
                    gameManager.FindMemento(memento.id);
                }
            }
            
            //can add check for ritual or other interactable objects here
            
        } else {
            gameManager.HideInteractable();
        }
    }

    private void FixedUpdate()
    {   
        //finding light direction facing
        lightPosition2D = new Vector2(transform.position.x, transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (battery <= 0){
            ToggleFlashlight();
        }

        //can change to make recharging take a bit after or something
        if(flashlightOn){
            battery -= 1f;
        } else {
            battery += 5f;
        }
    }

    void ToggleFlashlight(){
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
        StopCoroutine(CheckLightCollision(lookDir));
        StartCoroutine(CheckLightCollision(lookDir));
    }

    IEnumerator CheckLightCollision(Vector2 lookDir)
    {
        while (flashlightOn)
        {
            if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 3.5f))
            {
                if (hitInfo.collider.gameObject.CompareTag("Ghost"))
                {
                    print("Found Ghost");
                    hitInfo.collider.gameObject.BroadcastMessage("IncreaseAnger", 1);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(lookDir.x, lookDir.y, 0) * 3.5f, 0.5f);
    }

    
    //this method will check if there's a memento nearby. If there is, it will send out an event
    //containing the id of the memento that was found
    // bool checkForMemento()
    // {
        
    //     Debug.Log($"There are currently {mementos.Length} mementos left");
        
    //     foreach (GameObject memento in mementos)
    //     {
    //         int mementoId;
            
    //         if (Vector2.Distance(transform.position, memento.transform.position) <=
    //             mementoCheckingDistance) //here we have to give a condition for if memento is close to the player
    //         {
    //             mementoId = memento.GetComponent<MementoController>().id;
    //             Debug.Log($"Interacted with memento number {mementoId}");
                
    //             //send out the event
    //             if (onMementoFound != null)
    //             {
    //                 onMementoFound(mementoId);
    //             }
                
    //             return true;
    //         }
    //     }
        

    //     return false;
    // }

    // void CheckForRitual()
    // {
    //     if (mementos.Length == 0)
    //     {
    //         print("all mementos found");
    //         print((transform.position - ritual.transform.position).magnitude);
    //         if ((transform.position - ritual.transform.position).magnitude < 2)
    //         {
    //             print("Game End");
    //             ghost.updating = false;
    //         }
    //     }
    // }

    // void findRemainingMementos()
    // {
    //     mementos = GameObject.FindGameObjectsWithTag("Memento");
    // }
}

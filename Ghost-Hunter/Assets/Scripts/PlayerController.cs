using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("General")]

    public GameManager gameManager;
    public Camera cam;
    
    [Header("Look Detection")]
    public float radius = 5f;
    [Range(1, 360)]public float angle = 15f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    private Vector2 lookDir;
    


    [Header("Flashlight")]
    public Light2D flashlight;
    private bool flashlightOn = true;
    private float battery = 100f;
    public float depleteRate = 1f;
    public float rechargeRate = 5f;
    public Image batteryBar;
    Vector2 mousePos;

    private Vector2 lightPosition2D;
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
        
        StartCoroutine(CheckFov(lookDir));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0)){
            ToggleFlashlight();
        }

        //checking if there's anything interactable where the player is looking
        // if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 1f))
        // {
        //     if (hitInfo.collider.gameObject.CompareTag("Memento"))
        //     {
        //         gameManager.ShowInteractable(hitInfo.transform.position);
        //
        //         if (Input.GetKeyDown("e"))
        //         {
        //             //might not be the best way to do this
        //             Memento memento = hitInfo.collider.GetComponent<Memento>();
        //             gameManager.FindMemento(memento);
        //         }
        //     }
        //     
        //     //can add check for ritual or other interactable objects here
        //     
        // } else {
        //     gameManager.HideInteractable();
        // }
        
    }

    private void FixedUpdate()
    {   
        //finding light direction facing
        lightPosition2D = new Vector2(flashlight.transform.position.x, flashlight.transform.position.y);
        lookDir = mousePos - lightPosition2D;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y ,lookDir.x) * Mathf.Rad2Deg - 90f;
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (battery <= 0){
            ToggleFlashlight();
        }

        //can change to make recharging take a bit after or something
        if(flashlightOn){
            battery -= 0.5f;
        } else if(battery < 100) {
            battery += 2f;
        }

        batteryBar.fillAmount = battery / 100.0f;
    }

    void ToggleFlashlight(){
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
        StopCoroutine(CheckFov(lookDir));
        StartCoroutine(CheckFov(lookDir));
    }

    IEnumerator CheckFov(Vector2 lookDir)
    {
        while (true)
        {
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
            // if (Physics.SphereCast(transform.position, 0.5f, new Vector3(lookDir.x, lookDir.y, 0), out var hitInfo, 3.5f))
            // {
            //     if (hitInfo.collider.gameObject.CompareTag("Ghost"))
            //     {
            //         print("Found Ghost");
            //         hitInfo.collider.gameObject.BroadcastMessage("IncreaseAnger", 1);
            //     }
            // }
            //
            // yield return new WaitForSeconds(0.2f);
            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 dirToTarget = (target.position - transform.position);

                if (Vector2.Angle(transform.up, dirToTarget) < angle / 2)
                {
                    float distToTarget = Vector2.Distance(target.position, transform.position);
                    if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstructionLayer))
                    {
                        
                    }
                }
            }
        }
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireCube(transform.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(lookDir.x, lookDir.y, 0) * 3.5f, 0.5f);
    }
}

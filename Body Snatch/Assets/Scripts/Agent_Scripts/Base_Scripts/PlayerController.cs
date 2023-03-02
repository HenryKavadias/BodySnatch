using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public Material playerMat;
    public float playerMoveSpeedMod = 0;

    // Universial variables
    protected AgentControl agentCtrl;
    protected GameObject lastAttacker;
    protected float playerSpeed;

    public GameObject lastAttackerRef
    {
        get => lastAttacker;
        set => lastAttacker = value;
    }
    void Start()
    {
        agentCtrl = gameObject.GetComponent<AgentControl>();

        if (agentCtrl != null)
        {
            agentCtrl.SetTags("Player");    
            agentCtrl.indicator.GetComponent<Renderer>().material = playerMat;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            playerSpeed = agentCtrl.moveSpeed + playerMoveSpeedMod;
        }
        else
        {
            this.enabled = false;
        }
    }

    protected virtual void PlayerControls()
    {
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.identity;
        PlayerMovement();
    }
    protected virtual void PlayerMovement()
    { }

    protected virtual void FaceMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
    }

    //protected virtual void PlayerAttack() { }

    protected virtual void Update()
    {
        PlayerControls();
    }

    //void inforceBoundary()
    //{
    //    gameObject.GetComponent<Rigidbody>().position = new Vector3
    //        (Mathf.Clamp(gameObject.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
    //        0.0f,
    //        Mathf.Clamp(gameObject.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax));
    //}
    //void playerControls()
    //{
    //    gameObject.GetComponent<Rigidbody>().rotation = Quaternion.identity;
    //    playerMovement();
    //}
    //void playerMovement()
    //{
    //    float moveHorizontal = Input.GetAxis("Horizontal");
    //    float moveVertical = Input.GetAxis("Vertical");

    //    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    //    gameObject.GetComponent<Rigidbody>().velocity = movement * agent.moveSpeed;

    //    gameObject.GetComponent<Rigidbody>().position = new Vector3
    //        (gameObject.GetComponent<Rigidbody>().position.x,
    //        0.0f,
    //        gameObject.GetComponent<Rigidbody>().position.z);

    //    if (enableBoundary)
    //    {
    //        inforceBoundary();
    //    }
    //}

    //void Update()
    //{
    //    playerControls();
    //}
}

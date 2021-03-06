﻿using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Animator))]

public class IsoCharacterController: MonoBehaviour
{

    private Transform myTransform;              // this transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition
    public float Speed;                         // Speed at which the character moves
    public float Direction;                    // The Speed the character will move
    public GameObject playerMesh;
//    public float moveAnimSpeed;                // Float trigger for Float created in Mecanim (Use this to trigger transitions.
//    public float animSpeed = 1.5f;            // Animation Speed
//    public Animator anim;                     // Animator to Anim converter
//    public int idleState = Animator.StringToHash("Base Layer.Idle"); // String to Hash conversion for Mecanim "Base Layer"
//    public int runState = Animator.StringToHash("Base Layer.Run");  // String to Hash for Mecanim "Base Layer Run"
//    private AnimatorStateInfo currentBaseState;         // a reference to the current state of the animator, used for base layer
    private Collider col;




    void Start()
    {

        Physics.gravity = new Vector3(0, -200f, 0); // used In conjunction with RigidBody for better Gravity (Freeze Rotation X,Y,Z), set mass and use Gravity)
//        anim = GetComponent<Animator>();
//        idleState = Animator.StringToHash("Idle"); // Duplicate added due to Bug
//        runState = Animator.StringToHash("Run");
        myTransform = transform;                            // sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;
        // prevents myTransform reset
    }

    void FixedUpdate()
    {

        // keep track of the distance between this gameObject and destinationPosition      

//        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        // Set's speed in reference to distance

        if (destinationDistance < .5f)
        {
            Speed = 0;
        }
        else if (destinationDistance > .5f)
        {
            Speed = 100;

            //Below sends Floats to Mecanim, Raycast set's speed to X until destination is reached animation is played until speed drops
        }

        if (Speed > .5f)
        {
//            anim.SetFloat("moveAnimSpeed", 2.0f);
        }

        else if (Speed < .5f)
        {
//            anim.SetFloat("moveAnimSpeed", 0.0f);
        } //


        // Moves the Player if the Left Mouse Button was clicked


        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 2;
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                Quaternion targetRotation = Quaternion.LookRotation(hit.point - transform.position);
                playerMesh.transform.rotation = targetRotation;
                destinationPosition = hit.point;
            }
        }  
        // Moves the player if the mouse button is hold down
        else if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 2;
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                Quaternion targetRotation = Quaternion.LookRotation(hit.point - transform.position);
                playerMesh.transform.rotation = targetRotation;
                destinationPosition = hit.point;
            }
        }

        // To prevent code from running if not needed
        if (destinationDistance > .1f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, Speed * Time.deltaTime);
        }
    }
}
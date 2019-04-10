using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class player : MonoBehaviour
{
    private float distanceToGround;
    public Animator anim;
    private NavMeshAgent myAgent;
    public AudioSource jump;



    // Start is called before the first frame update
    void Start()
    {
        jump = GetComponent<AudioSource>();
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + .05f);

        if (Input.GetKey(KeyCode.Space))
        {
            //GetComponentInChildren<Rigidbody>().AddForce(Vector3.up*50);
            
            anim.SetTrigger("Jump");
            jump.Play(34100);
        }

        // create a raycast hit object
        RaycastHit hit;
        // create a ray object to the point where the player clicked
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        anim.SetFloat("Speed", myAgent.velocity.magnitude);

        // check if the player clicked on something
        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            // set nav mesh to move towards the point the player clicked
            myAgent.destination = hit.point;
        }
    }
}

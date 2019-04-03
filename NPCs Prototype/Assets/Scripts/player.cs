using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private float distanceToGround;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + .05f);

        //if (Input.GetKey(KeyCode.Space) && isGrounded)
        //{
        //    GetComponent<Rigidbody>().AddForce(Vector3.up*200);
        //}

        float rotation = Input.GetAxis("Horizontal") * 240;
        float translation = Input.GetAxis("Vertical") * 5;
        rotation *= Time.deltaTime;
        translation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        transform.Translate(0, 0, translation);

        if (isGrounded) { anim.SetFloat("Speed", Mathf.Abs(translation * 20)); }
        else { anim.SetFloat("Speed", 0); }
         
        //GetComponent<Rigidbody>().AddForce(Vector3.forward * );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_MOVEMENT_christos : MonoBehaviour
{
    public CharacterController Controller;
    public float speed = 12f;
    public float runspeed = 17f;
    public float walkspeed = 12f;
    public float crouchspeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isgrounded;

    public Transform t_mesh;
    public bool IsCrouch = false;                
    public float LocalScaleY;                   
    public float ControllerHeight;


    // Update is called once per frame
    void Update()
    {
        isgrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);
        if (isgrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runspeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkspeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            t_mesh.localScale = new Vector3(1, LocalScaleY, 1);
            Controller.height = ControllerHeight;
            speed = crouchspeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            t_mesh.localScale = new Vector3(1, 1, 1);
            Controller.height = 2.8f;
            speed = walkspeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        Controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
    }
    
}

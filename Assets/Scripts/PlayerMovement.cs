using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // this script manages the movement of the player

    private InputSystem_Actions _playerControls;
    private InputAction moveAction;

    private Rigidbody rb;

    private float speed = 0;
    private float maxSpeed = 8f;
    private float acceleration = 12f;
    private float stoppingForce = 18f;

    private Vector3 movement;
    private Vector3 lastMovement;
    private float movementX;
    private float movementY;

    public bool playerCanMove;



    void Awake()
    {
        _playerControls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();

        playerCanMove = true;
    }


    private void OnEnable()
    {
        moveAction = _playerControls.Player.Move;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }



    private void Update()
    {
        if (playerCanMove)
        {
            movementX = moveAction.ReadValue<Vector2>().x;
            movementY = moveAction.ReadValue<Vector2>().y;
        }
        else
        {
            movement = Vector3.zero;
        }
    }


    private void FixedUpdate()
    {
        movement = new Vector3(movementX, 0.0f, movementY);
        movement.Normalize();

        if (movement.sqrMagnitude > 0.0f)
        {
            Accelerate();
        }
        else if (movement.sqrMagnitude == 0.0f)
        {
            Decelerate();
        }
    }



    // this gives the movement a fade in
    private void Accelerate()
    {
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        else if (speed > maxSpeed)
        {
            speed -= stoppingForce * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        rb.linearVelocity = movement * speed;

        lastMovement = movement;
    }

    // this gives the movement a fade out
    private void Decelerate()
    {
        speed -= stoppingForce * Time.deltaTime;

        rb.linearVelocity = lastMovement * speed;

        if (speed <= 0.0f)
        {
            speed = 0.0f;
            lastMovement = UnityEngine.Vector3.zero;
        }
    }

}

using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    // Force to apply when the player jumps.
    public Vector2 jumpForce = new Vector2(0, 450);

    // How fast we'll let the player move in the x axis.
    public float maxSpeed = 3.0f;

    // A modifier to the force applied.
    public float speed = 50.0f;

    // The force to apply that we will get for the player's movement.
    float _xMove;

    // Set to true when the player can jump.
    bool _shouldJump;

    bool _onGround;

    float _yPrevious;

    bool _collidingWall;

    void Movement()
    {
        // Get the player's movement (-1 for left, 1 for right, 0 for stationary).
        _xMove = Input.GetAxis("Horizontal");

        if (_collidingWall && !_onGround)
            _xMove = 0;

        var rigidbody = GetComponent<Rigidbody>();

        if (_xMove != 0)
        {
            // Setting player horizontal movement.
            var xSpeed = Mathf.Abs(_xMove*rigidbody.velocity.x);
            if (xSpeed < maxSpeed)
            {
                var movememntForce = new Vector3(1, 0, 0);
                movememntForce *= _xMove*speed;

                RaycastHit hit;
                if (!rigidbody.SweepTest(movememntForce, out hit, 0.05f))
                    rigidbody.AddForce(movememntForce);
            }

            // Check the speed limits...
            if (Mathf.Abs(rigidbody.velocity.x) > maxSpeed)
            {
                Vector2 newVelocity;

                newVelocity.x = Mathf.Sign(rigidbody.velocity.x)*maxSpeed;
                newVelocity.y = rigidbody.velocity.y;

                rigidbody.velocity = newVelocity;
            }
        }
        else
        {
            // If we're not moving, get slightly slower.
            var newVelocity = rigidbody.velocity;

            // Reduce the current speed by 10%;
            newVelocity.x *= 0.9f;

            rigidbody.velocity = newVelocity;
        }
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump"))
            _shouldJump = true;

        Debug.Log("Should Jump: " + _shouldJump);

        // If the player should jump...
        if (_shouldJump && _onGround)
        {
            GetComponent<Rigidbody>().AddForce(jumpForce);
            _shouldJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_onGround)
            _collidingWall = true;
    }

    void OnCollisionExit(Collision collision)
    {
        _collidingWall = false;
    }

    void FixedUpdate()
    {
        // Move the player left and right.
        Movement();

        // Sets the camera to center on the player's position.
        // Keeping the camera's original depth.
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    // Use this for initialization
    void Start()
    {
        _shouldJump = false;
        _xMove = 0.0f;
        _onGround = false;
        _yPrevious = Mathf.Floor(transform.position.y);
        _collidingWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if we're on the ground...
        CheckGrounded();

        // Have the player jump if they press the jump button.
        Jumping();
    }

    void CheckGrounded()
    {
        /*
            Check if the player is hitting something from
            the center of the object (origin) to slightly below
            the bottom of it (distance).
        */
        var distance = (GetComponent<CapsuleCollider>().height/2*transform.localScale.y) + .01f;
        var floorDirection = transform.TransformDirection(-Vector3.up);
        var origin = transform.position;

        if (!_onGround)
        {
            // Check if there is something directly below us...
            if (Physics.Raycast(origin, floorDirection, distance))
                _onGround = true;
        }
        // If we are currently grounded, are we falling down or jumping?
        else if ((Mathf.Floor(transform.position.y) != _yPrevious))
            _onGround = false;

        Debug.Log("OnGround: " + _onGround);
    }

    void OnDrawGizmos()
    {
        var rigidbody = GetComponent<Rigidbody>();
        Debug.DrawLine(transform.position, transform.position + rigidbody.velocity, Color.red);
    }
}
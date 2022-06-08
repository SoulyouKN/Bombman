using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public GlobalStateManager globalManager;

    [Range (1, 2)] 
    public int playerNumber = 1;
    public float moveSpeed = 5f;
    public bool canDropBombs = true;
    public bool canMove = true;
    public bool dead = false;

    public GameObject bombPrefab;

    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateMovement ();
    }

    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false);
        if (!canMove)
        { 
            return;
        }

        //Depending on the player number, use different input for moving
        if (playerNumber == 1)
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }

    private void UpdatePlayer1Movement ()
    {
        if (Input.GetKey (KeyCode.W))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.A))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.S))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.D))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
        { //Drop bomb
            DropBomb ();
        }
    }
		
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
        {
            DropBomb ();
        }
    }


    private void DropBomb ()
    {
        if (bombPrefab)
        {
            Instantiate (bombPrefab,
                new Vector3 (Mathf.RoundToInt (myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt (myTransform.position.z)),
                bombPrefab.transform.rotation);
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (!dead && other.CompareTag ("Explosion"))
        { 
            Debug.Log ("P" + playerNumber + " hit by explosion!");

            dead = true; //Notify global state manager that this player died
            Destroy (gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;

    // Threshold for when the player falls off the board
    public float fallThreshold = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        setCountText();
        winTextObject.SetActive(false);
    }

    // FixedUpdate is called at a fixed interval
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxis("Horizontal"); // Using old input system
        movementY = Input.GetAxis("Vertical");   // Using old input system

        CheckForFall();
    }

    // This method handles player movement using the new input system
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Check if the player falls below the fall threshold
    void CheckForFall()
    {
        if (transform.position.y < fallThreshold)
        {
            LoseGame();
        }
    }

    // Set the count text
    void setCountText()
    {
        countText.text = "Count : " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    // Triggered when player collides with a collectible
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
            count++;
            setCountText();
        }
    }

    // Triggered when player collides with an enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseGame();
        }
    }

    // Method to handle the lose condition
    void LoseGame()
    {
        // Destroy the current object (player)
        Destroy(gameObject);
        // Update the winText to display "You Lose!"
        winTextObject.gameObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
    }
}

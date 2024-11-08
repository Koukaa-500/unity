using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            movementX = Input.GetAxis("Horizontal");
            movementY = Input.GetAxis("Vertical");

            CheckForFall();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void CheckForFall()
    {
        if (transform.position.y < fallThreshold)
        {
            LoseGame();
        }
    }

    void SetCountText()
    {
        countText.text = "Count : " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("u lose");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            

            LoseGame();
        }
    }

    void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Set the "You Lose" message to red and activate it
        winTextObject.SetActive(true);
        TextMeshProUGUI winText = winTextObject.GetComponent<TextMeshProUGUI>();
        winText.text = "You Lose!";
        winText.color = Color.red;

        // Restart the game after a short delay
        Invoke("RestartGame", 2f);
    }


    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text lives;
    public Text loseText;

    public AudioSource musicSource;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    Animator anim;

    private int scoreValue = 0;
    private int livesValue = 3;
    private int nextSceneToLoad;
    private bool facingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.text = "";
        loseText.text = "";
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }


    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (vertMovement * speed != 0)
        {
            anim.SetInteger("State", 2);
        }

        else if (hozMovement * speed != 0)
        {
            anim.SetInteger("State", 1);
        }

        else
        {
            anim.SetInteger("State", 0);
        }

        if (scoreValue >= 4)
        {
            SceneManager.LoadScene(nextSceneToLoad);
        }

        if (livesValue <= 0)
        {
            Destroy(gameObject);
            loseText.text = "You lose!";
        }
        {
            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "WinningCoin")
        {
            Destroy(collision.collider.gameObject);
            winText.text = "You win! Game Created by Hunter Coad!";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);

            livesValue = livesValue - 1;
            lives.text = "Lives: " + livesValue.ToString();
        }


    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
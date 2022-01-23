using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float xLimit1, xLimit2, yLimit1, yLimit2;
    public ParticleSystem particles;
    public AudioClip[] soundEffects;
    public GameObject[] endGameText;
    public float timeLeft = 11;
    public Text timeText;
    AudioSource audioSource;
    private bool _canMove = true;
    private bool gameEnd;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameEnd)
            timeLeft = timeLeft - Time.deltaTime;
        timeText.text = (Mathf.RoundToInt(timeLeft)).ToString();
        if(timeLeft < 0 && !gameEnd)
        {   
            StartCoroutine(playerDeath());
            gameEnd = true;
        }
        if (Input.GetButtonDown("MoveX") && _canMove) 
        {
            float moveVal = 0;
            float timeVal = 0;
            if(Input.GetAxisRaw("Horizontal") > 0) 
            {
                moveVal = 1;
                if(transform.position.x == xLimit1)
                {
                    _canMove = false;
                    return;
                }
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                moveVal = -1;
                if (transform.position.x == xLimit2)
                {
                    _canMove = false;
                    return;
                }
            }

            transform.position = new Vector2(transform.position.x + moveVal, transform.position.y);
            audioSource.PlayOneShot(soundEffects[0]);
            _canMove = false;

        }
        if (Input.GetButtonDown("MoveY") && _canMove)
        {
            float moveVal = 0;
            float timeVal = 0;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                moveVal = 1;
                if (transform.position.y == yLimit1)
                {
                    _canMove = false;
                    return;
                }
            }
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (transform.position.y == yLimit2)
                {
                    _canMove = false;
                    return;
                }
                moveVal = -1;
                
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + moveVal);
            audioSource.PlayOneShot(soundEffects[0]);
            _canMove = false;

        }
        if (Input.GetAxisRaw("Horizontal") == 0 && !_canMove) 
        {
            _canMove = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Danger")
        {
            StartCoroutine(playerDeath());
        }
        if(collision.gameObject.tag == "Win")
        {
            audioSource.PlayOneShot(soundEffects[2]);
            particles.Play();
            StartCoroutine(playerWin());
        }
    }
    IEnumerator playerDeath()
    {
        audioSource.PlayOneShot(soundEffects[1]);
        GetComponent<SpriteRenderer>().color = Color.red;
        endGameText[1].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        gameEnd = true;
        Destroy(gameObject);
    }
    IEnumerator playerWin()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        endGameText[0].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        gameEnd = true;
        print("win");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float pullForce = 100f;
    private float onRotationSpeed = 0f;
    public float rotateSpeed = 3600f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    public float moveSpeed = 5f;
    private Rigidbody2D rb2d;
    private UIController uiControl;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private Vector2 startPosition;
    public GameObject HookedTower{
        get{return hookedTower;}
    }
    public GameObject ClosestTower{
        get{return closestTower;}
    }
    public void setHookedTower(GameObject tower)
    {
        hookedTower = tower;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        uiControl = GameObject.Find("Canvas").GetComponent<UIController>();
        myAudio = this.gameObject.GetComponent<AudioSource>();
        startPosition = this.transform.position;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!isCrashed)
            {
                //Play SFX
                myAudio.Play();
                rb2d.velocity = new Vector3(0f, 0f, 0f);
                rb2d.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("Levelclear!");
            uiControl.endGame();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = collision.gameObject;
            //Change tower color back to green as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            //Change tower color back to normal
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void restartPosition()
    {
        //Set to start position
        this.transform.position = startPosition;
        //Restart rotation
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        //Set isCrashed to false
        isCrashed = false;
        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
            hookedTower = null;
            isPulled = false;
        }
    }

    void Update()
    {
        //Move the object
        onRotationSpeed = 0f;
        rb2d.velocity = -transform.up * moveSpeed;
        if ((Input.GetKey(KeyCode.Z) || (Input.GetKey(KeyCode.X))) && !isPulled)
        {
            Debug.Log("Z Pressed, hooking the object ...");

            /*
            Choosing closestTower automatically and "hook" the player to tower
            if (closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }
             */


            if (hookedTower)
            {
                if(Input.GetKey(KeyCode.Z))
                {
                    movePlayer(rotateSpeed);
                }

                if(Input.GetKey(KeyCode.X))
                {
                    movePlayer(-rotateSpeed);
                }
                
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
        {
            rb2d.angularVelocity = 0.0f;
            isPulled = false;
        }


        if (isCrashed)
        {
            if (!myAudio.isPlaying)
            {
                //Restart scene
                Debug.Log("Restart");
                restartPosition();
            }
        }
        /*
        else
        {
            //Move the object
            rb2d.velocity = -transform.up * moveSpeed;
            rb2d.angularVelocity = 0f;
        } */
    }

    private void movePlayer(float tempRotateSpeed)
    {
            float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

            //Gravitation toward tower
            Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
            float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
            rb2d.AddForce(pullDirection * newPullForce);
            onRotationSpeed += tempRotateSpeed;
            rb2d.angularVelocity = -onRotationSpeed / distance;
            isPulled = true;
    }

}

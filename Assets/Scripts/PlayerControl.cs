using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update

    //horizontal movement
    float hMove;
    public float speed = 50f;

    //she make my body rigid
    Rigidbody2D myBody;

    //sprite moment
    SpriteRenderer myRend;

    //JUMPING NEG.
    bool grounded = false;
    public float castDist = 1f;

    public float jumpPower = 2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;

    bool jump = false;
    int jumpCount;

    //animator
    Animator myAnim;
    void Start()
    {
        jumpCount = 0;
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && (grounded || jumpCount <= 1))
        {
            myAnim.SetBool("jumping", true);
            jump = true;
        }
    

       /* if(hMove > 0.2f|| hMove < -0.2f)
        {
            myAnim.SetBool("running", true);
        }
        else
        {
            myAnim.SetBool("running", false);
        }*/

        if(hMove > 0.2f)
        {
            myAnim.SetBool("running", true);
            myRend.flipX = false;
        }
        else if(hMove < -0.2f)
        {
            myAnim.SetBool("running", true);
            myRend.flipX = true;
        }
        else
        {
            myAnim.SetBool("running", false);
        }

        if(transform.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = hMove * speed;

        if(jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount += 1;
            jump = false;
        }

        if(myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        }
        else if(myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        if(hit.collider != null && hit.transform.tag == "Ground") //or .tag and tag everything as ground
        {
            myAnim.SetBool("jumping", false);
            grounded = true;
            jumpCount= 0;
        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "teleporter")
        {
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update

    //horizontal movement
    float hMove;
    public float speed = 2f;

    //she make my body rigid
    Rigidbody2D myBody;

    //JUMPING NEG.
    bool grounded = false;
    public float castDist = 1f;

    public float jumpPower = 2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;

    bool jump = false;

    //animator
    Animator myAnim;
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && grounded)
        {
            myAnim.SetBool("jumping", true);
            jump = true;
        }
    

        if(hMove > 0.2f || hMove < -0.2f)
        {
            myAnim.SetBool("running", true);
        }
        else
        {
            myAnim.SetBool("running", false);
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = hMove * speed;

        if(jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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

        if(hit.collider != null && hit.transform.name == "Ground") //or .tag and tag everything as ground
        {
            myAnim.SetBool("jumping", false);
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);
    }
}

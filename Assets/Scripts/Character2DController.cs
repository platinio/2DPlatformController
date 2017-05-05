using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character2DController : MonoBehaviour 
{

    [Header("Player Settings")]
    public float jumpForce;
    public float speed;
    public float flippingRange;

    [Header("General Raycasting Settings")]
    public LayerMask collisionLayer;
    public float rayLegth;

    [Header("RayCasting 1 Settings")]
    public float offSetX;
    public float offSetY;
    public Color rayColor;


    [Header("RayCasting 2 Settings")]
    public float _offSetX;
    public float _offSetY;
    public Color _rayColor;
        

    private Rigidbody2D RB;
    private Animator anim;
    private bool running;
    private bool isFacingRight;
    private bool edgeWarning;

    private bool IsGrounded
    {
        get
        {
            bool hit = Physics2D.Raycast(new Vector2(transform.position.x + offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + offSetY), Vector2.down, rayLegth, collisionLayer);
            bool _hit = Physics2D.Raycast(new Vector2(transform.position.x + _offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + _offSetY), Vector2.down, rayLegth, collisionLayer);
                       

            return hit || _hit;
        }

    }

    public void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isFacingRight = true;

    }


    void Update()
    {
        //reset state
        running = false;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKey(KeyCode.LeftArrow))
            Move(-1);
        else if (Input.GetKey(KeyCode.RightArrow))
            Move(1);
       
        SetAnimParam();
    }

    private void Move(int dir)
    {
        //set state
        running = true;

        if (dir == 1 && !isFacingRight)
            Flip();
        else if (dir == -1 && isFacingRight)
            Flip();

        transform.Translate(Vector2.right * Time.deltaTime * speed * dir);
    }

    private void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;

        isFacingRight = !isFacingRight; 
    }

    public void Jump()
    {
      
        //si no esta en el suelo retornamos
        if (!IsGrounded)
            return;

        _Jump();
    }

    public void ForceJump()
    {
        _Jump();
    }

    private void _Jump()
    {
        RB.velocity = new Vector2(0, 0);
        RB.AddForce(new Vector2(0, jumpForce));
    }

    
    private void SetAnimParam()
    {


        anim.SetBool("IsGrounded", IsGrounded);
        anim.SetBool("IsFalling", RB.velocity.y < 0.1);
        anim.SetBool("IsJumping", RB.velocity.y > 0.1);
        anim.SetBool("IsRunning", running);
    }

    //
    void OnDrawGizmosSelected()
    {
        //draw ray 1
        Debug.DrawLine(new Vector2(transform.position.x + offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + offSetY), new Vector2(transform.position.x + offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + offSetY - rayLegth), Color.red);
        //draw ray 2
        Debug.DrawLine(new Vector2(transform.position.x + _offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + _offSetY), new Vector2(transform.position.x + _offSetX + (isFacingRight ? 0 : flippingRange), transform.position.y + _offSetY - rayLegth), Color.blue);

    }

}

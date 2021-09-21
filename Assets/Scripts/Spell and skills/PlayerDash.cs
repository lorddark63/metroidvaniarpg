using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private float currentDash;
    private bool canDash = true;
    public bool isDashing;
    public float dashDuration;
    public float dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentDash = dashDuration;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    public void Dash()
    {

        if(Input.GetKey(KeyCode.C) && canDash)
        {
            if(currentDash <= 0)
            {
                StopDash();
            }
            else
            {
                currentDash -= Time.deltaTime;
                //criar particulas
                
                 StartDash();
            }
        }

        if(Input.GetKeyUp(KeyCode.C))
        {
            print("end dash");
            isDashing = false;
            canDash = true;
            currentDash = dashDuration;
        }
    }

    public void StartDash()
    {
        isDashing = true;
        if(PlayerController.instance.isLookingRight)
        {
            rb.velocity = Vector2.right * dashSpeed;
        }
        else
        {
            rb.velocity = Vector2.left * dashSpeed;
        }
    }

    public void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof( Rigidbody2D))]
[RequireComponent (typeof( BoxCollider2D))]
public class Player : MonoBehaviour 
{
    [SerializeField]
    private string playerName = "Unnamed";
    public enum controlSet {keyboard, controller}
    public Settings playerSettings;
    [SerializeField]
    private PlayerArm playerArm;
   
    [System.Serializable]
    public class Settings
    {
        [Range(1,90)]
        public float maxAngleToJump;
        public float walkSpeed, airControlSpeed, jumpHeight, maxVelocity, collisionPadding; 
    }
    [System.Serializable]
    private class PlayerArm
    {
        [SerializeField]
        private GameObject arm;
      
        public void rotate (float rotation)
        {
            this.arm.transform.rotation = Quaternion.Euler (0, 0, rotation);
        }
    }
    public bool debug = false;

    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private bool canJump = false;

    void Awake ()
    {
        this.rigidbody = GetComponent<Rigidbody2D> ();
        this.collider = GetComponent<BoxCollider2D> ();
    }
    // Use this for initialization
    void Start () 
    {

    }

    // Update is called once per frame
    void Update () 
    {
        this.playerArm.rotate (Time.time);
        float movementSpeed = this.playerSettings.walkSpeed;
        if (!this.canJump)
        {
            movementSpeed = this.playerSettings.airControlSpeed;
        }
//        if (this.rigidbody.velocity.sqrMagnitude > this.maxVelocity)
//        {
//            
//            float speedDecrease = -(this.rigidbody.velocity.sqrMagnitude - this.maxVelocity);
//            Vector2 normalizedVelocity = this.rigidbody.velocity.normalized;
//            this.rigidbody.AddForce (normalizedVelocity * speedDecrease);
//            Debug.LogWarning ("TOO FAST" + this.rigidbody.velocity.sqrMagnitude);
//        }

        if (Input.GetAxisRaw ("Horizontal") < 0)
        {
            //this.rigidbody.AddForce (Vector2.left * movementSpeed, ForceMode2D.Impulse);
            transform.Translate (Vector2.left * Time.deltaTime * movementSpeed);
        } 
        else if (Input.GetAxisRaw ("Horizontal") > 0)
        {
            //this.rigidbody.AddForce (Vector2.right * movementSpeed, ForceMode2D.Impulse);
            transform.Translate (Vector2.right * Time.deltaTime * movementSpeed);    
        }

        if (Input.GetAxisRaw ("Vertical") > 0 && this.canJump)
        {
            Jump (this.playerSettings.jumpHeight);
            this.canJump = false;
        }
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        foreach (ContactPoint2D contactPoint in collision.contacts) {
            Debug.DrawRay(new Vector2(contactPoint.point.x, contactPoint.point.y), contactPoint.normal, Color.black);
            //this.Log ( ""+Vector2.Angle (contactPoint.normal, Vector2.up) );
            if (this.debug)
            {
                Vector3 point1 = new Vector3 (transform.position.x+1, 
                                     transform.position.y - (this.collider.bounds.extents.y - this.playerSettings.collisionPadding),
                                     0);
                Debug.DrawRay (point1, Vector3.left, Color.green);
            }
            if (contactPoint.point.y <= (transform.position.y - (this.collider.bounds.extents.y - this.playerSettings.collisionPadding))) 
            {
                if (Vector2.Angle (contactPoint.normal, Vector2.up) <= this.playerSettings.maxAngleToJump) 
                {
                    this.canJump = true;
                    this.Log ( "enablejump");

                }
            }
        }
    }
    void Jump (float strength)
    {
        this.rigidbody.AddForce (Vector2.up * strength, ForceMode2D.Impulse);
    }

    void Log (string message)
    {
        if (this.debug)
        {
            Debug.Log (message);
        }
    }
}
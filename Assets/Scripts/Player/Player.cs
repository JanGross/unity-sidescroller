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
      
        public void rotateTowardsMouse ()
        {
            Vector3 mousePositionInWorldSpace = Helper2D.getMousePosInWorldSpace ();
            Vector3 armRotation = this.arm.transform.rotation.eulerAngles; 
            int direction = mousePositionInWorldSpace.x < this.arm.transform.position.x ? -1 : 1;
            armRotation.z = Vector3.Angle (this.arm.transform.position, mousePositionInWorldSpace) * direction;
            this.arm.transform.rotation = Quaternion.Euler(armRotation);
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
        this.playerArm.rotateTowardsMouse ();
        float movementSpeed = this.playerSettings.walkSpeed;
        if (!this.canJump)
        {
            movementSpeed = this.playerSettings.airControlSpeed;
        }

        if (Input.GetAxisRaw ("Horizontal") < 0)
        {
            this.moveInDirection (Vector2.left);
        } 
        else if (Input.GetAxisRaw ("Horizontal") > 0)
        {
            this.moveInDirection (Vector2.right);    
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
            if (this.debug)
            {
                Vector3 point1 = new Vector3 (
                                            transform.position.x - this.collider.bounds.extents.x, 
                                            transform.position.y - (this.collider.bounds.extents.y - this.playerSettings.collisionPadding),
                                            0);
                Debug.DrawRay (point1, Vector3.left, Color.green);
            }
            if (contactPoint.point.y <= (transform.position.y - (this.collider.bounds.extents.y - this.playerSettings.collisionPadding))) 
            {
                if (Vector2.Angle (contactPoint.normal, Vector2.up) <= this.playerSettings.maxAngleToJump) 
                {
                    this.canJump = true;
                }
            }
        }
    }
    void Jump (float strength)
    {
        this.rigidbody.AddForce (Vector2.up * strength, ForceMode2D.Impulse);
    }

    void moveInDirection(Vector2 direction, float speed = 0)
    {
        float moveSpeed = speed != 0 ? speed : this.playerSettings.walkSpeed;
        Vector2 rayStartPosition = (Vector2)transform.position + (direction.normalized * (this.collider.bounds.extents.x + 0.01f));
        RaycastHit2D hit = Physics2D.Raycast (rayStartPosition, direction, this.playerSettings.collisionPadding);
        Debug.DrawLine(rayStartPosition, hit.point, Color.gray);


        if (hit.collider == null)
        {
            transform.Translate (direction * Time.deltaTime * moveSpeed);
        }
        Debug.Log (hit.collider);
    }

    void Log (string message)
    {
        if (this.debug)
        {
            Debug.Log (message);
        }
    }
}
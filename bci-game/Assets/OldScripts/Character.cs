using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f; //all speed in units/s
    public float gravity = 8f; //gravity accelaration
    public float airPenalty = 0.75f; //reduction in left/right speed while midair
    public float jumpSpeed = 15f;
    public float jumpDuration = 0.75f; //seconds
    public bool isAlive = true;
    public int[] movementBlocked = {0, 0, 0, 0}; //up, down, right, left
    public Animator animator;
    
    private bool isFacingRight = true; //tracks if character is facing right
    private bool midair = false;
    private float fallTime = 0; //tracks time spent in the air
    private float jumpTime = 0; //tracks time since last jump
    private List<CollisionInfo> pastCollisions = new List<CollisionInfo>();


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (isAlive) {
            //get movement inputs, LR used to determine horizontal movement, up to start jump
            float[] inputs = GetInput();
            if (!midair && inputs[1] > 0) {
                Jump();
            } 
            
            //get horizontal velocity and check if movement is blocked
            float hChange = UpdateHorizontal(inputs[0]) * Time.deltaTime;
            if (hChange > 0 && movementBlocked[2] > 0) hChange = 0;
            else if (hChange < 0 && movementBlocked[3] > 0) hChange = 0;

            //get vertical velocity and check
            float vChange = UpdateVertical() * Time.deltaTime;
            if (vChange > 0 && movementBlocked[0] > 0) vChange = 0;
            else if (vChange < 0 && movementBlocked[1] > 0) vChange = 0;

            //update position
            StartCoroutine(MoveCharacter(new Vector3 (hChange, vChange, 0)));
            if (inputs[0] != 0) {
                animator.SetFloat("X", inputs[0]+0.5F);
                animator.SetBool("IsWalking", true);
            } else {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    public float UpdateHorizontal(float hInput) {
        float hMvmt = hInput * moveSpeed;
        if (midair) hMvmt *= airPenalty;
        return hMvmt;
    }

    public float UpdateVertical() {
        float vMvmt = 0f;

        if (midair) {
            if (jumpTime > 0) {
                animator.SetBool("isJumping", true);
                vMvmt += jumpSpeed * jumpTime;
                jumpTime -= Time.deltaTime;
            } else {
                animator.SetBool("isJumping", false);
            }
            //v = gt implementation
            fallTime += Time.deltaTime;
            vMvmt -= gravity * fallTime;
        }
        return vMvmt;

    }

    protected virtual IEnumerator MoveCharacter(Vector3 movement){
        transform.position += movement;
        yield return null;
    }

    // Data structure to store collision information
    private struct CollisionInfo
    {
        public string objectName;
        public int direction;

        public CollisionInfo(string name, int dir)
        {
            objectName = name;
            direction = dir;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        BoxCollider otherBox = other.gameObject.GetComponent<BoxCollider>();
        int collisionDirection = CheckCollisionDirection(GetComponent<BoxCollider>(), otherBox);

        if (collisionDirection >= 0)
        {
            movementBlocked[collisionDirection] += 1;
        }

        if (collisionDirection == 1)
        {
            midair = false;
            fallTime = 0;
        }
        
        if (collisionDirection == 0)
        {
            jumpTime = 0;
        }

        RemoveOverlap(collisionDirection, GetComponent<BoxCollider>(), otherBox);

        // Store collision info in the list
        pastCollisions.Add(new CollisionInfo(other.name, collisionDirection));
    }

void OnTriggerExit(Collider other)
{
    // Find and remove the corresponding collision info from the list
    string objectName = other.name;
    int direction = -1; // Initialize with an invalid value

    for (int i = 0; i < pastCollisions.Count; i++)
    {
        if (pastCollisions[i].objectName == objectName)
        {
            direction = pastCollisions[i].direction;
            pastCollisions.RemoveAt(i);
            break;
        }
    }

    if (direction >= 0)
    {
        movementBlocked[direction] -= 1;
    }

    if (direction == 1)
    {
        midair = true;
        if (jumpTime <= 0) fallTime = 0.5f;
        else fallTime = 0;
    }
}

    private int CheckCollisionDirection(BoxCollider self, BoxCollider other) {
        //0 = top, 1 = bottom, 2 = right, 3 = left

        float xpos = self.transform.position.x;
        float ypos = self.transform.position.y;
        float xsize = self.size.x * self.transform.localScale.x / 2;
        float ysize = self.size.y * self.transform.localScale.y / 2;
        
        //corner order: top right, top left, bottom right, bottom left
        Vector3[] selfCorners = {
            new Vector3(xpos + xsize, ypos + ysize, 0),
            new Vector3(xpos - xsize, ypos + ysize, 0),
            new Vector3(xpos + xsize, ypos - ysize, 0),
            new Vector3(xpos - xsize, ypos - ysize, 0)
        };

        xpos = other.transform.position.x;
        ypos = other.transform.position.y;
        xsize = other.size.x * other.transform.localScale.x / 2;
        ysize = other.size.y * other.transform.localScale.y / 2;

        Vector3[] otherCorners = {
            new Vector3(xpos + xsize, ypos + ysize, 0),
            new Vector3(xpos - xsize, ypos + ysize, 0),
            new Vector3(xpos + xsize, ypos - ysize, 0),
            new Vector3(xpos - xsize, ypos - ysize, 0)
        };

        //check if each corner is within other object
        bool[] collisionCorners = new bool[8];
        
        //self top right
        collisionCorners[0] = CompareCornerNW(otherCorners[3], selfCorners[0], otherCorners[0]);
        //self top left
        collisionCorners[1] = CompareCornerSW(otherCorners[2], selfCorners[1], otherCorners[1]);
        //self bottom right
        collisionCorners[2] = CompareCornerSW(otherCorners[2], selfCorners[2], otherCorners[1]);
        //self bottom left
        collisionCorners[3] = CompareCornerNW(otherCorners[3], selfCorners[3], otherCorners[0]);
        //other top right
        collisionCorners[4] = CompareCornerNW(selfCorners[3], otherCorners[0], selfCorners[0]);
        //self top left
        collisionCorners[5] = CompareCornerSW(selfCorners[2], otherCorners[1], selfCorners[1]);
        //self bottom right
        collisionCorners[6] = CompareCornerSW(selfCorners[2], otherCorners[2], selfCorners[1]);
        //other bottom left
        collisionCorners[7] = CompareCornerNW(selfCorners[3], otherCorners[3], selfCorners[0]);
        // for (int i = 0; i < 8; i++) if (collisionCorners[i]) Debug.Log(i);
       
        //check collision direction
        if (collisionCorners[0] && collisionCorners[1]) return 0;
        else if (collisionCorners[2] && collisionCorners[3]) return 1;
        else if (collisionCorners[0] && collisionCorners[2]) return 2;
        else if (collisionCorners[1] && collisionCorners[3]) return 3;
        else if (collisionCorners[0]) {
            Vector3 diff = selfCorners[0] - otherCorners[3];
            if (diff.x > diff.y) return 0;
            else return 2;
        }
        else if (collisionCorners[1]) {
            Vector3 diff = selfCorners[1] - otherCorners[2];
            if (-diff.x > diff.y) return 0;
            else return 3;
        }
        else if (collisionCorners[2]) {
            Vector3 diff = otherCorners[0] - selfCorners[2];
            if (diff.x > diff.y) return 1;
            else return 3;
        }
        else if (collisionCorners[3]) {
            Vector3 diff = otherCorners[1] - selfCorners[3];
            if (-diff.x > diff.y) return 1;
            else return 2;
        }

        if (collisionCorners[6] && collisionCorners[7]) return 0;
        else if (collisionCorners[4] && collisionCorners[5]) return 1;
        else if (collisionCorners[5] && collisionCorners[7]) return 2;
        else if (collisionCorners[4] && collisionCorners[6]) return 3;

        return -1;
    }

    //check if mid is between low and high in NW direction
    private bool CompareCornerNW(Vector3 low, Vector3 mid, Vector3 high) {
        Vector3 v1 = mid - low;
        Vector3 v2 = high - mid;
        return (v1.x >= 0 && v1.y >= 0 && v2.x >= 0 && v2.y >= 0);
    }

    //check if mid is between low and high in SW direction
    private bool CompareCornerSW(Vector3 low, Vector3 mid, Vector3 high) {
        Vector3 v1 = mid - low;
        Vector3 v2 = high - mid;
        return (v1.x <= 0 && v1.y >= 0 && v2.x <= 0 && v2.y >= 0);
    }

    //move character so it does not overlap with platform
    private void RemoveOverlap(int direction, BoxCollider self, BoxCollider other) {
        float overlap;
        Vector3 change = new Vector3(0, 0, 0);

        //calculate overlap of objects
        //top
        if (direction == 0) {
            overlap = self.transform.position.y + self.transform.localScale.y * self.size.y / 2 - 
                (other.transform.position.y - other.transform.localScale.y * other.size.y / 2);
            if (overlap > 0) change.y -= overlap;
        }
        //bottom
        if (direction == 1) {
            overlap = other.transform.position.y + other.transform.localScale.y * other.size.y / 2 - 
                (self.transform.position.y - self.transform.localScale.y * self.size.y / 2);
            if (overlap > 0) change.y += overlap;
        }
        //right
        if (direction == 2) {
            overlap = self.transform.position.x + self.transform.localScale.x * self.size.x / 2 - 
                (other.transform.position.x - other.transform.localScale.x * other.size.x / 2);
            if (overlap > 0) change.x -= overlap;
        }
        //left
        if (direction == 3) {
            overlap = other.transform.position.x + other.transform.localScale.x * other.size.x / 2 - 
                (self.transform.position.x - self.transform.localScale.x * self.size.x / 2);
            if (overlap > 0) change.x += overlap;
        }
        //Debug.Log(change);

        StartCoroutine(MoveCharacter(change));
    }

    public virtual float[] GetInput() {
        return new float[] {0, 0};
    }

    public bool Midair() {
        return midair;
    }

    public void Jump() {
        midair = true;
        jumpTime = jumpDuration;
        animator.SetTrigger("takeoff");
    }

    


}

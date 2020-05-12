using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieControls : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public float zombieSpeed = .03f;
    private bool isPlayerNear = false;
    public float timeBetweenLookingAround = 5.0f;
    public float lookingAroundTime = 3.0f;
    public float maxDistanceZombieWillChase = 20f;
    public float maxZombieTurnSpeed = 1f;
    public float raycastDistanceForTurning = 2f;
    public int terrainLayer = 9;
    public int shedLayer = 11;
    private bool hitSomething = false;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(StutterWalk());
    }

    // Update is called once per frame
    void Move()
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction,this.transform.forward);
        Debug.DrawRay(transform.position,direction*10,Color.green);
        Debug.DrawRay(transform.position,transform.forward*10,Color.green);
        float distanceBetween = Vector3.Distance(player.position, this.transform.position);
        int shouldTurn = 0;
        if((distanceBetween < maxDistanceZombieWillChase && angle < 45) || distanceBetween < maxDistanceZombieWillChase/2)
        {
            isPlayerNear = true;
            direction.y = 0;

            if (shouldTurn != 0)
            {
                direction = adjustDirection(shouldTurn);
            }
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,
                                                Quaternion.LookRotation(direction), maxZombieTurnSpeed/distanceBetween);
            
            anim.SetBool("isIdle",false);
            if(direction.magnitude < 1.25f)
            {
                anim.SetBool("isAttacking",true);
                anim.SetBool("isMoving",false);
            }
            else
            {
                anim.SetBool("isMoving",true);
                anim.SetBool("isAttacking",false);
                this.transform.Translate(0,0,zombieSpeed);
            }
        }
        else
        {
            isPlayerNear = false;
            anim.SetBool("isIdle",true);
            anim.SetBool("isMoving",false);
            anim.SetBool("isAttacking",false);
            shouldTurn = determineIfShouldTurn();
            if (shouldTurn != 0)
            {
                float turnSpeed = maxZombieTurnSpeed;
                //if (hitSomething == true)
                //    turnSpeed = 10f;
                direction = adjustDirection(shouldTurn);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            }
            else if (hitSomething == true)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(-this.transform.forward), 0.1f);
            }
        }
    }

    IEnumerator StutterWalk() 
    {
        bool isTimeToLookingAround = false;
        float timer = 0.00f;
        for (;;)
        {
            while(!isPlayerNear)
            {
                timer += Time.deltaTime;
                if (timer > timeBetweenLookingAround)
                    isTimeToLookingAround = true;
                if (!isTimeToLookingAround)
                {
                    anim.SetBool("isMoving",true);
                    anim.SetBool("isAttacking",false);
                    anim.SetBool("isIdle",false);
                    this.transform.Translate(0,0,zombieSpeed);
                    yield return null;
                }
                else
                {
                    anim.SetBool("isIdle",true);
                    anim.SetBool("isMoving",false);
                    anim.SetBool("isAttacking",false);
                    isTimeToLookingAround = false;
                    timer = 0.00f;
                    yield return new WaitForSeconds(lookingAroundTime);
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    int determineIfShouldTurn ()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << terrainLayer;
        layerMask = 1 << shedLayer;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        bool rayLeft = false;
        bool rayRight = false;
        CapsuleCollider zombieCollider = this.gameObject.GetComponent<CapsuleCollider>();
        Vector3 rayStartLeft = this.transform.position + new Vector3 (-zombieCollider.radius/4,zombieCollider.height/4,-zombieCollider.radius/2);
        Vector3 rayStartRight = this.transform.position + new Vector3 (zombieCollider.radius/4,zombieCollider.height/4,-zombieCollider.radius/2);
        rayLeft = Physics.SphereCast(rayStartLeft, zombieCollider.radius/2, transform.TransformDirection(Vector3.forward), out hitLeft, raycastDistanceForTurning, layerMask);
        rayRight = Physics.SphereCast(rayStartRight, zombieCollider.radius/2, transform.TransformDirection(Vector3.forward), out hitRight, raycastDistanceForTurning, layerMask);
        float rayDistanceLeft = 0f;
        float rayDistanceRight = 0f;
        if (rayLeft)
            rayDistanceLeft = hitLeft.distance;
        if (rayRight)
            rayDistanceRight = hitRight.distance;

        Debug.Log("left " + rayDistanceLeft);
        Debug.Log("right " + rayDistanceRight);
        if (rayDistanceLeft > rayDistanceRight)
        {
            return -1;
        }
        else if (rayDistanceRight > rayDistanceLeft)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    Vector3 adjustDirection (int turnDirection)
    {
        if (turnDirection < 0)
        {
            return -this.transform.right;
        }
        else
        {
            return this.transform.right;
        }
    }

    void OnTriggerStay (Collider collision)
    {
        hitSomething = true;
        /*if (collision.gameObject.layer == terrainLayer)
        {
            int shouldTurn = determineIfShouldTurn();
            if (shouldTurn != 0)
            {
                Vector3 direction = adjustDirection(shouldTurn);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,
                                                Quaternion.LookRotation(direction), 90f);
            }
        }*/
    }

    void OnTriggerExit()
    {
        hitSomething = false;
    }
}

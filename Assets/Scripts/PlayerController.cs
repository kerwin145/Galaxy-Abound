using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    Vector2 input;
    Vector2 mouseInput;

    //movement
    Vector2 velocity;
    [SerializeField] private Camera mainCamera;
    [SerializeField] float speed;
    [SerializeField] float boostMultiplier;
    bool boosted;
    bool mousePositionQueued = false;
    float boostMax = 20;
    float boostCriticalValue;
    float boostCurrent;
    bool boostCritical = false;
    [SerializeField] Slider boostMeter;
    [SerializeField] Image boostFill;

    //health and gameplay
    int healthMax = 15;
    int healthCurrent;
    [SerializeField] WeaponController weapon;

    enum headingTowards {stopped, up, right, down, left};
    headingTowards heading;
    headingTowards bulletHeading;

    // Start is called before the first frame update
    public void Init()
    {
        boostCriticalValue = boostMax * .3f;
        boostMeter.maxValue = boostMax;
        updateBoostMeter(boostMax);

        heading = headingTowards.stopped;
        healthCurrent = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //get location of right click
        if (Input.GetMouseButtonDown(1))
        {
            mousePositionQueued = true;
            mouseInput = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetKey(KeyCode.LeftShift)) boosted = true;
        else boosted = false;
        

        //bullet shoot function must go after boosted 
        //if you are hitting space, the bullet will be boosted if u are boosted.
        //an additional check for bullet boosting is needed if you give a mouse down position. The bullet will only boost if it is headed in the same direction as you.
        if (Input.GetKey(KeyCode.Space))
        {
            weapon.shoot(boosted, (int)heading);
        }else if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L))
        {
            headingTowards bulletHeading;
            if (Input.GetKey(KeyCode.I)) bulletHeading = headingTowards.up;
            else if (Input.GetKey(KeyCode.J)) bulletHeading = headingTowards.left;
            else if (Input.GetKey(KeyCode.K)) bulletHeading = headingTowards.down;
            else bulletHeading = headingTowards.right;

            weapon.shoot(boosted && bulletHeading == heading, (int)bulletHeading);

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseInput = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            headingTowards bulletHeading = getHeading(mouseInput);
            //not exactly perfect for filtering out mouse clicks that end up on the sprite, since sprite is not a square, but it will do for now
            bool mouseClickOnPlayer = Vector2.Distance(mouseInput, new Vector2(transform.position.x, transform.position.y)) < GetComponent<SpriteRenderer>().bounds.size.x / 2;
            weapon.shoot(boosted && bulletHeading == heading && !mouseClickOnPlayer, (int)bulletHeading);
       
        }
    }

    private void FixedUpdate()
    {
        handleMovement();
    }

    void handleMovement()
    {
        Vector3 targetPos = transform.position;

        //Handles moving parameters-----
        if (mousePositionQueued)
        {
            targetPos = mouseInput;
        }
        if (input.x != 0 || input.y != 0)
        {
            targetPos.x += input.x;
            targetPos.y += input.y;
            mousePositionQueued = false;
        }

        //^^^---------------------------


        //Handles direction----------
        heading = getHeading(targetPos);
        weapon.setPlayerDirection((int)heading);
        //^^^------------------------

        //Deals with boosting-----
        //if your not moving, then automatically consider boost as off
        if (!(mousePositionQueued || (input.x != 0 || input.y != 0)) && !boostCritical)
        {
            boosted = false;
        }

        //once you stop holding the boost button, you exit critical mode (in terms of story, consider it as an emergency use)
        if (boostCritical && !boosted) boostCritical = false;
        //while you are in critical range and try to boost, you stop boosting
        if (boostCritical && boostCurrent < boostCriticalValue) boosted = false;

        if (boosted && boostCurrent >= 0 && !boostCritical)
        {
            updateBoostMeter(boostCurrent - 4.5f * Time.deltaTime);
          
        }
        else if (boostCritical || !boosted)
        {
            //if you have gone into critical mode (aka completly depleted fuel), u will refuel slower while u are critical
            if(boostCritical)
                updateBoostMeter(boostCurrent + .65f * Time.deltaTime);
            else
                updateBoostMeter(boostCurrent + 1 * Time.deltaTime);

            if (boostCurrent > boostCriticalValue)
                boostCritical = false;
        }

        if (boostCurrent <= 0)
        {
            boostCritical = true;
            boosted = false;
        }

        //update boost meter UI
        if (boostCritical)
            boostFill.color = Color.red;
        else
            boostFill.color = new Color(231f / 255, 142f / 255, 31f / 255);

        // Debug.Log(boostMeter.value + " " + boostCritical);

        //^^^----------------------

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed * (boosted ? boostMultiplier : 1));

        if (mousePositionQueued && (transform.position - targetPos).sqrMagnitude < Mathf.Epsilon)
            mousePositionQueued = false;

        
    }

    void updateBoostMeter(float val)
    {
        Mathf.Clamp(val, 0, boostMax);
        boostCurrent = val;
        boostMeter.value = val;
    }

    headingTowards getHeading(Vector3 target)
    {
        float theta = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * 180 / Mathf.PI;

        if (target == transform.position) return headingTowards.stopped;
        if (theta >= -45 && theta <= 45) return headingTowards.right;
        if ((theta >= 135 && theta <= 180) || (theta >= -180 && theta <= -135)) return headingTowards.left;
        if (theta > 45 && theta < 135) return headingTowards.up;
        return headingTowards.down;
    }
}
    
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Ore.OreType carry1 = Ore.OreType.NONE;
    Ore.OreType carry2 = Ore.OreType.NONE;
    [SerializeField]
    float moveSpeed = 2f;
    float realMoveSpeed;
    float moveVelocity = 0;
    float slowRate = 0;
    public bool IsCarryingUpgrade { private set; get; }

    GameManager gameManager;
    Collider2D lastCollider;
    BoxCollider2D myCollider;

	// Use this for initialization
	void Start () {
        realMoveSpeed = moveSpeed;
        myCollider = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        IsCarryingUpgrade = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameManager.GameActive) {
            return;
        }
        HandleInput();
        HandleMovement();
	}

    private void HandleInput()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            realMoveSpeed = moveSpeed * (1 - slowRate);
            moveVelocity = -realMoveSpeed;
        } else if (Input.GetAxis("Horizontal") > 0)
        {
            realMoveSpeed = moveSpeed * (1 - slowRate);
            moveVelocity = realMoveSpeed;
        } else
        {
            moveVelocity = 0;
        }
        if (Input.GetButtonDown("Grab"))
        {
            HandleGrab();
        }
        if (Input.GetButtonDown("Use"))
        {
            HandleUse();
        }
    }

    private void HandleUse()
    {
        Debug.Log("Use button pressed.");
        //throw new NotImplementedException();
        if (myCollider.IsTouching(lastCollider))
        {
            if (lastCollider.GetComponent<OreBox>() != null)
            {
                if (carry1 == Ore.OreType.NONE && carry2 == Ore.OreType.NONE)
                {
                    lastCollider.GetComponent<OreBox>().CleanUp();
                } else
                {
                    //todo: Play "buzz" sound?
                    Debug.Log("Cannot clean when carrying an ore.");
                }
            }
        }
    }

    public Ore.OreType[] GetHeldOres()
    {
        return new Ore.OreType[2] { carry1, carry2 };
    }

        private void HandleGrab()
    {
        Debug.Log("Grab button pressed.");
        if (myCollider.IsTouching(lastCollider))
        {
            Debug.Log("Colliders match.");
            if ((carry1 == Ore.OreType.NONE || carry2 == Ore.OreType.NONE) && !IsCarryingUpgrade)
            {
                if (lastCollider.GetComponent<OreBox>() != null)
                {
                    OreBox activeBox = lastCollider.GetComponent<OreBox>();
                    Debug.Log("Grab used on " + lastCollider.name + " Ore Box (" + activeBox.GetBoxType().ToString() + ").");
                    if (activeBox.TakeOre())
                    {
                        if (carry1 == Ore.OreType.NONE)
                        {
                            carry1 = activeBox.GetBoxType();
                        }
                        else if (carry2 == Ore.OreType.NONE)
                        {
                            carry2 = activeBox.GetBoxType();
                        }
                        Debug.Log("Now carrying: " + carry1.ToString() + " and " + carry2.ToString() + ".");
                    }
                    else
                    {
                        Debug.Log("Could not take ore. Box is empty.");
                    }
                }
            }
            else if (IsCarryingUpgrade)
            {
                if (lastCollider.GetComponent<OreBox>() != null)
                {
                    OreBox activeBox = lastCollider.GetComponent<OreBox>();
                    Debug.Log("Grab used on " + lastCollider.name + " Ore Box (" + activeBox.GetBoxType().ToString() + ").");
                    activeBox.SetCapacity(activeBox.Capacity + 2);
                    Debug.Log("Upgrade used.");
                    IsCarryingUpgrade = false;
                }
            }
            if (lastCollider.GetComponent<UpgradeOTron>() != null)
            {
                UpgradeOTron upgradeOTron = lastCollider.GetComponent<UpgradeOTron>();
                bool getUpgrade = false;
                getUpgrade = upgradeOTron.CheckOres(new Ore.OreType[2] { carry1, carry2 });
                if (getUpgrade)
                {
                    carry1 = Ore.OreType.NONE;
                    carry2 = Ore.OreType.NONE;
                    IsCarryingUpgrade = true;
                }
            }
            if (lastCollider.GetComponent<OreProcessor>() != null)
            {
                Debug.Log("Activating Ore Processor.");
                OreProcessor processor = lastCollider.GetComponent<OreProcessor>();
                Debug.Log("Attempting to deposit Ore " + carry1.ToString() + " and " + carry2.ToString() + " into processor.");
                if (carry1 != Ore.OreType.NONE || carry2 != Ore.OreType.NONE)
                {
                    Debug.Log("Depositing Ores " + carry1.ToString() + " and " + carry2.ToString() + " into processor.");
                    processor.InsertOres(new Ore.OreType[] { carry1, carry2 });
                    carry1 = Ore.OreType.NONE;
                    carry2 = Ore.OreType.NONE;
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (moveVelocity != 0)
        {
            Vector3 position = transform.position;
            position.x += moveVelocity * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, -8.5f, 8.5f);
            transform.position = position;
        }
    }

    public void SetSlowRate(float amt)
    {
        slowRate = Mathf.Clamp(amt,0,0.6f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lastCollider = collision;
    }

}

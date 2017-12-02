using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreBox : MonoBehaviour {

    //BoxCollider2D myCollider2D;
    [SerializeField]
    Ore.OreType boxType;
    [SerializeField]
    SpriteRenderer myContents;
    [SerializeField]
    SpriteRenderer myOverflow;
    [SerializeField]
    SpriteRenderer myOverflowFront;
    GameManager gameManager;
    public int Capacity { private set; get; }
    public float CapPercent { private set; get; }
    int HeldOre;
    int Overflow;
    void Start()
    {
        HeldOre = 0;
        Capacity = 8;
        //myCollider2D = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        CapPercent = System.Convert.ToSingle(HeldOre) / Capacity;
        if (CapPercent == 0)
        {
            myContents.enabled = false;
        }
        else if (CapPercent < 0.2)
        {
            myContents.enabled = true;
            myContents.sprite = gameManager.GetRockPileSprites()[0];
        }
        else if (CapPercent < 0.4)
        {
            myContents.enabled = true;
            myContents.sprite = gameManager.GetRockPileSprites()[1];
        }
        else if (CapPercent < 0.6)
        {
            myContents.enabled = true;
            myContents.sprite = gameManager.GetRockPileSprites()[2];
        }
        else if (CapPercent < 0.8)
        {
            myContents.enabled = true;
            myContents.sprite = gameManager.GetRockPileSprites()[3];
        }
        else
        {
            myContents.enabled = true;
            myContents.sprite = gameManager.GetRockPileSprites()[4];
        }
        if (Overflow > 9) {
            myOverflow.sprite = gameManager.GetOverflowSprites()[3];
            myOverflow.enabled = true;
            myOverflowFront.enabled = true;
        }
        else if (Overflow > 6)
        {
            myOverflow.sprite = gameManager.GetOverflowSprites()[2];
            myOverflow.enabled = true;
            myOverflowFront.enabled = false;
        }
        else if (Overflow > 3)
        {
            myOverflow.sprite = gameManager.GetOverflowSprites()[1];
            myOverflow.enabled = true;
            myOverflowFront.enabled = false;
        }
        else if (Overflow > 0)
        {
            myOverflow.sprite = gameManager.GetOverflowSprites()[0];
            myOverflow.enabled = true;
            myOverflowFront.enabled = false;
        }
        else
        {
            myOverflow.enabled = false;
            myOverflowFront.enabled = false;
        }
    }

    public Ore.OreType GetBoxType()
    {
        return boxType;
    }

    public bool TakeOre()
    {
        if (HeldOre > 0)
        {
            HeldOre--;
            return true;
        }
        return false;
    }

    public void SetCapacity(int amt)
    {
        if (amt < Capacity) throw new System.Exception("Should not be reducing bin capacity.");
        Capacity = amt;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (Overflow > 0)
            {
                player.SetSlowRate(Overflow * .05f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.name != "Player")
        //{
        //    Debug.Log("Trigger entered by " + collision.name);
        //}
        Ore targetOre = collision.gameObject.GetComponent<Ore>();
        if (targetOre != null)
        {
            //Debug.Log("This is an [Ore] object.");
            if (targetOre.GetOreType() == boxType)
            {
                HeldOre++;
                if (HeldOre > Capacity) {
                    Overflow = Mathf.Clamp(Overflow+1,0,12);
                    HeldOre = Capacity;
                }
                Destroy(collision.gameObject);
                //Debug.Log("We should have destroyed the Ore object.");
            }
        }
    }

    public void CleanUp()
    {
        Debug.Log("Cleanup attempt. Overflow = "+Overflow);
        if (Overflow > 0)
        {
            Debug.Log("Ore cleaned up. Now " + Overflow);
            Overflow = Mathf.Clamp(Overflow-1,0,12);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.SetSlowRate(0);
        }
    }

}

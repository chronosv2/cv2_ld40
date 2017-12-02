using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreBox : MonoBehaviour {

    BoxCollider2D collider2D;
    float ColliderOffset = 0;
    [SerializeField]
    Ore.OreType boxType;
    int heldOre;

    void Start()
    {
        heldOre = 0;
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered by " + collision.name);
        Ore targetOre = collision.gameObject.GetComponent<Ore>();
        if (targetOre != null)
        {
            Debug.Log("This is an [Ore] object.");
            if (targetOre.GetOreType() == boxType)
            {
                heldOre++;
                Destroy(collision.gameObject);
                Debug.Log("We should have destroyed the Ore object.");
            }
        }
    }

}

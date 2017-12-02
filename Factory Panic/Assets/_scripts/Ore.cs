using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour {

    public enum OreType { RED, BLUE, YELLOW, GREEN }
    [SerializeField]
    OreType oreType;
    [SerializeField]
    float slowSpeed = 0.5f;
    [SerializeField]
    float fastSpeed = 1.2f;

    public OreType GetOreType()
    {
        return oreType;
    }
	
	// Update is called once per frame
	void Update () {
        HandleScaling();
        HandleMovement();
        HandleCollision();
    }

    private void HandleScaling()
    {
        //todo: Implement scaling sprite for distance down conveyor
    }

    private void HandleCollision()
    {
        //todo: Implement collision with bin
    }

    private void HandleMovement()
    {
        Vector3 position = transform.position;
        if (position.y > -0.32)
        {
            position.y -= slowSpeed * Time.deltaTime;
        }
        else
        {
            position.y -= fastSpeed * Time.deltaTime;
        }
        transform.position = position;
    }
}

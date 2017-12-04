using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour {

    public enum OreType { RED, BLUE, YELLOW, GREEN, NONE }
    [SerializeField]
    OreType oreType;
    [SerializeField]
    float slowSpeed = 0.5f;
    [SerializeField]
    float fastSpeed = 1.2f;
    float totalDistance;
    float movedDistance;


    private void Start()
    {
        float startY = transform.position.y;
        totalDistance = Mathf.Abs(0.32f - startY);
    }

    public OreType GetOreType()
    {
        return oreType;
    }
	
	// Update is called once per frame
	void Update () {
        HandleScaling();
        HandleMovement();
    }

    private void HandleScaling()
    {
        Vector3 scale = transform.localScale;
        float movePct = movedDistance / totalDistance;
        scale.x = Mathf.Lerp(0.2f, 1.0f, movePct);
        scale.y = Mathf.Lerp(0.2f, 1.0f, movePct);
        transform.localScale = scale;
        //todo: Implement scaling sprite for distance down conveyor
    }

    private void HandleMovement()
    {
        Vector3 position = transform.position;
        if (position.y > -0.32)
        {
            float moveRate = slowSpeed * Time.deltaTime;
            position.y -= moveRate;
            movedDistance += moveRate;
        }
        else
        {
            position.y -= fastSpeed * Time.deltaTime;
        }
        transform.position = position;
    }
}

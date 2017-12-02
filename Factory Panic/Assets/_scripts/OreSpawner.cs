using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour {

    [SerializeField]
    GameObject ore;
	GameManager gameManager;
    float timer = 0;
    float spawnTime = 2.0f;

	// Use this for initialization
	void Awake () {
		gameManager = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            spawnTime = gameManager.SpawnSpeed * Random.Range(0.7f, 1.31f);
            Instantiate(ore, transform.position, Quaternion.identity);
            timer = 0;
        }
	}
}

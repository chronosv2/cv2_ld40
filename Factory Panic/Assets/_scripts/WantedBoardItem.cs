using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WantedBoardItem : MonoBehaviour {

    Image[] blips;
    GameManager gameManager;

    // Use this for initialization
    void Start () {
        blips = new Image[4];
        blips[0] = transform.Find("Blip1").GetComponent<Image>();
        blips[1] = transform.Find("Blip2").GetComponent<Image>();
        blips[2] = transform.Find("Blip3").GetComponent<Image>();
        blips[3] = transform.Find("Blip4").GetComponent<Image>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Set(Ore.OreType[] ores)
    {
        for (int i = 0; i < 4; i++)
        {
            switch (ores[i])
            {
                case Ore.OreType.NONE:
                    blips[i].color = gameManager.GetOreColors()[0].color;
                    break;
                case Ore.OreType.BLUE:
                    blips[i].color = gameManager.GetOreColors()[1].color;
                    break;
                case Ore.OreType.GREEN:
                    blips[i].color = gameManager.GetOreColors()[2].color;
                    break;
                case Ore.OreType.RED:
                    blips[i].color = gameManager.GetOreColors()[3].color;
                    break;
                case Ore.OreType.YELLOW:
                    blips[i].color = gameManager.GetOreColors()[4].color;
                    break;
            }
        }
    }
}

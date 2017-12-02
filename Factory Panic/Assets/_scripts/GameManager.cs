using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

	[SerializeField]
	public float SpawnSpeed { private set; get; }
    [SerializeField]
    OreBox blueBox;
    [SerializeField]
    OreBox greenBox;
    [SerializeField]
    OreBox redBox;
    [SerializeField]
    OreBox yellowBox;
    public int BlueCount { private set; get; }
    public int GreenCount { private set; get; }
    public int RedCount { private set; get; }
    public int YellowCount { private set; get; }
    public int Score { private set; get; }
    [SerializeField]
    Sprite[] rockPileSprites;
    [SerializeField]
    Sprite[] overflowSprites;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI lastScoreText;
    [SerializeField]
    Image[] blips;
    [SerializeField]
    Material[] oreColors;

    Ore.OreType[] lastOres;
    int lastScored = 0;

    // Use this for initialization
    void Start () {
        SpawnSpeed = 3.0f;
        lastOres = new Ore.OreType[4] { Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE };
	}

    public void SetLastPattern(Ore.OreType[] ores, int value)
    {
        Debug.Log("Received Scoring Data: " + ores.Length + " worth " + value);
        for (int i=0;i<ores.Length;i++)
        {
            lastOres[i] = ores[i];
            Debug.Log("Transferred " + ores[i].ToString() + " to " + lastOres[i].ToString());
        }
        lastScored = value;
    }

    public Sprite[] GetRockPileSprites()
    {
        return rockPileSprites;
    }

    public Sprite[] GetOverflowSprites()
    {
        return overflowSprites;
    }

    public void AddScore(int amt)
    {
        Score += amt;
    }

	// Update is called once per frame
	void Update () {
        BlueCount = Mathf.Clamp(Mathf.FloorToInt(blueBox.CapPercent * 100),0,100);
        GreenCount = Mathf.Clamp(Mathf.FloorToInt(greenBox.CapPercent * 100), 0, 100);
        RedCount = Mathf.Clamp(Mathf.FloorToInt(redBox.CapPercent * 100), 0, 100);
        YellowCount = Mathf.Clamp(Mathf.FloorToInt(yellowBox.CapPercent * 100), 0, 100);
        scoreText.text = "Score: "+Score.ToString("00000000");
        lastScoreText.text = lastScored.ToString("0000")+" PTS.";
        for (int i=0;i<4;i++)
        {
            switch(lastOres[i])
            {
                case Ore.OreType.NONE:
                    blips[i].color = oreColors[0].color;
                    break;
                case Ore.OreType.BLUE:
                    blips[i].color = oreColors[1].color;
                    break;
                case Ore.OreType.GREEN:
                    blips[i].color = oreColors[2].color;
                    break;
                case Ore.OreType.RED:
                    blips[i].color = oreColors[3].color;
                    break;
                case Ore.OreType.YELLOW:
                    blips[i].color = oreColors[4].color;
                    break;
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OreProcessor : MonoBehaviour {

    Ore.OreType ore1 = Ore.OreType.NONE;
    Ore.OreType ore2 = Ore.OreType.NONE;
    Ore.OreType ore3 = Ore.OreType.NONE;
    Ore.OreType ore4 = Ore.OreType.NONE;
    [SerializeField]
    int startingValue = 100;
    [SerializeField]
    int increment = 25;
    [SerializeField]
    int baseScore = 50;
    [SerializeField]
    int noMatchScore = 20;
    [SerializeField]
    SpriteRenderer panel1;
    [SerializeField]
    SpriteRenderer panel2;
    [SerializeField]
    SpriteRenderer panel3;
    [SerializeField]
    SpriteRenderer panel4;
    [SerializeField]
    Material[] oreColors;
    [SerializeField]
    WantedBoardItem[] wantedItems;
    AudioSource audioSource;
    [SerializeField]
    AudioClip noMatchSFX;
    [SerializeField]
    AudioClip lowSFX;
    [SerializeField]
    AudioClip highSFX;
    

    public struct ScoringPatterns
    {
        public int Value;
        public Ore.OreType[] ores;
    }

    ScoringPatterns[] patterns;
    GameManager gameManager;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
        GenerateNewPatterns();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdatePanels();
    }

    private void UpdatePanels()
    {
        switch (ore1)
        {
            case Ore.OreType.NONE:
                panel1.material = oreColors[0];
                break;
            case Ore.OreType.BLUE:
                panel1.material = oreColors[1];
                break;
            case Ore.OreType.GREEN:
                panel1.material = oreColors[2];
                break;
            case Ore.OreType.RED:
                panel1.material = oreColors[3];
                break;
            case Ore.OreType.YELLOW:
                panel1.material = oreColors[4];
                break;
        }
        switch (ore2)
        {
            case Ore.OreType.NONE:
                panel2.material = oreColors[0];
                break;
            case Ore.OreType.BLUE:
                panel2.material = oreColors[1];
                break;
            case Ore.OreType.GREEN:
                panel2.material = oreColors[2];
                break;
            case Ore.OreType.RED:
                panel2.material = oreColors[3];
                break;
            case Ore.OreType.YELLOW:
                panel2.material = oreColors[4];
                break;
        }
        switch (ore3)
        {
            case Ore.OreType.NONE:
                panel3.material = oreColors[0];
                break;
            case Ore.OreType.BLUE:
                panel3.material = oreColors[1];
                break;
            case Ore.OreType.GREEN:
                panel3.material = oreColors[2];
                break;
            case Ore.OreType.RED:
                panel3.material = oreColors[3];
                break;
            case Ore.OreType.YELLOW:
                panel3.material = oreColors[4];
                break;
        }
        switch (ore4)
        {
            case Ore.OreType.NONE:
                panel4.material = oreColors[0];
                break;
            case Ore.OreType.BLUE:
                panel4.material = oreColors[1];
                break;
            case Ore.OreType.GREEN:
                panel4.material = oreColors[2];
                break;
            case Ore.OreType.RED:
                panel4.material = oreColors[3];
                break;
            case Ore.OreType.YELLOW:
                panel4.material = oreColors[4];
                break;
        }
    }

    public void InsertOres(Ore.OreType[] ores)
    {
        for (int i=0; i<2; i++)
        {
            if (ore1 == Ore.OreType.NONE)
            {
                ore1 = ores[i];
            } else if (ore2 == Ore.OreType.NONE)
            {
                ore2 = ores[i];
            }
            else if (ore3 == Ore.OreType.NONE)
            {
                ore3 = ores[i];
            }
            else if (ore4 == Ore.OreType.NONE)
            {
                ore4 = ores[i];
            }
        }
        if (ore1 != Ore.OreType.NONE && ore2 != Ore.OreType.NONE && ore3 != Ore.OreType.NONE && ore4 != Ore.OreType.NONE)
        {
            int val = ScoreOres();
            gameManager.AddScore(val);
            if (val != baseScore && val != noMatchScore)
            {
                gameManager.IncreaseMultiplier();
            }
            //Debug.Log("Scored " + val);
        }
    }

    int ScoreOres()
    {
        if (ore1 == ore2 && ore2 == ore3 && ore3 == ore4)
        {
            gameManager.SetLastPattern(new Ore.OreType[4] { ore1, ore2, ore3, ore4 }, baseScore);
            ore1 = Ore.OreType.NONE;
            ore2 = Ore.OreType.NONE;
            ore3 = Ore.OreType.NONE;
            ore4 = Ore.OreType.NONE;
            audioSource.PlayOneShot(lowSFX);
            return baseScore;
        } else
        {
            int rtnVal = 0;
            for (int i=0; i<9; i++)
            {
                if (ore1 == patterns[i].ores[0] && ore2 == patterns[i].ores[1] && ore3 == patterns[i].ores[2] && ore4 == patterns[i].ores[3])
                {
                    rtnVal = patterns[i].Value;
                }
            }
            if (rtnVal == 0) {
                rtnVal = noMatchScore;
            }
            gameManager.SetLastPattern(new Ore.OreType[4] { ore1, ore2, ore3, ore4 }, rtnVal);
            ore1 = Ore.OreType.NONE;
            ore2 = Ore.OreType.NONE;
            ore3 = Ore.OreType.NONE;
            ore4 = Ore.OreType.NONE;
            if (rtnVal==noMatchScore)
            {
                audioSource.PlayOneShot(noMatchSFX);
            } else
            {
                audioSource.PlayOneShot(highSFX);
            }
            return rtnVal;
        }
    }

    public void GenerateNewPatterns()
    {
        patterns = new ScoringPatterns[9];
        int currentValue = startingValue;
        for (int i=0; i<9; i++)
        {
            patterns[i].ores = new Ore.OreType[4];
            patterns[i].Value = currentValue;
            for (int j=0; j<4; j++)
            {
                int val = Random.Range(0, 4);
                switch (val)
                {
                    case 0:
                        patterns[i].ores[j] = Ore.OreType.BLUE;
                        break;
                    case 1:
                        patterns[i].ores[j] = Ore.OreType.GREEN;
                        break;
                    case 2:
                        patterns[i].ores[j] = Ore.OreType.RED;
                        break;
                    case 3:
                        patterns[i].ores[j] = Ore.OreType.YELLOW;
                        break;
                }
                if (j==3)
                {
                    bool duplicate = true;
                    for (int k=1;k<4;k++)
                    {
                        if (patterns[i].ores[k] != patterns[i].ores[k-1])
                        {
                            duplicate = false;
                        }
                    }
                    if (duplicate==true)
                    {
                        j = -1;
                    }
                }
            }
            //Debug.Log("Pattern " + i + ": Ores: " + patterns[i].ores[0].ToString() + "," + patterns[i].ores[1].ToString() + "," + patterns[i].ores[2].ToString() + "," + patterns[i].ores[3].ToString() + " at value " + patterns[i].Value);
            wantedItems[i].Set(new Ore.OreType[4] { patterns[i].ores[0], patterns[i].ores[1], patterns[i].ores[2], patterns[i].ores[3] });
            currentValue = currentValue + increment;
        }
    }

    public ScoringPatterns[] GetScoringPatterns()
    {   
        if (patterns.Length != 0)
        {
            return patterns;
        } else
        {
            ScoringPatterns[] empty = new ScoringPatterns[9];
            for (int i=0; i<9; i++)
            {
                empty[i].ores = new Ore.OreType[4] { Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE };
                empty[i].Value = 0;
            }
            return empty;
        }
    }
}

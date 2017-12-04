using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOTron : MonoBehaviour {

    Ore.OreType[] neededOres;
    [SerializeField]
    SpriteRenderer[] blips;
    GameManager gameManager;

    private void Start()
    {
        neededOres = new Ore.OreType[2];
        gameManager = FindObjectOfType<GameManager>();
        SetupNewUpgrade();
    }

    private void SetupNewUpgrade()
    {
        for (int i = 0; i < 2; i++)
        {
            int num = Random.Range(0, 4);
            switch (num)
            {
                case 0:
                    neededOres[i] = Ore.OreType.BLUE;
                    break;
                case 1:
                    neededOres[i] = Ore.OreType.GREEN;
                    break;
                case 2:
                    neededOres[i] = Ore.OreType.RED;
                    break;
                case 3:
                    neededOres[i] = Ore.OreType.YELLOW;
                    break;
            }
            blips[i].material = gameManager.GetOreColors()[num+1];
        }
    }

    public bool CheckOres(Ore.OreType[] ores)
    {
        if ((ores[0]==neededOres[0] && ores[1]==neededOres[1]) || (ores[0]==neededOres[1] && ores[1]==neededOres[0]))
        {
            SetupNewUpgrade();
            return true;
        }
        return false;
    }
}

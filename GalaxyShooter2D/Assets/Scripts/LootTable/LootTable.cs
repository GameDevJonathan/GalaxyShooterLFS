using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    // Start is called before the first frame update
    [System.Serializable]
    public class Drop
    {
        public PowerUpBehavior drop;
        public int weight;
    }

    public List<Drop> table;

   [System.NonSerialized]
    int totalWeight = -1;

    public int TotalWeight
    {
        get
        {
            if(totalWeight == -1)
            {
                CalculateTotalWeight();
            }
            return totalWeight;
        }
    }

    void CalculateTotalWeight()
    {
        totalWeight = 0;
        for(int i = 0; i < table.Count; i++)
        {
            totalWeight += table[i].weight;
        }
    }


    public PowerUpBehavior GetDrop()
    {
        int roll = Random.Range(0, TotalWeight);

        for (int i = 0; i < table.Count; i++)
        {
            roll -= table[i].weight;
            if (roll < 0)
            {
                return table[i].drop;
            }
        }

        return table[0].drop;
    }
}

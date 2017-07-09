using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPCの移動手順を示す配列
public class NPCRootData : MonoBehaviour {
    int[][,] rootData = {
        new int[,] {
            {1,1,1},
            {1,0,1},
            {1,1,1},
        },

        new int[,]
        {
            {1,1,1},
        },
    };

    public int[,] GetRoot(int index)
    {
        return rootData[index];
    }
}

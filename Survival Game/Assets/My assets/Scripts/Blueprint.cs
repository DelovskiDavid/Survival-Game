using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public string itemName;

    public string req1;
    public string req2;

    public int req1Amount;
    public int req2Amount;

    public int numOfReq;

    public int amountToCraft;

    public Blueprint(int craftingAmount, string name, int reqNumber, string R1, int R1Amount, string R2, int R2Amount)
    {
        itemName = name;

        numOfReq = reqNumber;

        req1 = R1;
        req1Amount = R1Amount;

        req2 = R2;
        req2Amount = R2Amount;

        amountToCraft = craftingAmount;
    }
}

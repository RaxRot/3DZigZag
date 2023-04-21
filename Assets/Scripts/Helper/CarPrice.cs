using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPrice : MonoBehaviour
{
    [SerializeField] private int price;

    public int GetPrice()
    {
        return price;
    }
}

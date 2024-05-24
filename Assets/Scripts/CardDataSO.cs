using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    public uint id;
    public Sprite image;
}

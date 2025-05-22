using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{
    public enum HandSide { Left, Right }
    [Tooltip("Specifies which hand this glove belongs to")]
    public HandSide side;
}

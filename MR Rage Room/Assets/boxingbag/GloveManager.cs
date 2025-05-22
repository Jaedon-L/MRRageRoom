using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveManager : MonoBehaviour
{
    private GameObject leftGlove;
    private GameObject rightGlove;

    public bool glovesOn = false;
    void Start()
    {
        // Find all Glove components in the scene
        Glove[] gloves = FindObjectsOfType<Glove>(true);
        foreach (var glove in gloves)
        {
            if (glove.side == Glove.HandSide.Left)
                leftGlove = glove.gameObject;
            else if (glove.side == Glove.HandSide.Right)
                rightGlove = glove.gameObject;
        }

        // Ensure gloves start disabled
        if (leftGlove != null) leftGlove.SetActive(false);
        if (rightGlove != null) rightGlove.SetActive(false);
    }
    public void ToggleGloves()
    {
        glovesOn = !glovesOn;
        leftGlove.SetActive(glovesOn);
        rightGlove.SetActive(glovesOn);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEUGameManager : MonoBehaviour
{
    public static BEUGameManager instance;

    private void Start()
    {
        instance = this;
    }

    public void ResetGame()
    {

    }
}

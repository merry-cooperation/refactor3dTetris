﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBtnScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Debug.Log("test button)");
        Time.timeScale = 0f;
    }
}

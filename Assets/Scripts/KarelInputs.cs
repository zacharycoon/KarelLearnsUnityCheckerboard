using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarelInputs : MonoBehaviour {

    //Do not mess with this line of code
    private Karel karel;

    //Do not mess with this function
    private void Start()
    {
        karel = GetComponent<Karel>();
    }

    public void StartKarel()
    {
        //Do not mess with this code
        karel.startKarel();
        CustomCode();
        karel.finish();
    }

    public void CustomCode()
    {
        //YOUR CODE HERE!!

    }  // CustomCode   
}

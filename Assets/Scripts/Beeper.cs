/*
 * 
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beeper : MonoBehaviour
{

    private TextMesh text;
    public int number_of_beepers = 0;
    private int invisible_beepers = 0;

    /*
     * 
     */
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        text.text = "";
        invisible_beepers = number_of_beepers;
        setBeeper();
    }

    /*
     * 
     */
    public TextMesh getText()
    {
        return text;
    }

    /*
     * 
     */
    public void putBeeper(bool custom_karel_mode)
    {
        if (!custom_karel_mode)
        {
            number_of_beepers++;

            if (number_of_beepers >= 2)
            {
                text.text = number_of_beepers.ToString();
                GetComponent<MeshRenderer>().enabled = true;
            }
            else if (number_of_beepers == 1)
            {
                text.text = "";
                GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                Debug.LogError("unexpected number of beepers in Beeper:putBeeper()");
            }
            invisible_beepers = number_of_beepers;
        }
        else
        {
            invisible_beepers++;
        }
    }

    public void setBeeper()
    {
        if (number_of_beepers >= 2)
        {
            text.text = number_of_beepers.ToString();
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if (number_of_beepers == 1)
        {
            text.text = "";
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if (number_of_beepers <= 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
            number_of_beepers = 0;
        }
        else
        {
            Debug.LogError("unexpected number of beepers in Beeper:putBeeper()");
        }
    }
    /*
     * 
     */
    public void pickBeeper(bool custom_karel_mode)
    {
        if (!custom_karel_mode)
        {
            number_of_beepers--;
            if (number_of_beepers >= 2)
            {
                text.text = number_of_beepers.ToString();
            }
            else if (number_of_beepers == 1)
            {
                text.text = "";
            }
            else if (number_of_beepers <= 0)
            {
                GetComponent<MeshRenderer>().enabled = false;
                number_of_beepers = 0;
            }
            else
            {
                Debug.LogError("unexpected number of beepers in Beeper:pickBeeper()");
            }
            invisible_beepers = number_of_beepers;
        }
        else
        {
            invisible_beepers--;
            if(invisible_beepers <= 0)
            {
                invisible_beepers = 0;
            }
        }
    }

    public int GetInvisibleBeepers()
    {
        return invisible_beepers;
    }
}

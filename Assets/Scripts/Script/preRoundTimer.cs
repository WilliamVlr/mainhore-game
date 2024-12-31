using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class preRoundTimer : MonoBehaviour
{
    public Text myText;
    public float timer = 3f;
    public float preround = 10f;
    public int timeDeficit = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (myText.text != "GO!")
        {
            if (preround > 0)
            {
                preround -= Time.deltaTime * timeDeficit; // Use unscaledDeltaTime for independent countdown
            }
            else
            {
                timer -= Time.deltaTime * timeDeficit; // Timer continues even with Time.timeScale = 0
                updateText((int)timer);
            }
        }
    }

    public void updateText(int tm)
    {
        if(tm == 0)
        {
            myText.text = "GO!";
        }else
        {
            myText.text = tm.ToString();
        }
    }
}

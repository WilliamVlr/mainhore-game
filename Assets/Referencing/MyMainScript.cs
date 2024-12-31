using UnityEngine;

public class MyMainScript : MonoBehaviour
{
    public MyHelperScript helperScript; // Reference to MyHelperScript

    void Start()
    {
        helperScript = GetComponent<MyHelperScript>();
    }
    void Update()
    {
        if (helperScript != null)
        {
            helperScript.SayHello();
        }
    }
}

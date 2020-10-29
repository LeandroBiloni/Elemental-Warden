using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroButtons : MonoBehaviour
{
    public GameObject firstScreen;
    public GameObject secondScreen;
    public GameObject thirdScreen;
    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        firstScreen.SetActive(true);
        secondScreen.SetActive(false);
        thirdScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        counter++;


        switch (counter)
        {
            case 1:
                firstScreen.SetActive(false);
                secondScreen.SetActive(true);
                break;

            case 2:
                secondScreen.SetActive(false);
                thirdScreen.SetActive(true);
                break;

            case 3:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void Skip()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

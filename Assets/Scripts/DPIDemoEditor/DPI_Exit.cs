using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPI_Exit : MonoBehaviour
{

    public GameObject UIExit;

    public Button ExitButton;
    public Button BackButton;
    // Start is called before the first frame update
    void Start()
    {
        ExitButton.onClick.AddListener(()=> { Application.Quit(); });
        BackButton.onClick.AddListener(() => { UIExit.SetActive(false); });
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIExit.SetActive(true);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public GameObject pauseObjects;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
		hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0){
				Time.timeScale = 1;
				hidePaused();
			}
		}
    }
    public void pauseControl(){
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				showPaused();
			} else if (Time.timeScale == 0){
				Time.timeScale = 1;
				hidePaused();
			}
	}
    public void showPaused(){
		pauseObjects.SetActive(true);
	}
    public void hidePaused(){
		pauseObjects.SetActive(false);
	}

	public void HideButton(){
		pauseObjects.SetActive(false);
		Time.timeScale = 1;
	}
}

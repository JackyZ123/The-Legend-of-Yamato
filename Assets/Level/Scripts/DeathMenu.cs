using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Death()
    {
        print("debug");
        SceneManager.LoadScene("Menu");
    }
}

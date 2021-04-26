using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject option;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowOption()
    {
        option.SetActive(!option.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
    public void Quit()
    {
        Application.Quit();
    }
}

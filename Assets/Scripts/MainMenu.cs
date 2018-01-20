using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AnimationClip menuExitAnim;
    [SerializeField]
    private GameObject MenuPanel, ExitPanel;
    private bool exitConf;

    private void Start()
    {
        exitConf = false;
        MenuPanel.SetActive(true);
        ExitPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            ExitConfirmation();
        }
    }

    public void ExitConfirmation()
    {
        exitConf = !exitConf;
        MenuPanel.SetActive(!exitConf);
        ExitPanel.SetActive(exitConf);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void LoadScene(int _index)
    {
        StartCoroutine(LoadAfterAnim(1));
    }

    IEnumerator LoadAfterAnim(int _sceneIndex)
    {
        //MenuPanel.GetComponent<Animator>().Play("MainMenuExit");
        //yield return new WaitForSecondsRealtime(menuExitAnim.length);
        yield return new WaitForSecondsRealtime(.1f);
        SceneManager.LoadScene(_sceneIndex);
    }
}

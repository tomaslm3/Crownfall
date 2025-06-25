using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject creditsPanel;

    public void Play(string sceneName) {
        var gm = GameManager.Instance;
        if (gm != null) Destroy(gm.gameObject);

        var gwm = FindObjectOfType<GridWorldManager>();
        if (gwm != null) Destroy(gwm.gameObject);

        var uwm = FindObjectOfType<UnitWorldManager>();
        if (uwm != null) Destroy(uwm.gameObject);

        var twm = FindObjectOfType<TurnWorldManager>();
        if (twm != null) Destroy(twm.gameObject);

        SceneManager.LoadScene(sceneName);
    }

    public void ShowControls() {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void ShowCredits() {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowMenu() {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void QuitToMenu(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}

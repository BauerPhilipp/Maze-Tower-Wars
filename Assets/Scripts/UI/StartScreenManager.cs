using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScreenManager : MonoBehaviour
{

    private VisualElement root;
    private VisualElement startButton;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<VisualElement>("StartButton");
        startButton.RegisterCallback<ClickEvent>(StartGame);
    }

    private void StartGame(ClickEvent e)
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("Level01");
    }

}

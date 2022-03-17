using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] int mapPos;
    [SerializeField] int mapQuantity;
    [SerializeField] string[] mapNames;
    [SerializeField] Button doneButton;
    PlayerInputs inputs;
    float timerPress;
    Vector3 newPos;
    GameObject inputSystem;

    void Start()
    {
        inputSystem = GameObject.FindGameObjectWithTag("InputSystem");
        inputs = inputSystem.GetComponent<PlayerInputs>();
        newPos = transform.position;
        inputSystem.transform.name = "destroy";
        inputSystem.transform.tag = "SceneSelector";
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Building Scene"))
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LoadSceneManager>().newScene = mapNames[mapPos];
            Destroy(gameObject);
        }

        if (timerPress <= 0 && (inputs.Right || inputs.Left))
            timerPress = 1;
        else if (timerPress > 0)
            timerPress -= Time.deltaTime;

        if (inputs.Right && mapPos < mapQuantity - 1)
        {
            mapPos++;
            newPos = new Vector3(newPos.x -25.28f, newPos.y, newPos.z);

        }
        else if (inputs.Left && mapPos > 0)
        {
            mapPos--;
            newPos = new Vector3(newPos.x + 25.28f, newPos.y, newPos.z);
        }

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 2);

        if (mapNames[mapPos] != "" && !doneButton.interactable)
            doneButton.interactable = true;
        else if(mapNames[mapPos] == "" && doneButton.interactable)
            doneButton.interactable = false;
    }

    public void loadScene()
    {
        Destroy(inputSystem);
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Building Scene");
    }
}

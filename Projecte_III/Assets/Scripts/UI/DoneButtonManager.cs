using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButtonManager : MonoBehaviour
{
    [SerializeField] internal Color selectedBttnImgColor;
    [SerializeField] Color readyBttnImgColor;

    public int buttonsActive = 0;
    [SerializeField] string goToScene;

    bool loadingLevel = false;

    PlayersManager playersManager;
    DoneButtonScript[] doneButtonScripts;

    public GameObject AudioObject;
    public AK.Wwise.Event ActualMusic;

    // Start is called before the first frame update
    void Start()
    {
        playersManager = GameObject.FindGameObjectWithTag("PlayersManager").GetComponent<PlayersManager>();
        doneButtonScripts = new DoneButtonScript[transform.childCount];
        for(int i = 0; i < doneButtonScripts.Length; i++)
        {
            doneButtonScripts[i] = transform.GetChild(i).GetComponent<DoneButtonScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AllPlayersReady() && !loadingLevel)
        {
            loadingLevel = true;
            StartCoroutine(ChangeSceneEvent());
        }
    }


    bool AllPlayersReady()
    {
        if (!doneButtonScripts[0].isActive) return false;

        foreach(DoneButtonScript button in doneButtonScripts)
        {
            if (button.isActive && !button.isReady) return false;
        }

        return true;
    }

    public DoneButtonScript GetButton(int _idx)
    {
        return doneButtonScripts[_idx];
    }

    IEnumerator ChangeSceneEvent()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (DoneButtonScript button in doneButtonScripts)
        {
            StartCoroutine(button.LerpBttnImgColor(readyBttnImgColor));
        }
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Changing Scene");
        playersManager.numOfPlayers = buttonsActive;
        //GameObject.FindGameObjectWithTag("RadialMenuManager").GetComponent<RadialMenuSetManager>().SetModifiersToChosenRMSet(playersManager.numOfPlayers - 1);

        ActualMusic.Stop(AudioObject);

        LoadSceneManager sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LoadSceneManager>();
        sceneManager.ChangeScene(goToScene);
    }
}

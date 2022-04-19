using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersHUD : MonoBehaviour
{
    [SerializeField] Sprite[] possibleModifiers;

    [SerializeField] RandomModifierGet player;
    public int id;

    Image modifier;
    Transform[] lives;

    // Start is called before the first frame update
    void Start()
    {
        modifier = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        modifier.gameObject.SetActive(false);

        player = GameObject.Find("PlayersManager").GetComponent<PlayersManager>().GetPlayer(id).GetComponentInChildren<RandomModifierGet>();

        lives = new Transform[transform.GetChild(0).childCount];
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i] = transform.GetChild(0).GetChild(i);
        }
    }

    public void UpdateLives(int currentLives)
    {
        if(lives[currentLives-1].gameObject.activeSelf)
        {
            lives[currentLives - 1].gameObject.SetActive(false);
        }
        else
        {
            lives[currentLives - 1].gameObject.SetActive(true);
        }
    }

    public void RollModifiers()
    {
        if(!modifier.gameObject.activeSelf)
        {
            IEnumerator roll = RollModifier();
            StartCoroutine(roll);
        }
    }

    IEnumerator RollModifier()
    {
        int _modifierIdx = Random.Range(0, possibleModifiers.Length);
        int timesShown = 0;
        int initial;
        do
        {
            initial = Random.Range(0, possibleModifiers.Length);

        } while (initial == _modifierIdx);
        
        if (initial >= possibleModifiers.Length) initial -= possibleModifiers.Length;
        int currentSprite = initial + 1;
        while(timesShown < 10)
        {
            if (currentSprite >= possibleModifiers.Length) currentSprite = 0;
            if (currentSprite == initial)
                timesShown++;

            modifier.sprite = possibleModifiers[currentSprite];

            if(!modifier.gameObject.activeSelf)
                modifier.gameObject.SetActive(true);

            currentSprite++;
            yield return new WaitForSeconds(0.05f);
        }

        modifier.sprite = possibleModifiers[_modifierIdx];
        RandomModifierGet.ModifierTypes _mod = (RandomModifierGet.ModifierTypes)_modifierIdx;
        player.SetModifier(_mod);

        yield return 0;
    }

    public void ClearModifiers()
    {
        if(modifier.gameObject.activeSelf)
            modifier.gameObject.SetActive(false);
    }
}

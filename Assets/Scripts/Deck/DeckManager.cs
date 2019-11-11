using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;

public class DeckManager : MonoBehaviour
{

    public GameObject scrollItemGo;
    public RectTransform scrollContent;
    public static Dictionary<string, object> deck;
    public static bool hasDeck;
    public static List<string> inDeck;

    // Start is called before the first frame update
    void Start()
    {
        callDeck();
        inDeck = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDeck)
        {
            foreach (var scroll in deck)
            {
                var scrollDic = (Dictionary<string, object>)scroll.Value;
                if(int.Parse(scrollDic["inDeck"].ToString()) == 1)
                {
                    addItems(GameManager.allScrolls[int.Parse(scrollDic["id"].ToString())], true, scroll.Key);
                    inDeck.Add(scroll.Key);
                }
                else
                {
                    addItems(GameManager.allScrolls[int.Parse(scrollDic["id"].ToString())], false, scroll.Key);
                }
            }

            hasDeck = false;
            scrollItemGo.SetActive(false);
        }

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                goBack();
                // Quit the application
            }
        }
    }

    public void goBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void setDeck()
    {
        foreach (var scroll in deck)
        {
            var scrollDic = (Dictionary<string, object>)scroll.Value;
            UIManager.reference.Child("decks").Child(UIManager.user.UserId).Child(scroll.Key).Child("inDeck").SetValueAsync(0);
        }
        foreach (string scrollID in inDeck)
        {
            UIManager.reference.Child("decks").Child(UIManager.user.UserId).Child(scrollID).Child("inDeck").SetValueAsync(1);
        }

        SceneManager.LoadScene("Menu");
    }

    public void reload()
    {
        SceneManager.LoadScene("Deck");
    }

    public static void callDeck()
    {
        UIManager.reference.Child("decks").Child(UIManager.user.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                deck = (Dictionary<string, object>)snapshot.Value;
                DeckManager.hasDeck = true;
                Debug.Log("User deck collected.");
            }
        });
    }

    public void addItems(Scroll scroll, bool inTheDeck, string scrollKey)
    {
        if (inTheDeck)
        {
            GameObject scrollItem = Instantiate(scrollItemGo);
            scrollItem.transform.SetParent(scrollContent, true);
            scrollItem.transform.localScale = new Vector3(5f, 5f, 5f);
            scrollItem.GetComponent<EditDeck>().scrollID = scrollKey;
            scrollItem.GetComponentInChildren<TextMeshProUGUI>().text = scroll.name;
            scrollItem.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = scroll.name;
            scrollItem.transform.Find("Effect Text").GetComponent<TextMeshProUGUI>().text = scroll.effect;
            scrollItem.transform.Find("Artwork").GetComponent<Image>().sprite = scroll.artwork;
            scrollItem.transform.Find("Energy").GetComponent<Image>().sprite = scroll.energy;
            scrollItem.transform.Find("Add Remove Button").gameObject.SetActive(true);
            scrollItem.transform.Find("In Deck").gameObject.SetActive(true);
        }
        else
        {
            GameObject scrollItem = Instantiate(scrollItemGo);
            scrollItem.transform.SetParent(scrollContent, true);
            scrollItem.transform.localScale = new Vector3(5f, 5f, 5f);
            scrollItem.GetComponent<EditDeck>().scrollID = scrollKey;
            scrollItem.GetComponentInChildren<TextMeshProUGUI>().text = scroll.name;
            scrollItem.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = scroll.name;
            scrollItem.transform.Find("Effect Text").GetComponent<TextMeshProUGUI>().text = scroll.effect;
            scrollItem.transform.Find("Artwork").GetComponent<Image>().sprite = scroll.artwork;
            scrollItem.transform.Find("Energy").GetComponent<Image>().sprite = scroll.energy;
            scrollItem.transform.Find("Add Remove Button").gameObject.SetActive(true);
        }
    }
}

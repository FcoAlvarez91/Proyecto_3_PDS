using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;

public class ItemManager : MonoBehaviour
{

    public GameObject scrollItemGo;
    public RectTransform scrollContent;
    public TextMeshProUGUI goldText;
    public static int gold;

    // Start is called before the first frame update
    void Start()
    {
        gold = 0;
        foreach(Scroll scroll in GameManager.allScrolls)
        {
            addItems(scroll);
        }

        scrollItemGo.SetActive(false);

        callGold();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "Coins: " + gold + "g";
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

    public static void callGold()
    {

        UIManager.reference.Child("users").Child(UIManager.user.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, object> userDic = (Dictionary<string, object>)snapshot.Value;
                object goldObj = userDic["gold"];
                
                gold = int.Parse(goldObj.ToString());
                Debug.Log("User gold collected: " + gold);
            }
        });
    }

    public void addItems(Scroll scroll)
    {
        GameObject scrollItem = Instantiate(scrollItemGo);
        scrollItem.transform.SetParent(scrollContent, true);
        scrollItem.transform.localScale = new Vector3(5f, 5f, 5f);
        scrollItem.GetComponent<BuyScroll>().scroll = scroll;
        scrollItem.GetComponentInChildren<TextMeshProUGUI>().text = scroll.name;
        scrollItem.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = scroll.name;
        scrollItem.transform.Find("Effect Text").GetComponent<TextMeshProUGUI>().text = scroll.effect;
        scrollItem.transform.Find("Artwork").GetComponent<Image>().sprite = scroll.artwork;
        scrollItem.transform.Find("Energy").GetComponent<Image>().sprite = scroll.energy;
        scrollItem.transform.Find("Buy Button").GetChild(0).GetComponent<TextMeshProUGUI>().text = scroll.cost + "g - Buy";

    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Functions;
// using Firebase.Messaging;
// using Firebase.Extensions;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI email;
    public TextMeshProUGUI password;
    public TMP_InputField friendReq;
    public static Firebase.Auth.FirebaseAuth auth;
    public static Firebase.Auth.FirebaseUser user;
    public static DatabaseReference reference;
    public GameObject mainMenu;
    public GameObject loginMenu;
    public GameObject friendListGO;
    public GameObject newGameInviteListItem;
    public GameObject statsListGO;
    public GameObject preGameInvListGO;
    public GameObject reqGameInviteListItemGO;
    public Dictionary<object, object> requests;
    // public Button[] friendRequests;
    public GameObject FriendRequestButton;
    public RectTransform RequestsRectTransform;
    public RectTransform friendListRectTransform;
    public RectTransform newGameListRectTransform;
    public RectTransform statsGameListRT;
    public RectTransform previewInvsRT;
    public RectTransform requestInvGameListRT;
    public RectTransform activeGameListRT;
    public Dictionary<object, object> friendList;
    public Dictionary<object, object> statistics;
    public Dictionary<object, object> gameInvRequestList;
    public Dictionary<object, object> activeGamesList;
    public Dictionary<string, string> preGameInvSelection = new Dictionary<string, string>();
    private bool reqButtonsMade = false;
    private bool friendListButtonsMade = false;
    private bool gameListInviteItemsMade = false;
    private bool statisticListMade = false;
    private bool previewGameListMade = false;
    private bool gameInvReqListMade = false;
    private bool statsListMade = false;
    private bool activeGameListMade = false;
    private List<int> playersInvitedNum = new List<int>(){ 1, 2, 3 };
    public Button continueButton;
    // public FirebaseFunctions functions;

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        //functions = Firebase.Functions.DefaultInstance;
    }

    /* public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("From: " + e.Message.From);
        UnityEngine.Debug.Log("Message ID: " + e.Message.MessageId);
    } */

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                emptyLists();
                Debug.Log("Signed out " + user.UserId);
                loginMenu.SetActive(true);
                mainMenu.SetActive(false);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.Email);
                setupEventHandlers();
                emptyLists();
                getLists();
                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }

    private void setupEventHandlers()
    {
        var userDataRefs = FirebaseDatabase.DefaultInstance.GetReference("users/" + auth.CurrentUser.UserId);

        userDataRefs.ChildAdded += HandleDataChanged;
        userDataRefs.ChildChanged += HandleDataChanged;
        userDataRefs.ChildRemoved += HandleDataChanged;
        userDataRefs.ChildMoved += HandleDataChanged;

    }

    private void HandleDataChanged (object sender, ChildChangedEventArgs args)
    {
        Debug.Log("Data changed in " + auth.CurrentUser.Email);
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        emptyLists();
        getLists();
    }

    private void getLists()
    {
        getFriendList();
        getInvites();
        //getStatistics();
        getGameInvRequestList();
        getActiveGameList();
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
        // auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // auth.StateChanged += AuthStateChanged;
        // AuthStateChanged(this, null);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        // Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Debug.Log("Firebase Messaging Initialized");
    }

    // Update is called once per frame
    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                SceneManager.LoadScene("Menu");
            }
        }
    }

    /* public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    } */

    public void logIn()
    {
        Debug.Log(email.text);
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void logOut()
    {
        auth.SignOut();
        mainMenu.SetActive(false);
        loginMenu.SetActive(true);
    }

    public void newUser()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError(email.ToString());
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled. ");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError(email.ToString());
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            mainMenu.SetActive(true);
            loginMenu.SetActive(false);

        });
    }

    public void sendRequest()
    {
        var data = new Dictionary<string, object>();
        data["requesterUid"] = user.UserId;
        data["accepterEmail"] = friendReq.text;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("sendFriendRequest");
        function.CallAsync(data);
        /* string key = reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Push().Key;
        reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Child(key).SetValueAsync(user.Email); */
        AndroidNotificationManager.sentFriendRequestNotification(friendReq.text);
        friendReq.text = "";
    }

    public void startGame(string gameName)
    {
        GameManager.currentGame = gameName;
        SceneManager.LoadScene("In Game");
    }

    public void goShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void goDeck()
    {
        SceneManager.LoadScene("Deck");
    }

    public void closeApp()
    {
        Application.Quit();
    }

    public void getInvites()
    {
        var data = new Dictionary<string, object>();
        data["uid"] = auth.CurrentUser.UserId;
        data["push"] = true;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getInvites");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea" + task.Exception);
            }
            else if (task.IsCompleted && task.Result.Data != null)
            {
                requests = (Dictionary<object, object>)task.Result.Data;
            }
        });
    }

    public void getActiveGameList()
    {
        var data = new Dictionary<string, object>();
        data["uid"] = auth.CurrentUser.UserId;
        data["push"] = true;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getActiveGames");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea" + task.Exception);
            }
            else if (task.IsCompleted && task.Result.Data != null)
            {
                activeGamesList = (Dictionary<object, object>)task.Result.Data;
            }
        });
    }

    public void makeRequestButtons()
    {
        if(requests != null && reqButtonsMade == false)
        {
            foreach (KeyValuePair<object, object> request in requests)
            {
                Debug.Log("Making button for: " + (string)request.Value);
                GameObject requestAcceptButton = Instantiate(FriendRequestButton);
                Debug.Log("Button instantiated");
                Button button = requestAcceptButton.GetComponent<Button>();
                button.onClick.AddListener(() => acceptFriendRequest((string)request.Value));
                requestAcceptButton.transform.SetParent(RequestsRectTransform, false);
                requestAcceptButton.GetComponentInChildren<TextMeshProUGUI>().text = (string)request.Value;
                Debug.Log("Made button for: " + (string)request.Value);
            }
            reqButtonsMade = true;
        }
    }

    public void acceptFriendRequest(string requesterEmail)
    {
        var data = new Dictionary<string, string>();
        data["requesterEmail"] = requesterEmail;
        data["accepterUid"] = user.UserId;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("acceptInvite");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Se acepto a: " + requesterEmail);
            }
        });
        getInvites();
    }

    public void getFriendList()
    {
        var data = new Dictionary<string, object>();
        data["uid"] = auth.CurrentUser.UserId;
        data["push"] = true;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getFriends");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea" + task.Exception);
            }
            else if (task.IsCompleted && task.Result.Data != null)
            {
                friendList = (Dictionary<object, object>)task.Result.Data;
                Debug.Log("friend List retrieved ");
            }
        });
    }

    public void getGameInvRequestList()
    {
        var data = new Dictionary<string, object>();
        data["uid"] = auth.CurrentUser.UserId;
        data["push"] = true;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getGameInvites");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea" + task.Exception);
            }
            else if (task.IsCompleted && task.Result.Data != null)
            {
                gameInvRequestList = (Dictionary<object, object>)task.Result.Data;
                Debug.Log("game request List retrieved ");
            }
        });
    }

    public void makeFriendList()
    {
        if (friendList != null && friendListButtonsMade == false)
        {
            foreach (KeyValuePair<object, object> friend in friendList)
            {
                GameObject friendInListObj = Instantiate(friendListGO);
                friendInListObj.transform.SetParent(friendListRectTransform, true);
                friendInListObj.GetComponentInChildren<TextMeshProUGUI>().text = (string)friend.Value;
                friendInListObj.transform.localScale = new Vector3(1f, 1f, 1f);
                friendInListObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            friendListButtonsMade = true;
        }
    }

    public void makeGameInviteList()
    {
        if (friendList != null && gameListInviteItemsMade == false)
        {
            foreach (KeyValuePair<object, object> friend in friendList)
            {
                makeGameInviteListItem((string)friend.Value);
            }
            gameListInviteItemsMade = true;
        }
    }

    public void getStatistics()
    {
        var data = new Dictionary<string, object>();
        data["uid"] = auth.CurrentUser.Email;
        data["push"] = true;
        Debug.Log("Call for stats of " + data["email"]);
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getStatistics");
        function.CallAsync(data).ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Ta mala la wea" + task.Exception);
            }
            else if (task.IsCompleted && task.Result.Data != null)
            {
                statistics = (Dictionary<object, object>)task.Result.Data;
                Debug.Log("statistics retrieved ");
                foreach (KeyValuePair<object, object> statistic in statistics)
                {
                    Debug.Log(statistic.Key.ToString() + ": " + statistic.Value.ToString());
                }
            }
        });
    }

    public void makeGameInviteRequestList()
    {
        if(gameInvReqListMade == false)
        {
            foreach (KeyValuePair<object, object> game in gameInvRequestList)
            {
                makeGameInviteRequestListItem(game.Value.ToString());
                Debug.Log(game.Value + " invite to game!");
            }
            gameInvReqListMade = true;
        }
    }

    public void makeActiveGameList()
    {
        if(activeGameListMade == false)
        {
            foreach (KeyValuePair<object, object> game in activeGamesList)
            {
                makeActiveGameListItem(game.Value.ToString());
            }
            activeGameListMade = true;
        }
    }

    public void makeActiveGameListItem(string gameName)
    {
        GameObject gameInListObj = Instantiate(reqGameInviteListItemGO);
        gameInListObj.transform.SetParent(activeGameListRT, true);
        gameInListObj.GetComponentInChildren<TextMeshProUGUI>().text = gameName;
        Button button = gameInListObj.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => startGame(gameName));
        gameInListObj.transform.localScale = new Vector3(1f, 1f, 1f);
        gameInListObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void makeGameInviteRequestListItem(string gameName)
    {
        GameObject gameInListObj = Instantiate(reqGameInviteListItemGO);
        gameInListObj.transform.SetParent(requestInvGameListRT, true);
        gameInListObj.GetComponentInChildren<TextMeshProUGUI>().text = gameName;
        Button button = gameInListObj.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => acceptGameInvite(gameName));
        gameInListObj.transform.localScale = new Vector3(1f, 1f, 1f);
        gameInListObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void invitePlayerOnClick(string friendInList, GameObject listItem)
    {
        GameObject previewListItem = Instantiate(preGameInvListGO);
        previewListItem.transform.SetParent(previewInvsRT, true);
        previewListItem.GetComponentInChildren<TextMeshProUGUI>().text = friendInList;
        preGameInvSelection.Add("user" + playersInvitedNum[0].ToString(), friendInList);
        int order = playersInvitedNum[0];
        playersInvitedNum.RemoveAt(0);
        Debug.Log("can invite: " + playersInvitedNum.Count);
        if(playersInvitedNum.Count == 0)
        {
            deactivateInviteButtons();
        }
        previewGameListMade = true;
        Debug.Log(order.ToString());
        Button button = previewListItem.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => removePlayerFromPreviewGameInvite(listItem, order));
        previewListItem.transform.localScale = new Vector3(1f, 1f, 1f);
        previewListItem.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void makeStatsList()
    {
        if(statsListMade == false)
        {
            foreach(KeyValuePair<object,object> statistic in statistics)
            {
                makeStatsListItem(statistic.Key.ToString().Replace('_',' ') + ": " + statistic.Value.ToString());
            }
            statsListMade = true;
        }
    }

    public void makeStatsListItem(string statText)
    {
        GameObject statsListItemObj = Instantiate(statsListGO);
        statsListItemObj.transform.SetParent(statsGameListRT, true);
        statsListItemObj.GetComponentInChildren<TextMeshProUGUI>().text = statText;
        statsListItemObj.transform.localScale = new Vector3(1f, 1f, 1f);
        statsListItemObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void makeGameInviteListItem(string friend)
    {
        GameObject friendInListObj = Instantiate(newGameInviteListItem);
        friendInListObj.transform.SetParent(newGameListRectTransform, true);
        friendInListObj.GetComponentInChildren<TextMeshProUGUI>().text = friend;
        Button button = friendInListObj.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => invitePlayerOnClick(friend, friendInListObj));
        friendInListObj.transform.localScale = new Vector3(1f, 1f, 1f);
        friendInListObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    
    public void removePlayerFromPreviewGameInvite(GameObject listItem, int orderInList)
    {
        listItem.SetActive(true);
        preGameInvSelection.Remove("user" + orderInList.ToString());
        playersInvitedNum.Add(orderInList);
    }

    public void deactivateInviteButtons()
    {
        Button[] listItems = newGameListRectTransform.GetComponentsInChildren<Button>();
        Debug.Log("Deactivating " + listItems.Length.ToString() + " Buttons");
        foreach(Button item in listItems)
        {
            item.interactable = false;
        }
        foreach(KeyValuePair<string,string> invite in preGameInvSelection)
        {
            Debug.Log("ready to send invite to " + invite.Value + " key: " + invite.Key);
        }
        continueButton.interactable = true;

    }

    public void sendGameInvites()
    {
        foreach (Transform child in previewInvsRT)
        {
            GameObject.Destroy(child.gameObject);
        }
        previewGameListMade = false;
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("inviteFriendsToNewGame");
        preGameInvSelection["creator"] = auth.CurrentUser.UserId;
        function.CallAsync(preGameInvSelection).ContinueWith((task) =>
        {
            if (task.IsCompleted)
            {
                preGameInvSelection = new Dictionary<string, string>();
            }
        });
    }

    public void acceptGameInvite(string gameName)
    {
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("acceptGameInvite");
        var data = new Dictionary<string, string>();
        data["uid"] = auth.CurrentUser.UserId;
        data["gameId"] = gameName;
        function.CallAsync(data);
    }

    public void emptyLists()
    {
        foreach (Transform child in RequestsRectTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in friendListRectTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in statsGameListRT)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in newGameListRectTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in previewInvsRT)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in requestInvGameListRT)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in activeGameListRT)
        {
            GameObject.Destroy(child.gameObject);
        }
        reqButtonsMade = false;
        friendListButtonsMade = false;
        gameListInviteItemsMade = false;
        statisticListMade = false;
        previewGameListMade = false;
        gameInvReqListMade = false;
        activeGameListMade = false;
    }
}

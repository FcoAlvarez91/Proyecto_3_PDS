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

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI email;
    public TextMeshProUGUI password;
    public TextMeshProUGUI friendReq;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public DatabaseReference reference;
    public GameObject mainMenu;
    public GameObject loginMenu;
    public GameObject friendListGO;
    public TextMeshProUGUI friendInListText;
    public Dictionary<object, object> requests;
    //public Button[] friendRequests;
    public GameObject FriendRequestButton;
    public RectTransform RequestsRectTransform;
    public RectTransform friendListRectTransform;
    public TextMeshProUGUI friendRequestButtonText;
    public Dictionary<object, object> friendList;
    //public FirebaseFunctions functions;

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        //functions = Firebase.Functions.DefaultInstance;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.Email);

                mainMenu.SetActive(true);
                loginMenu.SetActive(false);
            }
        }
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
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://four-mages.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //getInvites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        getInvites();
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
            Debug.Log("newUser not set");
            // Firebase user has been created
            //reference.Child("users").Child(UserId).Child("useremail").SetValueAsync(email.text);
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.Log("User " + newUser.UserId + "being created in database");
            reference.Child("users").Child(email.text.Replace('.', ',')).Child("DisplayName").SetValueAsync(email.text);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            mainMenu.SetActive(true);
            loginMenu.SetActive(false);

        });
        //inviteWait.Wait();
    }

    public void sendRequest()
    {
        string key = reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Push().Key;
        reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Child(key).SetValueAsync(user.Email);
    }

    public void startGame()
    {
        SceneManager.LoadScene("In Game");
    }

    public void closeApp()
    {
        Application.Quit();
    }

    /*public void getFriendRequestList()
    {
        functions.GetHttpCallable("getInvites").ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //handle error..
            }

            else if (task.IsComplete)
            {
                DataSnapshot friendSnap = task.Result;
                foreach (string friend in friendSnap) {
                    Debug.Log("Friend Reqs are: " + friend);
                }
            }
        });
    }*/

    public void getInvites()
    {
        // Call the function and extract the operation from the result.
        //var inviteWait = makeRequestButtons();
        var data = new Dictionary<string, object>();
        //Debug.Log("este ese el mail: " + auth.CurrentUser.Email);
        data["email"] = auth.CurrentUser.Email;
        data["push"] = true;
        //Debug.Log("getInvites");
        var function = Firebase.Functions.FirebaseFunctions.DefaultInstance.GetHttpsCallable("getInvites");
        //Debug.Log("function = " + function.GetType());
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

    public void makeRequestButtons()
    {
        if(requests != null)
        {
            foreach (KeyValuePair<object, object> request in requests)
            {
                Debug.Log("Making button for: " + (string)request.Value);
                GameObject requestAcceptButton = Instantiate(FriendRequestButton);
                Debug.Log("Button instantiated");
                Button button = requestAcceptButton.GetComponent<Button>();
                Debug.Log("Button exist?" + (button != null));
                button.onClick.AddListener(() => acceptFriendRequest((string)request.Value));
                requestAcceptButton.transform.SetParent(RequestsRectTransform, false);
                requestAcceptButton.GetComponentInChildren<TextMeshProUGUI>().text = (string)request.Value;
                Debug.Log("Made button for: " + (string)request.Value);
            }
        }
    }

    public void acceptFriendRequest(string requesterEmail)
    {
        var data = new Dictionary<string, string>();
        //Debug.Log("este ese el mail: " + auth.CurrentUser.Email);
        data["requesterEmail"] = requesterEmail;
        data["accepterEmail"] = user.Email;
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
        data["email"] = auth.CurrentUser.Email;
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

    public void makeFriendList()
    {
        if (friendList != null)
        {
            foreach (KeyValuePair<object, object> friend in friendList)
            {
                Debug.Log("Making List item for: " + (string)friend.Value);
                GameObject friendInListObj = Instantiate(friendListGO);
                friendInListObj.transform.SetParent(friendListRectTransform, true);
                friendInListObj.GetComponentInChildren<TextMeshProUGUI>().text = (string)friend.Value;
                friendInListObj.transform.localScale = new Vector3(1f, 1f, 1f);
                friendInListObj.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Debug.Log("Made List item for: " + (string)friend.Value);
            }
        }
    }
    public void acceptInvite()
    {

    }
}

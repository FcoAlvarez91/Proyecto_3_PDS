using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

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

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
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
                Debug.Log("Signed in " + user.UserId);

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

            mainMenu.SetActive(true);
            loginMenu.SetActive(false);
            // Firebase user has been created
            //reference.Child("users").Child(UserId).Child("useremail").SetValueAsync(email.text);
            Firebase.Auth.FirebaseUser newUser = task.Result;
            reference.Child("users").Child(email.text.Replace('.', ',')).Child("DisplayName").SetValueAsync(email.text);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });
    }

    public void sendRequest()
    {
        string key = reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Push().Key;
        reference.Child("users").Child(friendReq.text.Replace('.', ',')).Child("invites").Child(key).SetValueAsync(email.text);
    }

    public void startGame()
    {
        SceneManager.LoadScene("In Game");
    }

    public void closeApp()
    {
        Application.Quit();
    }
}

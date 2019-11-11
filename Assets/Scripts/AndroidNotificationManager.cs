using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class AndroidNotificationManager
{
    public AndroidNotificationChannel defaultNotificationChannel;

    private int identifier;

    public void notifStart()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        // newFriendNotification();

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://YOUR-FIREBASE-APP.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public static void newFriendNotification()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Friend Request!",
            Text = "Someone has sent you a Friend Request!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = System.DateTime.Now,
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }

    public static void sentFriendRequestNotification(string str)
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Friend Request Sent",
            Text = "You have sent a Friend Request to " + str,
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = System.DateTime.Now,
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
    }
}

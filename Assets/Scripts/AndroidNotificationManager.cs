using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class AndroidNotificationManager : MonoBehaviour
{
    public AndroidNotificationChannel defaultNotificationChannel;

    private int identifier;

    // Start is called before the first frame update
    void Start()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        newFriendNotification();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newFriendNotification()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Friend Request!",
            Text = "Someone has sent you a Friend Request!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = System.DateTime.Now,
        };

        identifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");


    }
}

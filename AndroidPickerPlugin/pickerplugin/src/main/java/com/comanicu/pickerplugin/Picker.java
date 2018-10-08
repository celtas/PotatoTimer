package com.comanicu.pickerplugin;

import android.app.Activity;

import com.unity3d.player.UnityPlayer;

public class Picker extends Activity{
    public static Activity getCurrentActivity(){
        return UnityPlayer.currentActivity;
    }
}

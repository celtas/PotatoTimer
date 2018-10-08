using UnityEngine;

namespace PotatoTimer.Scripts {
    public class PositiveButtonListner :AndroidJavaProxy{
    public PositiveButtonListner(TimerManager tm): base("android.content.DialogInterface$OnClickListener"){
        //リスナーを作成した時に呼び出される
    }
 
    public void onClick(AndroidJavaObject obj, int value){
        //ボタンが押された時に呼び出される
    }
}
}
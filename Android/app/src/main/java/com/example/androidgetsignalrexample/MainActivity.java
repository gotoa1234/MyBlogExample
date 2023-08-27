package com.example.androidgetsignalrexample;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.widget.TextView;

//1. SignalR 依賴庫
import com.microsoft.signalr.HubConnection;
import com.microsoft.signalr.HubConnectionBuilder;
import com.microsoft.signalr.HubConnectionState;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class MainActivity extends AppCompatActivity {
    private HubConnection hubConnection;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //1. 檢查網路
        if (isNetworkAvailable(this)) {
            Log.d("MyApp", "有網路");
        } else {
            Log.d("MyApp", "無網路");
        }

        //2-1. 建立Signalr連接
        try {
            hubConnection = HubConnectionBuilder.create("http://192.168.42.177:7531/UpdateHub").build();
            // 2-2. 註冊SignalR，等待被推播
            hubConnection.on("SendUpdate", (message) -> {
                try {
                    //2-3. 執行緒更新UI上物件
                    runOnUiThread(() -> {
                        TextView textView = findViewById(R.id.Information);
                        textView.setText("SignalR Server回傳訊息：" + message);
                    });
                }
                catch (Exception e)
                {
                    Log.e("MyApp" , "錯誤訊息: "+e.getMessage());
                }

            }, String.class);

            // 3. 啟動SignalR連線
            if(hubConnection.getConnectionState() == HubConnectionState.DISCONNECTED)
            {
                Log.d("MyApp", "進行連接SignalR");
                hubConnection.start().blockingAwait();
            }
        }
        catch (Exception e)
        {
            Log.d("MyApp", e.getMessage());
        }
    }
    private Handler handler = new Handler();

    //檢查Signalr狀態
    private Runnable connectRunnable = new Runnable() {
        @Override
        public void run() {
            Log.d("MyApp", "连接状态:" + hubConnection.getConnectionState());
            //hubConnection.start();
            // 2 秒后再次执行连接操作
            handler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    connectRunnable.run(); // 执行连接操作
                }
            }, 2000);
        }
    };

    //檢查網路狀態
    public static boolean isNetworkAvailable(Context context) {
        ConnectivityManager connectivityManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivityManager != null) {
            NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
            return activeNetworkInfo != null && activeNetworkInfo.isConnected();
        }
        return false;
    }
}
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using REAL.Networks;
using REAL.Tools;
using UnityEngine;

namespace REAL.Example
{
    /// <summary>
    /// WebSocket
    /// </summary>
    public static class RealSocket
    {
        #region Variables

        private static string _url;
        private static ClientWebSocket _ws;
        private static CancellationToken _ct;
        private static Action _onConnectError;
        private static Action<WebSocketState> _onWebSocketState;
        
        // public static bool IsConnected;

        public static bool IsOpen
        {
            get
            {
                if (_ws == null)
                    return false;
                return _ws.State == WebSocketState.Open;
            }
        }
        public static bool IsConnecting
        {
            get
            {
                if (_ws == null)
                    return false;
                return _ws.State == WebSocketState.Connecting;
            }
        }

        #endregion
     
        /// <summary>
        /// Connect to socket server
        /// </summary>
        /// <param name="onlyUnityJobs">true means jobs created by using Unity will be received only</param>
        public static async void Connect(bool onlyUnityJobs = true)
        {
            try
            {
                if (_ws is { State: WebSocketState.Connecting or WebSocketState.Open }) return;
                _url = GetUri(Commons.Renderer.real.login, onlyUnityJobs);

                _ws = new ClientWebSocket();
                _ct = new CancellationToken();
                
                // Debug.LogFormat("[WebSocket] start connection {0}", _url);
                
                var uri = new Uri(_url);
                await _ws.ConnectAsync(uri, _ct);
                
                // Connected = true;

                _onWebSocketState?.Invoke(_ws.State);

                const int capacity = 2048;
                var array = new byte[capacity];
                var buffer = new ArraySegment<byte>(array);
                var receiveList = new List<byte>();

                while (true)
                {
                    receiveList.Clear();
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(buffer, CancellationToken.None);
                        var block = new byte[result.Count];
                        if (buffer.Array != null) Buffer.BlockCopy(buffer.Array, 0, block, 0, block.Length);
                        receiveList.AddRange(block);
                    } while (!result.EndOfMessage);
                
                    var bytes = receiveList.ToArray();
                    var response = Encoding.UTF8.GetString(bytes);
                    var sockResponse = JsonUtility.FromJson<SocketResponse>(response);
                    Commons.Renderer.OnMessage(sockResponse);
                }
            }
            catch (Exception ex)
            {
                // Debug.LogError(ex);
                if(!Application.isPlaying) return;
                Commons.Renderer.canvas.infoPanel.SetStatus("Socket closed!");
            }
            finally
            {
                _ws?.Dispose();
                _ws = null;
            }
        }
        
        public static void Abort()
        {
            // Connected = false;
            _ws?.Abort();
        }

        public static void Logout()
        {
            _ws?.Dispose();
            Commons.Renderer.OnOffline();
        }

        private static string GetUri(LoginCred login, bool onlyUnityJobs)
        {
            // var ul = login.userCred;
            // var pl = login.prodCred;
            var insID = login.insID;
            var appKey = login.appKey;
            var prodKey = login.prodKey;
            var uri = $"wss://{(RealNetwork.Domain)}/realAPI?app_key={appKey}&prod_key={prodKey}&ins_id={insID}";
            if (onlyUnityJobs) uri += "&exp_from=u3d";
            return uri;
        }
    }

}


using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace PlannedRout.PlayersRegistry
{
    public sealed class PlayersDataHandler : MonoBehaviour
    {
        public event Action<PlayerData?> PlayerHasCheckedEvent = delegate { };

        private readonly static object locker = new object();
        private sealed class PlayerDataExistingChecking
        {
            private PlayerDataExistingChecking() { }
            public PlayerDataExistingChecking(string mail,string phoneNumber)
            {
                Mail = mail;
                PhoneNumber= phoneNumber;
            }

            private readonly string Mail;
            private readonly string PhoneNumber;
            private Thread CurrentCheckThread;
            public bool IsDoneChecking_=> CurrentCheckThread == null;
            public PlayerData? PlayerData_{ get; private set; } =null;

            public void StartChecking()
            {
                CurrentCheckThread = new Thread(new ThreadStart(CheckThread));
                CurrentCheckThread.Start();
            }
            public void StopChecking()
            {
                if (CurrentCheckThread != null)
                {
                    CurrentCheckThread.Abort();
                }
            }
            private void CheckThread()
            {
                lock (locker)
                {
                    string path =
#if UNITY_EDITOR
                    PlayersDataPath_Editor;
#else
                    PlayersDataPath;
#endif

                    using (StreamReader sr = new StreamReader(path))
                    {
                        while (sr.Peek() != -1)
                        {
                            var str = sr.ReadLine();
                            if (str.Contains(Mail)||str.Contains(PhoneNumber))
                            {
                                object packedData = JsonUtility.FromJson<PlayerData>(str);
                                if (packedData == null)
                                    break;
                                PlayerData data = (PlayerData)packedData;
                                if ((Mail.Length>0&& data.Mail == Mail )||
                                    (PhoneNumber.Length>0&& data.PhoneNumber == PhoneNumber))
                                {
                                    PlayerData_ = (PlayerData?)data;
                                    break;
                                }
                            }
                        }
                    }
                    CurrentCheckThread = null;
                }
            }
        }
        private sealed class PlayerDataWriting
        {
            private PlayerDataWriting() { }
            public PlayerDataWriting(PlayerData writtenData)
            {
                WrittenData = writtenData;
            }

            private readonly PlayerData WrittenData;
            private Thread CurrentWrittingThread;

            public void StartWriting()
            {
                CurrentWrittingThread = new Thread(new ThreadStart(WrittingThread));
                CurrentWrittingThread.Start();
            }
            private void WrittingThread()
            {
                lock (locker)
                {
                    string path =
#if UNITY_EDITOR
                        PlayersDataPath_Editor;
#else
                        PlayersDataPath;
#endif
                    using (StreamWriter writer=new StreamWriter(path, true))
                    {
                        string data = JsonUtility.ToJson(WrittenData);
                        writer.WriteLine(data);
                    }
                }
            }
        }

        public const string PlayersDataPath = "PlayersData.json";
#if UNITY_EDITOR
        public const string PlayersDataPath_Editor = "Assets/Scripts/Editor/PlayersData.json";
#endif
        
        public static PlayersDataHandler Instance_ { get; private set; }

        private PlayerDataExistingChecking CurrentChecking;

        private void Awake()
        {
            if (Instance_ != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance_ = this;
            enabled = false;
        }
        private void Update()
        {
            if (CurrentChecking.IsDoneChecking_)
            {
                PlayerHasCheckedEvent(CurrentChecking.PlayerData_);
                enabled = false;
                CurrentChecking = null;
            }
        }
        public void AsyncStartCheckingPlayerData(string mail,string phoneNumber)
        {
            if (mail.Length <= 0 && phoneNumber.Length <= 0)
            {
                PlayerHasCheckedEvent(null);
                return;
            }
            if (CurrentChecking != null)
            {
                CurrentChecking.StopChecking();
            }
            CurrentChecking=new PlayerDataExistingChecking(mail,phoneNumber);
            CurrentChecking.StartChecking();
            enabled = true;
        }
        public int GetLinesCount()
        {
            string path =
#if UNITY_EDITOR
                PlayersDataPath_Editor;
#else
                PlayersDataPath;
#endif
            int count = 0;
            using (StreamReader reader=new StreamReader(path))
            {
                string str = reader.ReadLine();
                while (str != null&&str.Length>1)
                {
                    count++;
                    str = reader.ReadLine();
                }
            }
            return count;
        }
        public void AsyncStartWritingPlayerData(PlayerData data)
        {
            var writer = new PlayerDataWriting(data);
            writer.StartWriting();
        }
    }
}
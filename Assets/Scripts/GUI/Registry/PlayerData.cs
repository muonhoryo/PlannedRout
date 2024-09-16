


using System;

namespace PlannedRout.PlayersRegistry
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerData(int dataNumber,string name,string mail,string phoneNumber)
        {
            DataNumber = dataNumber;
            Name = name;
            Mail = mail;
            PhoneNumber = phoneNumber;
        }

        public int DataNumber;
        public string Name;
        public string Mail;
        public string PhoneNumber;
    }
}
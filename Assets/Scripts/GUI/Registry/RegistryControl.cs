

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout.PlayersRegistry
{
    public sealed class RegistryControl : MonoBehaviour
    {
        private static char FieldInputSymbol;

        public event Action AddSymbolEvent = delegate { };
        public event Action RemoveSymbolEvent = delegate { };
        public event Action ChangeHandledFieldEvent = delegate { };

        public event Action<string> UpdateNameInfoEvent = delegate { };
        public event Action<string> UpdateMailInfoEvent = delegate { };
        public event Action<string> UpdateNumberInfoEvent = delegate { };

        private sealed class RegistryActiveFieldHandler
        {
            public event Action ResetInputSymbolShowingEvent = delegate { };

            public enum InputFieldType : byte
            {
                Name,
                Mail,
                Number
            }

            private RegistryActiveFieldHandler() { }
            public RegistryActiveFieldHandler(string fieldText,InputFieldType fieldType)
            {
                FieldText_Main_ = fieldText;
                HandledFieldType=fieldType;
                ResetInputSymbolShowing();
            }

            public string FieldText_Main_ { get; private set; }
            public string FieldText_Showed_ { get; private set; }
            private bool IsShowInputSymbol = true;
            public readonly InputFieldType HandledFieldType;
            
            public void AddSymbol(char sym)
            {
                FieldText_Main_ += sym;
                ResetInputSymbolShowing();
            }
            public void RemoveLastSymbol() 
            {
                if(FieldText_Main_.Length>0)
                    FieldText_Main_=FieldText_Main_.Remove(FieldText_Main_.Length - 1);
                ResetInputSymbolShowing();
            }

            public void ResetInputSymbolShowing()
            {
                ShowTextWithInputSymbol();
                ResetInputSymbolShowingEvent();
            }
            private void ShowTextWithInputSymbol()
            {
                IsShowInputSymbol = true;
                FieldText_Showed_ = FieldText_Main_ + FieldInputSymbol;
            }
            private void ShowRowText()
            {
                IsShowInputSymbol = false;
                FieldText_Showed_ = FieldText_Main_;
            }
            public void TurnNextForm() 
            {
                if (IsShowInputSymbol)
                {
                    ShowRowText();
                }
                else
                {
                    ShowTextWithInputSymbol();
                }
            }
        }

        [SerializeField] private Text NameField;
        [SerializeField] private Text MailField;
        [SerializeField] private Text NumberField;
        [SerializeField] private char InputSymbol;
        [SerializeField] private float InputSymbolBlinkingTime;
        [SerializeField] private string UpInputName;
        [SerializeField] private string DownInputName;
        [SerializeField] private string AcceptInputName;
        [SerializeField] private string RemoveLastInputName;
        [SerializeField] private StartButton StartButton;

        private Text HandledInputField;
        private RegistryActiveFieldHandler CurrentInputHandler;
        private Coroutine UpdateHandlerCoroutine;

        private bool IsCheckPlayerData = false;
        private PlayerData? GottenPlayerData = null;

        private void Awake()
        {
            FieldInputSymbol = InputSymbol;
            StartButton.enabled = false;
            AssignHandler(NameField, RegistryActiveFieldHandler.InputFieldType.Name);
        }
        private void Update()
        {
            if (Input.GetButtonDown(UpInputName))
            {
                MoveUp();
            }
            else if (Input.GetButtonDown(DownInputName))
            {
                MoveDown();
            }
            else
            {
                if (Input.GetButtonDown(AcceptInputName))
                {
                    Accept();
                }
                else if (CurrentInputHandler != null)
                {
                   if (Input.GetButtonDown(RemoveLastInputName))
                   {
                       RemoveLast();
                   }
                   else
                   {
                       CheckSymbolInput();
                   }
                }
            }
        }
        private void MoveDown()
        {
            if (CurrentInputHandler == null)
                return;
            RegistryActiveFieldHandler.InputFieldType prevFieldType = CurrentInputHandler.HandledFieldType;
            if (prevFieldType == RegistryActiveFieldHandler.InputFieldType.Mail)
            {
                StartMailAndPhoneNumChecking(CurrentInputHandler.FieldText_Main_, NumberField.text);
            }
            else if (prevFieldType == RegistryActiveFieldHandler.InputFieldType.Number)
            {
                StartMailAndPhoneNumChecking(MailField.text, CurrentInputHandler.FieldText_Main_);
            }
            if (prevFieldType != RegistryActiveFieldHandler.InputFieldType.Number)
            {
                bool isNameCurrentField = prevFieldType == RegistryActiveFieldHandler.InputFieldType.Name;
                AssignNewHandler(isNameCurrentField ? MailField : NumberField, prevFieldType + 1);
            }
            else
            {
                HandledInputField.text = CurrentInputHandler.FieldText_Main_;
                CurrentInputHandler = null;
                HandledInputField = null;
                if (UpdateHandlerCoroutine != null)
                    StopCoroutine(UpdateHandlerCoroutine);
                StartButton.Select();
            }
            CheckAllFieldsFilling();
            ChangeHandledFieldEvent();
        }
        private void MoveUp()
        {
            if(CurrentInputHandler==null)
            {
                AssignHandler(NumberField, RegistryActiveFieldHandler.InputFieldType.Number);
                StartButton.Unselect();
            }
            else
            {
                var prevFieldType = CurrentInputHandler.HandledFieldType;
                if (prevFieldType == RegistryActiveFieldHandler.InputFieldType.Mail)
                {
                    StartMailAndPhoneNumChecking(CurrentInputHandler.FieldText_Main_, NumberField.text);
                }
                else if (prevFieldType == RegistryActiveFieldHandler.InputFieldType.Number)
                {
                    StartMailAndPhoneNumChecking(MailField.text, CurrentInputHandler.FieldText_Main_);
                }

                if (prevFieldType != RegistryActiveFieldHandler.InputFieldType.Name)
                {
                    bool isNumberCurrentField = prevFieldType == RegistryActiveFieldHandler.InputFieldType.Number;
                    AssignNewHandler(isNumberCurrentField ? MailField : NameField, prevFieldType - 1);
                    ChangeHandledFieldEvent();
                }
                CheckAllFieldsFilling();
            }
        }
        private void Accept()
        {
            if (CurrentInputHandler == null)
            {
                if (StartButton.enabled)
                {
                    enabled = false;
                    if (IsCheckPlayerData)
                    {
                        DelayedGameStart();
                    }
                    else
                    {
                        StartGame();
                    }
                }
            }
            else
            {
                MoveDown();
            }
        }
        private void RemoveLast()
        {
            bool wasEmpty = CurrentInputHandler.FieldText_Main_.Length == 0;
            CurrentInputHandler.RemoveLastSymbol();
            if (!wasEmpty)
            {
                UpdateInfo();
                RemoveSymbolEvent();
            }
        }
        private void CheckSymbolInput()
        {
            string inputStr = Input.inputString;
            if (inputStr.Length > 0)
            {
                CurrentInputHandler.AddSymbol(inputStr[0]);
                UpdateInfo();
                AddSymbolEvent();
            }
        }
        private void UpdateInfo()
        {
            switch (CurrentInputHandler.HandledFieldType)
            {
                case RegistryActiveFieldHandler.InputFieldType.Name:
                    UpdateNameInfoEvent(CurrentInputHandler.FieldText_Main_);
                    break;
                case RegistryActiveFieldHandler.InputFieldType.Mail:
                    UpdateMailInfoEvent(CurrentInputHandler.FieldText_Main_);
                    break;
                case RegistryActiveFieldHandler.InputFieldType.Number:
                    UpdateNumberInfoEvent(CurrentInputHandler.FieldText_Main_);
                    break;
            }
        }
        private void CheckAllFieldsFilling()
        {
            bool isFill;
            if (CurrentInputHandler == null)
            {
                isFill = NameField.text.Length > 0 && NumberField.text.Length > 0;
            }
            else
            {
                isFill = NameField.text.Length > ((CurrentInputHandler.HandledFieldType == RegistryActiveFieldHandler.InputFieldType.Name) ? 1 : 0) &&
                NumberField.text.Length> ((CurrentInputHandler.HandledFieldType == RegistryActiveFieldHandler.InputFieldType.Number) ? 1 : 0);
            }

            if(isFill)
            {
                StartButton.enabled = true;
            }
            else
            {
                StartButton.enabled = false;
            }
        }
        private void StartMailAndPhoneNumChecking(string mail,string phoneNumber)
        {
            void GetPlayerData(PlayerData? data)
            {
                PlayersDataHandler.Instance_.PlayerHasCheckedEvent -= GetPlayerData;
                IsCheckPlayerData = false;
                GottenPlayerData = data;
            }
            if(!IsCheckPlayerData)
                PlayersDataHandler.Instance_.PlayerHasCheckedEvent += GetPlayerData;
            IsCheckPlayerData = true;
            PlayersDataHandler.Instance_.AsyncStartCheckingPlayerData(mail,phoneNumber);
        }
        private void StartGame()
        {
            PlayerData data;
            if (GottenPlayerData == null)
            {
                data = new PlayerData(PlayersDataHandler.Instance_.GetLinesCount() + 1, NameField.text, MailField.text, NumberField.text);
                PlayersDataHandler.Instance_.AsyncStartWritingPlayerData(data);
            }
            else
            {
                data = (PlayerData)GottenPlayerData;
            }
            PlayerDataContainer.Instance_.CurrentPlayerData_ = data;
            StartButton.StartGame();
        }
        private void DelayedGameStart()
        {
            void StartGameAction(PlayerData? data)
            {
                PlayersDataHandler.Instance_.PlayerHasCheckedEvent -= StartGameAction;
                StartGame();
            }
            PlayersDataHandler.Instance_.PlayerHasCheckedEvent += StartGameAction;
        }

        private void AssignNewHandler(Text newHandledInputField,RegistryActiveFieldHandler.InputFieldType fieldType)
        {
            HandledInputField.text = CurrentInputHandler.FieldText_Main_;
            AssignHandler(newHandledInputField, fieldType);
        }
        private void AssignHandler(Text handledInputField,RegistryActiveFieldHandler.InputFieldType fieldType)
        {
            HandledInputField = handledInputField;
            CurrentInputHandler = new RegistryActiveFieldHandler(HandledInputField.text, fieldType);
            CurrentInputHandler.ResetInputSymbolShowingEvent += ResetHandlerAction;
            ResetHandlerAction();
        }
        private IEnumerator UpdateHandledFieldShowing()
        {
            while (true)
            {
                yield return new WaitForSeconds(InputSymbolBlinkingTime);
                CurrentInputHandler.TurnNextForm();
                HandledInputField.text = CurrentInputHandler.FieldText_Showed_;
            }
        }
        private void ResetHandlerAction()
        {
            if(UpdateHandlerCoroutine!=null)
                StopCoroutine(UpdateHandlerCoroutine);
            HandledInputField.text = CurrentInputHandler.FieldText_Showed_;
            UpdateHandlerCoroutine = StartCoroutine(UpdateHandledFieldShowing());
        }
    }
}
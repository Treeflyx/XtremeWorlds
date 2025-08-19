#nullable disable

using System;
using Client.Game.UI;
using Client.Game.UI.Windows;
using Core.Configurations;
using Core.Globals;

public class Crystalshire
{
    public void UpdateWindow_Login()
    {
        var window = WindowLoader.FromLayout("winLogin");

        var username = SettingsManager.Instance.SaveUsername ? SettingsManager.Instance.Username : string.Empty;

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = Client.General.DestroyGame;
        window.GetChild("txtUsername").Text = username;
        window.GetChild("chkSaveUsername").Value = SettingsManager.Instance.SaveUsername ? 1 : 0;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinLogin.OnLogin;
        window.GetChild("btnExit").CallBack[(int) ControlState.MouseDown] = WinLogin.OnExit;
        window.GetChild("btnRegister").CallBack[(int) ControlState.MouseDown] = WinLogin.OnRegister;

        Gui.SetActiveControl(window, username.Length == 0 ? "txtUsername" : "txtPassword");
    }

    public void UpdateWindow_Register()
    {
        var window = WindowLoader.FromLayout("winRegister");

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinRegister.OnClose;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinRegister.OnRegister;
        window.GetChild("btnExit").CallBack[(int) ControlState.MouseDown] = WinRegister.OnClose;

        Gui.SetActiveControl(window, "txtUsername");
    }

    public void UpdateWindow_NewChar()
    {
        var window = WindowLoader.FromLayout("winNewChar");

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinNewChar.OnCancel;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinNewChar.OnAccept;
        window.GetChild("btnCancel").CallBack[(int) ControlState.MouseDown] = WinNewChar.OnCancel;
        window.GetChild("picScene").OnDraw = WinNewChar.OnDrawSprite;
        window.GetChild("btnLeft").CallBack[(int) ControlState.MouseDown] = WinNewChar.OnLeftClick;
        window.GetChild("btnRight").CallBack[(int) ControlState.MouseDown] = WinNewChar.OnRightClick;

        Gui.SetActiveControl(window, "txtName");
    }

    public void UpdateWindow_Chars()
    {
        var window = WindowLoader.FromLayout("winChars");

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinChars.OnClose;
        window.GetChild("picScene_3").OnDraw = WinChars.OnDraw;
        window.GetChild("btnSelectChar_1").CallBack[(int) ControlState.MouseDown] = WinChars.OnSelectCharacter1Click;
        window.GetChild("btnCreateChar_1").CallBack[(int) ControlState.MouseDown] = WinChars.OnCreateCharacter1Click;
        window.GetChild("btnDelChar_1").CallBack[(int) ControlState.MouseDown] = WinChars.OnDeleteCharacter1Click;
        window.GetChild("btnSelectChar_2").CallBack[(int) ControlState.MouseDown] = WinChars.OnSelectCharacter2Click;
        window.GetChild("btnCreateChar_2").CallBack[(int) ControlState.MouseDown] = WinChars.OnCreateCharacter2Click;
        window.GetChild("btnDelChar_2").CallBack[(int) ControlState.MouseDown] = WinChars.OnDeleteCharacter2Click;
        window.GetChild("btnSelectChar_3").CallBack[(int) ControlState.MouseDown] = WinChars.OnSelectCharacter3Click;
        window.GetChild("btnCreateChar_3").CallBack[(int) ControlState.MouseDown] = WinChars.OnCreateCharacter3Click;
        window.GetChild("btnDelChar_3").CallBack[(int) ControlState.MouseDown] = WinChars.OnDeleteCharacter3Click;
    }

    public void UpdateWindow_Jobs()
    {
        var window = WindowLoader.FromLayout("winJobs");

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinJobs.OnClose;
        window.GetChild("picParchment").OnDraw = WinJobs.OnDrawSprite;
        window.GetChild("btnLeft").CallBack[(int) ControlState.MouseDown] = WinJobs.OnLeftClick;
        window.GetChild("btnRight").CallBack[(int) ControlState.MouseDown] = WinJobs.OnRightClick;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinJobs.OnAccept;
        window.GetChild("picOverlay").CallBack[(int) ControlState.MouseDown] = WinJobs.OnClose;
        window.GetChild("picOverlay").OnDraw = WinJobs.OnDrawDescription;
    }

    public void UpdateWindow_Dialogue()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winDialogue",
            caption: "Warning",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 348, height: 145,
            icon: 38,
            visible: false,
            xOffset: 3, yOffset: 5,
            designNorm: Design.WindowNormal,
            designHover: Design.WindowNormal,
            designMousedown: Design.WindowNormal,
            canDrag: false);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinDialogue.OnClose);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 335, height: 113,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 103, top: 44, width: 144, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblHeader",
            left: 103, top: 40, width: 144, height: 10,
            text: "Header",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreateTextbox(
            windowIndex: windowIndex,
            name: "txtInput",
            left: 93, top: 75, width: 162, height: 18,
            font: Font.Arial,
            align: Alignment.Center,
            xOffset: 5, yOffset: 2,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblBody_1",
            left: 15, top: 60, width: 314, height: 10,
            text: "Invalid username or password.",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblBody_2",
            left: 15, top: 75, width: 314, height: 10,
            text: "Please try again!",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnYes",
            left: 104, top: 98, width: 68, height: 24,
            text: "Yes",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinDialogue.OnYes);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnNo",
            left: 180, top: 98, width: 68, height: 24,
            text: "No",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinDialogue.OnNo);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnOkay",
            left: 140, top: 98, width: 68, height: 24,
            text: "Okay",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinDialogue.OnOkay);

        Gui.SetActiveControl(windowIndex, Gui.GetControlIndex("winDialogue", "txtInput"));
    }

    public void UpdateWindow_Party()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winParty",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 4, top: 78, width: 252, height: 158,
            icon: 0,
            visible: false,
            designNorm: Design.WindowParty,
            designHover: Design.WindowParty,
            designMousedown: Design.WindowParty,
            canDrag: false);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName1",
            left: 60, top: 20, width: 173, height: 10,
            text: "Richard - Level 10",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName2",
            left: 60, top: 60, width: 173, height: 10,
            text: "Anna - Level 18",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName3",
            left: 60, top: 100, width: 173, height: 10,
            text: "Doleo - Level 25",
            font: Font.Arial);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP1",
            left: 58, top: 34, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP2",
            left: 58, top: 74, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP3",
            left: 58, top: 114, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP1",
            left: 58, top: 44, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP2",
            left: 58, top: 84, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP3",
            left: 58, top: 124, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP1",
            left: 58, top: 34, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP2",
            left: 58, top: 74, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP3",
            left: 58, top: 114, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP1",
            left: 58, top: 44, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP2",
            left: 58, top: 84, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP3",
            left: 58, top: 124, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picChar1",
            left: 20, top: 20, width: 32, height: 32,
            texturePath: DataPath.Characters);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picChar2",
            left: 20, top: 60, width: 32, height: 32,
            texturePath: DataPath.Characters);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picChar3",
            left: 20, top: 100, width: 32, height: 32,
            texturePath: DataPath.Characters);
    }

    public void UpdateWindow_Trade()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winTrade",
            caption: "Trading with [Name]",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 412, height: 386,
            icon: 112,
            visible: false,
            xOffset: 2, yOffset: 5,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            onDraw: WinTrade.OnDraw);

        Gui.CentralizeWindow(windowIndex);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 36, height: 36,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinTrade.OnClose);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 10, top: 312, width: 392, height: 66,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 36, top: 30, width: 142, height: 9,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblYourTrade",
            left: 36, top: 27, width: 142, height: 9,
            text: "Robin's Offer",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 236, top: 30, width: 142, height: 9,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblTheirTrade",
            left: 236, top: 27, width: 142, height: 9,
            text: "Richard's Offer",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 134, top: 340, width: 68, height: 24,
            text: "Accept",
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinTrade.OnAccept);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnDecline",
            left: 210, top: 340, width: 68, height: 24,
            text: "Decline",
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinTrade.OnClose);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStatus",
            left: 114, top: 322, width: 184, height: 10,
            text: "",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 25, top: 330, width: 100, height: 10,
            text: "Total Value",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex, "lblBlank",
            285, 330, 100, 10,
            "Total Value",
            Font.Georgia,
            Alignment.Center);

        Gui.CreateLabel(
            windowIndex,
            "lblYourValue",
            25, 344, 100, 10,
            "52,812g",
            Font.Georgia,
            Alignment.Center);

        Gui.CreateLabel(
            windowIndex, "lblTheirValue",
            285, 344, 100, 10,
            "12,531g",
            Font.Georgia,
            Alignment.Center);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picYour", left: 14, top: 46, width: 184, height: 260,
            callbackMousedown: WinTrade.OnYourTradeMouseMove,
            callbackMousemove: WinTrade.OnYourTradeMouseMove,
            callbackDblclick: WinTrade.OnYourTradeClick,
            onDraw: Gui.DrawYourTrade);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picTheir",
            left: 214, top: 46, width: 184, height: 260,
            callbackMousedown: WinTrade.OnTheirTradeMouseMove,
            callbackMousemove: WinTrade.OnTheirTradeMouseMove,
            callbackDblclick: WinTrade.OnTheirTradeMouseMove,
            onDraw: Gui.DrawTheirTrade);
    }

    public void UpdateWindow_EscMenu()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winEscMenu",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 210, height: 156,
            icon: 0,
            visible: false,
            designNorm: Design.WindowNoBar,
            designHover: Design.WindowNoBar,
            designMousedown: Design.WindowNoBar,
            canDrag: false);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 198, height: 144,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnReturn",
            left: 16, top: 16, width: 178, height: 28,
            text: "Return to Game",
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinEscMenu.OnClose);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnOptions",
            left: 16, top: 48, width: 178, height: 28,
            text: "Options",
            designNorm: Design.Orange,
            designHover: Design.OrangeHover,
            designMousedown: Design.OrangeClick,
            callbackMousedown: WinEscMenu.OnOptionsClick);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnMainMenu",
            left: 16, top: 80, width: 178, height: 28,
            text: "Back to Main Menu",
            designNorm: Design.Blue,
            designHover: Design.BlueHover,
            designMousedown: Design.BlueClick,
            callbackMousedown: WinEscMenu.OnMainMenuClick);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnExit",
            left: 16, top: 112, width: 178, height: 28,
            text: "Exit the Game",
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinEscMenu.OnExitClick);
    }

    public void UpdateWindow_Bars()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winBars",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 10, top: 10, width: 239, height: 77,
            icon: 0,
            visible: false,
            designNorm: Design.WindowNoBar,
            designHover: Design.WindowNoBar,
            designMousedown: Design.WindowNoBar,
            canDrag: false,
            clickThrough: true);

        Gui.ZOrderCon = 0;

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 227, height: 65,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picHP_Blank",
            left: 15, top: 15, width: 209, height: 13,
            imageNorm: 24,
            imageHover: 24,
            imageMousedown: 24);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picSP_Blank",
            left: 15, top: 32, width: 209, height: 13,
            imageNorm: 25,
            imageHover: 25,
            imageMousedown: 25);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picEXP_Blank",
            left: 15, top: 49, width: 209, height: 13,
            imageNorm: 26,
            imageHover: 26,
            imageMousedown: 26);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 0, top: 0, width: 0, height: 0,
            onDraw: WinBars.OnDraw);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picHealth",
            left: 16, top: 10, width: 44, height: 14,
            imageNorm: 21,
            imageHover: 21,
            imageMousedown: 21);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picSpirit",
            left: 16, top: 28, width: 44, height: 14,
            imageNorm: 22,
            imageHover: 22,
            imageMousedown: 22);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picExperience",
            left: 16, top: 45, width: 74, height: 14,
            imageNorm: 23,
            imageHover: 23,
            imageMousedown: 23);

        Gui.CreateLabel(
            windowIndex: Gui.Windows.Count,
            name: "lblHP",
            left: 15, top: 14, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex: Gui.Windows.Count,
            name: "lblMP",
            left: 15, top: 30, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblEXP",
            left: 15, top: 48, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            align: Alignment.Center);
    }

    public void UpdateWindow_Chat()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winChat",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 8, top: Client.GameState.ResolutionHeight - 178, width: 352, height: 152,
            icon: 0,
            visible: false,
            canDrag: false);

        Gui.ZOrderCon = 0;

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkGame",
            left: 10, top: 2, width: 49, height: 23,
            text: "Game",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnGameChannelClicked);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkMap",
            left: 60, top: 2, width: 49, height: 23,
            text: "Map",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnMapChannelClicked);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkGlobal",
            left: 110, top: 2, width: 49, height: 23,
            text: "Global",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnBroadcastChannelClicked);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkParty",
            left: 160, top: 2, width: 49, height: 23,
            text: "Party",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnPartyChannelClicked);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkClient.Guild",
            left: 210, top: 2, width: 49, height: 23,
            text: "Guild",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnGuildChannelClicked);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkPlayer",
            left: 260, top: 2, width: 49, height: 23,
            text: "Player",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnPrivateChannelClicked);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picNull",
            left: 0, top: 0, width: 0, height: 0,
            onDraw: WinChat.OnDraw);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnChat",
            left: 296, top: 140, width: 48, height: 20,
            text: "Say",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackNorm: WinChat.OnSayClick);

        Gui.CreateTextbox(
            windowIndex: windowIndex,
            name: "txtChat",
            left: 12, top: 143, width: 352, height: 25,
            visible: false);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnUp",
            left: 328, top: 28, width: 10, height: 13,
            imageNorm: 4,
            imageHover: 52,
            imageMousedown: 4,
            callbackMousedown: WinChat.OnUpButtonMouseDown);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnDown",
            left: 327, top: 122, width: 10, height: 13,
            imageNorm: 5,
            imageHover: 53,
            imageMousedown: 5,
            callbackMousedown: WinChat.OnDownButtonMouseDown);

        Gui.Windows[windowIndex].Controls[Gui.GetControlIndex("winChat", "btnUp")].CallBack[(int) ControlState.MouseUp] = WinChat.OnUpButtonMouseUp;
        Gui.Windows[windowIndex].Controls[Gui.GetControlIndex("winChat", "btnDown")].CallBack[(int) ControlState.MouseUp] = WinChat.OnDownButtonMouseUp;

        Gui.SetActiveControl(windowIndex, Gui.GetControlIndex("winChat", "txtChat"));

        var window = Gui.Windows[windowIndex];

        window.Controls[Gui.GetControlIndex("winChat", "chkGame")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Game];
        window.Controls[Gui.GetControlIndex("winChat", "chkMap")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Map];
        window.Controls[Gui.GetControlIndex("winChat", "chkGlobal")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Broadcast];
        window.Controls[Gui.GetControlIndex("winChat", "chkParty")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Party];
        window.Controls[Gui.GetControlIndex("winChat", "chkkGuild")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Guild];
        window.Controls[Gui.GetControlIndex("winChat", "chkPlayer")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Private];
    }

    public void UpdateWindow_ChatSmall()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winChatSmall",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 8, top: 0, width: 0, height: 0,
            icon: 0,
            visible: false,
            onDraw: WinChat.OnDrawSmall,
            canDrag: false,
            clickThrough: true);

        Gui.ZOrderCon = 0;

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblMsg",
            left: 12, top: 140, width: 286, height: 25,
            text: "Press 'Enter' to open chat",
            font: Font.Georgia);
    }

    public void UpdateWindow_Hotbar()
    {
        Gui.CreateWindow(
            name: "winHotbar",
            caption: "",
            font: Font.Georgia, zOrder: Gui.ZOrderWin,
            left: 432, top: 10, width: 418, height: 36, icon: 0,
            visible: false,
            callbackMousemove: WinHotBar.OnMouseMove,
            callbackMousedown: WinHotBar.OnMouseDown,
            callbackDblclick: WinHotBar.OnDoubleClick,
            onDraw: WinHotBar.OnDraw,
            canDrag: false,
            zChange: 0);
    }

    public void UpdateWindow_Menu()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winMenu", caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: Client.GameState.ResolutionWidth - 229,
            top: Client.GameState.ResolutionHeight - 31,
            width: 229, height: 30,
            icon: 0,
            visible: false,
            canDrag: false,
            clickThrough: true);

        Gui.ZOrderCon = 0;

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWood",
            left: 0, top: 5, width: 228, height: 20,
            designNorm: Design.Wood,
            designHover: Design.Wood,
            designMousedown: Design.Wood);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnChar",
            left: 8, top: 0, width: 29, height: 29,
            icon: 108,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinMenu.OnCharacterClick,
            xOffset: -1, yOffset: -2,
            tooltip: "Character (C)");

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnInv",
            left: 44, top: 0, width: 29, height: 29,
            icon: 1,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinMenu.OnInventoryClick,
            xOffset: -1, yOffset: -2,
            tooltip: "Inventory (I)");

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnSkills",
            left: 82, top: 0, width: 29, height: 29,
            icon: 109,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinMenu.OnSkillsClick,
            xOffset: -1, yOffset: -2,
            tooltip: "Skills (K)");

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnMap",
            left: 119, top: 0, width: 29, height: 29,
            icon: 106,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnMapClick,
            xOffset: -1, yOffset: -2);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClient.Guild",
            left: 155, top: 0, width: 29, height: 29,
            icon: 107,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnGuildClick,
            xOffset: -1, yOffset: -1);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnQuest",
            left: 190, top: 0, width: 29, height: 29,
            icon: 23,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackNorm: null,
            callbackHover: null,
            callbackMousedown: WinMenu.OnQuestClick,
            callbackMousemove: null,
            callbackDblclick: null,
            xOffset: -1, yOffset: -2);
    }

    public void UpdateWindow_Inventory()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winInventory",
            caption: "Inventory",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 202, height: 319,
            icon: 1,
            visible: false,
            xOffset: 2, yOffset: 7,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            callbackMousemove: WinInventory.OnMouseMove,
            callbackMousedown: WinInventory.OnMouseDown,
            callbackDblclick: WinInventory.OnDoubleClick,
            onDraw: WinInventory.OnDraw);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinMenu.OnInventoryClick);

        Gui.CreatePictureBox(
            windowIndex,
            "picBlank",
            8, 293, 186, 18,
            imageNorm: 67,
            imageHover: 67,
            imageMousedown: 67);
    }

    public void UpdateWindow_Character()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winCharacter",
            caption: "Character",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 174, height: 356,
            icon: 62,
            visible: false,
            xOffset: 2, yOffset: 6,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            callbackMousemove: WinCharacter.OnMouseMove,
            callbackMousedown: WinCharacter.OnMouseMove,
            callbackDblclick: WinCharacter.OnDoubleClick,
            onDraw: WinCharacter.OnDrawCharacter);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinMenu.OnCharacterClick);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 162, height: 287,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 34, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 54, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 74, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 94, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 114, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 134, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 154, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 18, top: 36, width: 147, height: 10,
            text: "Name",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblJob",
            left: 18, top: 56, width: 147, height: 10,
            text: "Job",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLevel",
            left: 18, top: 76, width: 147, height: 10,
            text: "Level",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblClient.Guild",
            left: 18, top: 96, width: 147, height: 10,
            text: "Client.Guild",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblHealth",
            left: 18, top: 116, width: 147, height: 10,
            text: "Health",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblSpirit",
            left: 18, top: 136, width: 147, height: 10,
            text: "Spirit",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblExperience",
            left: 18, top: 156, width: 147, height: 10,
            text: "Experience",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName2",
            left: 13, top: 36, width: 147, height: 10,
            text: "Name",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblJob2",
            left: 13, top: 56, width: 147, height: 10,
            text: "",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLevel2",
            left: 13, top: 76, width: 147, height: 10,
            text: "Level",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblClient.Guild2",
            left: 13, top: 96, width: 147, height: 10,
            text: "Client.Guild",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblHealth2",
            left: 13, top: 116, width: 147, height: 10,
            text: "Health",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblSpirit2",
            left: 13, top: 136, width: 147, height: 10,
            text: "Spirit",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblExperience2",
            left: 13, top: 156, width: 147, height: 10,
            text: "Experience",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 18, top: 176, width: 138, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 173, width: 138, height: 10,
            text: "Attributes",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 186, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 206, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 226, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 246, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 266, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 286, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 188, width: 138, height: 10,
            text: "Strength",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 208, width: 138, height: 10,
            text: "Vitality",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 228, width: 138, height: 10,
            text: "Intelligence",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 248, width: 138, height: 10,
            text: "Luck",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 268, width: 138, height: 10,
            text: "Spirit",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 288, width: 138, height: 10,
            text: "Stat Points",
            font: Font.Arial);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnStat_1",
            left: 144, top: 188, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint1);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnStat_2",
            left: 144, top: 208, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint2);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnStat_3",
            left: 144, top: 228, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint3);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnStat_4",
            left: 144, top: 248, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint4);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnStat_5",
            left: 144, top: 268, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint5);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_1",
            left: 144, top: 188, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_2",
            left: 144, top: 208, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_3",
            left: 144, top: 228, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_4",
            left: 144, top: 248, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_5",
            left: 144, top: 268, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStat_1",
            left: 42, top: 188, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStat_2",
            left: 42, top: 208, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStat_3",
            left: 42, top: 228, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStat_4",
            left: 42, top: 248, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblStat_5",
            left: 42, top: 268, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblPoints",
            left: 57, top: 288, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            align: Alignment.Right);
    }

    public void UpdateWindow_Description()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winDescription",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 193, height: 142,
            icon: 0,
            visible: false,
            designNorm: Design.WindowDescription,
            designHover: Design.WindowDescription,
            designMousedown: Design.WindowDescription,
            canDrag: false);

        Gui.ZOrderCon = 0;

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 8, top: 12, width: 177, height: 10,
            text: "Flame Sword",
            font: Font.Arial,
            align: Alignment.Center);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picSprite",
            left: 18, top: 32, width: 68, height: 68,
            designNorm: Design.DescriptionPicture,
            designHover: Design.DescriptionPicture,
            designMousedown: Design.DescriptionPicture,
            onDraw: WinDescription.OnDraw);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picSep",
            left: 96, top: 28, width: 0, height: 92,
            imageNorm: 44,
            imageHover: 44,
            imageMousedown: 44);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblJob",
            left: 5, top: 102, width: 92, height: 10,
            text: "Warrior",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblLevel",
            left: 5, top: 114, width: 92, height: 10,
            text: "Level 20",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBar",
            left: 19, top: 114, width: 66, height: 12,
            visible: false,
            imageNorm: 45,
            imageHover: 45,
            imageMousedown: 45);
    }

    public void UpdateWindow_RightClick()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winRightClickBG",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 800, height: 600,
            icon: 0,
            visible: false,
            callbackMousedown: WinPlayerMenu.OnClose,
            canDrag: false);

        Gui.CentralizeWindow(windowIndex);
    }

    public void UpdateWindow_PlayerMenu()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winPlayerMenu",
            caption: "",
            font: Font.Georgia, zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 110, height: 106,
            icon: 0,
            visible: false,
            designNorm: Design.WindowDescription,
            designHover: Design.WindowDescription,
            designMousedown: Design.WindowDescription,
            callbackMousedown: WinPlayerMenu.OnClose,
            canDrag: false);

        Gui.CentralizeWindow(windowIndex);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnName",
            left: 8, top: 8, width: 94, height: 18,
            text: "[Name]",
            designNorm: Design.MenuHeader,
            designHover: Design.MenuHeader,
            designMousedown: Design.MenuHeader,
            callbackMousedown: WinPlayerMenu.OnClose);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnParty",
            left: 8, top: 26, width: 94, height: 18,
            text: "Invite to Party",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnPartyInvite);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnTrade",
            left: 8, top: 44, width: 94, height: 18,
            text: "Request Trade",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnTradeRequest);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClient.Guild",
            left: 8, top: 62, width: 94, height: 18,
            text: "Invite to Client.Guild",
            designNorm: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnGuildInvite);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnPM",
            left: 8, top: 80, width: 94, height: 18,
            text: "Private Message",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnPrivateMessage);
    }

    public void UpdateWindow_DragBox()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winDragBox",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 32, height: 32,
            icon: 0,
            visible: false,
            onDraw: WinDragBox.OnDraw);

        Gui.Windows[windowIndex].CallBack[(int) ControlState.MouseUp] = WinDragBox.DragBox_Check;
    }

    public void UpdateWindow_Options()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winOptions",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 210, height: 212,
            icon: 0,
            visible: false,
            designNorm: Design.WindowNoBar,
            designHover: Design.WindowNoBar,
            designMousedown: Design.WindowNoBar);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 198, height: 200,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 35, top: 25, width: 140, height: 10,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 35, top: 22, width: 140, height: 0,
            text: "General Options",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkMusic",
            left: 35, top: 40, width: 80,
            text: "Music",
            theDesign: Design.CheckboxNormal);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkSound",
            left: 115, top: 40, width: 80,
            text: "Sound",
            theDesign: Design.CheckboxNormal);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkAutotile",
            left: 35, top: 60, width: 80,
            text: "Autotile",
            theDesign: Design.CheckboxNormal);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "chkFullscreen",
            left: 115, top: 60, width: 80,
            text: "Fullscreen",
            theDesign: Design.CheckboxNormal);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 35, top: 85, width: 140, height: 10,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 35, top: 92, width: 140, height: 10,
            text: "Select Resolution",
            font: Font.Georgia,
            align: Alignment.Center);

        Gui.CreateComboBox(
            windowIndex: windowIndex,
            name: "cmbRes",
            left: 30, top: 100, width: 150, height: 18,
            design: Design.ComboBoxNormal);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnConfirm",
            left: 65, top: 168, width: 80, height: 22,
            text: "Confirm",
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinOptions.OnConfirm);

        Client.GameLogic.SetOptionsScreen();
    }

    public void UpdateWindow_Combobox()
    {
        Gui.CreateWindow(
            name: "winComboMenuBG",
            caption: "ComboMenuBG",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 800, height: 600,
            icon: 0,
            visible: false,
            callbackDblclick: WinComboMenu.Close,
            zChange: 0);

        var windowIndex = Gui.CreateWindow(
            name: "winComboMenu",
            caption: "ComboMenu",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 100, height: 100,
            icon: 0,
            visible: false,
            designNorm: Design.ComboMenuNormal,
            clickThrough: false);

        Gui.CentralizeWindow(windowIndex);
    }

    public void UpdateWindow_Skills()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winSkills",
            caption: "Skills",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 202, height: 297,
            icon: 109,
            visible: false,
            xOffset: 2, yOffset: 7,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            callbackMousemove: WinSkills.OnMouseMove,
            callbackMousedown: WinSkills.OnMouseDown,
            callbackDblclick: WinSkills.OnDoubleClick,
            onDraw: WinSkills.OnDraw);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinMenu.OnSkillsClick);
    }

    public void UpdateWindow_Bank()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winBank",
            caption: "Bank",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 390, height: 373,
            icon: 0,
            visible: false,
            xOffset: 2, yOffset: 5,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            callbackMousemove: WinBank.OnMouseMove,
            callbackMousedown: WinBank.OnMouseDown,
            callbackDblclick: WinBank.OnDoubleClick,
            onDraw: WinBank.OnDraw);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 36, height: 36,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinBank.OnClose);
    }

    public void UpdateWindow_Shop()
    {
        var windowIndex = Gui.CreateWindow(
            name: "winShop",
            caption: "Shop",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 278, height: 293,
            icon: 17,
            visible: false,
            xOffset: 2, yOffset: 5,
            designNorm: Design.WindowEmpty,
            designHover: Design.WindowEmpty,
            designMousedown: Design.WindowEmpty,
            callbackMousemove: WinShop.OnMouseMove,
            callbackMousedown: WinShop.OnMouseDown,
            onDraw: WinShop.OnDrawBackground);

        Gui.CentralizeWindow(windowIndex);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 6, width: 36, height: 36,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinShop.OnClose);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 215, width: 266, height: 50,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment,
            onDraw: WinShop.OnDraw);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picItemBG",
            left: 13, top: 222, width: 36, height: 36,
            imageNorm: 30,
            imageHover: 30,
            imageMousedown: 30);

        Gui.CreatePictureBox(
            windowIndex: windowIndex,
            name: "picItem",
            left: 15, top: 224, width: 32, height: 32);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnBuy",
            left: 190, top: 228, width: 70, height: 24,
            text: "Buy", font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinShop.OnBuy);

        Gui.CreateButton(
            windowIndex: windowIndex,
            name: "btnSell",
            left: 190, top: 228, width: 70, height: 24,
            text: "Sell",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinShop.OnSell);

        Gui.CreateCheckBox(
            windowIndex: windowIndex,
            name: "CheckboxBuying",
            left: 173, top: 265, width: 49, height: 20,
            theDesign: Design.CheckboxBuying,
            callbackMousedown: WinShop.OnBuyingChecked);

        Gui.CreateCheckBox(
            windowIndex,
            "CheckboxSelling",
            222, 265, 49, 20,
            theDesign: Design.CheckboxSelling,
            callbackMousedown: WinShop.OnSellingChecked);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 56, top: 226, width: 300, height: 10,
            text: "Test Item",
            font: Font.Arial);

        Gui.CreateLabel(
            windowIndex: windowIndex,
            name: "lblCost",
            left: 56, top: 240, width: 300, height: 10,
            text: "1000g",
            font: Font.Arial);
    }
}
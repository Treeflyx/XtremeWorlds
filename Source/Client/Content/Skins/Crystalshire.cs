#nullable disable

using System;
using Client.Game.UI;
using Client.Game.UI.Windows;
using Core.Configurations;
using Core.Globals;
using Microsoft.Xna.Framework;

public class Crystalshire
{
    public void UpdateWindow_Login()
    {
        var windowIndex = Gui.UpdateWindow(
            name: "winLogin",
            caption: "Login",
            font: Font.Georgia, zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 276, height: 212,
            icon: 45,
            visible: true,
            xOffset: 3, yOffset: 5,
            designNorm: Design.WindowNormal,
            designHover: Design.WindowNormal,
            designMousedown: Design.WindowNormal);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.UpdatePictureBox(
            windowIndex,
            "picParchment",
            6, 26, 264, 180,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex,
            "picShadow_1",
            67, 43, 142, 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_2",
            left: 67, top: 79, width: 142, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: Client.General.DestroyGame);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 67, top: 134, width: 67, height: 22,
            text: "Accept",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinLogin.OnLogin);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnExit",
            left: 142, top: 134, width: 67, height: 22,
            text: "Exit",
            font: Font.Arial,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinLogin.OnExit);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblUsername",
            left: 72, top: 39, width: 142, height: 10,
            text: "Username",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblPassword",
            left: 72, top: 75, width: 142, height: 10,
            text: "Password",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        var username = SettingsManager.Instance.SaveUsername ? SettingsManager.Instance.Username : string.Empty;

        var textBoxUserName = Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtUsername",
            left: 67, top: 55, width: 142, height: 19,
            text: username,
            font: Font.Arial,
            xOffset: 5, yOffset: 3,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        var textBoxPassword = Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtPassword",
            left: 67, top: 86, width: 142, height: 19,
            font: Font.Arial,
            xOffset: 5, yOffset: 3,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite,
            censor: true);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkSaveUsername",
            left: 67, top: 114, width: 142,
            value: SettingsManager.Instance.SaveUsername ? 1 : 0,
            text: "Save Username?",
            font: Font.Arial,
            theDesign: Design.CheckboxNormal,
            callbackMousedown: WinLogin.OnSaveUserClicked);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnRegister",
            left: 12, top: Gui.Windows[windowIndex].Height - 35, width: 252, height: 22,
            text: "Register Account",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinLogin.OnRegister);

        Gui.SetActiveControl(windowIndex, username.Length == 0 ? textBoxUserName : textBoxPassword);
    }

    public void UpdateWindow_Register()
    {
        var windowIndex = Gui.UpdateWindow(
            "winRegister", "Register Account",
            Font.Georgia,
            Gui.ZOrderWin,
            0, 0, 276, 202,
            45,
            false,
            3, 5,
            Design.WindowNormal,
            Design.WindowNormal,
            Design.WindowNormal);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinRegister.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 264, height: 170,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_1",
            left: 67, top: 43, width: 142, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_2",
            left: 67, top: 79, width: 142, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_3",
            left: 67, top: 115, width: 142, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 68, top: 152, width: 67, height: 22,
            text: "Accept",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinRegister.OnRegister);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnExit",
            left: 142, top: 152, width: 67, height: 22,
            text: "Back",
            font: Font.Arial,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinRegister.OnClose);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblUsername",
            left: 66, top: 39, width: 142, height: 10,
            text: "Username",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblPassword",
            left: 66, top: 75, width: 142, height: 10,
            text: "Password", font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblRetypePassword",
            left: 66, top: 110, width: 142, height: 10,
            text: "Retype Password",
            font: Font.Arial,
            color: Color.White, align: Alignment.Center);

        Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtUsername",
            left: 67, top: 55, width: 142, height: 19,
            font: Font.Arial,
            isActive: true,
            xOffset: 5, yOffset: 3,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtPassword",
            left: 67, top: 90, width: 142, height: 19,
            font: Font.Arial,
            isActive: true,
            xOffset: 5, yOffset: 3,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite,
            censor: true);

        Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtRetypePassword",
            left: 67, top: 127, width: 142, height: 19,
            font: Font.Arial,
            isActive: true,
            xOffset: 5, yOffset: 3,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite,
            censor: true);

        Gui.SetActiveControl(windowIndex, Gui.GetControlIndex("winRegister", "txtUsername"));
    }

    public void UpdateWindow_NewChar()
    {
        var windowIndex = Gui.UpdateWindow(
            name: "winNewChar",
            caption: "Create Character",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0,
            width: 290, height: 172,
            icon: 17,
            visible: false,
            xOffset: 2, yOffset: 6,
            designNorm: Design.WindowNormal,
            designHover: Design.WindowNormal,
            designMousedown: Design.WindowNormal);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.UpdateButton(windowIndex: windowIndex, name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5,
            width: 16, height: 16,
            font: Font.Georgia,
            imageNorm: 8, imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinNewChar.OnCancel);

        // Parchment
        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 278, height: 140,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        // Name
        Gui.UpdatePictureBox(windowIndex, "picShadow_1", 29, 42, 124, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval);
        Gui.UpdateLabel(windowIndex, "lblName", 29, 39, 124, 10, "Name", Font.Arial, Color.White, Alignment.Center);

        // Textbox
        Gui.UpdateTextbox(windowIndex, "txtName", 29, 55, 124, 19, "", Font.Arial, Alignment.Left, true, 255, true, 5, 3, 0, 0, 0, Design.TextWhite, Design.TextWhite, Design.TextWhite);

        // Sex
        Gui.UpdatePictureBox(windowIndex, "picShadow_2", 29, 85, 124, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval);
        Gui.UpdateLabel(windowIndex, "lblGender", 29, 82, 124, 10, "Gender", Font.Arial, Color.White, Alignment.Center);
        Gui.UpdateCheckBox(windowIndex, "chkMale", 29, 103, 55, 15, 0, "Male", Font.Arial, Alignment.Center, true, 255, Design.CheckboxNormal, 0, false, null, null, WinNewChar.OnMaleChecked);
        Gui.UpdateCheckBox(windowIndex, "chkFemale", 90, 103, 62, 15, 0, "Female", Font.Arial, Alignment.Center, true, 255, Design.CheckboxNormal, 0, false, null, null, WinNewChar.OnFemaleChecked);

        // Buttons
        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 29, top: 127, width: 60, height: 24,
            text: "Accept",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinNewChar.OnAccept);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnCancel",
            left: 93, top: 127, width: 60, height: 24,
            text: "Cancel",
            font: Font.Arial,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinNewChar.OnCancel);

        // Sprite
        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_3",
            left: 175, top: 42, width: 76, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(windowIndex, "lblSprite", 175, 39, 76, 10, "Sprite", Font.Arial, Color.White, Alignment.Center);

        // Scene
        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picScene",
            left: 165, top: 55, width: 96, height: 96,
            imageNorm: 11,
            imageHover: 11,
            imageMousedown: 11,
            onDraw: WinNewChar.OnDrawSprite);

        // Buttons
        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnLeft",
            left: 163, top: 40, width: 10, height: 13,
            imageNorm: 12, imageHover: 14, imageMousedown: 16,
            callbackMousedown: WinNewChar.OnLeftClick);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnRight",
            left: 252, top: 40, width: 10, height: 13,
            callbackMousedown: WinNewChar.OnRightClick);

        Gui.SetActiveControl(windowIndex, Gui.GetControlIndex("winNewChar", "txtName"));
    }

    public void UpdateWindow_Chars()
    {
        var windowIndex = Gui.UpdateWindow(
            "winChars",
            "Characters",
            Font.Georgia,
            Gui.ZOrderWin,
            0, 0, 364, 229,
            62,
            false,
            3, 5,
            Design.WindowNormal,
            Design.WindowNormal,
            Design.WindowNormal);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinChars.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 352, height: 197,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_1",
            left: 22, top: 40, width: 98, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblCharName_1",
            left: 22, top: 37, width: 98, height: 10,
            text: "Blank Slot",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: Gui.Windows.Count,
            name: "picShadow_2",
            left: 132, top: 40, width: 98, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblCharName_2",
            left: 132, top: 37, width: 98, height: 10,
            text: "Blank Slot",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow_3",
            left: 242, top: 40, width: 98, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblCharName_3",
            left: 242, top: 37, width: 98, height: 10,
            text: "Blank Slot",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picScene_1",
            left: 23, top: 55, width: 96, height: 96,
            imageNorm: 11,
            imageHover: 11,
            imageMousedown: 11);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picScene_2",
            left: 133, top: 55, width: 96, height: 96,
            imageNorm: 11,
            imageHover: 11,
            imageMousedown: 11);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picScene_3",
            left: 243, top: 55, width: 96, height: 96,
            imageNorm: 11,
            imageHover: 11,
            imageMousedown: 11,
            onDraw: WinChars.Chars_OnDraw);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnSelectChar_1",
            left: 22, top: 155, width: 98, height: 24,
            text: "Select",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnSelectCharacter1Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnCreateChar_1",
            left: 22, top: 155, width: 98, height: 24,
            text: "Create",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnCreateCharacter1Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnDelChar_1",
            left: 22, top: 183, width: 98, height: 24,
            text: "Delete",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinChars.OnDeleteCharacter1Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnSelectChar_2",
            left: 132, top: 155, width: 98, height: 24,
            text: "Select",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnSelectCharacter2Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnCreateChar_2",
            left: 132, top: 155, width: 98, height: 24,
            text: "Create",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnCreateCharacter2Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnDelChar_2",
            left: 132, top: 183, width: 98, height: 24,
            text: "Delete",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinChars.OnDeleteCharacter2Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnSelectChar_3",
            left: 242, top: 155, width: 98, height: 24,
            text: "Select",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnSelectCharacter3Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnCreateChar_3",
            left: 242, top: 155, width: 98, height: 24,
            text: "Create",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinChars.OnCreateCharacter3Click);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnDelChar_3",
            left: 242, top: 183, width: 98, height: 24,
            text: "Delete",
            font: Font.Arial,
            visible: false,
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinChars.OnDeleteCharacter3Click);
    }

    public void UpdateWindow_Jobs()
    {
        var windowIndex = Gui.UpdateWindow(
            name: "winJobs",
            caption: "Select Job",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 364, height: 229,
            icon: 17,
            visible: false,
            xOffset: 2, yOffset: 6,
            designNorm: Design.WindowNormal,
            designHover: Design.WindowNormal,
            designMousedown: Design.WindowNormal);

        Gui.CentralizeWindow(windowIndex);
        Gui.ZOrderCon = 0;

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinJobs.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 352, height: 197,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment,
            callbackMousedown: WinJobs.OnClose,
            onDraw: WinJobs.OnDrawFace);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 183, top: 42, width: 98, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblJobName",
            left: 183, top: 39, width: 98, height: 10,
            text: "Warrior",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnLeft",
            left: 170, top: 40, width: 10, height: 13,
            imageNorm: 12,
            imageHover: 14,
            imageMousedown: 16,
            callbackMousedown: WinJobs.OnLeftClick);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnRight",
            left: 282, top: 40, width: 10, height: 13,
            imageNorm: 13,
            imageHover: 15,
            imageMousedown: 17,
            callbackMousedown: WinJobs.OnRightClick);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 183, top: 185, width: 98, height: 22,
            text: "Accept",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinJobs.OnAccept);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBackground",
            left: 127, top: 55, width: 210, height: 124,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picOverlay",
            left: 6, top: 26, width: 0, height: 0,
            callbackMousedown: WinJobs.OnClose,
            onDraw: WinJobs.Jobs_DrawText);
    }

    public void UpdateWindow_Dialogue()
    {
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinDialogue.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 335, height: 113,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 103, top: 44, width: 144, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblHeader",
            left: 103, top: 40, width: 144, height: 10,
            text: "Header",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtInput",
            left: 93, top: 75, width: 162, height: 18,
            font: Font.Arial,
            align: Alignment.Center,
            xOffset: 5, yOffset: 2,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblBody_1",
            left: 15, top: 60, width: 314, height: 10,
            text: "Invalid username or password.",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblBody_2",
            left: 15, top: 75, width: 314, height: 10,
            text: "Please try again!",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateButton(
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

        Gui.UpdateButton(
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

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName1",
            left: 60, top: 20, width: 173, height: 10,
            text: "Richard - Level 10",
            font: Font.Arial, color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName2",
            left: 60, top: 60, width: 173, height: 10,
            text: "Anna - Level 18",
            font: Font.Arial, color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName3",
            left: 60, top: 100, width: 173, height: 10,
            text: "Doleo - Level 25",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP1",
            left: 58, top: 34, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP2",
            left: 58, top: 74, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_HP3",
            left: 58, top: 114, width: 173, height: 9,
            imageNorm: 62,
            imageHover: 62,
            imageMousedown: 62);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP1",
            left: 58, top: 44, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP2",
            left: 58, top: 84, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEmptyBar_SP3",
            left: 58, top: 124, width: 173, height: 9,
            imageNorm: 63,
            imageHover: 63,
            imageMousedown: 63);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP1",
            left: 58, top: 34, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP2",
            left: 58, top: 74, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_HP3",
            left: 58, top: 114, width: 173, height: 9,
            imageNorm: 64,
            imageHover: 64,
            imageMousedown: 64);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP1",
            left: 58, top: 44, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP2",
            left: 58, top: 84, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBar_SP3",
            left: 58, top: 124, width: 173, height: 9,
            imageNorm: 65,
            imageHover: 65,
            imageMousedown: 65);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picChar1",
            left: 20, top: 20, width: 32, height: 32,
            texturePath: DataPath.Characters);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picChar2",
            left: 20, top: 60, width: 32, height: 32,
            texturePath: DataPath.Characters);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picChar3",
            left: 20, top: 100, width: 32, height: 32,
            texturePath: DataPath.Characters);
    }

    public void UpdateWindow_Trade()
    {
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 36, height: 36,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinTrade.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 10, top: 312, width: 392, height: 66,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 36, top: 30, width: 142, height: 9,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblYourTrade",
            left: 36, top: 27, width: 142, height: 9,
            text: "Robin's Offer",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 236, top: 30, width: 142, height: 9,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblTheirTrade",
            left: 236, top: 27, width: 142, height: 9,
            text: "Richard's Offer",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnAccept",
            left: 134, top: 340, width: 68, height: 24,
            text: "Accept",
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinTrade.OnAccept);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnDecline",
            left: 210, top: 340, width: 68, height: 24,
            text: "Decline",
            designNorm: Design.Red,
            designHover: Design.RedHover,
            designMousedown: Design.RedClick,
            callbackMousedown: WinTrade.OnClose);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStatus",
            left: 114, top: 322, width: 184, height: 10,
            text: "",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 25, top: 330, width: 100, height: 10,
            text: "Total Value",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex, "lblBlank",
            285, 330, 100, 10,
            "Total Value",
            Font.Georgia,
            Color.White,
            Alignment.Center);

        Gui.UpdateLabel(
            windowIndex,
            "lblYourValue",
            25, 344, 100, 10,
            "52,812g",
            Font.Georgia,
            Color.White,
            Alignment.Center);

        Gui.UpdateLabel(
            windowIndex, "lblTheirValue",
            285, 344, 100, 10,
            "12,531g",
            Font.Georgia,
            Color.White,
            Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picYour", left: 14, top: 46, width: 184, height: 260,
            callbackMousedown: WinTrade.OnYourTradeMouseMove,
            callbackMousemove: WinTrade.OnYourTradeMouseMove,
            callbackDblclick: WinTrade.OnYourTradeClick,
            onDraw: Gui.DrawYourTrade);

        Gui.UpdatePictureBox(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 198, height: 144,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnReturn",
            left: 16, top: 16, width: 178, height: 28,
            text: "Return to Game",
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinEscMenu.OnClose);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnOptions",
            left: 16, top: 48, width: 178, height: 28,
            text: "Options",
            designNorm: Design.Orange,
            designHover: Design.OrangeHover,
            designMousedown: Design.OrangeClick,
            callbackMousedown: WinEscMenu.OnOptionsClick);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnMainMenu",
            left: 16, top: 80, width: 178, height: 28,
            text: "Back to Main Menu",
            designNorm: Design.Blue,
            designHover: Design.BlueHover,
            designMousedown: Design.BlueClick,
            callbackMousedown: WinEscMenu.OnMainMenuClick);

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 227, height: 65,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picHP_Blank",
            left: 15, top: 15, width: 209, height: 13,
            imageNorm: 24,
            imageHover: 24,
            imageMousedown: 24);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picSP_Blank",
            left: 15, top: 32, width: 209, height: 13,
            imageNorm: 25,
            imageHover: 25,
            imageMousedown: 25);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picEXP_Blank",
            left: 15, top: 49, width: 209, height: 13,
            imageNorm: 26,
            imageHover: 26,
            imageMousedown: 26);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 0, top: 0, width: 0, height: 0,
            onDraw: WinBars.OnDraw);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picHealth",
            left: 16, top: 10, width: 44, height: 14,
            imageNorm: 21,
            imageHover: 21,
            imageMousedown: 21);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picSpirit",
            left: 16, top: 28, width: 44, height: 14,
            imageNorm: 22,
            imageHover: 22,
            imageMousedown: 22);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picExperience",
            left: 16, top: 45, width: 74, height: 14,
            imageNorm: 23,
            imageHover: 23,
            imageMousedown: 23);

        Gui.UpdateLabel(
            windowIndex: Gui.Windows.Count,
            name: "lblHP",
            left: 15, top: 14, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: Gui.Windows.Count,
            name: "lblMP",
            left: 15, top: 30, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblEXP",
            left: 15, top: 48, width: 209, height: 10,
            text: "999/999",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);
    }

    public void UpdateWindow_Chat()
    {
        var windowIndex = Gui.UpdateWindow(
            name: "winChat",
            caption: "",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 8, top: Client.GameState.ResolutionHeight - 178, width: 352, height: 152,
            icon: 0,
            visible: false,
            canDrag: false);

        Gui.ZOrderCon = 0;

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkGame",
            left: 10, top: 2, width: 49, height: 23,
            text: "Game",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnGameChannelClicked);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkMap",
            left: 60, top: 2, width: 49, height: 23,
            text: "Map",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnMapChannelClicked);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkGlobal",
            left: 110, top: 2, width: 49, height: 23,
            text: "Global",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnBroadcastChannelClicked);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkParty",
            left: 160, top: 2, width: 49, height: 23,
            text: "Party",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnPartyChannelClicked);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkClient.Guild",
            left: 210, top: 2, width: 49, height: 23,
            text: "Guild",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnGuildChannelClicked);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkPlayer",
            left: 260, top: 2, width: 49, height: 23,
            text: "Player",
            font: Font.Arial,
            theDesign: Design.CheckboxChat,
            callbackMousedown: WinChat.OnPrivateChannelClicked);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picNull",
            left: 0, top: 0, width: 0, height: 0,
            onDraw: WinChat.OnDraw);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnChat",
            left: 296, top: 140, width: 48, height: 20,
            text: "Say",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackNorm: WinChat.OnSayClick);

        Gui.UpdateTextbox(
            windowIndex: windowIndex,
            name: "txtChat",
            left: 12, top: 143, width: 352, height: 25,
            visible: false);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnUp",
            left: 328, top: 28, width: 10, height: 13,
            imageNorm: 4,
            imageHover: 52,
            imageMousedown: 4,
            callbackMousedown: WinChat.OnUpButtonMouseDown);

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblMsg",
            left: 12, top: 140, width: 286, height: 25,
            text: "Press 'Enter' to open chat",
            font: Font.Georgia,
            color: Color.White);
    }

    public void UpdateWindow_Hotbar()
    {
        Gui.UpdateWindow(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWood",
            left: 0, top: 5, width: 228, height: 20,
            designNorm: Design.Wood,
            designHover: Design.Wood,
            designMousedown: Design.Wood);

        Gui.UpdateButton(
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

        Gui.UpdateButton(
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

        Gui.UpdateButton(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnMap",
            left: 119, top: 0, width: 29, height: 29,
            icon: 106,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnMapClick,
            xOffset: -1, yOffset: -2);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClient.Guild",
            left: 155, top: 0, width: 29, height: 29,
            icon: 107,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnGuildClick,
            xOffset: -1, yOffset: -1);

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinMenu.OnInventoryClick);

        Gui.UpdatePictureBox(
            windowIndex,
            "picBlank",
            8, 293, 186, 18,
            imageNorm: 67,
            imageHover: 67,
            imageMousedown: 67);
    }

    public void UpdateWindow_Character()
    {
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 5, width: 16, height: 16,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinMenu.OnCharacterClick);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 26, width: 162, height: 287,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 34, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 54, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 74, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 94, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 114, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 134, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picWhiteBox",
            left: 13, top: 154, width: 148, height: 19,
            designNorm: Design.TextWhite,
            designHover: Design.TextWhite,
            designMousedown: Design.TextWhite);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 18, top: 36, width: 147, height: 10,
            text: "Name",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblJob",
            left: 18, top: 56, width: 147, height: 10,
            text: "Job",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLevel",
            left: 18, top: 76, width: 147, height: 10,
            text: "Level",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblClient.Guild",
            left: 18, top: 96, width: 147, height: 10,
            text: "Client.Guild",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblHealth",
            left: 18, top: 116, width: 147, height: 10,
            text: "Health",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblSpirit",
            left: 18, top: 136, width: 147, height: 10,
            text: "Spirit",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblExperience",
            left: 18, top: 156, width: 147, height: 10,
            text: "Experience",
            font: Font.Arial,
            color: Color.White);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName2",
            left: 13, top: 36, width: 147, height: 10,
            text: "Name",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblJob2",
            left: 13, top: 56, width: 147, height: 10,
            text: "",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLevel2",
            left: 13, top: 76, width: 147, height: 10,
            text: "Level",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblClient.Guild2",
            left: 13, top: 96, width: 147, height: 10,
            text: "Client.Guild",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblHealth2",
            left: 13, top: 116, width: 147, height: 10,
            text: "Health",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblSpirit2",
            left: 13, top: 136, width: 147, height: 10,
            text: "Spirit",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblExperience2",
            left: 13, top: 156, width: 147, height: 10,
            text: "Experience",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picShadow",
            left: 18, top: 176, width: 138, height: 9,
            designNorm: Design.BlackOval,
            designHover: Design.BlackOval,
            designMousedown: Design.BlackOval);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 173, width: 138, height: 10,
            text: "Attributes",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 186, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 206, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 226, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 246, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 266, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlackBox",
            left: 13, top: 286, width: 148, height: 19,
            designNorm: Design.TextBlack,
            designHover: Design.TextBlack,
            designMousedown: Design.TextBlack);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 188, width: 138, height: 10,
            text: "Strength",
            font: Font.Arial,
            color: Color.Yellow);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 208, width: 138, height: 10,
            text: "Vitality",
            font: Font.Arial,
            color: Color.Yellow);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 228, width: 138, height: 10,
            text: "Intelligence",
            font: Font.Arial,
            color: Color.Yellow);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 248, width: 138, height: 10,
            text: "Luck",
            font: Font.Arial,
            color: Color.Yellow);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 268, width: 138, height: 10,
            text: "Spirit",
            font: Font.Arial,
            color: Color.Yellow);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLabel",
            left: 18, top: 288, width: 138, height: 10,
            text: "Stat Points",
            font: Font.Arial,
            color: Color.Green);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnStat_1",
            left: 144, top: 188, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint1);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnStat_2",
            left: 144, top: 208, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint2);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnStat_3",
            left: 144, top: 228, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint3);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnStat_4",
            left: 144, top: 248, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint4);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnStat_5",
            left: 144, top: 268, width: 15, height: 15,
            imageNorm: 48,
            imageHover: 49,
            imageMousedown: 50,
            callbackMousedown: WinCharacter.OnSpendPoint5);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_1",
            left: 144, top: 188, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_2",
            left: 144, top: 208, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_3",
            left: 144, top: 228, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_4",
            left: 144, top: 248, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "btnGreyStat_5",
            left: 144, top: 268, width: 15, height: 15,
            imageNorm: 47,
            imageHover: 47,
            imageMousedown: 47);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStat_1",
            left: 42, top: 188, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStat_2",
            left: 42, top: 208, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStat_3",
            left: 42, top: 228, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStat_4",
            left: 42, top: 248, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblStat_5",
            left: 42, top: 268, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblPoints",
            left: 57, top: 288, width: 100, height: 15,
            text: "255",
            font: Font.Arial,
            color: Color.White,
            align: Alignment.Right);
    }

    public void UpdateWindow_Description()
    {
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 8, top: 12, width: 177, height: 10,
            text: "Flame Sword",
            font: Font.Arial,
            color: Color.Blue,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picSprite",
            left: 18, top: 32, width: 68, height: 68,
            designNorm: Design.DescriptionPicture,
            designHover: Design.DescriptionPicture,
            designMousedown: Design.DescriptionPicture,
            onDraw: WinDescription.OnDraw);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picSep",
            left: 96, top: 28, width: 0, height: 92,
            imageNorm: 44,
            imageHover: 44,
            imageMousedown: 44);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblJob",
            left: 5, top: 102, width: 92, height: 10,
            text: "Warrior",
            font: Font.Georgia,
            color: Color.Green,
            align: Alignment.Center);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblLevel",
            left: 5, top: 114, width: 92, height: 10,
            text: "Level 20",
            font: Font.Georgia,
            color: Color.Red,
            align: Alignment.Center);

        Gui.UpdatePictureBox(
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
        var windowIndex = Gui.UpdateWindow(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnName",
            left: 8, top: 8, width: 94, height: 18,
            text: "[Name]",
            designNorm: Design.MenuHeader,
            designHover: Design.MenuHeader,
            designMousedown: Design.MenuHeader,
            callbackMousedown: WinPlayerMenu.OnClose);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnParty",
            left: 8, top: 26, width: 94, height: 18,
            text: "Invite to Party",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnPartyInvite);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnTrade",
            left: 8, top: 44, width: 94, height: 18,
            text: "Request Trade",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnTradeRequest);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClient.Guild",
            left: 8, top: 62, width: 94, height: 18,
            text: "Invite to Client.Guild",
            designNorm: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnGuildInvite);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnPM",
            left: 8, top: 80, width: 94, height: 18,
            text: "Private Message",
            designHover: Design.MenuOption,
            callbackMousedown: WinPlayerMenu.OnPrivateMessage);
    }

    public void UpdateWindow_DragBox()
    {
        var windowIndex = Gui.UpdateWindow(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 6, width: 198, height: 200,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 35, top: 25, width: 140, height: 10,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 35, top: 22, width: 140, height: 0,
            text: "General Options",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkMusic",
            left: 35, top: 40, width: 80,
            text: "Music",
            theDesign: Design.CheckboxNormal);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkSound",
            left: 115, top: 40, width: 80,
            text: "Sound",
            theDesign: Design.CheckboxNormal);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkAutotile",
            left: 35, top: 60, width: 80,
            text: "Autotile",
            theDesign: Design.CheckboxNormal);

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "chkFullscreen",
            left: 115, top: 60, width: 80,
            text: "Fullscreen",
            theDesign: Design.CheckboxNormal);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picBlank",
            left: 35, top: 85, width: 140, height: 10,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblBlank",
            left: 35, top: 92, width: 140, height: 10,
            text: "Select Resolution",
            font: Font.Georgia,
            color: Color.White,
            align: Alignment.Center);

        Gui.UpdateComboBox(
            windowIndex: windowIndex,
            name: "cmbRes",
            left: 30, top: 100, width: 150, height: 18,
            design: Design.ComboBoxNormal);

        Gui.UpdateButton(
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
        Gui.UpdateWindow(
            name: "winComboMenuBG",
            caption: "ComboMenuBG",
            font: Font.Georgia,
            zOrder: Gui.ZOrderWin,
            left: 0, top: 0, width: 800, height: 600,
            icon: 0,
            visible: false,
            callbackDblclick: Gui.CloseComboMenu,
            zChange: 0);

        var windowIndex = Gui.UpdateWindow(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
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
        var windowIndex = Gui.UpdateWindow(
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

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnClose",
            left: Gui.Windows[windowIndex].Width - 19, top: 6, width: 36, height: 36,
            imageNorm: 8,
            imageHover: 9,
            imageMousedown: 10,
            callbackMousedown: WinShop.OnClose);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picParchment",
            left: 6, top: 215, width: 266, height: 50,
            designNorm: Design.Parchment,
            designHover: Design.Parchment,
            designMousedown: Design.Parchment,
            onDraw: WinShop.OnDraw);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picItemBG",
            left: 13, top: 222, width: 36, height: 36,
            imageNorm: 30,
            imageHover: 30,
            imageMousedown: 30);

        Gui.UpdatePictureBox(
            windowIndex: windowIndex,
            name: "picItem",
            left: 15, top: 224, width: 32, height: 32);

        Gui.UpdateButton(
            windowIndex: windowIndex,
            name: "btnBuy",
            left: 190, top: 228, width: 70, height: 24,
            text: "Buy", font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinShop.OnBuy);

        Gui.UpdateButton(
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

        Gui.UpdateCheckBox(
            windowIndex: windowIndex,
            name: "CheckboxBuying",
            left: 173, top: 265, width: 49, height: 20,
            theDesign: Design.CheckboxBuying,
            callbackMousedown: WinShop.OnBuyingChecked);

        Gui.UpdateCheckBox(
            windowIndex,
            "CheckboxSelling",
            222, 265, 49, 20,
            theDesign: Design.CheckboxSelling,
            callbackMousedown: WinShop.OnSellingChecked);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblName",
            left: 56, top: 226, width: 300, height: 10,
            text: "Test Item",
            font: Font.Arial,
            color: Color.Black);

        Gui.UpdateLabel(
            windowIndex: windowIndex,
            name: "lblCost",
            left: 56, top: 240, width: 300, height: 10,
            text: "1000g",
            font: Font.Arial,
            color: Color.Black);
    }
}
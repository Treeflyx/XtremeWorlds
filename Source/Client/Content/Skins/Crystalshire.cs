#nullable disable
using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Game.UI;
using Client.Game.UI.Windows;
using Client.Net;
using Core.Configurations;
using Core.Globals;
using Microsoft.Xna.Framework;

public class Crystalshire
{
    public void UpdateWindow_Login()
    {
        // Control the window
        Gui.UpdateWindow("winLogin", "Login", Font.Georgia, Gui.ZOrderWin, 0, 0, 276, 212, 45, true, 3, 5, Design.WindowNormal, Design.WindowNormal, Design.WindowNormal);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 264, 180, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, onDraw: argonDraw);

        // Shadows
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_1", 67, 43, 142, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_2", 67, 79, 142, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw2);

        // Close button
        var argcallbackMousedown3 = new Action(Client.General.DestroyGame);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3);

        // Buttons
        var argcallbackMousedown4 = new Action(Sender.btnLogin_Click);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnAccept", 67, 134, 67, 22, "Accept", Font.Arial, designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4);
        var argcallbackMousedown5 = new Action(Client.General.DestroyGame);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnExit", 142, 134, 67, 22, "Exit", Font.Arial, designNorm: Design.Red, designHover: Design.RedHover, designMousedown: Design.RedClick, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5);

        // Labels
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblUsername", 72, 39, 142, 10, "Username", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6, enabled: enabled);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblPassword", 72, 75, 142, 10, "Password", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7, enabled: enabled);

        // Textboxes
        if (SettingsManager.Instance.SaveUsername == true)
        {
            Action argcallbackNorm8 = null;
            Action argcallbackHover8 = null;
            Action argcallbackMousedown8 = null;
            Action argcallbackMousemove8 = null;
            Action argcallbackDblclick8 = null;
            Action argcallbackEnter = null;
            Gui.UpdateTextbox(Gui.Windows.Count, "txtUsername", 67, 55, 142, 19, SettingsManager.Instance.Username, Font.Arial, xOffset: 5, yOffset: 3, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, callbackEnter: argcallbackEnter);
        }
        else
        {
            Action argcallbackNorm9 = null;
            Action argcallbackHover9 = null;
            Action argcallbackMousedown9 = null;
            Action argcallbackMousemove9 = null;
            Action argcallbackDblclick9 = null;
            Action argcallbackEnter1 = null;
            Gui.UpdateTextbox(Gui.Windows.Count, "txtUsername", 67, 55, 142, 19, "", Font.Arial, xOffset: 5, yOffset: 3, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm9, callbackHover: argcallbackHover9, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, callbackEnter: argcallbackEnter1);
        }

        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argcallbackEnter2 = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtPassword", 67, 86, 142, 19, font: Font.Arial, align: Alignment.Left, xOffset: 5, yOffset: 3, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, censor: true, callbackNorm: argcallbackNorm10, callbackHover: argcallbackHover10, callbackMousedown: argcallbackMousedown10, callbackMousemove: argcallbackMousemove10, callbackDblclick: argcallbackDblclick10, callbackEnter: argcallbackEnter2);

        // Checkbox
        var argcallbackMousedown11 = new Action(WinLogin.OnSaveUserClicked);
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkSaveUsername", 67, 114, 142, value: Conversions.ToInteger(SettingsManager.Instance.SaveUsername), text: "Save Username?", font: Font.Arial, theDesign: Design.CheckboxNormal, callbackNorm: argcallbackNorm11, callbackHover: argcallbackHover11, callbackMousedown: argcallbackMousedown11, callbackMousemove: argcallbackMousemove11, callbackDblclick: argcallbackDblclick11);

        // Register Button
        var argcallbackMousedown12 = new Action(WinLogin.OnRegisterClick);
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnRegister", 12, Gui.Windows[Gui.Windows.Count].Height - 35, 252, 22, "Register Account", Font.Arial, designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm12, callbackHover: argcallbackHover12, callbackMousedown: argcallbackMousedown12, callbackMousemove: argcallbackMousemove12, callbackDblclick: argcallbackDblclick12);

        // Set the active control
        if (!(Strings.Len(Gui.Windows[Gui.GetWindowIndex("winLogin")].Controls[Gui.GetControlIndex("winLogin", "txtUsername")].Text) > 0))
        {
            Gui.SetActiveControl(Gui.GetWindowIndex("winLogin"), Gui.GetControlIndex("winLogin", "txtUsername"));
        }
        else
        {
            Gui.SetActiveControl(Gui.GetWindowIndex("winLogin"), Gui.GetControlIndex("winLogin", "txtPassword"));
        }
    }

    public void UpdateWindow_Register()
    {
        // Control the window
        Gui.UpdateWindow("winRegister", "Register Account", Font.Georgia, Gui.ZOrderWin, 0, 0, 276, 202, 45, false, 3, 5, Design.WindowNormal, Design.WindowNormal, Design.WindowNormal);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinRegister.OnClose);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, "", Font.Georgia, 0, 8, 9, 10, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 264, 170, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);

        // Shadows
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_1", 67, 43, 142, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_2", 67, 79, 142, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, onDraw: argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_3", 67, 115, 142, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw3);

        // Buttons
        var argcallbackMousedown5 = new Action(WinRegister.OnRegister);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnAccept", 68, 152, 67, 22, "Accept", Font.Arial, 0, 0, 0, 0, true, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown5, argcallbackMousemove5, argcallbackDblclick5);

        var argcallbackMousedown6 = new Action(WinRegister.OnClose);
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnExit", 142, 152, 67, 22, "Back", Font.Arial, 0, 0, 0, 0, true, 255, Design.Red, Design.RedHover, Design.RedClick, argcallbackNorm, argcallbackHover, argcallbackMousedown6, argcallbackMousemove6, argcallbackDblclick6);

        // Labels
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblUsername", 66, 39, 142, 10, "Username", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm4, argcallbackHover4, argcallbackMousedown7, argcallbackMousemove7, argcallbackDblclick7, enabled: enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblPassword", 66, 75, 142, 10, "Password", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm5, argcallbackHover5, argcallbackMousedown8, argcallbackMousemove8, argcallbackDblclick8, enabled: enabled);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblRetypePassword", 66, 110, 142, 10, "Retype Password", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm6, argcallbackHover6, argcallbackMousedown9, argcallbackMousemove9, argcallbackDblclick9, enabled: enabled);

        // Textboxes
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argcallbackEnter = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtUsername", 67, 55, 142, 19, "", Font.Arial, Alignment.Left, true, 255, true, 5, 3, 0, 0, 0, Design.TextWhite, Design.TextWhite, Design.TextWhite, false, 0, Constant.NameLength, argcallbackNorm7, argcallbackHover7, argcallbackMousedown10, argcallbackMousemove10, argcallbackDblclick10, argcallbackEnter);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argcallbackEnter1 = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtPassword", 67, 90, 142, 19, "", Font.Arial, Alignment.Left, true, 255, true, 5, 3, 0, 0, 0, Design.TextWhite, Design.TextWhite, Design.TextWhite, true, 0, Constant.NameLength, argcallbackNorm8, argcallbackHover8, argcallbackMousedown11, argcallbackMousemove11, argcallbackDblclick11, argcallbackEnter1);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argcallbackEnter2 = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtRetypePassword", 67, 127, 142, 19, "", Font.Arial, Alignment.Left, true, 255, true, 5, 3, 0, 0, 0, Design.TextWhite, Design.TextWhite, Design.TextWhite, true, 0, Constant.NameLength, argcallbackNorm9, argcallbackHover9, argcallbackMousedown12, argcallbackMousemove12, argcallbackDblclick12, argcallbackEnter2);

        Gui.SetActiveControl(Gui.GetWindowIndex("winRegister"), Gui.GetControlIndex("winRegister", "txtUsername"));
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

        Gui.UpdateButton(winNum: windowIndex, name: "btnClose",
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
            winNum: windowIndex,
            name: "btnAccept",
            left: 29, top: 127, width: 60, height: 24,
            text: "Accept",
            font: Font.Arial,
            designNorm: Design.Green,
            designHover: Design.GreenHover,
            designMousedown: Design.GreenClick,
            callbackMousedown: WinNewChar.OnAccept);

        Gui.UpdateButton(
            winNum: windowIndex,
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
            winNum: windowIndex,
            name: "btnLeft",
            left: 163, top: 40, width: 10, height: 13,
            imageNorm: 12, imageHover: 14, imageMousedown: 16,
            callbackMousedown: WinNewChar.OnLeftClick);

        Gui.UpdateButton(
            winNum: windowIndex,
            name: "btnRight",
            left: 252, top: 40, width: 10, height: 13,
            callbackMousedown: WinNewChar.OnRightClick);

        Gui.SetActiveControl(windowIndex, Gui.GetControlIndex("winNewChar", "txtName"));
    }

    public void UpdateWindow_Chars()
    {
        // Control the window
        Gui.UpdateWindow("winChars", "Characters", Font.Georgia, Gui.ZOrderWin, 0, 0, 364, 229, 62, false, 3, 5, Design.WindowNormal, Design.WindowNormal, Design.WindowNormal);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinChars.OnClose);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, "", Font.Georgia, 0, 8, 9, 10, true, 255, 0, 0, 0, argcallbackNorm, argcallbackMousedown, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick);

        // Parchment
        Action argcallbackHover = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 352, 197, true, false, 255, true, 0, 0, 0, Design.Parchment, Design.Parchment, Design.Parchment, "", argcallbackNorm, argcallbackHover, argcallbackMousedown1, argcallbackMousemove1, argcallbackDblclick1, argonDraw);

        // Names
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        bool enabled = false;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_1", 22, 40, 98, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval, "", argcallbackNorm1, argcallbackHover1, argcallbackMousedown2, argcallbackMousemove2, argcallbackDblclick2, argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblCharName_1", 22, 37, 98, 10, "Blank Slot", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm2, argcallbackHover2, argcallbackMousedown3, argcallbackMousemove3, argcallbackDblclick3, enabled);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_2", 132, 40, 98, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval, "", argcallbackNorm3, argcallbackHover3, argcallbackMousedown4, argcallbackMousemove4, argcallbackDblclick4, argonDraw2);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblCharName_2", 132, 37, 98, 10, "Blank Slot", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm4, argcallbackHover4, argcallbackMousedown5, argcallbackMousemove5, argcallbackDblclick5, enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow_3", 242, 40, 98, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval, "", argcallbackNorm5, argcallbackHover5, argcallbackMousedown6, argcallbackMousemove6, argcallbackDblclick6, argonDraw3);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblCharName_3", 242, 37, 98, 10, "Blank Slot", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm6, argcallbackHover6, argcallbackMousedown7, argcallbackMousemove7, argcallbackDblclick7, enabled);

        // Scenery Boxes
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw4 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picScene_1", 23, 55, 96, 96, true, false, 255, true, 11, 11, 11, 0, 0, 0, "", argcallbackNorm7, argcallbackHover7, argcallbackMousedown8, argcallbackMousemove8, argcallbackDblclick8, argonDraw4);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Action argonDraw5 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picScene_2", 133, 55, 96, 96, true, false, 255, true, 11, 11, 11, 0, 0, 0, "", argcallbackNorm8, argcallbackHover8, argcallbackMousedown9, argcallbackMousemove9, argcallbackDblclick9, argonDraw5);
        var argonDraw6 = new Action(WinChars.Chars_OnDraw);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picScene_3", 243, 55, 96, 96, true, false, 255, true, 11, 11, 11, 0, 0, 0, "", argcallbackNorm, argcallbackHover, argcallbackMousedown1, argcallbackMousemove1, argcallbackDblclick1, argonDraw6);

        // Control Buttons
        var argcallbackMousedown10 = new Action(WinChars.OnSelectCharacter1Click);
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnSelectChar_1", 22, 155, 98, 24, "Select", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown10, argcallbackMousemove10, argcallbackDblclick10);
        var argcallbackMousedown11 = new Action(WinChars.OnCreateCharacter1Click);
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnCreateChar_1", 22, 155, 98, 24, "Create", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown11, argcallbackMousemove11, argcallbackDblclick11);
        var argcallbackMousedown12 = new Action(WinChars.OnDeleteCharacter1Click);
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnDelChar_1", 22, 183, 98, 24, "Delete", Font.Arial, 0, 0, 0, 0, false, 255, Design.Red, Design.RedHover, Design.RedClick, argcallbackNorm, argcallbackHover, argcallbackMousedown12, argcallbackMousemove12, argcallbackDblclick12);
        var argcallbackMousedown13 = new Action(WinChars.OnSelectCharacter2Click);
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnSelectChar_2", 132, 155, 98, 24, "Select", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown13, argcallbackMousemove13, argcallbackDblclick13);
        var argcallbackMousedown14 = new Action(WinChars.OnCreateCharacter2Click);
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnCreateChar_2", 132, 155, 98, 24, "Create", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown14, argcallbackMousemove14, argcallbackDblclick14);
        var argcallbackMousedown15 = new Action(WinChars.OnDeleteCharacter2Click);
        Action argcallbackMousemove15 = null;
        Action argcallbackDblclick15 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnDelChar_2", 132, 183, 98, 24, "Delete", Font.Arial, 0, 0, 0, 0, false, 255, Design.Red, Design.RedHover, Design.RedClick, argcallbackNorm, argcallbackHover, argcallbackMousedown15, argcallbackMousemove15, argcallbackDblclick15);
        var argcallbackMousedown16 = new Action(WinChars.OnSelectCharacter3Click);
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnSelectChar_3", 242, 155, 98, 24, "Select", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown16, argcallbackMousemove16, argcallbackDblclick16);
        var argcallbackMousedown17 = new Action(WinChars.OnCreateCharacter3Click);
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnCreateChar_3", 242, 155, 98, 24, "Create", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown17, argcallbackMousemove17, argcallbackDblclick17);
        var argcallbackMousedown18 = new Action(WinChars.OnDeleteCharacter3Click);
        Action argcallbackMousemove18 = null;
        Action argcallbackDblclick18 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnDelChar_3", 242, 183, 98, 24, "Delete", Font.Arial, 0, 0, 0, 0, false, 255, Design.Red, Design.RedHover, Design.RedClick, argcallbackNorm, argcallbackHover, argcallbackMousedown18, argcallbackMousemove18, argcallbackDblclick18);
    }

    public void UpdateWindow_Jobs()
    {
        // Control window
        Gui.UpdateWindow("winJobs", "Select Job", Font.Georgia, Gui.ZOrderWin, 0, 0, 364, 229, 17, false, 2, 6, Design.WindowNormal, Design.WindowNormal, Design.WindowNormal);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinJobs.OnClose);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, "", Font.Georgia, 0, 8, 9, 10, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick);

        // Parchment
        var argonDraw = new Action(WinJobs.OnDrawFace);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 352, 197, true, false, 255, true, 0, 0, 0, Design.Parchment, Design.Parchment, Design.Parchment, "", argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick, argonDraw);

        // Job Name
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow", 183, 42, 98, 9, true, false, 255, true, 0, 0, 0, Design.BlackOval, Design.BlackOval, Design.BlackOval, "", argcallbackNorm, argcallbackHover, argcallbackMousedown1, argcallbackMousemove1, argcallbackDblclick1, argonDraw1);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblJobName", 183, 39, 98, 10, "Warrior", Font.Arial, Color.White, Alignment.Center, true, 255, false, false, argcallbackNorm1, argcallbackHover1, argcallbackMousedown2, argcallbackMousemove2, argcallbackDblclick2, enabled);

        // Select Buttons
        var argcallbackMousedown3 = new Action(WinJobs.OnLeftClick);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnLeft", 170, 40, 10, 13, "", Font.Georgia, 0, 12, 14, 16, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown3, argcallbackMousemove3, argcallbackDblclick3);

        var argcallbackMousedown4 = new Action(WinJobs.OnRightClick);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnRight", 282, 40, 10, 13, "", Font.Georgia, 0, 13, 15, 17, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown4, argcallbackMousemove4, argcallbackDblclick4);

        // Accept Button
        var argcallbackMousedown5 = new Action(WinJobs.OnAccept);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnAccept", 183, 185, 98, 22, "Accept", Font.Arial, 0, 0, 0, 0, true, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown5, argcallbackMousemove5, argcallbackDblclick5);

        // Text background
        Action argcallbackHover2 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBackground", 127, 55, 210, 124, true, false, 255, true, 0, 0, 0, Design.TextBlack, Design.TextBlack, Design.TextBlack, "", argcallbackNorm, argcallbackHover2, argcallbackMousedown6, argcallbackMousemove6, argcallbackDblclick6, argonDraw2);

        // Overlay
        var argonDraw3 = new Action(WinJobs.Jobs_DrawText);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picOverlay", 6, 26, 0, 0, true, false, 255, true, 0, 0, 0, 0, 0, 0, "", argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick, argonDraw3);
    }

    public void UpdateWindow_Dialogue()
    {
        // Control dialogue window
        Gui.UpdateWindow("winDialogue", "Warning", Font.Georgia, Gui.ZOrderWin, 0, 0, 348, 145, 38, false, 3, 5, Design.WindowNormal, Design.WindowNormal, Design.WindowNormal, canDrag: false);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinDialogue.OnClose);
        Action argcallbackNorm = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, "", Font.Georgia, 0, 8, 9, 10, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 335, 113, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);

        // Header
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow", 103, 44, 144, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblHeader", 103, 40, 144, 10, "Header", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, enabled: enabled);

        // Input
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackEnter = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtInput", 93, 75, 162, 18, font: Font.Arial, align: Alignment.Center, xOffset: 5, yOffset: 2, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, callbackEnter: argcallbackEnter);

        // Labels
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBody_1", 15, 60, 314, 10, "Invalid username or password.", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5, enabled: enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBody_2", 15, 75, 314, 10, "Please try again!", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6, enabled: enabled);

        // Buttons
        var argcallbackMousedown7 = new Action(WinDialogue.OnYes);
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnYes", 104, 98, 68, 24, "Yes", Font.Arial, 0, 0, 0, 0, false, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown7, argcallbackDblclick7, argcallbackMousemove7);
        var argcallbackMousedown8 = new Action(WinDialogue.OnNo);
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnNo", 180, 98, 68, 24, "No", Font.Arial, 0, 0, 0, 0, false, 255, Design.Red, Design.RedHover, Design.RedClick, argcallbackNorm, argcallbackHover, argcallbackMousedown8, argcallbackMousemove8, argcallbackDblclick8);
        var argcallbackMousedown9 = new Action(WinDialogue.OnOkay);
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnOkay", 140, 98, 68, 24, "Okay", Font.Arial, 0, 0, 0, 0, true, 255, Design.Green, Design.GreenHover, Design.GreenClick, argcallbackNorm, argcallbackHover, argcallbackMousedown9, argcallbackMousemove9, argcallbackDblclick9);

        // Set active control
        Gui.SetActiveControl(Gui.Windows.Count, Gui.GetControlIndex("winDialogue", "txtInput"));
    }

    public void UpdateWindow_Party()
    {
        // Control window
        Gui.UpdateWindow("winParty", "", Font.Georgia, Gui.ZOrderWin, 4, 78, 252, 158, 0, false, designNorm: Design.WindowParty, designHover: Design.WindowParty, designMousedown: Design.WindowParty, canDrag: false);

        // Name labels
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName1", 60, 20, 173, 10, "Richard - Level 10", Font.Arial, Color.White, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, enabled: enabled);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName2", 60, 60, 173, 10, "Anna - Level 18", Font.Arial, Color.White, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, enabled: enabled);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName3", 60, 100, 173, 10, "Doleo - Level 25", Font.Arial, Color.White, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, enabled: enabled);

        // Empty Bars - HP
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_HP1", 58, 34, 173, 9, imageNorm: 62, imageHover: 62, imageMousedown: 62, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, onDraw: argonDraw);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_HP2", 58, 74, 173, 9, imageNorm: 62, imageHover: 62, imageMousedown: 62, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw1);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_HP3", 58, 114, 173, 9, imageNorm: 62, imageHover: 62, imageMousedown: 62, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5, onDraw: argonDraw2);

        // Empty Bars - SP
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_SP1", 58, 44, 173, 9, imageNorm: 63, imageHover: 63, imageMousedown: 63, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6, onDraw: argonDraw3);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw4 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_SP2", 58, 84, 173, 9, imageNorm: 63, imageHover: 63, imageMousedown: 63, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7, onDraw: argonDraw4);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw5 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEmptyBar_SP3", 58, 124, 173, 9, imageNorm: 63, imageHover: 63, imageMousedown: 63, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, onDraw: argonDraw5);

        // Filled bars - HP
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Action argonDraw6 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_HP1", 58, 34, 173, 9, imageNorm: 64, imageHover: 64, imageMousedown: 64, callbackNorm: argcallbackNorm9, callbackHover: argcallbackHover9, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, onDraw: argonDraw6);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argonDraw7 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_HP2", 58, 74, 173, 9, imageNorm: 64, imageHover: 64, imageMousedown: 64, callbackNorm: argcallbackNorm10, callbackHover: argcallbackHover10, callbackMousedown: argcallbackMousedown10, callbackMousemove: argcallbackMousemove10, callbackDblclick: argcallbackDblclick10, onDraw: argonDraw7);
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argonDraw8 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_HP3", 58, 114, 173, 9, imageNorm: 64, imageHover: 64, imageMousedown: 64, callbackNorm: argcallbackNorm11, callbackHover: argcallbackHover11, callbackMousedown: argcallbackMousedown11, callbackMousemove: argcallbackMousemove11, callbackDblclick: argcallbackDblclick11, onDraw: argonDraw8);

        // Filled bars - SP
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argonDraw9 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_SP1", 58, 44, 173, 9, imageNorm: 65, imageHover: 65, imageMousedown: 65, callbackNorm: argcallbackNorm12, callbackHover: argcallbackHover12, callbackMousedown: argcallbackMousedown12, callbackMousemove: argcallbackMousemove12, callbackDblclick: argcallbackDblclick12, onDraw: argonDraw9);
        Action argcallbackNorm13 = null;
        Action argcallbackHover13 = null;
        Action argcallbackMousedown13 = null;
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Action argonDraw10 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_SP2", 58, 84, 173, 9, imageNorm: 65, imageHover: 65, imageMousedown: 65, callbackNorm: argcallbackNorm13, callbackHover: argcallbackHover13, callbackMousedown: argcallbackMousedown13, callbackMousemove: argcallbackMousemove13, callbackDblclick: argcallbackDblclick13, onDraw: argonDraw10);
        Action argcallbackNorm14 = null;
        Action argcallbackHover14 = null;
        Action argcallbackMousedown14 = null;
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Action argonDraw11 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar_SP3", 58, 124, 173, 9, imageNorm: 65, imageHover: 65, imageMousedown: 65, callbackNorm: argcallbackNorm14, callbackHover: argcallbackHover14, callbackMousedown: argcallbackMousedown14, callbackMousemove: argcallbackMousemove14, callbackDblclick: argcallbackDblclick14, onDraw: argonDraw11);

        // Shadows
        // Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow1", 20, 24, 32, 32, , , , , Tex_Shadow, Tex_Shadow, Tex_Shadow
        // Client.Gui.UpdatePictureBox Client.Gui.Windows.Count, "picShadow2", 20, 64, 32, 32, , , , , Tex_Shadow, Tex_Shadow, Tex_Shadow
        // Client.Gui.UpdatePictureBox Client.Gui.Windows.Count, "picShadow3", 20, 104, 32, 32, , , , , Tex_Shadow, Tex_Shadow, Tex_Shadow

        // Characters
        Action argcallbackNorm15 = null;
        Action argcallbackHover15 = null;
        Action argcallbackMousedown15 = null;
        Action argcallbackMousemove15 = null;
        Action argcallbackDblclick15 = null;
        Action argonDraw12 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picChar1", 20, 20, 32, 32, imageNorm: 0, imageHover: 0, imageMousedown: 0, texturePath: DataPath.Characters, callbackNorm: argcallbackNorm15, callbackHover: argcallbackHover15, callbackMousedown: argcallbackMousedown15, callbackMousemove: argcallbackMousemove15, callbackDblclick: argcallbackDblclick15, onDraw: argonDraw12);
        Action argcallbackNorm16 = null;
        Action argcallbackHover16 = null;
        Action argcallbackMousedown16 = null;
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Action argonDraw13 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picChar2", 20, 60, 32, 32, imageNorm: 0, imageHover: 0, imageMousedown: 0, texturePath: DataPath.Characters, callbackNorm: argcallbackNorm16, callbackHover: argcallbackHover16, callbackMousedown: argcallbackMousedown16, callbackMousemove: argcallbackMousemove16, callbackDblclick: argcallbackDblclick16, onDraw: argonDraw13);
        Action argcallbackNorm17 = null;
        Action argcallbackHover17 = null;
        Action argcallbackMousedown17 = null;
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Action argonDraw14 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picChar3", 20, 100, 32, 32, imageNorm: 0, imageHover: 0, imageMousedown: 0, texturePath: DataPath.Characters, callbackNorm: argcallbackNorm17, callbackHover: argcallbackHover17, callbackMousedown: argcallbackMousedown17, callbackMousemove: argcallbackMousemove17, callbackDblclick: argcallbackDblclick17, onDraw: argonDraw14);
    }

    public void UpdateWindow_Trade()
    {
        // Control window
        Gui.UpdateWindow("winTrade", "Trading with [Name]", Font.Georgia, Gui.ZOrderWin, 0, 0, 412, 386, 112, false, 2, 5, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, onDraw: WinTrade.OnDraw);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Close Button
        var argcallbackMousedown = new Action(WinTrade.OnClose);
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 36, 36, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown, callbackHover: argcallbackHover, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 10, 312, 392, 66, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);

        // Labels
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        bool enabled = false;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow", 36, 30, 142, 9, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw1);
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblYourTrade", 36, 27, 142, 9, "Robin's Offer", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, enabled: enabled);
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow", 36 + 200, 30, 142, 9, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw2);
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblTheirTrade", 36 + 200, 27, 142, 9, "Richard's Offer", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5, enabled: enabled);

        // Buttons
        var argcallbackMousedown6 = new Action(WinTrade.OnAccept);
        Gui.UpdateButton(Gui.Windows.Count, "btnAccept", 134, 340, 68, 24, "Accept", designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown6, callbackHover: argcallbackHover, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        var argcallbackMousedown7 = new Action(WinTrade.OnClose);
        Gui.UpdateButton(Gui.Windows.Count, "btnDecline", 210, 340, 68, 24, "Decline", designNorm: Design.Red, designHover: Design.RedHover, designMousedown: Design.RedClick, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown7, callbackHover: argcallbackHover, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Labels
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStatus", 114, 322, 184, 10, "", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, enabled: enabled);

        // Amounts
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBlank", 25, 330, 100, 10, "Total Value", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, enabled: enabled);
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBlank", 285, 330, 100, 10, "Total Value", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown10, callbackMousemove: argcallbackMousemove10, callbackDblclick: argcallbackDblclick10, enabled: enabled);
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblYourValue", 25, 344, 100, 10, "52,812g", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown11, callbackMousemove: argcallbackMousemove11, callbackDblclick: argcallbackDblclick11, enabled: enabled);
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblTheirValue", 285, 344, 100, 10, "12,531g", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown12, callbackMousemove: argcallbackMousemove12, callbackDblclick: argcallbackDblclick12, enabled: enabled);

        // Item Containers
        var argcallbackMousedown13 = new Action(WinTrade.OnYourTradeMouseMove);
        var argcallbackMousemove13 = new Action(WinTrade.OnYourTradeMouseMove);
        var argcallbackDblclick13 = new Action(WinTrade.OnYourTradeClick);
        var argonDraw3 = new Action(Gui.DrawYourTrade);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picYour", 14, 46, 184, 260, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown13, callbackHover: argcallbackHover, callbackMousemove: argcallbackMousemove13, callbackDblclick: argcallbackDblclick13, onDraw: argonDraw3);
        var argcallbackMousedown14 = new Action(WinTrade.OnTheirTradeMouseMove);
        var argcallbackMousemove14 = new Action(WinTrade.OnTheirTradeMouseMove);
        var argcallbackDblclick14 = new Action(WinTrade.OnTheirTradeMouseMove);
        var argonDraw4 = new Action(Gui.DrawTheirTrade);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picTheir", 214, 46, 184, 260, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown14, callbackHover: argcallbackHover, callbackMousemove: argcallbackMousemove14, callbackDblclick: argcallbackDblclick14, onDraw: argonDraw4);
    }

    public void UpdateWindow_EscMenu()
    {
        // Control window
        Gui.UpdateWindow("winEscMenu", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 210, 156, 0, false, designNorm: Design.WindowNoBar, designHover: Design.WindowNoBar, designMousedown: Design.WindowNoBar, canDrag: false, clickThrough: false);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 6, 198, 144, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, onDraw: argonDraw);

        // Buttons
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = WinEscMenu.OnClose;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnReturn", 16, 16, 178, 28, "Return to Game", designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1);

        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = WinEscMenu.OnOptionsClick;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnOptions", 16, 48, 178, 28, "Options", designNorm: Design.Orange, designHover: Design.OrangeHover, designMousedown: Design.OrangeClick, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2);

        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = WinEscMenu.OnMainMenuClick;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnMainMenu", 16, 80, 178, 28, "Back to Main Menu", designNorm: Design.Blue, designHover: Design.BlueHover, designMousedown: Design.BlueClick, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3);

        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = WinEscMenu.OnExitClick;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnExit", 16, 112, 178, 28, "Exit the Game", designNorm: Design.Red, designHover: Design.RedHover, designMousedown: Design.RedClick, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4);
    }

    public void UpdateWindow_Bars()
    {
        // Control window
        Gui.UpdateWindow("winBars", "", Font.Georgia, Gui.ZOrderWin, 10, 10, 239, 77, 0, false, designNorm: Design.WindowNoBar, designHover: Design.WindowNoBar, designMousedown: Design.WindowNoBar, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 6, 227, 65, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, onDraw: argonDraw);

        // Blank Bars
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picHP_Blank", 15, 15, 209, 13, imageNorm: 24, imageHover: 24, imageMousedown: 24, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picSP_Blank", 15, 32, 209, 13, imageNorm: 25, imageHover: 25, imageMousedown: 25, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picEXP_Blank", 15, 49, 209, 13, imageNorm: 26, imageHover: 26, imageMousedown: 26, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, onDraw: argonDraw3);

        // Bars
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        var argonDraw4 = new Action(Gui.Bars_OnDraw);
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlank", 0, 0, 0, 0, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw5 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picHealth", 16, 10, 44, 14, imageNorm: 21, imageHover: 21, imageMousedown: 21, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5, onDraw: argonDraw5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw6 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picSpirit", 16, 28, 44, 14, imageNorm: 22, imageHover: 22, imageMousedown: 22, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6, onDraw: argonDraw6);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw7 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picExperience", 16, 45, 74, 14, imageNorm: 23, imageHover: 23, imageMousedown: 23, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7, onDraw: argonDraw7);

        // Labels
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblHP", 15, 14, 209, 10, "999/999", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, enabled: enabled);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblMP", 15, 30, 209, 10, "999/999", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm9, callbackHover: argcallbackHover9, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, enabled: enabled);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblEXP", 15, 48, 209, 10, "999/999", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm10, callbackHover: argcallbackHover10, callbackMousedown: argcallbackMousedown10, callbackMousemove: argcallbackMousemove10, callbackDblclick: argcallbackDblclick10, enabled: enabled);
    }

    public void UpdateWindow_Chat()
    {
        // Control window
        Gui.UpdateWindow("winChat", "", Font.Georgia, Gui.ZOrderWin, 8, Client.GameState.ResolutionHeight - 178, 352, 152, 0, false, canDrag: false);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Channel boxes
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = WinChat.OnGameChannelClicked;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkGame", 10, 2, 49, 23, 0, "Game", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackMousedown = WinChat.OnMapChannelClicked;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkMap", 60, 2, 49, 23, 0, "Map", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackMousedown = WinChat.OnBroadcastChannelClicked;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkGlobal", 110, 2, 49, 23, 0, "Global", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackMousedown = WinChat.OnPartyChannelClicked;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkParty", 160, 2, 49, 23, 0, "Party", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackMousedown = WinChat.OnGuildChannelClicked;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkClient.Guild", 210, 2, 49, 23, 0, "Client.Guild", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackMousedown = WinChat.OnPrivateChannelClicked;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkPlayer", 260, 2, 49, 23, 0, "Player", Font.Arial, theDesign: Design.CheckboxChat, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Blank picturebox
        var argonDraw = new Action(WinChat.OnDraw);
        Action argcallbackNormPic = null;
        Action argcallbackHoverPic = null;
        Action argcallbackMousedownPic = null;
        Action argcallbackMousemovePic = null;
        Action argcallbackDblclickPic = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picNull", 0, 0, 0, 0, onDraw: argonDraw, callbackNorm: argcallbackNormPic, callbackHover: argcallbackHoverPic, callbackMousedown: argcallbackMousedownPic, callbackMousemove: argcallbackMousemovePic, callbackDblclick: argcallbackDblclickPic);

        // Chat button
        argcallbackNorm = WinChat.OnSayClick;
        Gui.UpdateButton(Gui.Windows.Count, "btnChat", 296, (124 + 16), 48, 20, "Say", Font.Arial, designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Chat Textbox
        Action argcallbackEnter = null;
        Gui.UpdateTextbox(Gui.Windows.Count, "txtChat", 12, 127 + 16, 352, 25, font: Font.Georgia, visible: false, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, callbackEnter: argcallbackEnter);

        // Buttons
        argcallbackNorm = WinChat.OnUpButtonMouseDown;
        Gui.UpdateButton(Gui.Windows.Count, "btnUp", 328, 28, 10, 13, imageNorm: 4, imageHover: 52, imageMousedown: 4, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
        argcallbackNorm = WinChat.OnDownButtonMouseDown;
        Gui.UpdateButton(Gui.Windows.Count, "btnDown", 327, 122, 10, 13, imageNorm: 5, imageHover: 53, imageMousedown: 5, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Custom Handlers for mouse up
        Gui.Windows[Gui.Windows.Count].Controls[Gui.GetControlIndex("winChat", "btnUp")].CallBack[(int) ControlState.MouseUp] = WinChat.OnUpButtonMouseUp;
        Gui.Windows[Gui.Windows.Count].Controls[Gui.GetControlIndex("winChat", "btnDown")].CallBack[(int) ControlState.MouseUp] = WinChat.OnDownButtonMouseUp;

        // Set the active control
        Gui.SetActiveControl(Gui.GetWindowIndex("winChat"), Gui.GetControlIndex("winChat", "txtChat"));

        // sort out the tabs
        {
            var withBlock = Gui.Windows[Gui.GetWindowIndex("winChat")];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkGame")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Game];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkMap")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Map];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkGlobal")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Broadcast];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkParty")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Party];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkkGuild")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Guild];
            withBlock.Controls[Gui.GetControlIndex("winChat", "chkPlayer")].Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Private];
        }
    }

    public void UpdateWindow_ChatSmall()
    {
        // Control window
        Gui.UpdateWindow("winChatSmall", "", Font.Georgia, Gui.ZOrderWin, 8, 0, 0, 0, 0, false, onDraw: WinChat.OnDrawSmall, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Chat Label
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblMsg", 12, 140, 286, 25, "Press 'Enter' to open chat", Font.Georgia, Color.White, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, enabled: enabled);
    }

    public void UpdateWindow_Hotbar()
    {
        // Control window
        Gui.UpdateWindow("winHotbar", "", Font.Georgia, Gui.ZOrderWin, 432, 10, 418, 36, 0, false, callbackMousemove: WinHotBar.OnMouseMove, callbackMousedown: WinHotBar.OnMouseDown, callbackDblclick: WinHotBar.OnDoubleClick, onDraw: WinHotBar.OnDraw, canDrag: false, zChange: Conversions.ToByte(false));
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
            isActive: false,
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
            winNum: windowIndex,
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
            winNum: windowIndex,
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
            winNum: windowIndex,
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
            winNum: windowIndex,
            name: "btnMap",
            left: 119, top: 0, width: 29, height: 29,
            icon: 106,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnMapClick,
            xOffset: -1, yOffset: -2);

        Gui.UpdateButton(
            winNum: windowIndex,
            name: "btnClient.Guild",
            left: 155, top: 0, width: 29, height: 29,
            icon: 107,
            designNorm: Design.Grey,
            designHover: Design.Grey,
            designMousedown: Design.Grey,
            callbackMousedown: WinMenu.OnGuildClick,
            xOffset: -1, yOffset: -1);

        Gui.UpdateButton(
            winNum: windowIndex,
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
        // Control window
        Gui.UpdateWindow("winInventory", "Inventory", Font.Georgia, Gui.ZOrderWin, 0, 0, 202, 319, 1, false, 2, 7, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, callbackMousemove: WinInventory.OnMouseMove, callbackMousedown: WinInventory.OnMouseDown, callbackDblclick: WinInventory.OnDoubleClick, onDraw: WinInventory.OnDraw);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinMenu.OnInventoryClick);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, "", Font.Georgia, 0, 8, 9, 10, true, 255, 0, 0, 0, argcallbackNorm, argcallbackHover, argcallbackMousedown, argcallbackMousemove, argcallbackDblclick);

        // Gold amount
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlank", 8, 293, 186, 18, imageNorm: 67, imageHover: 67, imageMousedown: 67, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        //UpdateLabel(Client.Gui.Windows.Count, "lblGold", 42L, 296L, 100L, 10L, "g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.Yellow, callback_norm: argcallback_norm1, callback_hover: argcallback_hover1, callback_mousedown: argcallback_mousedown2, callback_mousemove: argcallback_mousemove2, callback_dblclick: argcallback_dblclick2, enabled: enabled);

        // Drop
        //Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDrop", 155L, 294L, 38L, 16L, "Drop", Core.Font.Georgia, 0L, 0L, 0L, 0L, true, 255L, UiDesign.Green, UiDesign.GreenHover, UiDesign.GreenClick, argcallback_norm, argcallback_hover, argcallback_mousedown, argcallback_mousemove, argcallback_dblclick, 5L, 3L, "", false, true);
    }

    public void UpdateWindow_Character()
    {
        // Control window
        Gui.UpdateWindow("winCharacter", "Character", Font.Georgia, Gui.ZOrderWin, 0, 0, 174, 356, 62, false, 2, 6, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, callbackMousemove: WinCharacter.OnMouseMove, callbackMousedown: WinCharacter.OnMouseMove, callbackDblclick: WinCharacter.OnDoubleClick, onDraw: WinCharacter.OnDrawCharacter);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        Action argcallbackNorm = null;
        var argcallbackMousedown = new Action(WinMenu.OnCharacterClick);
        Action argcallbackHover = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 26, 162, 287, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);

        // White boxes
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 34, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 54, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, onDraw: argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 74, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw3);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw4 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 94, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5, onDraw: argonDraw4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw5 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 114, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6, onDraw: argonDraw5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw6 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 134, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7, onDraw: argonDraw6);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw7 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picWhiteBox", 13, 154, 148, 19, designNorm: Design.TextWhite, designHover: Design.TextWhite, designMousedown: Design.TextWhite, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, onDraw: argonDraw7);

        // Labels
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName", 18, 36, 147, 10, "Name", Font.Arial, Color.White, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, enabled: enabled);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblJob", 18, 56, 147, 10, "Job", Font.Arial, Color.White, callbackNorm: argcallbackNorm9, callbackHover: argcallbackHover9, callbackMousedown: argcallbackMousedown10, callbackMousemove: argcallbackMousemove10, callbackDblclick: argcallbackDblclick10, enabled: enabled);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLevel", 18, 76, 147, 10, "Level", Font.Arial, Color.White, callbackNorm: argcallbackNorm10, callbackHover: argcallbackHover10, callbackMousedown: argcallbackMousedown11, callbackMousemove: argcallbackMousemove11, callbackDblclick: argcallbackDblclick11, enabled: enabled);
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblClient.Guild", 18, 96, 147, 10, "Client.Guild", Font.Arial, Color.White, callbackNorm: argcallbackNorm11, callbackHover: argcallbackHover11, callbackMousedown: argcallbackMousedown12, callbackMousemove: argcallbackMousemove12, callbackDblclick: argcallbackDblclick12, enabled: enabled);
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Action argcallbackMousedown13 = null;
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblHealth", 18, 116, 147, 10, "Health", Font.Arial, Color.White, callbackNorm: argcallbackNorm12, callbackHover: argcallbackHover12, callbackMousedown: argcallbackMousedown13, callbackMousemove: argcallbackMousemove13, callbackDblclick: argcallbackDblclick13, enabled: enabled);
        Action argcallbackNorm13 = null;
        Action argcallbackHover13 = null;
        Action argcallbackMousedown14 = null;
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblSpirit", 18, 136, 147, 10, "Spirit", Font.Arial, Color.White, callbackNorm: argcallbackNorm13, callbackHover: argcallbackHover13, callbackMousedown: argcallbackMousedown14, callbackMousemove: argcallbackMousemove14, callbackDblclick: argcallbackDblclick14, enabled: enabled);
        Action argcallbackNorm14 = null;
        Action argcallbackHover14 = null;
        Action argcallbackMousedown15 = null;
        Action argcallbackMousemove15 = null;
        Action argcallbackDblclick15 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblExperience", 18, 156, 147, 10, "Experience", Font.Arial, Color.White, callbackNorm: argcallbackNorm14, callbackHover: argcallbackHover14, callbackMousedown: argcallbackMousedown15, callbackMousemove: argcallbackMousemove15, callbackDblclick: argcallbackDblclick15, enabled: enabled);
        Action argcallbackNorm15 = null;
        Action argcallbackHover15 = null;
        Action argcallbackMousedown16 = null;
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName2", 13, 36, 147, 10, "Name", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm15, callbackHover: argcallbackHover15, callbackMousedown: argcallbackMousedown16, callbackMousemove: argcallbackMousemove16, callbackDblclick: argcallbackDblclick16, enabled: enabled);
        Action argcallbackNorm16 = null;
        Action argcallbackHover16 = null;
        Action argcallbackMousedown17 = null;
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblJob2", 13, 56, 147, 10, "", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm16, callbackHover: argcallbackHover16, callbackMousedown: argcallbackMousedown17, callbackMousemove: argcallbackMousemove17, callbackDblclick: argcallbackDblclick17, enabled: enabled);
        Action argcallbackNorm17 = null;
        Action argcallbackHover17 = null;
        Action argcallbackMousedown18 = null;
        Action argcallbackMousemove18 = null;
        Action argcallbackDblclick18 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLevel2", 13, 76, 147, 10, "Level", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm17, callbackHover: argcallbackHover17, callbackMousedown: argcallbackMousedown18, callbackMousemove: argcallbackMousemove18, callbackDblclick: argcallbackDblclick18, enabled: enabled);
        Action argcallbackNorm18 = null;
        Action argcallbackHover18 = null;
        Action argcallbackMousedown19 = null;
        Action argcallbackMousemove19 = null;
        Action argcallbackDblclick19 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblClient.Guild2", 13, 96, 147, 10, "Client.Guild", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm18, callbackHover: argcallbackHover18, callbackMousedown: argcallbackMousedown19, callbackMousemove: argcallbackMousemove19, callbackDblclick: argcallbackDblclick19, enabled: enabled);
        Action argcallbackNorm19 = null;
        Action argcallbackHover19 = null;
        Action argcallbackMousedown20 = null;
        Action argcallbackMousemove20 = null;
        Action argcallbackDblclick20 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblHealth2", 13, 116, 147, 10, "Health", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm19, callbackHover: argcallbackHover19, callbackMousedown: argcallbackMousedown20, callbackMousemove: argcallbackMousemove20, callbackDblclick: argcallbackDblclick20, enabled: enabled);
        Action argcallbackNorm20 = null;
        Action argcallbackHover20 = null;
        Action argcallbackMousedown21 = null;
        Action argcallbackMousemove21 = null;
        Action argcallbackDblclick21 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblSpirit2", 13, 136, 147, 10, "Spirit", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm20, callbackHover: argcallbackHover20, callbackMousedown: argcallbackMousedown21, callbackMousemove: argcallbackMousemove21, callbackDblclick: argcallbackDblclick21, enabled: enabled);
        Action argcallbackNorm21 = null;
        Action argcallbackHover21 = null;
        Action argcallbackMousedown22 = null;
        Action argcallbackMousemove22 = null;
        Action argcallbackDblclick22 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblExperience2", 13, 156, 147, 10, "Experience", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm21, callbackHover: argcallbackHover21, callbackMousedown: argcallbackMousedown22, callbackMousemove: argcallbackMousemove22, callbackDblclick: argcallbackDblclick22, enabled: enabled);

        // Attributes
        Action argcallbackNorm22 = null;
        Action argcallbackHover22 = null;
        Action argcallbackMousedown23 = null;
        Action argcallbackMousemove23 = null;
        Action argcallbackDblclick23 = null;
        Action argonDraw8 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picShadow", 18, 176, 138, 9, designNorm: Design.BlackOval, designHover: Design.BlackOval, designMousedown: Design.BlackOval, callbackNorm: argcallbackNorm22, callbackHover: argcallbackHover22, callbackMousedown: argcallbackMousedown23, callbackMousemove: argcallbackMousemove23, callbackDblclick: argcallbackDblclick23, onDraw: argonDraw8);
        Action argcallbackNorm23 = null;
        Action argcallbackHover23 = null;
        Action argcallbackMousedown24 = null;
        Action argcallbackMousemove24 = null;
        Action argcallbackDblclick24 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 173, 138, 10, "Attributes", Font.Arial, Color.White, Alignment.Center, callbackNorm: argcallbackNorm23, callbackHover: argcallbackHover23, callbackMousedown: argcallbackMousedown24, callbackMousemove: argcallbackMousemove24, callbackDblclick: argcallbackDblclick24, enabled: enabled);

        // Black boxes
        Action argcallbackNorm24 = null;
        Action argcallbackHover24 = null;
        Action argcallbackMousedown25 = null;
        Action argcallbackMousemove25 = null;
        Action argcallbackDblclick25 = null;
        Action argonDraw9 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 186, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm24, callbackHover: argcallbackHover24, callbackMousedown: argcallbackMousedown25, callbackMousemove: argcallbackMousemove25, callbackDblclick: argcallbackDblclick25, onDraw: argonDraw9);
        Action argcallbackNorm25 = null;
        Action argcallbackHover25 = null;
        Action argcallbackMousedown26 = null;
        Action argcallbackMousemove26 = null;
        Action argcallbackDblclick26 = null;
        Action argonDraw10 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 206, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm25, callbackHover: argcallbackHover25, callbackMousedown: argcallbackMousedown26, callbackMousemove: argcallbackMousemove26, callbackDblclick: argcallbackDblclick26, onDraw: argonDraw10);
        Action argcallbackNorm26 = null;
        Action argcallbackHover26 = null;
        Action argcallbackMousedown27 = null;
        Action argcallbackMousemove27 = null;
        Action argcallbackDblclick27 = null;
        Action argonDraw11 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 226, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm26, callbackHover: argcallbackHover26, callbackMousedown: argcallbackMousedown27, callbackMousemove: argcallbackMousemove27, callbackDblclick: argcallbackDblclick27, onDraw: argonDraw11);
        Action argcallbackNorm27 = null;
        Action argcallbackHover27 = null;
        Action argcallbackMousedown28 = null;
        Action argcallbackMousemove28 = null;
        Action argcallbackDblclick28 = null;
        Action argonDraw12 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 246, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm27, callbackHover: argcallbackHover27, callbackMousedown: argcallbackMousedown28, callbackMousemove: argcallbackMousemove28, callbackDblclick: argcallbackDblclick28, onDraw: argonDraw12);
        Action argcallbackNorm28 = null;
        Action argcallbackHover28 = null;
        Action argcallbackMousedown29 = null;
        Action argcallbackMousemove29 = null;
        Action argcallbackDblclick29 = null;
        Action argonDraw13 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 266, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm28, callbackHover: argcallbackHover28, callbackMousedown: argcallbackMousedown29, callbackMousemove: argcallbackMousemove29, callbackDblclick: argcallbackDblclick29, onDraw: argonDraw13);
        Action argcallbackNorm29 = null;
        Action argcallbackHover29 = null;
        Action argcallbackMousedown30 = null;
        Action argcallbackMousemove30 = null;
        Action argcallbackDblclick30 = null;
        Action argonDraw14 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlackBox", 13, 286, 148, 19, designNorm: Design.TextBlack, designHover: Design.TextBlack, designMousedown: Design.TextBlack, callbackNorm: argcallbackNorm29, callbackHover: argcallbackHover29, callbackMousedown: argcallbackMousedown30, callbackMousemove: argcallbackMousemove30, callbackDblclick: argcallbackDblclick30, onDraw: argonDraw14);

        // Labels
        Action argcallbackNorm30 = null;
        Action argcallbackHover30 = null;
        Action argcallbackMousedown31 = null;
        Action argcallbackMousemove31 = null;
        Action argcallbackDblclick31 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 188, 138, 10, "Strength", Font.Arial, Color.Yellow, callbackNorm: argcallbackNorm30, callbackHover: argcallbackHover30, callbackMousedown: argcallbackMousedown31, callbackMousemove: argcallbackMousemove31, callbackDblclick: argcallbackDblclick3, enabled: enabled);
        Action argcallbackNorm31 = null;
        Action argcallbackHover31 = null;
        Action argcallbackMousedown32 = null;
        Action argcallbackMousemove32 = null;
        Action argcallbackDblclick32 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 208, 138, 10, "Vitality", Font.Arial, Color.Yellow, callbackNorm: argcallbackNorm31, callbackHover: argcallbackHover31, callbackMousedown: argcallbackMousedown32, callbackMousemove: argcallbackMousemove32, callbackDblclick: argcallbackDblclick32, enabled: enabled);
        Action argcallbackNorm32 = null;
        Action argcallbackHover32 = null;
        Action argcallbackMousedown33 = null;
        Action argcallbackMousemove33 = null;
        Action argcallbackDblclick33 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 228, 138, 10, "Intelligence", Font.Arial, Color.Yellow, callbackNorm: argcallbackNorm32, callbackHover: argcallbackHover32, callbackMousedown: argcallbackMousedown33, callbackMousemove: argcallbackMousemove33, callbackDblclick: argcallbackDblclick3, enabled: enabled);
        Action argcallbackNorm33 = null;
        Action argcallbackHover33 = null;
        Action argcallbackMousedown34 = null;
        Action argcallbackMousemove34 = null;
        Action argcallbackDblclick34 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 248, 138, 10, "Luck", Font.Arial, Color.Yellow, callbackNorm: argcallbackNorm33, callbackHover: argcallbackHover33, callbackMousedown: argcallbackMousedown34, callbackMousemove: argcallbackMousemove34, callbackDblclick: argcallbackDblclick34, enabled: enabled);
        Action argcallbackNorm34 = null;
        Action argcallbackHover34 = null;
        Action argcallbackMousedown35 = null;
        Action argcallbackMousemove35 = null;
        Action argcallbackDblclick35 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 268, 138, 10, "Spirit", Font.Arial, Color.Yellow, callbackNorm: argcallbackNorm34, callbackHover: argcallbackHover34, callbackMousedown: argcallbackMousedown35, callbackMousemove: argcallbackMousemove35, callbackDblclick: argcallbackDblclick35, enabled: enabled);
        Action argcallbackNorm35 = null;
        Action argcallbackHover35 = null;
        Action argcallbackMousedown36 = null;
        Action argcallbackMousemove36 = null;
        Action argcallbackDblclick36 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLabel", 18, 288, 138, 10, "Stat Points", Font.Arial, Color.Green, callbackNorm: argcallbackNorm35, callbackHover: argcallbackHover35, callbackMousedown: argcallbackMousedown36, callbackMousemove: argcallbackMousemove36, callbackDblclick: argcallbackDblclick36, enabled: enabled);

        // Buttons
        var argcallbackMousedown37 = new Action(WinCharacter.OnSpendPoint1);
        Action argcallbackMousemove37 = null;
        Action argcallbackDblclick37 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnStat_1", 144, 188, 15, 15, imageNorm: 48, imageHover: 49, imageMousedown: 50, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown37, callbackMousemove: argcallbackMousemove37, callbackDblclick: argcallbackDblclick37);
        var argcallbackMousedown38 = new Action(WinCharacter.OnSpendPoint2);
        Action argcallbackMousemove38 = null;
        Action argcallbackDblclick38 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnStat_2", 144, 208, 15, 15, imageNorm: 48, imageHover: 49, imageMousedown: 50, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown38, callbackMousemove: argcallbackMousemove38, callbackDblclick: argcallbackDblclick3);
        var argcallbackMousedown39 = new Action(WinCharacter.OnSpendPoint3);
        Action argcallbackMousemove39 = null;
        Action argcallbackDblclick39 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnStat_3", 144, 228, 15, 15, imageNorm: 48, imageHover: 49, imageMousedown: 50, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown39, callbackMousemove: argcallbackMousemove39, callbackDblclick: argcallbackDblclick39);
        var argcallbackMousedown40 = new Action(WinCharacter.OnSpendPoint4);
        Action argcallbackMousemove40 = null;
        Action argcallbackDblclick40 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnStat_4", 144, 248, 15, 15, imageNorm: 48, imageHover: 49, imageMousedown: 50, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown40, callbackMousemove: argcallbackMousemove40, callbackDblclick: argcallbackDblclick4);
        var argcallbackMousedown41 = new Action(WinCharacter.OnSpendPoint5);
        Action argcallbackMousemove41 = null;
        Action argcallbackDblclick41 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnStat_5", 144, 268, 15, 15, imageNorm: 48, imageHover: 49, imageMousedown: 50, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown41, callbackMousemove: argcallbackMousemove41, callbackDblclick: argcallbackDblclick41);

        // fake buttons
        Action argcallbackNorm36 = null;
        Action argcallbackHover36 = null;
        Action argcallbackMousedown42 = null;
        Action argcallbackMousemove42 = null;
        Action argcallbackDblclick42 = null;
        Action argonDraw15 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "btnGreyStat_1", 144, 188, 15, 15, imageNorm: 47, imageHover: 47, imageMousedown: 47, callbackNorm: argcallbackNorm36, callbackHover: argcallbackHover36, callbackMousedown: argcallbackMousedown42, callbackMousemove: argcallbackMousemove42, callbackDblclick: argcallbackDblclick42, onDraw: argonDraw15);
        Action argcallbackNorm37 = null;
        Action argcallbackHover37 = null;
        Action argcallbackMousedown43 = null;
        Action argcallbackMousemove43 = null;
        Action argcallbackDblclick43 = null;
        Action argonDraw16 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "btnGreyStat_2", 144, 208, 15, 15, imageNorm: 47, imageHover: 47, imageMousedown: 47, callbackNorm: argcallbackNorm37, callbackHover: argcallbackHover37, callbackMousedown: argcallbackMousedown43, callbackMousemove: argcallbackMousemove43, callbackDblclick: argcallbackDblclick43, onDraw: argonDraw16);
        Action argcallbackNorm38 = null;
        Action argcallbackHover38 = null;
        Action argcallbackMousedown44 = null;
        Action argcallbackMousemove44 = null;
        Action argcallbackDblclick44 = null;
        Action argonDraw17 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "btnGreyStat_3", 144, 228, 15, 15, imageNorm: 47, imageHover: 47, imageMousedown: 47, callbackNorm: argcallbackNorm38, callbackHover: argcallbackHover38, callbackMousedown: argcallbackMousedown44, callbackMousemove: argcallbackMousemove44, callbackDblclick: argcallbackDblclick44, onDraw: argonDraw17);
        Action argcallbackNorm39 = null;
        Action argcallbackHover39 = null;
        Action argcallbackMousedown45 = null;
        Action argcallbackMousemove45 = null;
        Action argcallbackDblclick45 = null;
        Action argonDraw18 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "btnGreyStat_4", 144, 248, 15, 15, imageNorm: 47, imageHover: 47, imageMousedown: 47, callbackNorm: argcallbackNorm39, callbackHover: argcallbackHover39, callbackMousedown: argcallbackMousedown45, callbackMousemove: argcallbackMousemove45, callbackDblclick: argcallbackDblclick45, onDraw: argonDraw18);
        Action argcallbackNorm40 = null;
        Action argcallbackHover40 = null;
        Action argcallbackMousedown46 = null;
        Action argcallbackMousemove46 = null;
        Action argcallbackDblclick46 = null;
        Action argonDraw19 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "btnGreyStat_5", 144, 268, 15, 15, imageNorm: 47, imageHover: 47, imageMousedown: 47, callbackNorm: argcallbackNorm40, callbackHover: argcallbackHover40, callbackMousedown: argcallbackMousedown46, callbackMousemove: argcallbackMousemove46, callbackDblclick: argcallbackDblclick46, onDraw: argonDraw19);

        // Labels
        Action argcallbackNorm41 = null;
        Action argcallbackHover41 = null;
        Action argcallbackMousedown47 = null;
        Action argcallbackMousemove47 = null;
        Action argcallbackDblclick47 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStat_1", 42, 188, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm41, callbackHover: argcallbackHover41, callbackMousedown: argcallbackMousedown47, callbackMousemove: argcallbackMousemove47, callbackDblclick: argcallbackDblclick47, enabled: enabled);
        Action argcallbackNorm42 = null;
        Action argcallbackHover42 = null;
        Action argcallbackMousedown48 = null;
        Action argcallbackMousemove48 = null;
        Action argcallbackDblclick48 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStat_2", 42, 208, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm42, callbackHover: argcallbackHover42, callbackMousedown: argcallbackMousedown48, callbackMousemove: argcallbackMousemove48, callbackDblclick: argcallbackDblclick48, enabled: enabled);
        Action argcallbackNorm43 = null;
        Action argcallbackHover43 = null;
        Action argcallbackMousedown49 = null;
        Action argcallbackMousemove49 = null;
        Action argcallbackDblclick49 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStat_3", 42, 228, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm43, callbackHover: argcallbackHover43, callbackMousedown: argcallbackMousedown49, callbackMousemove: argcallbackMousemove49, callbackDblclick: argcallbackDblclick49, enabled: enabled);
        Action argcallbackNorm44 = null;
        Action argcallbackHover44 = null;
        Action argcallbackMousedown50 = null;
        Action argcallbackMousemove50 = null;
        Action argcallbackDblclick50 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStat_4", 42, 248, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm44, callbackHover: argcallbackHover44, callbackMousedown: argcallbackMousedown50, callbackMousemove: argcallbackMousemove50, callbackDblclick: argcallbackDblclick50, enabled: enabled);
        Action argcallbackNorm45 = null;
        Action argcallbackHover45 = null;
        Action argcallbackMousedown51 = null;
        Action argcallbackMousemove51 = null;
        Action argcallbackDblclick51 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblStat_5", 42, 268, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm45, callbackHover: argcallbackHover45, callbackMousedown: argcallbackMousedown51, callbackMousemove: argcallbackMousemove51, callbackDblclick: argcallbackDblclick51, enabled: enabled);
        Action argcallbackNorm46 = null;
        Action argcallbackHover46 = null;
        Action argcallbackMousedown52 = null;
        Action argcallbackMousemove52 = null;
        Action argcallbackDblclick52 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblPoints", 57, 288, 100, 15, "255", Font.Arial, Color.White, Alignment.Right, callbackNorm: argcallbackNorm46, callbackHover: argcallbackHover46, callbackMousedown: argcallbackMousedown52, callbackMousemove: argcallbackMousemove52, callbackDblclick: argcallbackDblclick5, enabled: enabled);
    }

    public void UpdateWindow_Description()
    {
        // Control window
        Gui.UpdateWindow("winDescription", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 193, 142, 0, false, designNorm: Design.WindowDescription, designHover: Design.WindowDescription, designMousedown: Design.WindowDescription, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Name
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName", 8, 12, 177, 10, "Flame Sword", Font.Arial, Color.Blue, Alignment.Center, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, enabled: enabled);

        // Sprite box
        var argonDraw = new Action(WinDescription.OnDraw);
        Action argcallbackNormPic = null;
        Action argcallbackHoverPic = null;
        Action argcallbackMousedownPic = null;
        Action argcallbackMousemovePic = null;
        Action argcallbackDblclickPic = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picSprite", 18, 32, 68, 68, designNorm: Design.DescriptionPicture, designHover: Design.DescriptionPicture, designMousedown: Design.DescriptionPicture, callbackNorm: argcallbackNormPic, callbackHover: argcallbackHoverPic, callbackMousedown: argcallbackMousedownPic, callbackMousemove: argcallbackMousemovePic, callbackDblclick: argcallbackDblclickPic, onDraw: argonDraw);

        // Sep
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picSep", 96, 28, 0, 92, imageNorm: 44, imageHover: 44, imageMousedown: 44, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw1);

        // Requirements
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblJob", 5, 102, 92, 10, "Warrior", Font.Georgia, Color.Green, Alignment.Center, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, enabled: enabled);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblLevel", 5, 114, 92, 10, "Level 20", Font.Georgia, Color.Red, Alignment.Center, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, enabled: enabled);

        // Bar
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBar", 19, 114, 66, 12, false, imageNorm: 45, imageHover: 45, imageMousedown: 45, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4, onDraw: argonDraw2);
    }

    public void UpdateWindow_RightClick()
    {
        // Control window
        Gui.UpdateWindow("winRightClickBG", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 800, 600, 0, false, callbackMousedown: Gui.RightClick_Close, canDrag: false);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);
    }

    public void UpdateWindow_PlayerMenu()
    {
        // Control window  
        Gui.UpdateWindow("winPlayerMenu", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 110, 106, 0, false, designNorm: Design.WindowDescription, designHover: Design.WindowDescription, designMousedown: Design.WindowDescription, callbackMousedown: Gui.RightClick_Close, canDrag: false);

        // Centralize it  
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Name  
        var argcallbackMousedown = new Action(Gui.RightClick_Close);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnName", 8, 8, 94, 18, "[Name]", designNorm: Design.MenuHeader, designHover: Design.MenuHeader, designMousedown: Design.MenuHeader, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Options  
        var argcallbackMousedown1 = new Action(Gui.PlayerMenu_Party);
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnParty", 8, 26, 94, 18, "Invite to Party", designHover: Design.MenuOption, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1);

        var argcallbackMousedown2 = new Action(Gui.PlayerMenu_Trade);
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnTrade", 8, 44, 94, 18, "Request Trade", designHover: Design.MenuOption, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2);

        var argcallbackMousedown3 = new Action(Gui.PlayerMenu_Guild);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClient.Guild", 8, 62, 94, 18, "Invite to Client.Guild", designNorm: Design.MenuOption, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3);

        var argcallbackMousedown4 = new Action(Gui.PlayerMenu_Player);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnPM", 8, 80, 94, 18, "Private Message", designHover: Design.MenuOption, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4);
    }

    public void UpdateWindow_DragBox()
    {
        // Control window
        Gui.UpdateWindow("winDragBox", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 32, 32, 0, false, onDraw: Gui.DragBox_OnDraw);

        // Need to set up unique mouseup event
        Gui.Windows[Gui.Windows.Count].CallBack[(int) ControlState.MouseUp] = Gui.DragBox_Check;
    }

    public void UpdateWindow_Options()
    {
        Gui.UpdateWindow("winOptions", "", Font.Georgia, Gui.ZOrderWin, 0, 0, 210, 212, 0, Conversions.ToBoolean(0), designNorm: Design.WindowNoBar, designHover: Design.WindowNoBar, designMousedown: Design.WindowNoBar, isActive: false, clickThrough: false);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 6, 198, 200, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick, onDraw: argonDraw);

        // General
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlank", 35, 25, 140, 10, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBlank", 35, 22, 140, 0, "General Options", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, enabled: enabled);

        // Check boxes
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkMusic", 35, 40, 80, text: "Music", font: Font.Georgia, theDesign: Design.CheckboxNormal, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkSound", 115, 40, 80, text: "Sound", font: Font.Georgia, theDesign: Design.CheckboxNormal, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkAutotile", 35, 60, 80, text: "Autotile", font: Font.Georgia, theDesign: Design.CheckboxNormal, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "chkFullscreen", 115, 60, 80, text: "Fullscreen", font: Font.Georgia, theDesign: Design.CheckboxNormal, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6);

        // Resolution
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picBlank", 35, 85, 140, 10, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7, onDraw: argonDraw2);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblBlank", 35, 92, 140, 10, "Select Resolution", Font.Georgia, Color.White, Alignment.Center, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, enabled: enabled);

        // combobox
        Gui.UpdateComboBox(Gui.Windows.Count, "cmbRes", 30, 100, 150, 18, Design.ComboBoxNormal);

        // Button
        Action argcallbackMousedown9 = WinOptions.OnConfirm;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnConfirm", 65, 168, 80, 22, "Confirm", designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackHover: argcallbackHover, callbackNorm: argcallbackNorm, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9);

        // Populate the options screen
        Client.GameLogic.SetOptionsScreen();
    }

    public void UpdateWindow_Combobox()
    {
        // background window
        Gui.UpdateWindow("winComboMenuBG", "ComboMenuBG", Font.Georgia, Gui.ZOrderWin, 0, 0, 800, 600, 0, false, callbackDblclick: Gui.CloseComboMenu, zChange: Conversions.ToByte(false), isActive: false);

        // window
        Gui.UpdateWindow("winComboMenu", "ComboMenu", Font.Georgia, Gui.ZOrderWin, 0, 0, 100, 100, 0, false, designNorm: Design.ComboMenuNormal, isActive: false, clickThrough: false);

        // centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);
    }

    public void UpdateWindow_Skills()
    {
        // Control window
        Gui.UpdateWindow("winSkills", "Skills", Font.Georgia, Gui.ZOrderWin, 0, 0, 202, 297, 109, false, 2, 7, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, callbackMousemove: WinSkills.OnMouseMove, callbackMousedown: WinSkills.OnMouseDown, callbackDblclick: WinSkills.OnDoubleClick, onDraw: Gui.DrawSkills);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;

        // Close button
        var argcallbackMousedown = new Action(WinMenu.OnSkillsClick);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 16, 16, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
    }

    public void UpdateWindow_Bank()
    {
        Gui.UpdateWindow("winBank", "Bank", Font.Georgia, Gui.ZOrderWin, 0, 0, 390, 373, 0, false, 2, 5, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, callbackMousemove: WinBank.OnMouseMove, callbackMousedown: WinBank.OnMouseDown, callbackDblclick: WinBank.OnDoubleClick, onDraw: WinBank.OnDraw);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Set the index for spawning controls
        Gui.ZOrderCon = 0;
        var argcallbackMousedown = new Action(WinBank.OnClose);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 5, 36, 36, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);
    }

    public void UpdateWindow_Shop()
    {
        // Control window
        Gui.UpdateWindow("winShop", "Shop", Font.Georgia, Gui.ZOrderWin, 0, 0, 278, 293, 17, false, 2, 5, Design.WindowEmpty, Design.WindowEmpty, Design.WindowEmpty, callbackMousemove: WinShop.OnMouseMove, callbackMousedown: WinShop.OnMouseDown, onDraw: WinShop.OnDrawBackground);

        // Centralize it
        Gui.CentralizeWindow(Gui.Windows.Count);

        // Close button
        var argcallbackMousedown = new Action(WinShop.OnClose);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnClose", Gui.Windows[Gui.Windows.Count].Width - 19, 6, 36, 36, imageNorm: 8, imageHover: 9, imageMousedown: 10, callbackNorm: argcallbackNorm, callbackHover: argcallbackHover, callbackMousedown: argcallbackMousedown, callbackMousemove: argcallbackMousemove, callbackDblclick: argcallbackDblclick);

        // Parchment
        var argonDraw = new Action(WinShop.OnDraw);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picParchment", 6, 215, 266, 50, designNorm: Design.Parchment, designHover: Design.Parchment, designMousedown: Design.Parchment, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown1, callbackMousemove: argcallbackMousemove1, callbackDblclick: argcallbackDblclick1, onDraw: argonDraw);

        // Picture Box
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picItemBG", 13, 222, 36, 36, imageNorm: 30, imageHover: 30, imageMousedown: 30, callbackNorm: argcallbackNorm1, callbackHover: argcallbackHover1, callbackMousedown: argcallbackMousedown2, callbackMousemove: argcallbackMousemove2, callbackDblclick: argcallbackDblclick2, onDraw: argonDraw2);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw3 = null;
        Gui.UpdatePictureBox(Gui.Windows.Count, "picItem", 15, 224, 32, 32, callbackNorm: argcallbackNorm2, callbackHover: argcallbackHover2, callbackMousedown: argcallbackMousedown3, callbackMousemove: argcallbackMousemove3, callbackDblclick: argcallbackDblclick3, onDraw: argonDraw3);

        // Buttons
        var argcallbackMousedown4 = new Action(WinShop.OnBuy);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnBuy", 190, 228, 70, 24, "Buy", Font.Arial, designNorm: Design.Green, designHover: Design.GreenHover, designMousedown: Design.GreenClick, callbackNorm: argcallbackNorm3, callbackHover: argcallbackHover3, callbackMousedown: argcallbackMousedown4, callbackMousemove: argcallbackMousemove4, callbackDblclick: argcallbackDblclick4);
        var argcallbackMousedown5 = new Action(WinShop.OnSell);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Gui.UpdateButton(Gui.Windows.Count, "btnSell", 190, 228, 70, 24, "Sell", Font.Arial, visible: false, designNorm: Design.Red, designHover: Design.RedHover, designMousedown: Design.RedClick, callbackNorm: argcallbackNorm4, callbackHover: argcallbackHover4, callbackMousedown: argcallbackMousedown5, callbackMousemove: argcallbackMousemove5, callbackDblclick: argcallbackDblclick5);

        // Buying/Selling
        var argcallbackMousedown6 = new Action(WinShop.OnBuyingChecked);
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "CheckboxBuying", 173, 265, 49, 20, theDesign: Design.CheckboxBuying, callbackNorm: argcallbackNorm5, callbackHover: argcallbackHover5, callbackMousedown: argcallbackMousedown6, callbackMousemove: argcallbackMousemove6, callbackDblclick: argcallbackDblclick6);
        var argcallbackMousedown7 = new Action(WinShop.OnSellingChecked);
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Gui.UpdateCheckBox(Gui.Windows.Count, "CheckboxSelling", 222, 265, 49, 20, theDesign: Design.CheckboxSelling, callbackNorm: argcallbackNorm6, callbackHover: argcallbackHover6, callbackMousedown: argcallbackMousedown7, callbackMousemove: argcallbackMousemove7, callbackDblclick: argcallbackDblclick7);

        // Labels
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        bool enabled = false;
        Gui.UpdateLabel(Gui.Windows.Count, "lblName", 56, 226, 300, 10, "Test Item", Font.Arial, Color.Black, callbackNorm: argcallbackNorm7, callbackHover: argcallbackHover7, callbackMousedown: argcallbackMousedown8, callbackMousemove: argcallbackMousemove8, callbackDblclick: argcallbackDblclick8, enabled: enabled);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Gui.UpdateLabel(Gui.Windows.Count, "lblCost", 56, 240, 300, 10, "1000g", Font.Arial, Color.Black, callbackNorm: argcallbackNorm8, callbackHover: argcallbackHover8, callbackMousedown: argcallbackMousedown9, callbackMousemove: argcallbackMousemove9, callbackDblclick: argcallbackDblclick9, enabled: enabled);

        // Gold
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        //UpdateLabel(Client.Gui.Windows.Count, "lblGold", 44L, 269L, 300L, 10L, "g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Left, callback_norm: argcallback_norm9, callback_hover: argcallback_hover9, callback_mousedown: argcallback_mousedown10, callback_mousemove: argcallback_mousemove10, callback_dblclick: argcallback_dblclick10, enabled: enabled);
    }
}
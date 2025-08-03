using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Crystalshire
{

    public void UpdateWindow_Login()
    {
        // Control the window
        Client.Gui.UpdateWindow("winLogin", "Login", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 276L, 212L, 45L, true, 3L, 5L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 264L, 180L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, onDraw: ref argonDraw);

        // Shadows
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_1", 67L, 43L, 142L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_2", 67L, 79L, 142L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw2);

        // Close button
        var argcallbackMousedown3 = new Action(Client.General.DestroyGame);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3);

        // Buttons
        var argcallbackMousedown4 = new Action(Client.NetworkSend.btnLogin_Click);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnAccept", 67L, 134L, 67L, 22L, "Accept", Core.Font.Arial, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4);
        var argcallbackMousedown5 = new Action(Client.General.DestroyGame);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnExit", 142L, 134L, 67L, 22L, "Exit", Core.Font.Arial, designNorm: (long)UiDesign.Red, designHover: (long)UiDesign.RedHover, designMousedown: (long)UiDesign.RedClick, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5);

        // Labels
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblUsername", 72L, 39L, 142L, 10L, "Username", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6, enabled: ref enabled);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblPassword", 72L, 75L, 142L, 10L, "Password", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7, enabled: ref enabled);

        // Textboxes
        if (SettingsManager.Instance.SaveUsername == true)
        {
            Action argcallbackNorm8 = null;
            Action argcallbackHover8 = null;
            Action argcallbackMousedown8 = null;
            Action argcallbackMousemove8 = null;
            Action argcallbackDblclick8 = null;
            Action argcallbackEnter = null;
            Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtUsername", 67L, 55L, 142L, 19L, SettingsManager.Instance.Username, Core.Font.Arial, Alignment.Left, xOffset: 5L, yOffset: 3L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, callbackEnter: ref argcallbackEnter);
        }
        else
        {
            Action argcallbackNorm9 = null;
            Action argcallbackHover9 = null;
            Action argcallbackMousedown9 = null;
            Action argcallbackMousemove9 = null;
            Action argcallbackDblclick9 = null;
            Action argcallbackEnter1 = null;
            Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtUsername", 67L, 55L, 142L, 19L, "", Core.Font.Arial, Alignment.Left, xOffset: 5L, yOffset: 3L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm9, callbackHover: ref argcallbackHover9, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, callbackEnter: ref argcallbackEnter1);
        }
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argcallbackEnter2 = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtPassword", 67L, 86L, 142L, 19L, font: Core.Font.Arial, align: Alignment.Left, xOffset: 5L, yOffset: 3L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, censor: true, callbackNorm: ref argcallbackNorm10, callbackHover: ref argcallbackHover10, callbackMousedown: ref argcallbackMousedown10, callbackMousemove: ref argcallbackMousemove10, callbackDblclick: ref argcallbackDblclick10, callbackEnter: ref argcallbackEnter2);

        // Checkbox
        var argcallbackMousedown11 = new Action(Client.Gui.chkSaveUser_Click);
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkSaveUsername", 67L, 114L, 142L, value: Conversions.ToLong(SettingsManager.Instance.SaveUsername), text: "Save Username?", font: Core.Font.Arial, theDesign: (long)UiDesign.CheckboxNormal, callbackNorm: ref argcallbackNorm11, callbackHover: ref argcallbackHover11, callbackMousedown: ref argcallbackMousedown11, callbackMousemove: ref argcallbackMousemove11, callbackDblclick: ref argcallbackDblclick11);

        // Register Button
        var argcallbackMousedown12 = new Action(Client.Gui.btnRegister_Click);
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnRegister", 12L, Client.Gui.Windows[Client.Gui.Windows.Count].Height - 35L, 252L, 22L, "Register Account", Core.Font.Arial, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm12, callbackHover: ref argcallbackHover12, callbackMousedown: ref argcallbackMousedown12, callbackMousemove: ref argcallbackMousemove12, callbackDblclick: ref argcallbackDblclick12);

        // Set the active control
        if (!(Strings.Len(Client.Gui.Windows[Client.Gui.GetWindowIndex("winLogin")].Controls[Client.Gui.GetControlIndex("winLogin", "txtUsername")].Text) > 0))
        {
            Client.Gui.SetActiveControl(Client.Gui.GetWindowIndex("winLogin"), Client.Gui.GetControlIndex("winLogin", "txtUsername"));
        }
        else
        {
            Client.Gui.SetActiveControl(Client.Gui.GetWindowIndex("winLogin"), Client.Gui.GetControlIndex("winLogin", "txtPassword"));
        }
    }

    public void UpdateWindow_Register()
    {
        // Control the window
        Client.Gui.UpdateWindow("winRegister", "Register Account", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 276L, 202L, 45L, false, 3L, 5L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnReturnMain_Click);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 264L, 170L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);

        // Shadows
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_1", 67L, 43L, 142L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_2", 67L, 79L, 142L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, onDraw: ref argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_3", 67L, 115L, 142L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw3);
        // Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_4", 67, 151, 142, 9, , , , , , , , UiDesign.BlackOval, UiDesign.BlackOval, UiDesign.BlackOval)
        // Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_5", 67, 187, 142, 9, , , , , , , , UiDesign.BlackOval, UiDesign.BlackOval, UiDesign.BlackOval)

        // Buttons
        var argcallbackMousedown5 = new Action(Client.Gui.btnSendRegister_Click);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnAccept", 68L, 152L, 67L, 22L, "Accept", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown5, ref argcallbackMousemove5, ref argcallbackDblclick5, 0L, 0L, "", false);

        var argcallbackMousedown6 = new Action(Client.Gui.btnReturnMain_Click);
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnExit", 142L, 152L, 67L, 22L, "Back", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown6, ref argcallbackMousemove6, ref argcallbackDblclick6, 0L, 0L, "", false);

        // Labels
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblUsername", 66L, 39L, 142L, 10L, "Username", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm4, ref argcallbackHover4, ref argcallbackMousedown7, ref argcallbackMousemove7, ref argcallbackDblclick7, enabled: ref enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblPassword", 66L, 75L, 142L, 10L, "Password", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm5, ref argcallbackHover5, ref argcallbackMousedown8, ref argcallbackMousemove8, ref argcallbackDblclick8, enabled: ref enabled);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblRetypePassword", 66L, 110L, 142L, 10L, "Retype Password", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm6, ref argcallbackHover6, ref argcallbackMousedown9, ref argcallbackMousemove9, ref argcallbackDblclick9, enabled: ref enabled);
        // Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCode", 66, 147, 142, 10, "Secret Code", Core.Font.Arial, Alignment.Center)
        // Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCaptcha", 66, 183, 142, 10, "Captcha", Core.Font.Arial, Alignment.Center)

        // Textboxes
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argcallbackEnter = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtUsername", 67L, 55L, 142L, 19L, "", Core.Font.Arial, Alignment.Left, true, 255L, true, 5L, 3L, 0L, 0L, 0L, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, false, 0L, Constant.NameLength, ref argcallbackNorm7, ref argcallbackHover7, ref argcallbackMousedown10, ref argcallbackMousemove10, ref argcallbackDblclick10, ref argcallbackEnter);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argcallbackEnter1 = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtPassword", 67L, 90L, 142L, 19L, "", Core.Font.Arial, Alignment.Left, true, 255L, true, 5L, 3L, 0L, 0L, 0L, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, true, 0L, Constant.NameLength, ref argcallbackNorm8, ref argcallbackHover8, ref argcallbackMousedown11, ref argcallbackMousemove11, ref argcallbackDblclick11, ref argcallbackEnter1);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argcallbackEnter2 = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtRetypePassword", 67L, 127L, 142L, 19L, "", Core.Font.Arial, Alignment.Left, true, 255L, true, 5L, 3L, 0L, 0L, 0L, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, true, 0L, Constant.NameLength, ref argcallbackNorm9, ref argcallbackHover9, ref argcallbackMousedown12, ref argcallbackMousemove12, ref argcallbackDblclick12, ref argcallbackEnter2);
        // Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtCode", 67, 163, 142, 19, , Core.Font.Arial, , Alignment.Left, , , , , , UiDesign.TextWhite, UiDesign.TextWhite, UiDesign.TextWhite, False)
        // Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtCaptcha", 67, 235, 142, 19, , Core.Font.Arial, , Alignment.Left, , , , , , UiDesign.TextWhite, UiDesign.TextWhite, UiDesign.TextWhite, False)

        // Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picCaptcha", 67, 199, 156, 30, , , , , Tex_Captcha(GlobalCaptcha), Tex_Captcha(GlobalCaptcha), Tex_Captcha(GlobalCaptcha), UiDesign.BlackOval, UiDesign.BlackOval, UiDesign.BlackOval)

        Client.Gui.SetActiveControl(Client.Gui.GetWindowIndex("winRegister"), Client.Gui.GetControlIndex("winRegister", "txtUsername"));
    }

    public void UpdateWindow_NewChar()
    {
        // Control window
        Client.Gui.UpdateWindow("winNewChar", "Create Character", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 290L, 172L, 17L, false, 2L, 6L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnNewChar_Cancel);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 278L, 140L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.Parchment, (long)UiDesign.Parchment, (long)UiDesign.Parchment, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown1, ref argcallbackMousemove1, ref argcallbackDblclick1, ref argonDraw);

        // Name
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_1", 29L, 42L, 124L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm1, ref argcallbackHover1, ref argcallbackMousedown2, ref argcallbackMousemove2, ref argcallbackDblclick2, ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName", 29L, 39L, 124L, 10L, "Name", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm2, ref argcallbackHover2, ref argcallbackMousedown3, ref argcallbackMousemove3, ref argcallbackDblclick3, ref enabled);

        // Textbox
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackEnter = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtName", 29L, 55L, 124L, 19L, "", Core.Font.Arial, Alignment.Left, true, 255L, true, 5L, 3L, 0L, 0L, 0L, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, (long)UiDesign.TextWhite, false, 0L, Constant.NameLength, ref argcallbackNorm3, ref argcallbackHover3, ref argcallbackMousedown4, ref argcallbackMousemove4, ref argcallbackDblclick4, ref argcallbackEnter);

        // Sex
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_2", 29L, 85L, 124L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm4, ref argcallbackHover4, ref argcallbackMousedown5, ref argcallbackMousemove5, ref argcallbackDblclick5, ref argonDraw2);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblGender", 29L, 82L, 124L, 10L, "Gender", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm5, ref argcallbackHover5, ref argcallbackMousedown6, ref argcallbackMousemove6, ref argcallbackDblclick6, ref enabled);

        // Checkboxes
        var argcallbackMousedown7 = new Action(Client.Gui.chkNewChar_Male);
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkMale", 29L, 103L, 55L, 15L, 0L, "Male", Core.Font.Arial, Alignment.Center, true, 255L, (long)UiDesign.CheckboxNormal, 0L, false, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown7, ref argcallbackMousemove7, ref argcallbackDblclick7);
        var argcallbackMousedown8 = new Action(Client.Gui.chkNewChar_Female);
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkFemale", 90L, 103L, 62L, 15L, 0L, "Female", Core.Font.Arial, Alignment.Center, true, 255L, (long)UiDesign.CheckboxNormal, 0L, false, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown8, ref argcallbackMousemove8, ref argcallbackDblclick8);

        // Buttons
        var argcallbackMousedown9 = new Action(Client.Gui.btnNewChar_Accept);
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnAccept", 29L, 127L, 60L, 24L, "Accept", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown9, ref argcallbackMousemove9, ref argcallbackDblclick9, 0L, 0L, "", false);
        var argcallbackMousedown10 = new Action(Client.Gui.btnNewChar_Cancel);
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnCancel", 93L, 127L, 60L, 24L, "Cancel", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown10, ref argcallbackMousemove10, ref argcallbackDblclick10, 0L, 0L, "", false);

        // Sprite
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_3", 175L, 42L, 76L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm6, ref argcallbackHover6, ref argcallbackMousedown11, ref argcallbackMousemove11, ref argcallbackDblclick11, ref argonDraw3);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblSprite", 175L, 39L, 76L, 10L, "Sprite", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm7, ref argcallbackHover7, ref argcallbackMousedown12, ref argcallbackMousemove12, ref argcallbackDblclick12, ref enabled);

        // Scene
        var argonDraw4 = new Action(Client.Gui.NewChar_OnDraw);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picScene", 165L, 55L, 96L, 96L, true, false, 255L, true, 11L, 11L, 11L, 0L, 0L, 0L, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, ref argonDraw4);

        // Buttons
        var argcallbackMousedown13 = new Action(Client.Gui.btnNewChar_Left);
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnLeft", 163L, 40L, 10L, 13L, "", Core.Font.Georgia, 0L, 12L, 14L, 16L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown13, ref argcallbackMousemove13, ref argcallbackDblclick13, 0L, 0L, "", false);
        var argcallbackMousedown14 = new Action(Client.Gui.btnNewChar_Right);
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnRight", 252L, 40L, 10L, 13L, "", Core.Font.Georgia, 0L, 13L, 15L, 17L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown14, ref argcallbackMousemove14, ref argcallbackDblclick14, 0L, 0L, "", false);

        // Set the active control
        Client.Gui.SetActiveControl(Client.Gui.GetWindowIndex("winNewChar"), Client.Gui.GetControlIndex("winNewChar", "txtName"));
    }

    public void UpdateWindow_Chars()
    {
        // Control the window
        Client.Gui.UpdateWindow("winChars", "Characters", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 364L, 229L, 62L, false, 3L, 5L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnCharacters_Close);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackMousedown, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Parchment
        Action argcallbackHover = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 352L, 197L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.Parchment, (long)UiDesign.Parchment, (long)UiDesign.Parchment, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown1, ref argcallbackMousemove1, ref argcallbackDblclick1, ref argonDraw);

        // Names
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        bool enabled = false;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_1", 22L, 40L, 98L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm1, ref argcallbackHover1, ref argcallbackMousedown2, ref argcallbackMousemove2, ref argcallbackDblclick2, ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCharName_1", 22L, 37L, 98L, 10L, "Blank Slot", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm2, ref argcallbackHover2, ref argcallbackMousedown3, ref argcallbackMousemove3, ref argcallbackDblclick3, ref enabled);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_2", 132L, 40L, 98L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm3, ref argcallbackHover3, ref argcallbackMousedown4, ref argcallbackMousemove4, ref argcallbackDblclick4, ref argonDraw2);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCharName_2", 132L, 37L, 98L, 10L, "Blank Slot", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm4, ref argcallbackHover4, ref argcallbackMousedown5, ref argcallbackMousemove5, ref argcallbackDblclick5, ref enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow_3", 242L, 40L, 98L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm5, ref argcallbackHover5, ref argcallbackMousedown6, ref argcallbackMousemove6, ref argcallbackDblclick6, ref argonDraw3);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCharName_3", 242L, 37L, 98L, 10L, "Blank Slot", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm6, ref argcallbackHover6, ref argcallbackMousedown7, ref argcallbackMousemove7, ref argcallbackDblclick7, ref enabled);

        // Scenery Boxes
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw4 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picScene_1", 23L, 55L, 96L, 96L, true, false, 255L, true, 11L, 11L, 11L, 0L, 0L, 0L, "", ref argcallbackNorm7, ref argcallbackHover7, ref argcallbackMousedown8, ref argcallbackMousemove8, ref argcallbackDblclick8, ref argonDraw4);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Action argonDraw5 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picScene_2", 133L, 55L, 96L, 96L, true, false, 255L, true, 11L, 11L, 11L, 0L, 0L, 0L, "", ref argcallbackNorm8, ref argcallbackHover8, ref argcallbackMousedown9, ref argcallbackMousemove9, ref argcallbackDblclick9, ref argonDraw5);
        var argonDraw6 = new Action(Client.Gui.Chars_OnDraw);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picScene_3", 243L, 55L, 96L, 96L, true, false, 255L, true, 11L, 11L, 11L, 0L, 0L, 0L, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown1, ref argcallbackMousemove1, ref argcallbackDblclick1, ref argonDraw6);

        // Control Buttons
        var argcallbackMousedown10 = new Action(Client.Gui.btnAcceptChar_1);
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnSelectChar_1", 22L, 155L, 98L, 24L, "Select", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown10, ref argcallbackMousemove10, ref argcallbackDblclick10, 0L, 0L, "", false);
        var argcallbackMousedown11 = new Action(Client.Gui.btnCreateChar_1);
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnCreateChar_1", 22L, 155L, 98L, 24L, "Create", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown11, ref argcallbackMousemove11, ref argcallbackDblclick11, 0L, 0L, "", false);
        var argcallbackMousedown12 = new Action(Client.Gui.btnDelChar_1);
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDelChar_1", 22L, 183L, 98L, 24L, "Delete", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown12, ref argcallbackMousemove12, ref argcallbackDblclick12, 0L, 0L, "", false);
        var argcallbackMousedown13 = new Action(Client.Gui.btnAcceptChar_2);
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnSelectChar_2", 132L, 155L, 98L, 24L, "Select", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown13, ref argcallbackMousemove13, ref argcallbackDblclick13, 0L, 0L, "", false);
        var argcallbackMousedown14 = new Action(Client.Gui.btnCreateChar_2);
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnCreateChar_2", 132L, 155L, 98L, 24L, "Create", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown14, ref argcallbackMousemove14, ref argcallbackDblclick14, 0L, 0L, "", false);
        var argcallbackMousedown15 = new Action(Client.Gui.btnDelChar_2);
        Action argcallbackMousemove15 = null;
        Action argcallbackDblclick15 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDelChar_2", 132L, 183L, 98L, 24L, "Delete", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown15, ref argcallbackMousemove15, ref argcallbackDblclick15, 0L, 0L, "", false);
        var argcallbackMousedown16 = new Action(Client.Gui.btnAcceptChar_3);
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnSelectChar_3", 242L, 155L, 98L, 24L, "Select", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown16, ref argcallbackMousemove16, ref argcallbackDblclick16, 0L, 0L, "", false);
        var argcallbackMousedown17 = new Action(Client.Gui.btnCreateChar_3);
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnCreateChar_3", 242L, 155L, 98L, 24L, "Create", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown17, ref argcallbackMousemove17, ref argcallbackDblclick17, 0L, 0L, "", false);
        var argcallbackMousedown18 = new Action(Client.Gui.btnDelChar_3);
        Action argcallbackMousemove18 = null;
        Action argcallbackDblclick18 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDelChar_3", 242L, 183L, 98L, 24L, "Delete", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown18, ref argcallbackMousemove18, ref argcallbackDblclick18, 0L, 0L, "", false);
    }

    public void UpdateWindow_Jobs()
    {
        // Control window
        Client.Gui.UpdateWindow("winJobs", "Select Job", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 364L, 229L, 17L, false, 2L, 6L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.    Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnJobs_Close);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Parchment
        var argonDraw = new Action(Client.Gui.Jobs_DrawFace);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 352L, 197L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.Parchment, (long)UiDesign.Parchment, (long)UiDesign.Parchment, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, ref argonDraw);

        // Job Name
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow", 183L, 42L, 98L, 9L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, (long)UiDesign.BlackOval, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown1, ref argcallbackMousemove1, ref argcallbackDblclick1, ref argonDraw1);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblJobName", 183L, 39L, 98L, 10L, "Warrior", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, true, 255L, false, false, ref argcallbackNorm1, ref argcallbackHover1, ref argcallbackMousedown2, ref argcallbackMousemove2, ref argcallbackDblclick2, ref enabled);

        // Select Buttons
        var argcallbackMousedown3 = new Action(Client.Gui.btnJobs_Left);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnLeft", 170L, 40L, 10L, 13L, "", Core.Font.Georgia, 0L, 12L, 14L, 16L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown3, ref argcallbackMousemove3, ref argcallbackDblclick3, 0L, 0L, "", false);

        var argcallbackMousedown4 = new Action(Client.Gui.btnJobs_Right);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnRight", 282L, 40L, 10L, 13L, "", Core.Font.Georgia, 0L, 13L, 15L, 17L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown4, ref argcallbackMousemove4, ref argcallbackDblclick4, 0L, 0L, "", false);

        // Accept Button
        var argcallbackMousedown5 = new Action(Client.Gui.btnJobs_Accept);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnAccept", 183L, 185L, 98L, 22L, "Accept", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown5, ref argcallbackMousemove5, ref argcallbackDblclick5, 0L, 0L, "", false);

        // Text background
        Action argcallbackHover2 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBackground", 127L, 55L, 210L, 124L, true, false, 255L, true, 0L, 0L, 0L, (long)UiDesign.TextBlack, (long)UiDesign.TextBlack, (long)UiDesign.TextBlack, "", ref argcallbackNorm, ref argcallbackHover2, ref argcallbackMousedown6, ref argcallbackMousemove6, ref argcallbackDblclick6, ref argonDraw2);

        // Overlay
        var argonDraw3 = new Action(Client.Gui.Jobs_DrawText);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picOverlay", 6L, 26L, 0L, 0L, true, false, 255L, true, 0L, 0L, 0L, 0L, 0L, 0L, "", ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, ref argonDraw3);
    }

    public void UpdateWindow_Dialogue()
    {
        // Control dialogue window
        Client.Gui.UpdateWindow("winDialogue", "Warning", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 348L, 145L, 38L, false, 3L, 5L, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, (long)UiDesign.WindowNormal, canDrag: false);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnDialogue_Close);
        Action argcallbackNorm = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 335L, 113L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);

        // Header
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow", 103L, 44L, 144L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblHeader", 103L, 40L, 144L, 10L, "Header", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, enabled: ref enabled);

        // Input
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackEnter = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtInput", 93L, 75L, 162L, 18L, font: Core.Font.Arial, align: Alignment.Center, xOffset: 5L, yOffset: 2L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, callbackEnter: ref argcallbackEnter);

        // Labels
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBody_1", 15L, 60L, 314L, 10L, "Invalid username or password.", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5, enabled: ref enabled);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBody_2", 15L, 75L, 314L, 10L, "Please try again!", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6, enabled: ref enabled);

        // Buttons
        var argcallbackMousedown7 = new Action(Client.Gui.Dialogue_Yes);
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnYes", 104L, 98L, 68L, 24L, "Yes", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown7, ref argcallbackDblclick7, ref argcallbackMousemove7, 0L, 0L, "", false);
        var argcallbackMousedown8 = new Action(Client.Gui.Dialogue_No);
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnNo", 180L, 98L, 68L, 24L, "No", Core.Font.Arial, 0L, 0L, 0L, 0L, false, 255L, (long)UiDesign.Red, (long)UiDesign.RedHover, (long)UiDesign.RedClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown8, ref argcallbackMousemove8, ref argcallbackDblclick8, 0L, 0L, "", false);
        var argcallbackMousedown9 = new Action(Client.Gui.Dialogue_Okay);
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnOkay", 140L, 98L, 68L, 24L, "Okay", Core.Font.Arial, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown9, ref argcallbackMousemove9, ref argcallbackDblclick9, 0L, 0L, "", false);

        // Set active control
        Client.Gui.SetActiveControl(Client.Gui.Windows.Count, Client.Gui.GetControlIndex("winDialogue", "txtInput"));
    }

    public void UpdateWindow_Party()
    {
        // Control window
        Client.Gui.UpdateWindow("winParty", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 4L, 78L, 252L, 158L, 0L, false, designNorm: (long)UiDesign.WindowParty, designHover: (long)UiDesign.WindowParty, designMousedown: (long)UiDesign.WindowParty, canDrag: false);

        // Name labels
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName1", 60L, 20L, 173L, 10L, "Richard - Level 10", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, enabled: ref enabled);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName2", 60L, 60L, 173L, 10L, "Anna - Level 18", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, enabled: ref enabled);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName3", 60L, 100L, 173L, 10L, "Doleo - Level 25", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, enabled: ref enabled);

        // Empty Bars - HP
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_HP1", 58L, 34L, 173L, 9L, imageNorm: 62L, imageHover: 62L, imageMousedown: 62L, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, onDraw: ref argonDraw);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_HP2", 58L, 74L, 173L, 9L, imageNorm: 62L, imageHover: 62L, imageMousedown: 62L, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw1);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_HP3", 58L, 114L, 173L, 9L, imageNorm: 62L, imageHover: 62L, imageMousedown: 62L, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5, onDraw: ref argonDraw2);

        // Empty Bars - SP
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_SP1", 58L, 44L, 173L, 9L, imageNorm: 63L, imageHover: 63L, imageMousedown: 63L, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6, onDraw: ref argonDraw3);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw4 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_SP2", 58L, 84L, 173L, 9L, imageNorm: 63L, imageHover: 63L, imageMousedown: 63L, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7, onDraw: ref argonDraw4);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw5 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEmptyBar_SP3", 58L, 124L, 173L, 9L, imageNorm: 63L, imageHover: 63L, imageMousedown: 63L, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, onDraw: ref argonDraw5);

        // Filled bars - HP
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Action argonDraw6 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_HP1", 58L, 34L, 173L, 9L, imageNorm: 64L, imageHover: 64L, imageMousedown: 64L, callbackNorm: ref argcallbackNorm9, callbackHover: ref argcallbackHover9, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, onDraw: ref argonDraw6);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Action argonDraw7 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_HP2", 58L, 74L, 173L, 9L, imageNorm: 64L, imageHover: 64L, imageMousedown: 64L, callbackNorm: ref argcallbackNorm10, callbackHover: ref argcallbackHover10, callbackMousedown: ref argcallbackMousedown10, callbackMousemove: ref argcallbackMousemove10, callbackDblclick: ref argcallbackDblclick10, onDraw: ref argonDraw7);
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Action argonDraw8 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_HP3", 58L, 114L, 173L, 9L, imageNorm: 64L, imageHover: 64L, imageMousedown: 64L, callbackNorm: ref argcallbackNorm11, callbackHover: ref argcallbackHover11, callbackMousedown: ref argcallbackMousedown11, callbackMousemove: ref argcallbackMousemove11, callbackDblclick: ref argcallbackDblclick11, onDraw: ref argonDraw8);

        // Filled bars - SP
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Action argonDraw9 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_SP1", 58L, 44L, 173L, 9L, imageNorm: 65L, imageHover: 65L, imageMousedown: 65L, callbackNorm: ref argcallbackNorm12, callbackHover: ref argcallbackHover12, callbackMousedown: ref argcallbackMousedown12, callbackMousemove: ref argcallbackMousemove12, callbackDblclick: ref argcallbackDblclick12, onDraw: ref argonDraw9);
        Action argcallbackNorm13 = null;
        Action argcallbackHover13 = null;
        Action argcallbackMousedown13 = null;
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Action argonDraw10 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_SP2", 58L, 84L, 173L, 9L, imageNorm: 65L, imageHover: 65L, imageMousedown: 65L, callbackNorm: ref argcallbackNorm13, callbackHover: ref argcallbackHover13, callbackMousedown: ref argcallbackMousedown13, callbackMousemove: ref argcallbackMousemove13, callbackDblclick: ref argcallbackDblclick13, onDraw: ref argonDraw10);
        Action argcallbackNorm14 = null;
        Action argcallbackHover14 = null;
        Action argcallbackMousedown14 = null;
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Action argonDraw11 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar_SP3", 58L, 124L, 173L, 9L, imageNorm: 65L, imageHover: 65L, imageMousedown: 65L, callbackNorm: ref argcallbackNorm14, callbackHover: ref argcallbackHover14, callbackMousedown: ref argcallbackMousedown14, callbackMousemove: ref argcallbackMousemove14, callbackDblclick: ref argcallbackDblclick14, onDraw: ref argonDraw11);

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
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picChar1", 20L, 20L, 32L, 32L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, texturePath: Core.Path.Characters, callbackNorm: ref argcallbackNorm15, callbackHover: ref argcallbackHover15, callbackMousedown: ref argcallbackMousedown15, callbackMousemove: ref argcallbackMousemove15, callbackDblclick: ref argcallbackDblclick15, onDraw: ref argonDraw12);
        Action argcallbackNorm16 = null;
        Action argcallbackHover16 = null;
        Action argcallbackMousedown16 = null;
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Action argonDraw13 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picChar2", 20L, 60L, 32L, 32L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, texturePath: Core.Path.Characters, callbackNorm: ref argcallbackNorm16, callbackHover: ref argcallbackHover16, callbackMousedown: ref argcallbackMousedown16, callbackMousemove: ref argcallbackMousemove16, callbackDblclick: ref argcallbackDblclick16, onDraw: ref argonDraw13);
        Action argcallbackNorm17 = null;
        Action argcallbackHover17 = null;
        Action argcallbackMousedown17 = null;
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Action argonDraw14 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picChar3", 20L, 100L, 32L, 32L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, texturePath: Core.Path.Characters, callbackNorm: ref argcallbackNorm17, callbackHover: ref argcallbackHover17, callbackMousedown: ref argcallbackMousedown17, callbackMousemove: ref argcallbackMousemove17, callbackDblclick: ref argcallbackDblclick17, onDraw: ref argonDraw14);
    }

    public void UpdateWindow_Trade()
    {
        // Control window
        Client.Gui.UpdateWindow("winTrade", "Trading with [Name]", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 412L, 386L, 112L, false, 2L, 5L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, onDraw: new Action(Client.Gui.DrawTrade));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Close Button
        var argcallbackMousedown = new Action(Client.Gui.btnTrade_Close);
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 36L, 36L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown, callbackHover: ref argcallbackHover, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 10L, 312L, 392L, 66L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);

        // Labels
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        bool enabled = false;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow", 36L, 30L, 142L, 9L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw1);
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblYourTrade", 36L, 27L, 142L, 9L, "Robin's Offer", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, enabled: ref enabled);
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow", 36 + 200, 30L, 142L, 9L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw2);
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblTheirTrade", 36 + 200, 27L, 142L, 9L, "Richard's Offer", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5, enabled: ref enabled);

        // Buttons
        var argcallbackMousedown6 = new Action(Client.Gui.btnTrade_Accept);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnAccept", 134L, 340L, 68L, 24L, "Accept", Core.Font.Georgia, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown6, callbackHover: ref argcallbackHover, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        var argcallbackMousedown7 = new Action(Client.Gui.btnTrade_Close);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDecline", 210L, 340L, 68L, 24L, "Decline", Core.Font.Georgia, designNorm: (long)UiDesign.Red, designHover: (long)UiDesign.RedHover, designMousedown: (long)UiDesign.RedClick, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown7, callbackHover: ref argcallbackHover, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Labels
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStatus", 114L, 322L, 184L, 10L, "", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, enabled: ref enabled);

        // Amounts
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBlank", 25L, 330L, 100L, 10L, "Total Value", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, enabled: ref enabled);
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBlank", 285L, 330L, 100L, 10L, "Total Value", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown10, callbackMousemove: ref argcallbackMousemove10, callbackDblclick: ref argcallbackDblclick10, enabled: ref enabled);
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblYourValue", 25L, 344L, 100L, 10L, "52,812g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown11, callbackMousemove: ref argcallbackMousemove11, callbackDblclick: ref argcallbackDblclick11, enabled: ref enabled);
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblTheirValue", 285L, 344L, 100L, 10L, "12,531g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown12, callbackMousemove: ref argcallbackMousemove12, callbackDblclick: ref argcallbackDblclick12, enabled: ref enabled);

        // Item Containers
        var argcallbackMousedown13 = new Action(Client.Gui.TradeMouseMove_Your);
        var argcallbackMousemove13 = new Action(Client.Gui.TradeMouseMove_Your);
        var argcallbackDblclick13 = new Action(Client.Gui.TradeDoubleClick_Your);
        var argonDraw3 = new Action(Client.Gui.DrawYourTrade);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picYour", 14L, 46L, 184L, 260L, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown13, callbackHover: ref argcallbackHover, callbackMousemove: ref argcallbackMousemove13, callbackDblclick: ref argcallbackDblclick13, onDraw: ref argonDraw3);
        var argcallbackMousedown14 = new Action(Client.Gui.TradeMouseMove_Their);
        var argcallbackMousemove14 = new Action(Client.Gui.TradeMouseMove_Their);
        var argcallbackDblclick14 = new Action(Client.Gui.TradeMouseMove_Their);
        var argonDraw4 = new Action(Client.Gui.DrawTheirTrade);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picTheir", 214L, 46L, 184L, 260L, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown14, callbackHover: ref argcallbackHover, callbackMousemove: ref argcallbackMousemove14, callbackDblclick: ref argcallbackDblclick14, onDraw: ref argonDraw4);
    }

    public void UpdateWindow_EscMenu()
    {
        // Control window
        Client.Gui.UpdateWindow("winEscMenu", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 210L, 156L, 0L, false, designNorm: (long)UiDesign.WindowNoBar, designHover: (long)UiDesign.WindowNoBar, designMousedown: (long)UiDesign.WindowNoBar, canDrag: false, clickThrough: false);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 6L, 198L, 144L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, onDraw: ref argonDraw);

        // Buttons
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = new Action(Client.Gui.btnEscMenu_Return);
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnReturn", 16L, 16L, 178L, 28L, "Return to Game", Core.Font.Georgia, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1);

        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = new Action(Client.Gui.btnEscMenu_Options);
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnOptions", 16L, 48L, 178L, 28L, "Options", Core.Font.Georgia, designNorm: (long)UiDesign.Orange, designHover: (long)UiDesign.OrangeHover, designMousedown: (long)UiDesign.OrangeClick, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2);

        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = new Action(Client.Gui.btnEscMenu_MainMenu);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnMainMenu", 16L, 80L, 178L, 28L, "Back to Main Menu", Core.Font.Georgia, designNorm: (long)UiDesign.Blue, designHover: (long)UiDesign.BlueHover, designMousedown: (long)UiDesign.BlueClick, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3);

        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = new Action(Client.Gui.btnEscMenu_Exit);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnExit", 16L, 112L, 178L, 28L, "Exit the Game", Core.Font.Georgia, designNorm: (long)UiDesign.Red, designHover: (long)UiDesign.RedHover, designMousedown: (long)UiDesign.RedClick, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4);
    }

    public void UpdateWindow_Bars()
    {
        // Control window
        Client.Gui.UpdateWindow("winBars", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 10L, 10L, 239L, 77L, 0L, false, designNorm: (long)UiDesign.WindowNoBar, designHover: (long)UiDesign.WindowNoBar, designMousedown: (long)UiDesign.WindowNoBar, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 6L, 227L, 65L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, onDraw: ref argonDraw);

        // Blank Bars
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picHP_Blank", 15L, 15L, 209L, 13L, imageNorm: 24L, imageHover: 24L, imageMousedown: 24L, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picSP_Blank", 15L, 32L, 209L, 13L, imageNorm: 25L, imageHover: 25L, imageMousedown: 25L, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picEXP_Blank", 15L, 49L, 209L, 13L, imageNorm: 26L, imageHover: 26L, imageMousedown: 26L, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, onDraw: ref argonDraw3);

        // Bars
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        var argonDraw4 = new Action(Client.Gui.Bars_OnDraw);
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlank", 0L, 0L, 0L, 0L, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw5 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picHealth", 16L, 10L, 44L, 14L, imageNorm: 21L, imageHover: 21L, imageMousedown: 21L, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5, onDraw: ref argonDraw5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw6 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picSpirit", 16L, 28L, 44L, 14L, imageNorm: 22L, imageHover: 22L, imageMousedown: 22L, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6, onDraw: ref argonDraw6);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw7 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picExperience", 16L, 45L, 74L, 14L, imageNorm: 23L, imageHover: 23L, imageMousedown: 23L, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7, onDraw: ref argonDraw7);

        // Labels
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblHP", 15L, 14L, 209L, 10L, "999/999", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, enabled: ref enabled);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblMP", 15L, 30L, 209L, 10L, "999/999", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm9, callbackHover: ref argcallbackHover9, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, enabled: ref enabled);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblEXP", 15L, 48L, 209L, 10L, "999/999", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm10, callbackHover: ref argcallbackHover10, callbackMousedown: ref argcallbackMousedown10, callbackMousemove: ref argcallbackMousemove10, callbackDblclick: ref argcallbackDblclick10, enabled: ref enabled);
    }

    public void UpdateWindow_Chat()
    {
        // Control window
        Client.Gui.UpdateWindow("winChat", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 8L, Client.GameState.ResolutionHeight - 178, 352L, 152L, 0L, false, canDrag: false);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Channel boxes
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Game);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkGame", 10L, 2L, 49L, 23L, 0L, "Game", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Map);
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkMap", 60L, 2L, 49L, 23L, 0L, "Map", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Global);
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkGlobal", 110L, 2L, 49L, 23L, 0L, "Global", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Party);
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkParty", 160L, 2L, 49L, 23L, 0L, "Party", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Guild);
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkClient.Guild", 210L, 2L, 49L, 23L, 0L, "Client.Guild", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackMousedown = new Action(Client.Gui.CheckboxChat_Player);
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkPlayer", 260L, 2L, 49L, 23L, 0L, "Player", Core.Font.Arial, theDesign: (long)UiDesign.CheckboxChat, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Blank picturebox
        var argonDraw = new Action(Client.Gui.Chat_OnDraw);
        Action argcallbackNormPic = null;
        Action argcallbackHoverPic = null;
        Action argcallbackMousedownPic = null;
        Action argcallbackMousemovePic = null;
        Action argcallbackDblclickPic = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picNull", 0L, 0L, 0L, 0L, onDraw: ref argonDraw, callbackNorm: ref argcallbackNormPic, callbackHover: ref argcallbackHoverPic, callbackMousedown: ref argcallbackMousedownPic, callbackMousemove: ref argcallbackMousemovePic, callbackDblclick: ref argcallbackDblclickPic);

        // Chat button
        argcallbackNorm = new Action(Client.Gui.btnSay_Click);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnChat", 296L, (long)(124 + 16), 48L, 20L, "Say", Core.Font.Arial, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Chat Textbox
        Action argcallbackEnter = null;
        Client.Gui.UpdateTextbox(Client.Gui.Windows.Count, "txtChat", 12L, 127 + 16, 352L, 25L, font: Core.Font.Georgia, visible: false, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, callbackEnter: ref argcallbackEnter);

        // Buttons
        argcallbackNorm = new Action(Client.Gui.btnChat_Up);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnUp", 328L, 28L, 10L, 13L, imageNorm: 4L, imageHover: 52L, imageMousedown: 4L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
        argcallbackNorm = new Action(Client.Gui.btnChat_Down);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDown", 327L, 122L, 10L, 13L, imageNorm: 5L, imageHover: 53L, imageMousedown: 5L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Custom Handlers for mouse up
        Client.Gui.Windows[Client.Gui.Windows.Count].Controls[Client.Gui.GetControlIndex("winChat", "btnUp")].CallBack[(int)ControlState.MouseUp] = new Action(Client.Gui.btnChat_Up_MouseUp);
        Client.Gui.Windows[Client.Gui.Windows.Count].Controls[Client.Gui.GetControlIndex("winChat", "btnDown")].CallBack[(int)ControlState.MouseUp] = new Action(Client.Gui.btnChat_Down_MouseUp);

        // Set the active control
        Client.Gui.SetActiveControl(Client.Gui.GetWindowIndex("winChat"), Client.Gui.GetControlIndex("winChat", "txtChat"));

        // sort out the tabs
        {
            var withBlock = Client.Gui.Windows[Client.Gui.GetWindowIndex("winChat")];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkGame")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Game];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkMap")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Map];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkGlobal")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Broadcast];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkParty")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Party];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkkGuild")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Guild];
            withBlock.Controls[Client.Gui.GetControlIndex("winChat", "chkPlayer")].Value = SettingsManager.Instance.ChannelState[(int)ChatChannel.Private];
        }
    }

    public void UpdateWindow_ChatSmall()
    {
        // Control window
        Client.Gui.UpdateWindow("winChatSmall", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 8L, 0L, 0L, 0L, 0L, false, onDraw: new Action(Client.Gui.ChatSmall_OnDraw), canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Chat Label
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblMsg", 12L, 140L, 286L, 25L, "Press 'Enter' to open chat", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, enabled: ref enabled);
    }

    public void UpdateWindow_Hotbar()
    {
        // Control window
        Client.Gui.UpdateWindow("winHotbar", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 432L, 10L, 418L, 36L, 0L, false, callbackMousemove: new Action(Client.Gui.Hotbar_MouseMove), callbackMousedown: new Action(Client.Gui.Hotbar_MouseDown), callbackDblclick: new Action(Client.Gui.Hotbar_DoubleClick), onDraw: new Action(Client.Gui.DrawHotbar), canDrag: false, zChange: Conversions.ToByte(false));
    }

    public void UpdateWindow_Menu()
    {
        // Control window
        Client.Gui.UpdateWindow("winMenu", "", Core.Font.Georgia, Client.Gui.ZOrderWin, Client.GameState.ResolutionWidth - 229, Client.GameState.ResolutionHeight - 31, 229L, 30L, 0L, false, isActive: false, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Wood part
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWood", 0L, 5L, 228L, 20L, designNorm: (long)UiDesign.Wood, designHover: (long)UiDesign.Wood, designMousedown: (long)UiDesign.Wood, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, onDraw: ref argonDraw);
        // Buttons
        var argcallbackMousedown1 = new Action(Client.Gui.btnMenu_Char);
        Action callbackNorm = null;
        Action callbackHover = null;
        Action callbackMousemove = null;
        Action callbackDblclick = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnChar", 8L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 108L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-2, tooltip: "Character (C)");
        var argcallbackMousedown2 = new Action(Client.Gui.btnMenu_Inv);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnInv", 44L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 1L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-2, tooltip: "Inventory (I)");
        var argcallbackMousedown3 = new Action(Client.Gui.btnMenu_Skills);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnSkills", 82L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 109L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-2, tooltip: "Skills (K)");
        var argcallbackMousedown4 = new Action(Client.Gui.btnMenu_Map);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnMap", 119L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 106L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Grey, designHover: (long)UiDesign.Grey, designMousedown: (long)UiDesign.Grey, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-2);
        var argcallbackMousedown5 = new Action(Client.Gui.btnMenu_Guild);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClient.Guild", 155L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 107L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Grey, designHover: (long)UiDesign.Grey, designMousedown: (long)UiDesign.Grey, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-1);
        var argcallbackMousedown6 = new Action(Client.Gui.btnMenu_Quest);
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnQuest", 190L, 0L, 29L, 29L, text: "", font: Core.Font.Georgia, icon: 23L, imageNorm: 0L, imageHover: 0L, imageMousedown: 0L, visible: true, alpha: 255L, designNorm: (long)UiDesign.Grey, designHover: (long)UiDesign.Grey, designMousedown: (long)UiDesign.Grey, callbackNorm: ref callbackNorm, callbackHover: ref callbackHover, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref callbackMousemove, callbackDblclick: ref callbackDblclick, xOffset: (long)-1, yOffset: (long)-2);
    }

    public void UpdateWindow_Inventory()
    {
        // Control window
        Client.Gui.UpdateWindow("winInventory", "Inventory", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 202L, 319L, 1L, false, 2L, 7L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, callbackMousemove: new Action(Client.Gui.Inventory_MouseMove), callbackMousedown: new Action(Client.Gui.Inventory_MouseDown), callbackDblclick: new Action(Client.Gui.Inventory_DoubleClick), onDraw: new Action(Client.Gui.DrawInventory));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnMenu_Inv);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, "", Core.Font.Georgia, 0L, 8L, 9L, 10L, true, 255L, 0L, 0L, 0L, ref argcallbackNorm, ref argcallbackHover, ref argcallbackMousedown, ref argcallbackMousemove, ref argcallbackDblclick, 0L, 0L, "", false);

        // Gold amount
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlank", 8L, 293L, 186L, 18L, imageNorm: 67L, imageHover: 67L, imageMousedown: 67L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        //UpdateLabel(Client.Gui.Windows.Count, "lblGold", 42L, 296L, 100L, 10L, "g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.Yellow, callback_norm: ref argcallback_norm1, callback_hover: ref argcallback_hover1, callback_mousedown: ref argcallback_mousedown2, callback_mousemove: ref argcallback_mousemove2, callback_dblclick: ref argcallback_dblclick2, enabled: ref enabled);

        // Drop
        //Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnDrop", 155L, 294L, 38L, 16L, "Drop", Core.Font.Georgia, 0L, 0L, 0L, 0L, true, 255L, (long)UiDesign.Green, (long)UiDesign.GreenHover, (long)UiDesign.GreenClick, ref argcallback_norm, ref argcallback_hover, ref argcallback_mousedown, ref argcallback_mousemove, ref argcallback_dblclick, 5L, 3L, "", false, true);
    }

    public void UpdateWindow_Character()
    {
        // Control window
        Client.Gui.UpdateWindow("winCharacter", "Character", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 174L, 356L, 62L, false, 2L, 6L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, callbackMousemove: new Action(Client.Gui.Character_MouseMove), callbackMousedown: new Action(Client.Gui.Character_MouseMove), callbackDblclick: new Action(Client.Gui.Character_DoubleClick), onDraw: new Action(Client.Gui.DrawCharacter));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        Action argcallbackNorm = null;
        var argcallbackMousedown = new Action(Client.Gui.btnMenu_Char);
        Action argcallbackHover = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Parchment
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 26L, 162L, 287L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);

        // White boxes
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 34L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 54L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, onDraw: ref argonDraw2);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 74L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw3);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argonDraw4 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 94L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5, onDraw: ref argonDraw4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argonDraw5 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 114L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6, onDraw: ref argonDraw5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw6 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 134L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7, onDraw: ref argonDraw6);
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Action argonDraw7 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picWhiteBox", 13L, 154L, 148L, 19L, designNorm: (long)UiDesign.TextWhite, designHover: (long)UiDesign.TextWhite, designMousedown: (long)UiDesign.TextWhite, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, onDraw: ref argonDraw7);

        // Labels
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName", 18L, 36L, 147L, 10L, "Name", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, enabled: ref enabled);
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblJob", 18L, 56L, 147L, 10L, "Job", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm9, callbackHover: ref argcallbackHover9, callbackMousedown: ref argcallbackMousedown10, callbackMousemove: ref argcallbackMousemove10, callbackDblclick: ref argcallbackDblclick10, enabled: ref enabled);
        Action argcallbackNorm10 = null;
        Action argcallbackHover10 = null;
        Action argcallbackMousedown11 = null;
        Action argcallbackMousemove11 = null;
        Action argcallbackDblclick11 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLevel", 18L, 76L, 147L, 10L, "Level", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm10, callbackHover: ref argcallbackHover10, callbackMousedown: ref argcallbackMousedown11, callbackMousemove: ref argcallbackMousemove11, callbackDblclick: ref argcallbackDblclick11, enabled: ref enabled);
        Action argcallbackNorm11 = null;
        Action argcallbackHover11 = null;
        Action argcallbackMousedown12 = null;
        Action argcallbackMousemove12 = null;
        Action argcallbackDblclick12 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblClient.Guild", 18L, 96L, 147L, 10L, "Client.Guild", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm11, callbackHover: ref argcallbackHover11, callbackMousedown: ref argcallbackMousedown12, callbackMousemove: ref argcallbackMousemove12, callbackDblclick: ref argcallbackDblclick12, enabled: ref enabled);
        Action argcallbackNorm12 = null;
        Action argcallbackHover12 = null;
        Action argcallbackMousedown13 = null;
        Action argcallbackMousemove13 = null;
        Action argcallbackDblclick13 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblHealth", 18L, 116L, 147L, 10L, "Health", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm12, callbackHover: ref argcallbackHover12, callbackMousedown: ref argcallbackMousedown13, callbackMousemove: ref argcallbackMousemove13, callbackDblclick: ref argcallbackDblclick13, enabled: ref enabled);
        Action argcallbackNorm13 = null;
        Action argcallbackHover13 = null;
        Action argcallbackMousedown14 = null;
        Action argcallbackMousemove14 = null;
        Action argcallbackDblclick14 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblSpirit", 18L, 136L, 147L, 10L, "Spirit", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm13, callbackHover: ref argcallbackHover13, callbackMousedown: ref argcallbackMousedown14, callbackMousemove: ref argcallbackMousemove14, callbackDblclick: ref argcallbackDblclick14, enabled: ref enabled);
        Action argcallbackNorm14 = null;
        Action argcallbackHover14 = null;
        Action argcallbackMousedown15 = null;
        Action argcallbackMousemove15 = null;
        Action argcallbackDblclick15 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblExperience", 18L, 156L, 147L, 10L, "Experience", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, callbackNorm: ref argcallbackNorm14, callbackHover: ref argcallbackHover14, callbackMousedown: ref argcallbackMousedown15, callbackMousemove: ref argcallbackMousemove15, callbackDblclick: ref argcallbackDblclick15, enabled: ref enabled);
        Action argcallbackNorm15 = null;
        Action argcallbackHover15 = null;
        Action argcallbackMousedown16 = null;
        Action argcallbackMousemove16 = null;
        Action argcallbackDblclick16 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName2", 13L, 36L, 147L, 10L, "Name", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm15, callbackHover: ref argcallbackHover15, callbackMousedown: ref argcallbackMousedown16, callbackMousemove: ref argcallbackMousemove16, callbackDblclick: ref argcallbackDblclick16, enabled: ref enabled);
        Action argcallbackNorm16 = null;
        Action argcallbackHover16 = null;
        Action argcallbackMousedown17 = null;
        Action argcallbackMousemove17 = null;
        Action argcallbackDblclick17 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblJob2", 13L, 56L, 147L, 10L, "", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm16, callbackHover: ref argcallbackHover16, callbackMousedown: ref argcallbackMousedown17, callbackMousemove: ref argcallbackMousemove17, callbackDblclick: ref argcallbackDblclick17, enabled: ref enabled);
        Action argcallbackNorm17 = null;
        Action argcallbackHover17 = null;
        Action argcallbackMousedown18 = null;
        Action argcallbackMousemove18 = null;
        Action argcallbackDblclick18 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLevel2", 13L, 76L, 147L, 10L, "Level", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm17, callbackHover: ref argcallbackHover17, callbackMousedown: ref argcallbackMousedown18, callbackMousemove: ref argcallbackMousemove18, callbackDblclick: ref argcallbackDblclick18, enabled: ref enabled);
        Action argcallbackNorm18 = null;
        Action argcallbackHover18 = null;
        Action argcallbackMousedown19 = null;
        Action argcallbackMousemove19 = null;
        Action argcallbackDblclick19 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblClient.Guild2", 13L, 96L, 147L, 10L, "Client.Guild", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm18, callbackHover: ref argcallbackHover18, callbackMousedown: ref argcallbackMousedown19, callbackMousemove: ref argcallbackMousemove19, callbackDblclick: ref argcallbackDblclick19, enabled: ref enabled);
        Action argcallbackNorm19 = null;
        Action argcallbackHover19 = null;
        Action argcallbackMousedown20 = null;
        Action argcallbackMousemove20 = null;
        Action argcallbackDblclick20 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblHealth2", 13L, 116L, 147L, 10L, "Health", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm19, callbackHover: ref argcallbackHover19, callbackMousedown: ref argcallbackMousedown20, callbackMousemove: ref argcallbackMousemove20, callbackDblclick: ref argcallbackDblclick20, enabled: ref enabled);
        Action argcallbackNorm20 = null;
        Action argcallbackHover20 = null;
        Action argcallbackMousedown21 = null;
        Action argcallbackMousemove21 = null;
        Action argcallbackDblclick21 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblSpirit2", 13L, 136L, 147L, 10L, "Spirit", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm20, callbackHover: ref argcallbackHover20, callbackMousedown: ref argcallbackMousedown21, callbackMousemove: ref argcallbackMousemove21, callbackDblclick: ref argcallbackDblclick21, enabled: ref enabled);
        Action argcallbackNorm21 = null;
        Action argcallbackHover21 = null;
        Action argcallbackMousedown22 = null;
        Action argcallbackMousemove22 = null;
        Action argcallbackDblclick22 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblExperience2", 13L, 156L, 147L, 10L, "Experience", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm21, callbackHover: ref argcallbackHover21, callbackMousedown: ref argcallbackMousedown22, callbackMousemove: ref argcallbackMousemove22, callbackDblclick: ref argcallbackDblclick22, enabled: ref enabled);

        // Attributes
        Action argcallbackNorm22 = null;
        Action argcallbackHover22 = null;
        Action argcallbackMousedown23 = null;
        Action argcallbackMousemove23 = null;
        Action argcallbackDblclick23 = null;
        Action argonDraw8 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picShadow", 18L, 176L, 138L, 9L, designNorm: (long)UiDesign.BlackOval, designHover: (long)UiDesign.BlackOval, designMousedown: (long)UiDesign.BlackOval, callbackNorm: ref argcallbackNorm22, callbackHover: ref argcallbackHover22, callbackMousedown: ref argcallbackMousedown23, callbackMousemove: ref argcallbackMousemove23, callbackDblclick: ref argcallbackDblclick23, onDraw: ref argonDraw8);
        Action argcallbackNorm23 = null;
        Action argcallbackHover23 = null;
        Action argcallbackMousedown24 = null;
        Action argcallbackMousemove24 = null;
        Action argcallbackDblclick24 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 173L, 138L, 10L, "Attributes", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm23, callbackHover: ref argcallbackHover23, callbackMousedown: ref argcallbackMousedown24, callbackMousemove: ref argcallbackMousemove24, callbackDblclick: ref argcallbackDblclick24, enabled: ref enabled);

        // Black boxes
        Action argcallbackNorm24 = null;
        Action argcallbackHover24 = null;
        Action argcallbackMousedown25 = null;
        Action argcallbackMousemove25 = null;
        Action argcallbackDblclick25 = null;
        Action argonDraw9 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 186L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm24, callbackHover: ref argcallbackHover24, callbackMousedown: ref argcallbackMousedown25, callbackMousemove: ref argcallbackMousemove25, callbackDblclick: ref argcallbackDblclick25, onDraw: ref argonDraw9);
        Action argcallbackNorm25 = null;
        Action argcallbackHover25 = null;
        Action argcallbackMousedown26 = null;
        Action argcallbackMousemove26 = null;
        Action argcallbackDblclick26 = null;
        Action argonDraw10 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 206L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm25, callbackHover: ref argcallbackHover25, callbackMousedown: ref argcallbackMousedown26, callbackMousemove: ref argcallbackMousemove26, callbackDblclick: ref argcallbackDblclick26, onDraw: ref argonDraw10);
        Action argcallbackNorm26 = null;
        Action argcallbackHover26 = null;
        Action argcallbackMousedown27 = null;
        Action argcallbackMousemove27 = null;
        Action argcallbackDblclick27 = null;
        Action argonDraw11 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 226L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm26, callbackHover: ref argcallbackHover26, callbackMousedown: ref argcallbackMousedown27, callbackMousemove: ref argcallbackMousemove27, callbackDblclick: ref argcallbackDblclick27, onDraw: ref argonDraw11);
        Action argcallbackNorm27 = null;
        Action argcallbackHover27 = null;
        Action argcallbackMousedown28 = null;
        Action argcallbackMousemove28 = null;
        Action argcallbackDblclick28 = null;
        Action argonDraw12 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 246L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm27, callbackHover: ref argcallbackHover27, callbackMousedown: ref argcallbackMousedown28, callbackMousemove: ref argcallbackMousemove28, callbackDblclick: ref argcallbackDblclick28, onDraw: ref argonDraw12);
        Action argcallbackNorm28 = null;
        Action argcallbackHover28 = null;
        Action argcallbackMousedown29 = null;
        Action argcallbackMousemove29 = null;
        Action argcallbackDblclick29 = null;
        Action argonDraw13 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 266L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm28, callbackHover: ref argcallbackHover28, callbackMousedown: ref argcallbackMousedown29, callbackMousemove: ref argcallbackMousemove29, callbackDblclick: ref argcallbackDblclick29, onDraw: ref argonDraw13);
        Action argcallbackNorm29 = null;
        Action argcallbackHover29 = null;
        Action argcallbackMousedown30 = null;
        Action argcallbackMousemove30 = null;
        Action argcallbackDblclick30 = null;
        Action argonDraw14 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlackBox", 13L, 286L, 148L, 19L, designNorm: (long)UiDesign.TextBlack, designHover: (long)UiDesign.TextBlack, designMousedown: (long)UiDesign.TextBlack, callbackNorm: ref argcallbackNorm29, callbackHover: ref argcallbackHover29, callbackMousedown: ref argcallbackMousedown30, callbackMousemove: ref argcallbackMousemove30, callbackDblclick: ref argcallbackDblclick30, onDraw: ref argonDraw14);

        // Labels
        Action argcallbackNorm30 = null;
        Action argcallbackHover30 = null;
        Action argcallbackMousedown31 = null;
        Action argcallbackMousemove31 = null;
        Action argcallbackDblclick31 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 188L, 138L, 10L, "Strength", Core.Font.Arial, Microsoft.Xna.Framework.Color.Yellow, callbackNorm: ref argcallbackNorm30, callbackHover: ref argcallbackHover30, callbackMousedown: ref argcallbackMousedown31, callbackMousemove: ref argcallbackMousemove31, callbackDblclick: ref argcallbackDblclick3, enabled: ref enabled);
        Action argcallbackNorm31 = null;
        Action argcallbackHover31 = null;
        Action argcallbackMousedown32 = null;
        Action argcallbackMousemove32 = null;
        Action argcallbackDblclick32 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 208L, 138L, 10L, "Vitality", Core.Font.Arial, Microsoft.Xna.Framework.Color.Yellow, callbackNorm: ref argcallbackNorm31, callbackHover: ref argcallbackHover31, callbackMousedown: ref argcallbackMousedown32, callbackMousemove: ref argcallbackMousemove32, callbackDblclick: ref argcallbackDblclick32, enabled: ref enabled);
        Action argcallbackNorm32 = null;
        Action argcallbackHover32 = null;
        Action argcallbackMousedown33 = null;
        Action argcallbackMousemove33 = null;
        Action argcallbackDblclick33 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 228L, 138L, 10L, "Intelligence", Core.Font.Arial, Microsoft.Xna.Framework.Color.Yellow, callbackNorm: ref argcallbackNorm32, callbackHover: ref argcallbackHover32, callbackMousedown: ref argcallbackMousedown33, callbackMousemove: ref argcallbackMousemove33, callbackDblclick: ref argcallbackDblclick3, enabled: ref enabled);
        Action argcallbackNorm33 = null;
        Action argcallbackHover33 = null;
        Action argcallbackMousedown34 = null;
        Action argcallbackMousemove34 = null;
        Action argcallbackDblclick34 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 248L, 138L, 10L, "Luck", Core.Font.Arial, Microsoft.Xna.Framework.Color.Yellow, callbackNorm: ref argcallbackNorm33, callbackHover: ref argcallbackHover33, callbackMousedown: ref argcallbackMousedown34, callbackMousemove: ref argcallbackMousemove34, callbackDblclick: ref argcallbackDblclick34, enabled: ref enabled);
        Action argcallbackNorm34 = null;
        Action argcallbackHover34 = null;
        Action argcallbackMousedown35 = null;
        Action argcallbackMousemove35 = null;
        Action argcallbackDblclick35 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 268L, 138L, 10L, "Spirit", Core.Font.Arial, Microsoft.Xna.Framework.Color.Yellow, callbackNorm: ref argcallbackNorm34, callbackHover: ref argcallbackHover34, callbackMousedown: ref argcallbackMousedown35, callbackMousemove: ref argcallbackMousemove35, callbackDblclick: ref argcallbackDblclick35, enabled: ref enabled);
        Action argcallbackNorm35 = null;
        Action argcallbackHover35 = null;
        Action argcallbackMousedown36 = null;
        Action argcallbackMousemove36 = null;
        Action argcallbackDblclick36 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLabel", 18L, 288L, 138L, 10L, "Stat Points", Core.Font.Arial, Microsoft.Xna.Framework.Color.Green, callbackNorm: ref argcallbackNorm35, callbackHover: ref argcallbackHover35, callbackMousedown: ref argcallbackMousedown36, callbackMousemove: ref argcallbackMousemove36, callbackDblclick: ref argcallbackDblclick36, enabled: ref enabled);

        // Buttons
        var argcallbackMousedown37 = new Action(Client.Gui.Character_SpendPoint1);
        Action argcallbackMousemove37 = null;
        Action argcallbackDblclick37 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnStat_1", 144L, 188L, 15L, 15L, imageNorm: 48L, imageHover: 49L, imageMousedown: 50L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown37, callbackMousemove: ref argcallbackMousemove37, callbackDblclick: ref argcallbackDblclick37);
        var argcallbackMousedown38 = new Action(Client.Gui.Character_SpendPoint2);
        Action argcallbackMousemove38 = null;
        Action argcallbackDblclick38 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnStat_2", 144L, 208L, 15L, 15L, imageNorm: 48L, imageHover: 49L, imageMousedown: 50L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown38, callbackMousemove: ref argcallbackMousemove38, callbackDblclick: ref argcallbackDblclick3);
        var argcallbackMousedown39 = new Action(Client.Gui.Character_SpendPoint3);
        Action argcallbackMousemove39 = null;
        Action argcallbackDblclick39 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnStat_3", 144L, 228L, 15L, 15L, imageNorm: 48L, imageHover: 49L, imageMousedown: 50L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown39, callbackMousemove: ref argcallbackMousemove39, callbackDblclick: ref argcallbackDblclick39);
        var argcallbackMousedown40 = new Action(Client.Gui.Character_SpendPoint4);
        Action argcallbackMousemove40 = null;
        Action argcallbackDblclick40 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnStat_4", 144L, 248L, 15L, 15L, imageNorm: 48L, imageHover: 49L, imageMousedown: 50L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown40, callbackMousemove: ref argcallbackMousemove40, callbackDblclick: ref argcallbackDblclick4);
        var argcallbackMousedown41 = new Action(Client.Gui.Character_SpendPoint5);
        Action argcallbackMousemove41 = null;
        Action argcallbackDblclick41 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnStat_5", 144L, 268L, 15L, 15L, imageNorm: 48L, imageHover: 49L, imageMousedown: 50L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown41, callbackMousemove: ref argcallbackMousemove41, callbackDblclick: ref argcallbackDblclick41);

        // fake buttons
        Action argcallbackNorm36 = null;
        Action argcallbackHover36 = null;
        Action argcallbackMousedown42 = null;
        Action argcallbackMousemove42 = null;
        Action argcallbackDblclick42 = null;
        Action argonDraw15 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "btnGreyStat_1", 144L, 188L, 15L, 15L, imageNorm: 47L, imageHover: 47L, imageMousedown: 47L, callbackNorm: ref argcallbackNorm36, callbackHover: ref argcallbackHover36, callbackMousedown: ref argcallbackMousedown42, callbackMousemove: ref argcallbackMousemove42, callbackDblclick: ref argcallbackDblclick42, onDraw: ref argonDraw15);
        Action argcallbackNorm37 = null;
        Action argcallbackHover37 = null;
        Action argcallbackMousedown43 = null;
        Action argcallbackMousemove43 = null;
        Action argcallbackDblclick43 = null;
        Action argonDraw16 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "btnGreyStat_2", 144L, 208L, 15L, 15L, imageNorm: 47L, imageHover: 47L, imageMousedown: 47L, callbackNorm: ref argcallbackNorm37, callbackHover: ref argcallbackHover37, callbackMousedown: ref argcallbackMousedown43, callbackMousemove: ref argcallbackMousemove43, callbackDblclick: ref argcallbackDblclick43, onDraw: ref argonDraw16);
        Action argcallbackNorm38 = null;
        Action argcallbackHover38 = null;
        Action argcallbackMousedown44 = null;
        Action argcallbackMousemove44 = null;
        Action argcallbackDblclick44 = null;
        Action argonDraw17 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "btnGreyStat_3", 144L, 228L, 15L, 15L, imageNorm: 47L, imageHover: 47L, imageMousedown: 47L, callbackNorm: ref argcallbackNorm38, callbackHover: ref argcallbackHover38, callbackMousedown: ref argcallbackMousedown44, callbackMousemove: ref argcallbackMousemove44, callbackDblclick: ref argcallbackDblclick44, onDraw: ref argonDraw17);
        Action argcallbackNorm39 = null;
        Action argcallbackHover39 = null;
        Action argcallbackMousedown45 = null;
        Action argcallbackMousemove45 = null;
        Action argcallbackDblclick45 = null;
        Action argonDraw18 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "btnGreyStat_4", 144L, 248L, 15L, 15L, imageNorm: 47L, imageHover: 47L, imageMousedown: 47L, callbackNorm: ref argcallbackNorm39, callbackHover: ref argcallbackHover39, callbackMousedown: ref argcallbackMousedown45, callbackMousemove: ref argcallbackMousemove45, callbackDblclick: ref argcallbackDblclick45, onDraw: ref argonDraw18);
        Action argcallbackNorm40 = null;
        Action argcallbackHover40 = null;
        Action argcallbackMousedown46 = null;
        Action argcallbackMousemove46 = null;
        Action argcallbackDblclick46 = null;
        Action argonDraw19 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "btnGreyStat_5", 144L, 268L, 15L, 15L, imageNorm: 47L, imageHover: 47L, imageMousedown: 47L, callbackNorm: ref argcallbackNorm40, callbackHover: ref argcallbackHover40, callbackMousedown: ref argcallbackMousedown46, callbackMousemove: ref argcallbackMousemove46, callbackDblclick: ref argcallbackDblclick46, onDraw: ref argonDraw19);

        // Labels
        Action argcallbackNorm41 = null;
        Action argcallbackHover41 = null;
        Action argcallbackMousedown47 = null;
        Action argcallbackMousemove47 = null;
        Action argcallbackDblclick47 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStat_1", 42L, 188L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm41, callbackHover: ref argcallbackHover41, callbackMousedown: ref argcallbackMousedown47, callbackMousemove: ref argcallbackMousemove47, callbackDblclick: ref argcallbackDblclick47, enabled: ref enabled);
        Action argcallbackNorm42 = null;
        Action argcallbackHover42 = null;
        Action argcallbackMousedown48 = null;
        Action argcallbackMousemove48 = null;
        Action argcallbackDblclick48 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStat_2", 42L, 208L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm42, callbackHover: ref argcallbackHover42, callbackMousedown: ref argcallbackMousedown48, callbackMousemove: ref argcallbackMousemove48, callbackDblclick: ref argcallbackDblclick48, enabled: ref enabled);
        Action argcallbackNorm43 = null;
        Action argcallbackHover43 = null;
        Action argcallbackMousedown49 = null;
        Action argcallbackMousemove49 = null;
        Action argcallbackDblclick49 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStat_3", 42L, 228L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm43, callbackHover: ref argcallbackHover43, callbackMousedown: ref argcallbackMousedown49, callbackMousemove: ref argcallbackMousemove49, callbackDblclick: ref argcallbackDblclick49, enabled: ref enabled);
        Action argcallbackNorm44 = null;
        Action argcallbackHover44 = null;
        Action argcallbackMousedown50 = null;
        Action argcallbackMousemove50 = null;
        Action argcallbackDblclick50 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStat_4", 42L, 248L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm44, callbackHover: ref argcallbackHover44, callbackMousedown: ref argcallbackMousedown50, callbackMousemove: ref argcallbackMousemove50, callbackDblclick: ref argcallbackDblclick50, enabled: ref enabled);
        Action argcallbackNorm45 = null;
        Action argcallbackHover45 = null;
        Action argcallbackMousedown51 = null;
        Action argcallbackMousemove51 = null;
        Action argcallbackDblclick51 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblStat_5", 42L, 268L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm45, callbackHover: ref argcallbackHover45, callbackMousedown: ref argcallbackMousedown51, callbackMousemove: ref argcallbackMousemove51, callbackDblclick: ref argcallbackDblclick51, enabled: ref enabled);
        Action argcallbackNorm46 = null;
        Action argcallbackHover46 = null;
        Action argcallbackMousedown52 = null;
        Action argcallbackMousemove52 = null;
        Action argcallbackDblclick52 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblPoints", 57L, 288L, 100L, 15L, "255", Core.Font.Arial, Microsoft.Xna.Framework.Color.White, Alignment.Right, callbackNorm: ref argcallbackNorm46, callbackHover: ref argcallbackHover46, callbackMousedown: ref argcallbackMousedown52, callbackMousemove: ref argcallbackMousemove52, callbackDblclick: ref argcallbackDblclick5, enabled: ref enabled);
    }

    public void UpdateWindow_Description()
    {
        // Control window
        Client.Gui.UpdateWindow("winDescription", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 193L, 142L, 0L, false, designNorm: (long)UiDesign.WindowDescription, designHover: (long)UiDesign.WindowDescription, designMousedown: (long)UiDesign.WindowDescription, canDrag: false, clickThrough: true);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Name
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName", 8L, 12L, 177L, 10L, "Flame Sword", Core.Font.Arial, Microsoft.Xna.Framework.Color.Blue, Alignment.Center, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, enabled: ref enabled);

        // Sprite box
        var argonDraw = new Action(Client.Gui.Description_OnDraw);
        Action argcallbackNormPic = null;
        Action argcallbackHoverPic = null;
        Action argcallbackMousedownPic = null;
        Action argcallbackMousemovePic = null;
        Action argcallbackDblclickPic = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picSprite", 18L, 32L, 68L, 68L, designNorm: (long)UiDesign.DescriptionPicture, designHover: (long)UiDesign.DescriptionPicture, designMousedown: (long)UiDesign.DescriptionPicture, callbackNorm: ref argcallbackNormPic, callbackHover: ref argcallbackHoverPic, callbackMousedown: ref argcallbackMousedownPic, callbackMousemove: ref argcallbackMousemovePic, callbackDblclick: ref argcallbackDblclickPic, onDraw: ref argonDraw);

        // Sep
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picSep", 96L, 28L, 0L, 92L, imageNorm: 44L, imageHover: 44L, imageMousedown: 44L, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw1);

        // Requirements
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblJob", 5L, 102L, 92L, 10L, "Warrior", Core.Font.Georgia, Microsoft.Xna.Framework.Color.Green, Alignment.Center, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, enabled: ref enabled);
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblLevel", 5L, 114L, 92L, 10L, "Level 20", Core.Font.Georgia, Microsoft.Xna.Framework.Color.Red, Alignment.Center, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, enabled: ref enabled);

        // Bar
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBar", 19L, 114L, 66L, 12L, false, imageNorm: 45L, imageHover: 45L, imageMousedown: 45L, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4, onDraw: ref argonDraw2);
    }

    public void UpdateWindow_RightClick()
    {
        // Control window
        Client.Gui.UpdateWindow("winRightClickBG", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 800L, 600L, 0L, false, callbackMousedown: new Action(Client.Gui.RightClick_Close), canDrag: false);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);
    }

    public void UpdateWindow_PlayerMenu()
    {
        // Control window  
        Client.Gui.UpdateWindow("winPlayerMenu", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 110L, 106L, 0L, false, designNorm: (long)UiDesign.WindowDescription, designHover: (long)UiDesign.WindowDescription, designMousedown: (long)UiDesign.WindowDescription, callbackMousedown: new Action(Client.Gui.RightClick_Close), canDrag: false);

        // Centralize it  
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Name  
        var argcallbackMousedown = new Action(Client.Gui.RightClick_Close);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnName", 8L, 8L, 94L, 18L, "[Name]", Core.Font.Georgia, designNorm: (long)UiDesign.MenuHeader, designHover: (long)UiDesign.MenuHeader, designMousedown: (long)UiDesign.MenuHeader, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Options  
        var argcallbackMousedown1 = new Action(Client.Gui.PlayerMenu_Party);
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnParty", 8L, 26L, 94L, 18L, "Invite to Party", Core.Font.Georgia, designHover: (long)UiDesign.MenuOption, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1);

        var argcallbackMousedown2 = new Action(Client.Gui.PlayerMenu_Trade);
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnTrade", 8L, 44L, 94L, 18L, "Request Trade", Core.Font.Georgia, designHover: (long)UiDesign.MenuOption, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2);

        var argcallbackMousedown3 = new Action(Client.Gui.PlayerMenu_Guild);
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClient.Guild", 8L, 62L, 94L, 18L, "Invite to Client.Guild", Core.Font.Georgia, designNorm: (long)UiDesign.MenuOption, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3);

        var argcallbackMousedown4 = new Action(Client.Gui.PlayerMenu_Player);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnPM", 8L, 80L, 94L, 18L, "Private Message", Core.Font.Georgia, designHover: (long)UiDesign.MenuOption, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4);
    }

    public void UpdateWindow_DragBox()
    {
        // Control window
        Client.Gui.UpdateWindow("winDragBox", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 32L, 32L, 0L, false, onDraw: new Action(Client.Gui.DragBox_OnDraw));

        // Need to set up unique mouseup event
        Client.Gui.Windows[Client.Gui.Windows.Count].CallBack[(int)ControlState.MouseUp] = new Action(Client.Gui.DragBox_Check);
    }

    public void UpdateWindow_Options()
    {
        Client.Gui.UpdateWindow("winOptions", "", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 210L, 212L, 0L, Conversions.ToBoolean(0), designNorm: (long)UiDesign.WindowNoBar, designHover: (long)UiDesign.WindowNoBar, designMousedown: (long)UiDesign.WindowNoBar, isActive: false, clickThrough: false);

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Parchment
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Action argcallbackMousedown = null;
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argonDraw = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 6L, 198L, 200L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick, onDraw: ref argonDraw);

        // General
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Action argonDraw1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlank", 35L, 25L, 140L, 10L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw1);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBlank", 35L, 22L, 140L, 0L, "General Options", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, enabled: ref enabled);

        // Check boxes
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkMusic", 35L, 40L, 80L, text: "Music", font: Core.Font.Georgia, theDesign: (long)UiDesign.CheckboxNormal, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3);
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Action argcallbackMousedown4 = null;
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkSound", 115L, 40L, 80L, text: "Sound", font: Core.Font.Georgia, theDesign: (long)UiDesign.CheckboxNormal, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4);
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Action argcallbackMousedown5 = null;
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkAutotile", 35L, 60L, 80L, text: "Autotile", font: Core.Font.Georgia, theDesign: (long)UiDesign.CheckboxNormal, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5);
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Action argcallbackMousedown6 = null;
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "chkFullscreen", 115L, 60L, 80L, text: "Fullscreen", font: Core.Font.Georgia, theDesign: (long)UiDesign.CheckboxNormal, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6);

        // Resolution
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown7 = null;
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picBlank", 35L, 85L, 140L, 10L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7, onDraw: ref argonDraw2);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblBlank", 35L, 92L, 140L, 10L, "Select Resolution", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Center, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, enabled: ref enabled);

        // combobox
        Client.Gui.UpdateComboBox(Client.Gui.Windows.Count, "cmbRes", 30L, 100L, 150L, 18L, (long)UiDesign.ComboBoxNormal);

        // Button
        Action argcallbackMousedown9 = Client.Gui.btnOptions_Confirm;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnConfirm", 65L, 168L, 80L, 22L, "Confirm", Core.Font.Georgia, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackHover: ref argcallbackHover, callbackNorm: ref argcallbackNorm, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9);

        // Populate the options screen
        Client.GameLogic.SetOptionsScreen();
    }

    public void UpdateWindow_Combobox()
    {
        // background window
        Client.Gui.UpdateWindow("winComboMenuBG", "ComboMenuBG", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 800L, 600L, 0L, false, callbackDblclick: new Action(Client.Gui.CloseComboMenu), zChange: Conversions.ToByte(false), isActive: false);

        // window
        Client.Gui.UpdateWindow("winComboMenu", "ComboMenu", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 100L, 100L, 0L, false, designNorm: (long)UiDesign.ComboMenuNormal, isActive: false, clickThrough: false);

        // centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);
    }

    public void UpdateWindow_Skills()
    {
        // Control window
        Client.Gui.UpdateWindow("winSkills", "Skills", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 202L, 297L, 109L, false, 2L, 7L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, callbackMousemove: new Action(Client.Gui.Skills_MouseMove), callbackMousedown: new Action(Client.Gui.Skills_MouseDown), callbackDblclick: new Action(Client.Gui.Skills_DoubleClick), onDraw: new Action(Client.Gui.DrawSkills));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnMenu_Skills);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 16L, 16L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
    }

    public void UpdateWindow_Bank()
    {
        Client.Gui.UpdateWindow("winBank", "Bank", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 390L, 373L, 0L, false, 2L, 5L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, callbackMousemove: new Action(Client.Gui.Bank_MouseMove), callbackMousedown: new Action(Client.Gui.Bank_MouseDown), callbackDblclick: new Action(Client.Gui.Bank_DoubleClick), onDraw: new Action(Client.Gui.DrawBank));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Set the index for spawning controls
        Client.Gui.ZOrderCon = 0L;
        var argcallbackMousedown = new Action(Client.Gui.btnMenu_Bank);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 5L, 36L, 36L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);
    }

    public void UpdateWindow_Shop()
    {
        // Control window
        Client.Gui.UpdateWindow("winShop", "Shop", Core.Font.Georgia, Client.Gui.ZOrderWin, 0L, 0L, 278L, 293L, 17L, false, 2L, 5L, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, (long)UiDesign.WindowEmpty, callbackMousemove: new Action(Client.Gui.Shop_MouseMove), callbackMousedown: new Action(Client.Gui.Shop_MouseDown), onDraw: new Action(Client.Gui.DrawShopBackground));

        // Centralize it
        Client.Gui.CentralizeWindow(Client.Gui.Windows.Count);

        // Close button
        var argcallbackMousedown = new Action(Client.Gui.btnShop_Close);
        Action argcallbackMousemove = null;
        Action argcallbackDblclick = null;
        Action argcallbackNorm = null;
        Action argcallbackHover = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnClose", Client.Gui.Windows[Client.Gui.Windows.Count].Width - 19L, 6L, 36L, 36L, imageNorm: 8L, imageHover: 9L, imageMousedown: 10L, callbackNorm: ref argcallbackNorm, callbackHover: ref argcallbackHover, callbackMousedown: ref argcallbackMousedown, callbackMousemove: ref argcallbackMousemove, callbackDblclick: ref argcallbackDblclick);

        // Parchment
        var argonDraw = new Action(Client.Gui.DrawShop);
        Action argcallbackNorm1 = null;
        Action argcallbackHover1 = null;
        Action argcallbackMousedown1 = null;
        Action argcallbackMousemove1 = null;
        Action argcallbackDblclick1 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picParchment", 6L, 215L, 266L, 50L, designNorm: (long)UiDesign.Parchment, designHover: (long)UiDesign.Parchment, designMousedown: (long)UiDesign.Parchment, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown1, callbackMousemove: ref argcallbackMousemove1, callbackDblclick: ref argcallbackDblclick1, onDraw: ref argonDraw);

        // Picture Box
        Action argcallbackMousedown2 = null;
        Action argcallbackMousemove2 = null;
        Action argcallbackDblclick2 = null;
        Action argonDraw2 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picItemBG", 13L, 222L, 36L, 36L, imageNorm: 30L, imageHover: 30L, imageMousedown: 30L, callbackNorm: ref argcallbackNorm1, callbackHover: ref argcallbackHover1, callbackMousedown: ref argcallbackMousedown2, callbackMousemove: ref argcallbackMousemove2, callbackDblclick: ref argcallbackDblclick2, onDraw: ref argonDraw2);
        Action argcallbackNorm2 = null;
        Action argcallbackHover2 = null;
        Action argcallbackMousedown3 = null;
        Action argcallbackMousemove3 = null;
        Action argcallbackDblclick3 = null;
        Action argonDraw3 = null;
        Client.Gui.UpdatePictureBox(Client.Gui.Windows.Count, "picItem", 15L, 224L, 32L, 32L, callbackNorm: ref argcallbackNorm2, callbackHover: ref argcallbackHover2, callbackMousedown: ref argcallbackMousedown3, callbackMousemove: ref argcallbackMousemove3, callbackDblclick: ref argcallbackDblclick3, onDraw: ref argonDraw3);

        // Buttons
        var argcallbackMousedown4 = new Action(Client.Gui.BtnShopBuy);
        Action argcallbackMousemove4 = null;
        Action argcallbackDblclick4 = null;
        Action argcallbackNorm3 = null;
        Action argcallbackHover3 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnBuy", 190L, 228L, 70L, 24L, "Buy", Core.Font.Arial, designNorm: (long)UiDesign.Green, designHover: (long)UiDesign.GreenHover, designMousedown: (long)UiDesign.GreenClick, callbackNorm: ref argcallbackNorm3, callbackHover: ref argcallbackHover3, callbackMousedown: ref argcallbackMousedown4, callbackMousemove: ref argcallbackMousemove4, callbackDblclick: ref argcallbackDblclick4);
        var argcallbackMousedown5 = new Action(Client.Gui.BtnShopSell);
        Action argcallbackMousemove5 = null;
        Action argcallbackDblclick5 = null;
        Action argcallbackNorm4 = null;
        Action argcallbackHover4 = null;
        Client.Gui.UpdateButton(Client.Gui.Windows.Count, "btnSell", 190L, 228L, 70L, 24L, "Sell", Core.Font.Arial, visible: false, designNorm: (long)UiDesign.Red, designHover: (long)UiDesign.RedHover, designMousedown: (long)UiDesign.RedClick, callbackNorm: ref argcallbackNorm4, callbackHover: ref argcallbackHover4, callbackMousedown: ref argcallbackMousedown5, callbackMousemove: ref argcallbackMousemove5, callbackDblclick: ref argcallbackDblclick5);

        // Buying/Selling
        var argcallbackMousedown6 = new Action(Client.Gui.ChkShopBuying);
        Action argcallbackMousemove6 = null;
        Action argcallbackDblclick6 = null;
        Action argcallbackNorm5 = null;
        Action argcallbackHover5 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "CheckboxBuying", 173L, 265L, 49L, 20L, 0L, theDesign: (long)UiDesign.CheckboxBuying, callbackNorm: ref argcallbackNorm5, callbackHover: ref argcallbackHover5, callbackMousedown: ref argcallbackMousedown6, callbackMousemove: ref argcallbackMousemove6, callbackDblclick: ref argcallbackDblclick6);
        var argcallbackMousedown7 = new Action(Client.Gui.ChkShopSelling);
        Action argcallbackMousemove7 = null;
        Action argcallbackDblclick7 = null;
        Action argcallbackNorm6 = null;
        Action argcallbackHover6 = null;
        Client.Gui.UpdateCheckBox(Client.Gui.Windows.Count, "CheckboxSelling", 222L, 265L, 49L, 20L, 0L, theDesign: (long)UiDesign.CheckboxSelling, callbackNorm: ref argcallbackNorm6, callbackHover: ref argcallbackHover6, callbackMousedown: ref argcallbackMousedown7, callbackMousemove: ref argcallbackMousemove7, callbackDblclick: ref argcallbackDblclick7);

        // Labels
        Action argcallbackNorm7 = null;
        Action argcallbackHover7 = null;
        Action argcallbackMousedown8 = null;
        Action argcallbackMousemove8 = null;
        Action argcallbackDblclick8 = null;
        bool enabled = false;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblName", 56L, 226L, 300L, 10L, "Test Item", Core.Font.Arial, Microsoft.Xna.Framework.Color.Black, Alignment.Left, callbackNorm: ref argcallbackNorm7, callbackHover: ref argcallbackHover7, callbackMousedown: ref argcallbackMousedown8, callbackMousemove: ref argcallbackMousemove8, callbackDblclick: ref argcallbackDblclick8, enabled: ref enabled);
        Action argcallbackNorm8 = null;
        Action argcallbackHover8 = null;
        Action argcallbackMousedown9 = null;
        Action argcallbackMousemove9 = null;
        Action argcallbackDblclick9 = null;
        Client.Gui.UpdateLabel(Client.Gui.Windows.Count, "lblCost", 56L, 240L, 300L, 10L, "1000g", Core.Font.Arial, Microsoft.Xna.Framework.Color.Black, Alignment.Left, callbackNorm: ref argcallbackNorm8, callbackHover: ref argcallbackHover8, callbackMousedown: ref argcallbackMousedown9, callbackMousemove: ref argcallbackMousemove9, callbackDblclick: ref argcallbackDblclick9, enabled: ref enabled);

        // Gold
        Action argcallbackNorm9 = null;
        Action argcallbackHover9 = null;
        Action argcallbackMousedown10 = null;
        Action argcallbackMousemove10 = null;
        Action argcallbackDblclick10 = null;
        //UpdateLabel(Client.Gui.Windows.Count, "lblGold", 44L, 269L, 300L, 10L, "g", Core.Font.Georgia, Microsoft.Xna.Framework.Color.White, Alignment.Left, callback_norm: ref argcallback_norm9, callback_hover: ref argcallback_hover9, callback_mousedown: ref argcallback_mousedown10, callback_mousemove: ref argcallback_mousemove10, callback_dblclick: ref argcallback_dblclick10, enabled: ref enabled);
    }
}
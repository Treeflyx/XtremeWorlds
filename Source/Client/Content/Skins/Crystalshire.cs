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

        var userName = SettingsManager.Instance.SaveUsername ? SettingsManager.Instance.Username : string.Empty;

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = Client.General.DestroyGame;
        window.GetChild("txtUsername").Text = userName;
        window.GetChild("chkSaveUsername").Value = SettingsManager.Instance.SaveUsername ? 1 : 0;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinLogin.OnLogin;
        window.GetChild("btnExit").CallBack[(int) ControlState.MouseDown] = WinLogin.OnExit;
        window.GetChild("btnRegister").CallBack[(int) ControlState.MouseDown] = WinLogin.OnRegister;

        Gui.SetActiveControl(window, userName.Length == 0 ? "txtUsername" : "txtPassword");
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
        var window = WindowLoader.FromLayout("winDialogue");

        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinDialogue.OnClose;
        window.GetChild("btnYes").CallBack[(int) ControlState.MouseDown] = WinDialogue.OnYes;
        window.GetChild("btnNo").CallBack[(int) ControlState.MouseDown] = WinDialogue.OnNo;
        window.GetChild("btnOkay").CallBack[(int) ControlState.MouseDown] = WinDialogue.OnOkay;
        Gui.SetActiveControl(window, "txtInput");
    }

    public void UpdateWindow_Party()
    {
        var window = WindowLoader.FromLayout("winParty");
    }

    public void UpdateWindow_Trade()
    {
        var window = WindowLoader.FromLayout("winTrade");
        window.OnDraw = WinTrade.OnDraw;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinTrade.OnClose;
        window.GetChild("btnAccept").CallBack[(int) ControlState.MouseDown] = WinTrade.OnAccept;
        window.GetChild("btnDecline").CallBack[(int) ControlState.MouseDown] = WinTrade.OnClose;

        // Wire interactive picture boxes for trade regions
        window.GetChild("picYour").CallBack[(int) ControlState.MouseDown] = WinTrade.OnYourTradeMouseMove;
        window.GetChild("picYour").CallBack[(int) ControlState.MouseMove] = WinTrade.OnYourTradeMouseMove;
        window.GetChild("picYour").CallBack[(int) ControlState.DoubleClick] = WinTrade.OnYourTradeClick;

        window.GetChild("picTheir").CallBack[(int) ControlState.MouseDown] = WinTrade.OnTheirTradeMouseMove;
        window.GetChild("picTheir").CallBack[(int) ControlState.MouseMove] = WinTrade.OnTheirTradeMouseMove;
        window.GetChild("picTheir").CallBack[(int) ControlState.DoubleClick] = WinTrade.OnTheirTradeMouseMove;
    }

    public void UpdateWindow_EscMenu()
    {
        var window = WindowLoader.FromLayout("winEscMenu");
        window.GetChild("btnReturn").CallBack[(int) ControlState.MouseDown] = WinEscMenu.OnClose;
        window.GetChild("btnOptions").CallBack[(int) ControlState.MouseDown] = WinEscMenu.OnOptionsClick;
        window.GetChild("btnMainMenu").CallBack[(int) ControlState.MouseDown] = WinEscMenu.OnMainMenuClick;
        window.GetChild("btnExit").CallBack[(int) ControlState.MouseDown] = WinEscMenu.OnExitClick;
    }

    public void UpdateWindow_Bars()
    {
        var window = WindowLoader.FromLayout("winBars");
        window.GetChild("picOverlay").OnDraw = WinBars.OnDraw;
    }

    public void UpdateWindow_Chat()
    {
        var window = WindowLoader.FromLayout("winChat");
        // Picture box draw
        window.GetChild("picNull").OnDraw = WinChat.OnDraw;
        // Buttons
        window.GetChild("btnChat").CallBack[(int) ControlState.Normal] = WinChat.OnSayClick;
        window.GetChild("btnUp").CallBack[(int) ControlState.MouseDown] = WinChat.OnUpButtonMouseDown;
        window.GetChild("btnDown").CallBack[(int) ControlState.MouseDown] = WinChat.OnDownButtonMouseDown;
        window.GetChild("btnUp").CallBack[(int) ControlState.MouseUp] = WinChat.OnUpButtonMouseUp;
        window.GetChild("btnDown").CallBack[(int) ControlState.MouseUp] = WinChat.OnDownButtonMouseUp;
        // Checkboxes
        window.GetChild("chkGame").CallBack[(int) ControlState.MouseDown] = WinChat.OnGameChannelClicked;
        window.GetChild("chkMap").CallBack[(int) ControlState.MouseDown] = WinChat.OnMapChannelClicked;
        window.GetChild("chkGlobal").CallBack[(int) ControlState.MouseDown] = WinChat.OnBroadcastChannelClicked;
        window.GetChild("chkParty").CallBack[(int) ControlState.MouseDown] = WinChat.OnPartyChannelClicked;
        window.GetChild("chkGuild").CallBack[(int) ControlState.MouseDown] = WinChat.OnGuildChannelClicked;
        window.GetChild("chkPlayer").CallBack[(int) ControlState.MouseDown] = WinChat.OnPrivateChannelClicked;

        Gui.SetActiveControl(window, "txtChat");

        // Initialize checkbox states
        window.GetChild("chkGame").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Game];
        window.GetChild("chkMap").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Map];
        window.GetChild("chkGlobal").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Broadcast];
        window.GetChild("chkParty").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Party];
        window.GetChild("chkGuild").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Guild];
        window.GetChild("chkPlayer").Value = SettingsManager.Instance.ChannelState[(int) ChatChannel.Private];
    }

    public void UpdateWindow_ChatSmall()
    {
        var window = WindowLoader.FromLayout("winChatSmall");
        window.OnDraw = WinChat.OnDrawSmall;
    }

    public void UpdateWindow_Hotbar()
    {
        var window = WindowLoader.FromLayout("winHotbar");
        window.OnDraw = WinHotBar.OnDraw;
        // Wire window-level interactions
        window.CallBack[(int) ControlState.MouseMove] = WinHotBar.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinHotBar.OnMouseDown;
        window.CallBack[(int) ControlState.DoubleClick] = WinHotBar.OnDoubleClick;
    }

    public void UpdateWindow_Menu()
    {
        var window = WindowLoader.FromLayout("winMenu");
        window.GetChild("btnChar").CallBack[(int) ControlState.MouseDown] = WinMenu.OnCharacterClick;
        window.GetChild("btnInv").CallBack[(int) ControlState.MouseDown] = WinMenu.OnInventoryClick;
        window.GetChild("btnSkills").CallBack[(int) ControlState.MouseDown] = WinMenu.OnSkillsClick;
        window.GetChild("btnMap").CallBack[(int) ControlState.MouseDown] = WinMenu.OnMapClick;
        window.GetChild("btnGuild").CallBack[(int) ControlState.MouseDown] = WinMenu.OnGuildClick;
        window.GetChild("btnQuest").CallBack[(int) ControlState.MouseDown] = WinMenu.OnQuestClick;
    }

    public void UpdateWindow_Inventory()
    {
        var window = WindowLoader.FromLayout("winInventory");
        window.OnDraw = WinInventory.OnDraw;
        window.CallBack[(int) ControlState.MouseMove] = WinInventory.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinInventory.OnMouseDown;
        window.CallBack[(int) ControlState.DoubleClick] = WinInventory.OnDoubleClick;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinMenu.OnInventoryClick;
    }

    public void UpdateWindow_Character()
    {
        var window = WindowLoader.FromLayout("winCharacter");
        window.OnDraw = WinCharacter.OnDrawCharacter;
        window.CallBack[(int) ControlState.MouseMove] = WinCharacter.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinCharacter.OnMouseMove;
        window.CallBack[(int) ControlState.DoubleClick] = WinCharacter.OnDoubleClick;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinMenu.OnCharacterClick;
        // Stat buttons may exist in layout; wire them if present
        TryBind(window, "btnStat_1", ControlState.MouseDown, WinCharacter.OnSpendPoint1);
        TryBind(window, "btnStat_2", ControlState.MouseDown, WinCharacter.OnSpendPoint2);
        TryBind(window, "btnStat_3", ControlState.MouseDown, WinCharacter.OnSpendPoint3);
        TryBind(window, "btnStat_4", ControlState.MouseDown, WinCharacter.OnSpendPoint4);
        TryBind(window, "btnStat_5", ControlState.MouseDown, WinCharacter.OnSpendPoint5);
    }

    public void UpdateWindow_Description()
    {
        var window = WindowLoader.FromLayout("winDescription");
        window.GetChild("picSprite").OnDraw = WinDescription.OnDraw;
    }

    public void UpdateWindow_RightClick()
    {
        var window = WindowLoader.FromLayout("winRightClickBG");
        window.CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnClose;
    }

    public void UpdateWindow_PlayerMenu()
    {
        var window = WindowLoader.FromLayout("winPlayerMenu");
        window.CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnClose;
        window.GetChild("btnName").CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnClose;
        window.GetChild("btnParty").CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnPartyInvite;
        window.GetChild("btnTrade").CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnTradeRequest;
        window.GetChild("btnGuild").CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnGuildInvite;
        window.GetChild("btnPM").CallBack[(int) ControlState.MouseDown] = WinPlayerMenu.OnPrivateMessage;
    }

    public void UpdateWindow_DragBox()
    {
        var window = WindowLoader.FromLayout("winDragBox");
        window.OnDraw = WinDragBox.OnDraw;
        window.CallBack[(int) ControlState.MouseUp] = WinDragBox.DragBox_Check;
    }

    public void UpdateWindow_Options()
    {
        var window = WindowLoader.FromLayout("winOptions");
        window.GetChild("btnConfirm").CallBack[(int) ControlState.MouseDown] = WinOptions.OnConfirm;
        Client.GameLogic.SetOptionsScreen();
    }

    public void UpdateWindow_Combobox()
    {
        var bg = WindowLoader.FromLayout("winComboMenuBG");
        bg.CallBack[(int) ControlState.DoubleClick] = WinComboMenu.Close;

        WindowLoader.FromLayout("winComboMenu");
    }

    public void UpdateWindow_Skills()
    {
        var window = WindowLoader.FromLayout("winSkills");
        window.OnDraw = WinSkills.OnDraw;
        window.CallBack[(int) ControlState.MouseMove] = WinSkills.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinSkills.OnMouseDown;
        window.CallBack[(int) ControlState.DoubleClick] = WinSkills.OnDoubleClick;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinMenu.OnSkillsClick;
        }

    public void UpdateWindow_Bank()
    {
        var window = WindowLoader.FromLayout("winBank");
        window.OnDraw = WinBank.OnDraw;
        window.CallBack[(int) ControlState.MouseMove] = WinBank.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinBank.OnMouseDown;
        window.CallBack[(int) ControlState.DoubleClick] = WinBank.OnDoubleClick;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinBank.OnClose;
    }

    public void UpdateWindow_Shop()
    {
        var window = WindowLoader.FromLayout("winShop");
        window.OnDraw = WinShop.OnDrawBackground;
        window.CallBack[(int) ControlState.MouseMove] = WinShop.OnMouseMove;
        window.CallBack[(int) ControlState.MouseDown] = WinShop.OnMouseDown;
        window.GetChild("btnClose").CallBack[(int) ControlState.MouseDown] = WinShop.OnClose;
        window.GetChild("picParchment").OnDraw = WinShop.OnDraw;
        window.GetChild("btnBuy").CallBack[(int) ControlState.MouseDown] = WinShop.OnBuy;
        window.GetChild("btnSell").CallBack[(int) ControlState.MouseDown] = WinShop.OnSell;
        window.GetChild("CheckboxBuying").CallBack[(int) ControlState.MouseDown] = WinShop.OnBuyingChecked;
        window.GetChild("CheckboxSelling").CallBack[(int) ControlState.MouseDown] = WinShop.OnSellingChecked;
    }

    // Helper to bind callbacks if a control exists in layout
    private static void TryBind(Window window, string controlName, ControlState state, Action action)
    {
        try
        {
            window.GetChild(controlName).CallBack[(int) state] = action;
        }
        catch
        {
            // control not present in this skin; ignore
        }
    }
}
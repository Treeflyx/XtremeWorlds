using Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using Core.Globals;
using Eto.Forms;
using Eto.Drawing;
using static Core.Globals.Type;
using EventCommand = Core.Globals.EventCommand;
using Type = Core.Globals.Type;

namespace Client
{

    public partial class Editor_Event : Form
    {
        // Singleton access for legacy usage
        private static Editor_Event? _instance;
        public static Editor_Event Instance => _instance ??= new Editor_Event();
        // Expose a safe closer for external callers to avoid double-dispose
        public static void CloseIfOpen()
        {
            var inst = _instance;
            if (inst == null) return;
            try
            {
                // Prefer Close to allow Eto to unbind cleanly, then dispose
                inst.Close();
                inst.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // already disposed; ignore
            }
            finally
            {
                _instance = null;
            }
        }

        private int tmpGraphicIndex;
        private byte tmpGraphicType;

        public ComboBox cmbSwitch = new ComboBox();
        public ComboBox cmbVariable = new ComboBox();
        public ComboBox cmbChangeItemIndex = new ComboBox();
        public ComboBox cmbSetSelfSwitch = new ComboBox();
        public ComboBox cmbSetSelfSwitchTo = new ComboBox();
        public TextBox txtGoToLabel = new TextBox();
        public NumericStepper nudChangeItemsAmount = new NumericStepper();
        public CheckBox optChangeItemAdd = new CheckBox { Text = "Add" };
        public CheckBox optChangeItemSet = new CheckBox { Text = "Set" };
        public CheckBox optChangeItemRemove = new CheckBox { Text = "Remove" };
        public RadioButton optChangeSkillsAdd = new RadioButton { Text = "Add Skill" };
        public RadioButton optChangeSkillsRemove = new RadioButton { Text = "Remove Skill" };
        public RadioButton optCondition0 = new RadioButton { Text = "Player Var" };
        public RadioButton optCondition1 = new RadioButton { Text = "Player Switch" };
        public RadioButton optCondition2 = new RadioButton { Text = "Has Item" };
        public RadioButton optCondition3 = new RadioButton { Text = "Self Switch" };
        public RadioButton optCondition4 = new RadioButton { Text = "Class Is" };
        public RadioButton optCondition5 = new RadioButton { Text = "Learnt Skill" };
        public RadioButton optCondition6 = new RadioButton { Text = "Level" };
        public RadioButton optCondition8 = new RadioButton { Text = "Gender" };
        public RadioButton optCondition9 = new RadioButton { Text = "Time" };
        // Additional controls referenced later
        public RadioButton optChangeSexMale = new RadioButton { Text = "Male" };
        public RadioButton optChangeSexFemale = new RadioButton { Text = "Female" };
        public ComboBox cmbSetPK = new ComboBox();
        public NumericStepper nudGiveExp = new NumericStepper();
        public NumericStepper nudWPX = new NumericStepper();
        public NumericStepper nudWPY = new NumericStepper();
        public ComboBox cmbWarpPlayerDir = new ComboBox();
        public ComboBox cmbMoveWait = new ComboBox();
        // Add Text scope options
        public RadioButton optAddText_Map = new RadioButton { Text = "Map" };
        public RadioButton optAddText_Global = new RadioButton { Text = "Global" };
        // Animation play / targeting controls
        public ComboBox cmbPlayAnimEvent = new ComboBox();
        public ComboBox cmbAnimTargetType = new ComboBox();
        public NumericStepper nudPlayAnimTileX = new NumericStepper();
        public NumericStepper nudPlayAnimTileY = new NumericStepper();
        public Label lblPlayAnimX = new Label();
        public Label lblPlayAnimY = new Label();
        // Fog / weather / tint controls referenced later
        public NumericStepper nudFogData1 = new NumericStepper();
        public NumericStepper nudFogData2 = new NumericStepper();
        public ComboBox CmbWeather = new ComboBox();
        public NumericStepper nudWeatherIntensity = new NumericStepper();
        public NumericStepper nudMapTintData0 = new NumericStepper();
        public NumericStepper nudMapTintData1 = new NumericStepper();
        public NumericStepper nudMapTintData2 = new NumericStepper();
        public NumericStepper nudMapTintData3 = new NumericStepper();
        // Additional missing controls referenced in logic or Event.cs
        public NumericStepper nudWaitAmount = new NumericStepper();
        public ComboBox cmbSetAccess = new ComboBox();
        public Panel fraOpenShop = new Panel();
        public NumericStepper nudPicOffsetX = new NumericStepper();
        public NumericStepper nudPicOffsetY = new NumericStepper();
        public ComboBox cmbMoveType = new ComboBox();
        public ComboBox cmbMoveSpeed = new ComboBox();
        public ComboBox cmbMoveFreq = new ComboBox();
        public ComboBox cmbPositioning = new ComboBox();
        public ComboBox cmbTrigger = new ComboBox();
        public Panel pnlVariableSwitches = new Panel();
        public ListBox lstSwitches = new ListBox();
        public ListBox lstVariables = new ListBox();
        public Panel FraRenaming = new Panel();
        public Panel fraLabeling = new Panel();
        public TextBox txtRename = new TextBox();
        public Label lblEditing = new Label();
     // Core top-level controls (declare only what existing logic references)
        public ComboBox cmbCondition_PlayerVarIndex = new ComboBox();
        public ComboBox cmbCondition_PlayerVarCompare = new ComboBox();
        public ComboBox cmbPlayerSwitchSet = new ComboBox();
        public ComboBox cmbCondition_PlayerSwitch = new ComboBox();
        public ComboBox cmbCondtion_PlayerSwitchCondition = new ComboBox();
        public ComboBox cmbCondition_HasItem = new ComboBox();
        public ComboBox cmbCondition_JobIs = new ComboBox();
        public ComboBox cmbCondition_LearntSkill = new ComboBox();
        public ComboBox cmbCondition_LevelCompare = new ComboBox();
        public ComboBox cmbCondition_SelfSwitch = new ComboBox();
        public ComboBox cmbCondition_SelfSwitchCondition = new ComboBox();
        public ComboBox cmbCondition_Gender = new ComboBox();
        public ComboBox cmbCondition_Time = new ComboBox();
        public ComboBox cmbSwitchSet = new ComboBox();
        public Label txtLabelName = new Label();
        public NumericStepper nudChangeLevel = new NumericStepper();
        public ComboBox cmbChangeSkills = new ComboBox();
        public ComboBox cmbChangeJob = new ComboBox();
        public NumericStepper nudChangeSprite = new NumericStepper();
        public ComboBox cmbPlayAnim = new ComboBox();
        public ComboBox cmbPlayBGM = new ComboBox();
        public ComboBox cmbPlaySound = new ComboBox();
        public ComboBox cmbOpenShop = new ComboBox();
        public ComboBox cmbSpawnNpc = new ComboBox();
        public NumericStepper nudFogData0 = new NumericStepper();
        public NumericStepper nudWPMap = new NumericStepper();
        public Panel fraDialogue = new Panel();
        public Panel fraMoveRoute = new Panel();
        public ComboBox cmbEvent = new ComboBox();
        public TabControl tabPages = new TabControl();
        public ComboBox cmbHasItem = new ComboBox();
        public ComboBox cmbPlayerVar = new ComboBox();
        public ComboBox cmbPlayerSwitch = new ComboBox();
        public ComboBox cmbSelfSwitch = new ComboBox();
        public Button btnDeletePage = new Button { Text = "Delete Page" };
        public Button btnPastePage = new Button { Text = "Paste Page" };
        public NumericStepper nudShowPicture = new NumericStepper();
        public ComboBox cmbPicLoc = new ComboBox();
        public TextBox txtName = new TextBox { Width = 200 };
        public ImageView picGraphicSel = new ImageView();
        public ImageView picGraphic = new ImageView();
        public Panel fraGraphic = new Panel();
        public ComboBox cmbGraphic = new ComboBox();
        public NumericStepper nudGraphic = new NumericStepper();

        // Additional controls referenced in logic (declare as needed)
        public ListBox lstCommands = new ListBox();
        public Button btnAddCommand = new Button { Text = "Add" };
        public Button btnEditCommand = new Button { Text = "Edit" };
        public Button btnDeleteComand = new Button { Text = "Delete" };
        public Button btnClearCommand = new Button { Text = "Clear" };
        public TreeGridView tvCommands = new TreeGridView();

        // Numerous frame panels placeholders (keep as Panel)
        public Panel fraShowText = new Panel();
        public Panel fraShowChoices = new Panel();
        public Panel fraAddText = new Panel();
        public Panel fraShowChatBubble = new Panel();
        public Panel fraCommands = new Panel();
        public Panel fraPlayerVariable = new Panel();
        public Panel fraPlayerSwitch = new Panel();
        public Panel fraSetSelfSwitch = new Panel();
        public Panel fraConditionalBranch = new Panel();
        public Panel fraCreateLabel = new Panel();
        public Panel fraGoToLabel = new Panel();
        public Panel fraChangeItems = new Panel();
        public Panel fraChangeLevel = new Panel();
        public Panel fraChangeSkills = new Panel();
        public Panel fraChangeJob = new Panel();
        public Panel fraChangeSprite = new Panel();
        public Panel fraChangeGender = new Panel();
        public Panel fraChangePK = new Panel();
        public Panel fraGiveExp = new Panel();
        public Panel fraPlayerWarp = new Panel();
        public Panel fraMoveRouteWait = new Panel();
        public Panel fraSpawnNpc = new Panel();
        public Panel fraPlayAnimation = new Panel();
        public Panel fraSetFog = new Panel();
        public Panel fraSetWeather = new Panel();
        public Panel fraMapTint = new Panel();
        public Panel fraPlayBGM = new Panel();
        public Panel fraPlaySound = new Panel();
        public Panel fraSetWait = new Panel();
        public Panel fraSetAccess = new Panel();
        public Panel fraShowPic = new Panel();
        public ImageView picShowPic = new ImageView();

        // Text/entry controls referenced in logic
        public TextArea txtShowText = new TextArea();
        public TextBox txtChoicePrompt = new TextBox();
        public TextBox txtChoices1 = new TextBox();
        public TextBox txtChoices2 = new TextBox();
        public TextBox txtChoices3 = new TextBox();
        public TextBox txtChoices4 = new TextBox();
        public TextArea txtAddText_Text = new TextArea();
        public TextBox txtChatbubbleText = new TextBox();
        public ComboBox cmbChatBubbleTargetType = new ComboBox();
        public ComboBox cmbChatBubbleTarget = new ComboBox();
        public NumericStepper nudVariableData0 = new NumericStepper();
        public NumericStepper nudVariableData1 = new NumericStepper();
        public NumericStepper nudVariableData2 = new NumericStepper();
        public NumericStepper nudVariableData3 = new NumericStepper();
        public NumericStepper nudVariableData4 = new NumericStepper();
        public CheckBox optAddText_Player = new CheckBox { Text = "Player" };
        public CheckBox optVariableAction0 = new CheckBox { Text = "Set" };
        public CheckBox optVariableAction1 = new CheckBox { Text = "Add" };
        public CheckBox optVariableAction2 = new CheckBox { Text = "Sub" };
        public CheckBox optVariableAction3 = new CheckBox { Text = "Random" };

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
        }

        // Condition numeric controls
        public NumericStepper nudCondition_PlayerVarCondition = new NumericStepper();
        public NumericStepper nudCondition_HasItem = new NumericStepper();
        public NumericStepper nudCondition_LevelAmount = new NumericStepper();

        // Additional references used later
        public ComboBox cmbVariableDataType = new ComboBox();
        public ComboBox cmbPlayervarCompare = new ComboBox();
        public NumericStepper nudPlayerVariable = new NumericStepper();
        public CheckBox chkPlayerVar = new CheckBox { Text = "Player Var" };
        public CheckBox chkPlayerSwitch = new CheckBox { Text = "Player Switch" };
        public ComboBox cmbPlayerSwitchCompare = new ComboBox();
        public CheckBox chkHasItem = new CheckBox { Text = "Has Item" };
        public CheckBox chkSelfSwitch = new CheckBox { Text = "Self Switch" };
        public ComboBox cmbSelfSwitchCompare = new ComboBox();
        public CheckBox chkWalkAnim = new CheckBox { Text = "Walk Anim" };
        public CheckBox chkDirFix = new CheckBox { Text = "Dir Fix" };
        public CheckBox chkWalkThrough = new CheckBox { Text = "Walk Through" };
        public CheckBox chkShowName = new CheckBox { Text = "Show Name" };

        // Move route related
        public ListBox lstMoveRoute = new ListBox();
        public ListBox lstvwMoveRoute = new ListBox();
        public CheckBox chkIgnoreMove = new CheckBox { Text = "Ignore" };
        public CheckBox chkRepeatRoute = new CheckBox { Text = "Repeat" };
        public Button btnMoveRoute = new Button { Text = "Move Route" };
        public Button btnMoveRouteOk = new Button { Text = "Route OK" };
        public Button btnMoveRouteCancel = new Button { Text = "Cancel" };

        // Graphics selection
        public Button btnOK = new Button { Text = "OK" };
        public Button btnCancel = new Button { Text = "Cancel" };

        public CheckBox chkGlobal = new CheckBox { Text = "Global" };

        public Editor_Event()
        {
            _instance = this;
            Title = "Event Editor";
            ClientSize = new Size(1100, 750);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Ensure Load is subscribed first
            Load += Editor_Events_Load; // hook existing load logic
            // Basic left command tree + right content placeholder
            var left = new StackLayout
            {
                Orientation = Orientation.Vertical,
                Width = 250,
                Spacing = 4,
                Items =
                {
                    new Label{ Text = "Commands"},
                    new StackLayoutItem(tvCommands, true),
                    new Label{ Text = "Commands List"},
                    new StackLayoutItem(lstCommands, true),
                    new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 4,
                        Items = { btnAddCommand, btnEditCommand, btnDeleteComand, btnClearCommand }
                    }
                }
            };

            var right = new Scrollable
            {
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 6,
                    Items =
                    {
                        new StackLayout { Orientation = Orientation.Horizontal, Spacing = 4, Items = { new Label{ Text = "Name"}, txtName, chkGlobal } },
                        tabPages,
                        fraDialogue, // placeholder panels added so logic can toggle visibility
                        fraGraphic,
                        fraMoveRoute,
                        fraShowText, fraShowChoices, fraAddText, fraShowChatBubble
                    }
                }
            };

            Content = new Splitter
            {
                Orientation = Orientation.Horizontal,
                Panel1 = left,
                Panel2 = right,
                Position = 260
            };
        }

        #region Form

        public void ClearConditionFrame()
        {
            int i;

            cmbCondition_PlayerVarIndex.Enabled = false;
            cmbCondition_PlayerVarIndex.Items.Clear();

            for (i = 0; i < Constant.MaxVariables; i++)
                cmbCondition_PlayerVarIndex.Items.Add(i + 1 + ". " + Event.Variables[i]);
            cmbCondition_PlayerVarIndex.SelectedIndex = 0;
            cmbCondition_PlayerVarCompare.SelectedIndex = 0;
            cmbCondition_PlayerVarCompare.Enabled = false;
            nudCondition_PlayerVarCondition.Enabled = false;
            nudCondition_PlayerVarCondition.Value = 0;
            cmbCondition_PlayerSwitch.Enabled = false;
            cmbCondition_PlayerSwitch.Items.Clear();

            for (i = 0; i < Constant.MaxSwitches; i++)
                cmbCondition_PlayerSwitch.Items.Add(i + 1 + ". " + Event.Switches[i]);
            cmbCondition_PlayerSwitch.SelectedIndex = 0;
            cmbCondtion_PlayerSwitchCondition.Enabled = false;
            cmbCondtion_PlayerSwitchCondition.SelectedIndex = 0;
            cmbCondition_HasItem.Enabled = false;
            cmbCondition_HasItem.Items.Clear();

            for (i = 0; i < Constant.MaxItems; i++)
                cmbCondition_HasItem.Items.Add(i + 1 + ". " + Data.Item[i].Name);
            cmbCondition_HasItem.SelectedIndex = 0;
            nudCondition_HasItem.Enabled = false;
            nudCondition_HasItem.Value = 1;
            cmbCondition_JobIs.Enabled = false;
            cmbCondition_JobIs.Items.Clear();

            for (i = 0; i < Constant.MaxJobs; i++)
                cmbCondition_JobIs.Items.Add(i + 1 + ". " + Data.Job[i].Name);
            cmbCondition_JobIs.SelectedIndex = 0;
            cmbCondition_LearntSkill.Enabled = false;
            cmbCondition_LearntSkill.Items.Clear();

            for (i = 0; i < Constant.MaxSkills; i++)
                cmbCondition_LearntSkill.Items.Add(i + 1 + ". " + Strings.Trim(Data.Skill[i].Name));
            cmbCondition_LearntSkill.SelectedIndex = 0;
            cmbCondition_LevelCompare.Enabled = false;
            cmbCondition_LevelCompare.SelectedIndex = 0;
            nudCondition_LevelAmount.Enabled = false;
            nudCondition_LevelAmount.Value = 0;
            if (cmbCondition_SelfSwitch.Items.Count > -1)
            {
                cmbCondition_SelfSwitch.SelectedIndex = 0;
            }

            cmbCondition_SelfSwitch.Enabled = false;

            if (cmbCondition_SelfSwitchCondition.Items.Count > -1)
            {
                cmbCondition_SelfSwitchCondition.SelectedIndex = 0;
            }

            cmbCondition_SelfSwitchCondition.Enabled = false;

            cmbCondition_Gender.Enabled = false;

            cmbCondition_Time.Enabled = false;
        }

    private void Editor_Events_Load(object? sender, EventArgs e)
        {
            int i;

            Event.CurPageNum = 0;

            cmbSwitch.Items.Clear();
            for (i = 0; i < Constant.MaxSwitches; i++)
                cmbSwitch.Items.Add(i + 1 + ". " + Event.Switches[i]);
            cmbSwitch.SelectedIndex = 0;
            cmbVariable.Items.Clear();

            for (i = 0; i < Constant.MaxVariables; i++)
                cmbVariable.Items.Add(i + 1 + ". " + Event.Variables[i]);
            cmbVariable.SelectedIndex = 0;
            cmbChangeItemIndex.Items.Clear();
            for (i = 0; i < Constant.MaxItems; i++)
                cmbChangeItemIndex.Items.Add(Data.Item[i].Name);
            cmbChangeItemIndex.SelectedIndex = 0;
            nudChangeLevel.MinValue = 1;
            nudChangeLevel.MaxValue = Constant.MaxLevel;
            nudChangeLevel.Value = 1;
            cmbChangeSkills.Items.Clear();

            for (i = 0; i < Constant.MaxSkills; i++)
                cmbChangeSkills.Items.Add(Data.Skill[i].Name);
            cmbChangeSkills.SelectedIndex = 0;
            cmbChangeJob.Items.Clear();

            for (i = 0; i < Constant.MaxJobs; i++)
                cmbChangeJob.Items.Add(Strings.Trim(Data.Job[i].Name));
            cmbChangeJob.SelectedIndex = 0;
            nudChangeSprite.MaxValue = GameState.NumCharacters;
            cmbPlayAnim.Items.Clear();

            for (i = 0; i < Constant.MaxAnimations; i++)
                cmbPlayAnim.Items.Add(i + 1 + ". " + Data.Animation[i].Name);
            cmbPlayAnim.SelectedIndex = 0;

            cmbPlayBGM.Items.Clear();

            General.CacheMusic();
            var loopTo = Information.UBound(Sound.MusicCache);
            for (i = 0; i < loopTo; i++)
                cmbPlayBGM.Items.Add(Sound.MusicCache[i]);
            cmbPlayBGM.SelectedIndex = 0;
            cmbPlaySound.Items.Clear();

            General.CacheSound();
            var loopTo1 = Information.UBound(Sound.SoundCache);
            for (i = 0; i < loopTo1; i++)
                cmbPlaySound.Items.Add(Sound.SoundCache[i]);
            cmbPlaySound.SelectedIndex = 0;
            cmbOpenShop.Items.Clear();

            for (i = 0; i < Constant.MaxVariables; i++)
                cmbOpenShop.Items.Add(i + 1 + ". " + Data.Shop[i].Name);
            cmbOpenShop.SelectedIndex = 0;
            cmbSpawnNpc.Items.Clear();

            for (i = 0; i < Constant.MaxMapNpcs; i++)
            {
                if (Data.MyMap.Npc[i] > 0)
                {
                    cmbSpawnNpc.Items.Add(i + 1 + ". " + Data.Npc[Data.MyMap.Npc[i]].Name);
                }
                else
                {
                    cmbSpawnNpc.Items.Add(i + ". ");
                }
            }

            cmbSpawnNpc.SelectedIndex = 0;
            nudFogData0.MaxValue = GameState.NumFogs;
            nudWPMap.MaxValue = Constant.MaxVariables;

            // Layout sizing handled by Eto containers

            cmbEvent.Items.Add("This Event");
            cmbEvent.SelectedIndex = 0;

            // set the tabs
            tabPages.Pages.Clear();

            var loopTo2 = Event.TmpEvent.PageCount;
            for (i = 0; i < loopTo2; i++)
                tabPages.Pages.Add(new TabPage { Text = Conversion.Str(i + 1) });

            // items
            cmbHasItem.Items.Clear();
            for (i = 0; i < Constant.MaxItems; i++)
                cmbHasItem.Items.Add(i + 1 + ": " + Data.Item[i].Name);

            // variables
            cmbPlayerVar.Items.Clear();
            for (i = 0; i < Constant.MaxVariables; i++)
                cmbPlayerVar.Items.Add(i + 1 + ". " + Event.Variables[i]);
            // switches
            cmbPlayerSwitch.Items.Clear();
            for (i = 0; i < Constant.MaxSwitches; i++)
                cmbPlayerSwitch.Items.Add(i + 1 + ". " + Event.Switches[i]);
            cmbSelfSwitch.SelectedIndex = 0;

            // enable delete button
            if (Event.TmpEvent.PageCount > 1)
            {
                btnDeletePage.Enabled = true;
            }
            else
            {
                btnDeletePage.Enabled = false;
            }
            btnPastePage.Enabled = false;

            nudShowPicture.MaxValue = GameState.NumPictures;

            cmbPicLoc.SelectedIndex = 0;

            fraDialogue.Visible = false;

            if (tabPages.SelectedIndex == 0 && tabPages.Pages.Count > 1)
                tabPages.SelectedIndex = 1;

            // Load page 1 to start off with
            Event.CurPageNum = 0;
            if (string.IsNullOrEmpty(Event.TmpEvent.Name))
                Event.TmpEvent.Name = "";
            txtName.Text = Event.TmpEvent.Name;

            Event.EventEditorLoadPage(Event.CurPageNum);
            DrawGraphic();
        }

    private void Editor_Event_Resize(object? sender, EventArgs e) { }
    private void Editor_Event_Activated(object? sender, EventArgs e) { }

    public void DrawGraphic() { /* TODO: Reimplement drawing using Eto.Drawing */ }

    // WinForms FormClosing removed; handled by Eto partial if needed

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (fraGraphic.Visible == false)
            {
                Event.EventEditorOK();
                Event.TmpEvent = default;
            }
            else
            {
                if (Event.GraphicSelType == 0)
                {
                    Event.TmpEvent.Pages[Event.CurPageNum].GraphicType = (byte)cmbGraphic.SelectedIndex;
                    Event.TmpEvent.Pages[Event.CurPageNum].Graphic = (int)Math.Round(nudGraphic.Value);
                    Event.TmpEvent.Pages[Event.CurPageNum].GraphicX = Event.GraphicSelX;
                    Event.TmpEvent.Pages[Event.CurPageNum].GraphicY = Event.GraphicSelY;
                    Event.TmpEvent.Pages[Event.CurPageNum].GraphicX2 = Event.GraphicSelX2;
                    Event.TmpEvent.Pages[Event.CurPageNum].GraphicY2 = Event.GraphicSelY2;
                }
                else
                {
                    AddMoveRouteCommand(42);
                    Event.GraphicSelType = 0;
                }
                fraGraphic.Visible = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (fraGraphic.Visible == false)
            {
                Event.TmpEvent = default;
                Dispose();
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].GraphicType = tmpGraphicType;
                Event.TmpEvent.Pages[Event.CurPageNum].Graphic = tmpGraphicIndex;
                fraGraphic.Visible = false;
                DrawGraphic();
            }
        }

        private void TvCommands_AfterSelect(object? sender, EventArgs e)
        {
            // TODO: Implement Eto TreeView selection mapping
            var x = 0;
            var selectedText = string.Empty;
            switch (selectedText)
            {
                // Messages

                // show text
                case "Show Text":
                    {
                        txtShowText.Text = "";
                        fraDialogue.Visible = true;
                        fraShowText.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // show choices
                case "Show Choices":
                    {
                        txtChoicePrompt.Text = "";
                        txtChoices1.Text = "";
                        txtChoices2.Text = "";
                        txtChoices3.Text = "";
                        txtChoices4.Text = "";

                        fraDialogue.Visible = true;
                        fraShowChoices.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // chatbox text
                case "Add Chatbox Text":
                    {
                        txtAddText_Text.Text = "";
                        optAddText_Player.Checked = true;
                        fraDialogue.Visible = true;
                        fraAddText.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // chat bubble
                case "Show ChatBubble":
                    {
                        txtChatbubbleText.Text = "";
                        cmbChatBubbleTargetType.SelectedIndex = 0;
                        cmbChatBubbleTarget.Visible = false;
                        fraDialogue.Visible = true;
                        fraShowChatBubble.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // event progression
                // player variable
                case "Set Player Variable":
                    {
                        nudVariableData0.Value = 0;
                        nudVariableData1.Value = 0;
                        nudVariableData2.Value = 0;
                        nudVariableData3.Value = 0;
                        nudVariableData4.Value = 0;

                        cmbVariable.SelectedIndex = 0;
                        optVariableAction0.Checked = true;
                        fraDialogue.Visible = true;
                        fraPlayerVariable.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // player switch
                case "Set Player Switch":
                    {
                        cmbPlayerSwitchSet.SelectedIndex = 0;
                        cmbSwitch.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraPlayerSwitch.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // self switch
                case "Set Self Switch":
                    {
                        cmbSetSelfSwitchTo.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraSetSelfSwitch.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // flow control

                // conditional branch
                case "Conditional Branch":
                    {
                        fraDialogue.Visible = true;
                        fraConditionalBranch.Visible = true;
                        optCondition0.Checked = true;
                        ClearConditionFrame();
                        cmbCondition_PlayerVarIndex.Enabled = true;
                        cmbCondition_PlayerVarCompare.Enabled = true;
                        nudCondition_PlayerVarCondition.Enabled = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Exit Event Process
                case "Stop Event Processing":
                    {
                        Event.AddCommand((int)EventCommand.ExitEventProcess);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Label
                case "Label":
                    {
                        txtLabelName.Text = "";
                        fraCreateLabel.Visible = true;
                        fraCommands.Visible = false;
                        fraDialogue.Visible = true;
                        break;
                    }
                // GoTo Label
                case "GoTo Label":
                    {
                        txtGoToLabel.Text = "";
                        fraGoToLabel.Visible = true;
                        fraCommands.Visible = false;
                        fraDialogue.Visible = true;
                        break;
                    }
                // Player Control

                // Change Items
                case "Change Items":
                    {
                        cmbChangeItemIndex.SelectedIndex = 0;
                        optChangeItemSet.Checked = true;
                        nudChangeItemsAmount.Value = 0;
                        fraDialogue.Visible = true;
                        fraChangeItems.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Restore HP
                case "Restore HP":
                    {
                        Event.AddCommand((int)EventCommand.RestoreHealth);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Restore MP
                case "Restore MP":
                    {
                        Event.AddCommand((int)EventCommand.RestoreMana);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Restore SP
                case "Restore SP":
                    {
                        Event.AddCommand((int)EventCommand.RestoreStamina);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Level Up
                case "Level Up":
                    {
                        Event.AddCommand((int)EventCommand.ChangeLevel);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Change Level
                case "Change Level":
                    {
                        nudChangeLevel.Value = 1;
                        fraDialogue.Visible = true;
                        fraChangeLevel.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Change Skills
                case "Change Skills":
                    {
                        cmbChangeSkills.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraChangeSkills.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Change Job
                case "Change Job":
                    {
                        if (Constant.MaxJobs > 0)
                        {
                            if (cmbChangeJob.Items.Count == 0)
                            {
                                cmbChangeJob.Items.Clear();

                                for (int i = 0; i < Constant.MaxJobs; i++)
                                    cmbChangeJob.Items.Add(Strings.Trim(Data.Job[i].Name));
                                cmbChangeJob.SelectedIndex = 0;
                            }
                        }
                        fraDialogue.Visible = true;
                        fraChangeJob.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Change Sprite
                case "Change Sprite":
                    {
                        nudChangeSprite.Value = 1;
                        fraDialogue.Visible = true;
                        fraChangeSprite.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Change Gender
                case "Change Gender":
                    {
                        optChangeSexMale.Checked = true;
                        fraDialogue.Visible = true;
                        fraChangeGender.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Change PK
                case "Change PK":
                    {
                        cmbSetPK.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraChangePK.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Give Exp
                case "Give Experience":
                    {
                        nudGiveExp.Value = 0;
                        fraDialogue.Visible = true;
                        fraGiveExp.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Movement

                // Warp Player
                case "Warp Player":
                    {
                        nudWPMap.Value = 0;
                        nudWPX.Value = 0;
                        nudWPY.Value = 0;
                        cmbWarpPlayerDir.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraPlayerWarp.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Set Move Route
                case "Set Move Route":
                    {
                        fraMoveRoute.Visible = true;
                        lstMoveRoute.Items.Clear();
                        Event.ListOfEvents = new int[Data.MyMap.EventCount];
                        Event.ListOfEvents[0] = Event.EditorEvent;
                        for (int i = 0, loopTo = Data.MyMap.EventCount; i < loopTo; i++)
                        {
                            if (i != Event.EditorEvent)
                            {
                                cmbEvent.Items.Add(Data.MyMap.Event[i].Name);
                                x = x + 1;
                                Event.ListOfEvents[x] = i;
                            }
                        }
                        Event.IsMoveRouteCommand = true;
                        chkIgnoreMove.Checked = false;
                        chkRepeatRoute.Checked = false;
                        Event.TempMoveRouteCount = 0;
                        Event.TempMoveRoute = new Type.MoveRoute[1];
                        fraMoveRoute.Visible = true;
                        // fraMoveRoute.BringToFront(); // not available in Eto - rely on container layout & visibility
                        fraMoveRoute.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Wait for Route Completion
                case "Wait for Route Completion":
                    {
                        cmbMoveWait.Items.Clear();
                        Event.ListOfEvents = new int[Data.MyMap.EventCount];
                        Event.ListOfEvents[0] = Event.EditorEvent;
                        cmbMoveWait.Items.Add("This Event");
                        cmbMoveWait.SelectedIndex = 0;
                        cmbMoveWait.Enabled = true;
                        for (int i = 0, loopTo1 = Data.MyMap.EventCount; i < loopTo1; i++)
                        {
                            if (i != Event.EditorEvent)
                            {
                                cmbMoveWait.Items.Add(Data.MyMap.Event[i].Name);
                                x = x + 1;
                                Event.ListOfEvents[x] = i;
                            }
                        }
                        fraDialogue.Visible = true;
                        fraMoveRouteWait.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Force Spawn Npc
                case "Force Spawn Npc":
                    {
                        // lets populate the combobox
                        cmbSpawnNpc.Items.Clear();
                        for (int i = 0; i < Constant.MaxVariables; i++)
                            cmbSpawnNpc.Items.Add(Strings.Trim(Data.Npc[i].Name));
                        cmbSpawnNpc.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraSpawnNpc.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Hold Player
                case "Hold Player":
                    {
                        Event.AddCommand((int)EventCommand.HoldPlayer);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Release Player
                case "Release Player":
                    {
                        Event.AddCommand((int)EventCommand.ReleasePlayer);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Animation

                // Play Animation
                case "Play Animation":
                    {
                        cmbPlayAnimEvent.Items.Clear();

                        for (int i = 0, loopTo2 = Data.MyMap.EventCount; i < loopTo2; i++)
                            cmbPlayAnimEvent.Items.Add(i + 1 + ". " + Data.MyMap.Event[i].Name);
                        cmbPlayAnimEvent.SelectedIndex = 0;
                        cmbAnimTargetType.SelectedIndex = 0;
                        cmbPlayAnim.SelectedIndex = 0;
                        nudPlayAnimTileX.Value = 0;
                        nudPlayAnimTileY.Value = 0;
                        nudPlayAnimTileX.MaxValue = Data.MyMap.MaxX;
                        nudPlayAnimTileY.MaxValue = Data.MyMap.MaxY;
                        fraDialogue.Visible = true;
                        fraPlayAnimation.Visible = true;
                        fraCommands.Visible = false;
                        lblPlayAnimX.Visible = false;
                        lblPlayAnimY.Visible = false;
                        nudPlayAnimTileX.Visible = false;
                        nudPlayAnimTileY.Visible = false;
                        cmbPlayAnimEvent.Visible = false;
                        break;
                    }
                // Map Functions

                // Set Fog
                case "Set Fog":
                    {
                        nudFogData0.Value = 0;
                        nudFogData1.Value = 0;
                        nudFogData2.Value = 0;
                        fraDialogue.Visible = true;
                        fraSetFog.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Set Weather
                case "Set Weather":
                    {
                        CmbWeather.SelectedIndex = 0;
                        nudWeatherIntensity.Value = 0;
                        fraDialogue.Visible = true;
                        fraSetWeather.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Set Map Tinting
                case "Set Map Tinting":
                    {
                        nudMapTintData0.Value = 0;
                        nudMapTintData1.Value = 0;
                        nudMapTintData2.Value = 0;
                        nudMapTintData3.Value = 0;
                        fraDialogue.Visible = true;
                        fraMapTint.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Music and Sound

                // PlayBGM
                case "Play BGM":
                    {
                        cmbPlayBGM.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraPlayBGM.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Stop BGM
                case "Stop BGM":
                    {
                        Event.AddCommand((int)EventCommand.FadeOutBgm);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Play Sound
                case "Play Sound":
                    {
                        cmbPlaySound.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraPlaySound.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Stop Sounds
                case "Stop Sounds":
                    {
                        Event.AddCommand((int)EventCommand.StopSound);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Etc...

                // Wait...
                case "Wait...":
                    {
                        nudWaitAmount.Value = 1;
                        fraDialogue.Visible = true;
                        fraSetWait.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Set Access
                case "Set Access":
                    {
                        cmbSetAccess.SelectedIndex = 0;
                        fraDialogue.Visible = true;
                        fraSetAccess.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Shop, bank etc

                // Open bank
                case "Open Bank":
                    {
                        Event.AddCommand((int)EventCommand.OpenBank);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Open shop
                case "Open Shop":
                    {
                        fraDialogue.Visible = true;
                        fraOpenShop.Visible = true;
                        cmbOpenShop.SelectedIndex = 0;
                        fraCommands.Visible = false;
                        break;
                    }
                // cutscene options

                // Fade in
                case "Fade In":
                    {
                        Event.AddCommand((int)EventCommand.FadeIn);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Fade out
                case "Fade Out":
                    {
                        Event.AddCommand((int)EventCommand.FadeOut);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Flash white
                case "Flash White":
                    {
                        Event.AddCommand((int)EventCommand.FlashScreen);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
                // Show pic
                case "Show Picture":
                    {
                        nudShowPicture.Value = 0;
                        cmbPicLoc.SelectedIndex = 0;
                        nudPicOffsetX.Value = 0;
                        nudPicOffsetY.Value = 0;
                        fraDialogue.Visible = true;
                        fraShowPic.Visible = true;
                        fraCommands.Visible = false;
                        break;
                    }
                // Hide pic
                case "Hide Picture":
                    {
                        Event.AddCommand((int)EventCommand.HidePicture);
                        fraCommands.Visible = false;
                        fraDialogue.Visible = false;
                        break;
                    }
            }
        }

        private void BtnCancelCommand_Click(object sender, EventArgs e)
        {
            fraCommands.Visible = false;
        }

        #endregion

        #region Page Buttons

    private void TabPages_Click(object? sender, EventArgs e)
        {
            Event.CurPageNum = tabPages.SelectedIndex;
            Event.EventEditorLoadPage(Event.CurPageNum);
        }

    private void BtnNewPage_Click(object? sender, EventArgs e)
        {
            int pageCount;
            int i;

            if (chkGlobal.Checked == true)
            {
                Interaction.MsgBox("You cannot have multiple pages on global events!");
                return;
            }

            pageCount = Event.TmpEvent.PageCount + 1;

            // redim the array
            Array.Resize(ref Event.TmpEvent.Pages, pageCount);

            Event.TmpEvent.PageCount = pageCount;

            // set the tabs
            tabPages.Pages.Clear();

            var loopTo = Event.TmpEvent.PageCount;
            for (i = 0; i < loopTo; i++)
                tabPages.Pages.Add(new TabPage { Text = Conversion.Str(i + 1) });
            btnDeletePage.Enabled = true;
        }

    private void BtnCopyPage_Click(object? sender, EventArgs e)
        {
            Event.CopyEventPage = Event.TmpEvent.Pages[Event.CurPageNum];
            btnPastePage.Enabled = true;
        }

    private void BtnPastePage_Click(object? sender, EventArgs e)
        {
            Event.TmpEvent.Pages[Event.CurPageNum] = Event.CopyEventPage;
            Event.EventEditorLoadPage(Event.CurPageNum);
        }

    private void BtnDeletePage_Click(object? sender, EventArgs e)
        {
            Event.TmpEvent.Pages[Event.CurPageNum] = default;

            // move everything else down a notch
            if (Event.CurPageNum < Event.TmpEvent.PageCount)
            {
                for (int i = Event.CurPageNum, loopTo = Event.TmpEvent.PageCount - 1; i < loopTo; i++)
                    Event.TmpEvent.Pages[i] = Event.TmpEvent.Pages[i + 1];
            }
            Event.TmpEvent.PageCount = Event.TmpEvent.PageCount - 1;
            Event.CurPageNum = Event.TmpEvent.PageCount - 1;
            Event.EventEditorLoadPage(Event.CurPageNum);

            // set the tabs
            tabPages.Pages.Clear();

            for (int i = 0, loopTo1 = Event.TmpEvent.PageCount; i < loopTo1; i++)
                tabPages.Pages.Add(new TabPage { Text = Conversion.Str(i + 1) });

            // set the tab back
            if (Event.CurPageNum < Event.TmpEvent.PageCount)
            {
                // maintain selected index
            }
            else
            {
                // maintain selected index
            }
            // make sure we disable
            if (Event.TmpEvent.PageCount == 1)
            {
                btnDeletePage.Enabled = false;
            }

        }

    private void BtnClearPage_Click(object? sender, EventArgs e)
        {
            Event.TmpEvent.Pages[Event.CurPageNum] = default;
            Event.EventEditorLoadPage(Event.CurPageNum);
        }

    private void TxtName_TextChanged(object? sender, EventArgs e)
        {
            Event.TmpEvent.Name = Strings.Trim(txtName.Text);
        }

        #endregion

        #region Conditions

        private void ChkPlayerVar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayerVar.Checked == true)
            {
                cmbPlayerVar.Enabled = true;
                nudPlayerVariable.Enabled = true;
                cmbPlayervarCompare.Enabled = true;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkVariable = 1;
            }
            else
            {
                cmbPlayerVar.Enabled = false;
                nudPlayerVariable.Enabled = false;
                cmbPlayervarCompare.Enabled = false;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkVariable = 0;
            }
        }

        private void CmbPlayerVar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlayerVar.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].VariableIndex = cmbPlayerVar.SelectedIndex;
        }

        private void CmbPlayervarCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlayervarCompare.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].VariableCompare = cmbPlayervarCompare.SelectedIndex;
        }

        private void NudPlayerVariable_ValueChanged(object sender, EventArgs e)
        {
            Event.TmpEvent.Pages[Event.CurPageNum].VariableCondition = (int)Math.Round(nudPlayerVariable.Value);
        }

        private void ChkPlayerSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPlayerSwitch.Checked == true)
            {
                cmbPlayerSwitch.Enabled = true;
                cmbPlayerSwitchCompare.Enabled = true;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkSwitch = 1;
            }
            else
            {
                cmbPlayerSwitch.Enabled = false;
                cmbPlayerSwitchCompare.Enabled = false;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkSwitch = 0;
            }
        }

        private void CmbPlayerSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlayerSwitch.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].SwitchIndex = cmbPlayerSwitch.SelectedIndex;
        }

        private void CmbPlayerSwitchCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlayerSwitchCompare.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].SwitchCompare = cmbPlayerSwitchCompare.SelectedIndex;
        }

        private void ChkHasItem_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHasItem.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].ChkHasItem = 1;
                cmbHasItem.Enabled = true;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].ChkHasItem = 0;
                cmbHasItem.Enabled = false;
            }

        }

        private void CmbHasItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHasItem.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].HasItemIndex = cmbHasItem.SelectedIndex;
            Event.TmpEvent.Pages[Event.CurPageNum].HasItemAmount = (int)Math.Round(nudCondition_HasItem.Value);
        }

        private void ChkSelfSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelfSwitch.Checked == true)
            {
                cmbSelfSwitch.Enabled = true;
                cmbSelfSwitchCompare.Enabled = true;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkSelfSwitch = 1;
            }
            else
            {
                cmbSelfSwitch.Enabled = false;
                cmbSelfSwitchCompare.Enabled = false;
                Event.TmpEvent.Pages[Event.CurPageNum].ChkSelfSwitch = 0;
            }
        }

        private void CmbSelfSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelfSwitch.SelectedIndex == -1)
                return;

            if (Event.TmpEvent.Pages == null)
                return;

            Event.TmpEvent.Pages[Event.CurPageNum].SelfSwitchIndex = cmbSelfSwitch.SelectedIndex;
        }

        private void CmbSelfSwitchCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelfSwitchCompare.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].SelfSwitchCompare = cmbSelfSwitchCompare.SelectedIndex;
        }

        #endregion

        #region Graphic

        private void PicGraphic_Click(object sender, EventArgs e)
        {
            // BringToFront removed for Eto; ensure visible
            tmpGraphicIndex = Event.TmpEvent.Pages[Event.CurPageNum].Graphic;
            tmpGraphicType = Event.TmpEvent.Pages[Event.CurPageNum].GraphicType;
            fraGraphic.Visible = true;
            Event.GraphicSelType = 0;
        }

        private void CmbGraphic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGraphic.SelectedIndex == -1)
                return;

            Event.TmpEvent.Pages[Event.CurPageNum].GraphicType = (byte)cmbGraphic.SelectedIndex;
            // set the max on the scrollbar
            switch (cmbGraphic.SelectedIndex)
            {
                case 0: // None
                    {
                        nudGraphic.Enabled = false;
                        break;
                    }
                case 1: // character
                    {
                        nudGraphic.MaxValue = GameState.NumCharacters;
                        nudGraphic.Enabled = true;
                        break;
                    }
                case 2: // Tileset
                    {
                        nudGraphic.MaxValue = GameState.NumTileSets;
                        nudGraphic.Enabled = true;
                        break;
                    }
            }

            if (Event.TmpEvent.Pages[Event.CurPageNum].GraphicType == 1)
            {
                if (nudGraphic.Value <= 0 | nudGraphic.Value > GameState.NumCharacters)
                    return;
            }

            else if (Event.TmpEvent.Pages[Event.CurPageNum].GraphicType == 2)
            {
                if (nudGraphic.Value <= 0 | nudGraphic.Value > GameState.NumTileSets)
                    return;

            }
            DrawGraphic();
        }

        private void PicGraphicSel_MouseDown(object sender, MouseEventArgs e)
        {
            int X;
            int Y;

            X = (int)e.Location.X;
            Y = (int)e.Location.Y;

            int selW = (int)Math.Round(Math.Ceiling((decimal)(X)) - Event.GraphicSelX);
            int selH = (int)Math.Round(Math.Ceiling((decimal)(Y)) - Event.GraphicSelY);

            if (cmbGraphic.SelectedIndex == 2)
            {
                // Multi-tile (shift-mod) selection not yet implemented in Eto. Single-tile select:
                Event.GraphicSelX = (int)Math.Round(Math.Ceiling((decimal)(X)));
                Event.GraphicSelY = (int)Math.Round(Math.Ceiling((decimal)(Y)));
                Event.GraphicSelX2 = 1;
                Event.GraphicSelY2 = 1;
            }
            else if (cmbGraphic.SelectedIndex == 1)
            {
                Event.GraphicSelX = X;
                Event.GraphicSelY = Y;
                Event.GraphicSelX2 = 0;
                Event.GraphicSelY2 = 0;

                if (nudGraphic.Value <= 0 | nudGraphic.Value > GameState.NumCharacters)
                    return;

                for (int i = 0; i <= 3; i++)
                {
                    if (Event.GraphicSelX >= GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Characters, nudGraphic.Value.ToString())).Width / 4d * i & Event.GraphicSelX < GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Characters, nudGraphic.Value.ToString())).Width / 4d * (i + 1))
                    {
                        Event.GraphicSelX = i;
                    }
                }
                for (int i = 0; i <= 3; i++)
                {
                    if (Event.GraphicSelY >= GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Characters, nudGraphic.Value.ToString())).Height / 4d * i & Event.GraphicSelY < GameClient.GetGfxInfo(System.IO.Path.Combine(DataPath.Characters, nudGraphic.Value.ToString())).Height / 4d * (i + 1))
                    {
                        Event.GraphicSelY = i;
                    }
                }
            }
            DrawGraphic();
        }

        private void nudGraphic_ValueChanged(object sender, EventArgs e)
        {
            DrawGraphic();
        }

        #endregion

        #region Movement

        private void CmbMoveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMoveType.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].MoveType = (byte)cmbMoveType.SelectedIndex;
            if (cmbMoveType.SelectedIndex == 2)
            {
                btnMoveRoute.Enabled = true;
            }
            else
            {
                btnMoveRoute.Enabled = false;
            }
        }

        private void BtnMoveRoute_Click(object sender, EventArgs e)
        {
            // BringToFront removed for Eto
            lstMoveRoute.Items.Clear();
            Event.IsMoveRouteCommand = false;
            chkIgnoreMove.Checked = Conversions.ToBoolean(Event.TmpEvent.Pages[Event.CurPageNum].IgnoreMoveRoute);
            chkRepeatRoute.Checked = Conversions.ToBoolean(Event.TmpEvent.Pages[Event.CurPageNum].RepeatMoveRoute);
            Event.TempMoveRouteCount = Event.TmpEvent.Pages[Event.CurPageNum].MoveRouteCount;

            // Will it let me do this?
            Event.TempMoveRoute = Event.TmpEvent.Pages[Event.CurPageNum].MoveRoute;
            for (int i = 0, loopTo = Event.TempMoveRouteCount; i < loopTo; i++)
            {
                switch (Event.TempMoveRoute[i].Index)
                {
                    case 1:
                        {
                            lstMoveRoute.Items.Add("Move Up");
                            break;
                        }
                    case 2:
                        {
                            lstMoveRoute.Items.Add("Move Down");
                            break;
                        }
                    case 3:
                        {
                            lstMoveRoute.Items.Add("Move Left");
                            break;
                        }
                    case 4:
                        {
                            lstMoveRoute.Items.Add("Move Right");
                            break;
                        }
                    case 5:
                        {
                            lstMoveRoute.Items.Add("Move Randomly");
                            break;
                        }
                    case 6:
                        {
                            lstMoveRoute.Items.Add("Move Towards Player");
                            break;
                        }
                    case 7:
                        {
                            lstMoveRoute.Items.Add("Move Away From Player");
                            break;
                        }
                    case 8:
                        {
                            lstMoveRoute.Items.Add("Step Forward");
                            break;
                        }
                    case 9:
                        {
                            lstMoveRoute.Items.Add("Step Back");
                            break;
                        }
                    case 10:
                        {
                            lstMoveRoute.Items.Add("Wait 100ms");
                            break;
                        }
                    case 11:
                        {
                            lstMoveRoute.Items.Add("Wait 500ms");
                            break;
                        }
                    case 12:
                        {
                            lstMoveRoute.Items.Add("Wait 1000ms");
                            break;
                        }
                    case 13:
                        {
                            lstMoveRoute.Items.Add("Turn Up");
                            break;
                        }
                    case 14:
                        {
                            lstMoveRoute.Items.Add("Turn Down");
                            break;
                        }
                    case 15:
                        {
                            lstMoveRoute.Items.Add("Turn Left");
                            break;
                        }
                    case 16:
                        {
                            lstMoveRoute.Items.Add("Turn Right");
                            break;
                        }
                    case 17:
                        {
                            lstMoveRoute.Items.Add("Turn 90 Degrees To the Right");
                            break;
                        }
                    case 18:
                        {
                            lstMoveRoute.Items.Add("Turn 90 Degrees To the Left");
                            break;
                        }
                    case 19:
                        {
                            lstMoveRoute.Items.Add("Turn Around 180 Degrees");
                            break;
                        }
                    case 20:
                        {
                            lstMoveRoute.Items.Add("Turn Randomly");
                            break;
                        }
                    case 21:
                        {
                            lstMoveRoute.Items.Add("Turn Towards Player");
                            break;
                        }
                    case 22:
                        {
                            lstMoveRoute.Items.Add("Turn Away from Player");
                            break;
                        }
                    case 23:
                        {
                            lstMoveRoute.Items.Add("Set Speed 8x Slower");
                            break;
                        }
                    case 24:
                        {
                            lstMoveRoute.Items.Add("Set Speed 4x Slower");
                            break;
                        }
                    case 25:
                        {
                            lstMoveRoute.Items.Add("Set Speed 2x Slower");
                            break;
                        }
                    case 26:
                        {
                            lstMoveRoute.Items.Add("Set Speed to Normal");
                            break;
                        }
                    case 27:
                        {
                            lstMoveRoute.Items.Add("Set Speed 2x Faster");
                            break;
                        }
                    case 28:
                        {
                            lstMoveRoute.Items.Add("Set Speed 4x Faster");
                            break;
                        }
                    case 29:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Lowest");
                            break;
                        }
                    case 30:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Lower");
                            break;
                        }
                    case 31:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Normal");
                            break;
                        }
                    case 32:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Higher");
                            break;
                        }
                    case 33:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Highest");
                            break;
                        }
                    case 34:
                        {
                            lstMoveRoute.Items.Add("Turn On Walking Animation");
                            break;
                        }
                    case 35:
                        {
                            lstMoveRoute.Items.Add("Turn Off Walking Animation");
                            break;
                        }
                    case 36:
                        {
                            lstMoveRoute.Items.Add("Turn On Fixed Direction");
                            break;
                        }
                    case 37:
                        {
                            lstMoveRoute.Items.Add("Turn Off Fixed Direction");
                            break;
                        }
                    case 38:
                        {
                            lstMoveRoute.Items.Add("Turn On Walk Through");
                            break;
                        }
                    case 39:
                        {
                            lstMoveRoute.Items.Add("Turn Off Walk Through");
                            break;
                        }
                    case 40:
                        {
                            lstMoveRoute.Items.Add("Set Position Below Player");
                            break;
                        }
                    case 41:
                        {
                            lstMoveRoute.Items.Add("Set Position at Player Level");
                            break;
                        }
                    case 42:
                        {
                            lstMoveRoute.Items.Add("Set Position Above Player");
                            break;
                        }
                    case 43:
                        {
                            lstMoveRoute.Items.Add("Set Graphic");
                            break;
                        }
                }
            }

            fraMoveRoute.Visible = true;

        }

        private void CmbMoveSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMoveSpeed.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].MoveSpeed = (byte)cmbMoveSpeed.SelectedIndex;
        }

        private void CmbMoveFreq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMoveFreq.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].MoveFreq = (byte)cmbMoveFreq.SelectedIndex;
        }

        #endregion

        #region Positioning

        private void CmbPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Event.TmpEvent.Pages == null)
                return;

            if (cmbPositioning.SelectedIndex == -1)
                return;

            Event.TmpEvent.Pages[Event.CurPageNum].Position = (byte)cmbPositioning.SelectedIndex;
        }

        #endregion

        #region Trigger

        private void CmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Event.TmpEvent.Pages == null)
                return;

            if (cmbTrigger.SelectedIndex == -1)
                return;
            Event.TmpEvent.Pages[Event.CurPageNum].Trigger = (byte)cmbTrigger.SelectedIndex;
        }

        private void ChkGlobal_CheckedChanged(object sender, EventArgs e)
        {
            if (Event.TmpEvent.PageCount > 0)
            {
                if (MessageBox.Show("If you set the event to global you will lose all pages except for your first one. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            if (chkGlobal.Checked == true)
            {
                Event.TmpEvent.Globals = 1;
            }
            else
            {
                Event.TmpEvent.Globals = 0;
            }

            Event.TmpEvent.PageCount = 1;
            Event.CurPageNum = 0;
            tabPages.Pages.Clear();

            for (int i = 0, loopTo = Event.TmpEvent.PageCount; i < loopTo; i++)
                tabPages.Pages.Add(new TabPage { Text = (i + 1).ToString() });
            Event.EventEditorLoadPage(Event.CurPageNum);
        }

        #endregion

        #region Options

        private void ChkWalkAnim_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWalkAnim.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].WalkAnim = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].WalkAnim = 0;
            }

        }

        private void ChkDirFix_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDirFix.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].DirFix = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].DirFix = 0;
            }

        }

        private void ChkWalkThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWalkThrough.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].WalkThrough = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].WalkThrough = 0;
            }

        }

        private void ChkShowName_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowName.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].ShowName = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].ShowName = 0;
            }

        }

        #endregion

        #region Commands

        private void LstCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event.CurCommand = lstCommands.SelectedIndex;
        }

        private void BtnAddCommand_Click(object sender, EventArgs e)
        {
            if (lstCommands.SelectedIndex > -1)
            {
                Event.IsEdit = false;
                // tabPages.SelectedTab = TabPage
                fraCommands.Visible = true;
            }
        }

        private void BtnEditCommand_Click(object sender, EventArgs e)
        {
            Event.EditEventCommand();
        }

        private void BtnDeleteComand_Click(object sender, EventArgs e)
        {
            Event.DeleteEventCommand();
        }

        private void BtnClearCommand_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all event commands?", "Clear Event Commands?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Event.ClearEventCommands();
            }
        }

        #endregion

        #region Variables/Switches

        // 'Renaming Variables/Switches
        private void BtnLabeling_Click(object sender, EventArgs e)
        {
            // Show variable/switch management panel (Eto: rely on layout, no BringToFront/absolute positioning)
            pnlVariableSwitches.Visible = true;
            lstSwitches.Items.Clear();

            for (int i = 0; i < Constant.MaxSwitches; i++)
                lstSwitches.Items.Add((i + 1).ToString()  + ". " + Event.Switches[i]);
            lstVariables.Items.Clear();

            for (int i = 0; i < Constant.MaxVariables; i++)
                lstVariables.Items.Add((i + 1).ToString() + ". " + Event.Variables[i]);

        }

        private void BtnRename_Ok_Click(object sender, EventArgs e)
        {
            FraRenaming.Visible = false;
            fraLabeling.Visible = true;

            switch (Event.RenameType)
            {
                case 1:
                    {
                        // Variable
                        if (Event.RenameIndex >= 0 & Event.RenameIndex < Constant.MaxVariables)
                        {
                            Event.Variables[Event.RenameIndex] = txtRename.Text;
                            FraRenaming.Visible = false;
                            fraLabeling.Visible = true;
                            Event.RenameType = 0;
                            Event.RenameIndex = 0;
                        }

                        break;
                    }
                case 2:
                    {
                        // Switch
                        if (Event.RenameIndex >= 0 & Event.RenameIndex < Constant.MaxSwitches)
                        {
                            Event.Switches[Event.RenameIndex] = txtRename.Text;
                            FraRenaming.Visible = false;
                            fraLabeling.Visible = true;
                            Event.RenameType = 0;
                            Event.RenameIndex = 0;
                        }

                        break;
                    }
            }
            lstSwitches.Items.Clear();
            for (int i = 0; i < Constant.MaxSwitches; i++)
                lstSwitches.Items.Add((i + 1).ToString() + ". " + Strings.Trim(Event.Switches[i]));
            lstSwitches.SelectedIndex = 0;
            lstVariables.Items.Clear();

            for (int i = 0; i < Constant.MaxVariables; i++)
                lstVariables.Items.Add((i + 1).ToString() + ". " + Strings.Trim(Event.Variables[i]));
            lstVariables.SelectedIndex = 0;
        }

        private void BtnRename_Cancel_Click(object sender, EventArgs e)
        {
            FraRenaming.Visible = false;
            fraLabeling.Visible = true;

            Event.RenameType = 0;
            Event.RenameIndex = 0;
            lstSwitches.Items.Clear();

            for (int i = 0; i < Constant.MaxSwitches; i++)
                lstSwitches.Items.Add((i + 1).ToString() + ". " + Event.Switches[i]);
            lstSwitches.SelectedIndex = 0;
            lstVariables.Items.Clear();

            for (int i = 0; i < Constant.MaxVariables; i++)
                lstVariables.Items.Add((i + 1).ToString() + ". " + Event.Variables[i]);
            lstVariables.SelectedIndex = 0;
        }

        private void TxtRename_TextChanged(object sender, EventArgs e)
        {
            Event.TmpEvent.Name = Strings.Trim(txtName.Text);
        }

        private void LstVariables_DoubleClick(object sender, EventArgs e)
        {
            if (lstVariables.SelectedIndex > -1 & lstVariables.SelectedIndex < Constant.MaxVariables)
            {
                FraRenaming.Visible = true;
                fraLabeling.Visible = false;
                lblEditing.Text = "Editing Variable: " + lstVariables.SelectedIndex.ToString();
                txtRename.Text = Event.Variables[lstVariables.SelectedIndex];
                Event.RenameType = 1;
                Event.RenameIndex = lstVariables.SelectedIndex;
            }
        }

        private void LstSwitches_DoubleClick(object sender, EventArgs e)
        {
            if (lstSwitches.SelectedIndex > -1 & lstSwitches.SelectedIndex < Constant.MaxSwitches)
            {
                FraRenaming.Visible = true;
                fraLabeling.Visible = false;
                lblEditing.Text = "Editing Switch: " + lstSwitches.SelectedIndex.ToString();
                txtRename.Text = Event.Switches[lstSwitches.SelectedIndex];
                Event.RenameType = 2;
                Event.RenameIndex = lstSwitches.SelectedIndex;
            }
        }

        private void BtnRenameVariable_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedIndex > -1 & lstVariables.SelectedIndex < Constant.MaxVariables)
            {
                FraRenaming.Visible = true;
                fraLabeling.Visible = false;
                lblEditing.Text = "Editing Variable: " + lstVariables.SelectedIndex.ToString();
                txtRename.Text = Event.Variables[lstVariables.SelectedIndex];
                Event.RenameType = 1;
                Event.RenameIndex = lstVariables.SelectedIndex;
            }
        }

        private void BtnRenameSwitch_Click(object sender, EventArgs e)
        {
            if (lstSwitches.SelectedIndex > -1 & lstSwitches.SelectedIndex < Constant.MaxSwitches)
            {
                FraRenaming.Visible = true;
                lblEditing.Text = "Editing Switch: " + lstSwitches.SelectedIndex.ToString();
                txtRename.Text = Event.Switches[lstSwitches.SelectedIndex];
                Event.RenameType = 2;
                Event.RenameIndex = lstSwitches.SelectedIndex;
            }
        }

        private void BtnLabel_Ok_Click(object sender, EventArgs e)
        {
            pnlVariableSwitches.Visible = false;
            Event.SendSwitchesAndVariables();
        }

        private void BtnLabel_Cancel_Click(object sender, EventArgs e)
        {
            pnlVariableSwitches.Visible = false;
            Event.RequestSwitchesAndVariables();
        }

        // MoveRoute Commands
        private void LstvwMoveRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Eto ListBox: single SelectedIndex
            if (lstvwMoveRoute.SelectedIndex < 0)
                return;
            int selectedIndex = lstvwMoveRoute.SelectedIndex;

            switch (selectedIndex + 1)
            {
                // Set Graphic
                case 43:
                    {
                        // fraGraphic.Visible already controls z-order in Eto layouts
                        Event.GraphicSelType = 1;
                        break;
                    }

                default:
                    {
                        AddMoveRouteCommand(selectedIndex);
                        break;
                    }
            }
        }

        private void LstMoveRoute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Delete)
            {
                // remove move route command lol
                if (lstMoveRoute.SelectedIndex > -1)
                {
                    RemoveMoveRouteCommand(lstMoveRoute.SelectedIndex);
                }
            }
        }

        public void AddMoveRouteCommand(int Index)
        {
            int i;
            int X;

            Index = Index + 1;
            if (lstMoveRoute.SelectedIndex > -1)
            {
                i = lstMoveRoute.SelectedIndex;
                Event.TempMoveRouteCount += 1;
                Array.Resize(ref Event.TempMoveRoute, Event.TempMoveRouteCount);
                var loopTo = i;
                for (X = Event.TempMoveRouteCount; X > loopTo; X -= 1)
                    Event.TempMoveRoute[X + 1] = Event.TempMoveRoute[X];
                Event.TempMoveRoute[i].Index = Index;
                // if set graphic then...
                if (Index == 43)
                {
                    Event.TempMoveRoute[i].Data1 = cmbGraphic.SelectedIndex;
                    Event.TempMoveRoute[i].Data2 = (int)Math.Round(nudGraphic.Value);
                    Event.TempMoveRoute[i].Data3 = Event.GraphicSelX;
                    Event.TempMoveRoute[i].Data4 = Event.GraphicSelX2;
                    Event.TempMoveRoute[i].Data5 = Event.GraphicSelY;
                    Event.TempMoveRoute[i].Data6 = Event.GraphicSelY2;
                }
                PopulateMoveRouteList();
            }
            else
            {
                Event.TempMoveRouteCount += 1;
                Array.Resize(ref Event.TempMoveRoute, Event.TempMoveRouteCount);
                Event.TempMoveRoute[Event.TempMoveRouteCount].Index = Index;
                PopulateMoveRouteList();
                // if set graphic then....
                if (Index == 43)
                {
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data1 = cmbGraphic.SelectedIndex;
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data2 = (int)Math.Round(nudGraphic.Value);
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data3 = Event.GraphicSelX;
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data4 = Event.GraphicSelX2;
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data5 = Event.GraphicSelY;
                    Event.TempMoveRoute[Event.TempMoveRouteCount].Data6 = Event.GraphicSelY2;
                }
            }

        }

        public void RemoveMoveRouteCommand(int Index)
        {
            int i;

            Index = Index + 1;
            if (Index > 0 & Index <= Event.TempMoveRouteCount)
            {
                var loopTo = Event.TempMoveRouteCount;
                for (i = Index + 1; i < loopTo; i++)
                    Event.TempMoveRoute[i - 1] = Event.TempMoveRoute[i];
                Event.TempMoveRouteCount = Event.TempMoveRouteCount - 1;
                if (Event.TempMoveRouteCount == 0)
                {
                    Event.TempMoveRoute = new Type.MoveRoute[1];
                }
                else
                {
                    Array.Resize(ref Event.TempMoveRoute, Event.TempMoveRouteCount);
                }
                PopulateMoveRouteList();
            }

        }

        public void PopulateMoveRouteList()
        {
            int i;

            lstMoveRoute.Items.Clear();

            var loopTo = Event.TempMoveRouteCount;
            for (i = 0; i < loopTo; i++)
            {
                switch (Event.TempMoveRoute[i].Index)
                {
                    case 1:
                        {
                            lstMoveRoute.Items.Add("Move Up");
                            break;
                        }
                    case 2:
                        {
                            lstMoveRoute.Items.Add("Move Down");
                            break;
                        }
                    case 3:
                        {
                            lstMoveRoute.Items.Add("Move Left");
                            break;
                        }
                    case 4:
                        {
                            lstMoveRoute.Items.Add("Move Right");
                            break;
                        }
                    case 5:
                        {
                            lstMoveRoute.Items.Add("Move Randomly");
                            break;
                        }
                    case 6:
                        {
                            lstMoveRoute.Items.Add("Move Towards Player");
                            break;
                        }
                    case 7:
                        {
                            lstMoveRoute.Items.Add("Move Away From Player");
                            break;
                        }
                    case 8:
                        {
                            lstMoveRoute.Items.Add("Step Forward");
                            break;
                        }
                    case 9:
                        {
                            lstMoveRoute.Items.Add("Step Back");
                            break;
                        }
                    case 10:
                        {
                            lstMoveRoute.Items.Add("Wait 100ms");
                            break;
                        }
                    case 11:
                        {
                            lstMoveRoute.Items.Add("Wait 500ms");
                            break;
                        }
                    case 12:
                        {
                            lstMoveRoute.Items.Add("Wait 1000ms");
                            break;
                        }
                    case 13:
                        {
                            lstMoveRoute.Items.Add("Turn Up");
                            break;
                        }
                    case 14:
                        {
                            lstMoveRoute.Items.Add("Turn Down");
                            break;
                        }
                    case 15:
                        {
                            lstMoveRoute.Items.Add("Turn Left");
                            break;
                        }
                    case 16:
                        {
                            lstMoveRoute.Items.Add("Turn Right");
                            break;
                        }
                    case 17:
                        {
                            lstMoveRoute.Items.Add("Turn 90 Degrees To the Right");
                            break;
                        }
                    case 18:
                        {
                            lstMoveRoute.Items.Add("Turn 90 Degrees To the Left");
                            break;
                        }
                    case 19:
                        {
                            lstMoveRoute.Items.Add("Turn Around 180 Degrees");
                            break;
                        }
                    case 20:
                        {
                            lstMoveRoute.Items.Add("Turn Randomly");
                            break;
                        }
                    case 21:
                        {
                            lstMoveRoute.Items.Add("Turn Towards Player");
                            break;
                        }
                    case 22:
                        {
                            lstMoveRoute.Items.Add("Turn Away from Player");
                            break;
                        }
                    case 23:
                        {
                            lstMoveRoute.Items.Add("Set Speed 8x Slower");
                            break;
                        }
                    case 24:
                        {
                            lstMoveRoute.Items.Add("Set Speed 4x Slower");
                            break;
                        }
                    case 25:
                        {
                            lstMoveRoute.Items.Add("Set Speed 2x Slower");
                            break;
                        }
                    case 26:
                        {
                            lstMoveRoute.Items.Add("Set Speed to Normal");
                            break;
                        }
                    case 27:
                        {
                            lstMoveRoute.Items.Add("Set Speed 2x Faster");
                            break;
                        }
                    case 28:
                        {
                            lstMoveRoute.Items.Add("Set Speed 4x Faster");
                            break;
                        }
                    case 29:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Lowest");
                            break;
                        }
                    case 30:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Lower");
                            break;
                        }
                    case 31:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Normal");
                            break;
                        }
                    case 32:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Higher");
                            break;
                        }
                    case 33:
                        {
                            lstMoveRoute.Items.Add("Set Frequency Highest");
                            break;
                        }
                    case 34:
                        {
                            lstMoveRoute.Items.Add("Turn On Walking Animation");
                            break;
                        }
                    case 35:
                        {
                            lstMoveRoute.Items.Add("Turn Off Walking Animation");
                            break;
                        }
                    case 36:
                        {
                            lstMoveRoute.Items.Add("Turn On Fixed Direction");
                            break;
                        }
                    case 37:
                        {
                            lstMoveRoute.Items.Add("Turn Off Fixed Direction");
                            break;
                        }
                    case 38:
                        {
                            lstMoveRoute.Items.Add("Turn On Walk Through");
                            break;
                        }
                    case 39:
                        {
                            lstMoveRoute.Items.Add("Turn Off Walk Through");
                            break;
                        }
                    case 40:
                        {
                            lstMoveRoute.Items.Add("Set Position Below Player");
                            break;
                        }
                    case 41:
                        {
                            lstMoveRoute.Items.Add("Set Position at Player Level");
                            break;
                        }
                    case 42:
                        {
                            lstMoveRoute.Items.Add("Set Position Above Player");
                            break;
                        }
                    case 43:
                        {
                            lstMoveRoute.Items.Add("Set Graphic");
                            break;
                        }
                }
            }

        }

        private void ChkIgnoreMove_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIgnoreMove.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].IgnoreMoveRoute = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].IgnoreMoveRoute = 0;
            }
        }

        private void ChkRepeatRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRepeatRoute.Checked == true)
            {
                Event.TmpEvent.Pages[Event.CurPageNum].RepeatMoveRoute = 1;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].RepeatMoveRoute = 0;
            }
        }

        private void BtnMoveRouteOk_Click(object sender, EventArgs e)
        {
            if (Event.IsMoveRouteCommand == true)
            {
                if (!Event.IsEdit)
                {
                    Event.AddCommand((int)EventCommand.SetMoveRoute);
                }
                else
                {
                    Event.EditCommand();
                }
                Event.TempMoveRouteCount = 0;
                Event.TempMoveRoute = new Type.MoveRoute[1];
                fraMoveRoute.Visible = false;
            }
            else
            {
                Event.TmpEvent.Pages[Event.CurPageNum].MoveRouteCount = Event.TempMoveRouteCount;
                Event.TmpEvent.Pages[Event.CurPageNum].MoveRoute = Event.TempMoveRoute;
                Event.TempMoveRouteCount = 0;
                Event.TempMoveRoute = new Type.MoveRoute[1];
                fraMoveRoute.Visible = false;
            }
        }

        private void BtnMoveRouteCancel_Click(object sender, EventArgs e)
        {
            Event.TempMoveRouteCount = 0;
            Event.TempMoveRoute = new Type.MoveRoute[1];
            fraMoveRoute.Visible = false;
        }

        #endregion

        #region CommandFrames

        #region Show Text

        private void BtnShowTextOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ShowText);
            }
            else
            {
                Event.EditCommand();
            }

            // hide
            fraDialogue.Visible = false;
            fraShowText.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnShowTextCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraShowText.Visible = false;
        }

        #endregion

        #region Add Text

        private void BtnAddTextOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.AddText);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraAddText.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnAddTextCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraAddText.Visible = false;
        }

        #endregion

        #region Show Choices
        private void BtnShowChoicesOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ShowChoices);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraShowChoices.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnShowChoicesCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraShowChoices.Visible = false;
        }

        #endregion

        #region Show Chatbubble

        private void CmbChatBubbleTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChatBubbleTargetType.SelectedIndex == (int)TargetType.None)
            {
                cmbChatBubbleTarget.Visible = false;
            }
            else if (cmbChatBubbleTargetType.SelectedIndex == (int)TargetType.Player)
            {
                cmbChatBubbleTarget.Visible = true;
                cmbChatBubbleTarget.Items.Clear();

                for (int i = 0; i < Constant.MaxNpcs; i++)
                {
                    if (Data.MyMap.Npc[i] < 0)
                    {
                        cmbChatBubbleTarget.Items.Add(i + ". ");
                    }
                    else
                    {
                        cmbChatBubbleTarget.Items.Add(i + 1 + ". " + Data.Npc[Data.MyMap.Npc[i]].Name);
                    }
                }
                cmbChatBubbleTarget.SelectedIndex = 0;
            }
            else if (cmbChatBubbleTargetType.SelectedIndex == (int)TargetType.Npc)
            {
                cmbChatBubbleTarget.Visible = true;
                cmbChatBubbleTarget.Items.Clear();

                for (int i = 0, loopTo = Data.MyMap.EventCount; i < loopTo; i++)
                    cmbChatBubbleTarget.Items.Add(i + 1 + ". " + Data.MyMap.Event[i].Name);
                cmbChatBubbleTarget.SelectedIndex = 0;
            }

        }

        private void BtnShowChatBubbleOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ShowChatBubble);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraShowChatBubble.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnShowChatBubbleCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraShowChatBubble.Visible = false;
        }

        #endregion

        #region Set Player Variable

        private void OptVariableAction0_CheckedChanged(object sender, EventArgs e)
        {
            if (optVariableAction0.Checked == true)
            {
                nudVariableData0.Enabled = true;
                nudVariableData0.Value = 0;
                nudVariableData1.Enabled = false;
                nudVariableData1.Value = 0;
                nudVariableData2.Enabled = false;
                nudVariableData2.Value = 0;
                nudVariableData3.Value = 0;
                nudVariableData4.Enabled = false;
                nudVariableData4.Value = 0;
            }
        }

        private void OptVariableAction1_CheckedChanged(object sender, EventArgs e)
        {
            if (optVariableAction1.Checked == true)
            {
                nudVariableData0.Enabled = false;
                nudVariableData0.Value = 0;
                nudVariableData1.Enabled = true;
                nudVariableData1.Value = 0;
                nudVariableData2.Enabled = false;
                nudVariableData2.Value = 0;
                nudVariableData3.Enabled = false;
                nudVariableData3.Value = 0;
                nudVariableData4.Enabled = false;
                nudVariableData4.Value = 0;
            }
        }

        private void OptVariableAction2_CheckedChanged(object sender, EventArgs e)
        {
            if (optVariableAction2.Checked == true)
            {
                nudVariableData0.Enabled = false;
                nudVariableData0.Value = 0;
                nudVariableData1.Enabled = false;
                nudVariableData1.Value = 0;
                nudVariableData2.Enabled = true;
                nudVariableData2.Value = 0;
                nudVariableData3.Enabled = false;
                nudVariableData3.Value = 0;
                nudVariableData4.Enabled = false;
                nudVariableData4.Value = 0;
            }
        }

        private void OptVariableAction3_CheckedChanged(object sender, EventArgs e)
        {
            if (optVariableAction2.Checked == true)
            {
                nudVariableData0.Enabled = false;
                nudVariableData0.Value = 0;
                nudVariableData1.Enabled = false;
                nudVariableData1.Value = 0;
                nudVariableData2.Enabled = false;
                nudVariableData2.Value = 0;
                nudVariableData3.Enabled = true;
                nudVariableData3.Value = 0;
                nudVariableData4.Enabled = true;
                nudVariableData4.Value = 0;
            }
        }

        private void BtnPlayerVarOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ModifyVariable);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlayerVariable.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnPlayerVarCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlayerVariable.Visible = false;
        }

        #endregion

        #region Set Player Switch

        private void BtnSetPlayerSwitchOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ModifySwitch);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlayerSwitch.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSetPlayerSwitchCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlayerSwitch.Visible = false;
        }

        #endregion

        #region Set Self Switch

        private void BtnSelfswitchOk_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ModifySelfSwitch);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSetSelfSwitch.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSelfswitchCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSetSelfSwitch.Visible = false;
        }

        #endregion

        #region Conditional Branch

        private void OptCondition_Index0_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition0.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_PlayerVarIndex.Enabled = true;
            cmbCondition_PlayerVarCompare.Enabled = true;
            nudCondition_PlayerVarCondition.Enabled = true;
        }

        private void OptCondition1_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition1.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_PlayerSwitch.Enabled = true;
            cmbCondtion_PlayerSwitchCondition.Enabled = true;
        }

        private void OptCondition2_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition2.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_HasItem.Enabled = true;
            nudCondition_HasItem.Enabled = true;
        }

        private void OptCondition3_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition3.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_JobIs.Enabled = true;
        }

        private void OptCondition4_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition4.Checked)
                return;

            cmbCondition_LearntSkill.Enabled = true;
        }

        private void OptCondition5_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition5.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_LevelCompare.Enabled = true;
            nudCondition_LevelAmount.Enabled = true;
        }

        private void OptCondition6_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition6.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_SelfSwitch.Enabled = true;
            cmbCondition_SelfSwitchCondition.Enabled = true;
        }

        private void OptCondition8_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition8.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_Gender.Enabled = true;
        }

        private void OptCondition9_CheckedChanged(object sender, EventArgs e)
        {
            if (!optCondition9.Checked)
                return;

            ClearConditionFrame();

            cmbCondition_Time.Enabled = true;
        }

        private void BtnConditionalBranchOk_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ConditionalBranch);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraCommands.Visible = false;
            fraConditionalBranch.Visible = false;
        }

        private void BtnConditionalBranchCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraConditionalBranch.Visible = false;
        }

        #endregion

        #region Create Label

        private void BtnCreatelabelOk_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.Label);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraCreateLabel.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnCreateLabelCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraCreateLabel.Visible = false;
        }

        #endregion

        #region GoTo Label

        private void BtnGoToLabelOk_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.GoToLabel);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraGoToLabel.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnGoToLabelCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraGoToLabel.Visible = false;
        }

        #endregion

        #region Change Items

        private void BtnChangeItemsOk_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeItems);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraCommands.Visible = false;
            fraChangeItems.Visible = false;
        }

        private void BtnChangeItemsCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeItems.Visible = false;
        }

        #endregion

        #region Change Level

        private void BtnChangeLevelOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeLevel);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangeLevel.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangeLevelCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeLevel.Visible = false;
        }

        #endregion

        #region Change Skills

        private void BtnChangeSkillsOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeSkills);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangeSkills.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangeSkillsCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeSkills.Visible = false;
        }

        #endregion

        #region Change Job

        private void BtnChangeJobOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeJob);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangeJob.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangeJobCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeJob.Visible = false;
        }

        #endregion

        #region Change Sprite

        private void BtnChangeSpriteOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeSprite);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangeSprite.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangeSpriteCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeSprite.Visible = false;
        }

        #endregion

        #region Change Gender

        private void BtnChangeGenderOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.ChangeSex);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangeGender.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangeGenderCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangeGender.Visible = false;
        }

        #endregion

        #region Change PK

        private void BtnChangePkOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.SetPlayerKillable);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraChangePK.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnChangePkCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraChangePK.Visible = false;
        }

        #endregion

        #region Give Exp

        private void BtnGiveExpOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.GiveExperience);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraGiveExp.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnGiveExpCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraGiveExp.Visible = false;
        }

        #endregion

        #region Player Warp

        private void BtnPlayerWarpOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.WarpPlayer);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlayerWarp.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnPlayerWarpCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlayerWarp.Visible = false;
        }

        #endregion

        #region Route Completion

        private void BtnMoveWaitOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.WaitMovementCompletion);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraMoveRouteWait.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnMoveWaitCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraMoveRouteWait.Visible = false;
        }

        #endregion

        #region Spawn Npc

        private void BtnSpawnNpcOK_Click(object sender, EventArgs e)
        {
            if (Event.IsEdit == false)
            {
                Event.AddCommand((int)EventCommand.SpawnNpc);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSpawnNpc.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSpawnNpcancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSpawnNpc.Visible = false;
        }

        #endregion

        #region Play Animation

        private void OptPlayAnimPlayer_CheckedChanged(object sender, EventArgs e)
        {
            lblPlayAnimX.Visible = false;
            lblPlayAnimY.Visible = false;
            nudPlayAnimTileX.Visible = false;
            nudPlayAnimTileY.Visible = false;
            cmbPlayAnimEvent.Visible = false;
        }

        private void OptPlayAnimEvent_CheckedChanged(object sender, EventArgs e)
        {
            lblPlayAnimX.Visible = false;
            lblPlayAnimY.Visible = false;
            nudPlayAnimTileX.Visible = false;
            nudPlayAnimTileY.Visible = false;
            cmbPlayAnimEvent.Visible = true;
        }

        private void OptPlayAnimTile_CheckedChanged(object sender, EventArgs e)
        {
            lblPlayAnimX.Visible = true;
            lblPlayAnimY.Visible = true;
            nudPlayAnimTileX.Visible = true;
            nudPlayAnimTileY.Visible = true;
            cmbPlayAnimEvent.Visible = false;
        }

        private void BtnPlayAnimationOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.PlayAnimation);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlayAnimation.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnPlayAnimationCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlayAnimation.Visible = false;
        }

        #endregion

        #region Set Fog

        private void BtnSetFogOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.SetFog);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSetFog.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSetFogCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSetFog.Visible = false;
        }

        #endregion

        #region Set Weather

        private void BtnSetWeatherOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.SetWeather);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSetWeather.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSetWeatherCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSetWeather.Visible = false;
        }

        #endregion

        #region Set Map Tint

        private void BtnMapTintOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.SetScreenTint);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraMapTint.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnMapTintCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraMapTint.Visible = false;
        }

        #endregion

        #region Play BGM

        private void BtnPlayBgmOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.PlayBgm);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlayBGM.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnPlayBgmCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlayBGM.Visible = false;
        }

        #endregion

        #region Play Sound

        private void BtnPlaySoundOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.PlaySound);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraPlaySound.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnPlaySoundCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraPlaySound.Visible = false;
        }

        #endregion

        #region Wait

        private void BtnSetWaitOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.Wait);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSetWait.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSetWaitCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSetWait.Visible = false;
        }

        #endregion

        #region Set Access

        private void BtnSetAccessOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.SetAccessLevel);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraSetAccess.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnSetAccessCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraSetAccess.Visible = false;
        }

        #endregion

        #region Show Pic

        private void BtnShowPicOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.ShowPicture);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraShowPic.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnShowPicCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraShowPic.Visible = false;
        }

        private void nudShowPicture_Click(object sender, EventArgs e)
        {
            DrawPicture();
        }

        private void DrawPicture()
        {
            int Sprite;

            Sprite = (int)Math.Round(nudShowPicture.Value);

            if (Sprite < 1 | Sprite > GameState.NumPictures)
            {
                picShowPic.Image = null;
                return;
            }

            if (File.Exists(System.IO.Path.Combine(DataPath.Pictures, Sprite + GameState.GfxExt)))
            {
                var bmpPath = System.IO.Path.Combine(DataPath.Pictures, Sprite + GameState.GfxExt);
                try
                {
                    var bmp = new Eto.Drawing.Bitmap(bmpPath);
                    picShowPic.Image = bmp;
                }
                catch
                {
                    picShowPic.Image = null;
                }
            }
        }

        #endregion

        #region Open Shop

        private void BtnOpenShopOK_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
            {
                Event.AddCommand((int)EventCommand.OpenShop);
            }
            else
            {
                Event.EditCommand();
            }
            // hide
            fraDialogue.Visible = false;
            fraOpenShop.Visible = false;
            fraCommands.Visible = false;
        }

        private void BtnOpenShopCancel_Click(object sender, EventArgs e)
        {
            if (!Event.IsEdit)
                fraCommands.Visible = true;
            else
                fraCommands.Visible = false;
            fraDialogue.Visible = false;
            fraOpenShop.Visible = false;
        }

        #endregion
        #endregion
    }
}
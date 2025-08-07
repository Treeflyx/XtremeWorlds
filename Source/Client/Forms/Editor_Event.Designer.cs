using DarkUI.Controls;
using DarkUI.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Editor_Event : DarkForm
    {

        // Shared instance of the form
        private static Editor_Event _instance;

        // Public property to get the shared instance
        public static Editor_Event Instance
        {
            get
            {
                // Create a new instance if one does not exist or if it has been disposed
                if (_instance is null || _instance.IsDisposed)
                {
                    _instance = new Editor_Event();
                }
                return _instance;
            }
        }

        // Private constructor to prevent instantiation from outside
        private Editor_Event()
        {
            InitializeComponent(); // Call to initialize the form's controls
        }

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing & components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            TreeNode treeNode1 = new TreeNode("Show Text");
            TreeNode treeNode2 = new TreeNode("Show Choices");
            TreeNode treeNode3 = new TreeNode("Add Chatbox Text");
            TreeNode treeNode4 = new TreeNode("Show ChatBubble");
            TreeNode treeNode5 = new TreeNode("Messages", new TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4 });
            TreeNode treeNode6 = new TreeNode("Set Player Variable");
            TreeNode treeNode7 = new TreeNode("Set Player Switch");
            TreeNode treeNode8 = new TreeNode("Set Self Switch");
            TreeNode treeNode9 = new TreeNode("Event Processing", new TreeNode[] { treeNode6, treeNode7, treeNode8 });
            TreeNode treeNode10 = new TreeNode("Conditional Branch");
            TreeNode treeNode11 = new TreeNode("Stop Event Processing");
            TreeNode treeNode12 = new TreeNode("Label");
            TreeNode treeNode13 = new TreeNode("GoTo Label");
            TreeNode treeNode14 = new TreeNode("Flow Control", new TreeNode[] { treeNode10, treeNode11, treeNode12, treeNode13 });
            TreeNode treeNode15 = new TreeNode("Change Items");
            TreeNode treeNode16 = new TreeNode("Restore HP");
            TreeNode treeNode17 = new TreeNode("Restore MP");
            TreeNode treeNode18 = new TreeNode("Level Up");
            TreeNode treeNode19 = new TreeNode("Change Level");
            TreeNode treeNode20 = new TreeNode("Change Skills");
            TreeNode treeNode21 = new TreeNode("Change Job");
            TreeNode treeNode22 = new TreeNode("Change Sprite");
            TreeNode treeNode23 = new TreeNode("Change Gender");
            TreeNode treeNode24 = new TreeNode("Change PK");
            TreeNode treeNode25 = new TreeNode("Give Experience");
            TreeNode treeNode26 = new TreeNode("Player Options", new TreeNode[] { treeNode15, treeNode16, treeNode17, treeNode18, treeNode19, treeNode20, treeNode21, treeNode22, treeNode23, treeNode24, treeNode25 });
            TreeNode treeNode27 = new TreeNode("Warp Player");
            TreeNode treeNode28 = new TreeNode("Set Move Route");
            TreeNode treeNode29 = new TreeNode("Wait for Route Completion");
            TreeNode treeNode30 = new TreeNode("Force Spawn Npc");
            TreeNode treeNode31 = new TreeNode("Hold Player");
            TreeNode treeNode32 = new TreeNode("Release Player");
            TreeNode treeNode33 = new TreeNode("Movement", new TreeNode[] { treeNode27, treeNode28, treeNode29, treeNode30, treeNode31, treeNode32 });
            TreeNode treeNode34 = new TreeNode("Play Animation");
            TreeNode treeNode35 = new TreeNode("Animation", new TreeNode[] { treeNode34 });
            TreeNode treeNode36 = new TreeNode("Begin Quest");
            TreeNode treeNode37 = new TreeNode("Complete Task");
            TreeNode treeNode38 = new TreeNode("End Quest");
            TreeNode treeNode39 = new TreeNode("Questing", new TreeNode[] { treeNode36, treeNode37, treeNode38 });
            TreeNode treeNode40 = new TreeNode("Set Fog");
            TreeNode treeNode41 = new TreeNode("Set Weather");
            TreeNode treeNode42 = new TreeNode("Set Map Tinting");
            TreeNode treeNode43 = new TreeNode("Map Functions", new TreeNode[] { treeNode40, treeNode41, treeNode42 });
            TreeNode treeNode44 = new TreeNode("Play BGM");
            TreeNode treeNode45 = new TreeNode("Stop BGM");
            TreeNode treeNode46 = new TreeNode("Play Sound");
            TreeNode treeNode47 = new TreeNode("Stop Sounds");
            TreeNode treeNode48 = new TreeNode("Music and Sound", new TreeNode[] { treeNode44, treeNode45, treeNode46, treeNode47 });
            TreeNode treeNode49 = new TreeNode("Wait...");
            TreeNode treeNode50 = new TreeNode("Set Access");
            TreeNode treeNode51 = new TreeNode("Custom Script");
            TreeNode treeNode52 = new TreeNode("Etc...", new TreeNode[] { treeNode49, treeNode50, treeNode51 });
            TreeNode treeNode53 = new TreeNode("Open Bank");
            TreeNode treeNode54 = new TreeNode("Open Shop");
            TreeNode treeNode55 = new TreeNode("Shop and Bank", new TreeNode[] { treeNode53, treeNode54 });
            TreeNode treeNode56 = new TreeNode("Fade In");
            TreeNode treeNode57 = new TreeNode("Fade Out");
            TreeNode treeNode58 = new TreeNode("Flash White");
            TreeNode treeNode59 = new TreeNode("Show Picture");
            TreeNode treeNode60 = new TreeNode("Hide Picture");
            TreeNode treeNode61 = new TreeNode("Cutscene Options", new TreeNode[] { treeNode56, treeNode57, treeNode58, treeNode59, treeNode60 });
            tvCommands = new TreeView();
            fraPageSetUp = new DarkGroupBox();
            chkGlobal = new DarkCheckBox();
            btnClearPage = new DarkButton();
            btnDeletePage = new DarkButton();
            btnPastePage = new DarkButton();
            btnCopyPage = new DarkButton();
            btnNewPage = new DarkButton();
            txtName = new DarkTextBox();
            cmbLabel1 = new cmbLabel();
            tabPages = new DarkTabControl();
            TabPage1 = new TabPage();
            pnlTabPage = new Panel();
            DarkGroupBox2 = new DarkGroupBox();
            cmbPositioning = new DarkComboBox();
            fraGraphicPic = new DarkGroupBox();
            picGraphic = new PictureBox();
            DarkGroupBox6 = new DarkGroupBox();
            chkShowName = new DarkCheckBox();
            chkWalkThrough = new DarkCheckBox();
            chkDirFix = new DarkCheckBox();
            chkWalkAnim = new DarkCheckBox();
            DarkGroupBox5 = new DarkGroupBox();
            cmbTrigger = new DarkComboBox();
            DarkGroupBox4 = new DarkGroupBox();
            picGraphicSel = new PictureBox();
            DarkGroupBox3 = new DarkGroupBox();
            cmbLabel7 = new cmbLabel();
            cmbMoveFreq = new DarkComboBox();
            cmbLabel6 = new cmbLabel();
            cmbMoveSpeed = new DarkComboBox();
            btnMoveRoute = new DarkButton();
            cmbMoveType = new DarkComboBox();
            cmbLabel5 = new cmbLabel();
            DarkGroupBox1 = new DarkGroupBox();
            cmbSelfSwitchCompare = new DarkComboBox();
            cmbLabel4 = new cmbLabel();
            cmbSelfSwitch = new DarkComboBox();
            chkSelfSwitch = new DarkCheckBox();
            cmbHasItem = new DarkComboBox();
            chkHasItem = new DarkCheckBox();
            cmbPlayerSwitchCompare = new DarkComboBox();
            cmbLabel3 = new cmbLabel();
            cmbPlayerSwitch = new DarkComboBox();
            chkPlayerSwitch = new DarkCheckBox();
            nudPlayerVariable = new DarkNumericUpDown();
            cmbPlayervarCompare = new DarkComboBox();
            cmbLabel2 = new cmbLabel();
            cmbPlayerVar = new DarkComboBox();
            chkPlayerVar = new DarkCheckBox();
            DarkGroupBox8 = new DarkGroupBox();
            btnClearCommand = new DarkButton();
            btnDeleteCommand = new DarkButton();
            btnEditCommand = new DarkButton();
            btnAddCommand = new DarkButton();
            fraCommands = new Panel();
            lstCommands = new ListBox();
            fraGraphic = new DarkGroupBox();
            btnGraphicOk = new DarkButton();
            btnGraphicCancel = new DarkButton();
            cmbLabel13 = new cmbLabel();
            nudGraphic = new DarkNumericUpDown();
            cmbLabel12 = new cmbLabel();
            cmbGraphic = new DarkComboBox();
            cmbLabel11 = new cmbLabel();
            btnLabeling = new DarkButton();
            btnCancel = new DarkButton();
            btnOk = new DarkButton();
            fraMoveRoute = new DarkGroupBox();
            btnMoveRouteOk = new DarkButton();
            btnMoveRouteCancel = new DarkButton();
            chkRepeatRoute = new DarkCheckBox();
            chkIgnoreMove = new DarkCheckBox();
            DarkGroupBox10 = new DarkGroupBox();
            lstvwMoveRoute = new DarkListView();
            lstMoveRoute = new ListBox();
            cmbEvent = new DarkComboBox();
            ColumnHeader3 = new ColumnHeader();
            ColumnHeader4 = new ColumnHeader();
            pnlGraphicSel = new Panel();
            fraDialogue = new DarkGroupBox();
            fraShowChatBubble = new DarkGroupBox();
            btnShowChatBubbleOk = new DarkButton();
            btnShowChatBubbleCancel = new DarkButton();
            cmbLabel41 = new cmbLabel();
            cmbChatBubbleTarget = new DarkComboBox();
            cmbChatBubbleTargetType = new DarkComboBox();
            cmbLabel40 = new cmbLabel();
            txtChatbubbleText = new DarkTextBox();
            cmbLabel39 = new cmbLabel();
            fraOpenShop = new DarkGroupBox();
            btnOpenShopOk = new DarkButton();
            btnOpenShopCancel = new DarkButton();
            cmbOpenShop = new DarkComboBox();
            fraSetSelfSwitch = new DarkGroupBox();
            btnSelfswitchOk = new DarkButton();
            btnSelfswitchCancel = new DarkButton();
            cmbLabel47 = new cmbLabel();
            cmbSetSelfSwitchTo = new DarkComboBox();
            cmbLabel46 = new cmbLabel();
            cmbSetSelfSwitch = new DarkComboBox();
            fraPlaySound = new DarkGroupBox();
            btnPlaySoundOk = new DarkButton();
            btnPlaySoundCancel = new DarkButton();
            cmbPlaySound = new DarkComboBox();
            fraChangePK = new DarkGroupBox();
            btnChangePkOk = new DarkButton();
            btnChangePkCancel = new DarkButton();
            cmbSetPK = new DarkComboBox();
            fraCreateLabel = new DarkGroupBox();
            btnCreatelabelOk = new DarkButton();
            btnCreatelabelCancel = new DarkButton();
            txtLabelName = new DarkTextBox();
            lblLabelName = new cmbLabel();
            fraChangeJob = new DarkGroupBox();
            btnChangeJobOk = new DarkButton();
            btnChangeJobCancel = new DarkButton();
            cmbChangeJob = new DarkComboBox();
            cmbLabel38 = new cmbLabel();
            fraChangeSkills = new DarkGroupBox();
            btnChangeSkillsOk = new DarkButton();
            btnChangeSkillsCancel = new DarkButton();
            optChangeSkillsRemove = new DarkRadioButton();
            optChangeSkillsAdd = new DarkRadioButton();
            cmbChangeSkills = new DarkComboBox();
            cmbLabel37 = new cmbLabel();
            fraPlayerSwitch = new DarkGroupBox();
            btnSetPlayerSwitchOk = new DarkButton();
            btnSetPlayerswitchCancel = new DarkButton();
            cmbPlayerSwitchSet = new DarkComboBox();
            cmbLabel23 = new cmbLabel();
            cmbSwitch = new DarkComboBox();
            cmbLabel22 = new cmbLabel();
            fraSetWait = new DarkGroupBox();
            btnSetWaitOk = new DarkButton();
            btnSetWaitCancel = new DarkButton();
            cmbLabel74 = new cmbLabel();
            cmbLabel72 = new cmbLabel();
            cmbLabel73 = new cmbLabel();
            nudWaitAmount = new DarkNumericUpDown();
            fraMoveRouteWait = new DarkGroupBox();
            btnMoveWaitCancel = new DarkButton();
            btnMoveWaitOk = new DarkButton();
            cmbLabel79 = new cmbLabel();
            cmbMoveWait = new DarkComboBox();
            fraSpawnNpc = new DarkGroupBox();
            btnSpawnNpcOk = new DarkButton();
            btnSpawnNpcancel = new DarkButton();
            cmbSpawnNpc = new DarkComboBox();
            fraSetWeather = new DarkGroupBox();
            btnSetWeatherOk = new DarkButton();
            btnSetWeatherCancel = new DarkButton();
            cmbLabel76 = new cmbLabel();
            nudWeatherIntensity = new DarkNumericUpDown();
            cmbLabel75 = new cmbLabel();
            CmbWeather = new DarkComboBox();
            fraGiveExp = new DarkGroupBox();
            btnGiveExpOk = new DarkButton();
            btnGiveExpCancel = new DarkButton();
            nudGiveExp = new DarkNumericUpDown();
            cmbLabel77 = new cmbLabel();
            fraSetAccess = new DarkGroupBox();
            btnSetAccessOk = new DarkButton();
            btnSetAccessCancel = new DarkButton();
            cmbSetAccess = new DarkComboBox();
            fraChangeGender = new DarkGroupBox();
            btnChangeGenderOk = new DarkButton();
            btnChangeGenderCancel = new DarkButton();
            optChangeSexFemale = new DarkRadioButton();
            optChangeSexMale = new DarkRadioButton();
            fraShowChoices = new DarkGroupBox();
            txtChoices4 = new DarkTextBox();
            txtChoices3 = new DarkTextBox();
            txtChoices2 = new DarkTextBox();
            txtChoices1 = new DarkTextBox();
            cmbLabel56 = new cmbLabel();
            cmbLabel57 = new cmbLabel();
            cmbLabel55 = new cmbLabel();
            cmbLabel54 = new cmbLabel();
            cmbLabel52 = new cmbLabel();
            txtChoicePrompt = new DarkTextBox();
            btnShowChoicesOk = new DarkButton();
            btnShowChoicesCancel = new DarkButton();
            fraChangeLevel = new DarkGroupBox();
            btnChangeLevelOk = new DarkButton();
            btnChangeLevelCancel = new DarkButton();
            cmbLabel65 = new cmbLabel();
            nudChangeLevel = new DarkNumericUpDown();
            fraPlayerVariable = new DarkGroupBox();
            nudVariableData2 = new DarkNumericUpDown();
            optVariableAction2 = new DarkRadioButton();
            btnPlayerVarOk = new DarkButton();
            btnPlayerVarCancel = new DarkButton();
            cmbLabel51 = new cmbLabel();
            cmbLabel50 = new cmbLabel();
            nudVariableData4 = new DarkNumericUpDown();
            nudVariableData3 = new DarkNumericUpDown();
            optVariableAction3 = new DarkRadioButton();
            optVariableAction1 = new DarkRadioButton();
            nudVariableData1 = new DarkNumericUpDown();
            nudVariableData0 = new DarkNumericUpDown();
            optVariableAction0 = new DarkRadioButton();
            cmbVariable = new DarkComboBox();
            cmbLabel49 = new cmbLabel();
            fraPlayAnimation = new DarkGroupBox();
            btnPlayAnimationOk = new DarkButton();
            btnPlayAnimationCancel = new DarkButton();
            lblPlayAnimY = new cmbLabel();
            lblPlayAnimX = new cmbLabel();
            cmbPlayAnimEvent = new DarkComboBox();
            cmbLabel62 = new cmbLabel();
            cmbAnimTargetType = new DarkComboBox();
            nudPlayAnimTileY = new DarkNumericUpDown();
            nudPlayAnimTileX = new DarkNumericUpDown();
            cmbLabel61 = new cmbLabel();
            cmbPlayAnim = new DarkComboBox();
            fraChangeSprite = new DarkGroupBox();
            btnChangeSpriteOk = new DarkButton();
            btnChangeSpriteCancel = new DarkButton();
            cmbLabel48 = new cmbLabel();
            nudChangeSprite = new DarkNumericUpDown();
            picChangeSprite = new PictureBox();
            fraGoToLabel = new DarkGroupBox();
            btnGoToLabelOk = new DarkButton();
            btnGoToLabelCancel = new DarkButton();
            txtGoToLabel = new DarkTextBox();
            cmbLabel60 = new cmbLabel();
            fraMapTint = new DarkGroupBox();
            btnMapTintOk = new DarkButton();
            btnMapTintCancel = new DarkButton();
            cmbLabel42 = new cmbLabel();
            nudMapTintData3 = new DarkNumericUpDown();
            nudMapTintData2 = new DarkNumericUpDown();
            cmbLabel43 = new cmbLabel();
            cmbLabel44 = new cmbLabel();
            nudMapTintData1 = new DarkNumericUpDown();
            nudMapTintData0 = new DarkNumericUpDown();
            cmbLabel45 = new cmbLabel();
            fraShowPic = new DarkGroupBox();
            btnShowPicOk = new DarkButton();
            btnShowPicCancel = new DarkButton();
            cmbLabel71 = new cmbLabel();
            cmbLabel70 = new cmbLabel();
            cmbLabel67 = new cmbLabel();
            cmbLabel68 = new cmbLabel();
            nudPicOffsetY = new DarkNumericUpDown();
            nudPicOffsetX = new DarkNumericUpDown();
            cmbLabel69 = new cmbLabel();
            cmbPicLoc = new DarkComboBox();
            nudShowPicture = new DarkNumericUpDown();
            picShowPic = new PictureBox();
            fraConditionalBranch = new DarkGroupBox();
            cmbCondition_Time = new DarkComboBox();
            optCondition9 = new DarkRadioButton();
            btnConditionalBranchOk = new DarkButton();
            btnConditionalBranchCancel = new DarkButton();
            cmbCondition_Gender = new DarkComboBox();
            optCondition8 = new DarkRadioButton();
            cmbCondition_SelfSwitchCondition = new DarkComboBox();
            cmbLabel17 = new cmbLabel();
            cmbCondition_SelfSwitch = new DarkComboBox();
            optCondition6 = new DarkRadioButton();
            nudCondition_LevelAmount = new DarkNumericUpDown();
            optCondition5 = new DarkRadioButton();
            cmbCondition_LevelCompare = new DarkComboBox();
            cmbCondition_LearntSkill = new DarkComboBox();
            optCondition4 = new DarkRadioButton();
            cmbCondition_JobIs = new DarkComboBox();
            optCondition3 = new DarkRadioButton();
            nudCondition_HasItem = new DarkNumericUpDown();
            cmbLabel16 = new cmbLabel();
            cmbCondition_HasItem = new DarkComboBox();
            optCondition2 = new DarkRadioButton();
            optCondition1 = new DarkRadioButton();
            cmbLabel15 = new cmbLabel();
            cmbCondtion_PlayerSwitchCondition = new DarkComboBox();
            cmbCondition_PlayerSwitch = new DarkComboBox();
            nudCondition_PlayerVarCondition = new DarkNumericUpDown();
            cmbCondition_PlayerVarCompare = new DarkComboBox();
            cmbLabel14 = new cmbLabel();
            cmbCondition_PlayerVarIndex = new DarkComboBox();
            optCondition0 = new DarkRadioButton();
            fraPlayBGM = new DarkGroupBox();
            btnPlayBgmOk = new DarkButton();
            btnPlayBgmCancel = new DarkButton();
            cmbPlayBGM = new DarkComboBox();
            fraPlayerWarp = new DarkGroupBox();
            btnPlayerWarpOk = new DarkButton();
            btnPlayerWarpCancel = new DarkButton();
            cmbLabel31 = new cmbLabel();
            cmbWarpPlayerDir = new DarkComboBox();
            nudWPY = new DarkNumericUpDown();
            cmbLabel32 = new cmbLabel();
            nudWPX = new DarkNumericUpDown();
            cmbLabel33 = new cmbLabel();
            nudWPMap = new DarkNumericUpDown();
            cmbLabel34 = new cmbLabel();
            fraSetFog = new DarkGroupBox();
            btnSetFogOk = new DarkButton();
            btnSetFogCancel = new DarkButton();
            cmbLabel30 = new cmbLabel();
            cmbLabel29 = new cmbLabel();
            cmbLabel28 = new cmbLabel();
            nudFogData2 = new DarkNumericUpDown();
            nudFogData1 = new DarkNumericUpDown();
            nudFogData0 = new DarkNumericUpDown();
            fraShowText = new DarkGroupBox();
            cmbLabel27 = new cmbLabel();
            txtShowText = new DarkTextBox();
            btnShowTextCancel = new DarkButton();
            btnShowTextOk = new DarkButton();
            fraAddText = new DarkGroupBox();
            btnAddTextOk = new DarkButton();
            btnAddTextCancel = new DarkButton();
            optAddText_Global = new DarkRadioButton();
            optAddText_Map = new DarkRadioButton();
            optAddText_Player = new DarkRadioButton();
            cmbLabel25 = new cmbLabel();
            txtAddText_Text = new DarkTextBox();
            cmbLabel24 = new cmbLabel();
            fraChangeItems = new DarkGroupBox();
            btnChangeItemsOk = new DarkButton();
            btnChangeItemsCancel = new DarkButton();
            nudChangeItemsAmount = new DarkNumericUpDown();
            optChangeItemRemove = new DarkRadioButton();
            optChangeItemAdd = new DarkRadioButton();
            optChangeItemSet = new DarkRadioButton();
            cmbChangeItemIndex = new DarkComboBox();
            cmbLabel21 = new cmbLabel();
            pnlVariableSwitches = new Panel();
            FraRenaming = new DarkGroupBox();
            btnRename_Cancel = new DarkButton();
            btnRename_Ok = new DarkButton();
            fraRandom10 = new DarkGroupBox();
            txtRename = new DarkTextBox();
            lblEditing = new cmbLabel();
            fraLabeling = new DarkGroupBox();
            lstSwitches = new ListBox();
            lstVariables = new ListBox();
            btnLabel_Cancel = new DarkButton();
            lblRandomLabel36 = new cmbLabel();
            btnRenameVariable = new DarkButton();
            lblRandomLabel25 = new cmbLabel();
            btnRenameSwitch = new DarkButton();
            btnLabel_Ok = new DarkButton();
            fraPageSetUp.SuspendLayout();
            tabPages.SuspendLayout();
            pnlTabPage.SuspendLayout();
            DarkGroupBox2.SuspendLayout();
            fraGraphicPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGraphic).BeginInit();
            DarkGroupBox6.SuspendLayout();
            DarkGroupBox5.SuspendLayout();
            DarkGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGraphicSel).BeginInit();
            DarkGroupBox3.SuspendLayout();
            DarkGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayerVariable).BeginInit();
            DarkGroupBox8.SuspendLayout();
            fraCommands.SuspendLayout();
            fraGraphic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudGraphic).BeginInit();
            fraMoveRoute.SuspendLayout();
            DarkGroupBox10.SuspendLayout();
            fraDialogue.SuspendLayout();
            fraShowChatBubble.SuspendLayout();
            fraOpenShop.SuspendLayout();
            fraSetSelfSwitch.SuspendLayout();
            fraPlaySound.SuspendLayout();
            fraChangePK.SuspendLayout();
            fraCreateLabel.SuspendLayout();
            fraChangeJob.SuspendLayout();
            fraChangeSkills.SuspendLayout();
            fraPlayerSwitch.SuspendLayout();
            fraSetWait.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudWaitAmount).BeginInit();
            fraMoveRouteWait.SuspendLayout();
            fraSpawnNpc.SuspendLayout();
            fraSetWeather.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudWeatherIntensity).BeginInit();
            fraGiveExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudGiveExp).BeginInit();
            fraSetAccess.SuspendLayout();
            fraChangeGender.SuspendLayout();
            fraShowChoices.SuspendLayout();
            fraChangeLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeLevel).BeginInit();
            fraPlayerVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudVariableData2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData0).BeginInit();
            fraPlayAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayAnimTileY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPlayAnimTileX).BeginInit();
            fraChangeSprite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeSprite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picChangeSprite).BeginInit();
            fraGoToLabel.SuspendLayout();
            fraMapTint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData0).BeginInit();
            fraShowPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPicOffsetY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPicOffsetX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudShowPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picShowPic).BeginInit();
            fraConditionalBranch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCondition_LevelAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCondition_HasItem).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCondition_PlayerVarCondition).BeginInit();
            fraPlayBGM.SuspendLayout();
            fraPlayerWarp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudWPY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWPX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWPMap).BeginInit();
            fraSetFog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudFogData2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFogData1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFogData0).BeginInit();
            fraShowText.SuspendLayout();
            fraAddText.SuspendLayout();
            fraChangeItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeItemsAmount).BeginInit();
            pnlVariableSwitches.SuspendLayout();
            FraRenaming.SuspendLayout();
            fraRandom10.SuspendLayout();
            fraLabeling.SuspendLayout();
            SuspendLayout();
            // 
            // tvCommands
            // 
            tvCommands.BackColor = Color.FromArgb(45, 45, 48);
            tvCommands.BorderStyle = BorderStyle.FixedSingle;
            tvCommands.ForeColor = Color.Gainsboro;
            tvCommands.Location = new Point(7, 3);
            tvCommands.Margin = new Padding(4, 3, 4, 3);
            tvCommands.Name = "tvCommands";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Show Text";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Show Choices";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Add Chatbox Text";
            treeNode4.Name = "Node5";
            treeNode4.Text = "Show ChatBubble";
            treeNode5.Name = "NodeMessages";
            treeNode5.Text = "Messages";
            treeNode6.Name = "Node1";
            treeNode6.Text = "Set Player Variable";
            treeNode7.Name = "Node2";
            treeNode7.Text = "Set Player Switch";
            treeNode8.Name = "Node3";
            treeNode8.Text = "Set Self Switch";
            treeNode9.Name = "NodeProcessing";
            treeNode9.Text = "Event Processing";
            treeNode10.Name = "Node1";
            treeNode10.Text = "Conditional Branch";
            treeNode11.Name = "Node2";
            treeNode11.Text = "Stop Event Processing";
            treeNode12.Name = "Node3";
            treeNode12.Text = "Label";
            treeNode13.Name = "Node4";
            treeNode13.Text = "GoTo Label";
            treeNode14.Name = "NodeFlowControl";
            treeNode14.Text = "Flow Control";
            treeNode15.Name = "Node1";
            treeNode15.Text = "Change Items";
            treeNode16.Name = "Node2";
            treeNode16.Text = "Restore HP";
            treeNode17.Name = "Node3";
            treeNode17.Text = "Restore MP";
            treeNode18.Name = "Node4";
            treeNode18.Text = "Level Up";
            treeNode19.Name = "Node5";
            treeNode19.Text = "Change Level";
            treeNode20.Name = "Node6";
            treeNode20.Text = "Change Skills";
            treeNode21.Name = "Node7";
            treeNode21.Text = "Change Job";
            treeNode22.Name = "Node8";
            treeNode22.Text = "Change Sprite";
            treeNode23.Name = "Node9";
            treeNode23.Text = "Change Gender";
            treeNode24.Name = "Node10";
            treeNode24.Text = "Change PK";
            treeNode25.Name = "Node11";
            treeNode25.Text = "Give Experience";
            treeNode26.Name = "NodePlayerOptions";
            treeNode26.Text = "Player Options";
            treeNode27.Name = "Node1";
            treeNode27.Text = "Warp Player";
            treeNode28.Name = "Node2";
            treeNode28.Text = "Set Move Route";
            treeNode29.Name = "Node3";
            treeNode29.Text = "Wait for Route Completion";
            treeNode30.Name = "Node4";
            treeNode30.Text = "Force Spawn Npc";
            treeNode31.Name = "Node5";
            treeNode31.Text = "Hold Player";
            treeNode32.Name = "Node6";
            treeNode32.Text = "Release Player";
            treeNode33.Name = "NodeMovement";
            treeNode33.Text = "Movement";
            treeNode34.Name = "Node1";
            treeNode34.Text = "Play Animation";
            treeNode35.Name = "NodeAnimation";
            treeNode35.Text = "Animation";
            treeNode36.Name = "Node1";
            treeNode36.Text = "Begin Quest";
            treeNode37.Name = "Node2";
            treeNode37.Text = "Complete Task";
            treeNode38.Name = "Node3";
            treeNode38.Text = "End Quest";
            treeNode39.Name = "NodeQuesting";
            treeNode39.Text = "Questing";
            treeNode40.Name = "Node1";
            treeNode40.Text = "Set Fog";
            treeNode41.Name = "Node2";
            treeNode41.Text = "Set Weather";
            treeNode42.Name = "Node3";
            treeNode42.Text = "Set Map Tinting";
            treeNode43.Name = "NodeMapFunctions";
            treeNode43.Text = "Map Functions";
            treeNode44.Name = "Node1";
            treeNode44.Text = "Play BGM";
            treeNode45.Name = "Node2";
            treeNode45.Text = "Stop BGM";
            treeNode46.Name = "Node3";
            treeNode46.Text = "Play Sound";
            treeNode47.Name = "Node4";
            treeNode47.Text = "Stop Sounds";
            treeNode48.Name = "NodeSound";
            treeNode48.Text = "Music and Sound";
            treeNode49.Name = "Node1";
            treeNode49.Text = "Wait...";
            treeNode50.Name = "Node2";
            treeNode50.Text = "Set Access";
            treeNode51.Name = "Node3";
            treeNode51.Text = "Custom Script";
            treeNode52.Name = "NodeEtc";
            treeNode52.Text = "Etc...";
            treeNode53.Name = "Node1";
            treeNode53.Text = "Open Bank";
            treeNode54.Name = "Node2";
            treeNode54.Text = "Open Shop";
            treeNode55.Name = "NodeShopBank";
            treeNode55.Text = "Shop and Bank";
            treeNode56.Name = "Node1";
            treeNode56.Text = "Fade In";
            treeNode57.Name = "Node2";
            treeNode57.Text = "Fade Out";
            treeNode58.Name = "Node12";
            treeNode58.Text = "Flash White";
            treeNode59.Name = "Node13";
            treeNode59.Text = "Show Picture";
            treeNode60.Name = "Node14";
            treeNode60.Text = "Hide Picture";
            treeNode61.Name = "Node0";
            treeNode61.Text = "Cutscene Options";
            tvCommands.Nodes.AddRange(new TreeNode[] { treeNode5, treeNode9, treeNode14, treeNode26, treeNode33, treeNode35, treeNode39, treeNode43, treeNode48, treeNode52, treeNode55, treeNode61 });
            tvCommands.Size = new Size(444, 511);
            tvCommands.TabIndex = 1;
            tvCommands.AfterSelect += TvCommands_AfterSelect;
            // 
            // fraPageSetUp
            // 
            fraPageSetUp.BackColor = Color.FromArgb(45, 45, 48);
            fraPageSetUp.BorderColor = Color.FromArgb(90, 90, 90);
            fraPageSetUp.Controls.Add(chkGlobal);
            fraPageSetUp.Controls.Add(btnClearPage);
            fraPageSetUp.Controls.Add(btnDeletePage);
            fraPageSetUp.Controls.Add(btnPastePage);
            fraPageSetUp.Controls.Add(btnCopyPage);
            fraPageSetUp.Controls.Add(btnNewPage);
            fraPageSetUp.Controls.Add(txtName);
            fraPageSetUp.Controls.Add(cmbLabel1);
            fraPageSetUp.ForeColor = Color.Gainsboro;
            fraPageSetUp.Location = new Point(4, 3);
            fraPageSetUp.Margin = new Padding(4, 3, 4, 3);
            fraPageSetUp.Name = "fraPageSetUp";
            fraPageSetUp.Padding = new Padding(4, 3, 4, 3);
            fraPageSetUp.Size = new Size(923, 58);
            fraPageSetUp.TabIndex = 2;
            fraPageSetUp.TabStop = false;
            fraPageSetUp.Text = "General";
            // 
            // chkGlobal
            // 
            chkGlobal.AutoSize = true;
            chkGlobal.Location = new Point(327, 23);
            chkGlobal.Margin = new Padding(4, 3, 4, 3);
            chkGlobal.Name = "chkGlobal";
            chkGlobal.Size = new Size(92, 19);
            chkGlobal.TabIndex = 7;
            chkGlobal.Text = "Global Event";
            chkGlobal.CheckedChanged += ChkGlobal_CheckedChanged;
            // 
            // btnClearPage
            // 
            btnClearPage.Location = new Point(825, 18);
            btnClearPage.Margin = new Padding(4, 3, 4, 3);
            btnClearPage.Name = "btnClearPage";
            btnClearPage.Padding = new Padding(6);
            btnClearPage.Size = new Size(88, 27);
            btnClearPage.TabIndex = 6;
            btnClearPage.Text = "Clear Page";
            btnClearPage.Click += BtnClearPage_Click;
            // 
            // btnDeletePage
            // 
            btnDeletePage.Location = new Point(726, 18);
            btnDeletePage.Margin = new Padding(4, 3, 4, 3);
            btnDeletePage.Name = "btnDeletePage";
            btnDeletePage.Padding = new Padding(6);
            btnDeletePage.Size = new Size(92, 27);
            btnDeletePage.TabIndex = 5;
            btnDeletePage.Text = "Delete Page";
            btnDeletePage.Click += BtnDeletePage_Click;
            // 
            // btnPastePage
            // 
            btnPastePage.Location = new Point(631, 18);
            btnPastePage.Margin = new Padding(4, 3, 4, 3);
            btnPastePage.Name = "btnPastePage";
            btnPastePage.Padding = new Padding(6);
            btnPastePage.Size = new Size(88, 27);
            btnPastePage.TabIndex = 4;
            btnPastePage.Text = "Paste Page";
            btnPastePage.Click += BtnPastePage_Click;
            // 
            // btnCopyPage
            // 
            btnCopyPage.Location = new Point(537, 18);
            btnCopyPage.Margin = new Padding(4, 3, 4, 3);
            btnCopyPage.Name = "btnCopyPage";
            btnCopyPage.Padding = new Padding(6);
            btnCopyPage.Size = new Size(88, 27);
            btnCopyPage.TabIndex = 3;
            btnCopyPage.Text = "Copy Page";
            btnCopyPage.Click += BtnCopyPage_Click;
            // 
            // btnNewPage
            // 
            btnNewPage.Location = new Point(442, 18);
            btnNewPage.Margin = new Padding(4, 3, 4, 3);
            btnNewPage.Name = "btnNewPage";
            btnNewPage.Padding = new Padding(6);
            btnNewPage.Size = new Size(88, 27);
            btnNewPage.TabIndex = 2;
            btnNewPage.Text = "New Page";
            btnNewPage.Click += BtnNewPage_Click;
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(69, 73, 74);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(220, 220, 220);
            txtName.Location = new Point(98, 22);
            txtName.Margin = new Padding(4, 3, 4, 3);
            txtName.Name = "txtName";
            txtName.Size = new Size(221, 23);
            txtName.TabIndex = 1;
            txtName.TextChanged += TxtName_TextChanged;
            // 
            // cmbLabel1
            // 
            cmbLabel1.AutoSize = true;
            cmbLabel1.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel1.Location = new Point(10, 24);
            cmbLabel1.Margin = new Padding(4, 0, 4, 0);
            cmbLabel1.Name = "cmbLabel1";
            cmbLabel1.Size = new Size(74, 15);
            cmbLabel1.TabIndex = 0;
            cmbLabel1.Text = "Event Name:";
            // 
            // tabPages
            // 
            tabPages.Controls.Add(TabPage1);
            tabPages.Location = new Point(14, 68);
            tabPages.Margin = new Padding(4, 3, 4, 3);
            tabPages.Name = "tabPages";
            tabPages.SelectedIndex = 0;
            tabPages.Size = new Size(827, 22);
            tabPages.TabIndex = 3;
            tabPages.Click += TabPages_Click;
            // 
            // TabPage1
            // 
            TabPage1.BackColor = Color.DimGray;
            TabPage1.Location = new Point(4, 24);
            TabPage1.Margin = new Padding(4, 3, 4, 3);
            TabPage1.Name = "TabPage1";
            TabPage1.Padding = new Padding(4, 3, 4, 3);
            TabPage1.Size = new Size(819, 0);
            TabPage1.TabIndex = 0;
            TabPage1.Text = "1";
            TabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlTabPage
            // 
            pnlTabPage.Controls.Add(DarkGroupBox2);
            pnlTabPage.Controls.Add(fraGraphicPic);
            pnlTabPage.Controls.Add(DarkGroupBox6);
            pnlTabPage.Controls.Add(DarkGroupBox5);
            pnlTabPage.Controls.Add(DarkGroupBox4);
            pnlTabPage.Controls.Add(DarkGroupBox3);
            pnlTabPage.Controls.Add(DarkGroupBox1);
            pnlTabPage.Controls.Add(DarkGroupBox8);
            pnlTabPage.Controls.Add(fraCommands);
            pnlTabPage.Controls.Add(lstCommands);
            pnlTabPage.Controls.Add(fraGraphic);
            pnlTabPage.Location = new Point(4, 93);
            pnlTabPage.Margin = new Padding(4, 3, 4, 3);
            pnlTabPage.Name = "pnlTabPage";
            pnlTabPage.Size = new Size(923, 573);
            pnlTabPage.TabIndex = 4;
            // 
            // DarkGroupBox2
            // 
            DarkGroupBox2.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox2.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox2.Controls.Add(cmbPositioning);
            DarkGroupBox2.ForeColor = Color.Gainsboro;
            DarkGroupBox2.Location = new Point(214, 440);
            DarkGroupBox2.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Name = "DarkGroupBox2";
            DarkGroupBox2.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox2.Size = new Size(233, 57);
            DarkGroupBox2.TabIndex = 15;
            DarkGroupBox2.TabStop = false;
            DarkGroupBox2.Text = "Poisition";
            // 
            // cmbPositioning
            // 
            cmbPositioning.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPositioning.FormattingEnabled = true;
            cmbPositioning.Items.AddRange(new object[] { "Below Characters", "Same as Characters", "Above Characters" });
            cmbPositioning.Location = new Point(8, 22);
            cmbPositioning.Margin = new Padding(4, 3, 4, 3);
            cmbPositioning.Name = "cmbPositioning";
            cmbPositioning.Size = new Size(220, 24);
            cmbPositioning.TabIndex = 1;
            cmbPositioning.SelectedIndexChanged += CmbPositioning_SelectedIndexChanged;
            // 
            // fraGraphicPic
            // 
            fraGraphicPic.BackColor = Color.FromArgb(45, 45, 48);
            fraGraphicPic.BorderColor = Color.FromArgb(90, 90, 90);
            fraGraphicPic.Controls.Add(picGraphic);
            fraGraphicPic.ForeColor = Color.Gainsboro;
            fraGraphicPic.Location = new Point(4, 156);
            fraGraphicPic.Margin = new Padding(4, 3, 4, 3);
            fraGraphicPic.Name = "fraGraphicPic";
            fraGraphicPic.Padding = new Padding(4, 3, 4, 3);
            fraGraphicPic.Size = new Size(202, 268);
            fraGraphicPic.TabIndex = 12;
            fraGraphicPic.TabStop = false;
            fraGraphicPic.Text = "Graphic";
            // 
            // picGraphic
            // 
            picGraphic.BackgroundImageLayout = ImageLayout.None;
            picGraphic.Location = new Point(8, 22);
            picGraphic.Margin = new Padding(4, 3, 4, 3);
            picGraphic.Name = "picGraphic";
            picGraphic.Size = new Size(188, 239);
            picGraphic.TabIndex = 1;
            picGraphic.TabStop = false;
            picGraphic.Click += PicGraphic_Click;
            // 
            // DarkGroupBox6
            // 
            DarkGroupBox6.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox6.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox6.Controls.Add(chkShowName);
            DarkGroupBox6.Controls.Add(chkWalkThrough);
            DarkGroupBox6.Controls.Add(chkDirFix);
            DarkGroupBox6.Controls.Add(chkWalkAnim);
            DarkGroupBox6.ForeColor = Color.Gainsboro;
            DarkGroupBox6.Location = new Point(4, 430);
            DarkGroupBox6.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox6.Name = "DarkGroupBox6";
            DarkGroupBox6.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox6.Size = new Size(205, 129);
            DarkGroupBox6.TabIndex = 10;
            DarkGroupBox6.TabStop = false;
            DarkGroupBox6.Text = "Options";
            // 
            // chkShowName
            // 
            chkShowName.AutoSize = true;
            chkShowName.Location = new Point(8, 102);
            chkShowName.Margin = new Padding(4, 3, 4, 3);
            chkShowName.Name = "chkShowName";
            chkShowName.Size = new Size(90, 19);
            chkShowName.TabIndex = 3;
            chkShowName.Text = "Show Name";
            chkShowName.CheckedChanged += ChkShowName_CheckedChanged;
            // 
            // chkWalkThrough
            // 
            chkWalkThrough.AutoSize = true;
            chkWalkThrough.Location = new Point(8, 75);
            chkWalkThrough.Margin = new Padding(4, 3, 4, 3);
            chkWalkThrough.Name = "chkWalkThrough";
            chkWalkThrough.Size = new Size(100, 19);
            chkWalkThrough.TabIndex = 2;
            chkWalkThrough.Text = "Walk Through";
            chkWalkThrough.CheckedChanged += ChkWalkThrough_CheckedChanged;
            // 
            // chkDirFix
            // 
            chkDirFix.AutoSize = true;
            chkDirFix.Location = new Point(8, 48);
            chkDirFix.Margin = new Padding(4, 3, 4, 3);
            chkDirFix.Name = "chkDirFix";
            chkDirFix.Size = new Size(105, 19);
            chkDirFix.TabIndex = 1;
            chkDirFix.Text = "Direction Fixed";
            chkDirFix.CheckedChanged += ChkDirFix_CheckedChanged;
            // 
            // chkWalkAnim
            // 
            chkWalkAnim.AutoSize = true;
            chkWalkAnim.Location = new Point(8, 22);
            chkWalkAnim.Margin = new Padding(4, 3, 4, 3);
            chkWalkAnim.Name = "chkWalkAnim";
            chkWalkAnim.Size = new Size(130, 19);
            chkWalkAnim.TabIndex = 0;
            chkWalkAnim.Text = "No Walk Animation";
            chkWalkAnim.CheckedChanged += ChkWalkAnim_CheckedChanged;
            // 
            // DarkGroupBox5
            // 
            DarkGroupBox5.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox5.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox5.Controls.Add(cmbTrigger);
            DarkGroupBox5.ForeColor = Color.Gainsboro;
            DarkGroupBox5.Location = new Point(217, 374);
            DarkGroupBox5.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Name = "DarkGroupBox5";
            DarkGroupBox5.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox5.Size = new Size(233, 57);
            DarkGroupBox5.TabIndex = 4;
            DarkGroupBox5.TabStop = false;
            DarkGroupBox5.Text = "Trigger";
            // 
            // cmbTrigger
            // 
            cmbTrigger.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTrigger.FormattingEnabled = true;
            cmbTrigger.Items.AddRange(new object[] { "Action Button", "Player Touch", "Parallel Process" });
            cmbTrigger.Location = new Point(7, 22);
            cmbTrigger.Margin = new Padding(4, 3, 4, 3);
            cmbTrigger.Name = "cmbTrigger";
            cmbTrigger.Size = new Size(220, 24);
            cmbTrigger.TabIndex = 0;
            cmbTrigger.SelectedIndexChanged += CmbTrigger_SelectedIndexChanged;
            // 
            // DarkGroupBox4
            // 
            DarkGroupBox4.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox4.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox4.Controls.Add(picGraphicSel);
            DarkGroupBox4.ForeColor = Color.Gainsboro;
            DarkGroupBox4.Location = new Point(212, 308);
            DarkGroupBox4.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Name = "DarkGroupBox4";
            DarkGroupBox4.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox4.Size = new Size(233, 55);
            DarkGroupBox4.TabIndex = 3;
            DarkGroupBox4.TabStop = false;
            DarkGroupBox4.Text = "Positioning";
            // 
            // picGraphicSel
            // 
            picGraphicSel.BackgroundImageLayout = ImageLayout.None;
            picGraphicSel.Location = new Point(-220, -338);
            picGraphicSel.Margin = new Padding(4, 3, 4, 3);
            picGraphicSel.Name = "picGraphicSel";
            picGraphicSel.Size = new Size(936, 593);
            picGraphicSel.TabIndex = 5;
            picGraphicSel.TabStop = false;
            picGraphicSel.MouseDown += PicGraphicSel_MouseDown;
            // 
            // DarkGroupBox3
            // 
            DarkGroupBox3.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox3.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox3.Controls.Add(cmbLabel7);
            DarkGroupBox3.Controls.Add(cmbMoveFreq);
            DarkGroupBox3.Controls.Add(cmbLabel6);
            DarkGroupBox3.Controls.Add(cmbMoveSpeed);
            DarkGroupBox3.Controls.Add(btnMoveRoute);
            DarkGroupBox3.Controls.Add(cmbMoveType);
            DarkGroupBox3.Controls.Add(cmbLabel5);
            DarkGroupBox3.ForeColor = Color.Gainsboro;
            DarkGroupBox3.Location = new Point(214, 159);
            DarkGroupBox3.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Name = "DarkGroupBox3";
            DarkGroupBox3.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox3.Size = new Size(233, 142);
            DarkGroupBox3.TabIndex = 2;
            DarkGroupBox3.TabStop = false;
            DarkGroupBox3.Text = "Movement";
            // 
            // cmbLabel7
            // 
            cmbLabel7.AutoSize = true;
            cmbLabel7.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel7.Location = new Point(7, 115);
            cmbLabel7.Margin = new Padding(4, 0, 4, 0);
            cmbLabel7.Name = "cmbLabel7";
            cmbLabel7.Size = new Size(62, 15);
            cmbLabel7.TabIndex = 6;
            cmbLabel7.Text = "Frequency";
            // 
            // cmbMoveFreq
            // 
            cmbMoveFreq.DrawMode = DrawMode.OwnerDrawFixed;
            cmbMoveFreq.FormattingEnabled = true;
            cmbMoveFreq.Items.AddRange(new object[] { "Lowest", "Lower", "Normal", "Higher", "Highest" });
            cmbMoveFreq.Location = new Point(80, 112);
            cmbMoveFreq.Margin = new Padding(4, 3, 4, 3);
            cmbMoveFreq.Name = "cmbMoveFreq";
            cmbMoveFreq.Size = new Size(145, 24);
            cmbMoveFreq.TabIndex = 5;
            cmbMoveFreq.SelectedIndexChanged += CmbMoveFreq_SelectedIndexChanged;
            // 
            // cmbLabel6
            // 
            cmbLabel6.AutoSize = true;
            cmbLabel6.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel6.Location = new Point(7, 84);
            cmbLabel6.Margin = new Padding(4, 0, 4, 0);
            cmbLabel6.Name = "cmbLabel6";
            cmbLabel6.Size = new Size(42, 15);
            cmbLabel6.TabIndex = 4;
            cmbLabel6.Text = "Speed:";
            // 
            // cmbMoveSpeed
            // 
            cmbMoveSpeed.DrawMode = DrawMode.OwnerDrawFixed;
            cmbMoveSpeed.FormattingEnabled = true;
            cmbMoveSpeed.Items.AddRange(new object[] { "8x Slower", "4x Slower", "2x Slower", "Normal", "2x Faster", "4x Faster" });
            cmbMoveSpeed.Location = new Point(80, 81);
            cmbMoveSpeed.Margin = new Padding(4, 3, 4, 3);
            cmbMoveSpeed.Name = "cmbMoveSpeed";
            cmbMoveSpeed.Size = new Size(145, 24);
            cmbMoveSpeed.TabIndex = 3;
            cmbMoveSpeed.SelectedIndexChanged += CmbMoveSpeed_SelectedIndexChanged;
            // 
            // btnMoveRoute
            // 
            btnMoveRoute.Location = new Point(139, 47);
            btnMoveRoute.Margin = new Padding(4, 3, 4, 3);
            btnMoveRoute.Name = "btnMoveRoute";
            btnMoveRoute.Padding = new Padding(6);
            btnMoveRoute.Size = new Size(88, 27);
            btnMoveRoute.TabIndex = 2;
            btnMoveRoute.Text = "Move Route";
            btnMoveRoute.Click += BtnMoveRoute_Click;
            // 
            // cmbMoveType
            // 
            cmbMoveType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbMoveType.FormattingEnabled = true;
            cmbMoveType.Items.AddRange(new object[] { "Fixed Position", "Random", "Move Route" });
            cmbMoveType.Location = new Point(80, 16);
            cmbMoveType.Margin = new Padding(4, 3, 4, 3);
            cmbMoveType.Name = "cmbMoveType";
            cmbMoveType.Size = new Size(145, 24);
            cmbMoveType.TabIndex = 1;
            cmbMoveType.SelectedIndexChanged += CmbMoveType_SelectedIndexChanged;
            // 
            // cmbLabel5
            // 
            cmbLabel5.AutoSize = true;
            cmbLabel5.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel5.Location = new Point(7, 20);
            cmbLabel5.Margin = new Padding(4, 0, 4, 0);
            cmbLabel5.Name = "cmbLabel5";
            cmbLabel5.Size = new Size(34, 15);
            cmbLabel5.TabIndex = 0;
            cmbLabel5.Text = "Type:";
            // 
            // DarkGroupBox1
            // 
            DarkGroupBox1.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox1.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox1.Controls.Add(cmbSelfSwitchCompare);
            DarkGroupBox1.Controls.Add(cmbLabel4);
            DarkGroupBox1.Controls.Add(cmbSelfSwitch);
            DarkGroupBox1.Controls.Add(chkSelfSwitch);
            DarkGroupBox1.Controls.Add(cmbHasItem);
            DarkGroupBox1.Controls.Add(chkHasItem);
            DarkGroupBox1.Controls.Add(cmbPlayerSwitchCompare);
            DarkGroupBox1.Controls.Add(cmbLabel3);
            DarkGroupBox1.Controls.Add(cmbPlayerSwitch);
            DarkGroupBox1.Controls.Add(chkPlayerSwitch);
            DarkGroupBox1.Controls.Add(nudPlayerVariable);
            DarkGroupBox1.Controls.Add(cmbPlayervarCompare);
            DarkGroupBox1.Controls.Add(cmbLabel2);
            DarkGroupBox1.Controls.Add(cmbPlayerVar);
            DarkGroupBox1.Controls.Add(chkPlayerVar);
            DarkGroupBox1.ForeColor = Color.Gainsboro;
            DarkGroupBox1.Location = new Point(4, 7);
            DarkGroupBox1.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Name = "DarkGroupBox1";
            DarkGroupBox1.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox1.Size = new Size(443, 145);
            DarkGroupBox1.TabIndex = 0;
            DarkGroupBox1.TabStop = false;
            DarkGroupBox1.Text = "Conditions";
            // 
            // cmbSelfSwitchCompare
            // 
            cmbSelfSwitchCompare.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSelfSwitchCompare.FormattingEnabled = true;
            cmbSelfSwitchCompare.Items.AddRange(new object[] { "False = 0", "True = 1" });
            cmbSelfSwitchCompare.Location = new Point(260, 113);
            cmbSelfSwitchCompare.Margin = new Padding(4, 3, 4, 3);
            cmbSelfSwitchCompare.Name = "cmbSelfSwitchCompare";
            cmbSelfSwitchCompare.Size = new Size(103, 24);
            cmbSelfSwitchCompare.TabIndex = 14;
            cmbSelfSwitchCompare.SelectedIndexChanged += CmbSelfSwitchCompare_SelectedIndexChanged;
            // 
            // cmbLabel4
            // 
            cmbLabel4.AutoSize = true;
            cmbLabel4.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel4.Location = new Point(237, 117);
            cmbLabel4.Margin = new Padding(4, 0, 4, 0);
            cmbLabel4.Name = "cmbLabel4";
            cmbLabel4.Size = new Size(15, 15);
            cmbLabel4.TabIndex = 13;
            cmbLabel4.Text = "is";
            // 
            // cmbSelfSwitch
            // 
            cmbSelfSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSelfSwitch.FormattingEnabled = true;
            cmbSelfSwitch.Items.AddRange(new object[] { "None", "1 - A", "2 - B", "3 - C", "4 - D" });
            cmbSelfSwitch.Location = new Point(126, 113);
            cmbSelfSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbSelfSwitch.Name = "cmbSelfSwitch";
            cmbSelfSwitch.Size = new Size(103, 24);
            cmbSelfSwitch.TabIndex = 12;
            cmbSelfSwitch.SelectedIndexChanged += CmbSelfSwitch_SelectedIndexChanged;
            // 
            // chkSelfSwitch
            // 
            chkSelfSwitch.AutoSize = true;
            chkSelfSwitch.Location = new Point(7, 115);
            chkSelfSwitch.Margin = new Padding(4, 3, 4, 3);
            chkSelfSwitch.Name = "chkSelfSwitch";
            chkSelfSwitch.Size = new Size(83, 19);
            chkSelfSwitch.TabIndex = 11;
            chkSelfSwitch.Text = "Self Switch";
            chkSelfSwitch.CheckedChanged += ChkSelfSwitch_CheckedChanged;
            // 
            // cmbHasItem
            // 
            cmbHasItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbHasItem.FormattingEnabled = true;
            cmbHasItem.Location = new Point(126, 82);
            cmbHasItem.Margin = new Padding(4, 3, 4, 3);
            cmbHasItem.Name = "cmbHasItem";
            cmbHasItem.Size = new Size(237, 24);
            cmbHasItem.TabIndex = 10;
            cmbHasItem.SelectedIndexChanged += CmbHasItem_SelectedIndexChanged;
            // 
            // chkHasItem
            // 
            chkHasItem.AutoSize = true;
            chkHasItem.Location = new Point(7, 84);
            chkHasItem.Margin = new Padding(4, 3, 4, 3);
            chkHasItem.Name = "chkHasItem";
            chkHasItem.Size = new Size(108, 19);
            chkHasItem.TabIndex = 9;
            chkHasItem.Text = "Player Has Item";
            chkHasItem.CheckedChanged += ChkHasItem_CheckedChanged;
            // 
            // cmbPlayerSwitchCompare
            // 
            cmbPlayerSwitchCompare.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayerSwitchCompare.FormattingEnabled = true;
            cmbPlayerSwitchCompare.Items.AddRange(new object[] { "False = 0", "True = 1" });
            cmbPlayerSwitchCompare.Location = new Point(260, 51);
            cmbPlayerSwitchCompare.Margin = new Padding(4, 3, 4, 3);
            cmbPlayerSwitchCompare.Name = "cmbPlayerSwitchCompare";
            cmbPlayerSwitchCompare.Size = new Size(103, 24);
            cmbPlayerSwitchCompare.TabIndex = 8;
            cmbPlayerSwitchCompare.SelectedIndexChanged += CmbPlayerSwitchCompare_SelectedIndexChanged;
            // 
            // cmbLabel3
            // 
            cmbLabel3.AutoSize = true;
            cmbLabel3.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel3.Location = new Point(237, 54);
            cmbLabel3.Margin = new Padding(4, 0, 4, 0);
            cmbLabel3.Name = "cmbLabel3";
            cmbLabel3.Size = new Size(15, 15);
            cmbLabel3.TabIndex = 7;
            cmbLabel3.Text = "is";
            // 
            // cmbPlayerSwitch
            // 
            cmbPlayerSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayerSwitch.FormattingEnabled = true;
            cmbPlayerSwitch.Location = new Point(126, 51);
            cmbPlayerSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbPlayerSwitch.Name = "cmbPlayerSwitch";
            cmbPlayerSwitch.Size = new Size(103, 24);
            cmbPlayerSwitch.TabIndex = 6;
            cmbPlayerSwitch.SelectedIndexChanged += CmbPlayerSwitch_SelectedIndexChanged;
            // 
            // chkPlayerSwitch
            // 
            chkPlayerSwitch.AutoSize = true;
            chkPlayerSwitch.Location = new Point(7, 53);
            chkPlayerSwitch.Margin = new Padding(4, 3, 4, 3);
            chkPlayerSwitch.Name = "chkPlayerSwitch";
            chkPlayerSwitch.Size = new Size(96, 19);
            chkPlayerSwitch.TabIndex = 5;
            chkPlayerSwitch.Text = "Player Switch";
            chkPlayerSwitch.CheckedChanged += ChkPlayerSwitch_CheckedChanged;
            // 
            // nudPlayerVariable
            // 
            nudPlayerVariable.Location = new Point(371, 21);
            nudPlayerVariable.Margin = new Padding(4, 3, 4, 3);
            nudPlayerVariable.Name = "nudPlayerVariable";
            nudPlayerVariable.Size = new Size(65, 23);
            nudPlayerVariable.TabIndex = 4;
            nudPlayerVariable.ValueChanged += NudPlayerVariable_ValueChanged;
            // 
            // cmbPlayervarCompare
            // 
            cmbPlayervarCompare.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayervarCompare.FormattingEnabled = true;
            cmbPlayervarCompare.Items.AddRange(new object[] { "Equal To", "Great Than Or Equal To", "Less Than or Equal To", "Greater Than", "Less Than", "Does Not Equal" });
            cmbPlayervarCompare.Location = new Point(260, 20);
            cmbPlayervarCompare.Margin = new Padding(4, 3, 4, 3);
            cmbPlayervarCompare.Name = "cmbPlayervarCompare";
            cmbPlayervarCompare.Size = new Size(103, 24);
            cmbPlayervarCompare.TabIndex = 3;
            cmbPlayervarCompare.SelectedIndexChanged += CmbPlayervarCompare_SelectedIndexChanged;
            // 
            // cmbLabel2
            // 
            cmbLabel2.AutoSize = true;
            cmbLabel2.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel2.Location = new Point(237, 27);
            cmbLabel2.Margin = new Padding(4, 0, 4, 0);
            cmbLabel2.Name = "cmbLabel2";
            cmbLabel2.Size = new Size(15, 15);
            cmbLabel2.TabIndex = 2;
            cmbLabel2.Text = "is";
            // 
            // cmbPlayerVar
            // 
            cmbPlayerVar.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayerVar.FormattingEnabled = true;
            cmbPlayerVar.Location = new Point(126, 20);
            cmbPlayerVar.Margin = new Padding(4, 3, 4, 3);
            cmbPlayerVar.Name = "cmbPlayerVar";
            cmbPlayerVar.Size = new Size(103, 24);
            cmbPlayerVar.TabIndex = 1;
            cmbPlayerVar.SelectedIndexChanged += CmbPlayerVar_SelectedIndexChanged;
            // 
            // chkPlayerVar
            // 
            chkPlayerVar.AutoSize = true;
            chkPlayerVar.Location = new Point(7, 22);
            chkPlayerVar.Margin = new Padding(4, 3, 4, 3);
            chkPlayerVar.Name = "chkPlayerVar";
            chkPlayerVar.Size = new Size(102, 19);
            chkPlayerVar.TabIndex = 0;
            chkPlayerVar.Text = "Player Variable";
            chkPlayerVar.CheckedChanged += ChkPlayerVar_CheckedChanged;
            // 
            // DarkGroupBox8
            // 
            DarkGroupBox8.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox8.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox8.Controls.Add(btnClearCommand);
            DarkGroupBox8.Controls.Add(btnDeleteCommand);
            DarkGroupBox8.Controls.Add(btnEditCommand);
            DarkGroupBox8.Controls.Add(btnAddCommand);
            DarkGroupBox8.ForeColor = Color.Gainsboro;
            DarkGroupBox8.Location = new Point(454, 507);
            DarkGroupBox8.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox8.Name = "DarkGroupBox8";
            DarkGroupBox8.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox8.Size = new Size(458, 57);
            DarkGroupBox8.TabIndex = 9;
            DarkGroupBox8.TabStop = false;
            DarkGroupBox8.Text = "Commands";
            // 
            // btnClearCommand
            // 
            btnClearCommand.Location = new Point(364, 22);
            btnClearCommand.Margin = new Padding(4, 3, 4, 3);
            btnClearCommand.Name = "btnClearCommand";
            btnClearCommand.Padding = new Padding(6);
            btnClearCommand.Size = new Size(88, 27);
            btnClearCommand.TabIndex = 3;
            btnClearCommand.Text = "Clear";
            btnClearCommand.Click += BtnClearCommand_Click;
            // 
            // btnDeleteCommand
            // 
            btnDeleteCommand.Location = new Point(247, 22);
            btnDeleteCommand.Margin = new Padding(4, 3, 4, 3);
            btnDeleteCommand.Name = "btnDeleteCommand";
            btnDeleteCommand.Padding = new Padding(6);
            btnDeleteCommand.Size = new Size(88, 27);
            btnDeleteCommand.TabIndex = 2;
            btnDeleteCommand.Text = "Delete";
            btnDeleteCommand.Click += BtnDeleteComand_Click;
            // 
            // btnEditCommand
            // 
            btnEditCommand.Location = new Point(126, 22);
            btnEditCommand.Margin = new Padding(4, 3, 4, 3);
            btnEditCommand.Name = "btnEditCommand";
            btnEditCommand.Padding = new Padding(6);
            btnEditCommand.Size = new Size(88, 27);
            btnEditCommand.TabIndex = 1;
            btnEditCommand.Text = "Edit";
            btnEditCommand.Click += BtnEditCommand_Click;
            // 
            // btnAddCommand
            // 
            btnAddCommand.Location = new Point(7, 22);
            btnAddCommand.Margin = new Padding(4, 3, 4, 3);
            btnAddCommand.Name = "btnAddCommand";
            btnAddCommand.Padding = new Padding(6);
            btnAddCommand.Size = new Size(88, 27);
            btnAddCommand.TabIndex = 0;
            btnAddCommand.Text = "Add";
            btnAddCommand.Click += BtnAddCommand_Click;
            // 
            // fraCommands
            // 
            fraCommands.Controls.Add(tvCommands);
            fraCommands.Location = new Point(454, 7);
            fraCommands.Margin = new Padding(4, 3, 4, 3);
            fraCommands.Name = "fraCommands";
            fraCommands.Size = new Size(458, 556);
            fraCommands.TabIndex = 6;
            fraCommands.Visible = false;
            // 
            // lstCommands
            // 
            lstCommands.BackColor = Color.FromArgb(45, 45, 48);
            lstCommands.BorderStyle = BorderStyle.FixedSingle;
            lstCommands.ForeColor = Color.Gainsboro;
            lstCommands.FormattingEnabled = true;
            lstCommands.Location = new Point(454, 7);
            lstCommands.Margin = new Padding(4, 3, 4, 3);
            lstCommands.Name = "lstCommands";
            lstCommands.Size = new Size(458, 497);
            lstCommands.TabIndex = 8;
            lstCommands.SelectedIndexChanged += LstCommands_SelectedIndexChanged;
            // 
            // fraGraphic
            // 
            fraGraphic.BackColor = Color.FromArgb(45, 45, 48);
            fraGraphic.BorderColor = Color.FromArgb(90, 90, 90);
            fraGraphic.Controls.Add(btnGraphicOk);
            fraGraphic.Controls.Add(btnGraphicCancel);
            fraGraphic.Controls.Add(cmbLabel13);
            fraGraphic.Controls.Add(nudGraphic);
            fraGraphic.Controls.Add(cmbLabel12);
            fraGraphic.Controls.Add(cmbGraphic);
            fraGraphic.Controls.Add(cmbLabel11);
            fraGraphic.ForeColor = Color.Gainsboro;
            fraGraphic.Location = new Point(454, 6);
            fraGraphic.Margin = new Padding(4, 3, 4, 3);
            fraGraphic.Name = "fraGraphic";
            fraGraphic.Padding = new Padding(4, 3, 4, 3);
            fraGraphic.Size = new Size(458, 557);
            fraGraphic.TabIndex = 14;
            fraGraphic.TabStop = false;
            fraGraphic.Text = "Graphic Selection";
            fraGraphic.Visible = false;
            // 
            // btnGraphicOk
            // 
            btnGraphicOk.Location = new Point(761, 658);
            btnGraphicOk.Margin = new Padding(4, 3, 4, 3);
            btnGraphicOk.Name = "btnGraphicOk";
            btnGraphicOk.Padding = new Padding(6);
            btnGraphicOk.Size = new Size(88, 27);
            btnGraphicOk.TabIndex = 8;
            btnGraphicOk.Text = "Ok";
            // 
            // btnGraphicCancel
            // 
            btnGraphicCancel.Location = new Point(855, 658);
            btnGraphicCancel.Margin = new Padding(4, 3, 4, 3);
            btnGraphicCancel.Name = "btnGraphicCancel";
            btnGraphicCancel.Padding = new Padding(6);
            btnGraphicCancel.Size = new Size(88, 27);
            btnGraphicCancel.TabIndex = 7;
            btnGraphicCancel.Text = "Cancel";
            // 
            // cmbLabel13
            // 
            cmbLabel13.AutoSize = true;
            cmbLabel13.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel13.Location = new Point(12, 659);
            cmbLabel13.Margin = new Padding(4, 0, 4, 0);
            cmbLabel13.Name = "cmbLabel13";
            cmbLabel13.Size = new Size(181, 15);
            cmbLabel13.TabIndex = 6;
            cmbLabel13.Text = "Hold Shift to select multiple tiles.";
            // 
            // nudGraphic
            // 
            nudGraphic.Location = new Point(121, 57);
            nudGraphic.Margin = new Padding(4, 3, 4, 3);
            nudGraphic.Name = "nudGraphic";
            nudGraphic.Size = new Size(252, 23);
            nudGraphic.TabIndex = 3;
            nudGraphic.ValueChanged += nudGraphic_ValueChanged;
            // 
            // cmbLabel12
            // 
            cmbLabel12.AutoSize = true;
            cmbLabel12.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel12.Location = new Point(24, 59);
            cmbLabel12.Margin = new Padding(4, 0, 4, 0);
            cmbLabel12.Name = "cmbLabel12";
            cmbLabel12.Size = new Size(54, 15);
            cmbLabel12.TabIndex = 2;
            cmbLabel12.Text = "Number:";
            // 
            // cmbGraphic
            // 
            cmbGraphic.DrawMode = DrawMode.OwnerDrawFixed;
            cmbGraphic.FormattingEnabled = true;
            cmbGraphic.Items.AddRange(new object[] { "None", "Character", "Tileset" });
            cmbGraphic.Location = new Point(121, 21);
            cmbGraphic.Margin = new Padding(4, 3, 4, 3);
            cmbGraphic.Name = "cmbGraphic";
            cmbGraphic.Size = new Size(252, 24);
            cmbGraphic.TabIndex = 1;
            cmbGraphic.SelectedIndexChanged += CmbGraphic_SelectedIndexChanged;
            // 
            // cmbLabel11
            // 
            cmbLabel11.AutoSize = true;
            cmbLabel11.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel11.Location = new Point(24, 24);
            cmbLabel11.Margin = new Padding(4, 0, 4, 0);
            cmbLabel11.Name = "cmbLabel11";
            cmbLabel11.Size = new Size(83, 15);
            cmbLabel11.TabIndex = 0;
            cmbLabel11.Text = "Graphics Type:";
            // 
            // btnLabeling
            // 
            btnLabeling.Location = new Point(7, 658);
            btnLabeling.Margin = new Padding(4, 3, 4, 3);
            btnLabeling.Name = "btnLabeling";
            btnLabeling.Padding = new Padding(6);
            btnLabeling.Size = new Size(205, 27);
            btnLabeling.TabIndex = 6;
            btnLabeling.Text = "Edit Variables/Switches";
            btnLabeling.Click += BtnLabeling_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(828, 663);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(6);
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(734, 663);
            btnOk.Margin = new Padding(4, 3, 4, 3);
            btnOk.Name = "btnOk";
            btnOk.Padding = new Padding(6);
            btnOk.Size = new Size(88, 27);
            btnOk.TabIndex = 8;
            btnOk.Text = "Ok";
            btnOk.Click += BtnOK_Click;
            btnOk.MouseDown += BtnOK_Click;
            // 
            // fraMoveRoute
            // 
            fraMoveRoute.BackColor = Color.FromArgb(45, 45, 48);
            fraMoveRoute.BorderColor = Color.FromArgb(90, 90, 90);
            fraMoveRoute.Controls.Add(btnMoveRouteOk);
            fraMoveRoute.Controls.Add(btnMoveRouteCancel);
            fraMoveRoute.Controls.Add(chkRepeatRoute);
            fraMoveRoute.Controls.Add(chkIgnoreMove);
            fraMoveRoute.Controls.Add(DarkGroupBox10);
            fraMoveRoute.Controls.Add(lstMoveRoute);
            fraMoveRoute.Controls.Add(cmbEvent);
            fraMoveRoute.ForeColor = Color.Gainsboro;
            fraMoveRoute.Location = new Point(933, 14);
            fraMoveRoute.Margin = new Padding(4, 3, 4, 3);
            fraMoveRoute.Name = "fraMoveRoute";
            fraMoveRoute.Padding = new Padding(4, 3, 4, 3);
            fraMoveRoute.Size = new Size(108, 98);
            fraMoveRoute.TabIndex = 0;
            fraMoveRoute.TabStop = false;
            fraMoveRoute.Text = "Move Route";
            fraMoveRoute.Visible = false;
            // 
            // btnMoveRouteOk
            // 
            btnMoveRouteOk.Location = new Point(749, 497);
            btnMoveRouteOk.Margin = new Padding(4, 3, 4, 3);
            btnMoveRouteOk.Name = "btnMoveRouteOk";
            btnMoveRouteOk.Padding = new Padding(6);
            btnMoveRouteOk.Size = new Size(88, 27);
            btnMoveRouteOk.TabIndex = 7;
            btnMoveRouteOk.Text = "Ok";
            btnMoveRouteOk.Click += BtnMoveRouteOk_Click;
            // 
            // btnMoveRouteCancel
            // 
            btnMoveRouteCancel.Location = new Point(844, 497);
            btnMoveRouteCancel.Margin = new Padding(4, 3, 4, 3);
            btnMoveRouteCancel.Name = "btnMoveRouteCancel";
            btnMoveRouteCancel.Padding = new Padding(6);
            btnMoveRouteCancel.Size = new Size(88, 27);
            btnMoveRouteCancel.TabIndex = 6;
            btnMoveRouteCancel.Text = "Cancel";
            btnMoveRouteCancel.Click += BtnMoveRouteCancel_Click;
            // 
            // chkRepeatRoute
            // 
            chkRepeatRoute.AutoSize = true;
            chkRepeatRoute.Location = new Point(7, 524);
            chkRepeatRoute.Margin = new Padding(4, 3, 4, 3);
            chkRepeatRoute.Name = "chkRepeatRoute";
            chkRepeatRoute.Size = new Size(96, 19);
            chkRepeatRoute.TabIndex = 5;
            chkRepeatRoute.Text = "Repeat Route";
            chkRepeatRoute.CheckedChanged += ChkRepeatRoute_CheckedChanged;
            // 
            // chkIgnoreMove
            // 
            chkIgnoreMove.AutoSize = true;
            chkIgnoreMove.Location = new Point(7, 497);
            chkIgnoreMove.Margin = new Padding(4, 3, 4, 3);
            chkIgnoreMove.Name = "chkIgnoreMove";
            chkIgnoreMove.Size = new Size(164, 19);
            chkIgnoreMove.TabIndex = 4;
            chkIgnoreMove.Text = "Ignore if event can't move";
            chkIgnoreMove.CheckedChanged += ChkIgnoreMove_CheckedChanged;
            // 
            // DarkGroupBox10
            // 
            DarkGroupBox10.BackColor = Color.FromArgb(45, 45, 48);
            DarkGroupBox10.BorderColor = Color.FromArgb(90, 90, 90);
            DarkGroupBox10.Controls.Add(lstvwMoveRoute);
            DarkGroupBox10.ForeColor = Color.Gainsboro;
            DarkGroupBox10.Location = new Point(237, 12);
            DarkGroupBox10.Margin = new Padding(4, 3, 4, 3);
            DarkGroupBox10.Name = "DarkGroupBox10";
            DarkGroupBox10.Padding = new Padding(4, 3, 4, 3);
            DarkGroupBox10.Size = new Size(694, 479);
            DarkGroupBox10.TabIndex = 3;
            DarkGroupBox10.TabStop = false;
            DarkGroupBox10.Text = "Commands";
            // 
            // lstvwMoveRoute
            // 
            lstvwMoveRoute.BackColor = Color.DimGray;
            lstvwMoveRoute.Dock = DockStyle.Top;
            lstvwMoveRoute.Font = new Font("Microsoft Sans Serif", 8.25F);
            lstvwMoveRoute.ForeColor = Color.Gainsboro;
            lstvwMoveRoute.Location = new Point(4, 19);
            lstvwMoveRoute.Margin = new Padding(4, 3, 4, 3);
            lstvwMoveRoute.Name = "lstvwMoveRoute";
            lstvwMoveRoute.Size = new Size(686, 458);
            lstvwMoveRoute.TabIndex = 5;
            lstvwMoveRoute.Click += LstvwMoveRoute_SelectedIndexChanged;
            // 
            // lstMoveRoute
            // 
            lstMoveRoute.BackColor = Color.FromArgb(45, 45, 48);
            lstMoveRoute.BorderStyle = BorderStyle.FixedSingle;
            lstMoveRoute.ForeColor = Color.Gainsboro;
            lstMoveRoute.FormattingEnabled = true;
            lstMoveRoute.Location = new Point(7, 53);
            lstMoveRoute.Margin = new Padding(4, 3, 4, 3);
            lstMoveRoute.Name = "lstMoveRoute";
            lstMoveRoute.Size = new Size(222, 437);
            lstMoveRoute.TabIndex = 2;
            lstMoveRoute.KeyDown += LstMoveRoute_KeyDown;
            // 
            // cmbEvent
            // 
            cmbEvent.DrawMode = DrawMode.OwnerDrawFixed;
            cmbEvent.FormattingEnabled = true;
            cmbEvent.Location = new Point(7, 22);
            cmbEvent.Margin = new Padding(4, 3, 4, 3);
            cmbEvent.Name = "cmbEvent";
            cmbEvent.Size = new Size(222, 24);
            cmbEvent.TabIndex = 0;
            // 
            // ColumnHeader3
            // 
            ColumnHeader3.Text = "";
            ColumnHeader3.Width = 150;
            // 
            // ColumnHeader4
            // 
            ColumnHeader4.Text = "";
            ColumnHeader4.Width = 150;
            // 
            // pnlGraphicSel
            // 
            pnlGraphicSel.AutoScroll = true;
            pnlGraphicSel.Location = new Point(4, 92);
            pnlGraphicSel.Margin = new Padding(4, 3, 4, 3);
            pnlGraphicSel.Name = "pnlGraphicSel";
            pnlGraphicSel.Size = new Size(923, 573);
            pnlGraphicSel.TabIndex = 9;
            // 
            // fraDialogue
            // 
            fraDialogue.BackColor = Color.FromArgb(45, 45, 48);
            fraDialogue.BorderColor = Color.FromArgb(90, 90, 90);
            fraDialogue.Controls.Add(fraShowChatBubble);
            fraDialogue.Controls.Add(fraOpenShop);
            fraDialogue.Controls.Add(fraSetSelfSwitch);
            fraDialogue.Controls.Add(fraPlaySound);
            fraDialogue.Controls.Add(fraChangePK);
            fraDialogue.Controls.Add(fraCreateLabel);
            fraDialogue.Controls.Add(fraChangeJob);
            fraDialogue.Controls.Add(fraChangeSkills);
            fraDialogue.Controls.Add(fraPlayerSwitch);
            fraDialogue.Controls.Add(fraSetWait);
            fraDialogue.Controls.Add(fraMoveRouteWait);
            fraDialogue.Controls.Add(fraSpawnNpc);
            fraDialogue.Controls.Add(fraSetWeather);
            fraDialogue.Controls.Add(fraGiveExp);
            fraDialogue.Controls.Add(fraSetAccess);
            fraDialogue.Controls.Add(fraChangeGender);
            fraDialogue.Controls.Add(fraShowChoices);
            fraDialogue.Controls.Add(fraChangeLevel);
            fraDialogue.Controls.Add(fraPlayerVariable);
            fraDialogue.Controls.Add(fraPlayAnimation);
            fraDialogue.Controls.Add(fraChangeSprite);
            fraDialogue.Controls.Add(fraGoToLabel);
            fraDialogue.Controls.Add(fraMapTint);
            fraDialogue.Controls.Add(fraShowPic);
            fraDialogue.Controls.Add(fraConditionalBranch);
            fraDialogue.Controls.Add(fraPlayBGM);
            fraDialogue.Controls.Add(fraPlayerWarp);
            fraDialogue.Controls.Add(fraSetFog);
            fraDialogue.Controls.Add(fraShowText);
            fraDialogue.Controls.Add(fraAddText);
            fraDialogue.Controls.Add(fraChangeItems);
            fraDialogue.ForeColor = Color.Gainsboro;
            fraDialogue.Location = new Point(1056, 14);
            fraDialogue.Margin = new Padding(4, 3, 4, 3);
            fraDialogue.Name = "fraDialogue";
            fraDialogue.Padding = new Padding(4, 3, 4, 3);
            fraDialogue.Size = new Size(776, 687);
            fraDialogue.TabIndex = 10;
            fraDialogue.TabStop = false;
            fraDialogue.Visible = false;
            // 
            // fraShowChatBubble
            // 
            fraShowChatBubble.BackColor = Color.FromArgb(45, 45, 48);
            fraShowChatBubble.BorderColor = Color.FromArgb(90, 90, 90);
            fraShowChatBubble.Controls.Add(btnShowChatBubbleOk);
            fraShowChatBubble.Controls.Add(btnShowChatBubbleCancel);
            fraShowChatBubble.Controls.Add(cmbLabel41);
            fraShowChatBubble.Controls.Add(cmbChatBubbleTarget);
            fraShowChatBubble.Controls.Add(cmbChatBubbleTargetType);
            fraShowChatBubble.Controls.Add(cmbLabel40);
            fraShowChatBubble.Controls.Add(txtChatbubbleText);
            fraShowChatBubble.Controls.Add(cmbLabel39);
            fraShowChatBubble.ForeColor = Color.Gainsboro;
            fraShowChatBubble.Location = new Point(468, 209);
            fraShowChatBubble.Margin = new Padding(4, 3, 4, 3);
            fraShowChatBubble.Name = "fraShowChatBubble";
            fraShowChatBubble.Padding = new Padding(4, 3, 4, 3);
            fraShowChatBubble.Size = new Size(287, 163);
            fraShowChatBubble.TabIndex = 27;
            fraShowChatBubble.TabStop = false;
            fraShowChatBubble.Text = "Show ChatBubble";
            fraShowChatBubble.Visible = false;
            // 
            // btnShowChatBubbleOk
            // 
            btnShowChatBubbleOk.Location = new Point(98, 129);
            btnShowChatBubbleOk.Margin = new Padding(4, 3, 4, 3);
            btnShowChatBubbleOk.Name = "btnShowChatBubbleOk";
            btnShowChatBubbleOk.Padding = new Padding(6);
            btnShowChatBubbleOk.Size = new Size(88, 27);
            btnShowChatBubbleOk.TabIndex = 31;
            btnShowChatBubbleOk.Text = "Ok";
            btnShowChatBubbleOk.Click += BtnShowChatBubbleOK_Click;
            // 
            // btnShowChatBubbleCancel
            // 
            btnShowChatBubbleCancel.Location = new Point(192, 129);
            btnShowChatBubbleCancel.Margin = new Padding(4, 3, 4, 3);
            btnShowChatBubbleCancel.Name = "btnShowChatBubbleCancel";
            btnShowChatBubbleCancel.Padding = new Padding(6);
            btnShowChatBubbleCancel.Size = new Size(88, 27);
            btnShowChatBubbleCancel.TabIndex = 30;
            btnShowChatBubbleCancel.Text = "Cancel";
            btnShowChatBubbleCancel.Click += BtnShowChatBubbleCancel_Click;
            // 
            // cmbLabel41
            // 
            cmbLabel41.AutoSize = true;
            cmbLabel41.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel41.Location = new Point(7, 102);
            cmbLabel41.Margin = new Padding(4, 0, 4, 0);
            cmbLabel41.Name = "cmbLabel41";
            cmbLabel41.Size = new Size(39, 15);
            cmbLabel41.TabIndex = 29;
            cmbLabel41.Text = "Index:";
            // 
            // cmbChatBubbleTarget
            // 
            cmbChatBubbleTarget.DrawMode = DrawMode.OwnerDrawFixed;
            cmbChatBubbleTarget.FormattingEnabled = true;
            cmbChatBubbleTarget.Location = new Point(94, 98);
            cmbChatBubbleTarget.Margin = new Padding(4, 3, 4, 3);
            cmbChatBubbleTarget.Name = "cmbChatBubbleTarget";
            cmbChatBubbleTarget.Size = new Size(185, 24);
            cmbChatBubbleTarget.TabIndex = 28;
            // 
            // cmbChatBubbleTargetType
            // 
            cmbChatBubbleTargetType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbChatBubbleTargetType.FormattingEnabled = true;
            cmbChatBubbleTargetType.Items.AddRange(new object[] { "Player", "Npc", "Event" });
            cmbChatBubbleTargetType.Location = new Point(94, 67);
            cmbChatBubbleTargetType.Margin = new Padding(4, 3, 4, 3);
            cmbChatBubbleTargetType.Name = "cmbChatBubbleTargetType";
            cmbChatBubbleTargetType.Size = new Size(185, 24);
            cmbChatBubbleTargetType.TabIndex = 27;
            cmbChatBubbleTargetType.SelectedIndexChanged += CmbChatBubbleTargetType_SelectedIndexChanged;
            // 
            // cmbLabel40
            // 
            cmbLabel40.AutoSize = true;
            cmbLabel40.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel40.Location = new Point(7, 70);
            cmbLabel40.Margin = new Padding(4, 0, 4, 0);
            cmbLabel40.Name = "cmbLabel40";
            cmbLabel40.Size = new Size(69, 15);
            cmbLabel40.TabIndex = 2;
            cmbLabel40.Text = "Target Type:";
            // 
            // txtChatbubbleText
            // 
            txtChatbubbleText.BackColor = Color.FromArgb(69, 73, 74);
            txtChatbubbleText.BorderStyle = BorderStyle.FixedSingle;
            txtChatbubbleText.ForeColor = Color.FromArgb(220, 220, 220);
            txtChatbubbleText.Location = new Point(7, 37);
            txtChatbubbleText.Margin = new Padding(4, 3, 4, 3);
            txtChatbubbleText.Name = "txtChatbubbleText";
            txtChatbubbleText.Size = new Size(273, 23);
            txtChatbubbleText.TabIndex = 1;
            // 
            // cmbLabel39
            // 
            cmbLabel39.AutoSize = true;
            cmbLabel39.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel39.Location = new Point(7, 18);
            cmbLabel39.Margin = new Padding(4, 0, 4, 0);
            cmbLabel39.Name = "cmbLabel39";
            cmbLabel39.Size = new Size(93, 15);
            cmbLabel39.TabIndex = 0;
            cmbLabel39.Text = "ChatBubble Text";
            // 
            // fraOpenShop
            // 
            fraOpenShop.BackColor = Color.FromArgb(45, 45, 48);
            fraOpenShop.BorderColor = Color.FromArgb(90, 90, 90);
            fraOpenShop.Controls.Add(btnOpenShopOk);
            fraOpenShop.Controls.Add(btnOpenShopCancel);
            fraOpenShop.Controls.Add(cmbOpenShop);
            fraOpenShop.ForeColor = Color.Gainsboro;
            fraOpenShop.Location = new Point(470, 250);
            fraOpenShop.Margin = new Padding(4, 3, 4, 3);
            fraOpenShop.Name = "fraOpenShop";
            fraOpenShop.Padding = new Padding(4, 3, 4, 3);
            fraOpenShop.Size = new Size(287, 91);
            fraOpenShop.TabIndex = 39;
            fraOpenShop.TabStop = false;
            fraOpenShop.Text = "Open Shop";
            fraOpenShop.Visible = false;
            // 
            // btnOpenShopOk
            // 
            btnOpenShopOk.Location = new Point(51, 54);
            btnOpenShopOk.Margin = new Padding(4, 3, 4, 3);
            btnOpenShopOk.Name = "btnOpenShopOk";
            btnOpenShopOk.Padding = new Padding(6);
            btnOpenShopOk.Size = new Size(88, 27);
            btnOpenShopOk.TabIndex = 27;
            btnOpenShopOk.Text = "Ok";
            btnOpenShopOk.Click += BtnOpenShopOK_Click;
            // 
            // btnOpenShopCancel
            // 
            btnOpenShopCancel.Location = new Point(146, 54);
            btnOpenShopCancel.Margin = new Padding(4, 3, 4, 3);
            btnOpenShopCancel.Name = "btnOpenShopCancel";
            btnOpenShopCancel.Padding = new Padding(6);
            btnOpenShopCancel.Size = new Size(88, 27);
            btnOpenShopCancel.TabIndex = 26;
            btnOpenShopCancel.Text = "Cancel";
            btnOpenShopCancel.Click += BtnOpenShopCancel_Click;
            // 
            // cmbOpenShop
            // 
            cmbOpenShop.DrawMode = DrawMode.OwnerDrawFixed;
            cmbOpenShop.FormattingEnabled = true;
            cmbOpenShop.Location = new Point(10, 23);
            cmbOpenShop.Margin = new Padding(4, 3, 4, 3);
            cmbOpenShop.Name = "cmbOpenShop";
            cmbOpenShop.Size = new Size(263, 24);
            cmbOpenShop.TabIndex = 0;
            // 
            // fraSetSelfSwitch
            // 
            fraSetSelfSwitch.BackColor = Color.FromArgb(45, 45, 48);
            fraSetSelfSwitch.BorderColor = Color.FromArgb(90, 90, 90);
            fraSetSelfSwitch.Controls.Add(btnSelfswitchOk);
            fraSetSelfSwitch.Controls.Add(btnSelfswitchCancel);
            fraSetSelfSwitch.Controls.Add(cmbLabel47);
            fraSetSelfSwitch.Controls.Add(cmbSetSelfSwitchTo);
            fraSetSelfSwitch.Controls.Add(cmbLabel46);
            fraSetSelfSwitch.Controls.Add(cmbSetSelfSwitch);
            fraSetSelfSwitch.ForeColor = Color.Gainsboro;
            fraSetSelfSwitch.Location = new Point(468, 208);
            fraSetSelfSwitch.Margin = new Padding(4, 3, 4, 3);
            fraSetSelfSwitch.Name = "fraSetSelfSwitch";
            fraSetSelfSwitch.Padding = new Padding(4, 3, 4, 3);
            fraSetSelfSwitch.Size = new Size(287, 115);
            fraSetSelfSwitch.TabIndex = 29;
            fraSetSelfSwitch.TabStop = false;
            fraSetSelfSwitch.Text = "Self Switches";
            fraSetSelfSwitch.Visible = false;
            // 
            // btnSelfswitchOk
            // 
            btnSelfswitchOk.Location = new Point(98, 84);
            btnSelfswitchOk.Margin = new Padding(4, 3, 4, 3);
            btnSelfswitchOk.Name = "btnSelfswitchOk";
            btnSelfswitchOk.Padding = new Padding(6);
            btnSelfswitchOk.Size = new Size(88, 27);
            btnSelfswitchOk.TabIndex = 27;
            btnSelfswitchOk.Text = "Ok";
            btnSelfswitchOk.Click += BtnSelfswitchOk_Click;
            // 
            // btnSelfswitchCancel
            // 
            btnSelfswitchCancel.Location = new Point(192, 84);
            btnSelfswitchCancel.Margin = new Padding(4, 3, 4, 3);
            btnSelfswitchCancel.Name = "btnSelfswitchCancel";
            btnSelfswitchCancel.Padding = new Padding(6);
            btnSelfswitchCancel.Size = new Size(88, 27);
            btnSelfswitchCancel.TabIndex = 26;
            btnSelfswitchCancel.Text = "Cancel";
            btnSelfswitchCancel.Click += BtnSelfswitchCancel_Click;
            // 
            // cmbLabel47
            // 
            cmbLabel47.AutoSize = true;
            cmbLabel47.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel47.Location = new Point(7, 57);
            cmbLabel47.Margin = new Padding(4, 0, 4, 0);
            cmbLabel47.Name = "cmbLabel47";
            cmbLabel47.Size = new Size(38, 15);
            cmbLabel47.TabIndex = 3;
            cmbLabel47.Text = "Set To";
            // 
            // cmbSetSelfSwitchTo
            // 
            cmbSetSelfSwitchTo.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSetSelfSwitchTo.FormattingEnabled = true;
            cmbSetSelfSwitchTo.Items.AddRange(new object[] { "Off", "On" });
            cmbSetSelfSwitchTo.Location = new Point(84, 53);
            cmbSetSelfSwitchTo.Margin = new Padding(4, 3, 4, 3);
            cmbSetSelfSwitchTo.Name = "cmbSetSelfSwitchTo";
            cmbSetSelfSwitchTo.Size = new Size(195, 24);
            cmbSetSelfSwitchTo.TabIndex = 2;
            // 
            // cmbLabel46
            // 
            cmbLabel46.AutoSize = true;
            cmbLabel46.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel46.Location = new Point(7, 25);
            cmbLabel46.Margin = new Padding(4, 0, 4, 0);
            cmbLabel46.Name = "cmbLabel46";
            cmbLabel46.Size = new Size(67, 15);
            cmbLabel46.TabIndex = 1;
            cmbLabel46.Text = "Self Switch:";
            // 
            // cmbSetSelfSwitch
            // 
            cmbSetSelfSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSetSelfSwitch.FormattingEnabled = true;
            cmbSetSelfSwitch.Location = new Point(84, 22);
            cmbSetSelfSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbSetSelfSwitch.Name = "cmbSetSelfSwitch";
            cmbSetSelfSwitch.Size = new Size(195, 24);
            cmbSetSelfSwitch.TabIndex = 0;
            // 
            // fraPlaySound
            // 
            fraPlaySound.BackColor = Color.FromArgb(45, 45, 48);
            fraPlaySound.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlaySound.Controls.Add(btnPlaySoundOk);
            fraPlaySound.Controls.Add(btnPlaySoundCancel);
            fraPlaySound.Controls.Add(cmbPlaySound);
            fraPlaySound.ForeColor = Color.Gainsboro;
            fraPlaySound.Location = new Point(468, 207);
            fraPlaySound.Margin = new Padding(4, 3, 4, 3);
            fraPlaySound.Name = "fraPlaySound";
            fraPlaySound.Padding = new Padding(4, 3, 4, 3);
            fraPlaySound.Size = new Size(287, 88);
            fraPlaySound.TabIndex = 26;
            fraPlaySound.TabStop = false;
            fraPlaySound.Text = "Play Sound";
            fraPlaySound.Visible = false;
            // 
            // btnPlaySoundOk
            // 
            btnPlaySoundOk.Location = new Point(98, 53);
            btnPlaySoundOk.Margin = new Padding(4, 3, 4, 3);
            btnPlaySoundOk.Name = "btnPlaySoundOk";
            btnPlaySoundOk.Padding = new Padding(6);
            btnPlaySoundOk.Size = new Size(88, 27);
            btnPlaySoundOk.TabIndex = 27;
            btnPlaySoundOk.Text = "Ok";
            btnPlaySoundOk.Click += BtnPlaySoundOK_Click;
            // 
            // btnPlaySoundCancel
            // 
            btnPlaySoundCancel.Location = new Point(192, 53);
            btnPlaySoundCancel.Margin = new Padding(4, 3, 4, 3);
            btnPlaySoundCancel.Name = "btnPlaySoundCancel";
            btnPlaySoundCancel.Padding = new Padding(6);
            btnPlaySoundCancel.Size = new Size(88, 27);
            btnPlaySoundCancel.TabIndex = 26;
            btnPlaySoundCancel.Text = "Cancel";
            btnPlaySoundCancel.Click += BtnPlaySoundCancel_Click;
            // 
            // cmbPlaySound
            // 
            cmbPlaySound.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlaySound.FormattingEnabled = true;
            cmbPlaySound.Location = new Point(7, 22);
            cmbPlaySound.Margin = new Padding(4, 3, 4, 3);
            cmbPlaySound.Name = "cmbPlaySound";
            cmbPlaySound.Size = new Size(272, 24);
            cmbPlaySound.TabIndex = 0;
            // 
            // fraChangePK
            // 
            fraChangePK.BackColor = Color.FromArgb(45, 45, 48);
            fraChangePK.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangePK.Controls.Add(btnChangePkOk);
            fraChangePK.Controls.Add(btnChangePkCancel);
            fraChangePK.Controls.Add(cmbSetPK);
            fraChangePK.ForeColor = Color.Gainsboro;
            fraChangePK.Location = new Point(468, 120);
            fraChangePK.Margin = new Padding(4, 3, 4, 3);
            fraChangePK.Name = "fraChangePK";
            fraChangePK.Padding = new Padding(4, 3, 4, 3);
            fraChangePK.Size = new Size(287, 87);
            fraChangePK.TabIndex = 25;
            fraChangePK.TabStop = false;
            fraChangePK.Text = "Set Player PK";
            fraChangePK.Visible = false;
            // 
            // btnChangePkOk
            // 
            btnChangePkOk.Location = new Point(93, 53);
            btnChangePkOk.Margin = new Padding(4, 3, 4, 3);
            btnChangePkOk.Name = "btnChangePkOk";
            btnChangePkOk.Padding = new Padding(6);
            btnChangePkOk.Size = new Size(88, 27);
            btnChangePkOk.TabIndex = 27;
            btnChangePkOk.Text = "Ok";
            btnChangePkOk.Click += BtnChangePkOK_Click;
            // 
            // btnChangePkCancel
            // 
            btnChangePkCancel.Location = new Point(188, 53);
            btnChangePkCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangePkCancel.Name = "btnChangePkCancel";
            btnChangePkCancel.Padding = new Padding(6);
            btnChangePkCancel.Size = new Size(88, 27);
            btnChangePkCancel.TabIndex = 26;
            btnChangePkCancel.Text = "Cancel";
            btnChangePkCancel.Click += BtnChangePkCancel_Click;
            // 
            // cmbSetPK
            // 
            cmbSetPK.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSetPK.FormattingEnabled = true;
            cmbSetPK.Items.AddRange(new object[] { "No", "Yes" });
            cmbSetPK.Location = new Point(12, 22);
            cmbSetPK.Margin = new Padding(4, 3, 4, 3);
            cmbSetPK.Name = "cmbSetPK";
            cmbSetPK.Size = new Size(263, 24);
            cmbSetPK.TabIndex = 18;
            // 
            // fraCreateLabel
            // 
            fraCreateLabel.BackColor = Color.FromArgb(45, 45, 48);
            fraCreateLabel.BorderColor = Color.FromArgb(90, 90, 90);
            fraCreateLabel.Controls.Add(btnCreatelabelOk);
            fraCreateLabel.Controls.Add(btnCreatelabelCancel);
            fraCreateLabel.Controls.Add(txtLabelName);
            fraCreateLabel.Controls.Add(lblLabelName);
            fraCreateLabel.ForeColor = Color.Gainsboro;
            fraCreateLabel.Location = new Point(468, 152);
            fraCreateLabel.Margin = new Padding(4, 3, 4, 3);
            fraCreateLabel.Name = "fraCreateLabel";
            fraCreateLabel.Padding = new Padding(4, 3, 4, 3);
            fraCreateLabel.Size = new Size(287, 85);
            fraCreateLabel.TabIndex = 24;
            fraCreateLabel.TabStop = false;
            fraCreateLabel.Text = "Create Label";
            fraCreateLabel.Visible = false;
            // 
            // btnCreatelabelOk
            // 
            btnCreatelabelOk.Location = new Point(98, 52);
            btnCreatelabelOk.Margin = new Padding(4, 3, 4, 3);
            btnCreatelabelOk.Name = "btnCreatelabelOk";
            btnCreatelabelOk.Padding = new Padding(6);
            btnCreatelabelOk.Size = new Size(88, 27);
            btnCreatelabelOk.TabIndex = 27;
            btnCreatelabelOk.Text = "Ok";
            btnCreatelabelOk.Click += BtnCreatelabelOk_Click;
            // 
            // btnCreatelabelCancel
            // 
            btnCreatelabelCancel.Location = new Point(192, 52);
            btnCreatelabelCancel.Margin = new Padding(4, 3, 4, 3);
            btnCreatelabelCancel.Name = "btnCreatelabelCancel";
            btnCreatelabelCancel.Padding = new Padding(6);
            btnCreatelabelCancel.Size = new Size(88, 27);
            btnCreatelabelCancel.TabIndex = 26;
            btnCreatelabelCancel.Text = "Cancel";
            btnCreatelabelCancel.Click += BtnCreateLabelCancel_Click;
            // 
            // txtLabelName
            // 
            txtLabelName.BackColor = Color.FromArgb(69, 73, 74);
            txtLabelName.BorderStyle = BorderStyle.FixedSingle;
            txtLabelName.ForeColor = Color.FromArgb(220, 220, 220);
            txtLabelName.Location = new Point(93, 22);
            txtLabelName.Margin = new Padding(4, 3, 4, 3);
            txtLabelName.Name = "txtLabelName";
            txtLabelName.Size = new Size(186, 23);
            txtLabelName.TabIndex = 1;
            // 
            // lblLabelName
            // 
            lblLabelName.AutoSize = true;
            lblLabelName.ForeColor = Color.FromArgb(220, 220, 220);
            lblLabelName.Location = new Point(8, 24);
            lblLabelName.Margin = new Padding(4, 0, 4, 0);
            lblLabelName.Name = "lblLabelName";
            lblLabelName.Size = new Size(73, 15);
            lblLabelName.TabIndex = 0;
            lblLabelName.Text = "Label Name:";
            // 
            // fraChangeJob
            // 
            fraChangeJob.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeJob.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeJob.Controls.Add(btnChangeJobOk);
            fraChangeJob.Controls.Add(btnChangeJobCancel);
            fraChangeJob.Controls.Add(cmbChangeJob);
            fraChangeJob.Controls.Add(cmbLabel38);
            fraChangeJob.ForeColor = Color.Gainsboro;
            fraChangeJob.Location = new Point(468, 126);
            fraChangeJob.Margin = new Padding(4, 3, 4, 3);
            fraChangeJob.Name = "fraChangeJob";
            fraChangeJob.Padding = new Padding(4, 3, 4, 3);
            fraChangeJob.Size = new Size(287, 88);
            fraChangeJob.TabIndex = 23;
            fraChangeJob.TabStop = false;
            fraChangeJob.Text = "Change Player Job";
            fraChangeJob.Visible = false;
            // 
            // btnChangeJobOk
            // 
            btnChangeJobOk.Location = new Point(98, 53);
            btnChangeJobOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeJobOk.Name = "btnChangeJobOk";
            btnChangeJobOk.Padding = new Padding(6);
            btnChangeJobOk.Size = new Size(88, 27);
            btnChangeJobOk.TabIndex = 27;
            btnChangeJobOk.Text = "Ok";
            btnChangeJobOk.Click += BtnChangeJobOK_Click;
            // 
            // btnChangeJobCancel
            // 
            btnChangeJobCancel.Location = new Point(192, 53);
            btnChangeJobCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeJobCancel.Name = "btnChangeJobCancel";
            btnChangeJobCancel.Padding = new Padding(6);
            btnChangeJobCancel.Size = new Size(88, 27);
            btnChangeJobCancel.TabIndex = 26;
            btnChangeJobCancel.Text = "Cancel";
            btnChangeJobCancel.Click += BtnChangeJobCancel_Click;
            // 
            // cmbChangeJob
            // 
            cmbChangeJob.DrawMode = DrawMode.OwnerDrawFixed;
            cmbChangeJob.FormattingEnabled = true;
            cmbChangeJob.Location = new Point(57, 22);
            cmbChangeJob.Margin = new Padding(4, 3, 4, 3);
            cmbChangeJob.Name = "cmbChangeJob";
            cmbChangeJob.Size = new Size(222, 24);
            cmbChangeJob.TabIndex = 1;
            // 
            // cmbLabel38
            // 
            cmbLabel38.AutoSize = true;
            cmbLabel38.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel38.Location = new Point(9, 25);
            cmbLabel38.Margin = new Padding(4, 0, 4, 0);
            cmbLabel38.Name = "cmbLabel38";
            cmbLabel38.Size = new Size(28, 15);
            cmbLabel38.TabIndex = 0;
            cmbLabel38.Text = "Job:";
            // 
            // fraChangeSkills
            // 
            fraChangeSkills.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeSkills.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeSkills.Controls.Add(btnChangeSkillsOk);
            fraChangeSkills.Controls.Add(btnChangeSkillsCancel);
            fraChangeSkills.Controls.Add(optChangeSkillsRemove);
            fraChangeSkills.Controls.Add(optChangeSkillsAdd);
            fraChangeSkills.Controls.Add(cmbChangeSkills);
            fraChangeSkills.Controls.Add(cmbLabel37);
            fraChangeSkills.ForeColor = Color.Gainsboro;
            fraChangeSkills.Location = new Point(468, 125);
            fraChangeSkills.Margin = new Padding(4, 3, 4, 3);
            fraChangeSkills.Name = "fraChangeSkills";
            fraChangeSkills.Padding = new Padding(4, 3, 4, 3);
            fraChangeSkills.Size = new Size(287, 113);
            fraChangeSkills.TabIndex = 22;
            fraChangeSkills.TabStop = false;
            fraChangeSkills.Text = "Change Player Skills";
            fraChangeSkills.Visible = false;
            // 
            // btnChangeSkillsOk
            // 
            btnChangeSkillsOk.Location = new Point(98, 77);
            btnChangeSkillsOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeSkillsOk.Name = "btnChangeSkillsOk";
            btnChangeSkillsOk.Padding = new Padding(6);
            btnChangeSkillsOk.Size = new Size(88, 27);
            btnChangeSkillsOk.TabIndex = 27;
            btnChangeSkillsOk.Text = "Ok";
            btnChangeSkillsOk.Click += BtnChangeSkillsOK_Click;
            // 
            // btnChangeSkillsCancel
            // 
            btnChangeSkillsCancel.Location = new Point(192, 77);
            btnChangeSkillsCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeSkillsCancel.Name = "btnChangeSkillsCancel";
            btnChangeSkillsCancel.Padding = new Padding(6);
            btnChangeSkillsCancel.Size = new Size(88, 27);
            btnChangeSkillsCancel.TabIndex = 26;
            btnChangeSkillsCancel.Text = "Cancel";
            btnChangeSkillsCancel.Click += BtnChangeSkillsCancel_Click;
            // 
            // optChangeSkillsRemove
            // 
            optChangeSkillsRemove.AutoSize = true;
            optChangeSkillsRemove.Location = new Point(172, 51);
            optChangeSkillsRemove.Margin = new Padding(4, 3, 4, 3);
            optChangeSkillsRemove.Name = "optChangeSkillsRemove";
            optChangeSkillsRemove.Size = new Size(59, 19);
            optChangeSkillsRemove.TabIndex = 3;
            optChangeSkillsRemove.TabStop = true;
            optChangeSkillsRemove.Text = "Forget";
            // 
            // optChangeSkillsAdd
            // 
            optChangeSkillsAdd.AutoSize = true;
            optChangeSkillsAdd.Location = new Point(76, 51);
            optChangeSkillsAdd.Margin = new Padding(4, 3, 4, 3);
            optChangeSkillsAdd.Name = "optChangeSkillsAdd";
            optChangeSkillsAdd.Size = new Size(55, 19);
            optChangeSkillsAdd.TabIndex = 2;
            optChangeSkillsAdd.TabStop = true;
            optChangeSkillsAdd.Text = "Teach";
            // 
            // cmbChangeSkills
            // 
            cmbChangeSkills.DrawMode = DrawMode.OwnerDrawFixed;
            cmbChangeSkills.FormattingEnabled = true;
            cmbChangeSkills.Location = new Point(48, 20);
            cmbChangeSkills.Margin = new Padding(4, 3, 4, 3);
            cmbChangeSkills.Name = "cmbChangeSkills";
            cmbChangeSkills.Size = new Size(230, 24);
            cmbChangeSkills.TabIndex = 1;
            // 
            // cmbLabel37
            // 
            cmbLabel37.AutoSize = true;
            cmbLabel37.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel37.Location = new Point(7, 23);
            cmbLabel37.Margin = new Padding(4, 0, 4, 0);
            cmbLabel37.Name = "cmbLabel37";
            cmbLabel37.Size = new Size(31, 15);
            cmbLabel37.TabIndex = 0;
            cmbLabel37.Text = "Skill:";
            // 
            // fraPlayerSwitch
            // 
            fraPlayerSwitch.BackColor = Color.FromArgb(45, 45, 48);
            fraPlayerSwitch.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlayerSwitch.Controls.Add(btnSetPlayerSwitchOk);
            fraPlayerSwitch.Controls.Add(btnSetPlayerswitchCancel);
            fraPlayerSwitch.Controls.Add(cmbPlayerSwitchSet);
            fraPlayerSwitch.Controls.Add(cmbLabel23);
            fraPlayerSwitch.Controls.Add(cmbSwitch);
            fraPlayerSwitch.Controls.Add(cmbLabel22);
            fraPlayerSwitch.ForeColor = Color.Gainsboro;
            fraPlayerSwitch.Location = new Point(248, 450);
            fraPlayerSwitch.Margin = new Padding(4, 3, 4, 3);
            fraPlayerSwitch.Name = "fraPlayerSwitch";
            fraPlayerSwitch.Padding = new Padding(4, 3, 4, 3);
            fraPlayerSwitch.Size = new Size(212, 115);
            fraPlayerSwitch.TabIndex = 2;
            fraPlayerSwitch.TabStop = false;
            fraPlayerSwitch.Text = "Change Items";
            fraPlayerSwitch.Visible = false;
            // 
            // btnSetPlayerSwitchOk
            // 
            btnSetPlayerSwitchOk.Location = new Point(23, 83);
            btnSetPlayerSwitchOk.Margin = new Padding(4, 3, 4, 3);
            btnSetPlayerSwitchOk.Name = "btnSetPlayerSwitchOk";
            btnSetPlayerSwitchOk.Padding = new Padding(6);
            btnSetPlayerSwitchOk.Size = new Size(88, 27);
            btnSetPlayerSwitchOk.TabIndex = 9;
            btnSetPlayerSwitchOk.Text = "Ok";
            btnSetPlayerSwitchOk.Click += BtnSetPlayerSwitchOk_Click;
            // 
            // btnSetPlayerswitchCancel
            // 
            btnSetPlayerswitchCancel.Location = new Point(118, 83);
            btnSetPlayerswitchCancel.Margin = new Padding(4, 3, 4, 3);
            btnSetPlayerswitchCancel.Name = "btnSetPlayerswitchCancel";
            btnSetPlayerswitchCancel.Padding = new Padding(6);
            btnSetPlayerswitchCancel.Size = new Size(88, 27);
            btnSetPlayerswitchCancel.TabIndex = 8;
            btnSetPlayerswitchCancel.Text = "Cancel";
            btnSetPlayerswitchCancel.Click += BtnSetPlayerSwitchCancel_Click;
            // 
            // cmbPlayerSwitchSet
            // 
            cmbPlayerSwitchSet.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayerSwitchSet.FormattingEnabled = true;
            cmbPlayerSwitchSet.Items.AddRange(new object[] { "False", "True" });
            cmbPlayerSwitchSet.Location = new Point(60, 47);
            cmbPlayerSwitchSet.Margin = new Padding(4, 3, 4, 3);
            cmbPlayerSwitchSet.Name = "cmbPlayerSwitchSet";
            cmbPlayerSwitchSet.Size = new Size(145, 24);
            cmbPlayerSwitchSet.TabIndex = 3;
            // 
            // cmbLabel23
            // 
            cmbLabel23.AutoSize = true;
            cmbLabel23.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel23.Location = new Point(7, 53);
            cmbLabel23.Margin = new Padding(4, 0, 4, 0);
            cmbLabel23.Name = "cmbLabel23";
            cmbLabel23.Size = new Size(37, 15);
            cmbLabel23.TabIndex = 2;
            cmbLabel23.Text = "Set to";
            // 
            // cmbSwitch
            // 
            cmbSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSwitch.FormattingEnabled = true;
            cmbSwitch.Location = new Point(60, 15);
            cmbSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbSwitch.Name = "cmbSwitch";
            cmbSwitch.Size = new Size(145, 24);
            cmbSwitch.TabIndex = 1;
            // 
            // cmbLabel22
            // 
            cmbLabel22.AutoSize = true;
            cmbLabel22.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel22.Location = new Point(7, 18);
            cmbLabel22.Margin = new Padding(4, 0, 4, 0);
            cmbLabel22.Name = "cmbLabel22";
            cmbLabel22.Size = new Size(42, 15);
            cmbLabel22.TabIndex = 0;
            cmbLabel22.Text = "Switch";
            // 
            // fraSetWait
            // 
            fraSetWait.BackColor = Color.FromArgb(45, 45, 48);
            fraSetWait.BorderColor = Color.FromArgb(90, 90, 90);
            fraSetWait.Controls.Add(btnSetWaitOk);
            fraSetWait.Controls.Add(btnSetWaitCancel);
            fraSetWait.Controls.Add(cmbLabel74);
            fraSetWait.Controls.Add(cmbLabel72);
            fraSetWait.Controls.Add(cmbLabel73);
            fraSetWait.Controls.Add(nudWaitAmount);
            fraSetWait.ForeColor = Color.Gainsboro;
            fraSetWait.Location = new Point(468, 305);
            fraSetWait.Margin = new Padding(4, 3, 4, 3);
            fraSetWait.Name = "fraSetWait";
            fraSetWait.Padding = new Padding(4, 3, 4, 3);
            fraSetWait.Size = new Size(289, 103);
            fraSetWait.TabIndex = 41;
            fraSetWait.TabStop = false;
            fraSetWait.Text = "Wait...";
            fraSetWait.Visible = false;
            // 
            // btnSetWaitOk
            // 
            btnSetWaitOk.Location = new Point(58, 67);
            btnSetWaitOk.Margin = new Padding(4, 3, 4, 3);
            btnSetWaitOk.Name = "btnSetWaitOk";
            btnSetWaitOk.Padding = new Padding(6);
            btnSetWaitOk.Size = new Size(88, 27);
            btnSetWaitOk.TabIndex = 37;
            btnSetWaitOk.Text = "Ok";
            btnSetWaitOk.Click += BtnSetWaitOK_Click;
            // 
            // btnSetWaitCancel
            // 
            btnSetWaitCancel.Location = new Point(153, 67);
            btnSetWaitCancel.Margin = new Padding(4, 3, 4, 3);
            btnSetWaitCancel.Name = "btnSetWaitCancel";
            btnSetWaitCancel.Padding = new Padding(6);
            btnSetWaitCancel.Size = new Size(88, 27);
            btnSetWaitCancel.TabIndex = 36;
            btnSetWaitCancel.Text = "Cancel";
            btnSetWaitCancel.Click += BtnSetWaitCancel_Click;
            // 
            // cmbLabel74
            // 
            cmbLabel74.AutoSize = true;
            cmbLabel74.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel74.Location = new Point(82, 48);
            cmbLabel74.Margin = new Padding(4, 0, 4, 0);
            cmbLabel74.Name = "cmbLabel74";
            cmbLabel74.Size = new Size(120, 15);
            cmbLabel74.TabIndex = 35;
            cmbLabel74.Text = "Hint: 1000 Ms = 1 Sec";
            // 
            // cmbLabel72
            // 
            cmbLabel72.AutoSize = true;
            cmbLabel72.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel72.Location = new Point(254, 27);
            cmbLabel72.Margin = new Padding(4, 0, 4, 0);
            cmbLabel72.Name = "cmbLabel72";
            cmbLabel72.Size = new Size(23, 15);
            cmbLabel72.TabIndex = 34;
            cmbLabel72.Text = "Ms";
            // 
            // cmbLabel73
            // 
            cmbLabel73.AutoSize = true;
            cmbLabel73.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel73.Location = new Point(18, 27);
            cmbLabel73.Margin = new Padding(4, 0, 4, 0);
            cmbLabel73.Name = "cmbLabel73";
            cmbLabel73.Size = new Size(31, 15);
            cmbLabel73.TabIndex = 33;
            cmbLabel73.Text = "Wait";
            // 
            // nudWaitAmount
            // 
            nudWaitAmount.Location = new Point(58, 22);
            nudWaitAmount.Margin = new Padding(4, 3, 4, 3);
            nudWaitAmount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudWaitAmount.Name = "nudWaitAmount";
            nudWaitAmount.Size = new Size(190, 23);
            nudWaitAmount.TabIndex = 32;
            // 
            // fraMoveRouteWait
            // 
            fraMoveRouteWait.BackColor = Color.FromArgb(45, 45, 48);
            fraMoveRouteWait.BorderColor = Color.FromArgb(90, 90, 90);
            fraMoveRouteWait.Controls.Add(btnMoveWaitCancel);
            fraMoveRouteWait.Controls.Add(btnMoveWaitOk);
            fraMoveRouteWait.Controls.Add(cmbLabel79);
            fraMoveRouteWait.Controls.Add(cmbMoveWait);
            fraMoveRouteWait.ForeColor = Color.Gainsboro;
            fraMoveRouteWait.Location = new Point(468, 571);
            fraMoveRouteWait.Margin = new Padding(4, 3, 4, 3);
            fraMoveRouteWait.Name = "fraMoveRouteWait";
            fraMoveRouteWait.Padding = new Padding(4, 3, 4, 3);
            fraMoveRouteWait.Size = new Size(289, 87);
            fraMoveRouteWait.TabIndex = 48;
            fraMoveRouteWait.TabStop = false;
            fraMoveRouteWait.Text = "Move Route Wait";
            fraMoveRouteWait.Visible = false;
            // 
            // btnMoveWaitCancel
            // 
            btnMoveWaitCancel.Location = new Point(195, 53);
            btnMoveWaitCancel.Margin = new Padding(4, 3, 4, 3);
            btnMoveWaitCancel.Name = "btnMoveWaitCancel";
            btnMoveWaitCancel.Padding = new Padding(6);
            btnMoveWaitCancel.Size = new Size(88, 27);
            btnMoveWaitCancel.TabIndex = 26;
            btnMoveWaitCancel.Text = "Cancel";
            btnMoveWaitCancel.Click += BtnMoveWaitCancel_Click;
            // 
            // btnMoveWaitOk
            // 
            btnMoveWaitOk.Location = new Point(100, 53);
            btnMoveWaitOk.Margin = new Padding(4, 3, 4, 3);
            btnMoveWaitOk.Name = "btnMoveWaitOk";
            btnMoveWaitOk.Padding = new Padding(6);
            btnMoveWaitOk.Size = new Size(88, 27);
            btnMoveWaitOk.TabIndex = 27;
            btnMoveWaitOk.Text = "Ok";
            btnMoveWaitOk.Click += BtnMoveWaitOK_Click;
            // 
            // cmbLabel79
            // 
            cmbLabel79.AutoSize = true;
            cmbLabel79.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel79.Location = new Point(8, 25);
            cmbLabel79.Margin = new Padding(4, 0, 4, 0);
            cmbLabel79.Name = "cmbLabel79";
            cmbLabel79.Size = new Size(39, 15);
            cmbLabel79.TabIndex = 1;
            cmbLabel79.Text = "Event:";
            // 
            // cmbMoveWait
            // 
            cmbMoveWait.DrawMode = DrawMode.OwnerDrawFixed;
            cmbMoveWait.FormattingEnabled = true;
            cmbMoveWait.Location = new Point(60, 22);
            cmbMoveWait.Margin = new Padding(4, 3, 4, 3);
            cmbMoveWait.Name = "cmbMoveWait";
            cmbMoveWait.Size = new Size(222, 24);
            cmbMoveWait.TabIndex = 0;
            // 
            // fraSpawnNpc
            // 
            fraSpawnNpc.BackColor = Color.FromArgb(45, 45, 48);
            fraSpawnNpc.BorderColor = Color.FromArgb(90, 90, 90);
            fraSpawnNpc.Controls.Add(btnSpawnNpcOk);
            fraSpawnNpc.Controls.Add(btnSpawnNpcancel);
            fraSpawnNpc.Controls.Add(cmbSpawnNpc);
            fraSpawnNpc.ForeColor = Color.Gainsboro;
            fraSpawnNpc.Location = new Point(468, 475);
            fraSpawnNpc.Margin = new Padding(4, 3, 4, 3);
            fraSpawnNpc.Name = "fraSpawnNpc";
            fraSpawnNpc.Padding = new Padding(4, 3, 4, 3);
            fraSpawnNpc.Size = new Size(289, 89);
            fraSpawnNpc.TabIndex = 46;
            fraSpawnNpc.TabStop = false;
            fraSpawnNpc.Text = "Spawn Npc";
            fraSpawnNpc.Visible = false;
            // 
            // btnSpawnNpcOk
            // 
            btnSpawnNpcOk.Location = new Point(54, 54);
            btnSpawnNpcOk.Margin = new Padding(4, 3, 4, 3);
            btnSpawnNpcOk.Name = "btnSpawnNpcOk";
            btnSpawnNpcOk.Padding = new Padding(6);
            btnSpawnNpcOk.Size = new Size(88, 27);
            btnSpawnNpcOk.TabIndex = 27;
            btnSpawnNpcOk.Text = "Ok";
            btnSpawnNpcOk.Click += BtnSpawnNpcOK_Click;
            // 
            // btnSpawnNpcancel
            // 
            btnSpawnNpcancel.Location = new Point(148, 54);
            btnSpawnNpcancel.Margin = new Padding(4, 3, 4, 3);
            btnSpawnNpcancel.Name = "btnSpawnNpcancel";
            btnSpawnNpcancel.Padding = new Padding(6);
            btnSpawnNpcancel.Size = new Size(88, 27);
            btnSpawnNpcancel.TabIndex = 26;
            btnSpawnNpcancel.Text = "Cancel";
            btnSpawnNpcancel.Click += BtnSpawnNpcancel_Click;
            // 
            // cmbSpawnNpc
            // 
            cmbSpawnNpc.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSpawnNpc.FormattingEnabled = true;
            cmbSpawnNpc.Location = new Point(7, 22);
            cmbSpawnNpc.Margin = new Padding(4, 3, 4, 3);
            cmbSpawnNpc.Name = "cmbSpawnNpc";
            cmbSpawnNpc.Size = new Size(272, 24);
            cmbSpawnNpc.TabIndex = 0;
            // 
            // fraSetWeather
            // 
            fraSetWeather.BackColor = Color.FromArgb(45, 45, 48);
            fraSetWeather.BorderColor = Color.FromArgb(90, 90, 90);
            fraSetWeather.Controls.Add(btnSetWeatherOk);
            fraSetWeather.Controls.Add(btnSetWeatherCancel);
            fraSetWeather.Controls.Add(cmbLabel76);
            fraSetWeather.Controls.Add(nudWeatherIntensity);
            fraSetWeather.Controls.Add(cmbLabel75);
            fraSetWeather.Controls.Add(CmbWeather);
            fraSetWeather.ForeColor = Color.Gainsboro;
            fraSetWeather.Location = new Point(468, 406);
            fraSetWeather.Margin = new Padding(4, 3, 4, 3);
            fraSetWeather.Name = "fraSetWeather";
            fraSetWeather.Padding = new Padding(4, 3, 4, 3);
            fraSetWeather.Size = new Size(289, 110);
            fraSetWeather.TabIndex = 44;
            fraSetWeather.TabStop = false;
            fraSetWeather.Text = "Set Weather";
            fraSetWeather.Visible = false;
            // 
            // btnSetWeatherOk
            // 
            btnSetWeatherOk.Location = new Point(54, 76);
            btnSetWeatherOk.Margin = new Padding(4, 3, 4, 3);
            btnSetWeatherOk.Name = "btnSetWeatherOk";
            btnSetWeatherOk.Padding = new Padding(6);
            btnSetWeatherOk.Size = new Size(88, 27);
            btnSetWeatherOk.TabIndex = 34;
            btnSetWeatherOk.Text = "Ok";
            btnSetWeatherOk.Click += BtnSetWeatherOK_Click;
            // 
            // btnSetWeatherCancel
            // 
            btnSetWeatherCancel.Location = new Point(148, 76);
            btnSetWeatherCancel.Margin = new Padding(4, 3, 4, 3);
            btnSetWeatherCancel.Name = "btnSetWeatherCancel";
            btnSetWeatherCancel.Padding = new Padding(6);
            btnSetWeatherCancel.Size = new Size(88, 27);
            btnSetWeatherCancel.TabIndex = 33;
            btnSetWeatherCancel.Text = "Cancel";
            btnSetWeatherCancel.Click += BtnSetWeatherCancel_Click;
            // 
            // cmbLabel76
            // 
            cmbLabel76.AutoSize = true;
            cmbLabel76.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel76.Location = new Point(9, 51);
            cmbLabel76.Margin = new Padding(4, 0, 4, 0);
            cmbLabel76.Name = "cmbLabel76";
            cmbLabel76.Size = new Size(55, 15);
            cmbLabel76.TabIndex = 32;
            cmbLabel76.Text = "Intensity:";
            // 
            // nudWeatherIntensity
            // 
            nudWeatherIntensity.Location = new Point(102, 47);
            nudWeatherIntensity.Margin = new Padding(4, 3, 4, 3);
            nudWeatherIntensity.Name = "nudWeatherIntensity";
            nudWeatherIntensity.Size = new Size(181, 23);
            nudWeatherIntensity.TabIndex = 31;
            // 
            // cmbLabel75
            // 
            cmbLabel75.AutoSize = true;
            cmbLabel75.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel75.Location = new Point(7, 21);
            cmbLabel75.Margin = new Padding(4, 0, 4, 0);
            cmbLabel75.Name = "cmbLabel75";
            cmbLabel75.Size = new Size(78, 15);
            cmbLabel75.TabIndex = 1;
            cmbLabel75.Text = "Weather Type";
            // 
            // CmbWeather
            // 
            CmbWeather.DrawMode = DrawMode.OwnerDrawFixed;
            CmbWeather.FormattingEnabled = true;
            CmbWeather.Items.AddRange(new object[] { "None", "Rain", "Snow", "Hail", "Sand Storm", "Storm" });
            CmbWeather.Location = new Point(100, 17);
            CmbWeather.Margin = new Padding(4, 3, 4, 3);
            CmbWeather.Name = "CmbWeather";
            CmbWeather.Size = new Size(180, 24);
            CmbWeather.TabIndex = 0;
            // 
            // fraGiveExp
            // 
            fraGiveExp.BackColor = Color.FromArgb(45, 45, 48);
            fraGiveExp.BorderColor = Color.FromArgb(90, 90, 90);
            fraGiveExp.Controls.Add(btnGiveExpOk);
            fraGiveExp.Controls.Add(btnGiveExpCancel);
            fraGiveExp.Controls.Add(nudGiveExp);
            fraGiveExp.Controls.Add(cmbLabel77);
            fraGiveExp.ForeColor = Color.Gainsboro;
            fraGiveExp.Location = new Point(468, 406);
            fraGiveExp.Margin = new Padding(4, 3, 4, 3);
            fraGiveExp.Name = "fraGiveExp";
            fraGiveExp.Padding = new Padding(4, 3, 4, 3);
            fraGiveExp.Size = new Size(289, 84);
            fraGiveExp.TabIndex = 45;
            fraGiveExp.TabStop = false;
            fraGiveExp.Text = "Give Experience";
            fraGiveExp.Visible = false;
            // 
            // btnGiveExpOk
            // 
            btnGiveExpOk.Location = new Point(58, 52);
            btnGiveExpOk.Margin = new Padding(4, 3, 4, 3);
            btnGiveExpOk.Name = "btnGiveExpOk";
            btnGiveExpOk.Padding = new Padding(6);
            btnGiveExpOk.Size = new Size(88, 27);
            btnGiveExpOk.TabIndex = 27;
            btnGiveExpOk.Text = "Ok";
            btnGiveExpOk.Click += BtnGiveExpOK_Click;
            // 
            // btnGiveExpCancel
            // 
            btnGiveExpCancel.Location = new Point(153, 52);
            btnGiveExpCancel.Margin = new Padding(4, 3, 4, 3);
            btnGiveExpCancel.Name = "btnGiveExpCancel";
            btnGiveExpCancel.Padding = new Padding(6);
            btnGiveExpCancel.Size = new Size(88, 27);
            btnGiveExpCancel.TabIndex = 26;
            btnGiveExpCancel.Text = "Cancel";
            btnGiveExpCancel.Click += BtnGiveExpCancel_Click;
            // 
            // nudGiveExp
            // 
            nudGiveExp.Location = new Point(90, 22);
            nudGiveExp.Margin = new Padding(4, 3, 4, 3);
            nudGiveExp.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudGiveExp.Name = "nudGiveExp";
            nudGiveExp.Size = new Size(192, 23);
            nudGiveExp.TabIndex = 20;
            // 
            // cmbLabel77
            // 
            cmbLabel77.AutoSize = true;
            cmbLabel77.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel77.Location = new Point(7, 24);
            cmbLabel77.Margin = new Padding(4, 0, 4, 0);
            cmbLabel77.Name = "cmbLabel77";
            cmbLabel77.Size = new Size(55, 15);
            cmbLabel77.TabIndex = 0;
            cmbLabel77.Text = "Give Exp:";
            // 
            // fraSetAccess
            // 
            fraSetAccess.BackColor = Color.FromArgb(45, 45, 48);
            fraSetAccess.BorderColor = Color.FromArgb(90, 90, 90);
            fraSetAccess.Controls.Add(btnSetAccessOk);
            fraSetAccess.Controls.Add(btnSetAccessCancel);
            fraSetAccess.Controls.Add(cmbSetAccess);
            fraSetAccess.ForeColor = Color.Gainsboro;
            fraSetAccess.Location = new Point(468, 407);
            fraSetAccess.Margin = new Padding(4, 3, 4, 3);
            fraSetAccess.Name = "fraSetAccess";
            fraSetAccess.Padding = new Padding(4, 3, 4, 3);
            fraSetAccess.Size = new Size(289, 92);
            fraSetAccess.TabIndex = 42;
            fraSetAccess.TabStop = false;
            fraSetAccess.Text = "Set Access";
            fraSetAccess.Visible = false;
            // 
            // btnSetAccessOk
            // 
            btnSetAccessOk.Location = new Point(54, 55);
            btnSetAccessOk.Margin = new Padding(4, 3, 4, 3);
            btnSetAccessOk.Name = "btnSetAccessOk";
            btnSetAccessOk.Padding = new Padding(6);
            btnSetAccessOk.Size = new Size(88, 27);
            btnSetAccessOk.TabIndex = 27;
            btnSetAccessOk.Text = "Ok";
            btnSetAccessOk.Click += BtnSetAccessOK_Click;
            // 
            // btnSetAccessCancel
            // 
            btnSetAccessCancel.Location = new Point(148, 55);
            btnSetAccessCancel.Margin = new Padding(4, 3, 4, 3);
            btnSetAccessCancel.Name = "btnSetAccessCancel";
            btnSetAccessCancel.Padding = new Padding(6);
            btnSetAccessCancel.Size = new Size(88, 27);
            btnSetAccessCancel.TabIndex = 26;
            btnSetAccessCancel.Text = "Cancel";
            btnSetAccessCancel.Click += BtnSetAccessCancel_Click;
            // 
            // cmbSetAccess
            // 
            cmbSetAccess.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSetAccess.FormattingEnabled = true;
            cmbSetAccess.Items.AddRange(new object[] { "1: Player", "2: Moderator", "3: Mapper", "4: Developer", "5: Owner" });
            cmbSetAccess.Location = new Point(38, 22);
            cmbSetAccess.Margin = new Padding(4, 3, 4, 3);
            cmbSetAccess.Name = "cmbSetAccess";
            cmbSetAccess.Size = new Size(219, 24);
            cmbSetAccess.TabIndex = 0;
            // 
            // fraChangeGender
            // 
            fraChangeGender.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeGender.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeGender.Controls.Add(btnChangeGenderOk);
            fraChangeGender.Controls.Add(btnChangeGenderCancel);
            fraChangeGender.Controls.Add(optChangeSexFemale);
            fraChangeGender.Controls.Add(optChangeSexMale);
            fraChangeGender.ForeColor = Color.Gainsboro;
            fraChangeGender.Location = new Point(468, 420);
            fraChangeGender.Margin = new Padding(4, 3, 4, 3);
            fraChangeGender.Name = "fraChangeGender";
            fraChangeGender.Padding = new Padding(4, 3, 4, 3);
            fraChangeGender.Size = new Size(289, 83);
            fraChangeGender.TabIndex = 37;
            fraChangeGender.TabStop = false;
            fraChangeGender.Text = "Change Player Gender";
            fraChangeGender.Visible = false;
            // 
            // btnChangeGenderOk
            // 
            btnChangeGenderOk.Location = new Point(46, 48);
            btnChangeGenderOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeGenderOk.Name = "btnChangeGenderOk";
            btnChangeGenderOk.Padding = new Padding(6);
            btnChangeGenderOk.Size = new Size(88, 27);
            btnChangeGenderOk.TabIndex = 27;
            btnChangeGenderOk.Text = "Ok";
            btnChangeGenderOk.Click += BtnChangeGenderOK_Click;
            // 
            // btnChangeGenderCancel
            // 
            btnChangeGenderCancel.Location = new Point(140, 48);
            btnChangeGenderCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeGenderCancel.Name = "btnChangeGenderCancel";
            btnChangeGenderCancel.Padding = new Padding(6);
            btnChangeGenderCancel.Size = new Size(88, 27);
            btnChangeGenderCancel.TabIndex = 26;
            btnChangeGenderCancel.Text = "Cancel";
            btnChangeGenderCancel.Click += BtnChangeGenderCancel_Click;
            // 
            // optChangeSexFemale
            // 
            optChangeSexFemale.AutoSize = true;
            optChangeSexFemale.Location = new Point(164, 22);
            optChangeSexFemale.Margin = new Padding(4, 3, 4, 3);
            optChangeSexFemale.Name = "optChangeSexFemale";
            optChangeSexFemale.Size = new Size(63, 19);
            optChangeSexFemale.TabIndex = 1;
            optChangeSexFemale.TabStop = true;
            optChangeSexFemale.Text = "Female";
            // 
            // optChangeSexMale
            // 
            optChangeSexMale.AutoSize = true;
            optChangeSexMale.Location = new Point(61, 22);
            optChangeSexMale.Margin = new Padding(4, 3, 4, 3);
            optChangeSexMale.Name = "optChangeSexMale";
            optChangeSexMale.Size = new Size(51, 19);
            optChangeSexMale.TabIndex = 0;
            optChangeSexMale.TabStop = true;
            optChangeSexMale.Text = "Male";
            // 
            // fraShowChoices
            // 
            fraShowChoices.BackColor = Color.FromArgb(45, 45, 48);
            fraShowChoices.BorderColor = Color.FromArgb(90, 90, 90);
            fraShowChoices.Controls.Add(txtChoices4);
            fraShowChoices.Controls.Add(txtChoices3);
            fraShowChoices.Controls.Add(txtChoices2);
            fraShowChoices.Controls.Add(txtChoices1);
            fraShowChoices.Controls.Add(cmbLabel56);
            fraShowChoices.Controls.Add(cmbLabel57);
            fraShowChoices.Controls.Add(cmbLabel55);
            fraShowChoices.Controls.Add(cmbLabel54);
            fraShowChoices.Controls.Add(cmbLabel52);
            fraShowChoices.Controls.Add(txtChoicePrompt);
            fraShowChoices.Controls.Add(btnShowChoicesOk);
            fraShowChoices.Controls.Add(btnShowChoicesCancel);
            fraShowChoices.ForeColor = Color.Gainsboro;
            fraShowChoices.Location = new Point(468, 119);
            fraShowChoices.Margin = new Padding(4, 3, 4, 3);
            fraShowChoices.Name = "fraShowChoices";
            fraShowChoices.Padding = new Padding(4, 3, 4, 3);
            fraShowChoices.Size = new Size(289, 384);
            fraShowChoices.TabIndex = 32;
            fraShowChoices.TabStop = false;
            fraShowChoices.Text = "Show Choices";
            fraShowChoices.Visible = false;
            // 
            // txtChoices4
            // 
            txtChoices4.BackColor = Color.FromArgb(69, 73, 74);
            txtChoices4.BorderStyle = BorderStyle.FixedSingle;
            txtChoices4.ForeColor = Color.FromArgb(220, 220, 220);
            txtChoices4.Location = new Point(164, 201);
            txtChoices4.Margin = new Padding(4, 3, 4, 3);
            txtChoices4.Name = "txtChoices4";
            txtChoices4.Size = new Size(116, 23);
            txtChoices4.TabIndex = 34;
            // 
            // txtChoices3
            // 
            txtChoices3.BackColor = Color.FromArgb(69, 73, 74);
            txtChoices3.BorderStyle = BorderStyle.FixedSingle;
            txtChoices3.ForeColor = Color.FromArgb(220, 220, 220);
            txtChoices3.Location = new Point(7, 200);
            txtChoices3.Margin = new Padding(4, 3, 4, 3);
            txtChoices3.Name = "txtChoices3";
            txtChoices3.Size = new Size(116, 23);
            txtChoices3.TabIndex = 33;
            // 
            // txtChoices2
            // 
            txtChoices2.BackColor = Color.FromArgb(69, 73, 74);
            txtChoices2.BorderStyle = BorderStyle.FixedSingle;
            txtChoices2.ForeColor = Color.FromArgb(220, 220, 220);
            txtChoices2.Location = new Point(164, 155);
            txtChoices2.Margin = new Padding(4, 3, 4, 3);
            txtChoices2.Name = "txtChoices2";
            txtChoices2.Size = new Size(116, 23);
            txtChoices2.TabIndex = 32;
            // 
            // txtChoices1
            // 
            txtChoices1.BackColor = Color.FromArgb(69, 73, 74);
            txtChoices1.BorderStyle = BorderStyle.FixedSingle;
            txtChoices1.ForeColor = Color.FromArgb(220, 220, 220);
            txtChoices1.Location = new Point(7, 155);
            txtChoices1.Margin = new Padding(4, 3, 4, 3);
            txtChoices1.Name = "txtChoices1";
            txtChoices1.Size = new Size(116, 23);
            txtChoices1.TabIndex = 31;
            // 
            // cmbLabel56
            // 
            cmbLabel56.AutoSize = true;
            cmbLabel56.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel56.Location = new Point(161, 181);
            cmbLabel56.Margin = new Padding(4, 0, 4, 0);
            cmbLabel56.Name = "cmbLabel56";
            cmbLabel56.Size = new Size(53, 15);
            cmbLabel56.TabIndex = 30;
            cmbLabel56.Text = "Choice 4";
            // 
            // cmbLabel57
            // 
            cmbLabel57.AutoSize = true;
            cmbLabel57.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel57.Location = new Point(8, 181);
            cmbLabel57.Margin = new Padding(4, 0, 4, 0);
            cmbLabel57.Name = "cmbLabel57";
            cmbLabel57.Size = new Size(53, 15);
            cmbLabel57.TabIndex = 29;
            cmbLabel57.Text = "Choice 3";
            // 
            // cmbLabel55
            // 
            cmbLabel55.AutoSize = true;
            cmbLabel55.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel55.Location = new Point(161, 136);
            cmbLabel55.Margin = new Padding(4, 0, 4, 0);
            cmbLabel55.Name = "cmbLabel55";
            cmbLabel55.Size = new Size(53, 15);
            cmbLabel55.TabIndex = 28;
            cmbLabel55.Text = "Choice 2";
            // 
            // cmbLabel54
            // 
            cmbLabel54.AutoSize = true;
            cmbLabel54.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel54.Location = new Point(7, 136);
            cmbLabel54.Margin = new Padding(4, 0, 4, 0);
            cmbLabel54.Name = "cmbLabel54";
            cmbLabel54.Size = new Size(53, 15);
            cmbLabel54.TabIndex = 27;
            cmbLabel54.Text = "Choice 1";
            // 
            // cmbLabel52
            // 
            cmbLabel52.AutoSize = true;
            cmbLabel52.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel52.Location = new Point(8, 22);
            cmbLabel52.Margin = new Padding(4, 0, 4, 0);
            cmbLabel52.Name = "cmbLabel52";
            cmbLabel52.Size = new Size(47, 15);
            cmbLabel52.TabIndex = 26;
            cmbLabel52.Text = "Prompt";
            // 
            // txtChoicePrompt
            // 
            txtChoicePrompt.BackColor = Color.FromArgb(69, 73, 74);
            txtChoicePrompt.BorderStyle = BorderStyle.FixedSingle;
            txtChoicePrompt.ForeColor = Color.FromArgb(220, 220, 220);
            txtChoicePrompt.Location = new Point(10, 44);
            txtChoicePrompt.Margin = new Padding(4, 3, 4, 3);
            txtChoicePrompt.Multiline = true;
            txtChoicePrompt.Name = "txtChoicePrompt";
            txtChoicePrompt.Size = new Size(266, 89);
            txtChoicePrompt.TabIndex = 21;
            // 
            // btnShowChoicesOk
            // 
            btnShowChoicesOk.Location = new Point(98, 352);
            btnShowChoicesOk.Margin = new Padding(4, 3, 4, 3);
            btnShowChoicesOk.Name = "btnShowChoicesOk";
            btnShowChoicesOk.Padding = new Padding(6);
            btnShowChoicesOk.Size = new Size(88, 27);
            btnShowChoicesOk.TabIndex = 25;
            btnShowChoicesOk.Text = "Ok";
            btnShowChoicesOk.Click += BtnShowChoicesOk_Click;
            // 
            // btnShowChoicesCancel
            // 
            btnShowChoicesCancel.Location = new Point(192, 352);
            btnShowChoicesCancel.Margin = new Padding(4, 3, 4, 3);
            btnShowChoicesCancel.Name = "btnShowChoicesCancel";
            btnShowChoicesCancel.Padding = new Padding(6);
            btnShowChoicesCancel.Size = new Size(88, 27);
            btnShowChoicesCancel.TabIndex = 24;
            btnShowChoicesCancel.Text = "Cancel";
            btnShowChoicesCancel.Click += BtnShowChoicesCancel_Click;
            // 
            // fraChangeLevel
            // 
            fraChangeLevel.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeLevel.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeLevel.Controls.Add(btnChangeLevelOk);
            fraChangeLevel.Controls.Add(btnChangeLevelCancel);
            fraChangeLevel.Controls.Add(cmbLabel65);
            fraChangeLevel.Controls.Add(nudChangeLevel);
            fraChangeLevel.ForeColor = Color.Gainsboro;
            fraChangeLevel.Location = new Point(468, 338);
            fraChangeLevel.Margin = new Padding(4, 3, 4, 3);
            fraChangeLevel.Name = "fraChangeLevel";
            fraChangeLevel.Padding = new Padding(4, 3, 4, 3);
            fraChangeLevel.Size = new Size(289, 83);
            fraChangeLevel.TabIndex = 38;
            fraChangeLevel.TabStop = false;
            fraChangeLevel.Text = "Change Level";
            fraChangeLevel.Visible = false;
            // 
            // btnChangeLevelOk
            // 
            btnChangeLevelOk.Location = new Point(54, 52);
            btnChangeLevelOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeLevelOk.Name = "btnChangeLevelOk";
            btnChangeLevelOk.Padding = new Padding(6);
            btnChangeLevelOk.Size = new Size(88, 27);
            btnChangeLevelOk.TabIndex = 27;
            btnChangeLevelOk.Text = "Ok";
            btnChangeLevelOk.Click += BtnChangeLevelOK_Click;
            // 
            // btnChangeLevelCancel
            // 
            btnChangeLevelCancel.Location = new Point(148, 52);
            btnChangeLevelCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeLevelCancel.Name = "btnChangeLevelCancel";
            btnChangeLevelCancel.Padding = new Padding(6);
            btnChangeLevelCancel.Size = new Size(88, 27);
            btnChangeLevelCancel.TabIndex = 26;
            btnChangeLevelCancel.Text = "Cancel";
            btnChangeLevelCancel.Click += BtnChangeLevelCancel_Click;
            // 
            // cmbLabel65
            // 
            cmbLabel65.AutoSize = true;
            cmbLabel65.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel65.Location = new Point(8, 24);
            cmbLabel65.Margin = new Padding(4, 0, 4, 0);
            cmbLabel65.Name = "cmbLabel65";
            cmbLabel65.Size = new Size(37, 15);
            cmbLabel65.TabIndex = 24;
            cmbLabel65.Text = "Level:";
            // 
            // nudChangeLevel
            // 
            nudChangeLevel.Location = new Point(70, 22);
            nudChangeLevel.Margin = new Padding(4, 3, 4, 3);
            nudChangeLevel.Name = "nudChangeLevel";
            nudChangeLevel.Size = new Size(140, 23);
            nudChangeLevel.TabIndex = 23;
            // 
            // fraPlayerVariable
            // 
            fraPlayerVariable.BackColor = Color.FromArgb(45, 45, 48);
            fraPlayerVariable.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlayerVariable.Controls.Add(nudVariableData2);
            fraPlayerVariable.Controls.Add(optVariableAction2);
            fraPlayerVariable.Controls.Add(btnPlayerVarOk);
            fraPlayerVariable.Controls.Add(btnPlayerVarCancel);
            fraPlayerVariable.Controls.Add(cmbLabel51);
            fraPlayerVariable.Controls.Add(cmbLabel50);
            fraPlayerVariable.Controls.Add(nudVariableData4);
            fraPlayerVariable.Controls.Add(nudVariableData3);
            fraPlayerVariable.Controls.Add(optVariableAction3);
            fraPlayerVariable.Controls.Add(optVariableAction1);
            fraPlayerVariable.Controls.Add(nudVariableData1);
            fraPlayerVariable.Controls.Add(nudVariableData0);
            fraPlayerVariable.Controls.Add(optVariableAction0);
            fraPlayerVariable.Controls.Add(cmbVariable);
            fraPlayerVariable.Controls.Add(cmbLabel49);
            fraPlayerVariable.ForeColor = Color.Gainsboro;
            fraPlayerVariable.Location = new Point(468, 325);
            fraPlayerVariable.Margin = new Padding(4, 3, 4, 3);
            fraPlayerVariable.Name = "fraPlayerVariable";
            fraPlayerVariable.Padding = new Padding(4, 3, 4, 3);
            fraPlayerVariable.Size = new Size(287, 178);
            fraPlayerVariable.TabIndex = 31;
            fraPlayerVariable.TabStop = false;
            fraPlayerVariable.Text = "Player Variable";
            fraPlayerVariable.Visible = false;
            // 
            // nudVariableData2
            // 
            nudVariableData2.Location = new Point(140, 83);
            nudVariableData2.Margin = new Padding(4, 3, 4, 3);
            nudVariableData2.Name = "nudVariableData2";
            nudVariableData2.Size = new Size(140, 23);
            nudVariableData2.TabIndex = 29;
            // 
            // optVariableAction2
            // 
            optVariableAction2.AutoSize = true;
            optVariableAction2.Location = new Point(7, 83);
            optVariableAction2.Margin = new Padding(4, 3, 4, 3);
            optVariableAction2.Name = "optVariableAction2";
            optVariableAction2.Size = new Size(69, 19);
            optVariableAction2.TabIndex = 28;
            optVariableAction2.TabStop = true;
            optVariableAction2.Text = "Subtract";
            optVariableAction2.CheckedChanged += OptVariableAction2_CheckedChanged;
            // 
            // btnPlayerVarOk
            // 
            btnPlayerVarOk.Location = new Point(98, 143);
            btnPlayerVarOk.Margin = new Padding(4, 3, 4, 3);
            btnPlayerVarOk.Name = "btnPlayerVarOk";
            btnPlayerVarOk.Padding = new Padding(6);
            btnPlayerVarOk.Size = new Size(88, 27);
            btnPlayerVarOk.TabIndex = 27;
            btnPlayerVarOk.Text = "Ok";
            btnPlayerVarOk.Click += BtnPlayerVarOk_Click;
            // 
            // btnPlayerVarCancel
            // 
            btnPlayerVarCancel.Location = new Point(192, 143);
            btnPlayerVarCancel.Margin = new Padding(4, 3, 4, 3);
            btnPlayerVarCancel.Name = "btnPlayerVarCancel";
            btnPlayerVarCancel.Padding = new Padding(6);
            btnPlayerVarCancel.Size = new Size(88, 27);
            btnPlayerVarCancel.TabIndex = 26;
            btnPlayerVarCancel.Text = "Cancel";
            btnPlayerVarCancel.Click += BtnPlayerVarCancel_Click;
            // 
            // cmbLabel51
            // 
            cmbLabel51.AutoSize = true;
            cmbLabel51.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel51.Location = new Point(88, 115);
            cmbLabel51.Margin = new Padding(4, 0, 4, 0);
            cmbLabel51.Name = "cmbLabel51";
            cmbLabel51.Size = new Size(32, 15);
            cmbLabel51.TabIndex = 16;
            cmbLabel51.Text = "Low:";
            // 
            // cmbLabel50
            // 
            cmbLabel50.AutoSize = true;
            cmbLabel50.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel50.Location = new Point(184, 115);
            cmbLabel50.Margin = new Padding(4, 0, 4, 0);
            cmbLabel50.Name = "cmbLabel50";
            cmbLabel50.Size = new Size(36, 15);
            cmbLabel50.TabIndex = 15;
            cmbLabel50.Text = "High:";
            // 
            // nudVariableData4
            // 
            nudVariableData4.Location = new Point(229, 113);
            nudVariableData4.Margin = new Padding(4, 3, 4, 3);
            nudVariableData4.Name = "nudVariableData4";
            nudVariableData4.Size = new Size(51, 23);
            nudVariableData4.TabIndex = 14;
            // 
            // nudVariableData3
            // 
            nudVariableData3.Location = new Point(130, 113);
            nudVariableData3.Margin = new Padding(4, 3, 4, 3);
            nudVariableData3.Name = "nudVariableData3";
            nudVariableData3.Size = new Size(51, 23);
            nudVariableData3.TabIndex = 13;
            // 
            // optVariableAction3
            // 
            optVariableAction3.AutoSize = true;
            optVariableAction3.Location = new Point(7, 113);
            optVariableAction3.Margin = new Padding(4, 3, 4, 3);
            optVariableAction3.Name = "optVariableAction3";
            optVariableAction3.Size = new Size(70, 19);
            optVariableAction3.TabIndex = 12;
            optVariableAction3.TabStop = true;
            optVariableAction3.Text = "Random";
            optVariableAction3.CheckedChanged += OptVariableAction3_CheckedChanged;
            // 
            // optVariableAction1
            // 
            optVariableAction1.AutoSize = true;
            optVariableAction1.Location = new Point(170, 53);
            optVariableAction1.Margin = new Padding(4, 3, 4, 3);
            optVariableAction1.Name = "optVariableAction1";
            optVariableAction1.Size = new Size(47, 19);
            optVariableAction1.TabIndex = 11;
            optVariableAction1.TabStop = true;
            optVariableAction1.Text = "Add";
            optVariableAction1.CheckedChanged += OptVariableAction1_CheckedChanged;
            // 
            // nudVariableData1
            // 
            nudVariableData1.Location = new Point(229, 53);
            nudVariableData1.Margin = new Padding(4, 3, 4, 3);
            nudVariableData1.Name = "nudVariableData1";
            nudVariableData1.Size = new Size(51, 23);
            nudVariableData1.TabIndex = 10;
            // 
            // nudVariableData0
            // 
            nudVariableData0.Location = new Point(72, 53);
            nudVariableData0.Margin = new Padding(4, 3, 4, 3);
            nudVariableData0.Name = "nudVariableData0";
            nudVariableData0.Size = new Size(51, 23);
            nudVariableData0.TabIndex = 9;
            // 
            // optVariableAction0
            // 
            optVariableAction0.AutoSize = true;
            optVariableAction0.Location = new Point(7, 53);
            optVariableAction0.Margin = new Padding(4, 3, 4, 3);
            optVariableAction0.Name = "optVariableAction0";
            optVariableAction0.Size = new Size(41, 19);
            optVariableAction0.TabIndex = 2;
            optVariableAction0.TabStop = true;
            optVariableAction0.Text = "Set";
            optVariableAction0.CheckedChanged += OptVariableAction0_CheckedChanged;
            // 
            // cmbVariable
            // 
            cmbVariable.DrawMode = DrawMode.OwnerDrawFixed;
            cmbVariable.FormattingEnabled = true;
            cmbVariable.Location = new Point(70, 22);
            cmbVariable.Margin = new Padding(4, 3, 4, 3);
            cmbVariable.Name = "cmbVariable";
            cmbVariable.Size = new Size(208, 24);
            cmbVariable.TabIndex = 1;
            // 
            // cmbLabel49
            // 
            cmbLabel49.AutoSize = true;
            cmbLabel49.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel49.Location = new Point(7, 25);
            cmbLabel49.Margin = new Padding(4, 0, 4, 0);
            cmbLabel49.Name = "cmbLabel49";
            cmbLabel49.Size = new Size(51, 15);
            cmbLabel49.TabIndex = 0;
            cmbLabel49.Text = "Variable:";
            // 
            // fraPlayAnimation
            // 
            fraPlayAnimation.BackColor = Color.FromArgb(45, 45, 48);
            fraPlayAnimation.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlayAnimation.Controls.Add(btnPlayAnimationOk);
            fraPlayAnimation.Controls.Add(btnPlayAnimationCancel);
            fraPlayAnimation.Controls.Add(lblPlayAnimY);
            fraPlayAnimation.Controls.Add(lblPlayAnimX);
            fraPlayAnimation.Controls.Add(cmbPlayAnimEvent);
            fraPlayAnimation.Controls.Add(cmbLabel62);
            fraPlayAnimation.Controls.Add(cmbAnimTargetType);
            fraPlayAnimation.Controls.Add(nudPlayAnimTileY);
            fraPlayAnimation.Controls.Add(nudPlayAnimTileX);
            fraPlayAnimation.Controls.Add(cmbLabel61);
            fraPlayAnimation.Controls.Add(cmbPlayAnim);
            fraPlayAnimation.ForeColor = Color.Gainsboro;
            fraPlayAnimation.Location = new Point(468, 297);
            fraPlayAnimation.Margin = new Padding(4, 3, 4, 3);
            fraPlayAnimation.Name = "fraPlayAnimation";
            fraPlayAnimation.Padding = new Padding(4, 3, 4, 3);
            fraPlayAnimation.Size = new Size(289, 187);
            fraPlayAnimation.TabIndex = 36;
            fraPlayAnimation.TabStop = false;
            fraPlayAnimation.Text = "Play Animation";
            fraPlayAnimation.Visible = false;
            // 
            // btnPlayAnimationOk
            // 
            btnPlayAnimationOk.Location = new Point(100, 152);
            btnPlayAnimationOk.Margin = new Padding(4, 3, 4, 3);
            btnPlayAnimationOk.Name = "btnPlayAnimationOk";
            btnPlayAnimationOk.Padding = new Padding(6);
            btnPlayAnimationOk.Size = new Size(88, 27);
            btnPlayAnimationOk.TabIndex = 36;
            btnPlayAnimationOk.Text = "Ok";
            btnPlayAnimationOk.Click += BtnPlayAnimationOK_Click;
            // 
            // btnPlayAnimationCancel
            // 
            btnPlayAnimationCancel.Location = new Point(195, 152);
            btnPlayAnimationCancel.Margin = new Padding(4, 3, 4, 3);
            btnPlayAnimationCancel.Name = "btnPlayAnimationCancel";
            btnPlayAnimationCancel.Padding = new Padding(6);
            btnPlayAnimationCancel.Size = new Size(88, 27);
            btnPlayAnimationCancel.TabIndex = 35;
            btnPlayAnimationCancel.Text = "Cancel";
            btnPlayAnimationCancel.Click += BtnPlayAnimationCancel_Click;
            // 
            // lblPlayAnimY
            // 
            lblPlayAnimY.AutoSize = true;
            lblPlayAnimY.ForeColor = Color.FromArgb(220, 220, 220);
            lblPlayAnimY.Location = new Point(153, 122);
            lblPlayAnimY.Margin = new Padding(4, 0, 4, 0);
            lblPlayAnimY.Name = "lblPlayAnimY";
            lblPlayAnimY.Size = new Size(65, 15);
            lblPlayAnimY.TabIndex = 34;
            lblPlayAnimY.Text = "Map Tile Y:";
            // 
            // lblPlayAnimX
            // 
            lblPlayAnimX.AutoSize = true;
            lblPlayAnimX.ForeColor = Color.FromArgb(220, 220, 220);
            lblPlayAnimX.Location = new Point(7, 122);
            lblPlayAnimX.Margin = new Padding(4, 0, 4, 0);
            lblPlayAnimX.Name = "lblPlayAnimX";
            lblPlayAnimX.Size = new Size(65, 15);
            lblPlayAnimX.TabIndex = 33;
            lblPlayAnimX.Text = "Map Tile X:";
            // 
            // cmbPlayAnimEvent
            // 
            cmbPlayAnimEvent.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayAnimEvent.FormattingEnabled = true;
            cmbPlayAnimEvent.Location = new Point(97, 84);
            cmbPlayAnimEvent.Margin = new Padding(4, 3, 4, 3);
            cmbPlayAnimEvent.Name = "cmbPlayAnimEvent";
            cmbPlayAnimEvent.Size = new Size(185, 24);
            cmbPlayAnimEvent.TabIndex = 32;
            // 
            // cmbLabel62
            // 
            cmbLabel62.AutoSize = true;
            cmbLabel62.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel62.Location = new Point(5, 57);
            cmbLabel62.Margin = new Padding(4, 0, 4, 0);
            cmbLabel62.Name = "cmbLabel62";
            cmbLabel62.Size = new Size(66, 15);
            cmbLabel62.TabIndex = 31;
            cmbLabel62.Text = "Target Type";
            // 
            // cmbAnimTargetType
            // 
            cmbAnimTargetType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbAnimTargetType.FormattingEnabled = true;
            cmbAnimTargetType.Items.AddRange(new object[] { "Player", "Event", "Tile" });
            cmbAnimTargetType.Location = new Point(97, 53);
            cmbAnimTargetType.Margin = new Padding(4, 3, 4, 3);
            cmbAnimTargetType.Name = "cmbAnimTargetType";
            cmbAnimTargetType.Size = new Size(185, 24);
            cmbAnimTargetType.TabIndex = 30;
            // 
            // nudPlayAnimTileY
            // 
            nudPlayAnimTileY.Location = new Point(231, 120);
            nudPlayAnimTileY.Margin = new Padding(4, 3, 4, 3);
            nudPlayAnimTileY.Name = "nudPlayAnimTileY";
            nudPlayAnimTileY.Size = new Size(51, 23);
            nudPlayAnimTileY.TabIndex = 29;
            // 
            // nudPlayAnimTileX
            // 
            nudPlayAnimTileX.Location = new Point(85, 120);
            nudPlayAnimTileX.Margin = new Padding(4, 3, 4, 3);
            nudPlayAnimTileX.Name = "nudPlayAnimTileX";
            nudPlayAnimTileX.Size = new Size(51, 23);
            nudPlayAnimTileX.TabIndex = 28;
            // 
            // cmbLabel61
            // 
            cmbLabel61.AutoSize = true;
            cmbLabel61.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel61.Location = new Point(7, 25);
            cmbLabel61.Margin = new Padding(4, 0, 4, 0);
            cmbLabel61.Name = "cmbLabel61";
            cmbLabel61.Size = new Size(66, 15);
            cmbLabel61.TabIndex = 1;
            cmbLabel61.Text = "Animation:";
            // 
            // cmbPlayAnim
            // 
            cmbPlayAnim.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayAnim.FormattingEnabled = true;
            cmbPlayAnim.Location = new Point(72, 22);
            cmbPlayAnim.Margin = new Padding(4, 3, 4, 3);
            cmbPlayAnim.Name = "cmbPlayAnim";
            cmbPlayAnim.Size = new Size(209, 24);
            cmbPlayAnim.TabIndex = 0;
            // 
            // fraChangeSprite
            // 
            fraChangeSprite.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeSprite.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeSprite.Controls.Add(btnChangeSpriteOk);
            fraChangeSprite.Controls.Add(btnChangeSpriteCancel);
            fraChangeSprite.Controls.Add(cmbLabel48);
            fraChangeSprite.Controls.Add(nudChangeSprite);
            fraChangeSprite.Controls.Add(picChangeSprite);
            fraChangeSprite.ForeColor = Color.Gainsboro;
            fraChangeSprite.Location = new Point(468, 323);
            fraChangeSprite.Margin = new Padding(4, 3, 4, 3);
            fraChangeSprite.Name = "fraChangeSprite";
            fraChangeSprite.Padding = new Padding(4, 3, 4, 3);
            fraChangeSprite.Size = new Size(287, 135);
            fraChangeSprite.TabIndex = 30;
            fraChangeSprite.TabStop = false;
            fraChangeSprite.Text = "Change Sprite";
            fraChangeSprite.Visible = false;
            // 
            // btnChangeSpriteOk
            // 
            btnChangeSpriteOk.Location = new Point(98, 103);
            btnChangeSpriteOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeSpriteOk.Name = "btnChangeSpriteOk";
            btnChangeSpriteOk.Padding = new Padding(6);
            btnChangeSpriteOk.Size = new Size(88, 27);
            btnChangeSpriteOk.TabIndex = 30;
            btnChangeSpriteOk.Text = "Ok";
            btnChangeSpriteOk.Click += BtnChangeSpriteOK_Click;
            // 
            // btnChangeSpriteCancel
            // 
            btnChangeSpriteCancel.Location = new Point(192, 103);
            btnChangeSpriteCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeSpriteCancel.Name = "btnChangeSpriteCancel";
            btnChangeSpriteCancel.Padding = new Padding(6);
            btnChangeSpriteCancel.Size = new Size(88, 27);
            btnChangeSpriteCancel.TabIndex = 29;
            btnChangeSpriteCancel.Text = "Cancel";
            btnChangeSpriteCancel.Click += BtnChangeSpriteCancel_Click;
            // 
            // cmbLabel48
            // 
            cmbLabel48.AutoSize = true;
            cmbLabel48.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel48.Location = new Point(93, 77);
            cmbLabel48.Margin = new Padding(4, 0, 4, 0);
            cmbLabel48.Name = "cmbLabel48";
            cmbLabel48.Size = new Size(37, 15);
            cmbLabel48.TabIndex = 28;
            cmbLabel48.Text = "Sprite";
            // 
            // nudChangeSprite
            // 
            nudChangeSprite.Location = new Point(140, 73);
            nudChangeSprite.Margin = new Padding(4, 3, 4, 3);
            nudChangeSprite.Name = "nudChangeSprite";
            nudChangeSprite.Size = new Size(140, 23);
            nudChangeSprite.TabIndex = 27;
            // 
            // picChangeSprite
            // 
            picChangeSprite.BackColor = Color.Black;
            picChangeSprite.BackgroundImageLayout = ImageLayout.Zoom;
            picChangeSprite.Location = new Point(7, 22);
            picChangeSprite.Margin = new Padding(4, 3, 4, 3);
            picChangeSprite.Name = "picChangeSprite";
            picChangeSprite.Size = new Size(82, 107);
            picChangeSprite.TabIndex = 3;
            picChangeSprite.TabStop = false;
            // 
            // fraGoToLabel
            // 
            fraGoToLabel.BackColor = Color.FromArgb(45, 45, 48);
            fraGoToLabel.BorderColor = Color.FromArgb(90, 90, 90);
            fraGoToLabel.Controls.Add(btnGoToLabelOk);
            fraGoToLabel.Controls.Add(btnGoToLabelCancel);
            fraGoToLabel.Controls.Add(txtGoToLabel);
            fraGoToLabel.Controls.Add(cmbLabel60);
            fraGoToLabel.ForeColor = Color.Gainsboro;
            fraGoToLabel.Location = new Point(468, 294);
            fraGoToLabel.Margin = new Padding(4, 3, 4, 3);
            fraGoToLabel.Name = "fraGoToLabel";
            fraGoToLabel.Padding = new Padding(4, 3, 4, 3);
            fraGoToLabel.Size = new Size(289, 84);
            fraGoToLabel.TabIndex = 35;
            fraGoToLabel.TabStop = false;
            fraGoToLabel.Text = "GoTo Label";
            fraGoToLabel.Visible = false;
            // 
            // btnGoToLabelOk
            // 
            btnGoToLabelOk.Location = new Point(100, 51);
            btnGoToLabelOk.Margin = new Padding(4, 3, 4, 3);
            btnGoToLabelOk.Name = "btnGoToLabelOk";
            btnGoToLabelOk.Padding = new Padding(6);
            btnGoToLabelOk.Size = new Size(88, 27);
            btnGoToLabelOk.TabIndex = 27;
            btnGoToLabelOk.Text = "Ok";
            btnGoToLabelOk.Click += BtnGoToLabelOk_Click;
            // 
            // btnGoToLabelCancel
            // 
            btnGoToLabelCancel.Location = new Point(195, 51);
            btnGoToLabelCancel.Margin = new Padding(4, 3, 4, 3);
            btnGoToLabelCancel.Name = "btnGoToLabelCancel";
            btnGoToLabelCancel.Padding = new Padding(6);
            btnGoToLabelCancel.Size = new Size(88, 27);
            btnGoToLabelCancel.TabIndex = 26;
            btnGoToLabelCancel.Text = "Cancel";
            btnGoToLabelCancel.Click += BtnGoToLabelCancel_Click;
            // 
            // txtGoToLabel
            // 
            txtGoToLabel.BackColor = Color.FromArgb(69, 73, 74);
            txtGoToLabel.BorderStyle = BorderStyle.FixedSingle;
            txtGoToLabel.ForeColor = Color.FromArgb(220, 220, 220);
            txtGoToLabel.Location = new Point(91, 21);
            txtGoToLabel.Margin = new Padding(4, 3, 4, 3);
            txtGoToLabel.Name = "txtGoToLabel";
            txtGoToLabel.Size = new Size(191, 23);
            txtGoToLabel.TabIndex = 1;
            // 
            // cmbLabel60
            // 
            cmbLabel60.AutoSize = true;
            cmbLabel60.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel60.Location = new Point(4, 23);
            cmbLabel60.Margin = new Padding(4, 0, 4, 0);
            cmbLabel60.Name = "cmbLabel60";
            cmbLabel60.Size = new Size(73, 15);
            cmbLabel60.TabIndex = 0;
            cmbLabel60.Text = "Label Name:";
            // 
            // fraMapTint
            // 
            fraMapTint.BackColor = Color.FromArgb(45, 45, 48);
            fraMapTint.BorderColor = Color.FromArgb(90, 90, 90);
            fraMapTint.Controls.Add(btnMapTintOk);
            fraMapTint.Controls.Add(btnMapTintCancel);
            fraMapTint.Controls.Add(cmbLabel42);
            fraMapTint.Controls.Add(nudMapTintData3);
            fraMapTint.Controls.Add(nudMapTintData2);
            fraMapTint.Controls.Add(cmbLabel43);
            fraMapTint.Controls.Add(cmbLabel44);
            fraMapTint.Controls.Add(nudMapTintData1);
            fraMapTint.Controls.Add(nudMapTintData0);
            fraMapTint.Controls.Add(cmbLabel45);
            fraMapTint.ForeColor = Color.Gainsboro;
            fraMapTint.Location = new Point(468, 209);
            fraMapTint.Margin = new Padding(4, 3, 4, 3);
            fraMapTint.Name = "fraMapTint";
            fraMapTint.Padding = new Padding(4, 3, 4, 3);
            fraMapTint.Size = new Size(287, 167);
            fraMapTint.TabIndex = 28;
            fraMapTint.TabStop = false;
            fraMapTint.Text = "Map Tinting";
            fraMapTint.Visible = false;
            // 
            // btnMapTintOk
            // 
            btnMapTintOk.Location = new Point(98, 133);
            btnMapTintOk.Margin = new Padding(4, 3, 4, 3);
            btnMapTintOk.Name = "btnMapTintOk";
            btnMapTintOk.Padding = new Padding(6);
            btnMapTintOk.Size = new Size(88, 27);
            btnMapTintOk.TabIndex = 45;
            btnMapTintOk.Text = "Ok";
            btnMapTintOk.Click += BtnMapTintOK_Click;
            // 
            // btnMapTintCancel
            // 
            btnMapTintCancel.Location = new Point(192, 133);
            btnMapTintCancel.Margin = new Padding(4, 3, 4, 3);
            btnMapTintCancel.Name = "btnMapTintCancel";
            btnMapTintCancel.Padding = new Padding(6);
            btnMapTintCancel.Size = new Size(88, 27);
            btnMapTintCancel.TabIndex = 44;
            btnMapTintCancel.Text = "Cancel";
            btnMapTintCancel.Click += BtnMapTintCancel_Click;
            // 
            // cmbLabel42
            // 
            cmbLabel42.AutoSize = true;
            cmbLabel42.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel42.Location = new Point(6, 107);
            cmbLabel42.Margin = new Padding(4, 0, 4, 0);
            cmbLabel42.Name = "cmbLabel42";
            cmbLabel42.Size = new Size(51, 15);
            cmbLabel42.TabIndex = 43;
            cmbLabel42.Text = "Opacity:";
            // 
            // nudMapTintData3
            // 
            nudMapTintData3.Location = new Point(111, 103);
            nudMapTintData3.Margin = new Padding(4, 3, 4, 3);
            nudMapTintData3.Name = "nudMapTintData3";
            nudMapTintData3.Size = new Size(168, 23);
            nudMapTintData3.TabIndex = 42;
            // 
            // nudMapTintData2
            // 
            nudMapTintData2.Location = new Point(111, 74);
            nudMapTintData2.Margin = new Padding(4, 3, 4, 3);
            nudMapTintData2.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMapTintData2.Name = "nudMapTintData2";
            nudMapTintData2.Size = new Size(168, 23);
            nudMapTintData2.TabIndex = 41;
            // 
            // cmbLabel43
            // 
            cmbLabel43.AutoSize = true;
            cmbLabel43.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel43.Location = new Point(6, 76);
            cmbLabel43.Margin = new Padding(4, 0, 4, 0);
            cmbLabel43.Name = "cmbLabel43";
            cmbLabel43.Size = new Size(33, 15);
            cmbLabel43.TabIndex = 40;
            cmbLabel43.Text = "Blue:";
            // 
            // cmbLabel44
            // 
            cmbLabel44.AutoSize = true;
            cmbLabel44.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel44.Location = new Point(5, 50);
            cmbLabel44.Margin = new Padding(4, 0, 4, 0);
            cmbLabel44.Name = "cmbLabel44";
            cmbLabel44.Size = new Size(41, 15);
            cmbLabel44.TabIndex = 39;
            cmbLabel44.Text = "Green:";
            // 
            // nudMapTintData1
            // 
            nudMapTintData1.Location = new Point(111, 45);
            nudMapTintData1.Margin = new Padding(4, 3, 4, 3);
            nudMapTintData1.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMapTintData1.Name = "nudMapTintData1";
            nudMapTintData1.Size = new Size(168, 23);
            nudMapTintData1.TabIndex = 38;
            // 
            // nudMapTintData0
            // 
            nudMapTintData0.Location = new Point(111, 16);
            nudMapTintData0.Margin = new Padding(4, 3, 4, 3);
            nudMapTintData0.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudMapTintData0.Name = "nudMapTintData0";
            nudMapTintData0.Size = new Size(168, 23);
            nudMapTintData0.TabIndex = 37;
            // 
            // cmbLabel45
            // 
            cmbLabel45.AutoSize = true;
            cmbLabel45.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel45.Location = new Point(6, 18);
            cmbLabel45.Margin = new Padding(4, 0, 4, 0);
            cmbLabel45.Name = "cmbLabel45";
            cmbLabel45.Size = new Size(30, 15);
            cmbLabel45.TabIndex = 36;
            cmbLabel45.Text = "Red:";
            // 
            // fraShowPic
            // 
            fraShowPic.BackColor = Color.FromArgb(45, 45, 48);
            fraShowPic.BorderColor = Color.FromArgb(90, 90, 90);
            fraShowPic.Controls.Add(btnShowPicOk);
            fraShowPic.Controls.Add(btnShowPicCancel);
            fraShowPic.Controls.Add(cmbLabel71);
            fraShowPic.Controls.Add(cmbLabel70);
            fraShowPic.Controls.Add(cmbLabel67);
            fraShowPic.Controls.Add(cmbLabel68);
            fraShowPic.Controls.Add(nudPicOffsetY);
            fraShowPic.Controls.Add(nudPicOffsetX);
            fraShowPic.Controls.Add(cmbLabel69);
            fraShowPic.Controls.Add(cmbPicLoc);
            fraShowPic.Controls.Add(nudShowPicture);
            fraShowPic.Controls.Add(picShowPic);
            fraShowPic.ForeColor = Color.Gainsboro;
            fraShowPic.Location = new Point(1, 0);
            fraShowPic.Margin = new Padding(4, 3, 4, 3);
            fraShowPic.Name = "fraShowPic";
            fraShowPic.Padding = new Padding(4, 3, 4, 3);
            fraShowPic.Size = new Size(775, 686);
            fraShowPic.TabIndex = 40;
            fraShowPic.TabStop = false;
            fraShowPic.Text = "Show Picture";
            fraShowPic.Visible = false;
            // 
            // btnShowPicOk
            // 
            btnShowPicOk.Location = new Point(583, 651);
            btnShowPicOk.Margin = new Padding(4, 3, 4, 3);
            btnShowPicOk.Name = "btnShowPicOk";
            btnShowPicOk.Padding = new Padding(6);
            btnShowPicOk.Size = new Size(88, 27);
            btnShowPicOk.TabIndex = 55;
            btnShowPicOk.Text = "Ok";
            btnShowPicOk.Click += BtnShowPicOK_Click;
            // 
            // btnShowPicCancel
            // 
            btnShowPicCancel.Location = new Point(679, 651);
            btnShowPicCancel.Margin = new Padding(4, 3, 4, 3);
            btnShowPicCancel.Name = "btnShowPicCancel";
            btnShowPicCancel.Padding = new Padding(6);
            btnShowPicCancel.Size = new Size(88, 27);
            btnShowPicCancel.TabIndex = 54;
            btnShowPicCancel.Text = "Cancel";
            btnShowPicCancel.Click += BtnShowPicCancel_Click;
            // 
            // cmbLabel71
            // 
            cmbLabel71.AutoSize = true;
            cmbLabel71.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel71.Location = new Point(287, 27);
            cmbLabel71.Margin = new Padding(4, 0, 4, 0);
            cmbLabel71.Name = "cmbLabel71";
            cmbLabel71.Size = new Size(120, 15);
            cmbLabel71.TabIndex = 53;
            cmbLabel71.Text = "Offset from Location:";
            // 
            // cmbLabel70
            // 
            cmbLabel70.AutoSize = true;
            cmbLabel70.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel70.Location = new Point(130, 65);
            cmbLabel70.Margin = new Padding(4, 0, 4, 0);
            cmbLabel70.Name = "cmbLabel70";
            cmbLabel70.Size = new Size(53, 15);
            cmbLabel70.TabIndex = 52;
            cmbLabel70.Text = "Location";
            // 
            // cmbLabel67
            // 
            cmbLabel67.AutoSize = true;
            cmbLabel67.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel67.Location = new Point(435, 54);
            cmbLabel67.Margin = new Padding(4, 0, 4, 0);
            cmbLabel67.Name = "cmbLabel67";
            cmbLabel67.Size = new Size(17, 15);
            cmbLabel67.TabIndex = 51;
            cmbLabel67.Text = "Y:";
            // 
            // cmbLabel68
            // 
            cmbLabel68.AutoSize = true;
            cmbLabel68.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel68.Location = new Point(287, 56);
            cmbLabel68.Margin = new Padding(4, 0, 4, 0);
            cmbLabel68.Name = "cmbLabel68";
            cmbLabel68.Size = new Size(17, 15);
            cmbLabel68.TabIndex = 50;
            cmbLabel68.Text = "X:";
            // 
            // nudPicOffsetY
            // 
            nudPicOffsetY.Location = new Point(486, 52);
            nudPicOffsetY.Margin = new Padding(4, 3, 4, 3);
            nudPicOffsetY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudPicOffsetY.Name = "nudPicOffsetY";
            nudPicOffsetY.Size = new Size(66, 23);
            nudPicOffsetY.TabIndex = 49;
            // 
            // nudPicOffsetX
            // 
            nudPicOffsetX.Location = new Point(336, 52);
            nudPicOffsetX.Margin = new Padding(4, 3, 4, 3);
            nudPicOffsetX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            nudPicOffsetX.Name = "nudPicOffsetX";
            nudPicOffsetX.Size = new Size(66, 23);
            nudPicOffsetX.TabIndex = 48;
            // 
            // cmbLabel69
            // 
            cmbLabel69.AutoSize = true;
            cmbLabel69.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel69.Location = new Point(130, 26);
            cmbLabel69.Margin = new Padding(4, 0, 4, 0);
            cmbLabel69.Name = "cmbLabel69";
            cmbLabel69.Size = new Size(47, 15);
            cmbLabel69.TabIndex = 47;
            cmbLabel69.Text = "Picture:";
            // 
            // cmbPicLoc
            // 
            cmbPicLoc.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPicLoc.FormattingEnabled = true;
            cmbPicLoc.Items.AddRange(new object[] { "Top Left of Screen", "Center Screen", "Centered on Event", "Centered on Player" });
            cmbPicLoc.Location = new Point(133, 86);
            cmbPicLoc.Margin = new Padding(4, 3, 4, 3);
            cmbPicLoc.Name = "cmbPicLoc";
            cmbPicLoc.Size = new Size(144, 24);
            cmbPicLoc.TabIndex = 46;
            // 
            // nudShowPicture
            // 
            nudShowPicture.Location = new Point(186, 24);
            nudShowPicture.Margin = new Padding(4, 3, 4, 3);
            nudShowPicture.Name = "nudShowPicture";
            nudShowPicture.Size = new Size(88, 23);
            nudShowPicture.TabIndex = 45;
            nudShowPicture.Click += nudShowPicture_Click;
            // 
            // picShowPic
            // 
            picShowPic.BackColor = Color.Black;
            picShowPic.BackgroundImageLayout = ImageLayout.Stretch;
            picShowPic.Location = new Point(9, 21);
            picShowPic.Margin = new Padding(4, 3, 4, 3);
            picShowPic.Name = "picShowPic";
            picShowPic.Size = new Size(117, 107);
            picShowPic.TabIndex = 42;
            picShowPic.TabStop = false;
            // 
            // fraConditionalBranch
            // 
            fraConditionalBranch.BackColor = Color.FromArgb(45, 45, 48);
            fraConditionalBranch.BorderColor = Color.FromArgb(90, 90, 90);
            fraConditionalBranch.Controls.Add(cmbCondition_Time);
            fraConditionalBranch.Controls.Add(optCondition9);
            fraConditionalBranch.Controls.Add(btnConditionalBranchOk);
            fraConditionalBranch.Controls.Add(btnConditionalBranchCancel);
            fraConditionalBranch.Controls.Add(cmbCondition_Gender);
            fraConditionalBranch.Controls.Add(optCondition8);
            fraConditionalBranch.Controls.Add(cmbCondition_SelfSwitchCondition);
            fraConditionalBranch.Controls.Add(cmbLabel17);
            fraConditionalBranch.Controls.Add(cmbCondition_SelfSwitch);
            fraConditionalBranch.Controls.Add(optCondition6);
            fraConditionalBranch.Controls.Add(nudCondition_LevelAmount);
            fraConditionalBranch.Controls.Add(optCondition5);
            fraConditionalBranch.Controls.Add(cmbCondition_LevelCompare);
            fraConditionalBranch.Controls.Add(cmbCondition_LearntSkill);
            fraConditionalBranch.Controls.Add(optCondition4);
            fraConditionalBranch.Controls.Add(cmbCondition_JobIs);
            fraConditionalBranch.Controls.Add(optCondition3);
            fraConditionalBranch.Controls.Add(nudCondition_HasItem);
            fraConditionalBranch.Controls.Add(cmbLabel16);
            fraConditionalBranch.Controls.Add(cmbCondition_HasItem);
            fraConditionalBranch.Controls.Add(optCondition2);
            fraConditionalBranch.Controls.Add(optCondition1);
            fraConditionalBranch.Controls.Add(cmbLabel15);
            fraConditionalBranch.Controls.Add(cmbCondtion_PlayerSwitchCondition);
            fraConditionalBranch.Controls.Add(cmbCondition_PlayerSwitch);
            fraConditionalBranch.Controls.Add(nudCondition_PlayerVarCondition);
            fraConditionalBranch.Controls.Add(cmbCondition_PlayerVarCompare);
            fraConditionalBranch.Controls.Add(cmbLabel14);
            fraConditionalBranch.Controls.Add(cmbCondition_PlayerVarIndex);
            fraConditionalBranch.Controls.Add(optCondition0);
            fraConditionalBranch.ForeColor = Color.Gainsboro;
            fraConditionalBranch.Location = new Point(7, 8);
            fraConditionalBranch.Margin = new Padding(4, 3, 4, 3);
            fraConditionalBranch.Name = "fraConditionalBranch";
            fraConditionalBranch.Padding = new Padding(4, 3, 4, 3);
            fraConditionalBranch.Size = new Size(454, 516);
            fraConditionalBranch.TabIndex = 0;
            fraConditionalBranch.TabStop = false;
            fraConditionalBranch.Text = "Conditional Branch";
            fraConditionalBranch.Visible = false;
            // 
            // cmbCondition_Time
            // 
            cmbCondition_Time.DrawMode = DrawMode.OwnerDrawVariable;
            cmbCondition_Time.FormattingEnabled = true;
            cmbCondition_Time.Items.AddRange(new object[] { "Day", "Night", "Dawn", "Dusk" });
            cmbCondition_Time.Location = new Point(279, 267);
            cmbCondition_Time.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_Time.Name = "cmbCondition_Time";
            cmbCondition_Time.Size = new Size(167, 24);
            cmbCondition_Time.TabIndex = 33;
            // 
            // optCondition9
            // 
            optCondition9.AutoSize = true;
            optCondition9.Location = new Point(7, 268);
            optCondition9.Margin = new Padding(4, 3, 4, 3);
            optCondition9.Name = "optCondition9";
            optCondition9.Size = new Size(102, 19);
            optCondition9.TabIndex = 32;
            optCondition9.TabStop = true;
            optCondition9.Text = "Time of Day is:";
            optCondition9.CheckedChanged += OptCondition9_CheckedChanged;
            // 
            // btnConditionalBranchOk
            // 
            btnConditionalBranchOk.Location = new Point(264, 480);
            btnConditionalBranchOk.Margin = new Padding(4, 3, 4, 3);
            btnConditionalBranchOk.Name = "btnConditionalBranchOk";
            btnConditionalBranchOk.Padding = new Padding(6);
            btnConditionalBranchOk.Size = new Size(88, 27);
            btnConditionalBranchOk.TabIndex = 31;
            btnConditionalBranchOk.Text = "Ok";
            btnConditionalBranchOk.Click += BtnConditionalBranchOk_Click;
            // 
            // btnConditionalBranchCancel
            // 
            btnConditionalBranchCancel.Location = new Point(358, 480);
            btnConditionalBranchCancel.Margin = new Padding(4, 3, 4, 3);
            btnConditionalBranchCancel.Name = "btnConditionalBranchCancel";
            btnConditionalBranchCancel.Padding = new Padding(6);
            btnConditionalBranchCancel.Size = new Size(88, 27);
            btnConditionalBranchCancel.TabIndex = 30;
            btnConditionalBranchCancel.Text = "Cancel";
            btnConditionalBranchCancel.Click += BtnConditionalBranchCancel_Click;
            // 
            // cmbCondition_Gender
            // 
            cmbCondition_Gender.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_Gender.FormattingEnabled = true;
            cmbCondition_Gender.Items.AddRange(new object[] { "Male", "Female" });
            cmbCondition_Gender.Location = new Point(279, 236);
            cmbCondition_Gender.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_Gender.Name = "cmbCondition_Gender";
            cmbCondition_Gender.Size = new Size(167, 24);
            cmbCondition_Gender.TabIndex = 29;
            // 
            // optCondition8
            // 
            optCondition8.AutoSize = true;
            optCondition8.Location = new Point(7, 237);
            optCondition8.Margin = new Padding(4, 3, 4, 3);
            optCondition8.Name = "optCondition8";
            optCondition8.Size = new Size(109, 19);
            optCondition8.TabIndex = 28;
            optCondition8.TabStop = true;
            optCondition8.Text = "Player Gender is";
            optCondition8.CheckedChanged += OptCondition8_CheckedChanged;
            // 
            // cmbCondition_SelfSwitchCondition
            // 
            cmbCondition_SelfSwitchCondition.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_SelfSwitchCondition.FormattingEnabled = true;
            cmbCondition_SelfSwitchCondition.Items.AddRange(new object[] { "False", "True" });
            cmbCondition_SelfSwitchCondition.Location = new Point(306, 211);
            cmbCondition_SelfSwitchCondition.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_SelfSwitchCondition.Name = "cmbCondition_SelfSwitchCondition";
            cmbCondition_SelfSwitchCondition.Size = new Size(140, 24);
            cmbCondition_SelfSwitchCondition.TabIndex = 23;
            // 
            // cmbLabel17
            // 
            cmbLabel17.AutoSize = true;
            cmbLabel17.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel17.Location = new Point(273, 215);
            cmbLabel17.Margin = new Padding(4, 0, 4, 0);
            cmbLabel17.Name = "cmbLabel17";
            cmbLabel17.Size = new Size(15, 15);
            cmbLabel17.TabIndex = 22;
            cmbLabel17.Text = "is";
            // 
            // cmbCondition_SelfSwitch
            // 
            cmbCondition_SelfSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_SelfSwitch.FormattingEnabled = true;
            cmbCondition_SelfSwitch.Location = new Point(125, 211);
            cmbCondition_SelfSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_SelfSwitch.Name = "cmbCondition_SelfSwitch";
            cmbCondition_SelfSwitch.Size = new Size(140, 24);
            cmbCondition_SelfSwitch.TabIndex = 21;
            // 
            // optCondition6
            // 
            optCondition6.AutoSize = true;
            optCondition6.Location = new Point(7, 212);
            optCondition6.Margin = new Padding(4, 3, 4, 3);
            optCondition6.Name = "optCondition6";
            optCondition6.Size = new Size(82, 19);
            optCondition6.TabIndex = 20;
            optCondition6.TabStop = true;
            optCondition6.Text = "Self Switch";
            optCondition6.CheckedChanged += OptCondition6_CheckedChanged;
            // 
            // nudCondition_LevelAmount
            // 
            nudCondition_LevelAmount.Location = new Point(314, 181);
            nudCondition_LevelAmount.Margin = new Padding(4, 3, 4, 3);
            nudCondition_LevelAmount.Name = "nudCondition_LevelAmount";
            nudCondition_LevelAmount.Size = new Size(132, 23);
            nudCondition_LevelAmount.TabIndex = 19;
            // 
            // optCondition5
            // 
            optCondition5.AutoSize = true;
            optCondition5.Location = new Point(7, 181);
            optCondition5.Margin = new Padding(4, 3, 4, 3);
            optCondition5.Name = "optCondition5";
            optCondition5.Size = new Size(63, 19);
            optCondition5.TabIndex = 18;
            optCondition5.TabStop = true;
            optCondition5.Text = "Level is";
            optCondition5.CheckedChanged += OptCondition5_CheckedChanged;
            // 
            // cmbCondition_LevelCompare
            // 
            cmbCondition_LevelCompare.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_LevelCompare.FormattingEnabled = true;
            cmbCondition_LevelCompare.Items.AddRange(new object[] { "Equal To", "Great Than Or Equal To", "Less Than or Equal To", "Greater Than", "Less Than", "Does Not Equal" });
            cmbCondition_LevelCompare.Location = new Point(125, 180);
            cmbCondition_LevelCompare.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_LevelCompare.Name = "cmbCondition_LevelCompare";
            cmbCondition_LevelCompare.Size = new Size(181, 24);
            cmbCondition_LevelCompare.TabIndex = 17;
            // 
            // cmbCondition_LearntSkill
            // 
            cmbCondition_LearntSkill.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_LearntSkill.FormattingEnabled = true;
            cmbCondition_LearntSkill.Location = new Point(125, 149);
            cmbCondition_LearntSkill.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_LearntSkill.Name = "cmbCondition_LearntSkill";
            cmbCondition_LearntSkill.Size = new Size(321, 24);
            cmbCondition_LearntSkill.TabIndex = 16;
            // 
            // optCondition4
            // 
            optCondition4.AutoSize = true;
            optCondition4.Location = new Point(7, 150);
            optCondition4.Margin = new Padding(4, 3, 4, 3);
            optCondition4.Name = "optCondition4";
            optCondition4.Size = new Size(84, 19);
            optCondition4.TabIndex = 15;
            optCondition4.TabStop = true;
            optCondition4.Text = "Knows Skill";
            optCondition4.CheckedChanged += OptCondition4_CheckedChanged;
            // 
            // cmbCondition_JobIs
            // 
            cmbCondition_JobIs.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_JobIs.FormattingEnabled = true;
            cmbCondition_JobIs.Location = new Point(125, 118);
            cmbCondition_JobIs.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_JobIs.Name = "cmbCondition_JobIs";
            cmbCondition_JobIs.Size = new Size(321, 24);
            cmbCondition_JobIs.TabIndex = 14;
            // 
            // optCondition3
            // 
            optCondition3.AutoSize = true;
            optCondition3.Location = new Point(7, 119);
            optCondition3.Margin = new Padding(4, 3, 4, 3);
            optCondition3.Name = "optCondition3";
            optCondition3.Size = new Size(54, 19);
            optCondition3.TabIndex = 13;
            optCondition3.TabStop = true;
            optCondition3.Text = "Job Is";
            optCondition3.CheckedChanged += OptCondition3_CheckedChanged;
            // 
            // nudCondition_HasItem
            // 
            nudCondition_HasItem.Location = new Point(306, 88);
            nudCondition_HasItem.Margin = new Padding(4, 3, 4, 3);
            nudCondition_HasItem.Name = "nudCondition_HasItem";
            nudCondition_HasItem.Size = new Size(140, 23);
            nudCondition_HasItem.TabIndex = 12;
            // 
            // cmbLabel16
            // 
            cmbLabel16.AutoSize = true;
            cmbLabel16.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel16.Location = new Point(273, 90);
            cmbLabel16.Margin = new Padding(4, 0, 4, 0);
            cmbLabel16.Name = "cmbLabel16";
            cmbLabel16.Size = new Size(14, 15);
            cmbLabel16.TabIndex = 11;
            cmbLabel16.Text = "X";
            // 
            // cmbCondition_HasItem
            // 
            cmbCondition_HasItem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_HasItem.FormattingEnabled = true;
            cmbCondition_HasItem.Location = new Point(125, 87);
            cmbCondition_HasItem.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_HasItem.Name = "cmbCondition_HasItem";
            cmbCondition_HasItem.Size = new Size(140, 24);
            cmbCondition_HasItem.TabIndex = 10;
            // 
            // optCondition2
            // 
            optCondition2.AutoSize = true;
            optCondition2.Location = new Point(7, 88);
            optCondition2.Margin = new Padding(4, 3, 4, 3);
            optCondition2.Name = "optCondition2";
            optCondition2.Size = new Size(72, 19);
            optCondition2.TabIndex = 9;
            optCondition2.TabStop = true;
            optCondition2.Text = "Has Item";
            optCondition2.CheckedChanged += OptCondition2_CheckedChanged;
            // 
            // optCondition1
            // 
            optCondition1.AutoSize = true;
            optCondition1.Location = new Point(7, 57);
            optCondition1.Margin = new Padding(4, 3, 4, 3);
            optCondition1.Name = "optCondition1";
            optCondition1.Size = new Size(95, 19);
            optCondition1.TabIndex = 8;
            optCondition1.TabStop = true;
            optCondition1.Text = "Player Switch";
            optCondition1.CheckedChanged += OptCondition1_CheckedChanged;
            // 
            // cmbLabel15
            // 
            cmbLabel15.AutoSize = true;
            cmbLabel15.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel15.Location = new Point(273, 59);
            cmbLabel15.Margin = new Padding(4, 0, 4, 0);
            cmbLabel15.Name = "cmbLabel15";
            cmbLabel15.Size = new Size(15, 15);
            cmbLabel15.TabIndex = 7;
            cmbLabel15.Text = "is";
            // 
            // cmbCondtion_PlayerSwitchCondition
            // 
            cmbCondtion_PlayerSwitchCondition.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondtion_PlayerSwitchCondition.FormattingEnabled = true;
            cmbCondtion_PlayerSwitchCondition.Items.AddRange(new object[] { "False", "True" });
            cmbCondtion_PlayerSwitchCondition.Location = new Point(306, 55);
            cmbCondtion_PlayerSwitchCondition.Margin = new Padding(4, 3, 4, 3);
            cmbCondtion_PlayerSwitchCondition.Name = "cmbCondtion_PlayerSwitchCondition";
            cmbCondtion_PlayerSwitchCondition.Size = new Size(140, 24);
            cmbCondtion_PlayerSwitchCondition.TabIndex = 6;
            // 
            // cmbCondition_PlayerSwitch
            // 
            cmbCondition_PlayerSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_PlayerSwitch.FormattingEnabled = true;
            cmbCondition_PlayerSwitch.Location = new Point(125, 55);
            cmbCondition_PlayerSwitch.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_PlayerSwitch.Name = "cmbCondition_PlayerSwitch";
            cmbCondition_PlayerSwitch.Size = new Size(140, 24);
            cmbCondition_PlayerSwitch.TabIndex = 5;
            // 
            // nudCondition_PlayerVarCondition
            // 
            nudCondition_PlayerVarCondition.Location = new Point(391, 25);
            nudCondition_PlayerVarCondition.Margin = new Padding(4, 3, 4, 3);
            nudCondition_PlayerVarCondition.Name = "nudCondition_PlayerVarCondition";
            nudCondition_PlayerVarCondition.Size = new Size(55, 23);
            nudCondition_PlayerVarCondition.TabIndex = 4;
            // 
            // cmbCondition_PlayerVarCompare
            // 
            cmbCondition_PlayerVarCompare.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_PlayerVarCompare.FormattingEnabled = true;
            cmbCondition_PlayerVarCompare.Items.AddRange(new object[] { "Equal To", "Great Than Or Equal To", "Less Than or Equal To", "Greater Than", "Less Than", "Does Not Equal" });
            cmbCondition_PlayerVarCompare.Location = new Point(275, 24);
            cmbCondition_PlayerVarCompare.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_PlayerVarCompare.Name = "cmbCondition_PlayerVarCompare";
            cmbCondition_PlayerVarCompare.Size = new Size(102, 24);
            cmbCondition_PlayerVarCompare.TabIndex = 3;
            // 
            // cmbLabel14
            // 
            cmbLabel14.AutoSize = true;
            cmbLabel14.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel14.Location = new Point(252, 28);
            cmbLabel14.Margin = new Padding(4, 0, 4, 0);
            cmbLabel14.Name = "cmbLabel14";
            cmbLabel14.Size = new Size(15, 15);
            cmbLabel14.TabIndex = 2;
            cmbLabel14.Text = "is";
            // 
            // cmbCondition_PlayerVarIndex
            // 
            cmbCondition_PlayerVarIndex.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCondition_PlayerVarIndex.FormattingEnabled = true;
            cmbCondition_PlayerVarIndex.Location = new Point(125, 24);
            cmbCondition_PlayerVarIndex.Margin = new Padding(4, 3, 4, 3);
            cmbCondition_PlayerVarIndex.Name = "cmbCondition_PlayerVarIndex";
            cmbCondition_PlayerVarIndex.Size = new Size(120, 24);
            cmbCondition_PlayerVarIndex.TabIndex = 1;
            // 
            // optCondition0
            // 
            optCondition0.AutoSize = true;
            optCondition0.Location = new Point(7, 25);
            optCondition0.Margin = new Padding(4, 3, 4, 3);
            optCondition0.Name = "optCondition0";
            optCondition0.Size = new Size(101, 19);
            optCondition0.TabIndex = 0;
            optCondition0.TabStop = true;
            optCondition0.Text = "Player Variable";
            optCondition0.CheckedChanged += OptCondition_Index0_CheckedChanged;
            // 
            // fraPlayBGM
            // 
            fraPlayBGM.BackColor = Color.FromArgb(45, 45, 48);
            fraPlayBGM.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlayBGM.Controls.Add(btnPlayBgmOk);
            fraPlayBGM.Controls.Add(btnPlayBgmCancel);
            fraPlayBGM.Controls.Add(cmbPlayBGM);
            fraPlayBGM.ForeColor = Color.Gainsboro;
            fraPlayBGM.Location = new Point(468, 1);
            fraPlayBGM.Margin = new Padding(4, 3, 4, 3);
            fraPlayBGM.Name = "fraPlayBGM";
            fraPlayBGM.Padding = new Padding(4, 3, 4, 3);
            fraPlayBGM.Size = new Size(287, 87);
            fraPlayBGM.TabIndex = 21;
            fraPlayBGM.TabStop = false;
            fraPlayBGM.Text = "Play BGM";
            fraPlayBGM.Visible = false;
            // 
            // btnPlayBgmOk
            // 
            btnPlayBgmOk.Location = new Point(54, 53);
            btnPlayBgmOk.Margin = new Padding(4, 3, 4, 3);
            btnPlayBgmOk.Name = "btnPlayBgmOk";
            btnPlayBgmOk.Padding = new Padding(6);
            btnPlayBgmOk.Size = new Size(88, 27);
            btnPlayBgmOk.TabIndex = 27;
            btnPlayBgmOk.Text = "Ok";
            btnPlayBgmOk.Click += BtnPlayBgmOK_Click;
            // 
            // btnPlayBgmCancel
            // 
            btnPlayBgmCancel.Location = new Point(148, 53);
            btnPlayBgmCancel.Margin = new Padding(4, 3, 4, 3);
            btnPlayBgmCancel.Name = "btnPlayBgmCancel";
            btnPlayBgmCancel.Padding = new Padding(6);
            btnPlayBgmCancel.Size = new Size(88, 27);
            btnPlayBgmCancel.TabIndex = 26;
            btnPlayBgmCancel.Text = "Cancel";
            btnPlayBgmCancel.Click += BtnPlayBgmCancel_Click;
            // 
            // cmbPlayBGM
            // 
            cmbPlayBGM.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPlayBGM.FormattingEnabled = true;
            cmbPlayBGM.Location = new Point(7, 22);
            cmbPlayBGM.Margin = new Padding(4, 3, 4, 3);
            cmbPlayBGM.Name = "cmbPlayBGM";
            cmbPlayBGM.Size = new Size(271, 24);
            cmbPlayBGM.TabIndex = 0;
            // 
            // fraPlayerWarp
            // 
            fraPlayerWarp.BackColor = Color.FromArgb(45, 45, 48);
            fraPlayerWarp.BorderColor = Color.FromArgb(90, 90, 90);
            fraPlayerWarp.Controls.Add(btnPlayerWarpOk);
            fraPlayerWarp.Controls.Add(btnPlayerWarpCancel);
            fraPlayerWarp.Controls.Add(cmbLabel31);
            fraPlayerWarp.Controls.Add(cmbWarpPlayerDir);
            fraPlayerWarp.Controls.Add(nudWPY);
            fraPlayerWarp.Controls.Add(cmbLabel32);
            fraPlayerWarp.Controls.Add(nudWPX);
            fraPlayerWarp.Controls.Add(cmbLabel33);
            fraPlayerWarp.Controls.Add(nudWPMap);
            fraPlayerWarp.Controls.Add(cmbLabel34);
            fraPlayerWarp.ForeColor = Color.Gainsboro;
            fraPlayerWarp.Location = new Point(468, 7);
            fraPlayerWarp.Margin = new Padding(4, 3, 4, 3);
            fraPlayerWarp.Name = "fraPlayerWarp";
            fraPlayerWarp.Padding = new Padding(4, 3, 4, 3);
            fraPlayerWarp.Size = new Size(287, 112);
            fraPlayerWarp.TabIndex = 19;
            fraPlayerWarp.TabStop = false;
            fraPlayerWarp.Text = "Warp Player";
            fraPlayerWarp.Visible = false;
            // 
            // btnPlayerWarpOk
            // 
            btnPlayerWarpOk.Location = new Point(97, 78);
            btnPlayerWarpOk.Margin = new Padding(4, 3, 4, 3);
            btnPlayerWarpOk.Name = "btnPlayerWarpOk";
            btnPlayerWarpOk.Padding = new Padding(6);
            btnPlayerWarpOk.Size = new Size(88, 27);
            btnPlayerWarpOk.TabIndex = 46;
            btnPlayerWarpOk.Text = "Ok";
            btnPlayerWarpOk.Click += BtnPlayerWarpOK_Click;
            // 
            // btnPlayerWarpCancel
            // 
            btnPlayerWarpCancel.Location = new Point(191, 78);
            btnPlayerWarpCancel.Margin = new Padding(4, 3, 4, 3);
            btnPlayerWarpCancel.Name = "btnPlayerWarpCancel";
            btnPlayerWarpCancel.Padding = new Padding(6);
            btnPlayerWarpCancel.Size = new Size(88, 27);
            btnPlayerWarpCancel.TabIndex = 45;
            btnPlayerWarpCancel.Text = "Cancel";
            btnPlayerWarpCancel.Click += BtnPlayerWarpCancel_Click;
            // 
            // cmbLabel31
            // 
            cmbLabel31.AutoSize = true;
            cmbLabel31.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel31.Location = new Point(9, 51);
            cmbLabel31.Margin = new Padding(4, 0, 4, 0);
            cmbLabel31.Name = "cmbLabel31";
            cmbLabel31.Size = new Size(58, 15);
            cmbLabel31.TabIndex = 44;
            cmbLabel31.Text = "Direction:";
            // 
            // cmbWarpPlayerDir
            // 
            cmbWarpPlayerDir.DrawMode = DrawMode.OwnerDrawFixed;
            cmbWarpPlayerDir.FormattingEnabled = true;
            cmbWarpPlayerDir.Items.AddRange(new object[] { "Retain Direction", "Up", "Down", "Left", "Right" });
            cmbWarpPlayerDir.Location = new Point(112, 47);
            cmbWarpPlayerDir.Margin = new Padding(4, 3, 4, 3);
            cmbWarpPlayerDir.Name = "cmbWarpPlayerDir";
            cmbWarpPlayerDir.Size = new Size(166, 24);
            cmbWarpPlayerDir.TabIndex = 43;
            // 
            // nudWPY
            // 
            nudWPY.Location = new Point(233, 17);
            nudWPY.Margin = new Padding(4, 3, 4, 3);
            nudWPY.Name = "nudWPY";
            nudWPY.Size = new Size(46, 23);
            nudWPY.TabIndex = 42;
            // 
            // cmbLabel32
            // 
            cmbLabel32.AutoSize = true;
            cmbLabel32.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel32.Location = new Point(206, 20);
            cmbLabel32.Margin = new Padding(4, 0, 4, 0);
            cmbLabel32.Name = "cmbLabel32";
            cmbLabel32.Size = new Size(17, 15);
            cmbLabel32.TabIndex = 41;
            cmbLabel32.Text = "Y:";
            // 
            // nudWPX
            // 
            nudWPX.Location = new Point(152, 17);
            nudWPX.Margin = new Padding(4, 3, 4, 3);
            nudWPX.Name = "nudWPX";
            nudWPX.Size = new Size(46, 23);
            nudWPX.TabIndex = 40;
            // 
            // cmbLabel33
            // 
            cmbLabel33.AutoSize = true;
            cmbLabel33.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel33.Location = new Point(125, 20);
            cmbLabel33.Margin = new Padding(4, 0, 4, 0);
            cmbLabel33.Name = "cmbLabel33";
            cmbLabel33.Size = new Size(17, 15);
            cmbLabel33.TabIndex = 39;
            cmbLabel33.Text = "X:";
            // 
            // nudWPMap
            // 
            nudWPMap.Location = new Point(50, 17);
            nudWPMap.Margin = new Padding(4, 3, 4, 3);
            nudWPMap.Name = "nudWPMap";
            nudWPMap.Size = new Size(68, 23);
            nudWPMap.TabIndex = 38;
            // 
            // cmbLabel34
            // 
            cmbLabel34.AutoSize = true;
            cmbLabel34.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel34.Location = new Point(7, 20);
            cmbLabel34.Margin = new Padding(4, 0, 4, 0);
            cmbLabel34.Name = "cmbLabel34";
            cmbLabel34.Size = new Size(34, 15);
            cmbLabel34.TabIndex = 37;
            cmbLabel34.Text = "Map:";
            // 
            // fraSetFog
            // 
            fraSetFog.BackColor = Color.FromArgb(45, 45, 48);
            fraSetFog.BorderColor = Color.FromArgb(90, 90, 90);
            fraSetFog.Controls.Add(btnSetFogOk);
            fraSetFog.Controls.Add(btnSetFogCancel);
            fraSetFog.Controls.Add(cmbLabel30);
            fraSetFog.Controls.Add(cmbLabel29);
            fraSetFog.Controls.Add(cmbLabel28);
            fraSetFog.Controls.Add(nudFogData2);
            fraSetFog.Controls.Add(nudFogData1);
            fraSetFog.Controls.Add(nudFogData0);
            fraSetFog.ForeColor = Color.Gainsboro;
            fraSetFog.Location = new Point(468, 8);
            fraSetFog.Margin = new Padding(4, 3, 4, 3);
            fraSetFog.Name = "fraSetFog";
            fraSetFog.Padding = new Padding(4, 3, 4, 3);
            fraSetFog.Size = new Size(287, 111);
            fraSetFog.TabIndex = 18;
            fraSetFog.TabStop = false;
            fraSetFog.Text = "Set Fog";
            fraSetFog.Visible = false;
            // 
            // btnSetFogOk
            // 
            btnSetFogOk.Location = new Point(98, 77);
            btnSetFogOk.Margin = new Padding(4, 3, 4, 3);
            btnSetFogOk.Name = "btnSetFogOk";
            btnSetFogOk.Padding = new Padding(6);
            btnSetFogOk.Size = new Size(88, 27);
            btnSetFogOk.TabIndex = 41;
            btnSetFogOk.Text = "Ok";
            btnSetFogOk.Click += BtnSetFogOK_Click;
            // 
            // btnSetFogCancel
            // 
            btnSetFogCancel.Location = new Point(192, 77);
            btnSetFogCancel.Margin = new Padding(4, 3, 4, 3);
            btnSetFogCancel.Name = "btnSetFogCancel";
            btnSetFogCancel.Padding = new Padding(6);
            btnSetFogCancel.Size = new Size(88, 27);
            btnSetFogCancel.TabIndex = 40;
            btnSetFogCancel.Text = "Cancel";
            btnSetFogCancel.Click += BtnSetFogCancel_Click;
            // 
            // cmbLabel30
            // 
            cmbLabel30.AutoSize = true;
            cmbLabel30.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel30.Location = new Point(145, 48);
            cmbLabel30.Margin = new Padding(4, 0, 4, 0);
            cmbLabel30.Name = "cmbLabel30";
            cmbLabel30.Size = new Size(74, 15);
            cmbLabel30.TabIndex = 39;
            cmbLabel30.Text = "Fog Opacity:";
            // 
            // cmbLabel29
            // 
            cmbLabel29.AutoSize = true;
            cmbLabel29.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel29.Location = new Point(8, 48);
            cmbLabel29.Margin = new Padding(4, 0, 4, 0);
            cmbLabel29.Name = "cmbLabel29";
            cmbLabel29.Size = new Size(65, 15);
            cmbLabel29.TabIndex = 38;
            cmbLabel29.Text = "Fog Speed:";
            // 
            // cmbLabel28
            // 
            cmbLabel28.AutoSize = true;
            cmbLabel28.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel28.Location = new Point(8, 17);
            cmbLabel28.Margin = new Padding(4, 0, 4, 0);
            cmbLabel28.Name = "cmbLabel28";
            cmbLabel28.Size = new Size(30, 15);
            cmbLabel28.TabIndex = 37;
            cmbLabel28.Text = "Fog:";
            // 
            // nudFogData2
            // 
            nudFogData2.Location = new Point(223, 45);
            nudFogData2.Margin = new Padding(4, 3, 4, 3);
            nudFogData2.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudFogData2.Name = "nudFogData2";
            nudFogData2.Size = new Size(57, 23);
            nudFogData2.TabIndex = 36;
            // 
            // nudFogData1
            // 
            nudFogData1.Location = new Point(84, 46);
            nudFogData1.Margin = new Padding(4, 3, 4, 3);
            nudFogData1.Name = "nudFogData1";
            nudFogData1.Size = new Size(56, 23);
            nudFogData1.TabIndex = 35;
            // 
            // nudFogData0
            // 
            nudFogData0.Location = new Point(113, 14);
            nudFogData0.Margin = new Padding(4, 3, 4, 3);
            nudFogData0.Name = "nudFogData0";
            nudFogData0.Size = new Size(167, 23);
            nudFogData0.TabIndex = 34;
            // 
            // fraShowText
            // 
            fraShowText.BackColor = Color.FromArgb(45, 45, 48);
            fraShowText.BorderColor = Color.FromArgb(90, 90, 90);
            fraShowText.Controls.Add(cmbLabel27);
            fraShowText.Controls.Add(txtShowText);
            fraShowText.Controls.Add(btnShowTextCancel);
            fraShowText.Controls.Add(btnShowTextOk);
            fraShowText.ForeColor = Color.Gainsboro;
            fraShowText.Location = new Point(7, 351);
            fraShowText.Margin = new Padding(4, 3, 4, 3);
            fraShowText.Name = "fraShowText";
            fraShowText.Padding = new Padding(4, 3, 4, 3);
            fraShowText.Size = new Size(289, 328);
            fraShowText.TabIndex = 17;
            fraShowText.TabStop = false;
            fraShowText.Text = "Show Text";
            fraShowText.Visible = false;
            // 
            // cmbLabel27
            // 
            cmbLabel27.AutoSize = true;
            cmbLabel27.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel27.Location = new Point(8, 22);
            cmbLabel27.Margin = new Padding(4, 0, 4, 0);
            cmbLabel27.Name = "cmbLabel27";
            cmbLabel27.Size = new Size(28, 15);
            cmbLabel27.TabIndex = 26;
            cmbLabel27.Text = "Text";
            // 
            // txtShowText
            // 
            txtShowText.BackColor = Color.FromArgb(69, 73, 74);
            txtShowText.BorderStyle = BorderStyle.FixedSingle;
            txtShowText.ForeColor = Color.FromArgb(220, 220, 220);
            txtShowText.Location = new Point(10, 44);
            txtShowText.Margin = new Padding(4, 3, 4, 3);
            txtShowText.Multiline = true;
            txtShowText.Name = "txtShowText";
            txtShowText.Size = new Size(266, 121);
            txtShowText.TabIndex = 21;
            // 
            // btnShowTextCancel
            // 
            btnShowTextCancel.Location = new Point(195, 291);
            btnShowTextCancel.Margin = new Padding(4, 3, 4, 3);
            btnShowTextCancel.Name = "btnShowTextCancel";
            btnShowTextCancel.Padding = new Padding(6);
            btnShowTextCancel.Size = new Size(88, 27);
            btnShowTextCancel.TabIndex = 24;
            btnShowTextCancel.Text = "Cancel";
            btnShowTextCancel.Click += BtnShowTextCancel_Click;
            // 
            // btnShowTextOk
            // 
            btnShowTextOk.Location = new Point(100, 291);
            btnShowTextOk.Margin = new Padding(4, 3, 4, 3);
            btnShowTextOk.Name = "btnShowTextOk";
            btnShowTextOk.Padding = new Padding(6);
            btnShowTextOk.Size = new Size(88, 27);
            btnShowTextOk.TabIndex = 25;
            btnShowTextOk.Text = "Ok";
            btnShowTextOk.Click += BtnShowTextOk_Click;
            // 
            // fraAddText
            // 
            fraAddText.BackColor = Color.FromArgb(45, 45, 48);
            fraAddText.BorderColor = Color.FromArgb(90, 90, 90);
            fraAddText.Controls.Add(btnAddTextOk);
            fraAddText.Controls.Add(btnAddTextCancel);
            fraAddText.Controls.Add(optAddText_Global);
            fraAddText.Controls.Add(optAddText_Map);
            fraAddText.Controls.Add(optAddText_Player);
            fraAddText.Controls.Add(cmbLabel25);
            fraAddText.Controls.Add(txtAddText_Text);
            fraAddText.Controls.Add(cmbLabel24);
            fraAddText.ForeColor = Color.Gainsboro;
            fraAddText.Location = new Point(7, 419);
            fraAddText.Margin = new Padding(4, 3, 4, 3);
            fraAddText.Name = "fraAddText";
            fraAddText.Padding = new Padding(4, 3, 4, 3);
            fraAddText.Size = new Size(272, 216);
            fraAddText.TabIndex = 3;
            fraAddText.TabStop = false;
            fraAddText.Text = "Add Text";
            fraAddText.Visible = false;
            // 
            // btnAddTextOk
            // 
            btnAddTextOk.Location = new Point(64, 180);
            btnAddTextOk.Margin = new Padding(4, 3, 4, 3);
            btnAddTextOk.Name = "btnAddTextOk";
            btnAddTextOk.Padding = new Padding(6);
            btnAddTextOk.Size = new Size(88, 27);
            btnAddTextOk.TabIndex = 9;
            btnAddTextOk.Text = "Ok";
            btnAddTextOk.Click += BtnAddTextOk_Click;
            // 
            // btnAddTextCancel
            // 
            btnAddTextCancel.Location = new Point(159, 180);
            btnAddTextCancel.Margin = new Padding(4, 3, 4, 3);
            btnAddTextCancel.Name = "btnAddTextCancel";
            btnAddTextCancel.Padding = new Padding(6);
            btnAddTextCancel.Size = new Size(88, 27);
            btnAddTextCancel.TabIndex = 8;
            btnAddTextCancel.Text = "Cancel";
            btnAddTextCancel.Click += BtnAddTextCancel_Click;
            // 
            // optAddText_Global
            // 
            optAddText_Global.AutoSize = true;
            optAddText_Global.Location = new Point(202, 153);
            optAddText_Global.Margin = new Padding(4, 3, 4, 3);
            optAddText_Global.Name = "optAddText_Global";
            optAddText_Global.Size = new Size(59, 19);
            optAddText_Global.TabIndex = 5;
            optAddText_Global.TabStop = true;
            optAddText_Global.Text = "Global";
            // 
            // optAddText_Map
            // 
            optAddText_Map.AutoSize = true;
            optAddText_Map.Location = new Point(141, 153);
            optAddText_Map.Margin = new Padding(4, 3, 4, 3);
            optAddText_Map.Name = "optAddText_Map";
            optAddText_Map.Size = new Size(49, 19);
            optAddText_Map.TabIndex = 4;
            optAddText_Map.TabStop = true;
            optAddText_Map.Text = "Map";
            // 
            // optAddText_Player
            // 
            optAddText_Player.AutoSize = true;
            optAddText_Player.Location = new Point(71, 153);
            optAddText_Player.Margin = new Padding(4, 3, 4, 3);
            optAddText_Player.Name = "optAddText_Player";
            optAddText_Player.Size = new Size(57, 19);
            optAddText_Player.TabIndex = 3;
            optAddText_Player.TabStop = true;
            optAddText_Player.Text = "Player";
            // 
            // cmbLabel25
            // 
            cmbLabel25.AutoSize = true;
            cmbLabel25.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel25.Location = new Point(7, 156);
            cmbLabel25.Margin = new Padding(4, 0, 4, 0);
            cmbLabel25.Name = "cmbLabel25";
            cmbLabel25.Size = new Size(54, 15);
            cmbLabel25.TabIndex = 2;
            cmbLabel25.Text = "Channel:";
            // 
            // txtAddText_Text
            // 
            txtAddText_Text.BackColor = Color.FromArgb(69, 73, 74);
            txtAddText_Text.BorderStyle = BorderStyle.FixedSingle;
            txtAddText_Text.ForeColor = Color.FromArgb(220, 220, 220);
            txtAddText_Text.Location = new Point(7, 36);
            txtAddText_Text.Margin = new Padding(4, 3, 4, 3);
            txtAddText_Text.Multiline = true;
            txtAddText_Text.Name = "txtAddText_Text";
            txtAddText_Text.Size = new Size(259, 110);
            txtAddText_Text.TabIndex = 1;
            // 
            // cmbLabel24
            // 
            cmbLabel24.AutoSize = true;
            cmbLabel24.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel24.Location = new Point(7, 17);
            cmbLabel24.Margin = new Padding(4, 0, 4, 0);
            cmbLabel24.Name = "cmbLabel24";
            cmbLabel24.Size = new Size(28, 15);
            cmbLabel24.TabIndex = 0;
            cmbLabel24.Text = "Text";
            // 
            // fraChangeItems
            // 
            fraChangeItems.BackColor = Color.FromArgb(45, 45, 48);
            fraChangeItems.BorderColor = Color.FromArgb(90, 90, 90);
            fraChangeItems.Controls.Add(btnChangeItemsOk);
            fraChangeItems.Controls.Add(btnChangeItemsCancel);
            fraChangeItems.Controls.Add(nudChangeItemsAmount);
            fraChangeItems.Controls.Add(optChangeItemRemove);
            fraChangeItems.Controls.Add(optChangeItemAdd);
            fraChangeItems.Controls.Add(optChangeItemSet);
            fraChangeItems.Controls.Add(cmbChangeItemIndex);
            fraChangeItems.Controls.Add(cmbLabel21);
            fraChangeItems.ForeColor = Color.Gainsboro;
            fraChangeItems.Location = new Point(7, 450);
            fraChangeItems.Margin = new Padding(4, 3, 4, 3);
            fraChangeItems.Name = "fraChangeItems";
            fraChangeItems.Padding = new Padding(4, 3, 4, 3);
            fraChangeItems.Size = new Size(218, 138);
            fraChangeItems.TabIndex = 1;
            fraChangeItems.TabStop = false;
            fraChangeItems.Text = "Change Items";
            fraChangeItems.Visible = false;
            // 
            // btnChangeItemsOk
            // 
            btnChangeItemsOk.Location = new Point(29, 105);
            btnChangeItemsOk.Margin = new Padding(4, 3, 4, 3);
            btnChangeItemsOk.Name = "btnChangeItemsOk";
            btnChangeItemsOk.Padding = new Padding(6);
            btnChangeItemsOk.Size = new Size(88, 27);
            btnChangeItemsOk.TabIndex = 7;
            btnChangeItemsOk.Text = "Ok";
            btnChangeItemsOk.Click += BtnChangeItemsOk_Click;
            // 
            // btnChangeItemsCancel
            // 
            btnChangeItemsCancel.Location = new Point(124, 105);
            btnChangeItemsCancel.Margin = new Padding(4, 3, 4, 3);
            btnChangeItemsCancel.Name = "btnChangeItemsCancel";
            btnChangeItemsCancel.Padding = new Padding(6);
            btnChangeItemsCancel.Size = new Size(88, 27);
            btnChangeItemsCancel.TabIndex = 6;
            btnChangeItemsCancel.Text = "Cancel";
            btnChangeItemsCancel.Click += BtnChangeItemsCancel_Click;
            // 
            // nudChangeItemsAmount
            // 
            nudChangeItemsAmount.Location = new Point(10, 75);
            nudChangeItemsAmount.Margin = new Padding(4, 3, 4, 3);
            nudChangeItemsAmount.Name = "nudChangeItemsAmount";
            nudChangeItemsAmount.Size = new Size(201, 23);
            nudChangeItemsAmount.TabIndex = 5;
            // 
            // optChangeItemRemove
            // 
            optChangeItemRemove.AutoSize = true;
            optChangeItemRemove.Location = new Point(141, 48);
            optChangeItemRemove.Margin = new Padding(4, 3, 4, 3);
            optChangeItemRemove.Name = "optChangeItemRemove";
            optChangeItemRemove.Size = new Size(48, 19);
            optChangeItemRemove.TabIndex = 4;
            optChangeItemRemove.TabStop = true;
            optChangeItemRemove.Text = "Take";
            // 
            // optChangeItemAdd
            // 
            optChangeItemAdd.AutoSize = true;
            optChangeItemAdd.Location = new Point(79, 48);
            optChangeItemAdd.Margin = new Padding(4, 3, 4, 3);
            optChangeItemAdd.Name = "optChangeItemAdd";
            optChangeItemAdd.Size = new Size(48, 19);
            optChangeItemAdd.TabIndex = 3;
            optChangeItemAdd.TabStop = true;
            optChangeItemAdd.Text = "Give";
            // 
            // optChangeItemSet
            // 
            optChangeItemSet.AutoSize = true;
            optChangeItemSet.Location = new Point(10, 48);
            optChangeItemSet.Margin = new Padding(4, 3, 4, 3);
            optChangeItemSet.Name = "optChangeItemSet";
            optChangeItemSet.Size = new Size(55, 19);
            optChangeItemSet.TabIndex = 2;
            optChangeItemSet.TabStop = true;
            optChangeItemSet.Text = "Set to";
            // 
            // cmbChangeItemIndex
            // 
            cmbChangeItemIndex.DrawMode = DrawMode.OwnerDrawFixed;
            cmbChangeItemIndex.FormattingEnabled = true;
            cmbChangeItemIndex.Location = new Point(49, 15);
            cmbChangeItemIndex.Margin = new Padding(4, 3, 4, 3);
            cmbChangeItemIndex.Name = "cmbChangeItemIndex";
            cmbChangeItemIndex.Size = new Size(162, 24);
            cmbChangeItemIndex.TabIndex = 1;
            // 
            // cmbLabel21
            // 
            cmbLabel21.AutoSize = true;
            cmbLabel21.ForeColor = Color.FromArgb(220, 220, 220);
            cmbLabel21.Location = new Point(7, 18);
            cmbLabel21.Margin = new Padding(4, 0, 4, 0);
            cmbLabel21.Name = "cmbLabel21";
            cmbLabel21.Size = new Size(34, 15);
            cmbLabel21.TabIndex = 0;
            cmbLabel21.Text = "Item:";
            // 
            // pnlVariableSwitches
            // 
            pnlVariableSwitches.Controls.Add(FraRenaming);
            pnlVariableSwitches.Controls.Add(fraLabeling);
            pnlVariableSwitches.Location = new Point(933, 232);
            pnlVariableSwitches.Margin = new Padding(4, 3, 4, 3);
            pnlVariableSwitches.Name = "pnlVariableSwitches";
            pnlVariableSwitches.Size = new Size(108, 105);
            pnlVariableSwitches.TabIndex = 11;
            // 
            // FraRenaming
            // 
            FraRenaming.BorderColor = Color.FromArgb(51, 51, 51);
            FraRenaming.Controls.Add(btnRename_Cancel);
            FraRenaming.Controls.Add(btnRename_Ok);
            FraRenaming.Controls.Add(fraRandom10);
            FraRenaming.ForeColor = Color.Gainsboro;
            FraRenaming.Location = new Point(275, 495);
            FraRenaming.Margin = new Padding(4, 3, 4, 3);
            FraRenaming.Name = "FraRenaming";
            FraRenaming.Padding = new Padding(4, 3, 4, 3);
            FraRenaming.Size = new Size(425, 165);
            FraRenaming.TabIndex = 8;
            FraRenaming.TabStop = false;
            FraRenaming.Text = "Renaming Variable/Switch";
            FraRenaming.Visible = false;
            // 
            // btnRename_Cancel
            // 
            btnRename_Cancel.ForeColor = Color.Black;
            btnRename_Cancel.Location = new Point(267, 118);
            btnRename_Cancel.Margin = new Padding(4, 3, 4, 3);
            btnRename_Cancel.Name = "btnRename_Cancel";
            btnRename_Cancel.Padding = new Padding(5);
            btnRename_Cancel.Size = new Size(88, 27);
            btnRename_Cancel.TabIndex = 2;
            btnRename_Cancel.Text = "Cancel";
            btnRename_Cancel.Click += BtnRename_Cancel_Click;
            // 
            // btnRename_Ok
            // 
            btnRename_Ok.ForeColor = Color.Black;
            btnRename_Ok.Location = new Point(63, 118);
            btnRename_Ok.Margin = new Padding(4, 3, 4, 3);
            btnRename_Ok.Name = "btnRename_Ok";
            btnRename_Ok.Padding = new Padding(5);
            btnRename_Ok.Size = new Size(88, 27);
            btnRename_Ok.TabIndex = 1;
            btnRename_Ok.Text = "Ok";
            btnRename_Ok.Click += BtnRename_Ok_Click;
            // 
            // fraRandom10
            // 
            fraRandom10.BorderColor = Color.FromArgb(51, 51, 51);
            fraRandom10.Controls.Add(txtRename);
            fraRandom10.Controls.Add(lblEditing);
            fraRandom10.ForeColor = Color.Gainsboro;
            fraRandom10.Location = new Point(7, 22);
            fraRandom10.Margin = new Padding(4, 3, 4, 3);
            fraRandom10.Name = "fraRandom10";
            fraRandom10.Padding = new Padding(4, 3, 4, 3);
            fraRandom10.Size = new Size(411, 89);
            fraRandom10.TabIndex = 0;
            fraRandom10.TabStop = false;
            fraRandom10.Text = "Editing Variable/Switch";
            // 
            // txtRename
            // 
            txtRename.BackColor = Color.FromArgb(69, 73, 74);
            txtRename.BorderStyle = BorderStyle.FixedSingle;
            txtRename.ForeColor = Color.FromArgb(220, 220, 220);
            txtRename.Location = new Point(7, 47);
            txtRename.Margin = new Padding(4, 3, 4, 3);
            txtRename.Name = "txtRename";
            txtRename.Size = new Size(396, 23);
            txtRename.TabIndex = 1;
            txtRename.TextChanged += TxtRename_TextChanged;
            // 
            // lblEditing
            // 
            lblEditing.AutoSize = true;
            lblEditing.ForeColor = Color.FromArgb(220, 220, 220);
            lblEditing.Location = new Point(4, 29);
            lblEditing.Margin = new Padding(4, 0, 4, 0);
            lblEditing.Name = "lblEditing";
            lblEditing.Size = new Size(110, 15);
            lblEditing.TabIndex = 0;
            lblEditing.Text = "Naming Variable #1";
            // 
            // fraLabeling
            // 
            fraLabeling.BackColor = Color.FromArgb(45, 45, 48);
            fraLabeling.BorderColor = Color.FromArgb(90, 90, 90);
            fraLabeling.Controls.Add(lstSwitches);
            fraLabeling.Controls.Add(lstVariables);
            fraLabeling.Controls.Add(btnLabel_Cancel);
            fraLabeling.Controls.Add(lblRandomLabel36);
            fraLabeling.Controls.Add(btnRenameVariable);
            fraLabeling.Controls.Add(lblRandomLabel25);
            fraLabeling.Controls.Add(btnRenameSwitch);
            fraLabeling.Controls.Add(btnLabel_Ok);
            fraLabeling.ForeColor = Color.Gainsboro;
            fraLabeling.Location = new Point(228, 33);
            fraLabeling.Margin = new Padding(4, 3, 4, 3);
            fraLabeling.Name = "fraLabeling";
            fraLabeling.Padding = new Padding(4, 3, 4, 3);
            fraLabeling.Size = new Size(532, 447);
            fraLabeling.TabIndex = 0;
            fraLabeling.TabStop = false;
            fraLabeling.Text = "Label Variables and  Switches   ";
            // 
            // lstSwitches
            // 
            lstSwitches.BackColor = Color.FromArgb(45, 45, 48);
            lstSwitches.BorderStyle = BorderStyle.FixedSingle;
            lstSwitches.ForeColor = Color.Gainsboro;
            lstSwitches.FormattingEnabled = true;
            lstSwitches.Location = new Point(275, 45);
            lstSwitches.Margin = new Padding(4, 3, 4, 3);
            lstSwitches.Name = "lstSwitches";
            lstSwitches.Size = new Size(239, 332);
            lstSwitches.TabIndex = 7;
            lstSwitches.DoubleClick += LstSwitches_DoubleClick;
            // 
            // lstVariables
            // 
            lstVariables.BackColor = Color.FromArgb(45, 45, 48);
            lstVariables.BorderStyle = BorderStyle.FixedSingle;
            lstVariables.ForeColor = Color.Gainsboro;
            lstVariables.FormattingEnabled = true;
            lstVariables.Location = new Point(16, 45);
            lstVariables.Margin = new Padding(4, 3, 4, 3);
            lstVariables.Name = "lstVariables";
            lstVariables.Size = new Size(239, 332);
            lstVariables.TabIndex = 6;
            lstVariables.DoubleClick += LstVariables_DoubleClick;
            // 
            // btnLabel_Cancel
            // 
            btnLabel_Cancel.ForeColor = Color.Black;
            btnLabel_Cancel.Location = new Point(275, 393);
            btnLabel_Cancel.Margin = new Padding(4, 3, 4, 3);
            btnLabel_Cancel.Name = "btnLabel_Cancel";
            btnLabel_Cancel.Padding = new Padding(4, 3, 4, 3);
            btnLabel_Cancel.Size = new Size(88, 27);
            btnLabel_Cancel.TabIndex = 12;
            btnLabel_Cancel.Text = "Cancel";
            btnLabel_Cancel.Click += BtnLabel_Cancel_Click;
            // 
            // lblRandomLabel36
            // 
            lblRandomLabel36.AutoSize = true;
            lblRandomLabel36.ForeColor = Color.FromArgb(220, 220, 220);
            lblRandomLabel36.Location = new Point(342, 27);
            lblRandomLabel36.Margin = new Padding(4, 0, 4, 0);
            lblRandomLabel36.Name = "lblRandomLabel36";
            lblRandomLabel36.Size = new Size(88, 15);
            lblRandomLabel36.TabIndex = 5;
            lblRandomLabel36.Text = "Player Switches";
            // 
            // btnRenameVariable
            // 
            btnRenameVariable.ForeColor = Color.Black;
            btnRenameVariable.Location = new Point(16, 393);
            btnRenameVariable.Margin = new Padding(4, 3, 4, 3);
            btnRenameVariable.Name = "btnRenameVariable";
            btnRenameVariable.Padding = new Padding(4, 3, 4, 3);
            btnRenameVariable.Size = new Size(124, 27);
            btnRenameVariable.TabIndex = 9;
            btnRenameVariable.Text = "Rename Variable";
            btnRenameVariable.Click += BtnRenameVariable_Click;
            // 
            // lblRandomLabel25
            // 
            lblRandomLabel25.AutoSize = true;
            lblRandomLabel25.ForeColor = Color.FromArgb(220, 220, 220);
            lblRandomLabel25.Location = new Point(93, 24);
            lblRandomLabel25.Margin = new Padding(4, 0, 4, 0);
            lblRandomLabel25.Name = "lblRandomLabel25";
            lblRandomLabel25.Size = new Size(88, 15);
            lblRandomLabel25.TabIndex = 4;
            lblRandomLabel25.Text = "Player Variables";
            // 
            // btnRenameSwitch
            // 
            btnRenameSwitch.ForeColor = Color.Black;
            btnRenameSwitch.Location = new Point(387, 393);
            btnRenameSwitch.Margin = new Padding(4, 3, 4, 3);
            btnRenameSwitch.Name = "btnRenameSwitch";
            btnRenameSwitch.Padding = new Padding(4, 3, 4, 3);
            btnRenameSwitch.Size = new Size(127, 27);
            btnRenameSwitch.TabIndex = 10;
            btnRenameSwitch.Text = "Rename Switch";
            btnRenameSwitch.Click += BtnRenameSwitch_Click;
            // 
            // btnLabel_Ok
            // 
            btnLabel_Ok.ForeColor = Color.Black;
            btnLabel_Ok.Location = new Point(168, 393);
            btnLabel_Ok.Margin = new Padding(4, 3, 4, 3);
            btnLabel_Ok.Name = "btnLabel_Ok";
            btnLabel_Ok.Padding = new Padding(4, 3, 4, 3);
            btnLabel_Ok.Size = new Size(88, 27);
            btnLabel_Ok.TabIndex = 11;
            btnLabel_Ok.Text = "Ok";
            btnLabel_Ok.Click += BtnLabel_Ok_Click;
            // 
            // Editor_Event
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(1284, 712);
            Controls.Add(pnlVariableSwitches);
            Controls.Add(fraDialogue);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Controls.Add(btnLabeling);
            Controls.Add(tabPages);
            Controls.Add(fraPageSetUp);
            Controls.Add(pnlTabPage);
            Controls.Add(fraMoveRoute);
            Controls.Add(pnlGraphicSel);
            ForeColor = Color.Gainsboro;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(1300, 751);
            MinimumSize = new Size(1300, 751);
            Name = "Editor_Event";
            Text = "Event Editor";
            Activated += Editor_Event_Activated;
            FormClosing += Editor_Events_FormClosing;
            Load += Editor_Events_Load;
            Resize += Editor_Event_Resize;
            fraPageSetUp.ResumeLayout(false);
            fraPageSetUp.PerformLayout();
            tabPages.ResumeLayout(false);
            pnlTabPage.ResumeLayout(false);
            DarkGroupBox2.ResumeLayout(false);
            fraGraphicPic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picGraphic).EndInit();
            DarkGroupBox6.ResumeLayout(false);
            DarkGroupBox6.PerformLayout();
            DarkGroupBox5.ResumeLayout(false);
            DarkGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picGraphicSel).EndInit();
            DarkGroupBox3.ResumeLayout(false);
            DarkGroupBox3.PerformLayout();
            DarkGroupBox1.ResumeLayout(false);
            DarkGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayerVariable).EndInit();
            DarkGroupBox8.ResumeLayout(false);
            fraCommands.ResumeLayout(false);
            fraGraphic.ResumeLayout(false);
            fraGraphic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudGraphic).EndInit();
            fraMoveRoute.ResumeLayout(false);
            fraMoveRoute.PerformLayout();
            DarkGroupBox10.ResumeLayout(false);
            fraDialogue.ResumeLayout(false);
            fraShowChatBubble.ResumeLayout(false);
            fraShowChatBubble.PerformLayout();
            fraOpenShop.ResumeLayout(false);
            fraSetSelfSwitch.ResumeLayout(false);
            fraSetSelfSwitch.PerformLayout();
            fraPlaySound.ResumeLayout(false);
            fraChangePK.ResumeLayout(false);
            fraCreateLabel.ResumeLayout(false);
            fraCreateLabel.PerformLayout();
            fraChangeJob.ResumeLayout(false);
            fraChangeJob.PerformLayout();
            fraChangeSkills.ResumeLayout(false);
            fraChangeSkills.PerformLayout();
            fraPlayerSwitch.ResumeLayout(false);
            fraPlayerSwitch.PerformLayout();
            fraSetWait.ResumeLayout(false);
            fraSetWait.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudWaitAmount).EndInit();
            fraMoveRouteWait.ResumeLayout(false);
            fraMoveRouteWait.PerformLayout();
            fraSpawnNpc.ResumeLayout(false);
            fraSetWeather.ResumeLayout(false);
            fraSetWeather.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudWeatherIntensity).EndInit();
            fraGiveExp.ResumeLayout(false);
            fraGiveExp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudGiveExp).EndInit();
            fraSetAccess.ResumeLayout(false);
            fraChangeGender.ResumeLayout(false);
            fraChangeGender.PerformLayout();
            fraShowChoices.ResumeLayout(false);
            fraShowChoices.PerformLayout();
            fraChangeLevel.ResumeLayout(false);
            fraChangeLevel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeLevel).EndInit();
            fraPlayerVariable.ResumeLayout(false);
            fraPlayerVariable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudVariableData2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData4).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVariableData0).EndInit();
            fraPlayAnimation.ResumeLayout(false);
            fraPlayAnimation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayAnimTileY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPlayAnimTileX).EndInit();
            fraChangeSprite.ResumeLayout(false);
            fraChangeSprite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeSprite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picChangeSprite).EndInit();
            fraGoToLabel.ResumeLayout(false);
            fraGoToLabel.PerformLayout();
            fraMapTint.ResumeLayout(false);
            fraMapTint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData3).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMapTintData0).EndInit();
            fraShowPic.ResumeLayout(false);
            fraShowPic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPicOffsetY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPicOffsetX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudShowPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)picShowPic).EndInit();
            fraConditionalBranch.ResumeLayout(false);
            fraConditionalBranch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCondition_LevelAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCondition_HasItem).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCondition_PlayerVarCondition).EndInit();
            fraPlayBGM.ResumeLayout(false);
            fraPlayerWarp.ResumeLayout(false);
            fraPlayerWarp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudWPY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWPX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWPMap).EndInit();
            fraSetFog.ResumeLayout(false);
            fraSetFog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudFogData2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFogData1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFogData0).EndInit();
            fraShowText.ResumeLayout(false);
            fraShowText.PerformLayout();
            fraAddText.ResumeLayout(false);
            fraAddText.PerformLayout();
            fraChangeItems.ResumeLayout(false);
            fraChangeItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudChangeItemsAmount).EndInit();
            pnlVariableSwitches.ResumeLayout(false);
            FraRenaming.ResumeLayout(false);
            fraRandom10.ResumeLayout(false);
            fraRandom10.PerformLayout();
            fraLabeling.ResumeLayout(false);
            fraLabeling.PerformLayout();
            ResumeLayout(false);
        }

        internal TreeView tvCommands;
        internal DarkGroupBox fraPageSetUp;
        internal DarkTabControl tabPages;
        internal TabPage TabPage1;
        internal DarkTextBox txtName;
        internal cmbLabel cmbLabel1;
        internal DarkButton btnNewPage;
        internal DarkButton btnCopyPage;
        internal DarkButton btnPastePage;
        internal DarkButton btnClearPage;
        internal DarkButton btnDeletePage;
        internal Panel pnlTabPage;
        internal DarkGroupBox DarkGroupBox1;
        internal DarkCheckBox chkPlayerVar;
        internal DarkComboBox cmbPlayerVar;
        internal cmbLabel cmbLabel2;
        internal DarkComboBox cmbPlayervarCompare;
        internal DarkNumericUpDown nudPlayerVariable;
        internal DarkCheckBox chkPlayerSwitch;
        internal DarkComboBox cmbPlayerSwitch;
        internal DarkComboBox cmbPlayerSwitchCompare;
        internal cmbLabel cmbLabel3;
        internal DarkComboBox cmbHasItem;
        internal DarkCheckBox chkHasItem;
        internal DarkComboBox cmbSelfSwitch;
        internal DarkCheckBox chkSelfSwitch;
        internal DarkComboBox cmbSelfSwitchCompare;
        internal cmbLabel cmbLabel4;
        internal DarkGroupBox DarkGroupBox3;
        internal DarkCheckBox chkGlobal;
        internal cmbLabel cmbLabel5;
        internal DarkComboBox cmbMoveType;
        internal DarkButton btnMoveRoute;
        internal cmbLabel cmbLabel6;
        internal DarkComboBox cmbMoveSpeed;
        internal DarkComboBox cmbMoveFreq;
        internal cmbLabel cmbLabel7;
        internal DarkGroupBox DarkGroupBox4;
        internal DarkGroupBox DarkGroupBox5;
        internal DarkComboBox cmbTrigger;
        internal cmbLabel cmbLabel8;
        internal ListBox lstCommands;
        internal DarkGroupBox DarkGroupBox8;
        internal DarkButton btnAddCommand;
        internal DarkButton btnDeleteCommand;
        internal DarkButton btnEditCommand;
        internal DarkButton btnClearCommand;
        internal Panel fraCommands;
        internal DarkButton btnLabeling;
        internal DarkButton btnCancel;
        internal DarkButton btnOk;
        internal DarkGroupBox fraMoveRoute;
        internal DarkComboBox cmbEvent;
        internal ListBox lstMoveRoute;
        internal DarkGroupBox DarkGroupBox10;
        internal DarkListView lstvwMoveRoute;
        internal ColumnHeader ColumnHeader3;
        internal ColumnHeader ColumnHeader4;
        internal DarkCheckBox chkRepeatRoute;
        internal DarkCheckBox chkIgnoreMove;
        internal DarkButton btnMoveRouteOk;
        internal DarkButton btnMoveRouteCancel;
        internal DarkGroupBox fraDialogue;
        internal DarkGroupBox fraConditionalBranch;
        internal DarkRadioButton optCondition0;
        internal DarkComboBox cmbCondition_PlayerVarIndex;
        internal DarkNumericUpDown nudCondition_PlayerVarCondition;
        internal DarkComboBox cmbCondition_PlayerVarCompare;
        internal cmbLabel cmbLabel14;
        internal DarkRadioButton optCondition1;
        internal cmbLabel cmbLabel15;
        internal DarkComboBox cmbCondtion_PlayerSwitchCondition;
        internal DarkComboBox cmbCondition_PlayerSwitch;
        internal DarkRadioButton optCondition2;
        internal DarkNumericUpDown nudCondition_HasItem;
        internal cmbLabel cmbLabel16;
        internal DarkComboBox cmbCondition_HasItem;
        internal DarkRadioButton optCondition3;
        internal DarkComboBox cmbCondition_JobIs;
        internal DarkRadioButton optCondition4;
        internal DarkComboBox cmbCondition_LearntSkill;
        internal DarkRadioButton optCondition5;
        internal DarkComboBox cmbCondition_LevelCompare;
        internal DarkNumericUpDown nudCondition_LevelAmount;
        internal DarkRadioButton optCondition6;
        internal DarkComboBox cmbCondition_SelfSwitchCondition;
        internal cmbLabel cmbLabel17;
        internal DarkComboBox cmbCondition_SelfSwitch;
        internal DarkRadioButton optCondition8;
        internal DarkComboBox cmbCondition_Gender;
        internal DarkButton btnConditionalBranchOk;
        internal DarkButton btnConditionalBranchCancel;
        internal DarkGroupBox fraChangeItems;
        internal DarkGroupBox fraPlayerSwitch;
        internal DarkComboBox cmbChangeItemIndex;
        internal cmbLabel cmbLabel21;
        internal DarkRadioButton optChangeItemSet;
        internal DarkRadioButton optChangeItemRemove;
        internal DarkRadioButton optChangeItemAdd;
        internal DarkNumericUpDown nudChangeItemsAmount;
        internal DarkButton btnChangeItemsOk;
        internal DarkButton btnChangeItemsCancel;
        internal DarkComboBox cmbSwitch;
        internal cmbLabel cmbLabel22;
        internal cmbLabel cmbLabel23;
        internal DarkComboBox cmbPlayerSwitchSet;
        internal DarkButton btnSetPlayerSwitchOk;
        internal DarkButton btnSetPlayerswitchCancel;
        internal DarkGroupBox fraAddText;
        internal DarkTextBox txtAddText_Text;
        internal cmbLabel cmbLabel24;
        internal DarkRadioButton optAddText_Player;
        internal cmbLabel cmbLabel25;
        internal DarkRadioButton optAddText_Map;
        internal DarkButton btnAddTextOk;
        internal DarkButton btnAddTextCancel;
        internal DarkRadioButton optAddText_Global;
        internal DarkButton btnShowTextOk;
        internal DarkButton btnShowTextCancel;
        internal DarkTextBox txtShowText;
        internal cmbLabel cmbLabel27;
        internal DarkGroupBox fraShowText;
        internal DarkGroupBox fraSetFog;
        internal DarkButton btnSetFogOk;
        internal DarkButton btnSetFogCancel;
        internal cmbLabel cmbLabel30;
        internal cmbLabel cmbLabel29;
        internal cmbLabel cmbLabel28;
        internal DarkNumericUpDown nudFogData2;
        internal DarkNumericUpDown nudFogData1;
        internal DarkNumericUpDown nudFogData0;
        internal DarkGroupBox fraPlayerWarp;
        internal DarkButton btnPlayerWarpOk;
        internal DarkButton btnPlayerWarpCancel;
        internal cmbLabel cmbLabel31;
        internal DarkComboBox cmbWarpPlayerDir;
        internal DarkNumericUpDown nudWPY;
        internal cmbLabel cmbLabel32;
        internal DarkNumericUpDown nudWPX;
        internal cmbLabel cmbLabel33;
        internal DarkNumericUpDown nudWPMap;
        internal cmbLabel cmbLabel34;
        internal DarkGroupBox fraPlayBGM;
        internal DarkComboBox cmbPlayBGM;
        internal DarkButton btnPlayBgmOk;
        internal DarkButton btnPlayBgmCancel;
        internal DarkGroupBox fraChangeSkills;
        internal DarkComboBox cmbChangeSkills;
        internal cmbLabel cmbLabel37;
        internal DarkRadioButton optChangeSkillsAdd;
        internal DarkButton btnChangeSkillsOk;
        internal DarkButton btnChangeSkillsCancel;
        internal DarkRadioButton optChangeSkillsRemove;
        internal DarkGroupBox fraChangeJob;
        internal DarkComboBox cmbChangeJob;
        internal cmbLabel cmbLabel38;
        internal DarkButton btnChangeJobOk;
        internal DarkButton btnChangeJobCancel;
        internal DarkGroupBox fraCreateLabel;
        internal cmbLabel lblLabelName;
        internal DarkTextBox txtLabelName;
        internal DarkButton btnCreatelabelOk;
        internal DarkButton btnCreatelabelCancel;
        internal DarkGroupBox fraChangePK;
        internal DarkButton btnChangePkOk;
        internal DarkButton btnChangePkCancel;
        internal DarkComboBox cmbSetPK;
        internal DarkGroupBox fraPlaySound;
        internal DarkButton btnPlaySoundOk;
        internal DarkButton btnPlaySoundCancel;
        internal DarkComboBox cmbPlaySound;
        internal DarkGroupBox fraShowChatBubble;
        internal cmbLabel cmbLabel39;
        internal DarkTextBox txtChatbubbleText;
        internal cmbLabel cmbLabel40;
        internal DarkComboBox cmbChatBubbleTarget;
        internal DarkComboBox cmbChatBubbleTargetType;
        internal DarkButton btnShowChatBubbleOk;
        internal DarkButton btnShowChatBubbleCancel;
        internal cmbLabel cmbLabel41;
        internal DarkGroupBox fraMapTint;
        internal DarkButton btnMapTintOk;
        internal DarkButton btnMapTintCancel;
        internal cmbLabel cmbLabel42;
        internal DarkNumericUpDown nudMapTintData3;
        internal DarkNumericUpDown nudMapTintData2;
        internal cmbLabel cmbLabel43;
        internal cmbLabel cmbLabel44;
        internal DarkNumericUpDown nudMapTintData1;
        internal DarkNumericUpDown nudMapTintData0;
        internal cmbLabel cmbLabel45;
        internal DarkGroupBox fraSetSelfSwitch;
        internal DarkComboBox cmbSetSelfSwitch;
        internal cmbLabel cmbLabel46;
        internal DarkButton btnSelfswitchOk;
        internal DarkButton btnSelfswitchCancel;
        internal cmbLabel cmbLabel47;
        internal DarkComboBox cmbSetSelfSwitchTo;
        internal DarkGroupBox fraChangeSprite;
        internal PictureBox picChangeSprite;
        internal DarkButton btnChangeSpriteOk;
        internal DarkButton btnChangeSpriteCancel;
        internal cmbLabel cmbLabel48;
        internal DarkNumericUpDown nudChangeSprite;
        internal DarkGroupBox fraPlayerVariable;
        internal DarkComboBox cmbVariable;
        internal cmbLabel cmbLabel49;
        internal DarkRadioButton optVariableAction0;
        internal DarkRadioButton optVariableAction1;
        internal DarkNumericUpDown nudVariableData1;
        internal DarkNumericUpDown nudVariableData0;
        internal DarkRadioButton optVariableAction3;
        internal DarkNumericUpDown nudVariableData3;
        internal DarkRadioButton optVariableAction2;
        internal DarkButton btnPlayerVarOk;
        internal DarkButton btnPlayerVarCancel;
        internal cmbLabel cmbLabel51;
        internal cmbLabel cmbLabel50;
        internal DarkNumericUpDown nudVariableData4;
        internal DarkNumericUpDown nudVariableData2;
        internal DarkGroupBox fraShowChoices;
        internal cmbLabel cmbLabel52;
        internal DarkTextBox txtChoicePrompt;
        internal DarkButton btnShowChoicesOk;
        internal DarkButton btnShowChoicesCancel;
        internal cmbLabel cmbLabel56;
        internal cmbLabel cmbLabel57;
        internal cmbLabel cmbLabel55;
        internal cmbLabel cmbLabel54;
        internal DarkTextBox txtChoices4;
        internal DarkTextBox txtChoices3;
        internal DarkTextBox txtChoices2;
        internal DarkTextBox txtChoices1;
        internal DarkGroupBox fraGoToLabel;
        internal DarkTextBox txtGoToLabel;
        internal cmbLabel cmbLabel60;
        internal DarkButton btnGoToLabelOk;
        internal DarkButton btnGoToLabelCancel;
        internal DarkGroupBox fraPlayAnimation;
        internal cmbLabel cmbLabel61;
        internal DarkComboBox cmbPlayAnim;
        internal cmbLabel cmbLabel62;
        internal DarkComboBox cmbAnimTargetType;
        internal DarkNumericUpDown nudPlayAnimTileY;
        internal DarkNumericUpDown nudPlayAnimTileX;
        internal DarkComboBox cmbPlayAnimEvent;
        internal DarkButton btnPlayAnimationOk;
        internal DarkButton btnPlayAnimationCancel;
        internal cmbLabel lblPlayAnimY;
        internal cmbLabel lblPlayAnimX;
        internal DarkGroupBox fraChangeGender;
        internal DarkButton btnChangeGenderOk;
        internal DarkButton btnChangeGenderCancel;
        internal DarkRadioButton optChangeSexFemale;
        internal DarkRadioButton optChangeSexMale;
        internal DarkGroupBox fraChangeLevel;
        internal DarkButton btnChangeLevelOk;
        internal DarkButton btnChangeLevelCancel;
        internal cmbLabel cmbLabel65;
        internal DarkNumericUpDown nudChangeLevel;
        internal DarkGroupBox fraOpenShop;
        internal DarkComboBox cmbOpenShop;
        internal DarkButton btnOpenShopOk;
        internal DarkButton btnOpenShopCancel;
        internal DarkGroupBox fraShowPic;
        internal cmbLabel cmbLabel67;
        internal cmbLabel cmbLabel68;
        internal DarkNumericUpDown nudPicOffsetY;
        internal DarkNumericUpDown nudPicOffsetX;
        internal cmbLabel cmbLabel69;
        internal DarkComboBox cmbPicLoc;
        internal DarkNumericUpDown nudShowPicture;
        internal PictureBox picShowPic;
        internal DarkButton btnShowPicOk;
        internal DarkButton btnShowPicCancel;
        internal cmbLabel cmbLabel71;
        internal cmbLabel cmbLabel70;
        internal DarkGroupBox fraSetWait;
        internal DarkButton btnSetWaitOk;
        internal DarkButton btnSetWaitCancel;
        internal cmbLabel cmbLabel74;
        internal cmbLabel cmbLabel72;
        internal cmbLabel cmbLabel73;
        internal DarkNumericUpDown nudWaitAmount;
        internal DarkGroupBox fraSetAccess;
        internal DarkButton btnSetAccessOk;
        internal DarkButton btnSetAccessCancel;
        internal DarkComboBox cmbSetAccess;
        internal DarkGroupBox fraSetWeather;
        internal cmbLabel cmbLabel75;
        internal DarkComboBox CmbWeather;
        internal DarkButton btnSetWeatherOk;
        internal DarkButton btnSetWeatherCancel;
        internal cmbLabel cmbLabel76;
        internal DarkNumericUpDown nudWeatherIntensity;
        internal DarkGroupBox fraGiveExp;
        internal cmbLabel cmbLabel77;
        internal DarkGroupBox fraSpawnNpc;
        internal DarkComboBox cmbSpawnNpc;
        internal DarkButton btnGiveExpOk;
        internal DarkButton btnGiveExpCancel;
        internal DarkNumericUpDown nudGiveExp;
        internal DarkButton btnSpawnNpcOk;
        internal DarkButton btnSpawnNpcancel;
        internal DarkGroupBox fraMoveRouteWait;
        internal DarkButton btnMoveWaitCancel;
        internal DarkButton btnMoveWaitOk;
        internal cmbLabel cmbLabel79;
        internal DarkComboBox cmbMoveWait;
        internal Panel pnlVariableSwitches;
        internal DarkGroupBox fraLabeling;
        internal ListBox lstSwitches;
        internal ListBox lstVariables;
        internal DarkGroupBox FraRenaming;
        internal DarkButton btnRename_Cancel;
        internal DarkButton btnRename_Ok;
        internal DarkGroupBox fraRandom10;
        internal DarkTextBox txtRename;
        internal cmbLabel lblEditing;
        internal Panel pnlGraphicSel;
        internal DarkComboBox cmbCondition_Time;
        internal DarkRadioButton optCondition9;
        internal DarkGroupBox DarkGroupBox6;
        internal DarkCheckBox chkShowName;
        internal DarkCheckBox chkWalkThrough;
        internal DarkCheckBox chkDirFix;
        internal DarkCheckBox chkWalkAnim;
        internal DarkGroupBox fraGraphicPic;
        internal PictureBox picGraphic;
        internal DarkGroupBox fraGraphic;
        internal DarkButton btnGraphicOk;
        internal DarkButton btnGraphicCancel;
        internal cmbLabel cmbLabel13;
        internal DarkNumericUpDown nudGraphic;
        internal cmbLabel cmbLabel12;
        internal DarkComboBox cmbGraphic;
        internal cmbLabel cmbLabel11;
        internal DarkGroupBox DarkGroupBox2;
        internal DarkComboBox cmbPositioning;
        internal cmbLabel lblRandomLabel36;
        internal cmbLabel lblRandomLabel25;
        internal DarkButton btnLabel_Cancel;
        internal DarkButton btnRenameVariable;
        internal DarkButton btnRenameSwitch;
        internal DarkButton btnLabel_Ok;
        internal PictureBox picGraphicSel;
    }
}
using Eto.Forms;
using Eto.Drawing;
using Microsoft.VisualBasic;
using Core;
using System;

namespace Client
{
    public class Editor_Moral : Form
    {
        // Singleton instance for legacy static access
        private static Editor_Moral? _instance;
        public static Editor_Moral Instance => _instance ??= new Editor_Moral();
        public ListBox lstIndex = new ListBox();
        public TextBox txtName = new TextBox();
        public ComboBox cmbColor = new ComboBox();
        public CheckBox chkCanCast = new CheckBox { Text = "Can Cast" };
        public CheckBox chkCanPK = new CheckBox { Text = "Can PK" };
        public CheckBox chkCanPickupItem = new CheckBox { Text = "Can Pickup Item" };
        public CheckBox chkCanDropItem = new CheckBox { Text = "Can Drop Item" };
        public CheckBox chkCanUseItem = new CheckBox { Text = "Can Use Item" };
        public CheckBox chkDropItems = new CheckBox { Text = "Drop Items" };
        public CheckBox chkLoseExp = new CheckBox { Text = "Lose Exp" };
        public CheckBox chkPlayerBlock = new CheckBox { Text = "Player Block" };
        public CheckBox chkNpcBlock = new CheckBox { Text = "Npc Block" };
        public Button btnSave = new Button { Text = "Save" };
        public Button btnDelete = new Button { Text = "Delete" };
        public Button btnCancel = new Button { Text = "Cancel" };

        public Editor_Moral()
        {
            _instance = this;
            Title = "Moral Editor";
            ClientSize = new Size(500, 420);
            Padding = 10;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Editors.MoralEditorCancel();
        }

        private void InitializeComponent()
        {
            // Populate color combo (basic placeholder colors; actual content can be expanded)
            cmbColor.Items.Clear();
            cmbColor.Items.Add("White");
            cmbColor.Items.Add("Red");
            cmbColor.Items.Add("Green");
            cmbColor.Items.Add("Blue");
            cmbColor.SelectedIndex = 0;

            // Events
            lstIndex.SelectedIndexChanged += (s, e) => LstIndex_Click();
            txtName.TextChanged += (s, e) => TxtName_TextChanged();
            cmbColor.SelectedIndexChanged += (s, e) => CmbColor_SelectedIndexChanged();
            chkCanCast.CheckedChanged += (s, e) => chkCanCast_CheckedChanged();
            chkCanPK.CheckedChanged += (s, e) => chkCanPK_CheckedChanged();
            chkCanPickupItem.CheckedChanged += (s, e) => chkCanPickupItem_CheckedChanged();
            chkCanDropItem.CheckedChanged += (s, e) => chkCanDropItem_CheckedChanged();
            chkCanUseItem.CheckedChanged += (s, e) => chkCanUseItem_CheckedChanged();
            chkDropItems.CheckedChanged += (s, e) => chkDropItems_CheckedChanged();
            chkLoseExp.CheckedChanged += (s, e) => chkLoseExp_CheckedChanged();
            chkPlayerBlock.CheckedChanged += (s, e) => chkPlayerBlock_CheckedChanged();
            chkNpcBlock.CheckedChanged += (s, e) => chkNpcBlock_CheckedChanged();
            btnSave.Click += (s, e) => BtnSave_Click();
            btnDelete.Click += (s, e) => BtnDelete_Click();
            btnCancel.Click += (s, e) => BtnCancel_Click();
            Load += (s, e) => Editor_Moral_Load();

            // Layout
            var leftPanel = new DynamicLayout { Spacing = new Size(5, 5) };
            leftPanel.AddRow(new Label { Text = "Morals" });
            leftPanel.Add(lstIndex, yscale: true);

            var right = new DynamicLayout { Spacing = new Size(5, 5) };
            right.AddRow("Name:", txtName);
            right.AddRow("Color:", cmbColor);
            right.AddRow(chkCanCast, chkCanPK);
            right.AddRow(chkCanPickupItem, chkCanDropItem);
            right.AddRow(chkCanUseItem, chkDropItems);
            right.AddRow(chkLoseExp, null);
            right.AddRow(chkPlayerBlock, chkNpcBlock);
            right.AddRow(btnSave, btnDelete, btnCancel);

            Content = new TableLayout
            {
                Padding = 0,
                Spacing = new Size(10, 10),
                Rows =
                {
                    new TableRow(new TableCell(leftPanel, true), new TableCell(right, true))
                }
            };
        }

        private void Editor_Moral_Load()
        {
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxMorals; i++)
            {
                lstIndex.Items.Add($"{i + 1}: {Data.Moral[i].Name}");
            }
            if (lstIndex.Items.Count > 0)
            {
                lstIndex.SelectedIndex = 0;
                Editors.MoralEditorInit();
            }
        }

        private void LstIndex_Click() => Editors.MoralEditorInit();

        private void BtnSave_Click()
        {
            Editors.MoralEditorOK();
            Close();
        }

        private void BtnCancel_Click()
        {
            Editors.MoralEditorCancel();
            Close();
        }

        private void BtnDelete_Click()
        {
            int tmpindex = lstIndex.SelectedIndex;
            Moral.ClearMoral(GameState.EditorIndex);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Moral[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            Editors.MoralEditorInit();
        }

        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Data.Moral[GameState.EditorIndex].Name = Strings.Trim(txtName.Text);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Moral[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
        }

        private void chkCanCast_CheckedChanged() => Data.Moral[GameState.EditorIndex].CanCast = chkCanCast.Checked == true;
        private void chkCanPK_CheckedChanged() => Data.Moral[GameState.EditorIndex].CanPk = chkCanPK.Checked == true;
        private void chkCanPickupItem_CheckedChanged() => Data.Moral[GameState.EditorIndex].CanPickupItem = chkCanPickupItem.Checked == true;
        private void chkCanDropItem_CheckedChanged() => Data.Moral[GameState.EditorIndex].CanDropItem = chkCanDropItem.Checked == true;
        private void chkCanUseItem_CheckedChanged() => Data.Moral[GameState.EditorIndex].CanUseItem = chkCanUseItem.Checked == true;
        private void chkDropItems_CheckedChanged() => Data.Moral[GameState.EditorIndex].DropItems = chkDropItems.Checked == true;
        private void chkLoseExp_CheckedChanged() => Data.Moral[GameState.EditorIndex].LoseExp = chkLoseExp.Checked == true;
        private void chkPlayerBlock_CheckedChanged() => Data.Moral[GameState.EditorIndex].PlayerBlock = chkPlayerBlock.Checked == true;
        private void chkNpcBlock_CheckedChanged() => Data.Moral[GameState.EditorIndex].NpcBlock = chkNpcBlock.Checked == true;
        private void CmbColor_SelectedIndexChanged() => Data.Moral[GameState.EditorIndex].Color = (byte)(cmbColor.SelectedIndex >= 0 ? cmbColor.SelectedIndex : 0);
    }
}
using Eto.Forms;
using Eto.Drawing;
using Core;
using System;

namespace Client
{
    public class Editor_Shop : Form
    {
        private static Editor_Shop? _instance;
        public static Editor_Shop Instance => _instance ??= new Editor_Shop();

        // Public controls referenced by Editors.cs
        public ListBox lstIndex = new ListBox();
        public TextBox txtName = new TextBox();
        public NumericStepper nudBuy = new NumericStepper { MinValue = 0, MaxValue = 10000, Increment = 1 };
        public ComboBox cmbItem = new ComboBox();
        public ComboBox cmbCostItem = new ComboBox();
        public ListBox lstTradeItem = new ListBox();
        public ComboBox cmbItemCurrency = new ComboBox(); // reserved for future use
        public NumericStepper nudItemValue = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        public NumericStepper nudCostValue = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        private Button btnUpdate = new Button { Text = "Update Trade" };
        private Button btnDeleteTrade = new Button { Text = "Delete Trade" };
        private Button btnSave = new Button { Text = "Save" };
        private Button btnDelete = new Button { Text = "Delete" };
        private Button btnCancel = new Button { Text = "Cancel" };

        public Editor_Shop()
        {
            Title = "Shop Editor";
            ClientSize = new Size(800, 500);
            Padding = 10;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Editors.ShopEditorCancel();
        }

        private void InitializeComponent()
        {
            // Event wiring
            lstIndex.SelectedIndexChanged += (s, e) => LstIndex_Click();
            txtName.TextChanged += (s, e) => TxtName_TextChanged();
            nudBuy.ValueChanged += (s, e) => NudBuy_ValueChanged();
            cmbItem.SelectedIndexChanged += (s, e) => { };
            cmbCostItem.SelectedIndexChanged += (s, e) => { };
            lstTradeItem.SelectedIndexChanged += (s, e) => { };
            nudItemValue.ValueChanged += (s, e) => { };
            nudCostValue.ValueChanged += (s, e) => { };
            btnUpdate.Click += (s, e) => BtnUpdate_Click();
            btnDeleteTrade.Click += (s, e) => BtnDeleteTrade_Click();
            btnSave.Click += (s, e) => BtnSave_Click();
            btnDelete.Click += (s, e) => BtnDelete_Click();
            btnCancel.Click += (s, e) => BtnCancel_Click();
            Load += (s, e) => Editor_Shop_Load();

            // Layouts
            var listLayout = new DynamicLayout { Spacing = new Size(5,5) };
            listLayout.AddRow(new Label { Text = "Shops" });
            listLayout.Add(lstIndex, yscale: true);

            var tradeLayout = new DynamicLayout { Spacing = new Size(4,4) };
            tradeLayout.AddRow(new Label { Text = "Trades" });
            tradeLayout.Add(lstTradeItem, yscale: true);
            tradeLayout.AddRow("Item:", cmbItem, "Qty:", nudItemValue);
            tradeLayout.AddRow("Cost Item:", cmbCostItem, "Cost Qty:", nudCostValue);
            tradeLayout.AddRow(btnUpdate, btnDeleteTrade);

            var generalLayout = new DynamicLayout { Spacing = new Size(4,4) };
            generalLayout.AddRow("Name:", txtName, "Buy Rate:", nudBuy);
            generalLayout.Add(tradeLayout);
            generalLayout.AddRow(btnSave, btnDelete, btnCancel);

            Content = new TableLayout
            {
                Spacing = new Size(10,10),
                Rows = { new TableRow(new TableCell(listLayout, true), new TableCell(generalLayout, true)) }
            };
        }

        private void Editor_Shop_Load()
        {
            lstIndex.Items.Clear();
            for (int i = 0; i < Constant.MaxShops; i++)
                lstIndex.Items.Add($"{i + 1}: {Core.Data.Shop[i].Name}");

            cmbItem.Items.Clear();
            cmbCostItem.Items.Clear();
            for (int i = 0; i < Constant.MaxItems; i++)
            {
                cmbItem.Items.Add($"{i + 1}: {Core.Data.Item[i].Name}");
                cmbCostItem.Items.Add($"{i + 1}: {Core.Data.Item[i].Name}");
            }

            if (lstIndex.Items.Count > 0)
            {
                lstIndex.SelectedIndex = 0;
                Editors.ShopEditorInit();
            }
        }

        private void LstIndex_Click() => Editors.ShopEditorInit();
        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Core.Data.Shop[GameState.EditorIndex].Name = txtName.Text;
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Core.Data.Shop[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
        }
        private void NudBuy_ValueChanged() => Core.Data.Shop[GameState.EditorIndex].BuyRate = (int)Math.Round(nudBuy.Value);

        private void BtnUpdate_Click()
        {
            int index = lstTradeItem.SelectedIndex;
            if (index < 0 || index >= Constant.MaxTrades) return;
            ref var trade = ref Core.Data.Shop[GameState.EditorIndex].TradeItem[index];
            trade.Item = cmbItem.SelectedIndex;
            trade.ItemValue = (int)Math.Round(nudItemValue.Value);
            trade.CostItem = cmbCostItem.SelectedIndex;
            trade.CostValue = (int)Math.Round(nudCostValue.Value);
            Editors.UpdateShopTrade();
        }

        private void BtnDeleteTrade_Click()
        {
            int index = lstTradeItem.SelectedIndex;
            if (index < 0 || index >= Constant.MaxTrades) return;
            ref var trade = ref Core.Data.Shop[GameState.EditorIndex].TradeItem[index];
            trade.Item = -1;
            trade.ItemValue = 0;
            trade.CostItem = -1;
            trade.CostValue = 0;
            Editors.UpdateShopTrade();
        }

        private void BtnSave_Click() { Editors.ShopEditorOK(); Close(); }
        private void BtnCancel_Click() { Editors.ShopEditorCancel(); Close(); }
        private void BtnDelete_Click()
        {
            int tmpindex = lstIndex.SelectedIndex;
            Shop.ClearShop(GameState.EditorIndex);
            lstIndex.Items.RemoveAt(GameState.EditorIndex);
            lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Core.Data.Shop[GameState.EditorIndex].Name}" });
            lstIndex.SelectedIndex = tmpindex;
            Editors.ShopEditorInit();
        }
    }
}
using Eto.Forms;
using Eto.Drawing;
using Core;
using System;
using Core.Globals;

namespace Client
{
    public class Editor_Shop : Form
    {
        // Singleton access for legacy usage
        private static Editor_Shop? _instance;
        public static Editor_Shop Instance => _instance ??= new Editor_Shop();
        private bool _suppressIndexChanged;
        public ListBox lstIndex = new ListBox();
        public TextBox txtName = new TextBox { Width = 200 };
        public NumericStepper nudBuy = new NumericStepper { MinValue = 0, MaxValue = 10000, Increment = 1 };
        public ComboBox cmbItem = new ComboBox();
        public ComboBox cmbCostItem = new ComboBox();
        public ListBox lstTradeItem = new ListBox();
        public ComboBox cmbItemCurrency = new ComboBox();
        public NumericStepper nudItemValue = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        public NumericStepper nudCostValue = new NumericStepper { MinValue = 0, MaxValue = 1000000 };
        private Button btnUpdate = new Button { Text = "Update Trade" };
        private Button btnDeleteTrade = new Button { Text = "Delete Trade" };
        private Button btnSave = new Button { Text = "Save" };
        private Button btnDelete = new Button { Text = "Delete" };
        private Button btnCopy = new Button { Text = "Copy" };
        private Button btnCancel = new Button { Text = "Cancel" };
        private Core.Globals.Type.Shop _clipboardShop;
        private bool _hasClipboardShop;

        public Editor_Shop()
        {
            _instance = this;
            Title = "Shop Editor";
            ClientSize = new Size(900, 600);
            Padding = 10;
            InitializeComponent();
            Editors.AutoSizeWindow(this, 720, 480);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Editors.ShopEditorCancel();
        }

        private void InitializeComponent()
        {
            // Subscribe Load first
            Load += (s, e) => Editor_Shop_Load();

            // Event wiring
            lstIndex.SelectedIndexChanged += (s, e) =>
            {
                if (_suppressIndexChanged) return;
                if (lstIndex.SelectedIndex >= 0)
                    GameState.EditorIndex = lstIndex.SelectedIndex;
                LstIndex_Click();
            };
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
            btnCopy.Click += (s, e) => CopyOrPasteShop();

            // Adjust list to consistent width and restructure layout similar to other editors
            lstIndex.Width = 220;

            // Left panel (list)
            var leftPanel = new StackLayout
            {
                Padding = 4,
                Spacing = 4,
                Items =
                {
                    new Label { Text = "Shops", Font = SystemFonts.Bold(12) },
                    new StackLayoutItem(lstIndex, expand: true)
                }
            };

            // Trade editor: list on left, fields on right via splitter to avoid overlap
            var tradeFields = new DynamicLayout { Spacing = new Size(4,4) };
            tradeFields.AddRow("Item:", cmbItem);
            tradeFields.AddRow("Qty:", nudItemValue);
            tradeFields.AddRow("Cost Item:", cmbCostItem);
            tradeFields.AddRow("Cost Qty:", nudCostValue);

            var tradeListPanel = new StackLayout
            {
                Spacing = 4,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Items =
                {
                    new Label { Text = "Items", Font = SystemFonts.Bold(12) },
                    new StackLayoutItem(new Scrollable { Content = lstTradeItem }, expand: true),
                    new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnUpdate, btnDeleteTrade } }
                }
            };

            var tradeArea = new Splitter
            {
                Position = 560, // wider trade list
                Panel1 = tradeListPanel,
                Panel2 = new Scrollable { Content = tradeFields },
                Panel1MinimumSize = 260,
                Panel2MinimumSize = 180
            };

            var rightPanel = new StackLayout
            {
                Padding = 4,
                Spacing = 8,
                Items =
                {
                    new GroupBox { Text = "General", Content = new TableLayout
                        {
                            Spacing = new Size(4,4),
                            Rows = { new TableRow(new Label{Text="Name:"}, txtName, new Label{Text="Buy Rate:"}, nudBuy) }
                        }
                    },
                    // Make trades area expand to fill remaining space
                    new StackLayoutItem(new GroupBox { Text = "Trade", Content = tradeArea }, expand: true),
                    new StackLayout { Orientation = Orientation.Horizontal, Spacing = 6, Items = { btnSave, btnDelete, btnCopy, btnCancel } }
                }
            };

            Content = new Splitter
            {
                Position = 240,
                Panel1 = leftPanel,
                Panel2 = rightPanel
            };
        }

        private void Editor_Shop_Load()
        {
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.Clear();
                for (int i = 0; i < Constant.MaxShops; i++)
                    lstIndex.Items.Add($"{i + 1}: {Data.Shop[i].Name}");
                lstIndex.SelectedIndex = GameState.EditorIndex >= 0 ? GameState.EditorIndex : 0;

                cmbItem.Items.Clear();
                cmbCostItem.Items.Clear();
                for (int i = 0; i < Constant.MaxItems; i++)
                {
                    cmbItem.Items.Add($"{i + 1}: {Data.Item[i].Name}");
                    cmbCostItem.Items.Add($"{i + 1}: {Data.Item[i].Name}");
                }
            }
            finally
            {
                _suppressIndexChanged = false;
            }

            // Single init after population
            Editors.ShopEditorInit();
        }

        private void LstIndex_Click() => Editors.ShopEditorInit();
        private void TxtName_TextChanged()
        {
            if (lstIndex.SelectedIndex < 0) return;
            int tmpindex = lstIndex.SelectedIndex;
            Data.Shop[GameState.EditorIndex].Name = txtName.Text;
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.RemoveAt(GameState.EditorIndex);
                lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Shop[GameState.EditorIndex].Name}" });
                lstIndex.SelectedIndex = tmpindex;
            }
            finally { _suppressIndexChanged = false; }
        }
        private void NudBuy_ValueChanged() => Data.Shop[GameState.EditorIndex].BuyRate = (int)Math.Round(nudBuy.Value);

        private void BtnUpdate_Click()
        {
            int index = lstTradeItem.SelectedIndex;
            if (index < 0 || index >= Constant.MaxTrades) return;
            ref var trade = ref Data.Shop[GameState.EditorIndex].TradeItem[index];
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
            ref var trade = ref Data.Shop[GameState.EditorIndex].TradeItem[index];
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
            _suppressIndexChanged = true;
            try
            {
                lstIndex.Items.RemoveAt(GameState.EditorIndex);
                lstIndex.Items.Insert(GameState.EditorIndex, new ListItem { Text = $"{GameState.EditorIndex + 1}: {Data.Shop[GameState.EditorIndex].Name}" });
                lstIndex.SelectedIndex = tmpindex;
            }
            finally { _suppressIndexChanged = false; }
            Editors.ShopEditorInit();
        }

        private void CopyOrPasteShop()
        {
            int src = GameState.EditorIndex;
            if (!_hasClipboardShop)
            {
                if (src < 0 || src >= Constant.MaxShops) return;
                var s = Data.Shop[src];
                _clipboardShop = s; // struct copy
                if (s.TradeItem != null)
                {
                    _clipboardShop.TradeItem = new Core.Globals.Type.TradeItem[s.TradeItem.Length];
                    Array.Copy(s.TradeItem, _clipboardShop.TradeItem, s.TradeItem.Length);
                }
                _hasClipboardShop = true;
                btnCopy.Text = "Paste";
                return;
            }

            int def = GameState.EditorIndex + 1;
            var oneBased = Editors.PromptIndex(this, "Paste Shop", $"Paste shop into index (1..{Constant.MaxShops}):", 1, Constant.MaxShops, def);
            if (oneBased == null) return;
            int dst = oneBased.Value - 1;
            var n = _clipboardShop;
            if (n.TradeItem != null)
            {
                var arr = new Core.Globals.Type.TradeItem[n.TradeItem.Length];
                Array.Copy(n.TradeItem, arr, n.TradeItem.Length);
                n.TradeItem = arr;
            }
            Data.Shop[dst] = n;
            GameState.ShopChanged[dst] = true;

            if (lstIndex != null && dst >= 0 && dst < lstIndex.Items.Count)
            {
                _suppressIndexChanged = true;
                try
                {
                    lstIndex.Items.RemoveAt(dst);
                    lstIndex.Items.Insert(dst, new ListItem { Text = $"{dst + 1}: {Data.Shop[dst].Name}" });
                    lstIndex.SelectedIndex = dst;
                }
                finally { _suppressIndexChanged = false; }
            }
            GameState.EditorIndex = dst;
            Editors.ShopEditorInit();
        }
    }
}
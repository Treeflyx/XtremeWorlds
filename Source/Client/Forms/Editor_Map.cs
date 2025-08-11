using Eto.Forms;
using Eto.Drawing;
using Assimp.Configs;
using Client.Net;
using Core;
using Core.Configurations;
using Core.Globals;
using Core.Net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core.Globals.Type;
using Color = Microsoft.Xna.Framework.Color;
using Command = Eto.Forms.Command;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Type = Core.Globals.Type;

namespace Client
{

    public partial class Editor_Map : Form
    {
        // Singleton instance for legacy static access
        private static Editor_Map? _instance;
        public static Editor_Map Instance => _instance ??= new Editor_Map();
        private static readonly int tilesetOffsetX = 0;
        private static int tilesetOffsetY = 0;
        public RadioButton optTrap = new RadioButton{ Text = "Trap" };
        public RadioButton optHeal = new RadioButton{ Text = "Heal" };
        public RadioButton optBank = new RadioButton{ Text = "Bank" };
        public RadioButton optShop = new RadioButton{ Text = "Shop" };
        public RadioButton optNpcSpawn = new RadioButton{ Text = "Npc Spawn" };
        public RadioButton optResource = new RadioButton{ Text = "Resource" };
        public RadioButton optNpcAvoid = new RadioButton{ Text = "Npc Avoid" };
        public RadioButton optItem = new RadioButton{ Text = "Item" };
        public RadioButton optWarp = new RadioButton{ Text = "Warp" };
        public RadioButton optBlocked = new RadioButton{ Text = "Blocked" };
        public Panel pnlBack = new Panel();
        public ImageView picBackSelect = new ImageView();
        public Panel pnlAttributes = new Panel();
        public GroupBox fraAnimation = new GroupBox{ Text = "Animation" };
        public ComboBox cmbAnimation = new ComboBox();
        public Button brnAnimation = new Button{ Text = "OK" };
        public GroupBox fraMapWarp = new GroupBox{ Text = "Map Warp" };
        public Button btnMapWarp = new Button{ Text = "OK" };
        public Slider scrlMapWarpY = new Slider();
        public Slider scrlMapWarpX = new Slider();
        public Slider scrlMapWarpMap = new Slider();
        public Label lblMapWarpY = new Label();
        public Label lblMapWarpX = new Label();
        public Label lblMapWarpMap = new Label();
        public GroupBox fraNpcSpawn = new GroupBox{ Text = "NPC Spawn" };
        public ComboBox lstNpc = new ComboBox();
        public Button btnNpcSpawn = new Button{ Text = "OK" };
        public Slider scrlNpcDir = new Slider();
        public Label lblNpcDir = new Label();
        public GroupBox fraHeal = new GroupBox{ Text = "Heal" };
        public Slider scrlHeal = new Slider();
        public Label lblHeal = new Label();
        public ComboBox cmbHeal = new ComboBox();
        public Button btnHeal = new Button{ Text = "OK" };
        public GroupBox fraShop = new GroupBox{ Text = "Shop" };
        public ComboBox cmbShop = new ComboBox();
        public Button btnShop = new Button{ Text = "OK" };
        public GroupBox fraResource = new GroupBox{ Text = "Resource" };
        public Button btnResourceOk = new Button{ Text = "OK" };
        public Slider scrlResource = new Slider();
        public Label lblResource = new Label();
        public GroupBox fraMapItem = new GroupBox{ Text = "Map Item" };
        public ImageView picMapItem = new ImageView();
        public Button btnMapItem = new Button{ Text = "OK" };
        public Slider scrlMapItemValue = new Slider();
        public Slider scrlMapItem = new Slider();
        public Label lblMapItem = new Label();
        public GroupBox fraTrap = new GroupBox{ Text = "Trap" };
        public Button btnTrap = new Button{ Text = "OK" };
        public Slider scrlTrap = new   Slider();
        public Label lblTrap = new Label();
        public ToolBar? toolbar;
        public TabControl tabPages = new TabControl();
        public TabPage tpTiles = new TabPage{ Text = "Tiles" };
        public ComboBox cmbAutoTile = new ComboBox();
        public Label Label11 = new Label{ Text = "Autotile" };
        public Label Label10 = new Label{ Text = "Layer" };
        public ComboBox cmbLayers = new ComboBox();
        public Label Label9 = new Label{ Text = "Tileset" };
        public ComboBox cmbTileSets = new ComboBox();
        public TabPage tpAttributes = new TabPage{ Text = "Attributes" };
        public RadioButton optNoCrossing = new RadioButton{ Text = "No Crossing" };
        public Button btnFillAttributes = new Button{ Text = "Fill" };
        public RadioButton optInfo = new RadioButton{ Text = "Info" };
        public Label Label23 = new Label();
        public ComboBox cmbAttribute = new ComboBox();
        public RadioButton optAnimation = new RadioButton{ Text = "Animation" };
        public TabPage tpNpcs = new TabPage{ Text = "NPCs" };
        public GroupBox fraNpcs = new GroupBox{ Text = "NPCs" };
        public ListBox lstMapNpc = new ListBox();
        public Label Label18 = new Label();
        public Label Label17 = new Label();
        public ComboBox cmbNpcList = new ComboBox();
        public ComboBox ComboBox23 = new ComboBox();
        public TabPage tpSettings = new TabPage{ Text = "Settings" };
        public GroupBox fraMapSettings = new GroupBox{ Text = "Map Settings" };
        public Label Label22 = new Label();
        public ComboBox lstShop = new ComboBox();
        public Label Label8 = new Label();
        public ComboBox lstMoral = new ComboBox();
        public GroupBox fraMapLinks = new GroupBox{ Text = "Links" };
        public TextBox txtDown = new TextBox();
        public TextBox txtLeft = new TextBox();
        public Label lblMap = new Label();
        public TextBox txtRight = new TextBox();
        public TextBox txtUp = new TextBox();
        public GroupBox fraBootSettings = new GroupBox{ Text = "Boot" };
        public CheckBox chkIndoors = new CheckBox{ Text = "Indoors" };
        public CheckBox chkNoMapRespawn = new CheckBox{ Text = "No Respawn" };
        public TextBox txtBootMap = new TextBox();
        public Label Label5 = new Label();
        public TextBox txtBootY = new TextBox();
        public Label Label3 = new Label();
        public TextBox txtBootX = new TextBox();
        public Label Label4 = new Label();
        public GroupBox fraMaxSizes = new GroupBox{ Text = "Max Sizes" };
        public TextBox txtMaxY = new TextBox();
        public Label Label2 = new Label();
        public TextBox txtMaxX = new TextBox();
        public Label Label7 = new Label();
        public GroupBox GroupBox2 = new GroupBox{ Text = "Music" };
        public ListBox lstMusic = new ListBox();
        public Button btnPreview = new Button{ Text = "Preview" };
        public TextBox txtName = new TextBox();
        public Label Label6 = new Label();
        public TabPage tpDirBlock = new TabPage{ Text = "Dir Block" };
        public Label Label12 = new Label();
        public TabPage tpEvents = new TabPage{ Text = "Events" };
        public Label lblPasteMode = new Label();
        public Label lblCopyMode = new Label();
        public Button btnPasteEvent = new Button{ Text = "Paste" };
        public Label Label16 = new Label();
        public Button btnCopyEvent = new Button{ Text = "Copy" };
        public Label Label15 = new Label();
        public Label Label13 = new Label();
        public TabPage tpEffects = new TabPage{ Text = "Effects" };
        public GroupBox GroupBox6 = new GroupBox{ Text = "Brightness" };
        public Slider scrlMapBrightness = new Slider();
        public Label lblMapBrightness = new Label();
        public GroupBox GroupBox5 = new GroupBox{ Text = "Parallax" };
        public ComboBox cmbParallax = new ComboBox();
        public GroupBox GroupBox4 = new GroupBox{ Text = "Panorama" };
        public ComboBox cmbPanorama = new ComboBox();
        public GroupBox GroupBox3 = new GroupBox{ Text = "Tint" };
        public CheckBox chkTint = new CheckBox{ Text = "Tint" };
        public Label lblMapAlpha = new Label();
        public Label lblMapBlue = new Label();
        public Label lblMapGreen = new Label();
        public Label lblMapRed = new Label();
        public Slider scrlMapAlpha = new Slider();
        public Slider scrlMapBlue = new Slider();
        public Slider scrlMapGreen = new Slider();
        public Slider scrlMapRed = new Slider();
        public GroupBox GroupBox1 = new GroupBox{ Text = "Fog" };
        public Slider scrlFogOpacity = new Slider();
        public Label lblFogOpacity = new Label();
        public Slider scrlFogSpeed = new Slider();
        public Label lblFogSpeed = new Label();
        public Slider scrlIntensity = new Slider();
        public Label lblIntensity = new Label();
        public Slider scrlFog = new Slider();
        public Label lblFogIndex = new Label();
        public Label Label14 = new Label();
        public ComboBox cmbWeather = new ComboBox();

        public Editor_Map()
        {
            _instance = this;
            Title = "Map Editor";
            ClientSize = new Size(1200, 800);
            InitializeToolbar();
            lblMapBrightness.Text = "Brightness:";
        }

        private void InitializeToolbar()
        {
            toolbar = new ToolBar
            {
                Items =
                {
                    new ButtonToolItem{ Text = "Save", Command = new Command((_,__) => TsbSave_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Discard", Command = new Command((_,__) => TsbDiscard_Click(this, EventArgs.Empty))},
                    new SeparatorToolItem(),
                    new ButtonToolItem{ Text = "Grid", Command = new Command((_,__) => TsbMapGrid_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Opacity"},
                    new SeparatorToolItem(),
                    new ButtonToolItem{ Text = "Fill", Command = new Command((_,__) => TsbFill_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Clear", Command = new Command((_,__) => TsbClear_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Dropper", Command = new Command((_,__) => TsbEyeDropper_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Copy", Command = new Command((_,__) => tsbCopyMap_Click(this, EventArgs.Empty))},
                    new ButtonToolItem{ Text = "Delete"},
                    new ButtonToolItem{ Text = "Undo"},
                    new ButtonToolItem{ Text = "Redo"},
                    new ButtonToolItem{ Text = "Shot"},
                    new ButtonToolItem{ Text = "Tileset"}
                }
            };
            ToolBar = toolbar;
        }

        public static void DrawSelectionRectangle(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, float scale)
        {
            // Scale the selection rectangle based on the scale factor
            int scaledX = (int)Math.Round(GameState.EditorTileSelStart.X * GameState.SizeX * scale);
            int scaledY = (int)Math.Round(GameState.EditorTileSelStart.Y * GameState.SizeY * scale);
            int scaledWidth = (int)Math.Round(GameState.EditorTileWidth * GameState.SizeX * scale);
            int scaledHeight = (int)Math.Round(GameState.EditorTileHeight * GameState.SizeY * scale);

            // Define the scaled selection rectangle
            var selectionRect = new Rectangle(scaledX, scaledY, scaledWidth, scaledHeight);

            // Line thickness in pixels (adjust based on scaling if needed)
            int lineThickness = (int)Math.Round(1f * scale);

            // Top border
            spriteBatch.Draw(GameClient.PixelTexture, new Rectangle(selectionRect.X, selectionRect.Y, selectionRect.Width, lineThickness), Color.Red);

            // Bottom border
            spriteBatch.Draw(GameClient.PixelTexture, new Rectangle(selectionRect.X, selectionRect.Y + selectionRect.Height - lineThickness, selectionRect.Width, lineThickness), Color.Red);

            // Left border
            spriteBatch.Draw(GameClient.PixelTexture, new Rectangle(selectionRect.X, selectionRect.Y, lineThickness, selectionRect.Height), Color.Red);

            // Right border
            spriteBatch.Draw(GameClient.PixelTexture, new Rectangle(selectionRect.X + selectionRect.Width - lineThickness, selectionRect.Y, lineThickness, selectionRect.Height), Color.Red);
        }

        #region Toolbar

        private void TsbSave_Click(object sender, EventArgs e)
        {
            UpdateMap();
            Dispose();
        }

        private static void UpdateMap()
        {
            int x2;
            int y2;
            Tile[,] tempArr;

            if (Instance == null) return; // safety
            if (!Information.IsNumeric(Instance.txtMaxX.Text))
                Instance.txtMaxX.Text = Data.MyMap.MaxX.ToString();

            if (Conversion.Val(Instance.txtMaxX.Text) < SettingsManager.Instance.CameraWidth)
                Instance.txtMaxX.Text = SettingsManager.Instance.CameraWidth.ToString();

            if (Conversion.Val(Instance.txtMaxX.Text) > System.Byte.MaxValue)
                Instance.txtMaxX.Text = System.Byte.MaxValue.ToString();

            if (!Information.IsNumeric(Instance.txtMaxY.Text))
                Instance.txtMaxY.Text = Data.MyMap.MaxY.ToString();

            if (Conversion.Val(Instance.txtMaxY.Text) < SettingsManager.Instance.CameraHeight)
                Instance.txtMaxY.Text = SettingsManager.Instance.CameraHeight.ToString();

            if (Conversion.Val(Instance.txtMaxY.Text) > System.Byte.MaxValue)
                Instance.txtMaxY.Text = System.Byte.MaxValue.ToString();

            {
                ref var withBlock = ref Data.MyMap;
                withBlock.Name = Instance.txtName.Text;
                if (Instance.lstMusic.SelectedIndex >= 0)
                {
                    withBlock.Music = Instance.lstMusic.Items[Instance.lstMusic.SelectedIndex].ToString();
                }
                else
                {
                    withBlock.Music = "";
                }

                if (Instance.lstShop.SelectedIndex >= 0)
                {
                    withBlock.Shop = Instance.lstShop.SelectedIndex;
                }
                else
                {
                    withBlock.Shop = 0;
                }

                withBlock.Up = (int)Math.Round(Conversion.Val(Instance.txtUp.Text));
                withBlock.Down = (int)Math.Round(Conversion.Val(Instance.txtDown.Text));
                withBlock.Left = (int)Math.Round(Conversion.Val(Instance.txtLeft.Text));
                withBlock.Right = (int)Math.Round(Conversion.Val(Instance.txtRight.Text));
                withBlock.Moral = (byte)Instance.lstMoral.SelectedIndex;
                withBlock.BootMap = (int)Math.Round(Conversion.Val(Instance.txtBootMap.Text));
                withBlock.BootX = (byte)Math.Round(Conversion.Val(Instance.txtBootX.Text));
                withBlock.BootY = (byte)Math.Round(Conversion.Val(Instance.txtBootY.Text));

                // set the data before changing it  
                tempArr = (Tile[,])withBlock.Tile.Clone();

                x2 = withBlock.MaxX;
                y2 = withBlock.MaxY;

                // change the data  
                withBlock.MaxX = (byte)Math.Round(Conversion.Val(Instance.txtMaxX.Text));
                withBlock.MaxY = (byte)Math.Round(Conversion.Val(Instance.txtMaxY.Text));

                withBlock.Tile = new Type.Tile[(withBlock.MaxX), (withBlock.MaxY)];

                for (int i = 0; i < GameState.MaxTileHistory; i++)
                    Data.TileHistory[i].Tile = new Tile[(withBlock.MaxX), (withBlock.MaxY)];

                Data.Autotile = new Type.Autotile[(withBlock.MaxX), (withBlock.MaxY)];

                if (x2 > withBlock.MaxX)
                    x2 = withBlock.MaxX;

                if (y2 > withBlock.MaxY)
                    y2 = withBlock.MaxY;

                int layerCount = System.Enum.GetValues(typeof(MapLayer)).Length;

                var loopTo = (int)withBlock.MaxX;
                for (int x = 0; x < loopTo; x++)
                {
                    var loopTo1 = (int)withBlock.MaxY;
                    for (int y = 0; y < loopTo1; y++)
                    {
                        withBlock.Tile[x, y].Layer = new Type.Layer[layerCount];
                        Data.Autotile[x, y].Layer = new Type.QuarterTile[layerCount];

                        for (int i = 0; i < GameState.MaxTileHistory; i++)
                            Data.TileHistory[i].Tile[x, y].Layer = new Type.Layer[layerCount];

                        if (x < x2)
                        {
                            if (y < y2)
                            {
                                withBlock.Tile[x, y] = tempArr[x, y];
                            }
                        }
                    }
                }
            }

            MapEditorSend();
            GameState.GettingMap = true;
        }

        private void TsbFill_Click(object sender, EventArgs e)
        {
            MapLayer layer = (MapLayer)cmbLayers.SelectedIndex;
            MapEditorFillLayer(layer, (byte)cmbAutoTile.SelectedIndex, (byte)GameState.EditorTileX, (byte)GameState.EditorTileY);
        }

        private void TsbClear_Click(object sender, EventArgs e)
        {
            MapLayer layer = (MapLayer)Enum.ToObject(typeof(MapLayer), cmbLayers.SelectedIndex);
            MapEditorClearLayer(layer);
        }

        private void TsbEyeDropper_Click(object sender, EventArgs e)
        {
            UpdateEyeDropper();
        }

        private static void UpdateEyeDropper()
        {
            GameState.EyeDropper = !GameState.EyeDropper;
        }

        private void TsbDiscard_Click(object sender, EventArgs e)
        {
            MapEditorCancel();
            Dispose();
        }

        private void TsbMapGrid_Click(object sender, EventArgs e)
        {
            GameState.MapGrid = !GameState.MapGrid;
        }

        #endregion

    #region Tiles
    private void PicBackSelect_MouseDown(object? sender, MouseEventArgs e)
        {
            MapEditorChooseTile((int)e.Buttons, e.Location.X, e.Location.Y);
        }

    private void PicBackSelect_MouseMove(object? sender, MouseEventArgs e)
        {
            MapEditorDrag((int)e.Buttons, e.Location.X, e.Location.Y);
        }

        private void CmbTileSets_Click(object sender, EventArgs e)
        {
            if (GameState.CurTileset > GameState.NumTileSets)
            {
                cmbTileSets.SelectedIndex = 0;
            }

            Data.MyMap.Tileset = GameState.CurTileset;

            GameState.EditorTileSelStart = new Point(0, 0);
            GameState.EditorTileSelEnd = new Point(1, 1);
        }

        private void CmbAutoTile_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameState.CurAutotileType = cmbAutoTile.SelectedIndex;
            if (cmbAutoTile.SelectedIndex == 0)
            {
                GameState.EditorTileWidth = 1;
                GameState.EditorTileHeight = 1;
            }

            MapEditorChooseTile((int)MouseButtons.Primary, GameState.EditorTileX, GameState.EditorTileY);
        }

        #endregion

        #region Attributes

        private void ScrlMapWarpMap_Scroll(object sender, EventArgs e)
        {
            lblMapWarpMap.Text = "Map: " + scrlMapWarpMap.Value;
        }

        private void ScrlMapWarpX_Scroll(object sender, EventArgs e)
        {
            lblMapWarpX.Text = "X: " + scrlMapWarpX.Value;
        }

        private void ScrlMapWarpY_Scroll(object sender, EventArgs e)
        {
            lblMapWarpY.Text = "Y: " + scrlMapWarpY.Value;
        }

        private void BtnMapWarp_Click(object sender, EventArgs e)
        {
            GameState.EditorWarpMap = scrlMapWarpMap.Value;

            GameState.EditorWarpX = scrlMapWarpX.Value;
            GameState.EditorWarpY = scrlMapWarpY.Value;
            pnlAttributes.Visible = false;
            fraMapWarp.Visible = false;
        }

        private void OptWarp_CheckedChanged(object sender, EventArgs e)
        {
            if (optWarp.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraMapWarp.Visible = true;

            scrlMapWarpMap.MaxValue  = Constant.MaxMaps;
            scrlMapWarpMap.Value = 1;
            scrlMapWarpX.MaxValue = byte.MaxValue;
            scrlMapWarpY.MaxValue = byte.MaxValue;
            scrlMapWarpX.Value = 0;
            scrlMapWarpY.Value = 0;
        }

        private void ScrlMapItem_ValueChanged(object sender, EventArgs e)
        {
            if (Data.Item[scrlMapItem.Value].Type == (byte)ItemCategory.Currency | Data.Item[scrlMapItem.Value].Stackable == 1)
            {
                scrlMapItemValue.Enabled = true;
            }
            else
            {
                scrlMapItemValue.Value = 1;
                scrlMapItemValue.Enabled = false;
            }

            // DrawItem removed in refactor (image updated elsewhere)
            lblMapItem.Text = (scrlMapItem.Value + 1) + ". " + Data.Item[scrlMapItem.Value].Name + " x" + scrlMapItemValue.Value;
        }

        private void ScrlMapItemValue_ValueChanged(object sender, EventArgs e)
        {
            lblMapItem.Text = (scrlMapItem.Value + 1) + ". " + Data.Item[scrlMapItem.Value].Name + " x" + scrlMapItemValue.Value;
        }

        private void BtnMapItem_Click(object sender, EventArgs e)
        {
            GameState.ItemEditorNum = scrlMapItem.Value;
            GameState.ItemEditorValue = scrlMapItemValue.Value;
            pnlAttributes.Visible = false;
            fraMapItem.Visible = false;
        }

        private void OptItem_CheckedChanged(object sender, EventArgs e)
        {
            if (optItem.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraMapItem.Visible = true;

            lblMapItem.Text = Data.Item[scrlMapItem.Value].Name + " x" + scrlMapItemValue.Value;
            ScrlMapItem_ValueChanged(sender, e);
            // DrawItem removed in refactor (image updated elsewhere)
    }

        private void BtnResourceOk_Click(object sender, EventArgs e)
        {
            GameState.ResourceEditorNum = scrlResource.Value;
            pnlAttributes.Visible = false;
            fraResource.Visible = false;
        }

        private void ScrlResource_ValueChanged(object sender, EventArgs e)
        {
            lblResource.Text = "Resource: " + Data.Resource[scrlResource.Value].Name;
        }

        private void OptResource_CheckedChanged(object sender, EventArgs e)
        {
            if (optResource.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraResource.Visible = true;
            ScrlResource_ValueChanged(sender, e);
        }

        private void BtnNpcSpawn_Click(object sender, EventArgs e)
        {
            GameState.SpawnNpcNum = lstNpc.SelectedIndex;
            GameState.SpawnNpcDir = scrlNpcDir.Value;
            pnlAttributes.Visible = false;
            fraNpcSpawn.Visible = false;
        }

        private void OptNpcSpawn_CheckedChanged(object sender, EventArgs e)
        {
            int n;

            if (optNpcSpawn.Checked == false)
                return;

            lstNpc.Items.Clear();

            for (n = 0; n < Constant.MaxMapNpcs; n++)
            {
                if (Data.MyMap.Npc[n] > 0)
                {
                    lstNpc.Items.Add(n + ": " + Data.Npc[Data.MyMap.Npc[n]].Name);
                }
                else
                {
                    lstNpc.Items.Add(n.ToString());
                }
            }

            scrlNpcDir.Value = 0;
            lstNpc.SelectedIndex = 0;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraNpcSpawn.Visible = true;
        }

        private void BtnShop_Click(object sender, EventArgs e)
        {
            GameState.EditorShop = cmbShop.SelectedIndex;
            pnlAttributes.Visible = false;
            fraShop.Visible = false;
        }

        private void OptShop_CheckedChanged(object sender, EventArgs e)
        {
            if (optShop.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraShop.Visible = true;
        }

        private void BtnHeal_Click(object sender, EventArgs e)
        {
            GameState.MapEditorHealType = cmbHeal.SelectedIndex;
            GameState.MapEditorHealAmount = scrlHeal.Value;
            pnlAttributes.Visible = false;
            fraHeal.Visible = false;
        }

        private void ScrlHeal_Scroll(object sender, EventArgs e)
        {
            lblHeal.Text = "Amount: " + scrlHeal.Value;
        }

        private void OptHeal_CheckedChanged(object sender, EventArgs e)
        {
            if (optHeal.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraHeal.Visible = true;
            cmbHeal.SelectedIndex = 0;
        }

        private void ScrlTrap_ValueChanged(object sender, EventArgs e)
        {
            lblTrap.Text = "Amount: " + scrlTrap.Value;
        }

        private void BtnTrap_Click(object sender, EventArgs e)
        {
            GameState.MapEditorHealAmount = scrlTrap.Value;
            pnlAttributes.Visible = false;
            fraTrap.Visible = false;
        }

        private void OptTrap_CheckedChanged(object sender, EventArgs e)
        {
            if (optTrap.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraTrap.Visible = true;
        }

        private void BtnClearAttribute_Click(object sender, EventArgs e)
        {
            GameLogic.Dialogue("Map Editor", "Clear Attributes: ", "Are you sure you wish to clear attributes?", (byte)DialogueType.ClearAttributes, (byte)DialogueStyle.YesNo);
        }

        private void ScrlNpcDir_Scroll(object sender, EventArgs e)
        {
            switch (scrlNpcDir.Value)
            {
                case 0:
                    {
                        lblNpcDir.Text = "Direction: Up";
                        break;
                    }
                case 1:
                    {
                        lblNpcDir.Text = "Direction: Down";
                        break;
                    }
                case 2:
                    {
                        lblNpcDir.Text = "Direction: Left";
                        break;
                    }
                case 3:
                    {
                        lblNpcDir.Text = "Direction: Right";
                        break;
                    }
            }
        }

        private void OptBlocked_CheckedChanged(object sender, EventArgs e)
        {
            if (optBlocked.Checked)
                pnlAttributes.Visible = false;
        }
        #endregion

        #region Npc's

        private void CmbNpcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMapNpc.SelectedIndex > 0)
            {
                lstMapNpc.Items[lstMapNpc.SelectedIndex] = new ListItem { Text = lstMapNpc.SelectedIndex + ": " + Data.Npc[cmbNpcList.SelectedIndex].Name };
                Data.MyMap.Npc[lstMapNpc.SelectedIndex] = cmbNpcList.SelectedIndex;
            }
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            if (lstMusic.SelectedIndex > 0)
            {
                var itemObj = lstMusic.Items[lstMusic.SelectedIndex];
                var selectedFile = itemObj?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(selectedFile)) return;

                // If the selected music file is a MIDI file
                if (SettingsManager.Instance.MusicExt == ".mid")
                {
                    Sound.PlayMidi(System.IO.Path.Combine(DataPath.Music, selectedFile));
                }
                else
                {
                    Sound.PlayMusic(selectedFile);
                }
            }
        }

        #endregion

        #region Events

        private void BtnCopyEvent_Click(object sender, EventArgs e)
        {
            if (Event.EventCopy == false)
            {
                Event.EventCopy = true;
                lblCopyMode.Text = "CopyMode On";
                Event.EventPaste = false;
                lblPasteMode.Text = "PasteMode Off";
            }
            else
            {
                Event.EventCopy = false;
                lblCopyMode.Text = "CopyMode Off";
            }
        }

        private void BtnPasteEvent_Click(object sender, EventArgs e)
        {
            if (Event.EventPaste == false)
            {
                Event.EventPaste = true;
                lblPasteMode.Text = "PasteMode On";
                Event.EventCopy = false;
                lblCopyMode.Text = "CopyMode Off";
            }
            else
            {
                Event.EventPaste = false;
                lblPasteMode.Text = "PasteMode Off";
            }
        }

        #endregion

        #region Map Effects

        private void CmbWeather_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.MyMap.Weather = (byte)cmbWeather.SelectedIndex;
        }

        private void ScrlFog_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.Fog = scrlFog.Value;
            lblFogIndex.Text = "Fog: " + scrlFog.Value;
        }

        private void ScrlIntensity_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.WeatherIntensity = scrlIntensity.Value;
            lblIntensity.Text = "Intensity: " + scrlIntensity.Value;
        }

        private void ScrlFogSpeed_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.FogSpeed = (byte)scrlFogSpeed.Value;
            lblFogSpeed.Text = "Fog Speed: " + scrlFogSpeed.Value;
        }

        private void ScrlFogOpacity_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.FogOpacity = (byte)scrlFogOpacity.Value;
            lblFogOpacity.Text = "Fog Alpha: " + scrlFogOpacity.Value;
        }

        private void ChkUseTint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTint.Checked == true)
            {
                Data.MyMap.MapTint = true;
            }
            else
            {
                Data.MyMap.MapTint = false;
            }
        }

        private void ScrlMapRed_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.MapTintR = (byte)scrlMapRed.Value;
            lblMapRed.Text = "Red: " + scrlMapRed.Value;
        }

        private void ScrlMapGreen_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.MapTintG = (byte)scrlMapGreen.Value;
            lblMapGreen.Text = "Green: " + scrlMapGreen.Value;
        }

        private void ScrlMapBlue_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.MapTintB = (byte)scrlMapBlue.Value;
            lblMapBlue.Text = "Blue: " + scrlMapBlue.Value;
        }

        private void ScrlMapAlpha_Scroll(object sender, EventArgs e)
        {
            Data.MyMap.MapTintA = (byte)scrlMapAlpha.Value;
            lblMapAlpha.Text = "Alpha: " + scrlMapAlpha.Value;
        }

        private void CmbPanorama_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.MyMap.Panorama = (byte)cmbPanorama.SelectedIndex;
        }

        private void CmbParallax_SelectedIndexChanged(object sender, EventArgs e)
        {
            Data.MyMap.Parallax = (byte)cmbParallax.SelectedIndex;
        }

        public static void MapPropertiesInit()
        {
            int x;
            int y;
            int i;
            if (Instance == null) throw new InvalidOperationException("Editor_Map.Instance not initialized");
            Instance.txtName.Text = Strings.Trim(Data.MyMap.Name);

            // find the music we have set
            Instance.lstMusic.Items.Clear();
            Instance.lstMusic.Items.Add("None");
            Instance.lstMusic.SelectedIndex = 0;

            General.CacheMusic();

            var loopTo = Information.UBound(Sound.MusicCache);
            for (i = 0; i < loopTo; i++)
                Instance.lstMusic.Items.Add(Sound.MusicCache[i]);

            var loopTo1 = Instance.lstMusic.Items.Count;
            for (i = 0; i < loopTo1; i++)
            {
                if ((Instance.lstMusic.Items[i].ToString() ?? "") == (Data.MyMap.Music ?? ""))
                {
                    Instance.lstMusic.SelectedIndex = i;
                    break;
                }
            }

            // find the shop we have set
            Instance.lstShop.Items.Clear();

            for (i = 0; i < Constant.MaxShops; i++)
                Instance.lstShop.Items.Add(Data.Shop[i].Name);

            Instance.lstShop.SelectedIndex = 0;

            var loopTo2 = Instance.lstShop.Items.Count;
            for (i = 0; i < loopTo2; i++)
            {
                if ((Instance.lstShop.Items[i].ToString() ?? "") == (Data.Shop[Data.MyMap.Shop].Name ?? ""))
                {
                    Instance.lstShop.SelectedIndex = i;
                    break;
                }
            }

            // find the shop we have set
            Instance.lstMoral.Items.Clear();

            for (i = 0; i < Constant.MaxMorals; i++)
                Instance.lstMoral.Items.Add(Data.Moral[i].Name);

            Instance.lstMoral.SelectedIndex = 0;

            var loopTo3 = Instance.lstMoral.Items.Count;
            for (i = 0; i < loopTo3; i++)
            {
                if ((Instance.lstMoral.Items[i].ToString() ?? "") == (Data.Moral[Data.MyMap.Moral].Name ?? ""))
                {
                    Instance.lstMoral.SelectedIndex = i;
                    break;
                }
            }

            Instance.chkTint.Checked = Data.MyMap.MapTint;
            Instance.chkNoMapRespawn.Checked = Data.MyMap.NoRespawn;
            Instance.chkIndoors.Checked = Data.MyMap.Indoors;

            // rest of it
            Instance.txtUp.Text = Data.MyMap.Up.ToString();
            Instance.txtDown.Text = Data.MyMap.Down.ToString();
            Instance.txtLeft.Text = Data.MyMap.Left.ToString();
            Instance.txtRight.Text = Data.MyMap.Right.ToString();

            Instance.txtBootMap.Text = Data.MyMap.BootMap.ToString();
            Instance.txtBootX.Text = Data.MyMap.BootX.ToString();
            Instance.txtBootY.Text = Data.MyMap.BootY.ToString();

            Instance.lstMapNpc.Items.Clear();

            for (x = 0; x < Constant.MaxMapNpcs; x++)
            {
                if (x == 0)
                {
                    Instance.lstMapNpc.Items.Add("None");
                    continue;
                }

                if (Data.MyMap.Npc[x] >= 0 && Data.MyMap.Npc[x] <= Constant.MaxNpcs)
                {
                    Instance.lstMapNpc.Items.Add(x + ": " + Strings.Trim(Data.Npc[Data.MyMap.Npc[x]].Name));
                }
                else
                {
                    Instance.lstMapNpc.Items.Add(x + ": None");
                }
            }

            Instance.lstMapNpc.SelectedIndex = 0;

            for (y = 0; y < Constant.MaxNpcs; y++)
                Instance.cmbNpcList.Items.Add(y + 1 + ": " + Strings.Trim(Data.Npc[y].Name));

            Instance.cmbNpcList.SelectedIndex = 0;

            Instance.cmbAnimation.Items.Clear();

            for (y = 0; y < Constant.MaxAnimations; y++)
                Instance.cmbAnimation.Items.Add(y + 1 + ": " + Data.Animation[y].Name);

            Instance.cmbAnimation.SelectedIndex = 0;

            Instance.lblMap.Text = "Map: ";
            Instance.txtMaxX.Text = Data.MyMap.MaxX.ToString();
            Instance.txtMaxY.Text = Data.MyMap.MaxY.ToString();

            Instance.cmbWeather.SelectedIndex = Data.MyMap.Weather;
            Instance.scrlFog.Value = Data.MyMap.Fog;
            Instance.lblFogIndex.Text = "Fog: " + Instance.scrlFog.Value;
            Instance.scrlIntensity.Value = Data.MyMap.WeatherIntensity;
            Instance.lblIntensity.Text = "Intensity: " + Instance.scrlIntensity.Value;
            Instance.scrlFogOpacity.Value = Data.MyMap.FogOpacity;
            Instance.scrlFogSpeed.Value = Data.MyMap.FogSpeed;

            Instance.cmbPanorama.Items.Clear();

            var loopTo4 = GameState.NumPanoramas;
            for (i = 0; i < loopTo4; i++)
                Instance.cmbPanorama.Items.Add((i + 1).ToString());
            
            Instance.cmbPanorama.SelectedIndex = Data.MyMap.Panorama;
            
            Instance.cmbParallax.Items.Clear();
            
            var loopTo5 = GameState.NumParallax;
            for (i = 0; i < loopTo5; i++)
                Instance.cmbParallax.Items.Add((i + 1).ToString());
            
            Instance.cmbParallax.SelectedIndex = Data.MyMap.Parallax;

            Instance.tabPages.SelectedIndex = 0;
            Instance.scrlMapBrightness.Value = Data.MyMap.Brightness;
            Instance.lblMapBrightness.Text = "Brightness: " + Instance.scrlMapBrightness.Value;
            Instance.chkTint.Checked = Data.MyMap.MapTint;
            Instance.scrlMapRed.Value = Data.MyMap.MapTintR;
            Instance.scrlMapGreen.Value = Data.MyMap.MapTintG;
            Instance.scrlMapBlue.Value = Data.MyMap.MapTintB;
            Instance.scrlMapAlpha.Value = Data.MyMap.MapTintA;

            // show the form
            Instance.Visible = true;
        }

        public static void MapEditorInit()
        {
            // set the scrolly bars
            if (Data.MyMap.Tileset < 1 || Data.MyMap.Tileset > GameState.NumTileSets)
                Data.MyMap.Tileset = 1;

            GameState.EditorTileSelStart = new Point(0, 0);
            GameState.EditorTileSelEnd = new Point(1, 1);

            GameState.CurTileset = Data.MyMap.Tileset;

            // set shops for the shop attribute
            for (int i = 0; i < Constant.MaxShops; i++)
                Instance.cmbShop.Items.Add((i + 1) + ": " + Data.Shop[i].Name);

            // we're not in a shop
            if (Instance.cmbShop.Items.Count > 0)
                Instance.cmbShop.SelectedIndex = 0;

            Instance.optBlocked.Checked = true;

            Instance.cmbTileSets.Items.Clear();
            for (int i = 0, loopTo = GameState.NumTileSets; i < loopTo; i++)
                Instance.cmbTileSets.Items.Add((i + 1).ToString());
            
            Instance.cmbTileSets.SelectedIndex = 0;
            Instance.cmbAutoTile.SelectedIndex = 0;
            Instance.tabPages.SelectedIndex = 0;
            Instance.scrlMapBrightness.Value = Data.MyMap.Brightness;
            Instance.lblMapBrightness.Text = "Brightness: " + Instance.scrlMapBrightness.Value;
            Instance.chkTint.Checked = Data.MyMap.MapTint;
            Instance.scrlMapRed.Value = Data.MyMap.MapTintR;
            Instance.scrlMapGreen.Value = Data.MyMap.MapTintG;
            Instance.scrlMapBlue.Value = Data.MyMap.MapTintB;
            Instance.scrlMapAlpha.Value = Data.MyMap.MapTintA;
            Instance.Visible = true;
            MapPropertiesInit();

            if (GameState.MapData == true)
                GameState.GettingMap = false;
        }

        public static void MapEditorChooseTile(int Button, float X, float Y)
        {
            if (Button == (int)MouseButtons.Primary) // Primary (Left) Mouse Button
            {
                GameState.EditorTileWidth = 1;
                GameState.EditorTileHeight = 1;

                if (GameState.CurAutotileType > 0)
                {
                    switch (GameState.CurAutotileType)
                    {
                        case 1: // autotile
                            GameState.EditorTileWidth = 2;
                            GameState.EditorTileHeight = 3;
                            break;
                        case 2: // fake autotile
                            GameState.EditorTileWidth = 1;
                            GameState.EditorTileHeight = 1;
                            break;
                        case 3: // animated
                            GameState.EditorTileWidth = 6;
                            GameState.EditorTileHeight = 3;
                            break;
                        case 4: // cliff
                            GameState.EditorTileWidth = 2;
                            GameState.EditorTileHeight = 2;
                            break;
                        case 5: // waterfall
                            GameState.EditorTileWidth = 2;
                            GameState.EditorTileHeight = 3;
                            break;
                    }
                }

                // Corrected: Use integer division to get the tile index, not Math.Round
                GameState.EditorTileX = (int)((X + tilesetOffsetX) / GameState.SizeX);
                GameState.EditorTileY = (int)((Y + tilesetOffsetY) / GameState.SizeY);

                GameState.EditorTileSelStart = new Point(GameState.EditorTileX, GameState.EditorTileY);
                GameState.EditorTileSelEnd = new Point(
                    GameState.EditorTileX + GameState.EditorTileWidth,
                    GameState.EditorTileY + GameState.EditorTileHeight
                );
            }
        }

        public static void MapEditorDrag(int Button, float X, float Y)
        {
            if (GameState.CurAutotileType > 0)
                return;

            // Eto.Forms uses MouseButtons.Primary instead of Left
            if (Button == (int)MouseButtons.Primary) // Primary (Left) Mouse Button
            {
                // convert the pixel number to tile number
                X = (long)Math.Round((X + tilesetOffsetX) / GameState.SizeX) + 1L;
                Y = (long)Math.Round((Y + tilesetOffsetY) / GameState.SizeY) + 1L;

                // check it's not out of bounds
                if (X < 0f)
                    X = 0f;

                if ((double)X > (Instance.picBackSelect.Width + tilesetOffsetX) / (double)GameState.SizeX)
                    X = (float)((Instance.picBackSelect.Width + tilesetOffsetX) / (double)GameState.SizeX);
                
                if (Y < 0f)
                    Y = 0f;

                if ((double)Y > (Instance.picBackSelect.Height + tilesetOffsetY) / (double)GameState.SizeY)
                    Y = (float)((Instance.picBackSelect.Height + tilesetOffsetY) / (double)GameState.SizeY);

                // find out what to set the width + height of map editor to
                if (X > GameState.EditorTileX) // drag right
                {
                    GameState.EditorTileWidth = (int)Math.Round(X - GameState.EditorTileX);
                }

                if (Y > GameState.EditorTileY) // drag down
                {
                    GameState.EditorTileHeight = (int)Math.Round(Y - GameState.EditorTileY);
                }

                GameState.EditorTileSelStart = new Point(GameState.EditorTileX, GameState.EditorTileY);
                GameState.EditorTileSelEnd = new Point(GameState.EditorTileWidth, GameState.EditorTileHeight);
            }

        }

        public static void MapEditorMouseDown(int x, int y, bool movedMouse = true)
        {
            int i;
            bool isModified = false;

            if (GameState.CurX < 0 || GameState.CurY < 0 || GameState.CurX >= Data.MyMap.MaxX || GameState.CurY >= Data.MyMap.MaxY)
                return;

            if (!GameLogic.IsInBounds())
                return;

            if (GameState.EyeDropper)
            {
                MapEditorEyeDropper();
                return;
            }

            var withBlock = Data.MyMap.Tile[x, y];

            if (GameClient.IsMouseButtonDown(MouseButton.Left))
            {
                if (Instance.optInfo.Checked)
                {
                    if (GameState.Info == false)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            GameLogic.Dialogue("Map Editor", "Info: " + System.Enum.GetName(Data.MyMap.Tile[GameState.CurX, GameState.CurY].Type), " Data 1: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data1 + " Data 2: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data2 + " Data 3: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data3, (byte)DialogueType.Information, (byte)DialogueStyle.Okay);
                        }
                        else
                        {
                            GameLogic.Dialogue("Map Editor", "Info: " + System.Enum.GetName(Data.MyMap.Tile[GameState.CurX, GameState.CurY].Type2), " Data 1: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data1_2 + " Data 2: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data2_2 + " Data 3: " + Data.MyMap.Tile[GameState.CurX, GameState.CurY].Data3_2, (byte)DialogueType.Information, (byte)DialogueStyle.Okay);
                        }
                    }
                }

                if (GameState.MapEditorTab == (int)MapEditorTab.Tiles)
                {
                    if (GameState.EditorTileWidth == 1 & GameState.EditorTileHeight == 1) // single tile
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, false, (byte)GameState.CurAutotileType);
                    }
                    else if (GameState.CurAutotileType == 0) // multi tile!
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, true);
                    }
                    else
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, true, (byte)GameState.CurAutotileType);
                    }
                }
                else if (GameState.MapEditorTab == (int)MapEditorTab.Attributes)
                {
                    ref var withBlock1 = ref Data.MyMap.Tile[GameState.CurX, GameState.CurY];
                    // blocked tile
                    if (Instance.optBlocked.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Blocked;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Blocked;
                        }
                    }

                    // warp tile
                    if (Instance.optWarp.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Warp;
                            withBlock1.Data1 = GameState.EditorWarpMap;
                            withBlock1.Data2 = GameState.EditorWarpX;
                            withBlock1.Data3 = GameState.EditorWarpY;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Warp;
                            withBlock1.Data1_2 = GameState.EditorWarpMap;
                            withBlock1.Data2_2 = GameState.EditorWarpX;
                            withBlock1.Data3_2 = GameState.EditorWarpY;
                        }
                    }

                    // item spawn
                    if (Instance.optItem.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Item;
                            withBlock1.Data1 = GameState.ItemEditorNum;
                            withBlock1.Data2 = GameState.ItemEditorValue;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Item;
                            withBlock1.Data1_2 = GameState.ItemEditorNum;
                            withBlock1.Data2_2 = GameState.ItemEditorValue;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // Npc avoid
                    if (Instance.optNpcAvoid.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.NpcAvoid;
                            withBlock1.Data1 = 0;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.NpcAvoid;
                            withBlock1.Data1_2 = 0;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // resource
                    if (Instance.optResource.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Resource;
                            withBlock1.Data1 = GameState.ResourceEditorNum;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Resource;
                            withBlock1.Data1_2 = GameState.ResourceEditorNum;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // Npc spawn
                    if (Instance.optNpcSpawn.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.NpcSpawn;
                            withBlock1.Data1 = GameState.SpawnNpcNum;
                            withBlock1.Data2 = GameState.SpawnNpcDir;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.NpcSpawn;
                            withBlock1.Data1_2 = GameState.SpawnNpcNum;
                            withBlock1.Data2_2 = GameState.SpawnNpcDir;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // shop
                    if (Instance.optShop.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Shop;
                            withBlock1.Data1 = GameState.EditorShop;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Shop;
                            withBlock1.Data1_2 = GameState.EditorShop;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // bank
                    if (Instance.optBank.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Bank;
                            withBlock1.Data1 = 0;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Bank;
                            withBlock1.Data1_2 = 0;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // heal
                    if (Instance.optHeal.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Heal;
                            withBlock1.Data1 = GameState.MapEditorHealType;
                            withBlock1.Data2 = GameState.MapEditorHealAmount;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Heal;
                            withBlock1.Data1_2 = GameState.MapEditorHealType;
                            withBlock1.Data2_2 = GameState.MapEditorHealAmount;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // trap
                    if (Instance.optTrap.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Trap;
                            withBlock1.Data1 = GameState.MapEditorHealAmount;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Trap;
                            withBlock1.Data1_2 = GameState.MapEditorHealAmount;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // Animation
                    if (Instance.optAnimation.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.Animation;
                            withBlock1.Data1 = GameState.EditorAnimation;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.Animation;
                            withBlock1.Data1_2 = GameState.EditorAnimation;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }

                    // No Xing
                    if (Instance.optNoCrossing.Checked == true)
                    {
                        if (GameState.EditorAttribute == 1)
                        {
                            withBlock1.Type = TileType.NoCrossing;
                            withBlock1.Data1 = 0;
                            withBlock1.Data2 = 0;
                            withBlock1.Data3 = 0;
                        }
                        else
                        {
                            withBlock1.Type2 = TileType.NoCrossing;
                            withBlock1.Data1_2 = 0;
                            withBlock1.Data2_2 = 0;
                            withBlock1.Data3_2 = 0;
                        }
                    }
                }
                else if (GameState.MapEditorTab == (int)MapEditorTab.Directions)
                {
                    // Convert adjusted coordinates to game world coordinates
                    x = (int)Math.Round(GameState.TileView.Left + Math.Floor((GameState.CurMouseX + GameState.Camera.Left) % GameState.SizeX));
                    y = (int)Math.Round(GameState.TileView.Top + Math.Floor((GameState.CurMouseY + GameState.Camera.Top) % GameState.SizeY));

                    // see if it hits an arrow
                    for (i = 0; i < 4; i++)
                    {
                        // flip the value.
                        if (x >= GameState.DirArrowX[i] & x <= GameState.DirArrowX[i] + 16)
                        {
                            if (y >= GameState.DirArrowY[i] & y <= GameState.DirArrowY[i] + 16)
                            {
                                // flip the value.
                                bool localIsDirBlocked() { byte argdir = (byte)i; var dirBlocked = GameLogic.IsDirBlocked(ref Data.MyMap.Tile[GameState.CurX, GameState.CurY].DirBlock, ref argdir); return dirBlocked; }

                                byte argdir = (byte)i;
                                GameLogic.SetDirBlock(ref Data.MyMap.Tile[GameState.CurX, GameState.CurY].DirBlock, ref argdir, !localIsDirBlocked());
                                break;
                            }
                        }
                    }
                }
                else if (GameState.MapEditorTab == (int)MapEditorTab.Events)
                {
                    if (Editor_Event.Instance == null || Editor_Event.Instance.Visible == false)
                    {
                        if (Event.EventCopy)
                        {
                            Event.CopyEvent_Map(GameState.CurX, GameState.CurY);
                        }
                        else if (Event.EventPaste)
                        {
                            Event.PasteEvent_Map(GameState.CurX, GameState.CurY);
                        }
                        else
                        {
                            Event.AddEvent(GameState.CurX, GameState.CurY);
                        }
                    }
                }
            }

            if (GameClient.IsMouseButtonDown(MouseButton.Right))
            {
                if (GameState.MapEditorTab == (int)MapEditorTab.Tiles)
                {
                    if (GameState.EditorTileWidth == 1 & GameState.EditorTileHeight == 1) // single tile
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, false, (byte)GameState.CurAutotileType, 1);
                    }
                    else if (GameState.CurAutotileType == 0) // multi tile!
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, true, 0, 1);
                    }
                    else
                    {
                        MapEditorSetTile(GameState.CurX, GameState.CurY, GameState.CurLayer, true, (byte)GameState.CurAutotileType, 1);
                    }
                }
                else if (GameState.MapEditorTab == (int)MapEditorTab.Attributes)
                {
                    ref var withBlock2 = ref Data.MyMap.Tile[GameState.CurX, GameState.CurY];
                    // clear attribute
                    withBlock2.Type = 0;
                    withBlock2.Data1 = 0;
                    withBlock2.Data2 = 0;
                    withBlock2.Data3 = 0;
                    withBlock2.Type2 = 0;
                    withBlock2.Data1_2 = 0;
                    withBlock2.Data2_2 = 0;
                    withBlock2.Data3_2 = 0;
                }
                else if (GameState.MapEditorTab == (int)MapEditorTab.Events)
                    Event.DeleteEvent(GameState.CurX, GameState.CurY);
            }

            MapEditorHistory();

            x = 0;

            for (int x2 = 0, loopTo = Data.MyMap.MaxX; x2 < loopTo; x2++)
            {
                for (int y2 = 0, loopTo1 = Data.MyMap.MaxY; y2 < loopTo1; y2++)
                {
                    // Use Layer.Length instead of MapLayer.Count
                    for (int i2 = 0, loopTo2 = Data.MyMap.Tile[x2, y2].Layer != null ? Data.MyMap.Tile[x2, y2].Layer.Length : 0; i2 < loopTo2; i2++)
                    {
                        ref var currentTile = ref Data.MyMap.Tile[x2, y2];
                        ref var historyTile = ref Data.TileHistory[GameState.TileHistoryIndex].Tile[x2, y2];

                        // Check Layer array length for both tiles
                        if (currentTile.Layer == null || currentTile.Layer.Length <= i2 || historyTile.Layer == null || historyTile.Layer.Length <= i2)
                        {
                            continue; // Skip processing if Layer is not properly initialized
                        }

                        // Check if the tile is modified
                        isModified = currentTile.Data1 != historyTile.Data1 ||
                                            currentTile.Data2 != historyTile.Data2 ||
                                            currentTile.Data3 != historyTile.Data3 ||
                                            currentTile.Data1_2 != historyTile.Data1_2 ||
                                            currentTile.Data2_2 != historyTile.Data2_2 ||
                                            currentTile.Data3_2 != historyTile.Data3_2 ||
                                            currentTile.Type != historyTile.Type ||
                                            currentTile.Type2 != historyTile.Type2 ||
                                            currentTile.DirBlock != historyTile.DirBlock ||
                                            currentTile.Layer[i2].X != historyTile.Layer[i2].X ||
                                            currentTile.Layer[i2].Y != historyTile.Layer[i2].Y ||
                                            currentTile.Layer[i2].Tileset != historyTile.Layer[i2].Tileset ||
                                            currentTile.Layer[i2].AutoTile != historyTile.Layer[i2].AutoTile;

                        if (isModified)
                        {
                            historyTile.Data1 = currentTile.Data1;
                            historyTile.Data2 = currentTile.Data2;
                            historyTile.Data3 = currentTile.Data3;
                            historyTile.Data1_2 = currentTile.Data1_2;
                            historyTile.Data2_2 = currentTile.Data2_2;
                            historyTile.Data3_2 = currentTile.Data3_2;
                            historyTile.Type = currentTile.Type;
                            historyTile.Type2 = currentTile.Type2;
                            historyTile.DirBlock = currentTile.DirBlock;
                            historyTile.Layer[i2].X = currentTile.Layer[i2].X;
                            historyTile.Layer[i2].Y = currentTile.Layer[i2].Y;
                            historyTile.Layer[i2].Tileset = currentTile.Layer[i2].Tileset;
                            historyTile.Layer[i2].AutoTile = currentTile.Layer[i2].AutoTile;

                            if (historyTile.Layer[i2].AutoTile > 0)
                            {
                                x = 1;
                            }

                            Autotile.CacheRenderState(x2, y2, i2);
                        }
                    }
                }
            }

            if (GameClient.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || GameClient.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
            {
                MapEditorReplaceTile((MapLayer)GameState.CurLayer, GameState.CurX, GameState.CurY, withBlock);
            }

            if (x == 1)
            {
                // do a re-init so we can see our changes
                Autotile.InitAutotiles();
            }
        }

        public static void MapEditorCancel()
        {
            if (GameState.MyEditorType != EditorType.Map)
            {
                return;
            }

            var packetWriter = new PacketWriter(8);
            
            packetWriter.WriteEnum(Packets.ClientPackets.CNeedMap);
            packetWriter.WriteInt32(1);
            
            Network.Send(packetWriter);
            
            GameState.MyEditorType = EditorType.None;
            GameState.GettingMap = true;
            
            Sender.SendCloseEditor();

            // show gui
            Gui.ShowWindow(Gui.GetWindowIndex("winHotbar"), resetPosition: false);
            Gui.ShowWindow(Gui.GetWindowIndex("winMenu"), resetPosition: false);
            Gui.ShowWindow(Gui.GetWindowIndex("winBars"), resetPosition: false);
            Gui.HideChat();

            Editor_Event.Instance?.Dispose();

            GameState.TileHistoryHighIndex = 0;
            GameState.TileHistoryIndex = 0;
        }

        public static void MapEditorSend()
        {
            Map.SendMap();
            GameState.MyEditorType = EditorType.None;
            GameState.GettingMap = true;
            Sender.SendCloseEditor();

            // show gui
            Gui.ShowWindow(Gui.GetWindowIndex("winHotbar"), resetPosition: false);
            Gui.ShowWindow(Gui.GetWindowIndex("winMenu"), resetPosition: false);
            Gui.ShowWindow(Gui.GetWindowIndex("winBars"), resetPosition: false);
            Gui.HideChat();

            Editor_Event.Instance?.Dispose();

            GameState.TileHistoryHighIndex = 0;
            GameState.TileHistoryIndex = 0;
        }

        public static void MapEditorSetTile(int x, int y, int CurLayer, bool multitile = false, byte theAutotile = 0, byte eraseTile = 0)
        {
            int x2;
            int y2;
            int newTileX;
            int newTileY;

            newTileX = GameState.EditorTileX;
            newTileY = GameState.EditorTileY;

            if (Conversions.ToBoolean(eraseTile))
            {
                newTileX = 0;
                newTileY = 0;
            }

            if (theAutotile > 0)
            {
                ref var withBlock = ref Data.MyMap.Tile[x, y];
                // set layer
                withBlock.Layer[CurLayer].X = newTileX;
                withBlock.Layer[CurLayer].Y = newTileY;
                if (Conversions.ToBoolean(eraseTile))
                {
                    withBlock.Layer[CurLayer].Tileset = 0;
                }
                else
                {
                    withBlock.Layer[CurLayer].Tileset = GameState.CurTileset;
                }
                withBlock.Layer[CurLayer].AutoTile = theAutotile;
                Autotile.CacheRenderState(x, y, CurLayer);

                // do a re-init so we can see our changes
                Autotile.InitAutotiles();
                return;
            }

            if (!multitile) // single
            {
                ref var withBlock1 = ref Data.MyMap.Tile[x, y];
                // set layer
                withBlock1.Layer[CurLayer].X = newTileX;
                withBlock1.Layer[CurLayer].Y = newTileY;
                if (Conversions.ToBoolean(eraseTile))
                {
                    withBlock1.Layer[CurLayer].Tileset = 0;
                }
                else
                {
                    withBlock1.Layer[CurLayer].Tileset = GameState.CurTileset;
                }
                withBlock1.Layer[CurLayer].AutoTile = 0;
                Autotile.CacheRenderState(x, y, CurLayer);
            }
            else // multitile
            {
                y2 = 0; // starting tile for y axis
                var loopTo = GameState.CurY + GameState.EditorTileHeight;
                for (y = GameState.CurY; y < loopTo; y++)
                {
                    x2 = 0; // re-set x count every y loop
                    var loopTo1 = GameState.CurX + GameState.EditorTileWidth;
                    for (x = GameState.CurX; x < loopTo1; x++)
                    {
                        if (x >= 0 & x < Data.MyMap.MaxX)
                        {
                            if (y >= 0 & y < Data.MyMap.MaxY)
                            {
                                ref var withBlock2 = ref Data.MyMap.Tile[x, y];
                                withBlock2.Layer[CurLayer].X = newTileX + x2;
                                withBlock2.Layer[CurLayer].Y = newTileY + y2;
                                if (Conversions.ToBoolean(eraseTile))
                                {
                                    withBlock2.Layer[CurLayer].Tileset = 0;
                                }
                                else
                                {
                                    withBlock2.Layer[CurLayer].Tileset = GameState.CurTileset;
                                }
                                withBlock2.Layer[CurLayer].AutoTile = 0;
                                Autotile.CacheRenderState(x, y, CurLayer);
                            }
                        }
                        x2 += 1;
                    }
                    y2 += 1;
                }
            }
        }

        public static void MapEditorHistory()
        {
            if (GameState.TileHistoryIndex <= 0)
                GameState.TileHistoryIndex = 0;

            if (GameState.TileHistoryIndex >= GameState.MaxTileHistory - 1)
            {
                for (int i = 0; i < GameState.TileHistoryIndex; i++)
                {
                    Data.TileHistory[(int)i] = Data.TileHistory[(int)(i + 1)];
                }
            }
            else
            {
                GameState.TileHistoryIndex++;
                GameState.TileHistoryHighIndex++;

                if (GameState.TileHistoryHighIndex > GameState.MaxTileHistory)
                    GameState.TileHistoryHighIndex = GameState.MaxTileHistory;

            }

        }

        public static void MapEditorClearLayer(MapLayer layer)
        {
            GameLogic.Dialogue("Map Editor", "Clear Layer: " + layer.ToString(), "Are you sure you wish to clear this layer?", (byte)DialogueType.ClearLayer, (byte)DialogueStyle.YesNo, GameState.CurLayer, GameState.CurAutotileType);
        }

        public static void MapEditorFillLayer(MapLayer layer, byte theAutotile = 0, byte tileX = 0, byte tileY = 0)
        {
            GameLogic.Dialogue("Map Editor", "Fill Layer: " + layer.ToString(), "Are you sure you wish to fill this layer?", (byte)DialogueType.FillLayer, (byte)DialogueStyle.YesNo, GameState.CurLayer, GameState.CurAutotileType, tileX, tileY, (Instance?.cmbTileSets.SelectedIndex ?? -1) + 1);
        }

        public static void MapEditorEyeDropper()
        {
            int CurLayer;

            CurLayer = GameState.CurLayer;

            {
                ref var withBlock = ref Data.MyMap.Tile[GameState.CurX, GameState.CurY];
                GameState.CurTileset = withBlock.Layer[CurLayer].Tileset;
                MapEditorChooseTile((int)MouseButtons.Primary, withBlock.Layer[CurLayer].X * GameState.SizeX, withBlock.Layer[CurLayer].Y * GameState.SizeY);
                GameState.EyeDropper = !GameState.EyeDropper;
            }
        }

        public static void MapEditorUndo()
        {
            bool isModified = false;

            if (GameState.TileHistoryIndex <= 0)
            {
                return;
            }

            int layerCount = Enum.GetValues(typeof(MapLayer)).Length;

            for (int x = 0, loopTo = Data.MyMap.MaxX; x < loopTo; x++)
            {
                for (int y = 0, loopTo1 = Data.MyMap.MaxY; y < loopTo1; y++)
                {
                    for (int i = 0; i < layerCount; i++)
                    {
                        ref var currentTile = ref Data.MyMap.Tile[x, y];
                        ref var historyTile = ref Data.TileHistory[GameState.TileHistoryIndex].Tile[x, y];

                        if (currentTile.Layer == null || currentTile.Layer.Length <= i || historyTile.Layer == null || historyTile.Layer.Length <= i)
                        {
                            continue; // Skip processing if Layer is not properly initialized
                        }

                        if (!isModified)
                        {
                            // Check if the tile is modified
                            isModified = currentTile.Data1 != historyTile.Data1 ||
                                                currentTile.Data2 != historyTile.Data2 ||
                                                currentTile.Data3 != historyTile.Data3 ||
                                                currentTile.Data1_2 != historyTile.Data1_2 ||
                                                currentTile.Data2_2 != historyTile.Data2_2 ||
                                                currentTile.Data3_2 != historyTile.Data3_2 ||
                                                currentTile.Type != historyTile.Type ||
                                                currentTile.Type2 != historyTile.Type2 ||
                                                currentTile.DirBlock != historyTile.DirBlock ||
                                                currentTile.Layer[i].X != historyTile.Layer[i].X ||
                                                currentTile.Layer[i].Y != historyTile.Layer[i].Y ||
                                                currentTile.Layer[i].Tileset != historyTile.Layer[i].Tileset ||
                                                currentTile.Layer[i].AutoTile != historyTile.Layer[i].AutoTile;
                        }

                        currentTile.Data1 = historyTile.Data1;
                        currentTile.Data2 = historyTile.Data2;
                        currentTile.Data3 = historyTile.Data3;
                        currentTile.Data1_2 = historyTile.Data1_2;
                        currentTile.Data2_2 = historyTile.Data2_2;
                        currentTile.Data3_2 = historyTile.Data3_2;
                        currentTile.Type = historyTile.Type;
                        currentTile.Type2 = historyTile.Type2;
                        currentTile.DirBlock = historyTile.DirBlock;
                        currentTile.Layer[i].X = historyTile.Layer[i].X;
                        currentTile.Layer[i].Y = historyTile.Layer[i].Y;
                        currentTile.Layer[i].Tileset = historyTile.Layer[i].Tileset;
                        currentTile.Layer[i].AutoTile = historyTile.Layer[i].AutoTile;
                        Autotile.CacheRenderState(x, y, i);

                        if (currentTile.Layer[i].AutoTile > 0)
                        {
                            // do a re-init so we can see our changes
                            Autotile.InitAutotiles();
                        }
                    }
                }
            }

            GameState.TileHistoryIndex -= 1;

            if (!isModified)
            {
                MapEditorUndo();
            }
        }

        public static void MapEditorRedo()
        {
            bool isModified = false;

            if (GameState.TileHistoryIndex > GameState.TileHistoryHighIndex)
            {
                GameState.TileHistoryIndex--;
                return;
            }

            int layerCount = Enum.GetValues(typeof(MapLayer)).Length;

            for (int x = 0, loopTo = Data.MyMap.MaxX; x < loopTo; x++)
            {
                for (int y = 0, loopTo1 = Data.MyMap.MaxY; y < loopTo1; y++)
                {
                    for (int i = 0; i < layerCount; i++)
                    {
                        ref var currentTile = ref Data.MyMap.Tile[x, y];
                        ref var historyTile = ref Data.TileHistory[GameState.TileHistoryIndex].Tile[x, y];

                        if (currentTile.Layer == null || currentTile.Layer.Length <= i || historyTile.Layer == null || historyTile.Layer.Length <= i)
                        {
                            continue; // Skip processing if Layer is not properly initialized
                        }

                        if (!isModified)
                        {
                            // Check if the tile is modified
                            isModified = currentTile.Data1 != historyTile.Data1 ||
                                                currentTile.Data2 != historyTile.Data2 ||
                                                currentTile.Data3 != historyTile.Data3 ||
                                                currentTile.Data1_2 != historyTile.Data1_2 ||
                                                currentTile.Data2_2 != historyTile.Data2_2 ||
                                                currentTile.Data3_2 != historyTile.Data3_2 ||
                                                currentTile.Type != historyTile.Type ||
                                                currentTile.Type2 != historyTile.Type2 ||
                                                currentTile.DirBlock != historyTile.DirBlock ||
                                                currentTile.Layer[i].X != historyTile.Layer[i].X ||
                                                currentTile.Layer[i].Y != historyTile.Layer[i].Y ||
                                                currentTile.Layer[i].Tileset != historyTile.Layer[i].Tileset ||
                                                currentTile.Layer[i].AutoTile != historyTile.Layer[i].AutoTile;
                        }

                        currentTile.Data1 = historyTile.Data1;
                        currentTile.Data2 = historyTile.Data2;
                        currentTile.Data3 = historyTile.Data3;
                        currentTile.Data1_2 = historyTile.Data1_2;
                        currentTile.Data2_2 = historyTile.Data2_2;
                        currentTile.Data3_2 = historyTile.Data3_2;
                        currentTile.Type = historyTile.Type;
                        currentTile.Type2 = historyTile.Type2;
                        currentTile.DirBlock = historyTile.DirBlock;
                        currentTile.Layer[i].X = historyTile.Layer[i].X;
                        currentTile.Layer[i].Y = historyTile.Layer[i].Y;
                        currentTile.Layer[i].Tileset = historyTile.Layer[i].Tileset;
                        currentTile.Layer[i].AutoTile = historyTile.Layer[i].AutoTile;
                        Autotile.CacheRenderState(x, y, i);

                        if (currentTile.Layer[i].AutoTile > 0)
                        {
                            // do a re-init so we can see our changes
                            Autotile.InitAutotiles();
                        }
                    }
                }
            }

            GameState.TileHistoryIndex++;

            if (!isModified)
            {
                MapEditorRedo();
            }
        }

        public void ClearAttributeDialogue()
        {
            fraNpcSpawn.Visible = false;
            fraResource.Visible = false;
            fraMapItem.Visible = false;
            fraMapWarp.Visible = false;
            fraShop.Visible = false;
            fraHeal.Visible = false;
            fraTrap.Visible = false;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Data.MyMap.Name = txtName.Text;
        }

    // WinForms FormClosing replaced by manual call site when disposing
    private void Editor_Map_FormClosing(object? sender, EventArgs e) => MapEditorCancel();

        private void scrMapBrightness_Scroll(object sender, EventArgs e)
        {
            if (lblMapBrightness == null)
                return;

            Data.MyMap.Brightness = (byte)scrlMapBrightness.Value;
            lblMapBrightness.Text = "Brightness: " + scrlMapBrightness.Value;
        }

        public static void MapEditorCopyMap()
        {
            int i;
            int x;
            int y;

            // Get the number of layers from the MapLayer enum
            int layerCount = Enum.GetValues(typeof(MapLayer)).Length;

            if (GameState.CopyMap == false)
            {
                Data.TempTile = new Tile[Data.MyMap.MaxX, Data.MyMap.MaxY];
                GameState.TmpMaxX = Data.MyMap.MaxX;
                GameState.TmpMaxY = Data.MyMap.MaxY;

                var loopTo = (int)Data.MyMap.MaxX;
                for (x = 0; x < loopTo; x++)
                {
                    var loopTo1 = (int)Data.MyMap.MaxY;
                    for (y = 0; y < loopTo1; y++)
                    {
                        ref var withBlock = ref Data.MyMap.Tile[x, y];
                        Data.TempTile[x, y].Layer = new Type.Layer[layerCount];

                        Data.TempTile[x, y].Data1 = withBlock.Data1;
                        Data.TempTile[x, y].Data2 = withBlock.Data2;
                        Data.TempTile[x, y].Data3 = withBlock.Data3;
                        Data.TempTile[x, y].Type = withBlock.Type;
                        Data.TempTile[x, y].Data1_2 = withBlock.Data1_2;
                        Data.TempTile[x, y].Data2_2 = withBlock.Data2_2;
                        Data.TempTile[x, y].Data3_2 = withBlock.Data3_2;
                        Data.TempTile[x, y].Type2 = withBlock.Type2;
                        Data.TempTile[x, y].DirBlock = withBlock.DirBlock;

                        for (i = 0; i < layerCount; i++)
                        {
                            Data.TempTile[x, y].Layer[i].X = withBlock.Layer[i].X;
                            Data.TempTile[x, y].Layer[i].Y = withBlock.Layer[i].Y;
                            Data.TempTile[x, y].Layer[i].Tileset = withBlock.Layer[i].Tileset;
                            Data.TempTile[x, y].Layer[i].AutoTile = withBlock.Layer[i].AutoTile;
                        }
                    }
                }

                GameState.CopyMap = true;
                GameLogic.Dialogue("Map Editor", "Map Copy: ", "Press the button again to paste.", (byte)DialogueType.CopyMap, (byte)DialogueStyle.Okay);
            }
            else
            {
                Data.MyMap.MaxX = GameState.TmpMaxX;
                Data.MyMap.MaxY = GameState.TmpMaxY;

                var loopTo2 = (int)Data.MyMap.MaxX;
                for (x = 0; x < loopTo2; x++)
                {
                    var loopTo3 = (int)Data.MyMap.MaxY;
                    for (y = 0; y < loopTo3; y++)
                    {
                        ref var withBlock1 = ref Data.MyMap.Tile[x, y];
                        Array.Resize(ref Data.MyMap.Tile[x, y].Layer, layerCount);
                        Array.Resize(ref Data.Autotile[x, y].Layer, layerCount);

                        withBlock1.Data1 = Data.TempTile[x, y].Data1;
                        withBlock1.Data2 = Data.TempTile[x, y].Data2;
                        withBlock1.Data3 = Data.TempTile[x, y].Data3;
                        withBlock1.Type = Data.TempTile[x, y].Type;
                        withBlock1.Data1_2 = Data.TempTile[x, y].Data1_2;
                        withBlock1.Data2_2 = Data.TempTile[x, y].Data2_2;
                        withBlock1.Data3_2 = Data.TempTile[x, y].Data3_2;
                        withBlock1.Type2 = Data.TempTile[x, y].Type2;
                        withBlock1.DirBlock = Data.TempTile[x, y].DirBlock;

                        for (i = 0; i < layerCount; i++)
                        {
                            withBlock1.Layer[i].X = Data.TempTile[x, y].Layer[i].X;
                            withBlock1.Layer[i].Y = Data.TempTile[x, y].Layer[i].Y;
                            withBlock1.Layer[i].Tileset = Data.TempTile[x, y].Layer[i].Tileset;
                            withBlock1.Layer[i].AutoTile = Data.TempTile[x, y].Layer[i].AutoTile;
                            Autotile.CacheRenderState(x, y, i);
                        }
                    }
                }

                GameLogic.Dialogue("Map Editor", "Map Paste: ", "Map has been updated.", (byte)DialogueType.PasteMap, (byte)DialogueStyle.Okay);

                // do a re-init so we can see our changes
                Autotile.InitAutotiles();

                GameState.CopyMap = false;
            }
        }

        private void tsbCopyMap_Click(object sender, EventArgs e)
        {
            MapEditorCopyMap();
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            MapEditorUndo();
        }

        private void tsbRedo_Click(object sender, EventArgs e)
        {
            MapEditorRedo();
        }

        private void tsbOpacity_Click(object sender, EventArgs e)
        {
            GameState.HideLayers = !GameState.HideLayers;
        }

        private void tsbScreenshot_Click(object sender, EventArgs e)
        {
            GameClient.TakeScreenshot();
        }

        private void tsbTileset_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Data.MyMap.MaxY; y++)
            {
                for (int x = 0; x < Data.MyMap.MaxX; x++)
                {
                    for (int i = 0; i < Data.MyMap.Tile[x, y].Layer.Length; i++)
                    {
                        ref var tile = ref Data.MyMap.Tile[x, y];

                        if (tile.Layer[i].Tileset == 0)
                            continue;

                        tile.Layer[i].Tileset++;

                        Autotile.CacheRenderState(x, y, i);
                    }
                }
            }
        }

        private void optAnimation_CheckedChanged(object sender, EventArgs e)
        {
            if (optAnimation.Checked == false)
                return;

            ClearAttributeDialogue();
            pnlAttributes.Visible = true;
            fraAnimation.Visible = true;
        }

        private void brnAnimation_Click(object sender, EventArgs e)
        {
            GameState.EditorAnimation = cmbAnimation.SelectedIndex;
            pnlAttributes.Visible = false;
            fraAnimation.Visible = false;
        }

        private void chkRespawn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoMapRespawn.Checked == true)
            {
                Data.MyMap.NoRespawn = true;
            }
            else
            {
                Data.MyMap.NoRespawn = false;
            }
        }

        private void chkIndoors_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIndoors.Checked == true)
            {
                Data.MyMap.Indoors = true;
            }
            else
            {
                Data.MyMap.Indoors = false;
            }
        }

        private void cmbAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameState.EditorAttribute = (byte)(cmbAttribute.SelectedIndex);
        }

        private void tsbDeleteMap_Click(object sender, EventArgs e)
        {
            GameLogic.Dialogue("Map Editor", "Clear Map: ", "Are you sure you want to clear this map?", (byte)DialogueType.ClearMap, (byte)DialogueStyle.YesNo);
        }
    // Removed obsolete WinForms Paint handler (ImageView has no Paint event in Eto)

        private void btnFillAttributes_Click(object sender, EventArgs e)
        {
            GameLogic.Dialogue("Map Editor", "Fill Attributes: ", "Are you sure you wish to fill attributes?", (byte)DialogueType.FillAttributes, (byte)DialogueStyle.YesNo);
        }

        private void ToolStrip_MouseHover(object sender, EventArgs e)
        {
            Focus();
        }

        private void tabPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameState.MapEditorTab = Instance.tabPages.SelectedIndex;

            if (GameState.MapEditorTab == (int)MapEditorTab.Attributes)
            {
                cmbAttribute.SelectedIndex = 1;
            }
        }

        private void cmbTileSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameState.CurTileset = cmbTileSets.SelectedIndex + 1;
        }

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameState.CurLayer = cmbLayers.SelectedIndex;
        }

        /// <summary>
        /// Replaces the X/Y coordinates of all tiles in the given layer with the specified values.
        /// </summary>
        /// <param name="layer">The layer to update.</param>
        /// <param name="tileX">The new X coordinate to set.</param>
        /// <param name="tileY">The new Y coordinate to set.</param>
        public static void MapEditorReplaceTile(MapLayer layer, int tileX, int tileY, Type.Tile oldTile)
        {
            int maxX = Data.MyMap.MaxX;
            int maxY = Data.MyMap.MaxY;

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    ref var tile = ref Data.MyMap.Tile[x, y];
                    if ((int)MapEditorTab.Tiles == GameState.MapEditorTab)
                    {
                        if (tile.Layer[(int)layer].X == oldTile.Layer[(int)layer].X && tile.Layer[(int)layer].Y == oldTile.Layer[(int)layer].Y)
                        {
                            if (GameClient.IsMouseButtonDown(MouseButton.Left))
                            {
                                tile.Layer[(int)layer].X = Data.MyMap.Tile[tileX, tileY].Layer[(int)layer].X;
                                tile.Layer[(int)layer].Y = Data.MyMap.Tile[tileX, tileY].Layer[(int)layer].Y;
                                tile.Layer[(int)layer].Tileset = Data.MyMap.Tile[tileX, tileY].Layer[(int)layer].Tileset;
                            }
                            else if (GameClient.IsMouseButtonDown(MouseButton.Right))
                            {
                                tile.Layer[(int)layer].X = 0;
                                tile.Layer[(int)layer].Y = 0;
                                tile.Layer[(int)layer].Tileset = 0;
                            }
                            else
                            {
                                return; // No mouse button pressed, exit early
                            }

                            tile.Layer[(int)layer].AutoTile = 0;
                            Autotile.CacheRenderState(x, y, (int)layer);
                        }
                    }
                    else if ((int)MapEditorTab.Attributes == GameState.MapEditorTab)
                    {
                        if (GameClient.IsMouseButtonDown(MouseButton.Left))
                        {
                            if (GameState.EditorAttribute == 1)
                            {
                                tile.Data1 = Data.MyMap.Tile[tileX, tileY].Data1;
                                tile.Data2 = Data.MyMap.Tile[tileX, tileY].Data2;
                                tile.Data3 = Data.MyMap.Tile[tileX, tileY].Data3;
                                tile.Type = Data.MyMap.Tile[tileX, tileY].Type;
                            }
                            else
                            {
                                tile.Data1_2 = Data.MyMap.Tile[tileX, tileY].Data1_2;
                                tile.Data2_2 = Data.MyMap.Tile[tileX, tileY].Data2_2;
                                tile.Data3_2 = Data.MyMap.Tile[tileX, tileY].Data3_2;
                                tile.Type2 = Data.MyMap.Tile[tileX, tileY].Type2;
                            }
                        }

                        if (GameClient.IsMouseButtonDown(MouseButton.Right))
                        {
                            if (GameState.EditorAttribute == 1)
                            {
                                tile.Data1 = 0;
                                tile.Data2 = 0;
                                tile.Data3 = 0;
                                tile.Type = 0;
                            }
                            else
                            {
                                tile.Data1_2 = 0;
                                tile.Data2_2 = 0;
                                tile.Data3_2 = 0;
                                tile.Type2 = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion
}
using Eto.Forms;
using Eto.Drawing;
using Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Net;

namespace Client
{
    public class Editor_Script : Form
    {
        private static Editor_Script? _instance;
        public static Editor_Script Instance => _instance ??= new Editor_Script();

        private Button btnOpenScript = new Button { Text = "Open Script" };
        private Button btnSaveScript = new Button { Text = "Save Script" };
        private TextArea txtPreview = new TextArea { ReadOnly = true, Wrap = false, Size = new Size(600,400) };
        private Label lblInfo = new Label { Text = "Open the script in your external editor, then Save to reload and send." };

        private Editor_Script()
        {
            Title = "Script Editor";
            ClientSize = new Size(700, 520);
            Padding = 10;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            NetworkSend.SendCloseEditor();
            GameState.MyEditorType = EditorType.None;
        }

        private void InitializeComponent()
        {
            btnOpenScript.Click += (s, e) => OpenScript();
            btnSaveScript.Click += (s, e) => SaveScript();
            var buttons = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                Items = { btnOpenScript, btnSaveScript }
            };

            var layout = new DynamicLayout { Spacing = new Size(6,6) };
            layout.Add(lblInfo);
            layout.Add(buttons);
            layout.Add(txtPreview, yscale: true);

            Content = layout;
            Load += (s, e) => RefreshPreview();
        }

        private void OpenScript()
        {
            File.WriteAllLines(Script.TempFile, Core.Data.Script.Code);
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Script.TempFile,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Interaction.MsgBox($"Failed to open script: {ex.Message}");
            }
        }

        private void SaveScript()
        {
            if (!File.Exists(Script.TempFile))
            {
                Interaction.MsgBox("Open a script before saving.");
                return;
            }
            try
            {
                Core.Data.Script.Code = File.ReadAllLines(Script.TempFile);
                NetworkSend.SendSaveScript();
                RefreshPreview();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox($"Failed to save script: {ex.Message}");
            }
        }

        private void RefreshPreview()
        {
            txtPreview.Text = string.Join(Environment.NewLine, Core.Data.Script.Code ?? Array.Empty<string>());
        }
    }
}

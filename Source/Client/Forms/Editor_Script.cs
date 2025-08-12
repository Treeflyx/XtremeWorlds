using Eto.Forms;
using Eto.Drawing;
using Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Client.Net;
using Core.Globals;

namespace Client
{
    public class Editor_Script : Form
    {
        // Singleton access for legacy usage
        private static Editor_Script? _instance;
        public static Editor_Script Instance => _instance ??= new Editor_Script();

        public Button btnOpenScript = new Button { Text = "Open Script" };
        public Button btnSaveScript = new Button { Text = "Save Script" };
        public TextArea txtPreview = new TextArea { ReadOnly = true, Wrap = false, Size = new Size(600,400) };
        public Label lblInfo = new Label { Text = "Open the script in your external editor, then Save to reload and send." };

        public Editor_Script()
        {
            _instance = this;
            Title = "Script Editor";
            ClientSize = new Size(700, 520);
            Padding = 10;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (ReferenceEquals(_instance, this)) _instance = null;
            Sender.SendCloseEditor();
            GameState.MyEditorType = EditorType.None;
        }

        private void InitializeComponent()
        {
            // Ensure Load is subscribed first
            Load += (s, e) => RefreshPreview();

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
        }
        private void OpenScript()
        {
            try
            {
                var dir = Path.GetDirectoryName(Script.TempFile);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllLines(Script.TempFile, Data.Script.Code ?? Array.Empty<string>());

                if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS())
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = Script.TempFile,
                        UseShellExecute = true
                    });
                }
                else if (OperatingSystem.IsLinux())
                {
                    // Use xdg-open to launch the associated application on Linux
                    System.Diagnostics.Process.Start("xdg-open", Script.TempFile);
                }
                else
                {
                    Interaction.MsgBox("Unsupported platform for automatic script opening.");
                }
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
                Data.Script.Code = File.ReadAllLines(Script.TempFile);
                Sender.SendSaveScript();
                RefreshPreview();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox($"Failed to save script: {ex.Message}");
            }
        }

        private void RefreshPreview()
        {
            txtPreview.Text = string.Join(Environment.NewLine, Data.Script.Code ?? Array.Empty<string>());
        }
    }
}

using System.Drawing;
using System.Linq;
using System.Media;

namespace Universal_Font_Patcher_BDO;

public partial class Form1 : Form
{
    private static readonly Font checkboxFont = new Font("Calibri", 9F, FontStyle.Regular);

    public Form1()
    {
        InitializeComponent();
        using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound)) { audio.Play(); }
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        var paths = BdoPathDetector.DetectAll();

        if (paths.Count == 0)
        {
            lblDetectedPaths.Text = "No BDO installations found. Use Browse to select manually.";
            BtnContinue.Enabled = false;
            return;
        }

        foreach (var path in paths)
        {
            var cb = new CheckBox
            {
                Text = path,
                Tag = path,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = checkboxFont
            };
            cb.CheckedChanged += PathCheckbox_CheckedChanged;
            cb.Checked = true;
            flowPathsPanel.Controls.Add(cb);
        }

        TxtGamePath.Text = $"{paths.Count} installation(s) detected";
        PathCheckbox_CheckedChanged(sender, e);
    }

    private void PathCheckbox_CheckedChanged(object? sender, EventArgs? e)
    {
        bool anyChecked = flowPathsPanel.Controls
            .OfType<CheckBox>()
            .Any(cb => cb.Checked);

        bool fontSelected = !string.IsNullOrWhiteSpace(TxtFontPath.Text);

        BtnContinue.Enabled = anyChecked && fontSelected;
    }

    private void SiticoneCustomCheckBox6_CheckedChanged(object sender, EventArgs e)
    {
        using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound)) { audio.Play(); }
    }

    private void BtnContinue_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtFontPath.Text))
            return;

        var checkedPaths = flowPathsPanel.Controls
            .OfType<CheckBox>()
            .Where(cb => cb.Checked)
            .Select(cb => cb.Tag as string)
            .Where(path => path != null)
            .Cast<string>()
            .ToList();

        if (checkedPaths.Count == 0) return;

        int successCount = 0;
        int failCount = 0;
        var errors = new List<string>();

        foreach (string gamePath in checkedPaths)
        {
            try
            {
                string destDir = Path.Combine(gamePath, "prestringtable", "font");
                Directory.CreateDirectory(destDir);
                string destFile = Path.Combine(destDir, "pearl.ttf");
                File.Copy(TxtFontPath.Text, destFile, true);
                successCount++;
            }
            catch (Exception ex)
            {
                failCount++;
                errors.Add($"- {gamePath}: {ex.Message}");
            }
        }

        // Sound: play success only if ALL succeeded, error if any failed
        if (failCount == 0)
        {
            // Play original success sound
            using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.button_sound)) { audio.Play(); }
            MessageBox.Show($"Font patched successfully to {successCount} installation(s)!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            using (SoundPlayer error = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.error_sound)) { error.Play(); }
            string msg = $"Patched: {successCount} successful, {failCount} failed.\n\nErrors:\n{string.Join("\n", errors)}";
            MessageBox.Show(msg, "Patch Results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }



    private void BtnExit_Click(object sender, EventArgs e)
    {
        using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound)) { audio.Play(); }
        Environment.Exit(0);
    }

    private void BtnSelectGameFolder_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            // Check for the BDO launcher executable to validate the path
            string launcherPath = Path.Combine(fbd.SelectedPath, "BlackDesertLauncher.exe");
            bool hasLauncher = File.Exists(launcherPath);

            if (fbd.SelectedPath.Contains("BlackDesert") && hasLauncher)
            {
                using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound)) { audio.Play(); }

                // After validation, add browsed path as a new checkbox
                bool alreadyExists = flowPathsPanel.Controls
                    .OfType<CheckBox>()
                    .Any(cb => string.Equals(cb.Tag as string, fbd.SelectedPath, StringComparison.OrdinalIgnoreCase));

                if (!alreadyExists)
                {
                    var cb = new CheckBox
                    {
                        Text = $"Manual: {fbd.SelectedPath}",
                        Tag = fbd.SelectedPath,
                        ForeColor = Color.White,
                        BackColor = Color.Transparent,
                        Font = checkboxFont
                    };
                    cb.CheckedChanged += PathCheckbox_CheckedChanged;
                    cb.Checked = true;
                    flowPathsPanel.Controls.Add(cb);

                    // Update summary
                    int count = flowPathsPanel.Controls.OfType<CheckBox>().Count();
                    TxtGamePath.Text = $"{count} installation(s)";
                }
                else
                {
                    // Check the existing one
                    foreach (CheckBox cb in flowPathsPanel.Controls.OfType<CheckBox>())
                    {
                        if (string.Equals(cb.Tag as string, fbd.SelectedPath, StringComparison.OrdinalIgnoreCase))
                        {
                            cb.Checked = true;
                            break;
                        }
                    }
                }

                PathCheckbox_CheckedChanged(sender, e);
            }
            else
            {
                MessageBox.Show("The selected folder doesn't seem to be the proper one, could you double check please?", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
    }

    private void BtnSelectFont_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog1 = new()
        {
            InitialDirectory = @"C:\",
            Title = "Select the font you want to use",

            CheckFileExists = true,
            CheckPathExists = true,

            DefaultExt = "ttf",
            Filter = "TTF files (*.ttf)|*.ttf",
            FilterIndex = 2,
            RestoreDirectory = true,

            ReadOnlyChecked = true,
            ShowReadOnly = true
        };

        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            // Validate the font file before accepting
            try
            {
                using var testFont = new Font(openFileDialog1.FileName, 12);
                TxtFontPath.Text = openFileDialog1.FileName;
            }
            catch (Exception)
            {
                MessageBox.Show("The selected file is not a valid TrueType font.", "Invalid Font", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtFontPath.Text = "";
                return;
            }

            using (SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound)) { audio.Play(); }
        }
    }
}

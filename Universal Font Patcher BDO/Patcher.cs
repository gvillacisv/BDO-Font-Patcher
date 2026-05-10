using System.Drawing;
using System.Media;

namespace Universal_Font_Patcher_BDO;

public partial class Form1 : Form
{




        public Form1()
        {
            InitializeComponent();
            SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
            audio.Play();
        }

        private void siticoneCustomCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
            audio.Play();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
          
          

            string sourceFile = TxtFontPath.Text;
            string destinationFile = TxtGamePath.Text +"\\prestringtable\\font\\pearl.ttf";
            string dir = TxtGamePath.Text + "\\prestringtable\\font\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            try
            {
                File.Copy(sourceFile, destinationFile,true);
                SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.button_sound);
                audio.Play();
            }
            catch (IOException iox)
            {
                SoundPlayer error = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.error_sound);
                error.Play();
                MessageBox.Show(iox.Message);
            }

           
            
           
                
            
        }

       

        private void BtnExit_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
            audio.Play();
            Environment.Exit(0);
        }

        private void BtnSelectGameFolder_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
            audio.Play();
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    if (fbd.SelectedPath.Contains("BlackDesert"))
                    {
                        TxtGamePath.Text = fbd.SelectedPath;

                        audio.Play();

                    }
                    else
                    {
                        MessageBox.Show("The selected folder doesn't seem to be the proper one, could you double check please?","Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }

                }

            }
        }

        private void BtnSelectFont_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
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
                TxtFontPath.Text = openFileDialog1.FileName;
            }

            SoundPlayer audio = new SoundPlayer(Universal_Font_Patcher_BDO.Properties.Resources.checkbox_sound);
            audio.Play();
        }
    }

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace App
{
    public partial class Form1 : Form
    {
        private List<string> videoPaths = new List<string>();
        private string playlistFile = "playlist.txt";


        public Form1()
        {
            InitializeComponent();
            LoadPlaylist();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnPlayPause.Enabled = false;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv|All Files|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        videoPaths.Add(file);
                        listBoxPlaylist.Items.Add(Path.GetFileName(file));
                    }

                    SavePlaylist();
                }
            }
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                btnPlayPause.Text = "Play";
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                btnPlayPause.Text = "Pause";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex >= 0)
            {

                int selectedIndex = listBoxPlaylist.SelectedIndex;
                listBoxPlaylist.Items.RemoveAt(selectedIndex);
                videoPaths.RemoveAt(selectedIndex);
                SavePlaylist();
            }
            else
            {
                MessageBox.Show("Please select a video to delete");
            }
        }
        private void SavePlaylist()
        {
            try
            {
                File.WriteAllLines(playlistFile, videoPaths);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when saving list: {ex.Message}");
            }
        }

        private void LoadPlaylist()
        {
            if (File.Exists(playlistFile))
            {
                try
                {

                    videoPaths = new List<string>(File.ReadAllLines(playlistFile));
                    foreach (var video in videoPaths)
                    {
                        listBoxPlaylist.Items.Add(Path.GetFileName(video));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error when saving list: {ex.Message}");
                }
            }

        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex >= 0)
            {
                string selectedVideoPath = videoPaths[listBoxPlaylist.SelectedIndex];
                axWindowsMediaPlayer1.URL = selectedVideoPath;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                btnPlayPause.Text = "Pause";
                btnPlayPause.Enabled = true;
            }
        }



    }
}


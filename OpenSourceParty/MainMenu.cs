﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenSourceParty
{
    public partial class MainMenu : Form
    {
        FileManager fileMan;
        GamepadManager padMan;

        

        public MainMenu()
        {
            fileMan = new FileManager();   // Instantiate a new file manager.
            padMan = new GamepadManager();
            this.BackgroundImage = Image.FromFile(fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension));   // Set the background image.
            this.BackgroundImageLayout = ImageLayout.Stretch;
            InitializeComponent();
            padMan.Init();
            padMan[0].aDelagate = new GamepadState.GamepadDelegate(button1_Click);
            padMan[0].bDelagate = new GamepadState.GamepadDelegate(button2_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileMan.RandomGame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Games:");
            fileMan.printFileList(fileMan.MinigameDir, fileMan.GameExtension);
            Console.WriteLine("Backgrounds:");
            fileMan.printFileList(fileMan.BackgroundDir, fileMan.ImageExtension);
        }
    }
}
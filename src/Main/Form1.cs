using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USC.GISResearchLab.Common.Utils.FileBrowsers;
using System.IO;
using USC.GISResearchLab.Common.Utils.Files;

namespace BuildTools.VersionTester
{
    public enum MachineType
    {
        Native = 0, I386 = 0x014c, Itanium = 0x0200, x64 = 0x8664
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = FileBrowserUtils.BrowseForFolder(txtDirectory.Text);

            if (!String.IsNullOrEmpty(folder))
            {
                txtDirectory.Text = folder;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string file in Directory.EnumerateFiles(txtDirectory.Text, "*.dll", SearchOption.TopDirectoryOnly))
            {
                MachineType m = GetMachineType(file);
                txtOutput.Text += file + ": ";
                txtOutput.Text += m.ToString();
                txtOutput.Text += Environment.NewLine;
            }
        }
       

        public static MachineType GetMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60;
            const int MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];
            using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                s.Read(data, 0, 4096);
            }
            // dos header is 64 bytes, last element, long (4 bytes) is the address of the PE header
            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return (MachineType)machineUint;
        }

       
    }
}

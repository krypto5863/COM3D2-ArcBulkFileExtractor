using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcOutfitExtractorTool
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void ConvertButton_click(object sender, EventArgs e)
		{
			var fileOpen = new System.Windows.Forms.OpenFileDialog();

			fileOpen.Filter = "Arc File (*.arc)|*.arc";

			fileOpen.Multiselect = true;

			fileOpen.ShowDialog();

			if (!String.IsNullOrEmpty(fileOpen.FileName)) 
			{
				Program.ExtractFromArcs(fileOpen.FileNames);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
		private void DirectoryConvertButton_Click(object sender, EventArgs e)
		{
			var fileOpen = new System.Windows.Forms.FolderBrowserDialog();

			fileOpen.ShowDialog();

			if (!String.IsNullOrEmpty(fileOpen.SelectedPath))
			{
				Program.ExtractFromDirectory(fileOpen.SelectedPath);
			}
		}
	}
}

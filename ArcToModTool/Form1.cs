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
            OpenFileDialog fileOpen = new System.Windows.Forms.OpenFileDialog();

			fileOpen.Filter = "Arc File (*.arc)|*.arc";

			fileOpen.Multiselect = true;

			fileOpen.ShowDialog();

			if (!String.IsNullOrEmpty(fileOpen.FileName)) 
			{
				Program.ExtractFilesFromFiles(fileOpen.FileNames);
			}
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

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;

			//출처: https://nantano1.tistory.com/entry/C-윈폼에-파일을-드래그-앤-드랍-Drag-Drop-File [일상의 기록들]
		}

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			
			Program.ExtractFilesFromFiles(files);

			//foreach (string file in files)
			//{
			//	//Console.WriteLine(file);
			//}



			//출처: https://nantano1.tistory.com/entry/C-윈폼에-파일을-드래그-앤-드랍-Drag-Drop-File [일상의 기록들]

		}
    }
}

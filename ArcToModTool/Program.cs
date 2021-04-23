using CM3D2.Toolkit.Arc;
using CM3D2.Toolkit.Arc.Entry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcOutfitExtractorTool
{
	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		private static List<String> Extensions = new List<string>() { ".menu", ".tex", ".model", ".mate", ".pmat", ".col", ".psk" };

		public static void ExtractFromArcs(string[] arcFiles) 
		{
			ArcFileSystem fileSystem = new ArcFileSystem();

			foreach (string f in arcFiles) 
			{
				fileSystem.LoadArc(f);
			}

			ExtractFilesFromFileSystem(fileSystem);
		}

		public static void ExtractFromDirectory(string dir)
		{
			string[] files = Directory.GetFiles(dir, "*.arc", SearchOption.AllDirectories);

			ExtractFromArcs(files);
		}

		private static void ExtractFilesFromFileSystem(ArcFileSystem fileSystem) 
		{

			if (fileSystem.Files.Count() == 0) 
			{
				return;
			}

			var filesToExport = fileSystem.Files.Where(x => Extensions.Any(t => x.Name.Contains(t))).ToList();

			if (filesToExport.Count == 0)
			{
				MessageBox.Show("No files to export...");
				return;
			}

			using (var fbd = new FolderBrowserDialog())
			{

				if (filesToExport.Count == 0)
				{
					return;
				}

				MessageBox.Show("We found files to export! Select a destination directory now for files to be placed into.");

				fbd.ShowDialog();

				if (!String.IsNullOrEmpty(fbd.SelectedPath))
				{
					foreach (ArcFileEntry f in filesToExport)
					{
						var decompressed = f.Pointer.Decompress();

						File.WriteAllBytesAsync(fbd.SelectedPath + "\\" + f.Name, decompressed.Data);
					}

					MessageBox.Show($"Pulled {filesToExport.Count} files.");
				}
			}
		}
	}
}

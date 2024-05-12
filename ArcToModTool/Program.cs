using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using CM3D2.Toolkit.Guest4168Branch.Arc;

namespace ArcToModTool
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			MediaTypeNames.Application.SetHighDpiMode(HighDpiMode.SystemAware);
			MediaTypeNames.Application.EnableVisualStyles();
			MediaTypeNames.Application.SetCompatibleTextRenderingDefault(false);
			MediaTypeNames.Application.Run(new Form1());
		}

		private static readonly List<string> Extensions = new() { ".menu", ".tex", ".model", ".mate", ".pmat", ".col", ".psk", ".anm" };

		private static void ExtractFromArcs(IEnumerable<string> arcFiles)
		{
			var fileSystem = new ArcFileSystem();

			foreach (var f in arcFiles)
			{
				fileSystem.LoadArc(f);
			}

			ExtractFilesFromFileSystem(fileSystem);
		}

		private static void ExtractFilesFromFileSystem(ArcFileSystem fileSystem)
		{

			if (!fileSystem.Files.Any())
			{
				return;
			}

			var filesToExport = fileSystem.Files
				.Where(x => Extensions.Any(t => x.Value.Name.Contains(t)))
				.ToList();

			if (filesToExport.Count == 0)
			{
				MessageBox.Show(@"No files to export...");
				return;
			}

			using var fbd = new FolderBrowserDialog();
			if (filesToExport.Count == 0)
			{
				return;
			}

			MessageBox.Show(@"We found files to export! Select a destination directory now for files to be placed into.");

			fbd.ShowDialog();

			if (string.IsNullOrEmpty(fbd.SelectedPath))
			{
				return;
			}

			foreach (var f in filesToExport)
			{
				if (f.Value.Name.Contains(".anm") && !f.Value.FullName.Contains("dress"))
				{
					continue;
				}

				var decompressed = f.Value.Pointer.Decompress();
				var folderName = RemoveInvalidChars(f.Value.Parent.Name);
				var fileName = RemoveInvalidChars(f.Value.Name);

				var subDirectory = Path.Combine(fbd.SelectedPath, folderName);
				var fullDirectory = Path.Combine(subDirectory, fileName);

				if (!Directory.Exists(subDirectory))
				{
					Directory.CreateDirectory(subDirectory);
				}

				File.WriteAllBytesAsync(fullDirectory, decompressed.Data);
			}
			MessageBox.Show($@"Pulled {filesToExport.Count} files.");
		}
		private static string RemoveInvalidChars(string filename)
		{
			return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
		}
	}
}

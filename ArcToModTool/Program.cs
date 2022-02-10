using CM3D2.Toolkit.Guest4168Branch.Arc;
using CM3D2.Toolkit.Guest4168Branch.Arc.Entry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public static void ExtractFromDirectory(string dir)
		{
			string[] files = Directory.GetFiles(dir, "*.arc", SearchOption.AllDirectories);

			ExtractFilesFromFiles(files);
		}

		public static void ExtractFilesFromFiles(string[] files)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.ShowDialog();

				if (!String.IsNullOrEmpty(fbd.SelectedPath))
                {
					int cnt = 0;
					foreach (string fl in files)
					{
						ArcFileSystem fileSystem = new ArcFileSystem();
						fileSystem.LoadArc(fl);

						foreach (ArcFileEntry f in fileSystem.Files.Values)
						{

							var p = f.Parent.FullName.Replace("CM3D2ToolKit:", String.Empty);

							//Debug.WriteLine($"{f.Name} , {f.FullName}");
							//Debug.WriteLine($"{p} ,{f.Parent.FullName} ");
							//Debug.WriteLine($"{f.FileSystem.Name} , {f.FileSystem.Root.Name}");

							if (!Directory.Exists(fbd.SelectedPath + p))
							{
								Directory.CreateDirectory(fbd.SelectedPath + p);
							}

							var decompressed = f.Pointer.Decompress();

							File.WriteAllBytesAsync(fbd.SelectedPath + p + "\\" + f.Name, decompressed.Data);
						}

						cnt += fileSystem.Files.Count;
						
					}

					MessageBox.Show($"Pulled {cnt} files.");
                }
			}
		}
	}
}

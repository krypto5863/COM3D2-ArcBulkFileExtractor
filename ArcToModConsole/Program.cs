using CM3D2.Toolkit.Guest4168Branch.Arc;
using Serilog;
using System.CommandLine;

namespace ArcBulkFileExtractor
{
	internal class Program
	{
		private static readonly string[] ModFilesWhitelist = { ".menu", ".tex", ".model", ".mate", ".pmat", ".col", ".psk" };
		private static string[]? _whitelist;

		private static async Task<int> Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();

			var inputOption = new Option<FileSystemInfo[]>(
				aliases: new[] { "--targets", "-t" },
				description: "The files and/or directories from which to extract.")
			{
				IsRequired = true,
				AllowMultipleArgumentsPerToken = true
			};

			var outputOption = new Option<DirectoryInfo>(
				aliases: new[] { "--output", "-o" },
				description: "The directory where files will be placed.")
			{ IsRequired = true };

			var customExtensionsWhitelist = new Option<string[]>(
				aliases: new[] { "--extensions", "-e" },
				getDefaultValue: () => ModFilesWhitelist,
				description:
				"The file extensions to extract from the arc files." +
				"\nIncluding .csv will convert .nei to .csv format." +
				"\nUsage example: -e .anm .tex .mate")
			{
				AllowMultipleArgumentsPerToken = true
			};

			// Create the root command
			var rootCommand = new RootCommand("A simple tool that allows you to extract specific types of files from arc file/s in bulk.")
			{
				inputOption,
				outputOption,
				customExtensionsWhitelist,
			};

			rootCommand.SetHandler(async (input, output, filters) =>
			{
				_whitelist = filters;
				await ProcessInputPaths(input, output);
			}, inputOption, outputOption, customExtensionsWhitelist);

			var result = await rootCommand.InvokeAsync(args);

			if (args.Length <= 0)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}

			return result;
		}

		private static async Task ProcessInputPaths(IEnumerable<FileSystemInfo> paths, DirectoryInfo output)
		{
			paths = paths.OrderBy(m => m.FullName);

			var arcsToUnpack = new List<FileInfo>();
			var directoriesToUnpack = new List<DirectoryInfo>();

			foreach (var path in paths)
			{
				if (path.Exists == false)
				{
					Log.Error($"{path} does not exist.");
					continue;
				}

				switch (path)
				{
					case DirectoryInfo directoryInfo:
						directoriesToUnpack.Add(directoryInfo);
						continue;
					case FileInfo fileInfo:
						arcsToUnpack.Add(fileInfo);
						continue;
				}
			}

			if (directoriesToUnpack.Any())
			{
				await ExtractFromDirectory(directoriesToUnpack, output);
			}

			if (arcsToUnpack.Any())
			{
				await ExtractFromArcs(arcsToUnpack, output);
			}

			Log.Information("Done!");
		}

		private static async Task ExtractFromDirectory(IEnumerable<DirectoryInfo> directories, DirectoryInfo outputDirectoryInfo)
		{
			foreach (var directory in directories)
			{
				Log.Information($"Processing Directory: {directory.Name}...");

				var arcFiles = directory.EnumerateFiles("*.arc", SearchOption.AllDirectories)
					.ToArray();

				Log.Information($"{arcFiles.Length} arc files found.");
				await ExtractFromArcs(arcFiles, outputDirectoryInfo);
			}
		}

		private static async Task ExtractFromArcs(IEnumerable<FileInfo> arcFiles, DirectoryInfo outputDirectoryInfo)
		{
			var fileSystem = new ArcFileSystem();
			var counter = 0;
			var fileInfos = arcFiles as FileInfo[] ?? arcFiles.ToArray();
			foreach (var arc in fileInfos)
			{
				Log.Information($"Loading {++counter}/{fileInfos.Length}: {arc.Name}...");
				fileSystem.LoadArc(arc.FullName);
			}
			Log.Information("Beginning extraction...");
			await ExtractFilesFromFileSystem(fileSystem, outputDirectoryInfo);
		}

		private static async Task ExtractFilesFromFileSystem(ArcFileSystem fileSystem, DirectoryInfo outputFolder)
		{
			if (!fileSystem.Files.Any())
			{
				return;
			}

			var filesToExport = fileSystem.Files
				.Where(x => _whitelist.Any(t => x.Value.Name.EndsWith(t, StringComparison.OrdinalIgnoreCase) 
				                                || x.Value.Name.EndsWith(".nei") && _whitelist.Any(r => r.Equals(".csv", StringComparison.OrdinalIgnoreCase))))
				.ToList();

			if (filesToExport.Count == 0)
			{
				return;
			}

			var counter = 0;

			foreach (var f in filesToExport)
			{
				var decompressed = f.Value.Pointer.Decompress();
				var folderName = RemoveInvalidChars(f.Value.Parent.Name);
				var fileName = RemoveInvalidChars(f.Value.Name);

				var subDirectory = Path.Combine(outputFolder.FullName, folderName);
				var fullDirectory = Path.Combine(subDirectory, fileName);

				if (!Directory.Exists(subDirectory))
				{
					Directory.CreateDirectory(subDirectory);
				}

				if (fileName.EndsWith(".nei"))
				{
					if (_whitelist.Any(m => m.Equals(".nei", StringComparison.OrdinalIgnoreCase)))
					{
						await File.WriteAllBytesAsync(fullDirectory, decompressed.Data);
					}
					if (_whitelist.Any(m => m.Equals(".csv", StringComparison.OrdinalIgnoreCase)))
					{
						var csvFullPath = Path.Combine(subDirectory, Path.GetFileNameWithoutExtension(fileName) + ".csv");

						var csvColumnRows = NeiLib.NeiConverter.ToCSVList(decompressed.Data);
						var csvRows = csvColumnRows.Select(r => string.Join(",", r));
						await File.WriteAllLinesAsync(csvFullPath, csvRows);
					}
				}
				else
				{
					await File.WriteAllBytesAsync(fullDirectory, decompressed.Data);
				}

				Log.Information($"{++counter}/{filesToExport.Count}: {fileName}");
			}
		}

		private static string RemoveInvalidChars(string filename)
		{
			return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
		}
	}
}
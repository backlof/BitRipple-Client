using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChooseLocation
{
	 internal class Program
	 {
		  [STAThread]
		  private static void Main(string[] args)
		  {
				Execute(ChooseFolder);
		  }

		  private static void Execute(Action<JsonDataStorage> method)
		  {
				method(new JsonDataStorage());
		  }

		  private static void ChooseFolder(JsonDataStorage storage)
		  {
				try
				{
					 using (var folderBrowserDialog = new FolderBrowserDialog())
					 {
						  folderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();

						  if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						  {
								storage.LoadSettings();
								storage.Settings.Location = folderBrowserDialog.SelectedPath;

								JsonDataStorage.WriteFile(JsonDataStorage.SettingsPath, storage.Settings);
						  }
					 }
				}
				catch (Exception)
				{
				}
		  }
	 }
}

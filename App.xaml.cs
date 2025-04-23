using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;

namespace LocationTracker
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			// Set up the database service (use a file path from the local device storage)
			string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
			DatabaseService databaseService = new DatabaseService(dbPath);

			// Set MainPage with the database service passed to MainPage
			MainPage = new MainPage();
		}

	}
}

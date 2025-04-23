// using Microsoft.Maui.Controls;
// using Microsoft.Maui.Controls.Maps; // for Map and Position
// using Microsoft.Maui.Devices.Sensors; // for Geolocation
// using Microsoft.Maui.Maps;
// using System;
// using System.Collections.Generic;
// using SQLite;
// using System.IO;
// using System.Linq;

// namespace LocationTracker
// {
//     public partial class MainPage : ContentPage
//     {
//         private readonly DatabaseService _databaseService;
//         private readonly string _dbPath;

//         public MainPage()
//         {
//             InitializeComponent();
//             _dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db");
//             _databaseService = new DatabaseService(_dbPath);

//             AddSampleLocations();
//             DisplayLocationsAsHeatmap();
//         }


//         private async void OnTrackLocationClicked(object sender, EventArgs e)
//         {
//             var location = await Geolocation.GetLocationAsync(new GeolocationRequest
//             {
//                 DesiredAccuracy = GeolocationAccuracy.Best,
//                 Timeout = TimeSpan.FromSeconds(10)
//             });

//             if (location != null)
//             {
//                 var locationData = new LocationData
//                 {
//                     Latitude = location.Latitude,
//                     Longitude = location.Longitude,
//                     Timestamp = DateTime.Now
//                 };

//                 _databaseService.SaveLocation(locationData);
//                 DisplayLocationsAsHeatmap();
//             }
//             else
//             {
//                 await DisplayAlert("Error", "Unable to retrieve location.", "OK");
//             }
//         }

//         private void DisplayLocationsAsHeatmap()
//         {
//             var locations = _databaseService.GetLocations();
//             Console.WriteLine($"Retrieved {locations.Count} locations from the database.");

//             if (locations.Count == 0)
//             {
//                 Console.WriteLine("No locations to display on the heatmap.");
//                 return;
//             }

//             locationMap.Pins.Clear();
//             locationMap.MapElements.Clear();

//             foreach (var location in locations)
//             {
//                 var intensity = GetLocationIntensity(location, locations);
//                 Console.WriteLine($"Rendering location: {location.Latitude}, {location.Longitude} with intensity {intensity}");

//                 var heatCircle = new Circle
//                 {
//                     Center = new Location(location.Latitude, location.Longitude),
//                     Radius = Distance.FromMeters(30),
//                     StrokeColor = Color.FromRgba(255, 0, 0, intensity),
//                     FillColor = Color.FromRgba(255, 0, 0, intensity)
//                 };

//                 locationMap.MapElements.Add(heatCircle);
//             }
//         }


//         private double GetLocationIntensity(LocationData location, List<LocationData> allLocations)
//         {
//             double intensity = 0;

//             foreach (var loc in allLocations)
//             {
//                 var distance = Math.Sqrt(Math.Pow(loc.Latitude - location.Latitude, 2) + Math.Pow(loc.Longitude - location.Longitude, 2));
//                 if (distance < 0.01) // Adjust proximity threshold as needed
//                 {
//                     intensity += 0.1; // Increase intensity based on proximity
//                 }
//             }

//             return Math.Min(1, intensity); // Cap intensity to 1 for valid RGBA values
//         }

//         private async void OnShowCurrentLocationClicked(object sender, EventArgs e)
//         {
//             var location = await Geolocation.GetLocationAsync(new GeolocationRequest
//             {
//                 DesiredAccuracy = GeolocationAccuracy.Best,
//                 Timeout = TimeSpan.FromSeconds(10)
//             });

//             if (location != null)
//             {
//                 locationMap.MoveToRegion(MapSpan.FromCenterAndRadius(
//                     new Location(location.Latitude, location.Longitude),
//                     Distance.FromMiles(1)));

//                 locationMap.Pins.Clear();
//                 locationMap.Pins.Add(new Pin
//                 {
//                     Label = "You are here",
//                     Location = new Location(location.Latitude, location.Longitude)
//                 });
//             }
//             else
//             {
//                 await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
//             }
//         }

//         private void AddSampleLocations()
//         {
//             var sampleLocations = new List<LocationData>
//             {
//                 new LocationData { Latitude = 37.7749, Longitude = -122.4194, Timestamp = DateTime.Now }, // San Francisco
//                 new LocationData { Latitude = 37.7740, Longitude = -122.4190, Timestamp = DateTime.Now },
//                 new LocationData { Latitude = 37.7750, Longitude = -122.4200, Timestamp = DateTime.Now },
//                 new LocationData { Latitude = 37.7745, Longitude = -122.4195, Timestamp = DateTime.Now },
//                 new LocationData { Latitude = 37.7742, Longitude = -122.4192, Timestamp = DateTime.Now }
//             };

//             foreach (var location in sampleLocations)
//             {
//                 _databaseService.SaveLocation(location);
//                 Console.WriteLine($"Saved Location: {location.Latitude}, {location.Longitude}");
//             }
//         }

//     }

//     public class DatabaseService
//     {
//         private readonly SQLiteConnection _database;

//         public DatabaseService(string databasePath)
//         {
//             _database = new SQLiteConnection(databasePath);
//             _database.CreateTable<LocationData>();
//         }

//         public void SaveLocation(LocationData location)
//         {
//             _database.Insert(location);
//         }

//         public List<LocationData> GetLocations()
//         {
//             return _database.Table<LocationData>().ToList();
//         }
//     }

//     public class LocationData
//     {
//         [PrimaryKey, AutoIncrement]
//         public int Id { get; set; }
//         public double Latitude { get; set; }
//         public double Longitude { get; set; }
//         public DateTime Timestamp { get; set; }
//     }
// }


using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps; // for Map and Position
using Microsoft.Maui.Devices.Sensors; // for Geolocation
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Linq;

namespace LocationTracker
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly string _dbPath;

        public MainPage()
        {
            InitializeComponent();
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db");
            _databaseService = new DatabaseService(_dbPath);

            // Add some sample locations for heatmap visualization
            AddSampleLocations();
        }

        private async void OnTrackLocationClicked(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                var locationData = new LocationData
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Timestamp = DateTime.Now
                };

                _databaseService.SaveLocation(locationData);
                DisplayLocationsAsHeatmap();
            }
            else
            {
                await DisplayAlert("Error", "Unable to retrieve location.", "OK");
            }
        }

        private void DisplayLocationsAsHeatmap()
        {
            var locations = _databaseService.GetLocations();

            // Clear the existing pins or overlays
            locationMap.Pins.Clear();
            locationMap.MapElements.Clear();

            foreach (var location in locations)
            {
                var intensity = GetLocationIntensity(location, locations);

                // Use a Circle to represent heatmap points
                var heatCircle = new Circle
                {
                    Center = new Location(location.Latitude, location.Longitude),
                    Radius = Distance.FromMeters(35), // Adjust radius as needed
                    StrokeColor = Color.FromRgba(255, 0, 0, intensity), // Red with intensity
                    FillColor = Color.FromRgba(255, 0, 0, intensity) // Red with intensity
                };

                // Add the circle to the map
                locationMap.MapElements.Add(heatCircle);
            }
        }

        private double GetLocationIntensity(LocationData location, List<LocationData> allLocations)
        {
            double intensity = 0;

            foreach (var loc in allLocations)
            {
                var distance = Math.Sqrt(Math.Pow(loc.Latitude - location.Latitude, 2) + Math.Pow(loc.Longitude - location.Longitude, 2));
                if (distance < 0.01) // Adjust proximity threshold as needed
                {
                    intensity += 0.1; // Increase intensity based on proximity
                }
            }

            return Math.Min(1, intensity); // Cap intensity to 1 for valid RGBA values
        }

        private async void OnShowCurrentLocationClicked(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                locationMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(location.Latitude, location.Longitude),
                    Distance.FromMiles(1)));

                locationMap.Pins.Clear();
                locationMap.Pins.Add(new Pin
                {
                    Label = "You are here",
                    Location = new Location(location.Latitude, location.Longitude)
                });
            }
            else
            {
                await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
            }
        }

        private void AddSampleLocations()
        {
            var sampleLocations = new List<LocationData>
            {
                new LocationData { Latitude = 37.7749, Longitude = -122.4194, Timestamp = DateTime.Now }, // San Francisco
                new LocationData { Latitude = 37.7740, Longitude = -122.4190, Timestamp = DateTime.Now },
                new LocationData { Latitude = 37.7750, Longitude = -122.4200, Timestamp = DateTime.Now },
                new LocationData { Latitude = 37.7745, Longitude = -122.4195, Timestamp = DateTime.Now },
                new LocationData { Latitude = 37.7742, Longitude = -122.4192, Timestamp = DateTime.Now }
            };

            foreach (var location in sampleLocations)
            {
                _databaseService.SaveLocation(location);
            }
        }

        private async void OnShowSavedLocationsClicked(object sender, EventArgs e)
        {
            var locations = _databaseService.GetLocations();
            if (locations.Any())
            {
                string locationList = string.Join("\n", locations.Select(l => $"Lat: {l.Latitude}, Lng: {l.Longitude}, Time: {l.Timestamp}"));
                await DisplayAlert("Saved Locations", locationList, "OK");
            }
            else
            {
                await DisplayAlert("No Locations", "No saved locations found.", "OK");
            }
        }
    }

    public class DatabaseService
    {
        private readonly SQLiteConnection _database;

        public DatabaseService(string databasePath)
        {
            _database = new SQLiteConnection(databasePath);
            _database.CreateTable<LocationData>();
        }

        public void SaveLocation(LocationData location)
        {
            _database.Insert(location);
        }

        public List<LocationData> GetLocations()
        {
            return _database.Table<LocationData>().ToList();
        }
    }

    public class LocationData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

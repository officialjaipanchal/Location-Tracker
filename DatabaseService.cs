// using SQLite;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;

// namespace LocationTracker
// {
//     public class DatabaseService
//     {
//         private readonly SQLiteConnection _database;

//         public DatabaseService(string dbPath)
//         {
//             _database = new SQLiteConnection(dbPath);
//             _database.CreateTable<LocationData>();
//         }

//         public DatabaseService()
//         {
//         }

//         // Save location data to the database
//         public void SaveLocation(LocationData location)
//         {
//             _database.Insert(location);
//         }

//         // Get all saved location data
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

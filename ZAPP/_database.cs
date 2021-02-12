using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using Environment = System.Environment;
using Mono.Data.Sqlite;

namespace ZAPP
{
    class _database
    {
        //Context definieren
        private Context context;
        private string baseUrl = "http://192.168.1.111/cockpit-master/api/collections/get/{0}?token=eb84b98553c992cd134e1f63c43051";
        private string tasksUrl = "http://192.168.1.111/cockpit-master/api/collections/get/tasks?token=eb84b98553c992cd134e1f63c43051";
        private string usersUrl = "http://192.168.1.111/cockpit-master/api/collections/get/users?token=eb84b98553c992cd134e1f63c43051";
        private string visitsUrl = "http://192.168.1.111/cockpit-master/api/collections/get/visits?token=eb84b98553c992cd134e1f63c43051";
        private string clientsUrl = "http://192.168.1.111/cockpit-master/api/collections/get/clients?token=eb84b98553c992cd134e1f63c43051";
        private string[] collections = { "tasks", "users", "visits", "clients" };

        //Constructor
        public _database(Context context)
        {
            this.context = context;
            this.createDatabase();
            foreach(string collection in collections)
            {
                this.downloadData(String.Format(baseUrl, collection));
            }
            this.getAllData();
        }

        //Database maken
        public void createDatabase()
        {
            string pathToDatabase = getDatabasePath();

            if (!File.Exists(pathToDatabase))
            {
                SqliteConnection.CreateFile(pathToDatabase);

                string connectionString = String.Format("Data Source = {0};", pathToDatabase);
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        // Table data
                        cmd.CommandText = "CREATE TABLE users(id INTEGER PRIMARY KEY AUTOINCREMENT, _id text, username text, password text)";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "CREATE TABLE tasks(id INTEGER PRIMARY KEY AUTOINCREMENT, _id text, bezoek_id text, titel text, omschrijving text, voltooid integer)";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "CREATE TABLE visits(id INTEGER PRIMARY KEY AUTOINCREMENT, _id text, client_id text, datum text, tijd text, aanwezig integer)";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "CREATE TABLE clients(id INTEGER PRIMARY KEY AUTOINCREMENT, _id text, naam text, adres text, postcode text, woonplaats text, telefoonnummer text)";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                    }
                    conn.Close();
                }
            }
        }

        public ArrayList getAllData()
        {
            //Create a new ArrayList
            ArrayList dataList = new ArrayList();

            return dataList;
        }

        public void downloadData(string url)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            try
            {
                byte[] myDataBuffer = webClient.DownloadData(url);
                string download = Encoding.ASCII.GetString(myDataBuffer);
                JsonValue value = JsonValue.Parse(download);
                foreach (JsonObject result in value["entries"])
                {
                    if (url == clientsUrl)
                    {
                        clientRecord record = new clientRecord(result);
                        record.insertDataRecord(this);
                    }
                    else if (url == usersUrl)
                    {
                        userRecord record = new userRecord(result);
                        record.insertDataRecord(this);
                    }
                    else if (url == visitsUrl)
                    {
                        visitRecord record = new visitRecord(result);
                        record.insertDataRecord(this);
                    }
                    else if (url == tasksUrl)
                    {
                        taskRecord record = new taskRecord(result);
                        record.insertDataRecord(this);
                    }

                }
            }
            catch (WebException)
            {
                // Doe vooralsnog niks, straks wellicht een boolean terug
                // geven of e.e.a. gelukt is of niet
                Console.WriteLine("Download failed");
            }
        }

        public string getDatabasePath()
        {
            Resources res = this.context.Resources;
            //string insertTableData = res.GetString(Resource.String.insertTableData, record.code, record.description);
            string app_name = res.GetString(Resource.String.app_name);
            string app_version = res.GetString(Resource.String.app_version);

            string dbname = "_db_" + app_name + "_" + app_version + ".sqlite";
            //Console.WriteLine(dbname);
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string pathToDatabase = Path.Combine(documentsPath, dbname);

            return pathToDatabase;
        }

        public void insertDataRecord(string insertString)
        {
            string databasePath = getDatabasePath();
            string connectionString = String.Format("Data Source = {0};", databasePath);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Table data
                    cmd.CommandText = insertString;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("INSERT QUERY EXECUTED");
                }
                conn.Close();
            }
        }

        public class dataRecord
        {
            public int id;
            public string _id;

            public dataRecord(JsonValue record)
            {
                _id = record["_id"];
            }

            public dataRecord(SqliteDataReader record)
            {
                id = (int)(Int64)record["id"];
                _id = (string)record["_id"];
            }

        }

        public class visitRecord : dataRecord
        {
            private string client_id;
            private string datum;
            private string tijd;
            private int aanwezig = 0;

            public visitRecord(JsonValue record) : base(record)
            {
                client_id = (string)record["client_id"]["_id"];
                datum = (string)record["datum"];
                tijd = (string)record["tijd"];
                aanwezig = (int)record["aanwezig"];
            }

            public visitRecord(SqliteDataReader record) : base(record)
            {
                client_id = (string)record["client_id"];
                datum = (string)record["datum"];
                tijd = (string)record["tijd"];
                aanwezig = (int)record["aanwezig"];
            }

            public void insertDataRecord(_database database)
            {
                string insertString = String.Format("INSERT INTO visits (_id, client_id, datum, tijd, aanwezig) values ('{0}', '{1}', '{2}', '{3}', {4})", _id, client_id, datum, tijd, aanwezig);
                database.insertDataRecord(insertString);
            }
        }
        public class userRecord : dataRecord
        {
            private string username;
            private string password;

            public userRecord(JsonValue record) : base(record)
            {
                this.username = (string)record["username"];
                this.password = (string)record["password"];
            }

            public userRecord(SqliteDataReader record) : base(record)
            {
                this.username = (string)record["username"];
                this.password = (string)record["password"];
            }

            public void insertDataRecord(_database database)
            {
                string insertString = String.Format("INSERT INTO users (_id, username, password) values ('{0}', '{1}', '{2}')", _id, username, password);
                database.insertDataRecord(insertString);
            }
        }

        public class clientRecord : dataRecord
        {
            private string naam;
            private string adres;
            private string postcode;
            private string woonplaats;
            private string telefoonnummer;


            public clientRecord(JsonValue record) : base(record)
            {
                naam = record["naam"];
                adres = record["adres"];
                postcode = record["postcode"];
                woonplaats = record["woonplaats"];
                telefoonnummer = record["telefoonnummer"];
            }

            public clientRecord(SqliteDataReader record) : base(record)
            {
                naam = (string)record["naam"];
                adres = (string)record["adres"];
                postcode = (string)record["postcode"];
                woonplaats = (string)record["woonplaats"];
                telefoonnummer = (string)record["telefoonnummer"];
            }
            public void insertDataRecord(_database database)
            {
                string insertString = String.Format("INSERT INTO clients (_id, naam, adres, postcode, woonplaats, telefoonnummer) values ('{0}','{1}','{2}','{3}','{4}','{5}')", _id, naam, adres, postcode, woonplaats, telefoonnummer);
                database.insertDataRecord(insertString);
            }
        }

        public class taskRecord : dataRecord
        {
            public string bezoek_id;
            public string titel;
            public string omschrijving = null;
            public int voltooid = 0;

            public taskRecord(JsonValue record) : base(record)
            {
                bezoek_id = (string)record["bezoek_id"]["_id"];
                titel = (string)record["titel"];
                omschrijving = (string)record["omschrijving"];
                voltooid = (int)record["voltooid"];
            }

            public taskRecord(SqliteDataReader record) : base(record)
            {
                bezoek_id = (string)record["bezoek_id"];
                titel = (string)record["titel"];
                omschrijving = (string)record["omschrijving"];
                voltooid = (int)record["voltooid"];
            }
            public void insertDataRecord(_database database)
            {
                string insertString = String.Format("INSERT INTO tasks (_id, bezoek_id, titel, omschrijving, voltooid) values ('{0}', '{1}', '{2}', '{3}', {4})", _id, bezoek_id, titel, omschrijving, voltooid);
                database.insertDataRecord(insertString);
            }
        }
    }
}
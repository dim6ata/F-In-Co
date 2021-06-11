using ENTERPRISE_CWK2.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// A singleton class that is responsible for connecting and closing database connections
    /// </summary>
    public class SystemDB
    {
        private static SystemDB systemDBInstance;
        private static object syncObj = new object();

        //MAKE SINGLETON

        //prevents reflection access to constructor 
        private SystemDB()
        {
            if (systemDBInstance != null)
            {
                throw new Exception("Only one SystemDB can exist at the same time.");
            }
        }

        /// <summary>
        /// Property that instantiates only one instance of this class.
        /// </summary>
        public static SystemDB Instance
        {
            get
            {
                if (systemDBInstance == null)
                {
                    lock (syncObj)
                    {
                        if (systemDBInstance == null)
                        {
                            systemDBInstance = new SystemDB();
                        }
                    }
                }
                return systemDBInstance;
            }

        }

        /// <summary>
        /// Responsible for connecting to database.
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection connect()
        {
            string dbName = "FincoDB.db";
            string connectionString = String.Format("Data source={0};Version=3;" +
                "New=False;Compress=True;", dbName);
            SQLiteConnection conn = null;
            try
            {
                conn = new SQLiteConnection(connectionString);
                conn.Open();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL connection error - " + e);
            }

            return conn;
        }

        /// <summary>
        /// Closes both readers and connections 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="conn"></param>
        public void CloseConnections(SQLiteDataReader reader, SQLiteConnection conn)
        {
            
            if (reader != null) reader.Close();
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
        }

        /// <summary>
        /// Closes connection
        /// </summary>
        /// <param name="conn"></param>
        public void CloseConnection(SQLiteConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Android.Util;
using AndroidSQLite.Resources.Model;

namespace AndroidSQLite.Resources.DataHelper
{
   
    public class DataBase
    {
        public static DataBase db = null;
        public static DataBase getDataBase()
        {
            if(DataBase.db == null)
            {
                db = new DataBase();
            }
            return db;
        }

        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDataBase()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.CreateTable<Task>();
                    return true;
                }
        }


        public bool insertIntoTablePerson(Task task)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Insert(task);
                    return true;
                }
        }

        public List<Task> selectTablePerson()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var a = connection.Table<Task>().ToList();
                    return connection.Table<Task>().ToList();
                   
                }
        }
       

        public bool updateTablePerson(Task person)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Query<Task>("UPDATE Task set Name=?,Date=?,Time=?,Description=?,Priority=?,Category=?,Done=? Where Id=?",
                        person.Name,person.Date,person.Time,person.Description,person.Priority,person.Category,person.Done,person.Id);
                    return true;
                }
        }

        public bool deleteTablePerson(Task person)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Delete(person);
                    return true;
                }
        }

        public bool selectQueryTablePerson(int Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    return true;
                }
        }


        public long selectQuery(long Id)
        {
                using(var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                   var someData = connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    long dataId =  someData[0].Id;

                    Console.WriteLine(someData[0].Date.Day + "/" + someData[0].Date.Month + "/" + someData[0].Date.Year);
                    return dataId;
                }
        }
        //Отдает поля из БД
        public List<Task> get_Element(long Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var someData = connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    return someData;
                }
        }


        public Task get_Last()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var someData = connection.Query<Task>("SELECT * FROM Task ORDER BY Id");
                    Console.WriteLine("Id найденное функцией get_Last - " + someData[someData.Count-1].Id);
                    return someData[someData.Count - 1];
                }
        }
        //---------------------------------------------------------------------------------------------------
        //Достижения
        public bool createDataBaseAchivments()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.CreateTable<Achievement1>();
                    return true;
                }
        }
        public List<Achievement1> getAchievments()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                return connection.Table<Achievement1>().ToList();
            }
        }
        public void updateTableAchievements(string Category, long Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                        if (Category == "Спорт")
                        {
                            connection.Query<Achievement1>("UPDATE Achievement1 set SportExp = SportExp + 1  Where Name=?", "Достижения");
                            connection.Query<Achievement1>("UPDATE Achievement1 set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Образоване")
                        {
                            connection.Query<Achievement1>("UPDATE Achievement1 set EducationExp = EducationExp + 1 Where Name=?", "Достижения");
                            connection.Query<Achievement1>("UPDATE Achievement1 set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Спорт")
                        {
                            connection.Query<Achievement1>("UPDATE Achievement1 set SportExp = SportExp + 1  Where Name=?", "Достижения");
                            connection.Query<Achievement1>("UPDATE Achievement1 set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Финансы")
                        {
                            connection.Query<Achievement1>("UPDATE Achievement1 set FinansiExp = FinansiExp  + 1 Where Name=?", "Достижения");
                            connection.Query<Achievement1>("UPDATE Achievement1 set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Прочее")
                        {
                            connection.Query<Achievement1>("UPDATE Achievement1 set OtherExp = OtherExp + 1 Where Name=?", "Достижения");
                            connection.Query<Achievement1>("UPDATE Achievement1 set MainExp = MainExp + 1 Where Name=?", "Достижения");
                        }

                }
        }

        // ---------------------------------------------------------------------------------------------
        // Достижения3
        public bool createDataBaseAchivments3()
        {   
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.CreateTable<Achievement3>();
                return true;
            }
        }

        public bool insertIntoTableAchievement3(Achievement3 achievement3)
        { 
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.Insert(achievement3);
                return true;
            }
        }

        public List<Achievement3> selectTableAchievement3()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                var a = connection.Table<Achievement3>().ToList();
                return connection.Table<Achievement3>().ToList();

            }
        }






        // -------------------------------------------------------------------------------------
        // BannedId
        public bool createDataBaseBannedId()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.CreateTable<BannedId>();
                return true;
            }
        }

        public List<BannedId> getBannedId()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                return connection.Table<BannedId>().ToList();
            }
        }

        public List<BannedId> findBannedId(int Id)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                return connection.Query<BannedId>("SELECT * FROM BannedId where Id = ?",Id).ToList();
                
            }
        }


        public bool insertIntoTableBannedId(BannedId bannedId)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.Insert(bannedId);
                return true;
            }
        }
    }
}
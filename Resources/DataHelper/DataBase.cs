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
        //Опыт
        public bool createDataBaseExp()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.CreateTable<Exp>();
                    return true;
                }
        }
        public List<Exp> getExp()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                return connection.Table<Exp>().ToList();
            }
        }
        public void updateTableExp(string Category, long Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                        if (Category == "Спорт")
                        {
                            connection.Query<Exp>("UPDATE Exp set SportExp = SportExp + 1  Where Name=?", "Достижения");
                            connection.Query<Exp>("UPDATE Exp set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Образоване")
                        {
                            connection.Query<Exp>("UPDATE Exp set EducationExp = EducationExp + 1 Where Name=?", "Достижения");
                            connection.Query<Exp>("UPDATE Exp set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Спорт")
                        {
                            connection.Query<Exp>("UPDATE Exp set SportExp = SportExp + 1  Where Name=?", "Достижения");
                            connection.Query<Exp>("UPDATE Exp set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Финансы")
                        {
                            connection.Query<Exp>("UPDATE Exp set FinansiExp = FinansiExp  + 1 Where Name=?", "Достижения");
                            connection.Query<Exp>("UPDATE Exp set MainExp = MainExp + 1  Where Name=?", "Достижения");
                        }
                        if (Category == "Прочее")
                        {
                            connection.Query<Exp>("UPDATE Exp set OtherExp = OtherExp + 1 Where Name=?", "Достижения");
                            connection.Query<Exp>("UPDATE Exp set MainExp = MainExp + 1 Where Name=?", "Достижения");
                        }

                }
        }

        // ---------------------------------------------------------------------------------------------
        // Ачивки
        public bool createDataBaseAchivment()
        {   
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.CreateTable<Achievement>();
                return true;
            }
        }

        public bool insertIntoTableAchievement(Achievement achievement)
        { 
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                connection.Insert(achievement);
                return true;
            }
        }

        public List<Achievement> selectTableAchievement()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
            {
                var a = connection.Table<Achievement>().ToList();
                return connection.Table<Achievement>().ToList();

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
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
        string dbTable = "PersonsTest.db";
        public bool createDataBase()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.CreateTable<Task>();
                    return true;
                }
        }


        public bool insertIntoTableTask(Task task)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.Insert(task);
                    return true;
                }
        }

        public List<Task> selectTableTask()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    //var a = connection.Table<Task>().ToList();
                    return connection.Table<Task>().ToList();
                }
        }
       

        public bool updateTableTask(Task task)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.Query<Task>("UPDATE Task set Name=?,Date=?,Time=?,Description=?,Priority=?,Category=?,Done=? Where Id=?",
                        task.Name,task.Date,task.Time,task.Description,task.Priority,task.Category,task.Done,task.Id);
                    return true;
                }
        }

        public bool deleteTableTask(Task task)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.Delete(task);
                    return true;
                }
        }

        public void delTask(int Id)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.Query<Task>("DELETE FROM Task Where Id=?", Id);
            }
        }

        public void delAllDoneTask()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.Query<Task>("DELETE FROM Task Where Done=?",true);
            }
        }

        public List<Task> showCategory(string Category)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                var someData = connection.Query<Task>("SELECT * FROM Task WHERE Category=?", Category);
                return someData;
            }
            
        }

        public List<Task> showDate(DateTime date)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                Console.WriteLine("date = "+date);
                var someData = connection.Query<Task>("SELECT * FROM Task WHERE Date=?", date);
                return someData;
            }

        }


        public bool selectQueryTask(int Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    return true;
                }
        }


        public long selectQuery(long Id)
        {
                using(var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
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
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    var someData = connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    return someData;
                }
        }


        public Task get_Last()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    var someData = connection.Query<Task>("SELECT * FROM Task ORDER BY Id");
                    //Console.WriteLine("Id найденное функцией get_Last - " + someData[someData.Count-1].Id);
                    return someData[someData.Count - 1];
                }
        }
        //---------------------------------------------------------------------------------------------------
        //Опыт
        public bool createDataBaseExp()
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
                {
                    connection.CreateTable<Exp>();
                    return true;
                }
        }
        public List<Exp> getExp()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                return connection.Table<Exp>().ToList();
            }
        }

        public void insertExp(Exp exp)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.Insert(exp);
            }
        }

        public void updateTableExp(string Category, long Id)
        {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
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
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.CreateTable<Achievement>();
                return true;
            }
        }

        public bool insertIntoTableAchievement(Achievement achievement)
        { 
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.Insert(achievement);
                return true;
            }
        }

        public List<Achievement> selectTableAchievement()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                var a = connection.Table<Achievement>().ToList();
                return connection.Table<Achievement>().ToList();

            }
        }

        // -------------------------------------------------------------------------------------
        // Settings

        public bool createDataBaseSettings()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.CreateTable<Settings>();
                return true;
            }
        }

        public List<Settings> getSettings()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                return connection.Table<Settings>().ToList();
            }
        }
        public void insertStartSettings()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                Settings settings = new Settings()
                {
                    fastDel = true
                };
                connection.Insert(settings);
            }
        }
        public void updateSettings(bool value)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                //connection.Update(settings);
                connection.Query<Settings>("UPDATE Settings set fastDel = ?",value);
            }
        }


        // -------------------------------------------------------------------------------------
        // BannedId
        public bool createDataBaseBannedId()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.CreateTable<BannedId>();
                return true;
            }
        }

        public List<BannedId> getBannedId()
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                return connection.Table<BannedId>().ToList();
            }
        }

        public List<BannedId> findBannedId(int Id)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                return connection.Query<BannedId>("SELECT * FROM BannedId where Id = ?",Id).ToList();
                
            }
        }


        public bool insertIntoTableBannedId(BannedId bannedId)
        {
            using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, dbTable)))
            {
                connection.Insert(bannedId);
                return true;
            }
        }
    }
}
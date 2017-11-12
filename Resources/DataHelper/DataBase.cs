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
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.CreateTable<Person>();
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }


        public bool insertIntoTablePerson(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Insert(person);
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<Person> selectTablePerson()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var a = connection.Table<Person>().ToList();
                    return connection.Table<Person>().ToList();
                   
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                Console.WriteLine("Ничего не найдено");
                return null;
            }
        }
       

        public bool updateTablePerson(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Query<Person>("UPDATE Person set Name=?,Date=?,Time=?,Description=?,Priority=?,Category=?,Done=? Where Id=?",
                        person.Name,person.Date,person.Time,person.Description,person.Priority,person.Category,person.Done,person.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteTablePerson(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Delete(person);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTablePerson(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.Query<Person>("SELECT * FROM Person Where Id=?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }


        public long selectQuery(long Id)
        {
            try
            {
                using(var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                   var someData = connection.Query<Person>("SELECT * FROM Person Where Id=?", Id);
                    long dataId =  someData[0].Id;

                    Console.WriteLine(someData[0].Date.Day + "/" + someData[0].Date.Month + "/" + someData[0].Date.Year);
                    return dataId;
                }
            }
            catch(SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }
        //Отдает поля из БД
        public List<Person> get_Element(long Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var someData = connection.Query<Person>("SELECT * FROM Person Where Id=?", Id);
                   

                    return someData;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }


        public List<Person> get_Last()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    var someData = connection.Query<Person>("SELECT * FROM Person Where Name=?", "1");
                    //var someData = connection.Query<Person>("SELECT last_insert_rowid()");


                    return someData.ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //---------------------------------------------------------------------------------------------------
        //Достижения
        public bool createDataBaseAchivments()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    connection.CreateTable<Achievement>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public void updateTableAchievements(string Category)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "PersonsTest.db")))
                {
                    
                    Achievement achivments = new Achievement();
                    if (Category == "Спорт")
                    {
                        connection.Query<Achievement>("UPDATE Achievement set SportExp = SportExp + 1 Where Name=?","Достижения");
                        connection.Query<Achievement>("UPDATE Achievement set MainExp = MainExp + 1 Where Name=?", "Достижения");
                    }
                    if (Category == "Образоване")
                    {
                        connection.Query<Achievement>("UPDATE Achievement set EducationExp = EducationExp + 1 Where Name=?", "Достижения");
                        connection.Query<Achievement>("UPDATE Achievement set MainExp = MainExp + 1 Where Name=?", "Достижения");
                    }
                    if (Category == "Спорт")
                    { 
                        connection.Query<Achievement>("UPDATE Achievement set SportExp = SportExp + 1 Where Name=?", "Достижения");
                        connection.Query<Achievement>("UPDATE Achievement set MainExp = MainExp + 1 Where Name=?", "Достижения");
                    }
                    if (Category == "Финансы")
                    {
                        connection.Query<Achievement>("UPDATE Achievement set FinansiExp = FinansiExp + 1 Where Name=?", "Достижения");
                        connection.Query<Achievement>("UPDATE Achievement set MainExp = MainExp + 1 Where Name=?", "Достижения");
                    }
                    if(Category == "Прочее")
                    {
                        connection.Query<Achievement>("UPDATE Achievement set OtherExp = OtherExp + 1 Where Name=?", "Достижения");
                        connection.Query<Achievement>("UPDATE Achievement set MainExp = MainExp + 1 Where Name=?", "Достижения");

                    }
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                
            }
        }
    }
}
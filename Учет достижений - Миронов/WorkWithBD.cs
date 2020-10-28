using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Учет_достижений___Миронов
{
	class WorkWithBD
	{
        private MySqlConnection connection = new MySqlConnection("server=192.168.0.4;user=root;database=mydb;password=8725;");
        public int Authentication(string name, int password)
        {
            int rank = -1;
            connection.Open();
            MySqlCommand command = new MySqlCommand($"SELECT users.rank FROM mydb.users WHERE users.Name = '{name}' AND users.password = '{password}'", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                rank = Convert.ToInt32(reader.GetValue(0));
            }
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return rank;
        }
        public List<string> ListGroup()
            => ListQuest(new MySqlCommand($"SELECT group.Code FROM mydb.group", connection));
        public List<string> ListStudens() =>
            ListQuest(new MySqlCommand("SELECT mydb.student.NymberBook " +
                "FROM mydb.student right join mydb.group on mydb.student.Group_Code = mydb.group.Code", connection));
        public List<string> ListStudens(string Group) => 
            ListQuest(new MySqlCommand("SELECT mydb.student.NymberBook " +
                "FROM mydb.student right join mydb.group on mydb.student.Group_Code = mydb.group.Code" +
                $" WHERE mydb.group.Code = '{Group}'", connection));
        public List<string> ListEvents()
            => ListQuest(new MySqlCommand("SELECT mydb.events.name FROM mydb.events", connection));
        public List<string> ListEventInfo(string name)
        {
            List<string> list = new List<string>();
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT mydb.achievements.Student_NymberBook, mydb.achievements.Info " + 
                "FROM mydb.events LEFT JOIN " + 
                "mydb.achievements ON mydb.events.ID = mydb.achievements.Events_ID " + 
                $"WHERE mydb.events.Name = '{name}' " + 
                "order by mydb.achievements.Info", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    list.Add($"{reader.GetString(0)} - {reader.GetString(1)}");
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return list;
        }
        public List<string> RatingAll()
        {
            List<StudentRating> listStd = new List<StudentRating>();
            foreach (string item in ListStudens())
                listStd.Add(new StudentRating()
                {
                    NumberBook = item, Rating = RatingStudent(item)
                }) ;
            List<StudentRating> newlistStd = listStd.OrderByDescending(i => i.Rating).ToList();
            List<string> retList = new List<string>();
			foreach (StudentRating item in newlistStd)
                retList.Add($"{item.NumberBook} - {item.Rating}");
            return retList;
        }
        private List<string> ListQuest (MySqlCommand command)
        {
            List<string> list = new List<string>();
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    list.Add(reader.GetString(0));
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return list;
        }
        public int RatingGroup(string Group) =>
            SummRating($" WHERE mydb.student.Group_Code = '{Group}'");
        public int RatingStudent(string numberBook) =>
            SummRating($" WHERE mydb.student.NymberBook = '{numberBook}'");
        private int SummRating(string Quest)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT mydb.achievements.Info " +
                " FROM mydb.achievements right join mydb.student on mydb.student.NymberBook = mydb.achievements.Student_NymberBook " +
                Quest, connection);
            int sum = 0;
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    sum += RatingSum(reader.GetString(0));
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return sum;
        }
        private int RatingSum(string str)
        {
			switch (str)
			{
                case "I место": return 5;
                case "II место": return 4;
                case "III место": return 3;
                case "Участие": return 2;
                default: return 0;
			}
		}
        public List<string> ListArStrudent(string number)
        {
            List<string> list = new List<string>();
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT mydb.achievements.Student_NymberBook, mydb.achievements.Info " +
                "FROM mydb.events LEFT JOIN " +
                "mydb.achievements ON mydb.events.ID = mydb.achievements.Events_ID "+
                $"WHERE mydb.achievements.Student_NymberBook = '{number}' "+
                "order by mydb.achievements.Info", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                    list.Add($"{reader.GetString(0)} {reader.GetString(1)}");
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return list;
        }
        private class StudentRating
        {
            public string NumberBook { get; set; }
            public int Rating { get; set; }
        }
        public int AddEvents(string info)
        {
            int req = -1, maxId = MaxIDEvent();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"INSERT INTO `mydb`.`events` (`ID`, `Name`) VALUES ('{maxId}', '{info}');", connection);
			try
			{
                req = command.ExecuteNonQuery() != -1 ? maxId : -1;                    
			}
			catch (Exception){}
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return req;
        }
        public int AddAchievements(string number, int eventID, string Name)
        {
            int req = -1, maxId = MaxIDAchievements();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"INSERT INTO `mydb`.`achievements` (`ID`, `Student_NymberBook`, `Events_ID`, `Info`) VALUES ('{maxId}', '{number}', '{eventID}', '{Name}')", connection);
            try
            {
                req = command.ExecuteNonQuery();
            }
            catch (Exception) { }
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return req;
        }
        private int MaxIDEvent()
        {
            int max = -1;
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT mydb.events.ID FROM mydb.events order by mydb.events.ID DESC", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                max = Convert.ToInt32(reader.GetValue(0));
            }
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return ++max;
        }
        private int MaxIDAchievements()
        {
            int max = -1;
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT mydb.achievements.ID FROM mydb.achievements order by mydb.achievements.ID DESC", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                max = Convert.ToInt32(reader.GetValue(0));
            }
            reader.Dispose();
            reader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            return ++max;
        }
    }
}

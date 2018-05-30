using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using NUnit.Framework;
using URISUtil.DataAccess;
using URISUtil.Response;
using WallUserMicroService.DataAccess;
using WallUserMicroService.Models;

namespace WallUserMicroService.Tests.UnitTests
{
    
    public class UserUnitTest
    {
        ActiveStatusEnum active = ActiveStatusEnum.Active;
        SqlConnection connection;

        [SetUp]
        public void TestInitialize()
        {
            FileInfo file = new FileInfo("C:\\Users\\Gligoric\\Documents\\URIS\\userMSTestInsert.sql");
            string script = file.OpenText().ReadToEnd();
            connection = new SqlConnection(DBFunctions.ConnectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandText = script;
            connection.Open();
            command.ExecuteNonQuery();
        }

        [Test]
        public void GetUsersSuccess()
        {
            List<User> users = UserDB.GetAllUsers(active);
            Guid userId = users[0].Id;
            Assert.AreEqual(users.Count, 3);
        }

        [Test]
        public void GetUserSuccess()
        {
            Guid id = UserDB.GetAllUsers(active)[0].Id;
            User user = UserDB.GetUser(id);
            Assert.IsNotNull(user);
        }

        [Test]

        public void GetUserFailed()
        {
            Guid id = Guid.NewGuid();
            User user = UserDB.GetUser(id);
            Assert.IsNull(user);
        }

        [Test]
        public void InsertUserSuccess()
        {
            User user = new User
            {
                FirstName = "Jala",
                LastName = "Brat",
                Nickname = "JalaBrat",
                Email = "jalabrt@gmail.com",
                Phone = "0672625321",
                Username = "jlbrtt",
                Password = "brtttt",
                Active = true
            };
            int oldNumberOfUsers = UserDB.GetAllUsers(active).Count;
            UserDB.InsertUser(user);
            Assert.AreEqual(UserDB.GetAllUsers(active).Count, oldNumberOfUsers + 1);
        }

        [Test]
        public void InsertUserFailed()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void UpdateUserSuccess()
        {
            Guid id = UserDB.GetAllUsers(active)[0].Id;
            User user = new User
            {
                FirstName = "Buba",
                LastName = "Correli",
                Nickname = "bubacc",
                Email = "bubacor@gmail.com",
                Phone = "06726253222",
                Username = "buba",
                Password = "correliiiii",
                Active = true
            };

            User updatedUser = UserDB.UpdateUser(user, id);
            Assert.AreEqual(user.FirstName, updatedUser.FirstName);
            Assert.AreEqual(user.LastName, updatedUser.LastName);
            Assert.AreEqual(user.Nickname, updatedUser.Nickname);
            Assert.AreEqual(user.Email, updatedUser.Email);
            Assert.AreEqual(user.Phone, updatedUser.Phone);
            Assert.AreEqual(user.Username, updatedUser.Username);
            Assert.AreEqual(user.Password, updatedUser.Password);
            Assert.AreEqual(user.Active, updatedUser.Active);
        }

        [Test]
        public void UpdateUserFailed()
        {
            Guid id = Guid.NewGuid();
            User user = new User
            {
                FirstName = "Buba2",
                LastName = "Correli2",
                Nickname = "bubacc2",
                Email = "bubacor2@gmail.com",
                Phone = "06726253222111",
                Username = "buba2",
                Password = "",
                Active = true
            };

            User updatedUser = UserDB.UpdateUser(user, id);
            Assert.IsNull(updatedUser);

        }

        [Test]
        public void DeleteUserSuccess()
        {
            Guid id = UserDB.GetAllUsers(active)[0].Id;
            UserDB.DeleteUser(id);
            Assert.AreEqual(UserDB.GetUser(id).Active, false);
        }

        [Test]
        public void DeleteUserFailed()
        {
            int numberOfOldUsers = UserDB.GetAllUsers(active).Count;
            UserDB.DeleteUser(Guid.NewGuid());
            Assert.AreEqual(numberOfOldUsers, UserDB.GetAllUsers(active).Count);
        }

        [TearDown]
        public void TestCleanup()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = String.Format(@"DROP TABLE [user].[User]");
            command.ExecuteNonQuery();
            connection.Close();

        }

    }
    
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
using AmonicAirLines.page;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using AmonicAirLines;


namespace UnitTestAnomicAirLines
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            Usr usr = new Usr();
            string userName = "1";
            string password = "1";
            Task.Run(async () =>
            {
                usr = await StartPage.checkUser(userName, password);
            }).Wait();
            Assert.AreEqual(usr.status, "admin");
        }
        [TestMethod]
        public void TestLogin1()
        {
            Usr usr = new Usr();
            string userName = "forTest@mail.com";
            string password = "password_tester";
            Task.Run(async () =>
            {
                usr = await StartPage.checkUser(userName, password);
            }).Wait();
            Assert.AreEqual(usr.status, "user");
        }
        [TestMethod]
        public void TestLogin2()
        {
            Usr usr = new Usr();
            string userName = "3";
            string password = "w";
            Task.Run(async () =>
            {
                usr = await StartPage.checkUser(userName, password);
            }).Wait();
            Assert.AreEqual(usr.status, "false");
        }
        [TestMethod]
        public void TestLogin3()
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            for (int i = 0; i < adminMainPage.OfficeList.Count(); i++)
            {
                Console.WriteLine(adminMainPage.OfficeList[i].Title);
            }
        }
        [TestMethod]
        public void TestLogin4()
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            for (int i = 0; i < adminMainPage.users1.Count(); i++)
            {
                Console.WriteLine(adminMainPage.users1[i].FirstName);
            }
        }

        [TestMethod]
        public void TestLogin5()
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            for (int i = 0; i < adminMainPage.users1.Count(); i++)
            {
                Console.WriteLine(adminMainPage.users1[i].FirstName);
            }
        }
        [TestMethod]
        public void TestLogin6()
        {
            AdminMainPage adminMainPage = new AdminMainPage();
            adminMainPage.OfficeComboBox.SelectedIndex = 2;
            adminMainPage.OfficeComboBox_SelectionChanged(null, null);
            for (int i = 0; i < adminMainPage.users1.Count(); i++)
            {
                Console.WriteLine(adminMainPage.users1[i].FirstName);
            }
        }

    }
}

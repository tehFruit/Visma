using Meetings.Data;
using Meetings.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace Meetings.UnitTests
{
    [TestClass]
    public class MeetingServiceTests
    {
        [TestMethod]
        public void NewMeeting_Success()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            int meetingCount = repo.GetAllMeetings().Count();

            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);

            int meetingCountAfter = repo.GetAllMeetings().Count();
            repo.DeleteMeeting("Name", "Person");

            Assert.IsTrue((meetingCountAfter - meetingCount) == 1);
        }

        [TestMethod]
        public void DeleteMeeting_Success()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            bool status = repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(status);
        }

        [TestMethod]
        public void DeleteMeeting_MeetingNotFound()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            bool status = repo.DeleteMeeting("FalseName", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void DeleteMeeting_WrongPerson()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            bool status = repo.DeleteMeeting("NName", "PersonWrong");
            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void AddPersonToMeeting_Success()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            bool status =  repo.AddPersonToMeeting("Name", new Person("Person1", DateTime.Now));
            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(status);
        }

        [TestMethod]
        public void AddPersonToMeeting_MeetingNotFound()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);

            bool status = repo.AddPersonToMeeting("FalseName", new Person("Person1", DateTime.Now));
            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }


        [TestMethod]
        public void AddPersonToMeeting_PersonAlreadyInMeeting()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);

            repo.AddPersonToMeeting("Name", new Person("Person1", DateTime.Now));
            bool status = repo.AddPersonToMeeting("Name", new Person("Person1", DateTime.Now));
            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void RemovePersonFromMeeting_MeetingDoesNotExist()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.AddPersonToMeeting("Name", new Person("Person1", DateTime.Now));

            bool status = repo.RemovePersonFromMeeting("FalseName", "Person1");

            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void RemovePersonFromMeeting_PersonNotInMeeting()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);

            bool status = repo.RemovePersonFromMeeting("Name", "Person1");

            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void RemovePersonFromMeeting_RemoveResponsiblePerson()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.AddPersonToMeeting("Name", new Person("Person", DateTime.Now));

            bool status = repo.RemovePersonFromMeeting("Name", "Person");

            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(!status);
        }

        [TestMethod]
        public void RemovePersonFromMeeting_Success()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.AddPersonToMeeting("Name", new Person("Peter", DateTime.Now));

            bool status = repo.RemovePersonFromMeeting("Name", "Peter");

            repo.DeleteMeeting("Name", "Person");
            Assert.IsTrue(status);
        }

        [TestMethod]
        public void GetAllMeetings()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);

            int count = repo.GetAllMeetings().Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void GetMeetingsByDescription()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);

            Meeting meeting1 = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting1);

            int count = repo.GetMeetingsByDescription(".Net").Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void GetMeetingsByResponsiblePerson()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);

            Meeting meeting1 = new Meeting("Name", "Ben", "Description", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting1);

            int count = repo.GetMeetingsByResponsiblePerson("Ben").Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Ben");
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetMeetingsByCategory()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);

            Meeting meeting1 = new Meeting("Name", "Person", "Description", Category.Hub, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting1);

            int count = repo.GetMeetingsByCategory(Category.CodeMonkey).Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void GetMeetingsByType()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting);

            Meeting meeting1 = new Meeting("Name", "Person", "Description", Category.CodeMonkey, Meetings.Models.Type.Live, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting1);

            int count = repo.GetMeetingsByType(Meetings.Models.Type.InPerson).Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void GetMeetingsByDates()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, 
                DateTime.ParseExact("2022-06-06 19:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateTime.ParseExact("2022-06-06 19:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));

            Meeting meeting1 = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson,
                DateTime.ParseExact("2022-06-08 15:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateTime.ParseExact("2022-06-08 16:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));

            Meeting meeting2 = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson,
                DateTime.ParseExact("2022-07-01 19:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateTime.ParseExact("2022-07-01 19:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            repo.NewMeeting(meeting);
            repo.NewMeeting(meeting1);
            repo.NewMeeting(meeting2);

            int count = repo.GetMeetingsByDates(DateTime.ParseExact("2022-06-01 00:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateTime.ParseExact("2022-06-20 00:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetMeetingsByAttendees()
        {
            MeetingRepository repo = new MeetingRepository("test.json");
            Meeting meeting = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting);
            repo.AddPersonToMeeting("Name", new Person("Andrew", DateTime.Now));
            repo.AddPersonToMeeting("Name", new Person("Michael", DateTime.Now));
            repo.AddPersonToMeeting("Name", new Person("Paul", DateTime.Now));

            Meeting meeting1 = new Meeting("Name", "Person", ".Net is cool", Category.CodeMonkey, Meetings.Models.Type.InPerson, DateTime.Now, DateTime.Now);
            repo.NewMeeting(meeting1);

            int count = repo.GetMeetingsByAttendees(2).Count();
            repo.DeleteMeeting("Name", "Person");
            repo.DeleteMeeting("Name", "Person");
            Assert.AreEqual(1, count);
        }
    }
}
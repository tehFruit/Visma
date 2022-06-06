using Meetings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Meetings.Data
{
    public class MeetingRepository : IMeetingRepository
    {
        private string fileName;

        public MeetingRepository(string file)
        {
            this.fileName = file;
        }

        public bool AddPersonToMeeting(string meetingName, Person person)
        {
            List<Meeting> meetingList = GetAllMeetings();
            if (meetingList == null)
                return false;

            int index = meetingList.FindIndex(m => m.Name == meetingName);
            if(index == -1)
            {
                Console.WriteLine("Meeting does not exist");
                return false;
            }

            if(!meetingList[index].Participants.Where(p => p.Name == person.Name).Any())
            {
                meetingList[index].Participants.Add(person);
                ToFile(meetingList);
                return true;
            }
            else{
                Console.WriteLine("User already in meeting");
                return false;
            }
            
        }

        public bool DeleteMeeting(string meetingName, string currentUser)
        {
            List<Meeting> meetingList = GetAllMeetings();
            if (meetingList == null)
                return false;

            int index = meetingList.FindIndex(m => m.Name == meetingName);
            if (index == -1)
            {
                Console.WriteLine("Meeting does not exist");
                return false;
            }

            if (currentUser.ToLower() == meetingList[index].ResponsiblePerson.ToLower())
            {
                meetingList.RemoveAt(index);
                ToFile(meetingList);
                return true;
            }
            return false;
        }

        public List<Meeting> GetAllMeetings()
        {
            List<Meeting> meetings = new List<Meeting>();

            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                meetings = JsonSerializer.Deserialize<List<Meeting>>(json);
            }

            return meetings;
        }

        public List<Meeting> GetMeetingsByAttendees(int moreThan)
        {
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.Participants.Count > moreThan).ToList();
            return meetingList;
        }

        public List<Meeting> GetMeetingsByCategory(Category category)
        {
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.Category == category).ToList();
            return meetingList;
        }

        public List<Meeting> GetMeetingsByDates(DateTime from, DateTime to)
        {
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.StartDate >= from).Where(m => m.EndDate <= to).ToList();
            return meetingList;
        }

        public List<Meeting> GetMeetingsByDescription(string description)
        {
            description = description.ToLower();
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.Description.ToLower().Contains(description)).ToList();
            return meetingList;
        }

        public List<Meeting> GetMeetingsByResponsiblePerson(string responsiblePerson)
        {
            responsiblePerson = responsiblePerson.ToLower();
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.ResponsiblePerson.ToLower() == responsiblePerson).ToList();
            return meetingList;
        }

        public List<Meeting> GetMeetingsByType(Models.Type type)
        {
            List<Meeting> meetingList = GetAllMeetings();
            meetingList = meetingList.Where(m => m.Type == type).ToList();
            return meetingList;
        }

        public bool NewMeeting(Meeting meeting)
        {
            List<Meeting> meetingList = GetAllMeetings();
            if (meetingList == null)
                return false;

            meetingList.Add(meeting);
            ToFile(meetingList);
            return true;
        }

        public bool RemovePersonFromMeeting(string meetingName, string personName)
        {
            List<Meeting> meetingList = GetAllMeetings();
            if (meetingList == null)
                return false;

            int index = meetingList.FindIndex(m => m.Name == meetingName);
            if (index == -1)
            {
                Console.WriteLine("Meeting does not exist");
                return false;
            }

            int indexPerson = meetingList[index].Participants.FindIndex(p => p.Name == personName);
            if(indexPerson == -1)
            {
                Console.WriteLine("Person is not in meeting");
                return false;
            }

            if (meetingList[index].Participants[indexPerson].Name == meetingList[index].ResponsiblePerson)
            {
                return false;
            }
            else
            {
                meetingList[index].Participants.RemoveAt(indexPerson);
                ToFile(meetingList);
                return true;
            }
        }

        private void ToFile(List<Meeting> meetings)
        {
            string serialized = System.Text.Json.JsonSerializer.Serialize(meetings);
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine(serialized);
            }
        }
    }
}

using Meetings.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings.Data
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _repo;
        public MeetingService(IMeetingRepository repo)
        {
            this._repo = repo;
        }
        
        public List<Meeting> GetAllMeetings()
        {
            return _repo.GetAllMeetings();
        }

        public void NewMeeting()
        {
            Console.WriteLine("Enter meeting's name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter meeting's responsible person's name:");
            string respnsiblePerson = Console.ReadLine();

            Console.WriteLine("Enter meeting's description");
            string description = Console.ReadLine();

            Console.WriteLine("Enter meeting's category (1 - CodeMonkey, 2 - Hub, 3 - Short, 4 - TeamBuilding)");
            int categoryNum = Convert.ToInt32(Console.ReadLine());
            Category category = (Category)categoryNum;

            Console.WriteLine("Enter meeting's type (1 - Live, 2 - InPerson)");
            int typeNum = Convert.ToInt32(Console.ReadLine());
            Meetings.Models.Type type = (Meetings.Models.Type)typeNum;

            Console.WriteLine("Enter meeting's start date in format yyyy-MM-dd HH:mm");
            string start = Console.ReadLine();
            DateTime startDate = DateTime.ParseExact(start, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.WriteLine("Enter meeting's end date in format yyyy-MM-dd HH:mm");
            string end = Console.ReadLine();
            DateTime endDate = DateTime.ParseExact(start, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            if(this._repo.NewMeeting(new Meeting(name, respnsiblePerson, description, category, type, startDate, endDate)))
            {
                Console.WriteLine("Meeting was created succeffully");
            }
            else
            {
                Console.WriteLine("Failed to create a new meeting");
            }
        }

        public void DeleteMeeting()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter meeting's name:");
            string meetName = Console.ReadLine();

            if(this._repo.DeleteMeeting(meetName, name))
            {
                Console.WriteLine("Meeting was deleted successfully");
            }
            else
            {
                Console.WriteLine("Failed to delete the meeting");
            }
        }

        public void addPersonToMeeting()
        {
            Console.WriteLine("Enter person's name");
            string name = Console.ReadLine();

            Console.WriteLine("Enter date when the person is being added in format yyyy-MM-dd HH:mm");
            string adding = Console.ReadLine();
            DateTime addingDate = DateTime.ParseExact(adding, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.WriteLine("Enter meeting's name:");
            string meetName = Console.ReadLine();

            if(this._repo.AddPersonToMeeting(meetName, new Person(name, addingDate)))
            {
                Console.WriteLine("Person was added to the meeting");
            }
            else
            {
                Console.WriteLine("Failed to add person to the meeting");
            }
        }

        public void RemovePersonFromMeeting()
        {
            Console.WriteLine("Enter meeting's name");
            string meeting = Console.ReadLine();

            Console.WriteLine("Enter person's name");
            string person = Console.ReadLine();

            if(this._repo.RemovePersonFromMeeting(meeting, person))
            {
                Console.WriteLine("Person was removed from meeting");
            }
            else
            {
                Console.WriteLine("Person was not removed ftom meeting");
            }
        }

        public void MeetingsList(string[] args)
        {
            List<Meeting> meetings = null;
            switch (args[1])
            {
                case "description":
                    meetings = this._repo.GetMeetingsByDescription(args[2]);
                    break;
                case "responsiblePerson":
                    meetings = this._repo.GetMeetingsByResponsiblePerson(args[2]);
                    break;
                case "category":
                    meetings = this._repo.GetMeetingsByCategory((Category)Enum.Parse(typeof(Category), args[2]));
                    break;
                case "type":
                    meetings = this._repo.GetMeetingsByType((Meetings.Models.Type)Enum.Parse(typeof(Meetings.Models.Type), args[2]));
                    break;
                case "dates":
                    meetings = this._repo.GetMeetingsByDates(
                        DateTime.ParseExact(args[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateTime.ParseExact(args[3], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    break;
                case "attendees":
                    meetings = this._repo.GetMeetingsByAttendees(Convert.ToInt32(args[2]));
                    break;

                default:
                    Console.WriteLine("Invalid command");
                    break;
            }

            if(meetings == null)
            {
                return;
            }

            for (int i = 0; i < meetings.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Meeting properties:");
                Console.WriteLine(String.Format("   Name: {0}", meetings[i].Name));
                Console.WriteLine(String.Format("   Responsible person: {0}", meetings[i].ResponsiblePerson));
                Console.WriteLine(String.Format("   Description: {0}", meetings[i].Description));
                Console.WriteLine(String.Format("   Category: {0}", meetings[i].Category));
                Console.WriteLine(String.Format("   Type: {0}", meetings[i].Type));
                Console.WriteLine(String.Format("   StartDate: {0}", meetings[i].StartDate));
                Console.WriteLine(String.Format("   EndDate: {0}", meetings[i].EndDate));

                if(meetings[i].Participants.Count > 0)
                {
                    Console.WriteLine("   Participants:");
                    for (int j = 0; j < meetings[i].Participants.Count; j++)
                    {
                        Console.WriteLine(String.Format("       Name: {0}", meetings[i].Participants[j].Name));
                        Console.WriteLine(String.Format("       DateAdded: {0}", meetings[i].Participants[j].DateAdded));
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}

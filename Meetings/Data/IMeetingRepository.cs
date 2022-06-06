using Meetings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings.Data
{
    public interface IMeetingRepository
    {
        bool NewMeeting(Meeting meeting);
        bool DeleteMeeting(string meetingName, string currentUser);
        bool AddPersonToMeeting(string meetingName, Person person);
        bool RemovePersonFromMeeting(string meetingName, string personName);
        List<Meeting> GetMeetingsByDescription(string description);
        List<Meeting> GetMeetingsByResponsiblePerson(string responsiblePerson);
        List<Meeting> GetMeetingsByCategory(Category category);
        List<Meeting> GetMeetingsByType(Meetings.Models.Type type);
        List<Meeting> GetMeetingsByDates(DateTime from, DateTime to);
        List<Meeting> GetMeetingsByAttendees(int moreThan);
        List<Meeting> GetAllMeetings();
    }
}

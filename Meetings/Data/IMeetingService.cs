using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings.Data
{
    public interface IMeetingService
    {
        void NewMeeting();
        void DeleteMeeting();
        void addPersonToMeeting();
        void RemovePersonFromMeeting();
        void MeetingsList(string[] args);
    }
}

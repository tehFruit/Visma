// See https://aka.ms/new-console-template for more information
using Meetings.Data;

MeetingRepository repo = new MeetingRepository("data.json");
MeetingService service = new MeetingService(repo);

switch (args[0])
{
    case "newMeeting":
        service.NewMeeting();
        break;

    case "deleteMeeting":
        service.DeleteMeeting();
        break;

    case "addPerson":
        service.addPersonToMeeting();
        break;

    case "removePerson":
        service.RemovePersonFromMeeting();
        break;

    case "meetings":
        service.MeetingsList(args);
        break;

    default:
        Console.WriteLine("Invalid command");
        break;
}
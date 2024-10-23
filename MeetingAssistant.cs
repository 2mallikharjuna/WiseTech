using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class MeetingAssistant
{
    public static string GetEarliestMeetTime(List<string> events, int k)
    {
        // Parse events and build busy times
        var busyTimes = ParseBusyTimes(events);

        // Find earliest available meeting time
        return FindEarliestMeetingTime(busyTimes, k);
    }

    private static Dictionary<string, List<Tuple<DateTime, DateTime>>> ParseBusyTimes(List<string> events)
    {
        var busyTimes = new Dictionary<string, List<Tuple<DateTime, DateTime>>>();

        foreach (var e in events)
        {
            var parts = e.Split();
            var name = parts[0];
            var startTime = DateTime.ParseExact(parts[2], "HH:mm", CultureInfo.InvariantCulture);
            var endTime = DateTime.ParseExact(parts[3], "HH:mm", CultureInfo.InvariantCulture);
            if (!busyTimes.ContainsKey(name))
            {
                busyTimes[name] = new List<Tuple<DateTime, DateTime>>();
            }
            busyTimes[name].Add(new Tuple<DateTime, DateTime>(startTime, endTime));
        }

        return busyTimes;
    }

    private static string FindEarliestMeetingTime(Dictionary<string, List<Tuple<DateTime, DateTime>>> busyTimes, int k)
    {
        var intervals = busyTimes.Values.SelectMany(i => i).OrderBy(interval => interval.Item1).ToList();
        var dayStart = DateTime.ParseExact("00:00", "HH:mm", CultureInfo.InvariantCulture);
        var dayEnd = DateTime.ParseExact("23:59", "HH:mm", CultureInfo.InvariantCulture);
        
        DateTime lastEndTime = dayStart;

        foreach (var interval in intervals)
        {
            if (lastEndTime + TimeSpan.FromMinutes(k) <= interval.Item1)
            {
                return lastEndTime.ToString("HH:mm");
            }
            lastEndTime = new[] { lastEndTime, interval.Item2 }.Max();
        }

        // Check for time after the last meeting until the end of the day
        if (lastEndTime + TimeSpan.FromMinutes(k) <= dayEnd)
        {
            return lastEndTime.ToString("HH:mm");
        }

        return "-1"; // No available time found
    }

    static void Main(string[] args)
    {
        string[] events = new string[] { "sam sleep 12:00 18:59", "alex gaming 00:00 11:00" };
        List<string> list = events.ToList();
        Console.WriteLine(GetEarliestMeetTime(list, 60)); // Output: "11:00"
    }
}

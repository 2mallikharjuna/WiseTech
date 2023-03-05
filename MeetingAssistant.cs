using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class MeetingAssistant
{
    public static T MaxValue<T>(params T[] values) where T : IComparable
    {
        return values.Max();
    }

    public static T MinValue<T>(params T[] values) where T : IComparable
    {
        return values.Min();
    }

    
    public static string getEarliestMeetTime(List<string> events, int k)
    {
        // Create a dictionary to store the busy time intervals for each person
        var busyTimes = new Dictionary<string, List<Tuple<DateTime, DateTime>>>();

        // Parse the events and add the busy time intervals to the dictionary
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

        // Initialize the earliest start time and latest end time to the beginning and end of the day, respectively
        var earliestStartTime = DateTime.ParseExact("00:00", "HH:mm", null);
        var latestEndTime = DateTime.ParseExact("23:59", "HH:mm", null);
        var firstIntervals = busyTimes.First().Value;
        var secondIntervals = busyTimes.Last().Value;        
        foreach (var firstInterval in firstIntervals)
        {
            bool isIntersecting = false;
            foreach (var secondInterval in secondIntervals)
            {
                if (firstInterval.Item1 < secondInterval.Item2 && firstInterval.Item2 > secondInterval.Item1)
                {
                    // The two intervals intersect
                    isIntersecting = true;
                    break;
                }

                if (!isIntersecting)
                {
                    //nonIntersectionInterval = firstInterval;
                    var gap = firstInterval.Item2 - firstInterval.Item1;
                    if (gap >= TimeSpan.FromMinutes(k))
                    {
                        earliestStartTime = MaxValue<DateTime>(earliestStartTime, firstInterval.Item1);
                        latestEndTime = MinValue<DateTime>(latestEndTime, firstInterval.Item2);
                        /*break*/;
                    }
                    // Check if there is a time slot of k minutes available for all people
                    if (earliestStartTime + TimeSpan.FromMinutes(k) <= latestEndTime)
                    {
                        return earliestStartTime.ToString("HH:mm");
                    }
                    
                }
            }
        }
        return "-1";
    }

    static void Main(string[] args)
    {
        string[] events = new string[] { "sam sleep 12:00 18:59", "alex gaming 00:00 11:00" };
        List<string> list = events.ToList();
        Console.WriteLine(getEarliestMeetTime(list, 60));
    }
}
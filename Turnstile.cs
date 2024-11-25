using System;
using System.Collections.Generic;

public class Solution
{
    const int DIRECTION_NONE = -1;
    const int DIRECTION_ENTER = 0;
    const int DIRECTION_EXIT = 1; // High priority

    // Helper class to store indexed time
    public class IndexedTime
    {
        public int Index { get; set; }
        public int Time { get; set; }

        public IndexedTime(int index, int time)
        {
            Index = index;
            Time = time;
        }
    }

    // Helper method to update the times in the queues
    public static void UpdateTime(Queue<IndexedTime> enterQueue, Queue<IndexedTime> exitQueue, int currentTime)
    {
        // Update time for the enter queue
        UpdateQueueTime(enterQueue, currentTime);
        // Update time for the exit queue
        UpdateQueueTime(exitQueue, currentTime);
    }

    // Helper method to update time for a single queue
    public static void UpdateQueueTime(Queue<IndexedTime> queue, int currentTime)
    {
        foreach (var item in queue)
        {
            if (item.Time == currentTime)
            {
                item.Time++;
            }
            else
            {
                return;
            }
        }
    }

    // Main method to get the times for all the people
    public static int[] GetTimes(int[] time, int[] direction)
    {
        var enterQueue = new Queue<IndexedTime>();
        var exitQueue = new Queue<IndexedTime>();

        int queueSize = direction.Length;

        // Populate the queues with indexed times based on direction
        for (int i = 0; i < queueSize; i++)
        {
            var indexedTime = new IndexedTime(i, time[i]);
            if (direction[i] == DIRECTION_ENTER)
            {
                enterQueue.Enqueue(indexedTime);
            }
            else if (direction[i] == DIRECTION_EXIT)
            {
                exitQueue.Enqueue(indexedTime);
            }
        }

        // Result array to store the result times for each person
        int[] resultTime = new int[queueSize];
        Array.Fill(resultTime, -1); // Initialize all times to -1

        int prevDirection = DIRECTION_NONE;
        int prevUseTime = -1;

        while (true)
        {
            // Peek the first elements in each queue
            IndexedTime enter = enterQueue.Count > 0 ? enterQueue.Peek() : null;
            IndexedTime exit = exitQueue.Count > 0 ? exitQueue.Peek() : null;

            // If both queues are empty, break the loop
            if (enter == null && exit == null)
            {
                break;
            }

            // If enter queue is empty, process the exit queue
            if (enter == null && exit != null)
            {
                resultTime[exit.Index] = exit.Time;
                exitQueue.Dequeue();

                UpdateTime(enterQueue, exitQueue, exit.Time);
                continue;
            }
            // If exit queue is empty, process the enter queue
            else if (exit == null && enter != null)
            {
                resultTime[enter.Index] = enter.Time;
                enterQueue.Dequeue();

                UpdateTime(enterQueue, exitQueue, enter.Time);
                continue;
            }

            int enterTime = enter.Time;
            int exitTime = exit.Time;

            // Time collision
            if (enterTime == exitTime)
            {
                // If in the previous second the turnstile was not used, exit goes first
                if (prevUseTime < (enterTime - 1))
                {
                    prevDirection = DIRECTION_EXIT;
                }

                switch (prevDirection)
                {
                    case DIRECTION_NONE:
                    case DIRECTION_EXIT:
                        // Exit goes first
                        resultTime[exit.Index] = exit.Time;
                        exitQueue.Dequeue();

                        UpdateTime(enterQueue, exitQueue, exit.Time);

                        prevDirection = DIRECTION_EXIT;
                        prevUseTime = exit.Time;
                        break;

                    case DIRECTION_ENTER:
                        // Enter goes first
                        resultTime[enter.Index] = enter.Time;
                        enterQueue.Dequeue();

                        UpdateTime(enterQueue, exitQueue, enter.Time);

                        prevDirection = DIRECTION_ENTER;
                        prevUseTime = enter.Time;
                        break;
                }
            }
            else
            {
                // No time collision
                if (enterTime < exitTime)
                {
                    resultTime[enter.Index] = enter.Time;
                    enterQueue.Dequeue();

                    UpdateTime(enterQueue, exitQueue, enter.Time);

                    prevDirection = DIRECTION_ENTER;
                    prevUseTime = enter.Time;
                }
                else
                {
                    resultTime[exit.Index] = exit.Time;
                    exitQueue.Dequeue();

                    UpdateTime(enterQueue, exitQueue, exit.Time);

                    prevDirection = DIRECTION_EXIT;
                    prevUseTime = exit.Time;
                }
            }
        }

        return resultTime;
    }

    // Main entry point for testing the function
    public static void Main(string[] args)
    {
        // Test input
        int[] time = { 0, 1, 1, 3, 3 };
        int[] direction = { 0, 1, 0, 0, 1 };

        // Function call
        int[] result = GetTimes(time, direction);

        // Output the result
        Console.WriteLine($"Output: [{string.Join(", ", result)}]"); // Expected: [0, 2, 1, 4, 3]
    }
}

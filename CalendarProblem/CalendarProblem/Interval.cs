namespace CalendarProblem;

public class Interval
{
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }

    //constructor
    public Interval(DateTime startTime, DateTime endTime)
    {
        this.startTime = startTime;
        this.endTime = endTime;
    }
}
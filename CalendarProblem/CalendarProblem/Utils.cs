namespace CalendarProblem;

public class Utils
{
    /**
     * transforma stringul in DateTime
     * in plus, daca ora este de forma 9:00 o transforma in 09:00
     */
    public static DateTime convertStringInDateTime(String dateTime)
    {
        if (dateTime.Length == 4)
        {
            dateTime = "0" + dateTime;
        }
        return DateTime.ParseExact(dateTime, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
    }
        
        
    /**
     * primeste 2 DateTime uri si returneaza valoarea mai mica
     */
    public static DateTime findMin(DateTime val1, DateTime val2)
    {
        if (val1.CompareTo(val2) >= 0)
        {
            return val2;
        }

        return val1;
    }
        
    
    /**
     * primeste 2 DateTime uri si returneaza valoarea mai mare
     */
    public static DateTime findMax(DateTime val1, DateTime val2)
    {
        if (val1.CompareTo(val2) >= 0)
        {
            return val1;
        }

        return val2;
    }

    
    /**
     * primeste intervalul ca string si-l transforma in obiectul Interval
     */
    public static Interval convertRange(string range)
    {
        string[] times = range.Replace("[", "").Replace("]", "").Split(",");
        DateTime startTime = convertStringInDateTime(times[0].Replace("'", ""));
        DateTime endTime = convertStringInDateTime(times[1].Replace("'", ""));
        return new Interval(startTime, endTime);
    }
    

    /**
     * Primeste 2 intervale si returneaza inveralul comun
     * EX: pt ['10:00','20:00'] si ['9:00','18:30']  -> ['10:00','18:30']
     */
    public static Interval findMinimumRange(Interval range1, Interval range2)
    {
        return new Interval(findMax(range1.startTime, range2.startTime), findMin(range1.endTime, range2.endTime));
    }
    
    
    /**
     * Primeste 2 intervale si returneaza inveralul maxim format
     * EX: pt ['10:00','11:00'] si ['10:30','12:30']  -> ['10:00','12:30']
     */
    public static Interval findMaximumRange(Interval range1, Interval range2)
    {
        return new Interval(findMin(range1.startTime, range2.startTime), findMax(range1.endTime, range2.endTime));
    }
    
    
    /**
     * Daca intervalul exista(stratTime mai mic decat endTime) returneaza True, altfel False
     */
    public static bool ifExistRange(Interval i)
    {
        if (i.startTime > i.endTime)
        {
            return false;
        }

        return true;
    }
    
    
    /**
     * verifica daca cele 2 intervale au un punct comun sau nu
     */
    public static bool verifyCombination(Interval el1, Interval el2)
    {
        return ifExistRange(findMinimumRange(el1, el2));
    }
    
    
    /**
     * primeste timpul pentru un meeting in minute si-l transforma in TimeSpan sub forma de HH:mm
     */
    public static TimeSpan convertTimeMeetingInDateTime(int timeMeeting)
    {
        return TimeSpan.FromMinutes(timeMeeting);
    }
        

    /**
     * calculeaza diferenta de timp dintre 2 intervale
     */
    public static TimeSpan timeDifference(Interval i1, Interval i2)
    {
        return i2.startTime - i1.endTime;
    }
}
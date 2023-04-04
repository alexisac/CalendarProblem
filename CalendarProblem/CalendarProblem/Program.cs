
namespace CalendarProblem
{
    internal class Program
    {
        /**
         * parseaza stringul primit si formeaza lista de intervale
         */
        public static List<Interval> convertCalendar(String calendar)
        {
            // creaza lista
            List<Interval> listCalendar = new List<Interval>();

            // transforma stringul primit 
            string[] intervals = calendar.Replace("[[", "").Replace("]]", "").Split("], [");

            // separa fiecare interval dupa ",", le transforma in DateTime si le adauga
            foreach (String interval in intervals)
            {
                string[] times = interval.Split(",");
                DateTime startTime = Utils.convertStringInDateTime(times[0].Replace("'", ""));
                DateTime endTime = Utils.convertStringInDateTime(times[1].Replace("'", ""));
                listCalendar.Add(new Interval(startTime, endTime));
            }
            return listCalendar;
        }


        /**
         * primeste lista si un interval. Verifica daca poate reuni ultimul interval din lista cu intervalul curent.
         * Daca poate face asta atunci le reuneste si-l adauga in lista, altfel adauga doar intervalul primit in lista
         */
        public static List<Interval> combineTwoRanges(List<Interval> lista, Interval range)
        {
            if (lista.Count >= 1 && Utils.verifyCombination(lista.ElementAt(lista.Count - 1), range))
            {
                Interval salv = lista.ElementAt(lista.Count - 1);
                lista.RemoveAt(lista.Count-1);
                lista.Add(Utils.findMaximumRange(salv, range));
            }
            else
            {
                lista.Add(range);   
            }

            return lista;
        }
        
        
        /**
         * primeste 2 lista de intervale indisponibile si returneaza lista obtinuta prin reuniunea celor 2 liste si a intervalelor din cele 2 liste
         * Adica primeste intervalele ['10:00','11:00'] si ['10:30','13:00'] va returna ['10:00','13:00']
         */
        public static List<Interval> listOfInaccessibleRange(List<Interval> listPers1, List<Interval> listPers2)
        {
            List<Interval> newList = new List<Interval>();
            
            int i = 0;  // cursor pe i
            int j = 0;  // cursor pe j

            while (listPers1.Count > i && listPers2.Count > j)
            {
                /**
                 * Daca exista intervalul minim format din cele 2 INTERVALE(obiectul) atunci le poate compune intr-un interval mai mare
                 * EX: pt intervalele ['10:00','11:00'] si ['10:30','12:30'] intervalul minim este ['10:30','11:00'] deci este un interval valid
                 *      adica au un interval comun asa ca invervalul maxim este -> ['10:00','12:30']
                 * EX: pt intervalele ['10:00','11:00'] si ['12:30','13:30'] intervalul minim este ['12:30', '11:00'] deci e un interval gresit
                 *      asa ca nu au un punct comun cele 2 intervale deci ele nu se pot grupa intr-un singur interval
                 */
                if (Utils.verifyCombination(listPers1.ElementAt(i), listPers2.ElementAt(j)))
                {
                    Interval newElem = Utils.findMaximumRange(listPers1.ElementAt(i), listPers2.ElementAt(j));
                    newList = combineTwoRanges(newList, newElem);
                    i++;
                    j++;
                }
                else
                {
                    if (listPers1.ElementAt(i).startTime > listPers2.ElementAt(j).startTime)
                    {
                        newList = combineTwoRanges(newList, listPers2.ElementAt(j));
                        j++;
                    }
                    else
                    {
                        newList = combineTwoRanges(newList, listPers1.ElementAt(i));
                        i++;
                    }
                }
            }

            while (listPers1.Count > i)
            {
                newList = combineTwoRanges(newList, listPers1.ElementAt(i));
                i++;
            }

            while (listPers2.Count > j)
            {
                newList = combineTwoRanges(newList, listPers2.ElementAt(j));
                j++;
            }


            /*
            List<Interval> verifiedList = new List<Interval>();
            //Aici voi parcurge iar vectorul si daca sunt intervale care se mai pot unifica o voi face
            // ex: ['10:00', '11:00'] si ['11:00', '12:00'] -> ['10:00','12:00']
            for (i = 0; i < newList.Count - 1; i++)
            {
                if (Utils.ifExistRange(Utils.findMinimumRange(newList.ElementAt(i), newList.ElementAt(i+1))))
                {
                    verifiedList.Add(Utils.findMaximumRange(newList.ElementAt(i), newList.ElementAt(i+1)));
                    i++;
                }
            }
            */
            
            return newList;
        }

        
        /**
         * restrange intervalul de intervale indisponibile
         * primeste lista de intervale in care cei 2 utilizatori nu sunt disponibili
         * primeste intervalul in care sunt la munca ambii utilizatori
         * returneaza lista de intervale indisponibile restransa
         */
        public static List<Interval> fitTheRange(List<Interval> lista, Interval range)
        {
            bool find = false;

            while (!find && lista.Count >= 1)
            {
                if (Utils.verifyCombination(lista.ElementAt(0), range))
                {
                    DateTime startTimeSalv = Utils.findMinimumRange(lista.ElementAt(0), range).startTime;
                    lista.ElementAt(0).startTime = startTimeSalv;
                    find = true;
                }
                else
                {
                    lista.RemoveAt(0);
                }
            }

            find = false;
            
            while (!find && lista.Count >= 1)
            {
                if (Utils.verifyCombination(lista.ElementAt(lista.Count-1), range))
                {
                    DateTime endTimeSalv = Utils.findMinimumRange(lista.ElementAt(lista.Count-1), range).endTime;
                    lista.ElementAt(lista.Count-1).endTime = endTimeSalv;
                    find = true;
                }
                else
                {
                    lista.RemoveAt(lista.Count-1);
                }
            }

            return lista;
        }
        

        /**
         * parcurge lista de intervale, calculeaza timpul dintre 2 intervale si verifica daca timpul dat este mai mic sau nu
         */
        public static List<Interval> findRanges(List<Interval> lista, int timeMeeting)
        {
            List<Interval> newList = new List<Interval>();
            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (Utils.timeDifference(lista.ElementAt(i), lista.ElementAt(i + 1)) >=
                    Utils.convertTimeMeetingInDateTime(timeMeeting))
                {
                    newList.Add(new Interval(lista.ElementAt(i).endTime, lista.ElementAt(i + 1).startTime));
                }
            }

            return newList;
        }
        
        
        
        static void Main(string[] args)
        {
            // citire date
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("The input data will have the form:");
            Console.WriteLine("FOR booked calendar: [['8:00','9:30'], ['10:30','11:30'], ['12:00','15:00'], ['15:00','17:00']]");
            Console.WriteLine("FOR calendar range limits: ['8:00','17:30']");
            Console.WriteLine("FOR meetingTime: 110");
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            
            Console.WriteLine("booked calendar1: ");
            String calendar1 = Console.ReadLine();
            
            Console.WriteLine("calendar1 range limits:");
            String calendar1range = Console.ReadLine();
            
            Console.WriteLine("booked calendar2: ");
            String calendar2 = Console.ReadLine();
            
            Console.WriteLine("calendar2 range limits:");
            String calendar2range = Console.ReadLine();
            
            Console.WriteLine("Meeting Time Minutes:");
            int timeMeeting = Convert.ToInt32(Console.ReadLine());
            
            // transforma stringurile primite in lista de intervale
            List<Interval> listInterval1 = convertCalendar(calendar1);
            List<Interval> listInterval2 = convertCalendar(calendar2);
            
            // obtine intervalul comun din cele 2 range uri
            Interval interval = Utils.findMinimumRange(Utils.convertRange(calendar1range), Utils.convertRange(calendar2range));

            //obtine lista de intervale disponibile
            List<Interval> lista =
                findRanges(fitTheRange(listOfInaccessibleRange(listInterval1, listInterval2), interval), timeMeeting);
            
            // afisare
            if (lista.Count >= 1)
            {
                foreach (Interval i in lista)
                {
                    Console.WriteLine(i.startTime + " | " + i.endTime);
                }
            }
            else
            {
                Console.WriteLine("Nu s-a gasit niciun interval disponibil cu aceasta dimensiune");
            }
        }
    }
}
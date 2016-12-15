# Era
Natural language date &amp; time parser for .NET

# Quick Start
    DateTime dateTime;
    Console.WriteLine(Parser.TryParse("tomorrow", out dateTime));
    Console.WriteLine(dateTime);
    
# What does it support?
The best place to find out what grammar is supported is to look at the test cases within the Era.Facts project, but the following is some of the support grammar;
* 11/10/1978
* 11/10/78
* 11/Oct/78
* 11-Oct-78
* 11.Oct.78
* 10 Sep 1978
* 10/sep/1978
* 12
* 12/5
* Sep
* September
* Sep 2002
* September 2002
* Sep/2002
* September/2002
* 1978
* 1978-Sep
* 1978-Sep-10
* 3:45:54 AM
* 3:45:54AM
* 3:45 AM
* 3:45AM
* 3 AM
* 23:45:54
* 23:45
* 12 AM
* 12 PM
* 11/10/1978 3 PM
* 11/10/1978 3:45 PM
* 11/10/1978 3:45:54 PM
* Sep 3 PM
* now
* today
* tomorrow
* yesterday
* sunday
* monday
* tuesday
* wednesday
* thursday
* friday
* saturday
* monday 2am


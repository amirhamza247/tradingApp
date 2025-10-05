using System.Buffers;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace App;


public class Person
{
  //////// MY SPECIAL METHODS //////////

  //this methods converts Readline and Writeline to Print and Input.
  static void print(string input)
  {
    Console.WriteLine(input);
  }
  static string input()
  {
    return Console.ReadLine() ?? "";
  }


  //this method gives text in console color.
  static void paint(ConsoleColor color, string input, string newline = "newline")
  {
    if (newline == "newline")
    {
      Console.ForegroundColor = color;
      print(input);
      Console.ResetColor();
    }
    if (newline == "sameline")
    {
      Console.ForegroundColor = color;
      Console.Write(input);
      Console.ResetColor();
    }
  }

  //this is to help vs code with debugging.
  // try { Console.Clear(); } catch { Console.WriteLine("\n------------------\n");}

  /////////^^^^^^^^^ MY SPECIAL METHODS ^^^^^^^^^//////////
  public string Name;
  public string Email;
  public string _Password;
  public List<Items> myItemsList;


  public Person(string name, string email, string _password)
  {
    Name = name;
    Email = email;
    _Password = _password;
    myItemsList = new List<Items>();
  }

  public bool tryLogin(string email, string password)
  {
    return email == Email && password == _Password;
  }

  public string getEmail()
  {
    return Email;
  }

  public void ShowOutgoingRequests()
  {
    return;
  }

}
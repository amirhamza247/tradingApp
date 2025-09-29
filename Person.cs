using System.ComponentModel;
using System.Reflection.Metadata;

namespace App;

public class Person : IUser
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
  public List<Items> myItems;


  public Person(string name, string email, string _password)
  {
    Name = name;
    Email = email;
    _Password = _password;
    myItems = new List<Items>();
  }

  public bool tryLogin(string email, string password)
  {
    return email == Email && password == _Password;
  }

  public string getEmail()
  {
    return Email;
  }
  public void uploadItem(string name, string description, string id)
  {
    Items newItem = new Items(name, description, id);
    myItems.Add(newItem);
    Console.Write("\nItem ");
    paint(ConsoleColor.Green, $"{newItem.Name}", "sameline");
    Console.Write(" has been successfully uploaded!\n");
  }

  public void ShowMenu()
  {
    Menu activeMenu = new Menu();
    bool personLogedin = true;
    while (personLogedin)
    {
      switch (activeMenu)
      {
        case Menu.Main:
          try { Console.Clear(); } catch { print("---------------"); }
          paint(ConsoleColor.DarkYellow, "\n-----Welcome to Trade World-----\n");
          paint(ConsoleColor.DarkYellow, "\n_____________________\nMain menu:\n[1] Show my Items\n[2] Upload Item\n[3] Trade\n[4] Logout\n_____________________\n");
          paint(ConsoleColor.DarkYellow, "\n► ", "sameline");

          switch (input())
          {
            case "1":
              print("You have no items yet..");
              print("press enter to continue:");
              input();
              break;

            case "2":
              print("what do you want to upload...");
              print("press enter to continue:");
              input();
              break;

            case "3":
              print("You have no items to Trade yet..");
              print("press enter to continue:");
              input();
              break;

            case "4":
              personLogedin = false;
              break;

            default:
              print("please enter a valid option...");
              break;

          }
          break;

        case Menu.ShowMyItems:
          break;

        case Menu.UploadItem:
          break;

        case Menu.Trade:
          break;

        case Menu.Logout:
          break;

        default:
          print("Please enter a valid option...");
          break;


      }
    }

  }
}
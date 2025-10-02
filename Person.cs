using System.ComponentModel;
using System.Globalization;
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

  public void uploadItem()
  {
    Console.Write("\nWrite the name of your item: ");
    string itemName = input().ToLower().Trim();
    Console.Write("\nWrite the description of your item: ");
    string itemDescription = input().ToLower().Trim();

    Items newItem = new Items(itemName, itemDescription);
    myItemsList.Add(newItem);
    Console.Write("\nItem ");
    paint(ConsoleColor.Green, $"{newItem.Name}", "sameline");
    Console.Write(" has been successfully uploaded!\n");
  }



  public void ShowMyItems()
  {
    paint(ConsoleColor.DarkYellow, "\nItems in your inventory:");
    foreach (Items item in myItemsList)
    {
      print($"\nName: {item.Name}\nDescription: {item.Description}\n\n");
    }
  }

  public void ShowAllTradeItems(List<IUser> allUsers)
  {
    paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading in trade world:");
    int itemNumber = 1;
    foreach (IUser person in allUsers)
    {
      if (person is Person p && p.myItemsList?.Count > 0)
      {
        paint(ConsoleColor.DarkYellow, $"\n--- {p.Name}'s Inventory ---");
        foreach (Items currentItem in p.myItemsList)
        {
          print($"[{itemNumber}] Name: {currentItem.Name}\nDescription: {currentItem.Description}\n");
          itemNumber++;
        }

        if (itemNumber == 1)
        { print("There is no items currently avalible for trade."); }
      }
    }
  }



  public void ShowMenu(List<IUser> allUsers)
  {
    Menu activeMenu = new Menu();
    bool personLogedin = true;
    while (personLogedin)
    {
      switch (activeMenu)
      {
        case Menu.Main:
          try { Console.Clear(); } catch { print("---------------"); }
          paint(ConsoleColor.DarkYellow, $"\n-----Welcome to Trade World-----");
          paint(ConsoleColor.DarkYellow, "_____________________\nMain menu:\n[1] Show my Items\n[2] Upload Item\n[3] Trade\n[4] Logout\n_____________________\n");
          paint(ConsoleColor.DarkYellow, "\n► ", "sameline");

          switch (input())
          {
            case "1":
              activeMenu = Menu.ShowMyItems;
              break;

            case "2":
              activeMenu = Menu.UploadItem;

              break;

            case "3":
              activeMenu = Menu.Trade;
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
          ShowMyItems();

          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Main;
          break;

        case Menu.UploadItem:
          uploadItem();

          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Main;
          break;

        case Menu.Trade:
          ShowAllTradeItems(allUsers);
          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Main;
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
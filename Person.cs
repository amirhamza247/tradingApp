using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

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

  public void ShowAllTradeItems(List<IUser> allUsers, Person activePerson)
  {
    paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading in trade world:");
    int itemNumber = 1;
    foreach (IUser person in allUsers)
    {
      if (person is Person p && p.myItemsList?.Count > 0)
      {
        if (p == activePerson)
        {
          paint(ConsoleColor.DarkYellow, $"\n--- Your Inventory {p.Name} ---");
        }
        else
        {
          paint(ConsoleColor.DarkYellow, $"\n--- {p.Name}'s Inventory ---");
        }
        foreach (Items currentItem in p.myItemsList)
        {
          print($"[{itemNumber}] Name: {currentItem.Name}\nDescription: {currentItem.Description}\n");
          itemNumber++;
        }
      }
    }
    if (itemNumber == 1)
    { print("There is no items currently avalible for trade."); }
  }

  public void SelectAndSendRequest(List<IUser> allUsers, List<TradeRequest> allRequests)
  {
    if (myItemsList.Count == 0)
    {
      paint(ConsoleColor.Red, "\nYou must upload at least one item before sending a trade request");
      return;
    }
    paint(ConsoleColor.DarkYellow, "Write **number** of the item you want to trade with: ", "sameline");
    if (!int.TryParse(input(), out int targetItemNumber))
    {
      print("Invalid item number.");
      return;
    }

    int currentItemCount = 1;
    Person targetOwner = null;
    Items targetItem = null;

    foreach (IUser currentPerson in allUsers)
    {
      if (currentPerson is Person p && p.myItemsList.Count > 0)
      {
        foreach (Items item in p.myItemsList)
        {
          if (currentItemCount == targetItemNumber)
          {
            if (p == this)
            {
              paint(ConsoleColor.Red, "\nYou cannot trade for your own item. Please select an item from another user.");
              return;
            }
            targetOwner = p;
            targetItem = item;
            break;
          }
          currentItemCount++;
        }
      }
      if (targetItem != null) { break; }
    }

    if (targetItem == null)
    {
      print("Item not found...");
      return;
    }

    Console.Write("\nYou selected: ");
    paint(ConsoleColor.DarkYellow, $"{targetItem.Name} ", "sameline");
    paint(ConsoleColor.DarkYellow, $"\n\nNow, which of your items will you offer? Enter 1-{myItemsList.Count}: ", "sameline");
    if (!int.TryParse(input(), out int offerIndex) || offerIndex < 1 || offerIndex > myItemsList.Count)
    {
      print("\nInvalid offer item number."); return;
    }

    Items offerItem = myItemsList[offerIndex - 1];

    TradeRequest request = new TradeRequest(requester: this, requesterItem: offerItem, owner: targetOwner, ownerItem: targetItem);

    allRequests.Add(request);
    Console.Write("\nTrade request sent! Offering your:");
    paint(ConsoleColor.DarkYellow, $" [{offerItem.Name}] ", "sameline");
    Console.Write("for");
    paint(ConsoleColor.DarkYellow, $" [{targetItem.Name}]");

  }



  public void ShowMenu(List<IUser> allUsers, List<TradeRequest> allRequests)
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
          try { Console.Clear(); } catch { print("---------------"); }
          paint(ConsoleColor.DarkYellow, "\n--- Trade Menu ---\n\n[1] Show all items avalible for trade \n[2] My requests \n[3] Others requests \n[4] Trade history \n[5] Back to Main Menu\n  ");
          switch (input())
          {

            case "1":
              activeMenu = Menu.TradeMarket;
              break;

            case "2":
              activeMenu = Menu.MyRequests;
              break;

            case "3":
              activeMenu = Menu.OthersRequest;
              break;

            case "4":
              activeMenu = Menu.TradeHistory;
              break;

            case "5":
              activeMenu = Menu.Main;
              break;

            default:
              print("Please enter a valid option...");
              break;
          }
          break;
        case Menu.TradeMarket:
          try { Console.Clear(); } catch { print("---------------"); }
          ShowAllTradeItems(allUsers, this);
          SelectAndSendRequest(allUsers, allRequests);

          print("\nTo go to Trade menu, press enter...");
          input();
          activeMenu = Menu.Trade;
          break;

        case Menu.MyRequests:
          try { Console.Clear(); } catch { print("---------------"); }
          print("\nno requests yet.");
          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Trade;
          break;

        case Menu.OthersRequest:
          try { Console.Clear(); } catch { print("---------------"); }
          print("\nno requests yet.");
          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Trade;
          break;

        case Menu.TradeHistory:
          try { Console.Clear(); } catch { print("---------------"); }
          print("\nno trades made yet.");
          print("\nTo go to main menu press enter...");
          input();
          activeMenu = Menu.Trade;
          break;

        case Menu.Logout:
          break;

        default:
          print("\nPlease enter a valid option...");
          break;
      }
    }

  }
}
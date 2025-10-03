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

  public void ShowAllTradeItems(List<Person> allUsers, Person activePerson)
  {
    paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading in trade world:");
    int userIndex = 1;
    foreach (Person p in allUsers)
    {
      if (p.myItemsList?.Count > 0)
      {
        if (p == activePerson)
        {
          paint(ConsoleColor.DarkYellow, $"\n--- Your Inventory {p.Name} ---");
        }
        else
        {
          paint(ConsoleColor.DarkYellow, $"\n--- [{userIndex}] {p.Name}'s Inventory ---");
        }
        int localItemNumber = 1;

        foreach (Items currentItem in p.myItemsList)
        {
          print($"[{localItemNumber}] Name: {currentItem.Name}\nDescription: {currentItem.Description}\n");
          localItemNumber++;
        }

      }
    }
    if (userIndex == 1 && activePerson.myItemsList.Count == 0)
    { print("There is no items currently avalible for trade."); }
  }
  /*   public void ShowAllTradeItems(List<Person> allUsers, Person activePerson)
    {
      paint(ConsoleColor.DarkYellow, "\nItems avaliable for trading in trade world:");
      int itemNumber = 1;
      foreach (Person person in allUsers)
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
    } */

  public void SelectAndSendRequest(List<Person> allUsers, List<TradeRequest> allRequests, Person active_user)
  {
    if (myItemsList.Count == 0)
    {
      paint(ConsoleColor.Red, "\nYou must upload at least one item before sending a trade request");
      return;
    }


    paint(ConsoleColor.DarkYellow, "\n--- Trade Selection --\n");
    paint(ConsoleColor.DarkYellow, "Write **number** for the item you want to trade with: ", "sameline");
    if (!int.TryParse(input(), out int targetUserNumber))
    {
      print("Invalid item number.");
      return;
    }


    Person targetOwner = null;
    int currentUserIndex = 1;


    foreach (Person p in allUsers)
    {
      if (p != this && p.myItemsList?.Count > 0)
      {
        if (currentUserIndex == targetUserNumber)
        {
          targetOwner = p;
          break;
        }
      }

    }

    if (targetOwner == null)
    {
      print("User not found or user number out of range..");
    }

    int maxItemIndex = targetOwner.myItemsList.Count;
    paint(ConsoleColor.DarkYellow, $"\nYou selected {targetOwner.Name}'s inventory.\nNow, which of their items (1-{maxItemIndex}) do you want to trade for? ");
    if (!int.TryParse(input(), out int targetItemIndex) || targetItemIndex < 1 || targetItemIndex > maxItemIndex)
    {
      paint(ConsoleColor.Red, "Invalid item number.");
      return;
    }
    Items targetItem = targetOwner.myItemsList[targetItemIndex - 1];

    Console.Write("\nYou selected :");
    paint(ConsoleColor.DarkYellow, $"{targetItem.Name}", "sameline");
    paint(ConsoleColor.DarkYellow, $"\n\nNow, which of you items will you offer? Enter 1-{myItemsList.Count}: ");

    if (!int.TryParse(input(), out int offerIndex) || offerIndex < 1 || offerIndex > myItemsList.Count)
    {
      paint(ConsoleColor.Red, "Invalid offer item number."); return;
    }
    Items offerItem = myItemsList[offerIndex - 1];

    TradeRequest request = new TradeRequest(requester: this, requesterItem: offerItem, owner: targetOwner, ownerItem: targetItem);
    allRequests.Add(request);

    Console.Write("\nTrade request sent! Offering your:");
    paint(ConsoleColor.DarkYellow, $" [{offerItem.Name}] ", "sameline");
    Console.Write("for");
    paint(ConsoleColor.DarkYellow, $" [{targetItem.Name}]");
    Console.Write("from");
    paint(ConsoleColor.DarkYellow, $" [{targetOwner.Name}]");

  }


  public void ProcessRequest(TradeRequest selectedRequest, List<TradeRequest> allRequests)
  {
    try { Console.Clear(); } catch { print("---------------"); }

    if (selectedRequest.Status != TradeStatus.Pending || selectedRequest.Owner != this)
    {
      paint(ConsoleColor.Red, "Cannot process this request (either status is not pending or you are not the owner).");
      print("Press enter to go to previous menu...");
      input();
      return;
    }

    paint(ConsoleColor.DarkYellow, "\nPlease select an option:");
    paint(ConsoleColor.DarkYellow, "\n[1] Accept \n[2] Deny \n[3] Previous menu");
    switch (input())
    {
      case "1":
        selectedRequest.Status = TradeStatus.Accepted;
        paint(ConsoleColor.Green, "Let's gooooo, Trade completeted succesfully!");

        Person requester = selectedRequest.Requester;

        this.myItemsList.Remove(selectedRequest.OwnerItem);
        requester.myItemsList.Add(selectedRequest.OwnerItem);


        requester.myItemsList.Remove(selectedRequest.RequesterItem);
        this.myItemsList.Add(selectedRequest.RequesterItem);

        Items TadeItem1 = selectedRequest.OwnerItem;
        Items TadeItem2 = selectedRequest.RequesterItem;

        foreach (TradeRequest request in allRequests)
        {

          if (request.Status == TradeStatus.Pending && request != selectedRequest)
          {
            if (request.OwnerItem == TadeItem1 || request.OwnerItem == TadeItem2 || request.RequesterItem == TadeItem1 || request.RequesterItem == TadeItem2)
            {
              request.Status = TradeStatus.Denied;
              paint(ConsoleColor.Red, $"\nConflicting request denined:  {request.Requester.Name} for {request.OwnerItem.Name}");
            }
          }
        }


        print("Press enter to go to previous menu...");
        input();
        break;

      case "2":
        selectedRequest.Status = TradeStatus.Denied;
        paint(ConsoleColor.Red, "Trade requst denied.");

        print("Press enter to go to previous menu...");
        input();
        break;

      case "3":
        break;

      default:
        print("Invalid selection, Press enter to go to previous menu...");
        print("Press enter to go to previous menu...");
        input();
        break;
    }
    return;
  }



  public void ShowIncomingRequests(List<TradeRequest> allRequests, Person activeUser)
  {
    try { Console.Clear(); } catch { print("---------------"); }

    Person owner = this;
    List<TradeRequest> pendingRequestsToMe = new List<TradeRequest>();
    foreach (TradeRequest request in allRequests)
    {
      if (request.Owner == owner && request.Status == TradeStatus.Pending)
      {
        pendingRequestsToMe.Add(request);
      }
    }
    if (pendingRequestsToMe.Count == 0)
    {
      print("You have no pending trade requests...");
      print("\nPress enter to go to previous menu.");
      input();
      return;
    }

    paint(ConsoleColor.DarkYellow, $"\n --- Incoming Trade Request ({pendingRequestsToMe.Count} pedning) ---");

    int requestNumber = 1;
    foreach (TradeRequest request in pendingRequestsToMe)
    {
      print($"\n[{requestNumber}] From: {request.Requester.Name}");
      print($"    Wants: {request.OwnerItem.Name} (your item)");
      print($"    Offers: {request.RequesterItem.Name} ");
      requestNumber++;
    }

    paint(ConsoleColor.DarkYellow, "\nWrite the *number* of the request you want to ACCEPt or Deny, or press enter to go back: ", "sameline");
    string inputChoice = input();

    if (string.IsNullOrWhiteSpace(inputChoice)) return;

    if (int.TryParse(inputChoice, out int choiceIndex) && choiceIndex >= 1 && choiceIndex <= pendingRequestsToMe.Count)
    {
      TradeRequest selectedRequest = pendingRequestsToMe[choiceIndex - 1];
      ProcessRequest(selectedRequest, allRequests);

    }
    else
    {
      paint(ConsoleColor.Red, "Invalid selection..");
      input();
    }
  }


  public void ShowOutgoingRequests()
  {
    return;
  }



  public void ShowMenu(List<Person> allUsers, List<TradeRequest> allRequests)
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
          try { Console.Clear(); } catch { print("---------------"); }

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
          SelectAndSendRequest(allUsers, allRequests, this);

          paint(ConsoleColor.DarkYellow, "\nTo go to Trade menu, press enter...");
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
          ShowIncomingRequests(allRequests, this);

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
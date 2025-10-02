namespace App;

public class TradeRequest
{

  public Person Requester;
  public Items RequesterItem;
  public Person Owner;
  public Items OwnerItem;

  public TradeStatus Status = TradeStatus.Pending;

  public TradeRequest(Person requester, Items requesterItem, Person owner, Items ownerItem)
  {
    Requester = requester;
    RequesterItem = requesterItem;
    Owner = owner;
    OwnerItem = ownerItem;
  }



}
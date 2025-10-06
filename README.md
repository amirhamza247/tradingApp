# tradingApp
This program is preloaded with demo users and items.
Instructions on how to navigate and use the tradingApp!


1. You have to creat an account to be able to trade, that is possiable by pressing 2.
Or you can login by using one of the alreading exsisting users: 
- email: amir@mail.com   password: amir
- email: max@mail.com    password: max
- email: jakob@mail.com  password: jakob

I chose to not have any special requirment for password to keep things simple.
2. After you have created an account you can log in by pressing 1.

3. When you are logedin, you can do following things:
- View you inventory by pressing 1.
- Upload an item to your inventory by pressing 2.
- Open Trade menu by pressing 3.
- Logout by pressing 4.

4. If you want to go to trade menu than select 3.
In your trade menu you can do following things:
- Show all items avalible for trade in Trade market by pressing 1.
- View trade requests that you have sent to others by pressing 2.
- View Incoming trade requests from others by pressing 3. 
- View you completed trade history by pressing 4.

5. To exit the program go back until you are in main menu than select exit by pressing 3.








Summary of what choices i have made and why....

What I have used:
In this project I have  made consle based Trading app. I have used Composition because i had to build complex objects for exemple, In my Person class I have a list (myItemsList) from Items class and in my TradeRequest class I have objects both from Perosn and Items class. 

I have also used Encapsulation by gathering data as in Person.Name, Person.Email and methods that applies on the data such as Person.tryLogin(). My getter method person.getEmail() is another example of encaspulation.

I have used Enums to make my code more cleaner and safer for controlling menus and tradestatus.



What I have not used:
I decided not to use inheritance because in would make my classes more bound to complex hierarchy which would lead to harder code to maintain.

I have not used Interfaces because I did not need it. I might have used interfaces if I needed same property or same method in multiple class but in this case composition worked just fine, because I could directly access necessary properties, with out needing to do complex type casting.
# Rocket-Elevators-Csharp-Controller
This is the C# commercial controller program.

### This controller is capable of supporting two main events:

1. A person presses a call button on a floor to request an elevator. The controller selects an available cage and it is routed to that person based on two parameters provided by pressing the button:
- The floor where the person is
- The direction in which he will go (up or down)
2. A person at the Lobby requests a floor and is sent to the correct column. The parameters provided are :
- The floor where the user want to go
- The direction in which he will go (up or down)

In the scenarios provided, we use a building of 66 floors including 6 basements served by 4 columns of 5 cages each. The floors are separated amongst the columns in the following way: B6 to B1, 2 to 20, 21 to 40, 41 to 60. All the columns serve the 1st floor (Lobby). There are no floor buttons inside the elevators. Instead, there is a panel at the Lobby with which the users select where they want to go.

### Example of scenario

Scenario 1:
Elevator B1 at 20th floor going to the 5th floor
Elevator B2 at 3rd floor going to the 15th floor
Elevator B3 at 13th floor going to Lobby
Elevator B4 at 15th floor going to the 2nd floor
Elevator B5 at 6th floor going to Lobby

Someone at Lobby wants to go to the 20th floor.
Elevator B5 is expected to be sent.


### Running the tests

To launch the tests, make sure to be at the root of the repository and run:

`dotnet test`


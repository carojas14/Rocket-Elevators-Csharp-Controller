using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {

        //instance variable
        public int ID;
        public string status;
        public int amountOfFloors;
        public int _amountOfElevators;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public List<int> servedFloorsList;
        public bool isBasement;
        private int elevatorID = 1;

        public Column(int _ID,string _status, int _amountOfFloors ,int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            this.ID = _ID;
            this.status = _status;
            this._amountOfElevators = _amountOfElevators;
            this.elevatorsList = new List<Elevator>();
            this.callButtonsList = new List<CallButton>();
            this.servedFloorsList = _servedFloors;
            this.isBasement = _isBasement;

            createElevators(_amountOfFloors, _amountOfElevators);
            createCallButtons(_amountOfFloors, _isBasement);
        }


        /**
        * Create a list of call buttons for each column using CallButton class
        **/
        public void createCallButtons(int _amountOfFloors, bool _isBasement)
        {
            if(_isBasement)
            {
                int buttonFloor = -1;
                for (int i = 0; i < _amountOfFloors; i++)
                {
                    CallButton callButton = new CallButton(buttonFloor, "up");
                    this.callButtonsList.Add(callButton);
                    buttonFloor--;
                }
            }
            else
            {
                int buttonFloor = 1;
                for (int i = 0; i < _amountOfFloors; i++)
                {
                    CallButton callButton = new CallButton(buttonFloor, "down");
                    this.callButtonsList.Add(callButton);
                    buttonFloor++;
                }
            }
        }

        /**
        * Create a list of elevators for each column using Elevator class
        **/
        public void createElevators(int _amountOfFloors, int _amountOfElevators)
        {
            for (int i = 0; i < _amountOfElevators; i++)
            {
                Elevator elevator = new Elevator(this.elevatorID);
                this.elevatorsList.Add(elevator);
                this.elevatorID++;

            }

        }


        /**
        * Simulate when a user press a button on a floor to go back to the first floor
        **/
        public Elevator requestElevator(int userPosition, string direction)
        {
            Elevator elevator = this.findElevator(userPosition, direction);
            elevator.addNewRequest(userPosition);
            elevator.move();
            elevator.addNewRequest(1);
            elevator.move();
            return elevator;
        }


        /*
        * Find the best elevator, prioritizing the elevator that is already moving,
        * that is closer to the user's floor and that goes to the same direction that user wants
        */
        public Elevator findElevator(int requestedFloor, string requestedDirection)
        {
            Elevator bestElevator = null;
            int bestScore = 6;
            int referenceGap = 10000000;
            Tuple<Elevator, int, int> bestElevatorInformations;

            //If requested floor is the lobby
            if (requestedFloor == 1)
            {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed.
                    if (1 == elevator.currentFloor && elevator.status == "stopped")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is at the lobby and has no requests.
                    else if (1 == elevator.currentFloor && elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is lower than user and is coming up. It means that user is requesting an elevator to go to a basement, and the elevator is on same way to user.
                    else if (1 > elevator.currentFloor && elevator.direction == "up")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is above user and is coming down. It means that user is requesting an elevator to go to a floor, and the elevator is on same way to user.
                    else if (1 < elevator.currentFloor && elevator.direction == "down")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not at the first floor, but doesn't have any request.
                    else if (elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not available, but still could take the call if nothing better is found.
                    else
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.Item1;
                    bestScore = bestElevatorInformations.Item2;
                    referenceGap = bestElevatorInformations.Item3;
                }
            }
            //Rquested floor is not lobby
            else
            {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    //The elevator is at the same level as user, and is about to depart to the first floor
                    if (requestedFloor == elevator.currentFloor && elevator.status == "stopped" && requestedDirection == elevator.direction)
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is lower than user and is going up. User is on a basement, and the elevator can pick user up on it's way
                    else if (requestedFloor > elevator.currentFloor && elevator.direction == "up" && requestedDirection == "up")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is higher than user and is going down. User is on a floor, and the elevator can pick user up on it's way
                    else if (requestedFloor < elevator.currentFloor && elevator.direction == "down" && requestedDirection == "down")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is idle and has no requests
                    else if (elevator.status == "idle")
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not available, but still could take the call if nothing better is found
                    else
                    {
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    bestElevator = bestElevatorInformations.Item1;
                    bestScore = bestElevatorInformations.Item2;
                    referenceGap = bestElevatorInformations.Item3;
                }
            }
            return bestElevator;
        }


        /**
        * Called by findElevator to compare current elevator in elevatorList with
        * other elevators and return best elevator.
        **/
        public Tuple<Elevator, int, int> checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, int bestScore, int referenceGap, Elevator bestElevator, int floor)
        {
            if (scoreToCheck < bestScore)
            {
                bestScore = scoreToCheck;
                bestElevator = newElevator;
                referenceGap = Math.Abs(newElevator.currentFloor - floor);
            }
            else if (bestScore == scoreToCheck)
            {
                int gap = Math.Abs(newElevator.currentFloor - floor);
                if (referenceGap > gap)
                {
                    bestElevator = newElevator;
                    referenceGap = gap;
                }
            }
            var elevatorIsBetter = new Tuple<Elevator, int, int>(bestElevator, bestScore, referenceGap);

            return elevatorIsBetter;
        }

    }
}
using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        //instance variable
        public int ID = 1;
        public string status;
        public int currentFloor;
        public int amountOfFloors;
        public int amountOfElevators;
        public string direction;
        public Door door;
        public List<int> floorRequestsList;
        public List<int> completedRequestsList;

        public Elevator(int _elevatorID)
        {
            this.ID = _elevatorID;
            this.status = "idle";
            this.currentFloor = 1;
            this.door = new Door(ID);
            this.floorRequestsList = new List<int>();
            this.direction = "none";
            this.completedRequestsList = new List<int>();

        }

        /**
        * Move elevator to requested floor
        **/
        public void move()
        {
            while (this.floorRequestsList.Count != 0)
            {
                this.status = "moving";
                this.sortFloorList();
                int destination = floorRequestsList[0];
                //Elevator position is lower than requested floor
                if (this.direction == "up")
                {
                    while (currentFloor < destination)
                    {
                        this.currentFloor++;
                    }
                }
                else if (this.direction == "down")
                {
                    while (currentFloor > destination)
                    {
                        this.currentFloor--;
                    }
                }
                this.status = "stopped";
                this.operateDoors();
                this.completedRequestsList.Add(floorRequestsList[0]);
                this.floorRequestsList.RemoveAt(0);
            }
            this.status = "idle";
        }

        public void sortFloorList()
        {
            if (this.direction == "up")
            {
                this.floorRequestsList.Sort();
            }
            else
            {
                this.floorRequestsList.Reverse();
            }
        }

        /**
        * Manage doors
        **/
        public void operateDoors()
        {
            this.door.status = "opened";
            if (this.door.status != "overweight")
            {
                this.door.status = "closing";
                if (this.door.status != "obstruction")
                {
                    this.door.status = "closed";
                }
                else
                {
                    this.operateDoors();
                }
            }
        }

        /**
        * User press a button outside the elevator
        **/
        public void addNewRequest(int _requestedFloor)
        {
            if (!this.floorRequestsList.Contains(_requestedFloor))
            {
                floorRequestsList.Add(_requestedFloor);
            }
            if (this.currentFloor < _requestedFloor)
            {
                this.direction = "up";
            }
            if (this.currentFloor > _requestedFloor)
            {
                this.direction = "down";
            }
        }
    }
}
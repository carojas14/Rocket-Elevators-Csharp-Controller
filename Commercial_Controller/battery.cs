using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {

        //instance variable
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfColumns;
        public int amountOfBasements;
        public int amountOfElevatorPerColumn;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestsButtonsList;
        private int columnID = 1;
        private int floorRequestButtonID = 1;


        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _ID;
            this.status = "online";
            this.amountOfColumns = _amountOfColumns;
            this.amountOfFloors = _amountOfFloors;
            this.amountOfBasements = _amountOfBasements;
            this.amountOfElevatorPerColumn = _amountOfElevatorPerColumn;
            this.columnsList = new List<Column>();
            this.floorRequestsButtonsList = new List<FloorRequestButton>();


            if (_amountOfBasements > 0)
            {
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                this.amountOfColumns--;
            }

            this.createFloorRequestButtons(amountOfFloors);
            this.createColumns(amountOfColumns, amountOfFloors, amountOfBasements, amountOfElevatorPerColumn);

        }

        /**
        * Create columns for basement floors used Column class
        **/
        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            List<int> servedFloors = new List<int>();
            int floor = -1;
            for (int i = 0; i < _amountOfBasements; i++)
            {
                servedFloors.Add(floor);
                floor--;
            }

            Column basementColumn = new Column(this.columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(basementColumn);
            this.columnID++;
        }

        /**
        * Create columns used Column class
        **/
        public void createColumns(int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        {
            int amountOfFloorsPerColumn = (int)Math.Ceiling((double)amountOfFloors / amountOfColumns);
            int floor = 1;
            for (int i = 0; i < amountOfColumns; i++)
            {
                List<int> servedFloorsList = new List<int>();
                for (int x = 0; x < amountOfFloorsPerColumn; x++)
                {
                    if(floor <= this.amountOfFloors)
                    {
                        servedFloorsList.Add(floor);
                        floor++;
                    }
                }
                Column column = new Column(this.columnID, "online", amountOfFloors, amountOfElevatorPerColumn, servedFloorsList, false);
                this.columnsList.Add(column);
                this.columnID++;

            }
        }

        /**
        * Create buttons requests used FloorRequestButton class
        **/
        public void createFloorRequestButtons(int _amountOfFloors)
        {
            int buttonFloor = 1;
            for (int i = 0; i < _amountOfFloors; i++)
            {
                FloorRequestButton floorRequestButton = new FloorRequestButton(this.floorRequestButtonID, "off", buttonFloor, "up");
                this.floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor++;
                this.floorRequestButtonID++;
            }
        }


        /**
        * Create buttons requests for the basement floors used FloorRequestButton class
        **/
        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;
            for (int i = 0; i < _amountOfBasements; i++)
            {
                FloorRequestButton basementFloorRequestButton = new FloorRequestButton(this.floorRequestButtonID, "off", buttonFloor, "down");
                this.floorRequestsButtonsList.Add(basementFloorRequestButton);
                buttonFloor--;
                this.floorRequestButtonID++;
            }
        }


        /**
        * Find the best column to requested floor
        **/
        public Column findBestColumn(int requestedFloor)
        {
            Column bestColumn = null;
            foreach (Column column in this.columnsList)
            {
                if (column.servedFloorsList.Contains(requestedFloor))
                {
                    bestColumn = column;
                    break;
                }
            }
            return bestColumn;
        }

        /**
        * Simulate when a user press a button at the lobby
        **/
        public (Column, Elevator) assignElevator(int requestedFloor, string direction)
        {
            Column chosenColumn = this.findBestColumn(requestedFloor);

            Elevator chosenElevator = chosenColumn.findElevator(1, direction);

            chosenElevator.addNewRequest(1);

            chosenElevator.move();

            chosenElevator.addNewRequest(requestedFloor);

            chosenElevator.move();

            return(chosenColumn, chosenElevator);
        }


    }
}


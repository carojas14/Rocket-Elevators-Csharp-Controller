namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class CallButton
    {
        public int ID;
        public string status;
        public int floor;
        public string direction;
        public CallButton(int _floor, string _direction)
        {
            this.ID = 1;
            this.status = "online";
            this.floor = _floor;
            this.direction = _direction;
        }
    }
}
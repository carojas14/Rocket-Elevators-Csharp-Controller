namespace Commercial_Controller
{
    public class Door
    {
        public int ID;
        public string status;
        public Door(int _ID)
        {
            this.ID = _ID;
            this.status = "closed";
        }
    }
}
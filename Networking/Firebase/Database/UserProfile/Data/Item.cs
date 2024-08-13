namespace AXitUnityTemplate.Networking.Firebase.Database.UserProfile.Data
{
    public class Item
    {
        public string ItemId   { get; set; }
        public string Name     { get; set; }
        public string Type     { get; set; }
        public int    Quantity { get; set; }

        public Item(string itemId, string name, string type, int quantity)
        {
            this.ItemId   = itemId;
            this.Name     = name;
            this.Type     = type;
            this.Quantity = quantity;
        }
    }
}
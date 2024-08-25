namespace AXitUnityTemplate.Networking.FirebaseNetwork.Database.UserProfile.Data
{
    public class Item
    {
        public string ItemId   { get; set; }
        public string Name     { get; set; }
        public string Type     { get; set; }
        public int    Quantity { get; set; }
        
        public Item() { }

        public Item(string itemId, string name, string type, int quantity)
        {
            this.ItemId   = itemId;
            this.Name     = name;
            this.Type     = type;
            this.Quantity = quantity;
        }
    }
}
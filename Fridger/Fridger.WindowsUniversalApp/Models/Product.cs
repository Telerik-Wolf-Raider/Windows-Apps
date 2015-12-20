namespace Fridger.WindowsUniversalApp.Models
{
    using SQLite.Net.Attributes;

    public class Product
    {       
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        
        /// <summary>
        /// if null - it is not present in the fridge but should not be bought
        /// </summary>
        public bool? ShouldBeBougth { get; set; }

        public override string ToString()
        {
            return $"#{this.Id}; Name: {this.Name}; Should be bougth: {this.ShouldBeBougth}";
        }
    }
}

namespace MilkStore.API.Models.StorageModel
{
    public class RequestCreateStorageModel
    {
        public int StorageID { get; set; }

        public string StorageName { get; set; } = null!;
        public int Delete { get; set; }
    }
}

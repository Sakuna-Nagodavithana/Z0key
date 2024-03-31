namespace Z0key.Models
{
    public class ReturnKey
    {
        public string Key {get;}
        public DateTime LastModifiedTime {get;}

        public ReturnKey(string key, DateTime lastModifiedTime)
        {
            this.Key = key;
            this.LastModifiedTime = lastModifiedTime;
        }


    }
}

namespace zapzap.common.Entities
{
    using System.Collections.Generic;

    public class Group
    {
        public Group()
        {
            ConnectedUsers = new List<User>();
        }

        public string Name { get; set; }
        public List<User> ConnectedUsers { get; set; }
    }
}

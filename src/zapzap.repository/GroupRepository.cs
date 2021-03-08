namespace zapzap.repository
{
    using System.Collections.Generic;
    using zapzap.common.Entities;
    using zapzap.common.Repositories;
    using System.Linq;
    using zapzap.common;

    public class GroupRepository : IGroupRepository
    {
        private List<Group> _groupList = new List<Group>();

        public GroupRepository()
        {
            Create(Constants.Messages.DefaultGroupName);
        }

        public void Create(string name)
        {
            _groupList.Add(new Group() { Name = name });
        }

        public Group GetByName(string name)
        {
            return _groupList.FirstOrDefault(x => x.Name.Equals(name));
        }

        public List<Group> ListAll()
        {
            return _groupList;
        }

        public void Remove(Group group)
        {
            _groupList.Remove(group);
        }

        public void AddUserInGroup(Group group, User user)
        {
            _groupList.FirstOrDefault(x => x.Equals(group)).ConnectedUsers.Add(user);
        }

        public Group GetGroupBySocket(string sockeId)
        {
            foreach (Group group in _groupList)
            {
                if (group.ConnectedUsers.Any(x => x.SocketID.Equals(sockeId)))
                    return group;
            }
            return default;
        }

        public Group GetGroupByUser(string userName)
        {
            foreach (Group group in _groupList)
            {
                if (group.ConnectedUsers.Any(x => x.Name.Equals(userName)))
                    return group;
            }
            return default;
        }

        public void RemoveUser(Group group, User user)
        {
            group.ConnectedUsers.Remove(user);
        }
    }
}

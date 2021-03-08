namespace zapzap.common.Services
{
    using System.Collections.Generic;
    using zapzap.common.Entities;

    public interface IGroupServices
    {
        void CreateNewGroup(string newGroupName);
        void PutUserInGroup(Group group, User user);
        void PutUserInGroup(string groupName, User user);
        List<User> GetConnectedUserByGroup(string groupName);
        Group GetGroupBySocket(string sockeId);
        Group GetGroupByUser(string userName);
        List<Group> ListAll();
        bool RemoveUser(Group group, User user);
    }
}

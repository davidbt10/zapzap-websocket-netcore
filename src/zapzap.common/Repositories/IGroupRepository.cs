namespace zapzap.common.Repositories
{
    using System.Collections.Generic;
    using zapzap.common.Entities;

    public interface IGroupRepository
    {
        void Create(string name);    
        List<Group> ListAll();
        Group GetByName(string name);
        void Remove(Group group);
        void AddUserInGroup(Group group, User user);
        Group GetGroupBySocket(string sockeId);
        Group GetGroupByUser(string userName);
        void RemoveUser(Group group, User user);
    }
}

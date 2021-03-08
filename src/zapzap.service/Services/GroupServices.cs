namespace zapzap.service.Services
{
    using System.Collections.Generic;
    using zapzap.common.Entities;
    using zapzap.common.Repositories;
    using zapzap.common.Services;

    public class GroupServices : IGroupServices
    {
        private readonly IGroupRepository _groupRepository;

        public GroupServices(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void PutUserInGroup(string groupName, User user)
        {
            PutUserInGroup(_groupRepository.GetByName(groupName), user);
        }

        public void PutUserInGroup(Group group, User user)
        {
            _groupRepository.AddUserInGroup(group, user);
        }

        public List<User> GetConnectedUserByGroup(string groupName)
        {
            Group group = _groupRepository.GetByName(groupName);
            return group.ConnectedUsers;
        }

        public Group GetGroupBySocket(string sockeId)
        {
            return _groupRepository.GetGroupBySocket(sockeId);
        }

        public Group GetGroupByUser(string userName)
        {
            return _groupRepository.GetGroupByUser(userName);
        }

        public List<Group> ListAll()
        {
            return _groupRepository.ListAll();
        }

        public bool RemoveUser(Group group, User user)
        {
            try
            {
                _groupRepository.RemoveUser(group, user);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void CreateNewGroup(string newGroupName)
        {
            _groupRepository.Create(newGroupName);
        }
    }
}
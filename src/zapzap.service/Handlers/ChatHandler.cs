namespace zapzap.service.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using zapzap.common;
    using zapzap.common.Entities;
    using zapzap.common.Providers;
    using zapzap.common.Services;

    public class ChatHandler : IChatHandler
    {
        private readonly IGroupServices _groupServices;
        private readonly IWebsocketHandler _websocketHandler;

        public ChatHandler(IGroupServices groupServices, IWebsocketHandler websocketHandler)
        {
            _groupServices = groupServices;
            _websocketHandler = websocketHandler;
        }

        public async Task ReceiveAsync(WebSocket socket, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string socketID = _websocketHandler.GetSocketID(socket);
                await ProcessRequest(socketID, text.Trim());
            }
        }

        private async Task ProcessRequest(string socketID, string message)
        {
            Group courrentGroup = _groupServices.GetGroupBySocket(socketID);

            if (courrentGroup == default)
                await RegisterUserInDefaultGorup(socketID, message);
            else
            {
                string command = message.Split(' ').FirstOrDefault();
                User connectedUser = courrentGroup.ConnectedUsers.Select(x => x).FirstOrDefault(y => y.SocketID.Equals(socketID));

                switch (command)
                {
                    case Constants.Commands.Help:
                        await SendMessageToUser(socketID, Constants.Messages.HelpCommands);
                        break;

                    case Constants.Commands.PrivateMessage:
                        await SendPrivateMessage(courrentGroup, message, connectedUser);
                        break;

                    case Constants.Commands.Mention:
                        await SendMentionMessage(courrentGroup, message, connectedUser);
                        break;

                    case Constants.Commands.ListGroups:
                        await ListAllGroups(connectedUser);
                        break;

                    case Constants.Commands.NewGroup:
                        await CreateNewGroup(courrentGroup, message, connectedUser);
                        break;

                    case Constants.Commands.SwitchGroup:
                        await SwitchGroup(courrentGroup, message, connectedUser);
                        break;

                    case Constants.Commands.Exit:
                        await Exit(courrentGroup, connectedUser);
                        break;

                    default:
                        string formattedMessage = string.Format(Constants.Messages.MessageToGroup, connectedUser.Name, message);
                        await SendMessageToGroup(courrentGroup, formattedMessage);
                        break;
                }
            }
        }

        private async Task RegisterUserInDefaultGorup(string socketID, string message)
        {
            if (_groupServices.GetGroupByUser(message) == default)
            {
                User newUser = new User()
                {
                    Name = message,
                    SocketID = socketID
                };

                _groupServices.PutUserInGroup(Constants.Messages.DefaultGroupName, newUser);
                await SendMessageToUser(socketID, string.Format(Constants.Messages.Joined, newUser.Name, Constants.Messages.DefaultGroupName));
                await SendMessageToUser(socketID, string.Format(Constants.Messages.HelpMessage));

                List<User> connectedUsers = _groupServices.GetConnectedUserByGroup(Constants.Messages.DefaultGroupName);
                List<string> socketsList = connectedUsers.Select(x => x.SocketID).Where(y => y != socketID).ToList();
                await _websocketHandler.SendMessageToGroupAsync(socketsList, string.Format(Constants.Messages.NewUserInChat, newUser.Name, Constants.Messages.DefaultGroupName));
            }
            else
                await SendMessageToUser(socketID, string.Format(Constants.Messages.NameAlreadyTaken, message));
        }




        private async Task SendMessageToGroup(Group group, string message)
        {
            List<string> socketsList = group.ConnectedUsers.Select(x => x.SocketID).ToList();
            await _websocketHandler.SendMessageToGroupAsync(socketsList, message);
        }

        private async Task SendMessageToUser(string SocketID, string message)
        {
            await _websocketHandler.SendMessageAsync(SocketID, message);
        }

        private async Task<bool> ValidateBlankMessage(string SocketID, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                return true;

            await SendMessageToUser(SocketID, Constants.Messages.MessageBlank);
            return false;
        }




        private async Task SendPrivateMessage(Group courrentGroup, string message, User connectedUser)
        {
            string[] words = message.Split();
            string receiverName = words.Take(2).Last();

            User receiver = courrentGroup.ConnectedUsers.FirstOrDefault(x => x.Name.Equals(receiverName));

            if (receiver == default)
            {
                await SendMessageToUser(connectedUser.SocketID, string.Format(Constants.Messages.UserNotFound, receiverName));
                return;
            }

            string messageSend = message.Split(receiverName).Last();

            if (await ValidateBlankMessage(connectedUser.SocketID, messageSend))
            {
                string formattedMessage = string.Format(Constants.Messages.PrivateMessage, connectedUser.Name, receiver.Name, messageSend);

                await SendMessageToUser(receiver.SocketID, formattedMessage);
                await SendMessageToUser(connectedUser.SocketID, formattedMessage);
            }
        }

        private async Task SendMentionMessage(Group courrentGroup, string message, User connectedUser)
        {
            string[] words = message.Split();
            string receiverName = words.Take(2).Last();

            string messageSend = message.Split(receiverName).Last();

            if (await ValidateBlankMessage(connectedUser.SocketID, messageSend))
            {
                string formattedMessage = string.Format(Constants.Messages.MentionMessage, connectedUser.Name, receiverName, messageSend);
                await SendMessageToGroup(courrentGroup, formattedMessage);
            }
        }





        private async Task ListAllGroups(User connectedUser)
        {
            List<Group> groups = _groupServices.ListAll();
            string list = string.Empty;

            foreach (Group group in groups)
            {
                list += " - " + group.Name + "<br />";
            }

            await SendMessageToUser(connectedUser.SocketID, string.Format(Constants.Messages.GroupList, list));
        }

        private async Task CreateNewGroup(Group courrentGroup, string message, User connectedUser)
        {
            try
            {
                if (_groupServices.RemoveUser(courrentGroup, connectedUser))
                {
                    string[] words = message.Split();
                    string newGroupName = words.Take(2).Last();

                    if (await ValidateBlankMessage(connectedUser.SocketID, newGroupName))
                    {
                        _groupServices.CreateNewGroup(newGroupName);
                        _groupServices.PutUserInGroup(newGroupName, connectedUser);

                        await SendMessageToUser(connectedUser.SocketID, string.Format(Constants.Messages.GroupJoined, newGroupName));
                    }
                }
                else
                    await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedLeaveGroup);
            }
            catch (System.Exception)
            {
                await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedLeaveGroup);
            }
        }

        private async Task SwitchGroup(Group courrentGroup, string message, User connectedUser)
        {
            try
            {
                if (_groupServices.RemoveUser(courrentGroup, connectedUser))
                {
                    string[] words = message.Split();
                    string newGroupName = words.Take(2).Last();

                    if (await ValidateBlankMessage(connectedUser.SocketID, newGroupName))
                    {
                        _groupServices.PutUserInGroup(newGroupName, connectedUser);
                        await SendMessageToUser(connectedUser.SocketID, string.Format(Constants.Messages.GroupJoined, newGroupName));

                        Group newGroup = _groupServices.GetGroupByUser(connectedUser.Name);
                        await SendMessageToGroup(newGroup, string.Format(Constants.Messages.NewUserInGroup, connectedUser.Name));
                    }
                }
                else
                    await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedSwitchGroup);
            }
            catch (System.Exception)
            {
                await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedSwitchGroup);
            }
        }



        private async Task Exit(Group courrentGroup, User connectedUser)
        {
            try
            {
                if (_groupServices.RemoveUser(courrentGroup, connectedUser))
                {
                    await SendMessageToUser(connectedUser.SocketID, Constants.Messages.ExitUser);
                    await _websocketHandler.RemoveSocket(connectedUser.SocketID);

                    await SendMessageToGroup(courrentGroup, string.Format(Constants.Messages.ExitGroup, connectedUser.Name));
                }
                else
                    await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedExitGroup);

            }
            catch (System.Exception)
            {
                await SendMessageToUser(connectedUser.SocketID, Constants.Messages.FailedExitGroup);
            }
        }
    }
}

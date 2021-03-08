namespace zapzap.common
{
    public class Constants
    {
        public class Messages
        {
            public const string DefaultGroupName = "#General";
            public const string WelcomeMessage = "*** Welcome to our chat server. Please provide a nickname: ";
            public const string NameAlreadyTaken = "*** Sorry, the nickname {0} is already taken. Please choose a different one: ";
            public const string NewUserInChat = "{0} has joined {1}";
            public const string HelpMessage = "*** Type '/help' to list the commands";
            public const string Joined = "*** You are registred as {0}. Joining {1}";
            public const string GroupJoined = "*** Joining {0}";
            public const string NewUserInGroup = "{0} joined the group";
            
            public const string MessageToGroup = "{0} says: {1}";
            public const string PrivateMessage = "{0} says privately to {1}: {2}";
            public const string MentionMessage = "{0} says to {1}: {2}";
            public const string UserNotFound = "*** User {0} not found";
            public const string MessageBlank = "*** Message cannot be blank";
            public const string GroupList = "List of existing groups: <br /> {0}";
            public const string FailedLeaveGroup = "Failed to leave the current group";
            public const string FailedSwitchGroup = "Failed to switch groups";
            public const string ExitUser = "*** Disconected. Bye!";
            public const string ExitGroup = "*** {0} left the group!";
            public const string FailedExitGroup = "Failed to exit";

            public const string HelpCommands = @" '/p {User Name} {Message}' - To send private message <br />
                                                  '/m {User Name} {Message}' - To mention a user <br />
                                                  '/n {Group Name}' - To create a new group <br />
                                                  '/l' - To list groups <br />
                                                  '/s {Group Name}' - To switch group <br />
                                                  '/exit - To exit";
        }

        public class Commands
        {
            public const string Help = "/help";
            public const string PrivateMessage = "/p";
            public const string Mention = "/m";
            public const string NewGroup = "/n";
            public const string ListGroups = "/l";
            public const string SwitchGroup = "/s";
            public const string Exit = "/exit";
        }
    }
}

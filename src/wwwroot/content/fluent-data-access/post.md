# Fluent data access

Working in a team of developers, the following might happen:

Developer 1 creates a piece of UI and adds a method to load data into said UI. The method he creates is called GetUserInfo(…)

Here comes Developer 2 and he also creates a piece of UI that displays user info. Developer 2 however, needs to display more than just the user info, he also needs the requests made by the user. And then GetUserInfoWithRequests(…) is created next to the already existing GetUserInfo method.

Developer 3 wants to display some user info as well, but not just the info by itself, and also without the requests but WITH the users team information. Can you guess what happens? … Right. GetUserInfoWithTeamMembers(…) is added to our list.

So we now have three methods that do something similar, but not quite the same:

    GetUserInfo(…)
    GetUserInfoWithRequests(…)
    GetUserInfoWithTeamMembers(…)

If you let this run uncontrolled it will turn in to a maintenance nightmare! We could simply end the discussion and say it’s a lack of discipline within the team, but there is a nice way to help prevent this through code!

What is needed is a query-building object that will handle the loading of all this data for us. Without resorting to separate methods that all touch the database.

This query-building object has at least one method: Fetch(). The fetch method is the only method that will touch the database. The constructor of our object will take in the key on which to filter. In our example, this would probably be the primary key for the user in our database. Lets call it a UserInfoRetriever to match the example.

 
    public class UserInfoRetriever
    {
        readonly int _userId;

        public UserInfoRetriever(int userId)
        {
            _userId = userId;
        }

        public User Fetch()
        {
            using (var context = new DbContext())
            {
                return context.Users.Where(u => u.UserId == _userId).SingleOrDefault();
            }
        }
    }

The code above is what this might look like for the first method in our set of three. But we have more! Lets expand the class a bit further to support the other scenarios.

    public class UserInfoRetriever
    {
        readonly int _userId;

        bool _withRequests;
        bool _withTeamMembers;

        public UserInfoRetriever(int userId)
        {
            _userId = userId;
        }

        public UserInfoRetriever WithRequests()
        {
            _withRequests = true;
            return this;
        }

        public UserInfoRetriever WithTeamMembers()
        {
            _withTeamMembers = true;
            return this;
        }

        public User Fetch()
        {
            using (var context = new DbContext())
            {
                var users = context.Users;

                if (_withRequests)
                {
                    users.Include("Requests");
                }

                if (_withTeamMembers)
                {
                    users.Include("TeamMembers");
                }

                return context.Users.Where(u => u.UserId == _userId).SingleOrDefault();
            }
        }
    }

This version of the class implements two extra methods. They only change the bools on our class and then returns itself. This is where the cool stuff is at. Because we return to ourselves in the methods, we can chain them together to form queries as we see fit like this:

    var userInfo = new UserInfoRetriever(10).Fetch();

    var userInfo = new UserInfoRetriever(10).WithTeamMembers().WithRequests().Fetch();

    var userInfo = new UserInfoRetriever(10).WithRequests().Fetch();

var userInfo = new UserInfoRetriever(10).WithTeamMembers().Fetch();
As you can see, we can load the data any way we like and our data-access code is still all in one place (in the Fetch() method). Adding new scenarios is very easy and it prevents (with a little discipline of course :-)) willy-nilly methods that all handle their own data-access.
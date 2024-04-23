// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace TargetingConsoleApp.Identity
{
    class InMemoryUserRepository : IUserRepository
    {
        public static readonly IEnumerable<User> Users = new User[]
        {
            new User("Jeff"),
            new User("Alicia"),
            new User("Susan"),
            new User("JohnDoe", new List<string> { "Management" }),
            new User("JaneDoe", new List<string> { "Management" }),
            new User("Tim", new List<string> { "TeamMembers" }),
            new User("Tanya", new List<string> { "TeamMembers" }),
            new User("Alec", new List<string> { "TeamMembers" }),
            new User("Betty", new List<string> { "TeamMembers" }),
            new User("Anne"),
            new User("Chuck", new List<string> { "Contractor" })
        };

        public Task<User> GetUser(string id)
        {
            User? result = Users.FirstOrDefault(user => user.Id.Equals(id));

            if (result == null)
            {
                throw new InvalidOperationException($"User with ID '{id}' cannot be found.");
            }

            return Task.FromResult(result);
        }
    }
}

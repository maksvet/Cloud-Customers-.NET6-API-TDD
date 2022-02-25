
using CloudCustomers.API.Models;
using System.Collections.Generic;

namespace CloudCustomers.UnitTests.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> GetTestUsers() => new()
        {
            new User
            {

                Name = "m",
                Address = new Address()
                {
                    Street = "McCowan",
                    City = "Sto",
                    ZipCode = 53704
                },
                Email = "m@m.m"


            },
            new User
            {

                Name = "n",
                Address = new Address()
                {
                    Street = "McCowan1",
                    City = "Sto1",
                    ZipCode = 53704
                },
                Email = "m1@m.m"


            },
            new User
            {

                Name = "o",
                Address = new Address()
                {
                    Street = "McCowan2",
                    City = "Sto2",
                    ZipCode = 53704
                },
                Email = "m2@m.m"

            }

        };
    }
}

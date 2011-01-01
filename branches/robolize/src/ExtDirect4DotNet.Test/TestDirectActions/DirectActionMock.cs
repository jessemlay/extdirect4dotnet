using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtDirect4DotNet.Test.TestDirectActions {
    [DirectAction]
    public class DirectActionMock {
        [DirectMethod]
        public Person I_take_and_return_a_person_object(string name, int age, DateTime graduationDate, Person friend) {
            Person person = new Person {
                Name = name,
                Age = age,
                GraduationDate = graduationDate, //DateTime.Parse("5/27/1997")
                Friends = new List<Person> {
                    friend
                }
            };
            return person;
        }

        [DirectMethod]
        public string SayHelloTo(string name) {
            return string.Format("Hello, {0}", name);
        }
    }

    public class Person {
        public int Age { get; set; }

        public List<Person> Friends { get; set; }

        public DateTime GraduationDate { get; set; }

        public string Name { get; set; }
    }
}
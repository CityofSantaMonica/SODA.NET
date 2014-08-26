using System.Runtime.Serialization;
namespace SODA.Utilities.Tests.Mocks
{
    class SimpleEntityMock
    {
        public string foo { get; private set; }
        public string bar { get; private set; }
        
        public SimpleEntityMock(string foo, string bar)
        {
            this.foo = foo;
            this.bar = bar;
        }
    }

    class ComplexEntityMock
    {
        public string name { get; private set; }

        public SimpleEntityMock[] entities { get; private set; }
        
        public ComplexEntityMock(string name, SimpleEntityMock[] entities)
        {
            this.name = name;
            this.entities = entities;
        }
    }

    [DataContract]
    class DataContractEntityMock
    {
        [DataMember(Name = ":foo")]
        public string foo { get; private set; }

        [DataMember(Name = ":bar")]
        public string bar { get; private set; }

        public string bup { get; set; }
        
        public DataContractEntityMock(string foo, string bar, string bup)
        {
            this.foo = foo;
            this.bar = bar;
            this.bup = bup;
        }
    }
}

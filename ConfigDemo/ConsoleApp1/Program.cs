






using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;



var json = """
           [{
               "Logging": {
                   "LogLevel": {
                       "Default": "Debug",
                       "System": "Warning",
                       "Microsoft": "Information"
                   }
               },
               "ConnectionStrings": {
                   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-ConsoleApp1;Trusted_Connection=True;MultipleActiveResultSets=true"
               }
           }]
           """;
//var obj = (JObject)JsonConvert.DeserializeObject(json);
//var obj = JArray.Parse(json);

//Console.WriteLine(obj[0]["Logging"]["LogLevel"]["System"]);
//Console.WriteLine(obj[0]["ConnectionStrings"]["DefaultConnection"]);


//var obj = new JObject(
//    new JProperty("Logging", new JObject(
//        new JProperty("LogLevel", new JObject(
//            new JProperty("Default", "Debug"),
//            new JProperty("System", "Warning"),
//            new JProperty("Microsoft", "Information")
//        ))
//    )),
//    new JProperty("ConnectionStrings", new JObject(
//        new JProperty("DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=aspnet-ConsoleApp1;Trusted_Connection=True;MultipleActiveResultSets=true")
//    ))
//);
//Console.WriteLine(obj);









//Person person = new Person {
//	Id = Guid.NewGuid(),
//	Name = "Jak",
//	Age = 21,
//	Birth = new DateTime(2004, 5, 15),
//	Hobbies = new List<string> { "Reading", "Traveling", "Gaming" },
//	Gender = Gender.Famale,
//	WorkDays = WorkDays.Monday | WorkDays.Wednesday | WorkDays.Friday,
//};
//Console.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));

//class Person  {

//	public Guid Id { get; set; }
//	[JsonConverter(typeof(DateTimeFormatConverter))]
//	public DateTime Birth { get; set; }

//	public string Name { get; set; }
//	public int Age { get; set; }
//	public List<string> Hobbies { get; set; }
//	[JsonConverter(typeof(StringEnumConverter))]
//	public Gender Gender { get; set; }
//	[JsonConverter(typeof(StringEnumConverter))]
//	public WorkDays WorkDays { get; set; }
//	public string Description => $"{Name} is {Age} years old. Have {Hobbies.Count} Hobbies";
//}

//enum Gender {
//	Male, Famale
//}
//[Flags]
//enum WorkDays {
//	Monday = 1 << 0,
//	Tuesday = 1 << 1,
//	Wednesday = 1 << 2,
//	Thursday = 1 << 3,
//	Friday = 1 << 4,
//	Saturday = 1 << 5,
//	Sunday = 1 << 6,
//	All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
//}


//class DateTimeFormatConverter:IsoDateTimeConverter {
//	public DateTimeFormatConverter() {
//		DateTimeFormat = "yyyy-MM-dd";
//	}
//}
























//using System.Text.Json.Serialization;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;


//var student = new Student {
//	Name = "John Doe",
//	Age = 20,
//	School = "University of Examples",
//	Address = null
//};
//var employee = new Employee {
//	Name = "Jane Smith",
//	Age = 30,
//	Company = "Example Corp",
//	Salary = 60000.50
//};
//var student2 = new Student {
//	Name = "John Doe2",
//	Age = 21,
//	School = "University of Examples2",
//	Address = "123 Main"
//};

//var list = new List<Person>();
//list.Add(student);
//list.Add(employee);

//var settings = new JsonSerializerSettings {
//	Formatting = Formatting.Indented,
//	ContractResolver = new DefaultContractResolver() {
//		NamingStrategy = new DefaultNamingStrategy()
//	},
//	NullValueHandling = NullValueHandling.Ignore,
//	StringEscapeHandling = StringEscapeHandling.Default,
//	TypeNameHandling = TypeNameHandling.Auto,
//	ObjectCreationHandling = ObjectCreationHandling.Auto,
//};
//var serializeObject = JsonConvert.SerializeObject(student2, settings);
//Console.WriteLine(serializeObject);

//var json = """
//		   {
//		     "School": "University of Examples2",
//		     "Address": "重庆三峡学院",
//		     "Name": "Jak",
//		     "Age": 21
//		   }
//		   """;
//JsonConvert.PopulateObject(json, student2, settings);
////var json = """
////           [
////             {
////               "$type": "Student, ConsoleApp1",
////               "School": "University of Examples",
////               "Name": "John Doe",
////               "Age": 20
////             },
////             {
////               "$type": "Employee, ConsoleApp1",
////               "Company": "Example Corp",
////               "Salary": 60000.5,
////               "Name": "Jane Smith",
////               "Age": 30
////             }
////           ]
////           """;

////var persons = JsonConvert.DeserializeObject<List<Person>>(json,settings);
////Console.WriteLine((persons[0] as Student).School);
//return;
////var json = """
////           {
////             "Name": "John Doe",
////             "Age": 20,
////             "School": "University of Examples"
////           }
////           """;
////var p = JsonConvert.DeserializeObject<Student>(json);
////Console.WriteLine(p.Name);





//Console.ReadKey();


//class Person {
//	public string Name { get; set; }
//	public int Age { get; set; }
//}

//class Student :Person{
//	public string School { get; set; }
//	public string ? Address { get; set; }
//}
//class Employee : Person {
//	public string Company { get; set; }
//	public double Salary { get; set; }
//}
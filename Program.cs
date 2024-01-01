using System;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ReadWriteFileJson
{
    class Program
    {

        public static void Main()
        {

            Console.WriteLine($@"
                ======= CRUD of Users =======
                1 - Create User
                2 - Read Users
                3 - Find User
                4 - Delete User
                0 - Exit
            ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    InputQuestionPerson();
                    Program.Main();
                    break;
                case "2":
                    List<Person> listPerson = ReadFile();
                    listPerson.ForEach(p => Console.WriteLine($@"
                        Name: {p?.Name}
                        Email: {p?.Email}
                        Phone: {p?.Phone}                    
                    "));
                    Program.Main();
                    break;
                    
                case "3":
                    Console.WriteLine("Email para busca: ");
                    string? findEmail = Console.ReadLine();
                    Person? person = FindPersonByEmail(findEmail);

                    if (string.IsNullOrEmpty(person?.Email)){
                        Console.WriteLine($"Email: {findEmail} nao encontrado!!!");
                    } else {
                        Console.WriteLine($@"
                            Name: {person?.Name}
                            Email: {person?.Email}
                            Phone: {person?.Phone}
                        ");
                    }
                    Program.Main();
                    break;
                case "4":
                    Console.WriteLine("Email para remover: ");
                    string? deleteEmail = Console.ReadLine();

                    bool deletePerson = DeletePersonByEmail(deleteEmail);

                    if (!deletePerson) {
                        Console.WriteLine("Nao foi possivel encontrado o email!!!");
                    } else {
                        Console.WriteLine("Usuário removido com sucesso!!");
                    }
                    Program.Main();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Option not found!");
                    Program.Main();
                    break;
            }


            // Console.WriteLine("Email para remover: ");
            // string? email = Console.ReadLine();

            // bool deletePerson = DeletePersonByEmail(email);

            // if (!deletePerson) {
            //     Console.WriteLine("Nao foi possivel encontrado o email!!!");
            // } else {
            //     Console.WriteLine("Usuário removido com sucesso!!");
            // }


            // Console.WriteLine("Email para Atualizar: ");
            // string? email = Console.ReadLine();
            
            // Person person = new();

            // Console.WriteLine("Nome: ");
            // person.Name = Console.ReadLine();

            // Console.WriteLine("Email: ");
            // person.Email = Console.ReadLine();
            
            // Console.WriteLine("Phone: ");
            // person.Phone = long.Parse(Console.ReadLine());

            // UpdatePersonByEmail(person, email);
            

            // Console.WriteLine("Email para busca: ");
            // string? email = Console.ReadLine();
            // Person? person = FindPersonByEmail(email);

            // if (string.IsNullOrEmpty(person?.Email)){
            //     Console.WriteLine($"Email: {email} nao encontrado!!!");
            // } else {
            //     Console.WriteLine($@"
            //         Name: {person?.Name}
            //         Email: {person?.Email}
            //         Phone: {person?.Phone}
            //     ");
            // }
        }

        public static List<Person> ReadFile() {
            try 
            {
                string jsonFilePath = "person.json";
                
                using (StreamReader reader = File.OpenText(jsonFilePath))
                {
                    string jsonContent = reader.ReadToEnd();

                    List<Person> peoples = JsonSerializer.Deserialize<List<Person>>(jsonContent);
                    
                    return peoples is null ? new List<Person>() : peoples;
                }
            } catch(Exception ex) 
            {
                Console.WriteLine($"Exception : {ex}");
                return new List<Person>();
            }
        }

        public static void InputQuestionPerson() {
            Person person = new();
            Console.WriteLine("Digite seu nome: ");
            person.Name = Console.ReadLine();

            Console.WriteLine("Digite seu e-mail: ");
            person.Email = Console.ReadLine();

            Console.WriteLine("Digite seu Telefone: ");
            person.Phone = long.Parse(Console.ReadLine());

            List<Person> listPeoples = ReadFile();

            Person findPerson = listPeoples.FirstOrDefault(p => p.Email == person.Email);

            if (string.IsNullOrEmpty(findPerson?.Email)) 
            {
                Console.WriteLine($"E-mail not exists: {person.Email}");
                listPeoples.Add(person);
                listPeoples.ForEach(e => Console.WriteLine($"Name: {e.Name} - E-mail: {e.Email} - Phone: {e.Phone}"));

                WriteFile(listPeoples);
            } else {
                Console.WriteLine($"E-mail {person.Email} already exists!!!");
            }
        }

        public static void WriteFile(List<Person> listPerson) {
            // Specify the path to the JSON file:
            string filePath = "person.json";

            // Serialize the object to a JSON string:
            string jsonString = JsonSerializer
                .Serialize(
                    listPerson.ToList(), 
                    new JsonSerializerOptions 
                    { 
                        WriteIndented = true 
                    }
                );

            // Write the JSON string to the file:
            File.WriteAllText(filePath, jsonString);
        }

        public static dynamic? FindPersonByEmail(string email) 
        {
            var listPerson = ReadFile();
            var person = listPerson.FirstOrDefault(p => p.Email == email);

            if (string.IsNullOrEmpty(person?.Email))
            {
                return null;
            }

            return person;
        }

        public static bool UpdatePersonByEmail(Person person, string email)
        {
            var listPerson = ReadFile();
            int index = listPerson.FindIndex(p => p.Email == email);

            if (index != -1)
            {
                listPerson[index].Name = person.Name;
                listPerson[index].Email = person.Email;
                listPerson[index].Phone = person.Phone;
                WriteFile(listPerson);

                ReadFile().ForEach(p => Console.WriteLine($@"
                    Name: {p.Name}
                    Email: {p.Email}
                    Phone: {p.Phone}
                "));

                return true;
            } 
            return false;
        }

        public static bool DeletePersonByEmail(string email)
        {
            var listPerson = ReadFile();
            int index = listPerson.FindIndex(p => p.Email == email);
            
            if (index != -1)
            {
                listPerson.RemoveAt(index);

                WriteFile(listPerson);
                return true;
            }
            return false;
        }
    }    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace ReadWriteFileJson
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Phone { get; set; } = 0;
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Speciality { get; set; }
        public int Salary { get; set; }

        //[JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime BirthDate { get; set; }

        //[JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime EmployementDate { get; set; }
    }

    ////public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    ////{
    ////    private const string Format = "yyyy-MM-dd";

    ////    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    ////    {
    ////        return DateOnly.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
    ////    }

    ////    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    ////    {
    ////        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    ////    }
    //}

}


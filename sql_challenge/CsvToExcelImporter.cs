using OfficeOpenXml;

namespace CsvToExcelImporter
{
    public class Entity
    {
        public int EntityId { get; set; }
        public string? EntityFirstName { get; set; }
        public string? EntityMiddleName { get; set; }
        public string? EntityLastName { get; set; }
        public DateTime? EntityDob { get; set; }
        public bool IsMaster { get; set; }
        public string? Address { get; set; }
        public string? EntityGender { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- input.csv"); // Provide the path to the CSV file as an argument
                return;
            }

            string csvFilePath = args[0];
            string outputExcelPath = "output.xlsx"; // You can change this to any desired output path

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine($"File not found: {csvFilePath}");
                return;
            }

            var entities = ReadAndSanitizeCsv(csvFilePath);

            if (entities.Count == 0)
            {
                Console.WriteLine("No valid data to import.");
                return;
            }

            WriteToExcel(entities, outputExcelPath);
            Console.WriteLine($"Data imported to {outputExcelPath}");
        }

        private static List<Entity> ReadAndSanitizeCsv(string filePath)
        {
            var entities = new List<Entity>();
            var lines = File.ReadAllLines(filePath).Skip(1); // Skip header

            foreach (var line in lines)
            {
                var values = ParseCsvLine(line); // Custom parse to handle commas in fields
                if (values.Length != 8) continue; // Invalid row

                // Sanitize and parse
                if (!int.TryParse(values[0].Trim(), out int entityId)) continue; // Required int

                string? firstName = SanitizeString(values[1], 128);
                string? middleName = SanitizeString(values[2], 128);
                string? lastName = SanitizeString(values[3], 128);

                DateTime? dob = null;
                if (!string.IsNullOrWhiteSpace(values[4]))
                {
                    if (DateTime.TryParse(values[4].Trim(), out DateTime parsedDob))
                    {
                        dob = parsedDob;
                    }
                }

                bool isMaster;
                var isMasterStr = values[5].Trim().ToLower();
                if (isMasterStr == "1" || isMasterStr == "true") isMaster = true;
                else if (isMasterStr == "0" || isMasterStr == "false") isMaster = false;
                else continue; // Required, invalid skip

                string? address = SanitizeString(values[6], 512);
                string? gender = SanitizeString(values[7], 16);

                entities.Add(new Entity
                {
                    EntityId = entityId,
                    EntityFirstName = firstName,
                    EntityMiddleName = middleName,
                    EntityLastName = lastName,
                    EntityDob = dob,
                    IsMaster = isMaster,
                    Address = address,
                    EntityGender = gender
                });
            }

            return entities;
        }

        private static string? SanitizeString(string? input, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            return input.Trim().Substring(0, Math.Min(input.Trim().Length, maxLength));
        }

        private static string[] ParseCsvLine(string line)
        {
            // Simple CSV parser handling quoted fields with commas
            var fields = new List<string>();
            var current = string.Empty;
            var inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"' && !inQuotes)
                {
                    inQuotes = true;
                }
                else if (c == '"' && inQuotes)
                {
                    inQuotes = false;
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current);
                    current = string.Empty;
                }
                else
                {
                    current += c;
                }
            }
            fields.Add(current);
            return fields.ToArray();
        }

        private static void WriteToExcel(List<Entity> entities, string filePath)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Quoc Anh");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Entities");

            // Header
            worksheet.Cells[1, 1].Value = "entity_id";
            worksheet.Cells[1, 2].Value = "entity_first_name";
            worksheet.Cells[1, 3].Value = "entity_middle_name";
            worksheet.Cells[1, 4].Value = "entity_last_name";
            worksheet.Cells[1, 5].Value = "entity_dob";
            worksheet.Cells[1, 6].Value = "is_master";
            worksheet.Cells[1, 7].Value = "address";
            worksheet.Cells[1, 8].Value = "entity_gender";

            // Data
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                worksheet.Cells[i + 2, 1].Value = entity.EntityId;
                worksheet.Cells[i + 2, 2].Value = entity.EntityFirstName;
                worksheet.Cells[i + 2, 3].Value = entity.EntityMiddleName;
                worksheet.Cells[i + 2, 4].Value = entity.EntityLastName;
                if (entity.EntityDob.HasValue)
                {
                    worksheet.Cells[i + 2, 5].Value = entity.EntityDob.Value;
                    worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "yyyy-mm-dd";
                }
                worksheet.Cells[i + 2, 6].Value = entity.IsMaster ? 1 : 0;
                worksheet.Cells[i + 2, 7].Value = entity.Address;
                worksheet.Cells[i + 2, 8].Value = entity.EntityGender;
            }

            package.SaveAs(new FileInfo(filePath));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    class QuarterlyIncomeReport
    {
        static void Main(string[] args)
        {
            // create a new instance of the class
            QuarterlyIncomeReport report = new QuarterlyIncomeReport();

            // call the GenerateSalesData method
            SalesData[] salesData = report.GenerateSalesData();
            
            // call the QuarterlySalesReport method
            report.QuarterlySalesReport(salesData);
        }

        /* public struct SalesData includes the following fields: date sold, department name, product ID, quantity sold, unit price */
        public struct SalesData
        {
            public DateOnly dateSold;
            public string departmentName;
            public string productID;
            public int quantitySold;
            public double unitPrice;
            public double baseCost;
            public int volumeDiscount;
        }

        public struct ProdDepartments
        {
            // Static array containing the names of various departments
            public static string[] departmentNames = new string[]
            {
                "Men's Wear",       // Department for men's clothing
                "Women's Wear",     // Department for women's clothing
                "Children's Wear",  // Department for children's clothing
                "Footwear",         // Department for shoes and related items
                "Accessories",      // Department for accessories like bags, belts, etc.
                "Sportswear",       // Department for sports-related clothing
                "Underwear",        // Department for undergarments
                "Outerwear"         // Department for coats, jackets, and other outerwear
            };

            // Static array containing the abbreviations for the department names
            public static string[] departmentAbbreviations = new string[]
            {
                "MNWR",  // Abbreviation for Men's Wear
                "WNWR",  // Abbreviation for Women's Wear
                "CHWR",  // Abbreviation for Children's Wear
                "FTWR",  // Abbreviation for Footwear
                "ACCS",  // Abbreviation for Accessories
                "SPRT",  // Abbreviation for Sportswear
                "UNDW",  // Abbreviation for Underwear
                "OTWR"   // Abbreviation for Outerwear
            };
        }

        public struct ManufacturingSites
        {
            // Static array containing the codes for various manufacturing sites
            public static string[] manSites = new string[]
            {
                "US1",  // Manufacturing site in the United States
                "US2",  // Manufacturing site in the United States
                "US3",  // Manufacturing site in the United States
                "CA1",  // Manufacturing site in Canada
                "CA2",  // Manufacturing site in Canada
                "CA3",  // Manufacturing site in Canada
                "MX1",  // Manufacturing site in Mexico
                "MX2",  // Manufacturing site in Mexico
                "MX3",  // Manufacturing site in Mexico
                "MX4"   // Manufacturing site in Mexico
            };
        }

        /* the GenerateSalesData method returns 1000 SalesData records. It assigns random values to each field of the data structure */
        public SalesData[] GenerateSalesData()
        {
            SalesData[] salesData = new SalesData[1000];
            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                salesData[i].dateSold = new DateOnly(2023, random.Next(1, 13), random.Next(1, 29));
                salesData[i].departmentName = ProdDepartments.departmentNames[random.Next(ProdDepartments.departmentNames.Length)];
                
                int indexOfDept = Array.IndexOf(ProdDepartments.departmentNames, salesData[i].departmentName);
                string deptAbb = ProdDepartments.departmentAbbreviations[indexOfDept];
                string firstDigit = (indexOfDept + 1).ToString();
                int nextTwoDigitsValue = random.Next(1, 100);
                string nextTwoDigits = nextTwoDigitsValue < 10 ? "0" + nextTwoDigitsValue.ToString() : nextTwoDigitsValue.ToString();
                string[] sizes = { "XS", "S", "M", "L", "XL" };
                string sizeCode = sizes[random.Next(sizes.Length)];
                string[] colors = { "BK", "BL", "GR", "RD", "YL", "OR", "WT", "GY" };
                string colorCode = colors[random.Next(colors.Length)];
                string manufacturingSite = ManufacturingSites.manSites[random.Next(ManufacturingSites.manSites.Length)];
                                
                salesData[i].productID = $"{deptAbb}-{firstDigit}{nextTwoDigits}-{sizeCode}-{colorCode}-{manufacturingSite}";
                salesData[i].quantitySold = random.Next(1, 101);
                salesData[i].unitPrice = random.Next(25, 300) + random.NextDouble();
                double discountPercentage = random.Next(5, 21) / 100.0;
                salesData[i].baseCost = salesData[i].unitPrice * (1 - discountPercentage);
                salesData[i].volumeDiscount = (int)(salesData[i].quantitySold * 0.10);
            }

            return salesData;
        }

        public void QuarterlySalesReport(SalesData[] salesData)
        {
            // create dictionaries to store the quarterly sales data and profit data
            Dictionary<string, Dictionary<string, double>> quarterlySalesByDept = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, Dictionary<string, double>> quarterlyProfitByDept = new Dictionary<string, Dictionary<string, double>>();

            // create dictionaries to store the total quarterly sales and profit for all departments
            Dictionary<string, double> totalQuarterlySales = new Dictionary<string, double>();
            Dictionary<string, double> totalQuarterlyProfit = new Dictionary<string, double>();

            // iterate through the sales data
            foreach (SalesData data in salesData)
            {
            // calculate the total sales and profit for each quarter
            string quarter = GetQuarter(data.dateSold.Month);
            double totalSales = data.quantitySold * data.unitPrice;
            double totalProfit = totalSales - (data.quantitySold * data.baseCost);

            if (!quarterlySalesByDept.ContainsKey(quarter))
            {
                quarterlySalesByDept[quarter] = new Dictionary<string, double>();
                quarterlyProfitByDept[quarter] = new Dictionary<string, double>();
            }

            if (quarterlySalesByDept[quarter].ContainsKey(data.departmentName))
            {
                quarterlySalesByDept[quarter][data.departmentName] += totalSales;
                quarterlyProfitByDept[quarter][data.departmentName] += totalProfit;
            }
            else
            {
                quarterlySalesByDept[quarter][data.departmentName] = totalSales;
                quarterlyProfitByDept[quarter][data.departmentName] = totalProfit;
            }

            // update total quarterly sales and profit for all departments
            if (totalQuarterlySales.ContainsKey(quarter))
            {
                totalQuarterlySales[quarter] += totalSales;
                totalQuarterlyProfit[quarter] += totalProfit;
            }
            else
            {
                totalQuarterlySales[quarter] = totalSales;
                totalQuarterlyProfit[quarter] = totalProfit;
            }
            }

            // display the quarterly sales report in order
            Console.WriteLine("Quarterly Sales Report");
            Console.WriteLine("----------------------");
            foreach (string quarter in new[] { "Q1", "Q2", "Q3", "Q4" })
            {
            Console.WriteLine($"{quarter}:");
            if (quarterlySalesByDept.ContainsKey(quarter))
            {
                foreach (var dept in quarterlySalesByDept[quarter])
                {
                double sales = dept.Value;
                double profit = quarterlyProfitByDept[quarter][dept.Key];
                double profitPercentage = (profit / sales) * 100;
                Console.WriteLine("  Department: {0}, Sales: {1:C}, Profit: {2:C}, Profit Percentage: {3:F2}%", dept.Key, sales, profit, profitPercentage);
                }

                // display total sales, profit, and profit percentage for the quarter
                double totalSales = totalQuarterlySales[quarter];
                double totalProfit = totalQuarterlyProfit[quarter];
                double totalProfitPercentage = (totalProfit / totalSales) * 100;
                Console.WriteLine("  Total Sales: {0:C}, Total Profit: {1:C}, Total Profit Percentage: {2:F2}%", totalSales, totalProfit, totalProfitPercentage);
            }
            else
            {
                Console.WriteLine("  No sales data available.");
            }
            }
        }

        public string GetQuarter(int month)
        {
            if (month >= 1 && month <= 3)
            {
                return "Q1";
            }
            else if (month >= 4 && month <= 6)
            {
                return "Q2";
            }
            else if (month >= 7 && month <= 9)
            {
                return "Q3";
            }
            else
            {
                return "Q4";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lab08
{
    internal class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();
        static void Main(string[] args)
        {
            Joining();
            Console.Read();
        }

        static void IntroToLINQ()
        {
            //estructura de ejemplo
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            //crear query
            var numQuery = from num in numbers where (num % 2) == 0 select num;

            // ejecutar query
            foreach (int num in numQuery)
            {
                Console.WriteLine("{0, 1}", num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomers = from cust in context.clientes select cust;
            //lambda
            var queryAllCustomersLambda = context.clientes
                .Select(cust => cust);

            foreach(var item in queryAllCustomersLambda)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       select cust;

            //lambda
            var queryLondonCustomersLambda = context.clientes
                .Where(cust => cust.Ciudad == "Londres")
                .Select(cust => cust);

            foreach (var item in queryLondonCustomersLambda)
            {
                Console.WriteLine(item.Ciudad);
            }
        }

        static void Ordering()
        {
            var queryLondonCustomers3 = from cust in context.clientes
                                        where cust.Ciudad == "Londres"
                                        orderby cust.NombreCompañia ascending
                                        select cust;

            //lambda
            var queryLondonCustomers3Lambda = context.clientes
                .Where(cust => cust.Ciudad == "Londres")
                .OrderBy(cust => cust.NombreCompañia) //OrderBy por defecto es ascending
                .Select(cust => cust);

            foreach (var item in queryLondonCustomers3Lambda)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Grouping()
        {
            var queryCustomersByCity = from cust in context.clientes
                                       group cust by cust.Ciudad;

            //lambda
            var queryCustomersByCityLambda = context.clientes
                .GroupBy(cust => cust.Ciudad);

            foreach (var customerGroup in queryCustomersByCityLambda)
            {
                Console.WriteLine(customerGroup.Key);
                foreach (clientes customer in customerGroup)
                {
                    Console.WriteLine("         {0}", customer.NombreCompañia);
                }
            }
        }

        static void Grouping2()
        {
            var custQuery = from cust in context.clientes
                            group cust by cust.Ciudad into custGroup
                            where custGroup.Count() > 2
                            orderby custGroup.Key
                            select custGroup;

            //lambda
            var customerGroup = context.clientes
                .GroupBy(cust => cust.Ciudad);
            //basicamente el into es como un "as" así que lo guardamos
            //en una variable y de ese hacemos el where
            var custQueryLambda = customerGroup
                .Where(custGroup => custGroup.Count() > 2)
                .OrderBy(custGroup => custGroup.Key)
                .Select(custGroup => custGroup);

            foreach (var item in custQueryLambda)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void Joining()
        {
            var innerJoinQuery =
                from cust in context.clientes
                join dist in context.Pedidos
                on cust.idCliente equals dist.IdCliente
                select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };

            var innerJoinQueryLambda = context.clientes
                .Join(context.Pedidos,
                    cust => cust.idCliente,
                    dist => dist.IdCliente,
                    (cust, dist) => new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario });

            foreach (var item in innerJoinQueryLambda)
            {
                Console.WriteLine(item.CustomerName);
            }
        }
    }
}

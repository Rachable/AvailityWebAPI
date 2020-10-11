using System.Collections.Generic;
using System;
using Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Services
{
    public class CustomerService : ICustomerService
    {
        internal AppDb Db { get; set; }


        public CustomerService(AppDb db)
        {
            Db = db;
        }
        public PagedList<Customer> GetAllCustomers(CustomerParameters customerParameters)
        {
            try
            {
                Db.Connection.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"SELECT `id`, `first_name`, `last_name`, `date_created`, `date_last_updated` FROM `customers`";
                var result = ReadAll(cmd.ExecuteReader());
                return PagedList<Customer>.ToPagedList(result.AsQueryable().OrderBy(on => on.Id),
                                                       customerParameters.PageNumber,
                                                       customerParameters.PageSize);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }
        }
        public int Add(Customer newCustomer)
        {
            try
            {
                Db.Connection.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO `customers` (`first_name`, `last_name`) VALUES (@firstName, @lastName);
                                    SELECT LAST_INSERT_ID();
                                    COMMIT;";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@firstName",
                    DbType = DbType.String,
                    Value = newCustomer.FirstName,
                });
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@lastName",
                    DbType = DbType.String,
                    Value = newCustomer.LastName,
                });
                var Id = Convert.ToInt32(cmd.ExecuteScalar());
                return Id;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }
        }
        public Customer GetById(int id)
        {
            try
            {
                Db.Connection.Open();
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"SELECT `id`, `first_name`, `last_name`, `date_created`, `date_last_updated` FROM `customers` WHERE `id` = @id";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = id,
                });
                var result = ReadAll(cmd.ExecuteReader());
                return result.Count > 0 ? result[0] : null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }
        }
        public bool Update(Customer newCustomer)
        {
            try
            {
                if (GetById(newCustomer.Id) != null)
                {
                    Db.Connection.Open();
                    using var cmd = Db.Connection.CreateCommand();
                    cmd.CommandText = @"UPDATE `customers` set `first_name` = @firstName, `last_name` = @lastName WHERE `id` = @id;";
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@firstName",
                        DbType = DbType.String,
                        Value = newCustomer.FirstName,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@lastName",
                        DbType = DbType.String,
                        Value = newCustomer.LastName,
                    });
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@id",
                        DbType = DbType.Int32,
                        Value = newCustomer.Id,
                    });
                    cmd.ExecuteNonQuery();

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }

        }
        public bool Remove(int id)
        {
            try
            {
                if (GetById(id) != null)
                {
                    Db.Connection.Open();
                    using var cmd = Db.Connection.CreateCommand();
                    cmd.CommandText = @"DELETE FROM `customers` WHERE `id` = @id";
                    cmd.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@id",
                        DbType = DbType.Int32,
                        Value = id,
                    });
                    var result = ReadAll(cmd.ExecuteReader());

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }
        }

        private List<Customer> ReadAll(DbDataReader reader)
        {
            try
            {
                var customers = new List<Customer>();
                using (reader)
                {
                    while (reader.Read())
                    {
                        var post = new Customer
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateBecameCustomer = reader.GetDateTime(3),
                            DateLastModified = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null
                        };
                        customers.Add(post);
                    }
                }
                return customers;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }
        }
    }
}
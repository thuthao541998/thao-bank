using System;
using System.Collections.Generic;
using System.Data;
using ConsoleApp3.model;
using MySql.Data.MySqlClient;
using SpringHeroBank.entity;
using SpringHeroBank.error;
using SpringHeroBank.utility;

namespace SpringHeroBank.model {
    public class TransactionModel {
        public List<Transaction> GetTransactionsByAccount (string accountNumber) {
            List<Transaction> list = new List<Transaction> ();
            Transaction transaction = null;
            DbConnection.Instance ().OpenConnection ();
            try {
                var queryString = "select * from  `transaction` where ( senderAccountNumber = @accountNumber and status = 2 ) or (receiverAccountNumber = @accountNumber and status = 2)";
                var cmd = new MySqlCommand (queryString, DbConnection.Instance ().Connection);
                cmd.Parameters.AddWithValue ("@accountNumber", accountNumber);
                var reader = cmd.ExecuteReader ();

                while (reader.Read ()) {
                    var id = reader.GetString ("id");
                    var createdAt = reader.GetString ("createdAt");
                    var updatedAt = reader.GetString ("updatedAt");
                    var type = reader.GetInt32 ("type");
                    var amount = reader.GetDecimal ("amount");
                    var content = reader.GetString ("content");
                    var senderAccountNumber = reader.GetString ("senderAccountNumber");
                    var receiverAccountNumber = reader.GetString ("receiverAccountNumber");
                    var status = reader.GetInt32 ("status");
                    transaction = new Transaction (id, createdAt, updatedAt, (Transaction.TransactionType) type, amount, content, senderAccountNumber, receiverAccountNumber, (Transaction.ActiveStatus) status);
                    list.Add (transaction);

                }
                reader.Close ();
                DbConnection.Instance ().CloseConnection ();
                return list;
            } catch (SpringHeroTransactionException e) {
                Console.WriteLine ("Invalid transactions at that time");
                throw;
            }

        }
        public List<Transaction> GetTransactionsBySetTime (string time, string accountNumber) {
            List<Transaction> list = new List<Transaction> ();
            DbConnection.Instance ().OpenConnection ();
            var queryString = "select * from `transaction` where ( senderAccountNumber = @accountNumber and status = 2 ) or (receiverAccountNumber = @accountNumber and status = 2) and DATE_SUB(CURDATE(),INTERVAL 1 " + time + ")";
            var cmd = new MySqlCommand (queryString, DbConnection.Instance ().Connection);
            cmd.Parameters.AddWithValue ("@accountNumber", accountNumber);
            var reader = cmd.ExecuteReader ();
            Transaction transaction = null;
            try {               
                while (reader.Read ()) {
                    var id = reader.GetString ("id");
                    var createdAt = reader.GetString ("createdAt");
                    var updatedAt = reader.GetString ("updatedAt");
                    var type = reader.GetInt32 ("type");
                    var amount = reader.GetDecimal ("amount");
                    var content = reader.GetString ("content");
                    var senderAccountNumber = reader.GetString ("senderAccountNumber");
                    var receiverAccountNumber = reader.GetString ("receiverAccountNumber");
                    var status = reader.GetInt32 ("status");
                    transaction = new Transaction (id, createdAt, updatedAt, (Transaction.TransactionType) type, amount, content, senderAccountNumber, receiverAccountNumber, (Transaction.ActiveStatus) status);
                    list.Add (transaction);
                }
                reader.Close ();
                DbConnection.Instance ().CloseConnection ();
                return list;
            } catch (SpringHeroTransactionException e) {
                Console.WriteLine ("Invalid transactions at that time");
                throw;
            }

        }      

        public List<Transaction> GetTransactionsByTime (string date,string date2,string accountNumber) {
            List<Transaction> list = new List<Transaction> ();
            DbConnection.Instance ().OpenConnection ();
            var queryString = "select * from `transaction` where (`createdAt` BETWEEN '"+date+" 00:00:00' and '"+date2+" 23:59:59' ) and status = 2 and (senderAccountNumber = @accountNumber OR receiverAccountNumber = @accountNumber)";
            var cmd = new MySqlCommand (queryString, DbConnection.Instance ().Connection);
            cmd.Parameters.AddWithValue ("@accountNumber", accountNumber);           
            var reader = cmd.ExecuteReader ();
            Transaction transaction = null;
            try {                           
                while (reader.Read ()) {
                    var id = reader.GetString ("id");
                    var createdAt = reader.GetString ("createdAt");
                    var updatedAt = reader.GetString ("updatedAt");
                    var type = reader.GetInt32 ("type");
                    var amount = reader.GetDecimal ("amount");
                    var content = reader.GetString ("content");
                    var senderAccountNumber = reader.GetString ("senderAccountNumber");
                    var receiverAccountNumber = reader.GetString ("receiverAccountNumber");
                    var status = reader.GetInt32 ("status");
                    transaction = new Transaction (id, createdAt, updatedAt, (Transaction.TransactionType) type, amount, content, senderAccountNumber, receiverAccountNumber, (Transaction.ActiveStatus) status);
                    list.Add (transaction);
                }
                reader.Close ();
                DbConnection.Instance ().CloseConnection ();
                return list;
            } catch (SpringHeroTransactionException e) {
                Console.WriteLine ("Invalid transactions at that time");
                throw;
            }
        }
    }
}